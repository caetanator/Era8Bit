/**
 * TimexCartridge.cs
 *
 * PURPOSE
 *  This is a library to read and write the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *  This class read and writes from the .DCK media file format that strores Timex Command Cartridges for the 
 *  Timex Sinclair/Computer 2068 (TS2068/TC2068) and Unipolbrit Komputer 2086 (UK2086).
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSof.Era8Bit.Library8Bit.MediaFormats" project:
 *      caetanator@hotmail.com
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2016-2017 José Caetano Silva
 *
 * HISTORY
 *  2016-12-01: Created.
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using CaetanoSof.Era8Bit.Library8Bit;
using CaetanoSof.Utils.EndianUtils;
using CaetanoSof.Utils.StreamUtils;

namespace CaetanoSof.Era8Bit.Library8Bit.MediaFormats
{
    /*
     * http://www.worldofspectrum.org/warajevo/Fformats.html#dck
     * http://trastero.speccy.org/cosas/JL/CartuchosTimex/ROM-EPROM.html
     * http://timex.comboios.info/tscart.html
     * http://www.crashonline.org.uk/19/timex.htm
     * 
     * 
     * 
     */

    /*       
        DCK FILE
        --------------------------------------------------------------------------------

        DCK files keeps information about memory content of various Timex memory expansions, 
        and information which chunks of extra memory are RAM chunks and which chunks are ROM chunks. 
        Such files have relatively simple format. At the beginning of a DCK file, a nine-byte 
        header is located. First byte is bank ID with following meaning: 

            0: DOCK bank (the most frequent variant);
            1-253: Reserved for expansions which allow more than three 64 KB banks 
                   (not implemented at this moment);
            254: EXROM bank (using this ID you may insert RAM or ROM chunks into Extended ROM bank, 
                 such hardware units exist on real Timex Sinclair);
            255: HOME bank (mainly useless, HOME content is typically stored in a Z80 file); 
                 however, using this bank ID you may replace content of Timex Home ROM, 
                 or turn Timex HOME ROM into RAM.

        This numbering of banks is in according to convention used in various routines from Timex ROM. 

        After the first byte, following eight bytes corresponds to eight 8 KB chunks in the bank. 
        Organization of each byte is as follows: 

        bit D0: 0 = read-only chunk, 1 = read/write chunk;
        bit D1: 0 = memory image for corresponding chunk is not present in DCK file, 1 = memory image is present in DCK file;
        bits D2-D7: reserved (all zeros).

        To be more clear, these bytes will have following values: 
        0, for non-existent chunks (reading from such chunks must return default values for given bank; for example, #FF in DOCK bank, and ghost images of 8 KB Timex EXROM in EXROM bank) 
        1, for RAM chunks, where initial RAM content is not given (in the emulator such chunks will be initially filled with zeros) 
        2, for ROM chunks 
        3, for RAM chunks where initial RAM content is given (this is need to allow saving content of expanded RAM; also this is useful for emulating non-volatile battery-protected RAM expansions) 
        After the header, a pure image of each presented chunk is stored in DCK file. Some examples will help understanding of such organization. 16 KB long LROS program needs header 0,2,2,0,0,0,0,0,0 in front of pure binary image of this program. 24 KB long AROS program needs header 255,0,0,0,0,2,2,2,0 in front of binary image of it to become a valid DCK file. 64 KB DOCK RAM disc cartridge (64K of empty RAM) may be described as only 9-byte long DCK file with content 0,1,1,1,1,1,1,1,1. 32 KB EXROM RAM disc cartridge mapped at address 32768 may be described also using 9-byte long DCK file with content 254,0,0,0,0,1,1,1,1. If you put a 9-byte header 255,2,2,0,0,0,0,0,0 in front of binary image of standard ZX Spectrum ROM, you will get DCK file which will replace Timex HOME ROM with ordinary Spectrum ROM (e.g. you will achieve Timex Sinclair 2048). At the last, if you put a header 255,3,3,0,0,0,0,0,0 in front of binary image of Timex HOME ROM, you will allow writing in the HOME ROM! 

        That's all if only one bank is stored in DCK file. Else, after the memory image, a new 9-byte header for next bank follows, and so on. 
        */

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 9)]
    public struct DckHeaderStruct
    {
        public byte MemoryBankID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] MemoryBankChunkType;
    }

    public enum TimexCartridgeBankID : int
    {
        /// <summary>
        /// Unknown Timex cartridge format.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 8 KB to 64K RAM and/or ROM Timex cartridge format, used for programs in Timex BASIC and/or Z80 machine code.
        /// <para>This format have to types: LROS (Language ROM Oriented software), AROS (Application ROM Oritened software).</para>
        /// <para>"Timex Chess" and "Timex Crazy Bugs" use this format.</para>
        /// </summary>
        DOCK = 0,

        /// <summary>
        /// Replaces the TS2068/TC2068 8 KB Extended ROM (starts at 4000h) Timex cartridge format by ROM or RAM.
        /// </summary>
        EXROM = 254,

        /// <summary>
        /// Replaces the TS2068/TC2068 16K Home ROM (starts at 0000h) Timex cartridge by ROM or RAM.
        /// <para>"Timex Spectrum 48 KB/TC2048 Emulator" and "Timex Time Word" use this format.</para>
        /// </summary>
        HOME = 255
    };

    /// <summary>
    /// Enumeration with the type of program in Timex cartridge DOCK: LROS or AROS.
    /// <para>Both type of programs have a short header at the beginning which contains the necessary informations (start address etc.) 
    /// for their execution.</para>
    /// </summary>
    public enum TimexCartridgeDockType : int
    {
        /// <summary>
        /// Unknown Timex cartridge DOCK type.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Timex cartridge DOCK type is of the LROS (Language ROM Oriented software) format.
        /// <para>LROS programs are mapped at address 0 (0000h) in the DOCK bank and they must 
        /// be written in machine code.</para>
        /// <para>They can replace the TS2068/TC2068 ROMs.</para>
        /// <para>LROS programs always have autorun and they will be started after initialization of the 
        /// Timex computer is finished.</para>
        /// <para>"Timex Chess" and "Zebra OS_64" use this format.</para>
        /// </summary>
        LROS = 1,

        /// <summary>
        /// Timex cartridge DOCK type is of the AROS (Application ROM Oritened software) format.
        /// <para>AROS programs are mapped at address 32768 (8000h) in the DOCK bank, and may be either in Z80 machine code or
        /// in Timex BASIC (The BASIC interpreter allows the running of BASIC programs from the DOCK bank).</para>
        /// <para>The TS2068/TC2068 ROMs can't be replaced.</para>
        /// <para>AROS programs may be or may not to be autorun programs.</para>
        /// <para>"Timex Crazy Bugs" use this format.</para>
        /// </summary>
        AROS = 2
    };

    public enum TimexCartridgeDockArosLanguage : int
    {
        /// <summary>
        /// Unknown Timex cartridge DOCK format, type AROS, program language.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// The Timex cartridge DOCK format, type AROS, program language used is Timex BASIC with optional machine code.
        /// </summary>
        BASIC = 1,

        /// <summary>
        /// The Timex cartridge DOCK format, type AROS, program language used is machine code only.
        /// </summary>
        MACHINE_CODE = 2
    };

    public enum TimexCartridge8KChunkType : int
    {
        Unknown = -1,
        NON_EXISTENT = 0,
        RAM_NOT_ON_FILE = 1,
        ROM_ON_FILE = 2,
        RAM_ON_FILE = 3,
    };

    public class TimexCartridge8KChunk
    {
        public TimexCartridge8KChunkType Type;
        public byte[] MemoryChunk;

        public TimexCartridge8KChunk()
        {
            this.Type = TimexCartridge8KChunkType.Unknown;
            this.MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
            this.MemoryChunk = Encoding.ASCII.GetBytes(new string('\xFF', GlobalMemorySizeConstants.KB8));
        }
    }

    public class TimexCartridgeBank
    {
        #region Class Constants
        private const uint CHUNK_IS_RAM_FLAG = 0x01;
        private const uint CHUNK_MEM_PRESENT_FLAG = 0x02;
        private const uint CHUNK_RESERVED_MASK = 0xFC;
        #endregion // Class Constants

        #region Class Properties
        public bool DataChanged { get; private set; } = false;

        public int MemoryBankID { get; set; } = (int)TimexCartridgeBankID.Unknown;

        public TimexCartridgeDockType CartridgeDockType { get; private set; } = TimexCartridgeDockType.Unknown;

        public TimexCartridgeDockArosLanguage CartridgeDockArosLanguage { get; private set; } = TimexCartridgeDockArosLanguage.Unknown;

        public int NumberOf8KChunksInFile { get; private set; } = 0;

        public TimexCartridge8KChunk[] MemoryBanksChunks { get; private set; } = new TimexCartridge8KChunk[8];
        #endregion // Class Properties

        #region Class Constructors
        public TimexCartridgeBank()
        {
            // Do nothing
        }

        public TimexCartridgeBank(Stream streamIn)
        {
            this.Read(streamIn);
        }
        #endregion // Class Constructors

        #region Class Methods
        public bool IsHomeBank()
        {
            return (this.MemoryBankID == (int)TimexCartridgeBankID.HOME);
        }
        public bool IsExRomBank()
        {
            return (this.MemoryBankID == (int)TimexCartridgeBankID.EXROM);
        }
        public bool IsDockBank()
        {
            return (this.MemoryBankID == (int)TimexCartridgeBankID.DOCK);
        }

        public bool IsDockAROS()
        {
            return (CartridgeDockType == TimexCartridgeDockType.AROS);
        }
        public bool IsDockLROS()
        {
            return (CartridgeDockType == TimexCartridgeDockType.AROS);
        }

        public bool IsArosLangBASIC()
        {
            return (CartridgeDockArosLanguage == TimexCartridgeDockArosLanguage.BASIC);
        }
        public bool IsArosLangMachineCode()
        {
            return (CartridgeDockArosLanguage == TimexCartridgeDockArosLanguage.MACHINE_CODE);
        }


        public List<String[]> getInfo()
        {
            List<String[]> listRet = new List<String[]>();

            try
            {
                String strAux;

                // Cartridge Bank Info
                switch (this.MemoryBankID)
                {
                    case (int)TimexCartridgeBankID.DOCK:
                        strAux = "DOCK Memory Bank";
                        break;

                    case (int)TimexCartridgeBankID.EXROM:
                        strAux = "EXROM Memory Bank";
                        break;

                    case (int)TimexCartridgeBankID.HOME:
                        strAux = "HOME Memory Bank";
                        break;

                    default:
                        strAux = "Reserved Memory Bank ID " + this.MemoryBankID;
                        break;
                }
                listRet.Add(new String[2] { "Cartridge Memory Bank ID", strAux });

                // 8 KB memory chunks in the bank
                listRet.Add(new String[2] { "Cartridge 8 KB Memory Bank Chunk Type", null });
                int memoryBaseStart;
                int memoryBaseEnd;
                for (int i = 0; i < 8; i++)
                {
                    memoryBaseStart = i * GlobalMemorySizeConstants.KB8;
                    memoryBaseEnd = ((i + 1) * GlobalMemorySizeConstants.KB8) - 1;
                    switch (this.MemoryBanksChunks[i].Type)
                    {
                        case TimexCartridge8KChunkType.NON_EXISTENT:
                            strAux = "System RAM/ROM";
                            break;

                        case TimexCartridge8KChunkType.RAM_NOT_ON_FILE:
                            strAux = "RAM (not on file)";
                            break;

                        case TimexCartridge8KChunkType.ROM_ON_FILE:
                            strAux = "ROM";
                            break;

                        case TimexCartridge8KChunkType.RAM_ON_FILE:
                            strAux = "RAM (on file)";
                            break;

                        default:
                            strAux = "Unknown memory chunk type";
                            break;
                    }
                    strAux = String.Format("\t{0} - [{1,5:####0} - {2,5:####0}] / [{1:X4}h - {2:X4}h]: {3}", i, memoryBaseStart, memoryBaseEnd, strAux).ToString();
                    listRet.Add(new String[2] { null, strAux });
                }

                // Cartridge subtype
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listRet;
        }
        #endregion // Class Methods

        #region Interface IMediaFormat
        public void Read(Stream streamIn)
        {
            //// Read DCK File Header
            DckHeaderStruct structDCK = (DckHeaderStruct)StreamUtils.ReadStructure<DckHeaderStruct>(streamIn);

            // Cartridge type
            //int nextByte = (byte)streamIn.ReadByte();
            //this.MemoryBankID = (TimexCartridgeBankID)nextByte;
            int nextByte = (int)structDCK.MemoryBankID;
            if ((nextByte == (int)TimexCartridgeBankID.DOCK) || (nextByte == (int)TimexCartridgeBankID.EXROM) || (nextByte == (int)TimexCartridgeBankID.HOME))
            {
                this.MemoryBankID = nextByte;
            }
            else
            {
                // FIXME: Bug? Reserved ID, not supported
                this.MemoryBankID = nextByte;
            }

            // The eight 8 KB memory chunks information
            int memoryBaseStart = -1;
            int memoryBaseEnd = -1;
            int fileSize = 0;
            bool bIsRAM;
            bool bIsOnFile;
            for (int i = 0; i < 8; i++)
            {
                memoryBaseStart = i * GlobalMemorySizeConstants.KB8;
                memoryBaseEnd = ((i + 1) * GlobalMemorySizeConstants.KB8) - 1;
                this.MemoryBanksChunks[i] = new TimexCartridge8KChunk();

                //nextByte = (byte)streamIn.ReadByte();
                nextByte = (int)structDCK.MemoryBankChunkType[i];
                bIsRAM = ((nextByte & CHUNK_IS_RAM_FLAG) == CHUNK_IS_RAM_FLAG);
                bIsOnFile = ((nextByte & CHUNK_MEM_PRESENT_FLAG) == CHUNK_MEM_PRESENT_FLAG);
                if ((nextByte & CHUNK_RESERVED_MASK) == 0)
                {
                    switch ((TimexCartridge8KChunkType)nextByte)
                    {
                        case TimexCartridge8KChunkType.NON_EXISTENT:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.NON_EXISTENT;
                            switch (this.MemoryBankID)
                            {
                                case (int)TimexCartridgeBankID.DOCK:
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\uFFFF', GlobalMemorySizeConstants.KB8));
                                    break;

                                case (int)TimexCartridgeBankID.EXROM:
                                    // FIXME: Should copy Extension ROM in 8Kb chunks
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\uFFFF', GlobalMemorySizeConstants.KB8));
                                    break;
                                case (int)TimexCartridgeBankID.HOME:
                                    // FIXME: Is this OK?
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                                    break;
                            }
                            break;

                        case TimexCartridge8KChunkType.RAM_NOT_ON_FILE:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.RAM_NOT_ON_FILE;
                            this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                            break;

                        case TimexCartridge8KChunkType.ROM_ON_FILE:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.ROM_ON_FILE;
                            if ((i == 0) && (this.MemoryBankID == (int)TimexCartridgeBankID.DOCK))
                            {
                                //cartrigeIsLROS = true;
                                //cartrigePosLROS = fileSize;
                            }
                            else if ((i == 4) && (this.MemoryBankID == (int)TimexCartridgeBankID.DOCK))
                            {
                                //cartrigeIsAROS = true;
                                //cartrigePosAROS = fileSize;
                            }
                            ++NumberOf8KChunksInFile;
                            break;

                        case TimexCartridge8KChunkType.RAM_ON_FILE:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.RAM_ON_FILE;
                            ++NumberOf8KChunksInFile;
                            break;

                        default:
                            // FIXME: Bug?
                            break;
                    }
                }
                else
                {
                    // TODO: Bug?
                    this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.Unknown;
                    if (bIsOnFile)
                    {
                        ++fileSize;
                    }
                }
                if (bIsOnFile)
                {
                    StreamUtils.ReadBytes(streamIn, ref this.MemoryBanksChunks[i].MemoryChunk);
                }

                /*
                listRet.Add(new String[2] { "Cartridge Sub-Type", strAux });
                if (this.IsTypeLROS)
                {
                    strAux = "LROS";
                    // Not Used
                    cartridgeFile.Position = 9 + (cartrigePosLROS * GlobalConstants.KB8);
                    nextByte = (byte)cartridgeFile.ReadByte();
                    listRet += "\tNot Used: 0x" + nextByte.ToString("X2") + Environment.NewLine;

                    // Type
                    listRet += "\tCartridge Type: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch ((TimexCartridgeDockType)nextByte)
                    {
                        case TimexCartridgeDockType.LROS:
                            listRet += "LROS";
                            break;

                        default:
                            listRet += "Type Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    listRet += Environment.NewLine;

                    // Starting Address
                    uint memAdressLSB = (uint)cartridgeFile.ReadByte();
                    uint memAdressMSB = (uint)cartridgeFile.ReadByte();
                    if (GlobalConstants.IsLittleEndian)
                    {
                        memAdressMSB = memAdressMSB << 8;
                        memAdressMSB += memAdressLSB;
                    }
                    else
                    {
                        //memAdressMSB = memAdressMSB;
                        memAdressMSB += memAdressLSB << 8;
                    }

                    listRet += "\tStarting Address: " + memAdressMSB + " (0x" + memAdressMSB.ToString("X4") + ")" + Environment.NewLine;

                    // Memory Chunk Specification
                    uint memChunks = (uint)cartridgeFile.ReadByte();
                    listRet += "\tMemory Chunk Specification: " + Environment.NewLine;
                    // When writing to the Horizontal Select Register (Port F4H), the Chunk Specification is High Active.
                    for (int i = 0; i < 8; i++)
                    {
                        listRet += "\t\tChunk " + i + ": " + (memChunks & 1) + Environment.NewLine;
                        memChunks = memChunks >> 1;
                    }
                }
                else if (cartrigeIsAROS)
                {
                    listRet += "Cartridge Sub-Type: AROS" + Environment.NewLine;
                    // Language
                    cartridgeFile.Position = 9 + (cartrigePosAROS * GlobalConstants.KB8);
                    listRet += "\tLanguage Type: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch ((TimexCartridgeDockArosLanguage)nextByte)
                    {
                        case TimexCartridgeDockArosLanguage.BASIC:
                            listRet += "BASIC [and Machine Code]";
                            break;

                        case TimexCartridgeDockArosLanguage.MACHINE_CODE:
                            listRet += "Machine Code";
                            break;

                        default:
                            listRet += "Language Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    listRet += Environment.NewLine;

                    // Type
                    listRet += "\tCartridge Type: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch ((TimexCartridgeDockType)nextByte)
                    {
                        case TimexCartridgeDockType.AROS:
                            listRet += "AROS";
                            break;

                        default:
                            listRet += "Type Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    listRet += Environment.NewLine;

                    // Starting Address
                    uint memAdressLSB = (uint)cartridgeFile.ReadByte();
                    uint memAdressMSB = (uint)cartridgeFile.ReadByte();
                    if (GlobalConstants.IsLittleEndian)
                    {
                        memAdressMSB = memAdressMSB << 8;
                        memAdressMSB += memAdressLSB;
                    }
                    else
                    {
                        //memAdressMSB = memAdressMSB;
                        memAdressMSB += memAdressLSB << 8;
                    }

                    listRet += "\tStarting Address: " + memAdressMSB + " (0x" + memAdressMSB.ToString("X4") + ")" + Environment.NewLine;

                    // Memory Chunk Specification
                    uint memChunks = (uint)cartridgeFile.ReadByte();
                    listRet += "\tMemory Chunk Specification: " + Environment.NewLine;
                    // NOTE: Bits 0-3 must he set to 1 for proper execution.
                    for (int i = 0; i < 8; i++)
                    {
                        listRet += "\t\tChunk " + i + ": " + (memChunks & 1) + Environment.NewLine;
                        memChunks = memChunks >> 1;
                    }

                    // Autostart Specification
                    listRet += "\tAutostart Specification: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch (nextByte)
                    {
                        case 0:
                            listRet += "No Autostart";
                            break;

                        case 1:
                            listRet += "Autostart";
                            break;

                        default:
                            listRet += "Autostart Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    listRet += Environment.NewLine;

                    // Number of bytes of RAM to be Reserved for Machine Code Variables
                    memAdressLSB = (uint)cartridgeFile.ReadByte();
                    memAdressMSB = (uint)cartridgeFile.ReadByte();
                    if (GlobalConstants.IsLittleEndian)
                    {
                        memAdressMSB = memAdressMSB << 8;
                        memAdressMSB += memAdressLSB;
                    }
                    else
                    {
                        //memAdressMSB = memAdressMSB;
                        memAdressMSB += memAdressLSB << 8;
                    }

                    listRet += "\tReserved RAM: " + memAdressMSB + " Bytes" + Environment.NewLine;
                }
                */
            }
        }

        public void Write(Stream streamOut)
        {
            throw new NotImplementedException();
        }
        #endregion // Interface IMediaFormat
    }

    /// <summary>
    /// DCK file format reader and writer.
    /// <para>This file format is used to store images of Timex Command Cartridges (TCC), used by TS2068 and TC2068 computers.</para>
    /// <para>TCC actually add up to 64K of additional ROM and/or RAM memory to the computer.</para>
    /// <para>Timex made the TS1510 cartridge player to add the command cartridges to the TS1000 and TS1500 computer. These cartridges 
    /// are incompatible with the TS2068 cartridges.</para>
    /// <para>ZX Interface 2 (IF2) cartridges are also incompatible.</para>
    /// </summary>
    public class TimexCartridge : IMediaFormat
    {
        #region Class Constants
        private const uint CHUNK_IS_RAM_FLAG = 0x01;
        private const uint CHUNK_MEM_PRESENT_FLAG = 0x02;
        private const uint CHUNK_RESERVED_MASK = 0xFC;
        #endregion // Class Constants

        #region Interface IMediaFormat
        public MediaFormatType Type { get; private set; } = MediaFormatType.CARTRIDGE;
        public String[] Extensions { get; private set; } = { "dck" };
        public string Description
        {
            get
            {
                return "Timex Command Cartridge (TCC)";
            }

            private set { }
        }
        public String FileName { get; private set; } = "";
        public long FileSize { get; private set; } = 0;

        public bool DataChanged { get; private set; } = false;
        #endregion // Interface IMediaFormat

        #region Class Properties
        public TimexCartridgeBankID MemoryBankID { get; private set; } = TimexCartridgeBankID.Unknown;

        public TimexCartridgeDockType CartridgeDockType { get; private set; } = TimexCartridgeDockType.Unknown;

        public TimexCartridgeDockArosLanguage CartridgeDockArosType { get; private set; } = TimexCartridgeDockArosLanguage.Unknown;

        public int NumberOf8KChunks { get; private set; } = 0;

        public TimexCartridge8KChunk[] MemoryBanksChunks { get; private set; } = new TimexCartridge8KChunk[8];

        public bool IsTypeAROS { get; private set; } = false;
        public bool IsTypeLROS { get; private set; } = false;
        #endregion // Class Properties

        #region Class Constructors
        public TimexCartridge()
        {
            // Do nothing
        }

        public TimexCartridge(String fileName)
        {
            this.Load(fileName);
        }
        #endregion // Class Constructors

        #region Class Methods
        public static String getInfo(String fileName)
        {
            String strRet = "";

            try
            {
                TimexCartridgeBankID cartridgeType = TimexCartridgeBankID.Unknown;
                bool cartrigeIsAROS = false;
                int cartrigePosAROS = -1;
                bool cartrigeIsLROS = false;
                int cartrigePosLROS = -1;

                FileStream cartridgeFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                // File name
                strRet = "File Name: <" + fileName + ">" + Environment.NewLine;

                // Cartridge type
                strRet += "Cartridge Type: ";
                byte nextByte = (byte)cartridgeFile.ReadByte();
                cartridgeType = (TimexCartridgeBankID)nextByte;
                switch (cartridgeType)
                {
                    case TimexCartridgeBankID.DOCK:
                        strRet += "DOCK bank";
                        break;

                    case TimexCartridgeBankID.EXROM:
                        strRet += "EXROM bank";
                        break;

                    case TimexCartridgeBankID.HOME:
                        strRet += "HOME bank";
                        break;

                    default:
                        strRet += "Unknown bank format (0x" + nextByte.ToString("X2") + ")";
                        break;
                }
                strRet += Environment.NewLine;

                // 8 KB memory chunks in the bank
                strRet += "Memory Type:" + Environment.NewLine;
                int memoryBaseStart = 0;
                int memoryBaseEnd = GlobalMemorySizeConstants.KB8 - 1;
                int fileSize = 0;
                for (int i = 0; i < 8; i++)
                {
                    memoryBaseStart = i * GlobalMemorySizeConstants.KB8;
                    memoryBaseEnd = ((i + 1) * GlobalMemorySizeConstants.KB8) - 1;
                    strRet += "\tMemory From " + memoryBaseStart + " (0x" + ((ushort)memoryBaseStart).ToString("X4") + ") to " + memoryBaseEnd + " (0x" + ((ushort)memoryBaseEnd).ToString("X4") + ") is ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch (nextByte)
                    {
                        case 0:
                            strRet += "System RAM/ROM";
                            break;

                        case 1:
                            strRet += "RAM (not on file)";
                            break;

                        case 2:
                            strRet += "ROM";
                            if((i == 0) && (cartridgeType == TimexCartridgeBankID.DOCK))
                            {
                                cartrigeIsLROS = true;
                                cartrigePosLROS = fileSize;
                            }
                            else if ((i == 4) && (cartridgeType == TimexCartridgeBankID.DOCK))
                            {
                                cartrigeIsAROS = true;
                                cartrigePosAROS = fileSize;
                            }
                            ++fileSize;
                            break;

                        case 3:
                            strRet += "RAM (on file)";
                            ++fileSize;
                            break;

                        default:
                            strRet += "Unknown chunk format (0x" + nextByte.ToString("X2") + ")";
                            ++fileSize;
                            break;
                    }
                    strRet += Environment.NewLine;
                }

                // Cartridge subtype
                if (cartrigeIsLROS)
                {
                    strRet += "Cartridge Sub-Type: LROS" + Environment.NewLine;
                    // Not Used
                    cartridgeFile.Position = 9 + (cartrigePosLROS * GlobalMemorySizeConstants.KB8);
                    nextByte = (byte)cartridgeFile.ReadByte();
                    strRet += "\tNot Used: 0x" + nextByte.ToString("X2") + Environment.NewLine;

                    // Type
                    strRet += "\tCartridge Type: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch ((TimexCartridgeDockType)nextByte)
                    {
                        case TimexCartridgeDockType.LROS:
                            strRet += "LROS";
                            break;

                        default:
                            strRet += "Type Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    strRet += Environment.NewLine;

                    // Starting Address
                    uint memAdressLSB = (uint)cartridgeFile.ReadByte();
                    uint memAdressMSB = (uint)cartridgeFile.ReadByte();
                    if(EndianUtils.IsLittleEndian)
                    {
                        memAdressMSB = memAdressMSB << 8;
                        memAdressMSB += memAdressLSB;
                    }
                    else
                    {
                        //memAdressMSB = memAdressMSB;
                        memAdressMSB += memAdressLSB << 8;
                    }

                    strRet += "\tStarting Address: " + memAdressMSB + " (0x" + memAdressMSB.ToString("X4") + ")" + Environment.NewLine;

                    // Memory Chunk Specification
                    uint memChunks = (uint)cartridgeFile.ReadByte();
                    strRet += "\tMemory Chunk Specification: " + Environment.NewLine;
                    // When writing to the Horizontal Select Register (Port F4H), the Chunk Specification is High Active.
                    for (int i = 0; i < 8; i++)
                    {
                        strRet += "\t\tChunk " + i + ": " + (memChunks & 1) + Environment.NewLine;
                        memChunks = memChunks >> 1;
                    }
                }
                else if (cartrigeIsAROS)
                {
                    strRet += "Cartridge Sub-Type: AROS" + Environment.NewLine;
                    // Language
                    cartridgeFile.Position = 9 + (cartrigePosAROS * GlobalMemorySizeConstants.KB8);
                    strRet += "\tLanguage Type: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch ((TimexCartridgeDockArosLanguage)nextByte)
                    {
                        case TimexCartridgeDockArosLanguage.BASIC:
                            strRet += "BASIC [and Machine Code]";
                            break;

                        case TimexCartridgeDockArosLanguage.MACHINE_CODE:
                            strRet += "Machine Code";
                            break;

                        default:
                            strRet += "Language Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    strRet += Environment.NewLine;

                    // Type
                    strRet += "\tCartridge Type: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch ((TimexCartridgeDockType)nextByte)
                    {
                        case TimexCartridgeDockType.AROS:
                            strRet += "AROS";
                            break;

                        default:
                            strRet += "Type Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    strRet += Environment.NewLine;

                    // Starting Address
                    uint memAdressLSB = (uint)cartridgeFile.ReadByte();
                    uint memAdressMSB = (uint)cartridgeFile.ReadByte();
                    if(EndianUtils.IsLittleEndian)
                    {
                        memAdressMSB = memAdressMSB << 8;
                        memAdressMSB += memAdressLSB;
                    }
                    else
                    {
                        //memAdressMSB = memAdressMSB;
                        memAdressMSB += memAdressLSB << 8;
                    }

                    strRet += "\tStarting Address: " + memAdressMSB + " (0x" + memAdressMSB.ToString("X4") + ")" + Environment.NewLine;

                    // Memory Chunk Specification
                    uint memChunks = (uint)cartridgeFile.ReadByte();
                    strRet += "\tMemory Chunk Specification: " + Environment.NewLine;
                    // NOTE: Bits 0-3 must he set to 1 for proper execution.
                    for (int i = 0; i < 8; i++)
                    {
                        strRet += "\t\tChunk " + i + ": " + (memChunks & 1) + Environment.NewLine;
                        memChunks = memChunks >> 1;
                    }

                    // Autostart Specification
                    strRet += "\tAutostart Specification: ";
                    nextByte = (byte)cartridgeFile.ReadByte();
                    switch (nextByte)
                    {
                        case 0:
                            strRet += "No Autostart";
                            break;

                        case 1:
                            strRet += "Autostart";
                            break;

                        default:
                            strRet += "Autostart Error (0x" + nextByte.ToString("X2") + ")";
                            break;
                    }
                    strRet += Environment.NewLine;

                    // Number of bytes of RAM to be Reserved for Machine Code Variables
                    memAdressLSB = (uint)cartridgeFile.ReadByte();
                    memAdressMSB = (uint)cartridgeFile.ReadByte();
                    if(EndianUtils.IsLittleEndian)
                    {
                        memAdressMSB = memAdressMSB << 8;
                        memAdressMSB += memAdressLSB;
                    }
                    else
                    {
                        //memAdressMSB = memAdressMSB;
                        memAdressMSB += memAdressLSB << 8;
                    }

                    strRet += "\tReserved RAM: " + memAdressMSB + " Bytes" + Environment.NewLine;
                }

                // File size
                strRet += "File Size (Real): " + cartridgeFile.Length + " Bytes" + Environment.NewLine;
                strRet += "File Size (Calculated): " + (9 + (GlobalMemorySizeConstants.KB8 * fileSize)) + " Bytes" + Environment.NewLine;
                strRet += Environment.NewLine;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
            }

            return strRet;
        }
        #endregion // Class Methods

        #region Interface IMediaFormat
        public void Read(Stream streamIn)
        {
            //// Read DCK File Header
            DckHeaderStruct structDCK = (DckHeaderStruct)StreamUtils.ReadStructure<DckHeaderStruct>(streamIn);

            // Cartridge type
            //int nextByte = (byte)streamIn.ReadByte();
            //this.MemoryBankID = (TimexCartridgeBankID)nextByte;
            int nextByte = (int)structDCK.MemoryBankID;
            if ((nextByte == (int)TimexCartridgeBankID.DOCK) || (nextByte == (int)TimexCartridgeBankID.EXROM) || (nextByte == (int)TimexCartridgeBankID.HOME))
            {
                this.MemoryBankID = (TimexCartridgeBankID)structDCK.MemoryBankID;
            }
            else
            {
                // TODO: Bug?
                this.MemoryBankID = TimexCartridgeBankID.Unknown;
            }

            // The eight 8 KB memory chunks information
            int memoryBaseStart = -1;
            int memoryBaseEnd = -1;
            int fileSize = 0;
            bool bIsRAM;
            bool bIsOnFile;
            for (int i = 0; i < 8; i++)
            {
                memoryBaseStart = i * GlobalMemorySizeConstants.KB8;
                memoryBaseEnd = ((i + 1) * GlobalMemorySizeConstants.KB8) - 1;
                this.MemoryBanksChunks[i] = new TimexCartridge8KChunk();

                //nextByte = (byte)streamIn.ReadByte();
                nextByte = (int)structDCK.MemoryBankChunkType[i];
                bIsRAM = ((nextByte & CHUNK_IS_RAM_FLAG) == CHUNK_IS_RAM_FLAG);
                bIsOnFile = ((nextByte & CHUNK_MEM_PRESENT_FLAG) == CHUNK_MEM_PRESENT_FLAG);
                if ((nextByte & CHUNK_RESERVED_MASK) == 0)
                {
                    switch ((TimexCartridge8KChunkType)nextByte)
                    {
                        case TimexCartridge8KChunkType.NON_EXISTENT:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.NON_EXISTENT;
                            switch (this.MemoryBankID)
                            {
                                case TimexCartridgeBankID.DOCK:
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\uFFFF', GlobalMemorySizeConstants.KB8));
                                    break;

                                case TimexCartridgeBankID.EXROM:
                                    // FIXME: Should copy Extension ROM in 8Kb chunks
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\uFFFF', GlobalMemorySizeConstants.KB8));
                                    break;
                                case TimexCartridgeBankID.HOME:
                                    // FIXME: Is this OK?
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                                    break;
                            }
                            break;

                        case TimexCartridge8KChunkType.RAM_NOT_ON_FILE:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.RAM_NOT_ON_FILE;
                            this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                            break;

                        case TimexCartridge8KChunkType.ROM_ON_FILE:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.ROM_ON_FILE;
                            if ((i == 0) && (this.MemoryBankID == TimexCartridgeBankID.DOCK))
                            {
                                //cartrigeIsLROS = true;
                                //cartrigePosLROS = fileSize;
                            }
                            else if ((i == 4) && (this.MemoryBankID == TimexCartridgeBankID.DOCK))
                            {
                                //cartrigeIsAROS = true;
                                //cartrigePosAROS = fileSize;
                            }
                            ++NumberOf8KChunks;
                            break;

                        case TimexCartridge8KChunkType.RAM_ON_FILE:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.RAM_ON_FILE;
                            ++NumberOf8KChunks;
                            break;

                        default:
                            // TODO: Bug?
                            break;
                    }
                }
                else
                {
                    // TODO: Bug?
                    this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.Unknown;
                    if (bIsOnFile)
                    {
                        ++fileSize;
                    }
                }
                if (bIsOnFile)
                {
                    StreamUtils.ReadBytes(streamIn, ref this.MemoryBanksChunks[i].MemoryChunk);
                }
            }
        }

        public void Write(Stream streamOut)
        {
            throw new NotImplementedException();
        }

        public void Load(String fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }

            try
            {
                FileStream cartridgeFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                long fileSize = cartridgeFile.Length;
                if (fileSize >= 9)
                {
                    this.Read(cartridgeFile);
                    this.FileName = fileName;
                    this.FileSize = fileSize;
                }
                else
                {
                    throw new Exception("Bad file size");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Save(String fileName, uint fileVersion = 0)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }

            try
            {
                FileStream cartridgeFile = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                this.Write(cartridgeFile);
                long fileSize = cartridgeFile.Length;
                if (fileSize >= 9)
                {
                    this.FileName = fileName;
                    this.FileSize = fileSize;
                    this.DataChanged = false;
                }
                else
                {
                    throw new Exception("Bad file size");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion // Interface IMediaFormat
    }
}

