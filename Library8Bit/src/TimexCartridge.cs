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
 *      José Caetano Silva, jcaetano@users.sourceforge.net
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

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 5)]
    public struct DckLrosStruct
    {
        /// <summary>
        /// Not used, must be 0
        /// </summary>
        public byte NotUsed;

        /// <summary>
        /// Dock memory bank type, must be TimexCartridgeDockType.LROS
        /// </summary>
        public byte DockType;

        /// <summary>
        /// The adress of the machine code intruction to start/jump to, stored in LSB MSB order
        /// </summary>
        public ushort ProgStartingAddress;

        /// <summary>
        /// The memory chunk specification. When writing to the Horizontal Select Register (Port F4H), the Chunk Specification is High Active
        /// </summary>
        public byte MemoryChunkSpecification;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 8)]
    public struct DckArosStruct
    {
        /// <summary>
        /// Dock AROS language type, must be TimexCartridgeDockArosLanguage.BASIC or TimexCartridgeDockArosLanguage.MACHINE_CODE
        /// </summary>
        public byte LanguageType;

        /// <summary>
        /// Dock memory bank type, must be TimexCartridgeDockType.LROS
        /// </summary>
        public byte DockType;

        /// <summary>
        /// The BASIC line or machine code adress of the intruction to start/jump to, stored in LSB MSB order
        /// </summary>
        public ushort ProgStartingAddress;

        /// <summary>
        /// The memory chunk specification. When writing to the Horizontal Select Register (Port F4H), the Chunk Specification is High Active
        /// </summary>
        public byte MemoryChunkSpecification;

        /// <summary>
        /// The program autostart specification: 0=> No; 1 => Yes
        /// </summary>
        public byte AutostartSpecification;


        /// <summary>
        /// Number of bytes of RAM to be reserved for machine code variables, stored in LSB MSB order
        /// </summary>
        public ushort ReservedRAM;
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
        public TimexCartridge8KChunkType Type { get; set; } = TimexCartridge8KChunkType.Unknown;
        public bool IsRAM { get; set; } = false;
        public bool IsOnFile { get; set; } = false;
        public byte[] MemoryChunk;

        public TimexCartridge8KChunk()
        {
            // Do nothing
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

        public DckLrosStruct sLrosProgramHeader;
        public DckArosStruct sArosProgramHeader;
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
            return IsDockBank() && (CartridgeDockType == TimexCartridgeDockType.AROS);
        }
        public bool IsDockLROS()
        {
            return IsDockBank() && (CartridgeDockType == TimexCartridgeDockType.AROS);
        }

        public bool IsArosLangBASIC()
        {
            return IsDockAROS() && (CartridgeDockArosLanguage == TimexCartridgeDockArosLanguage.BASIC);
        }
        public bool IsArosLangMachineCode()
        {
            return IsDockAROS() && (CartridgeDockArosLanguage == TimexCartridgeDockArosLanguage.MACHINE_CODE);
        }

        public DckLrosStruct GetLrosProgramHeader()
        {
            return this.sLrosProgramHeader;
        }

        public DckArosStruct GeALrosProgramHeader()
        {
            return this.sArosProgramHeader;
        }

        public List<String[]> GetInfo()
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
                            if(this.MemoryBanksChunks[i].IsRAM)
                            {
                                strAux += " RAM";
                            }
                            else
                            {
                                strAux += " ROM";
                            }
                            if (this.MemoryBanksChunks[i].IsOnFile)
                            {
                                strAux += " (on file)";
                            }
                            else
                            {
                                strAux += " (not on file)";
                            }
                            break;
                    }
                    strAux = String.Format("\t{0} - [{1,5:####0} - {2,5:####0}] / [{1:X4}h - {2:X4}h]: {3}", i, memoryBaseStart, memoryBaseEnd, strAux).ToString();
                    listRet.Add(new String[2] { null, strAux });
                }

                // Cartridge DOCK bank information
                if (this.MemoryBankID == (int)TimexCartridgeBankID.DOCK)
                {
                    if (this.CartridgeDockType == TimexCartridgeDockType.LROS)
                    {
                        // This is a LROS machine code program
                        listRet.Add(new String[2] { "DOCK Memory Bank Type", "LROS" });

                        listRet.Add(new String[2] { "\tReserved (=0)", this.sLrosProgramHeader.NotUsed.ToString() });

                        if(this.sLrosProgramHeader.DockType == (int)TimexCartridgeDockType.LROS)
                        {
                            strAux = "Machine Code";
                        }
                        else
                        {
                            strAux = String.Format("Unknown Language {0}", this.sLrosProgramHeader.DockType).ToString();
                        }
                        listRet.Add(new String[2] { "\tPrograming Language", strAux });

                        strAux = String.Format("{0,5:####0} / {0:X4}h", this.sLrosProgramHeader.ProgStartingAddress).ToString();
                        listRet.Add(new String[2] { "\tMachine Code Start Adress", strAux });

                        // Memory Chunk Specification
                        uint memChunks = (uint)this.sLrosProgramHeader.MemoryChunkSpecification;
                        listRet.Add(new String[2] { "\tMemory Chunk Specification", "" });

                        // NOTE: Bits 0-3 must he set to 1 for proper execution.
                        for (int i = 0; i < 8; i++)
                        {
                            strAux = String.Format("\t\tChunk {0}: {1}", i, ((((memChunks & 1) == 0) ? "Present" : "Not Present"))).ToString();
                            listRet.Add(new String[2] { null, strAux });
                            memChunks = memChunks >> 1;
                        }
                    }
                    else if (this.CartridgeDockType == TimexCartridgeDockType.AROS)
                    {
                        // This is an AROS BASIC program with an optional machine code program
                        listRet.Add(new String[2] { "DOCK Memory Bank Type", "AROS" });

                        if (this.sArosProgramHeader.DockType == (int)TimexCartridgeDockType.AROS)
                        {
                            switch (this.sArosProgramHeader.LanguageType)
                            {
                                case (int)TimexCartridgeDockArosLanguage.BASIC:
                                    strAux = "BASIC";
                                    if (this.sArosProgramHeader.ReservedRAM > 0)
                                    {
                                        strAux = " + Machine Code";
                                    }
                                    break;

                                case (int)TimexCartridgeDockArosLanguage.MACHINE_CODE:
                                    strAux = "Machine Code";
                                    break;

                                default:
                                    strAux = String.Format("Unknown Language {0}", this.sArosProgramHeader.LanguageType).ToString();
                                    break;
                            }
                        }
                        else
                        {
                            strAux = String.Format("Unknown Type {0}", this.sArosProgramHeader.DockType).ToString();
                        }
                        listRet.Add(new String[2] { "\tPrograming Language", strAux });

                        if (this.CartridgeDockArosLanguage == TimexCartridgeDockArosLanguage.BASIC)
                        {
                            // AROS BASIC
                            strAux = String.Format("{0,5:####0}", this.sArosProgramHeader.ProgStartingAddress).ToString();
                            listRet.Add(new String[2] { "\tBASIC Code Start Line", strAux });
                        }
                        else
                        {
                            // AROS Machine Code
                            strAux = String.Format("{0,5:####0} / {0:X4}h", this.sArosProgramHeader.ProgStartingAddress).ToString();
                            listRet.Add(new String[2] { "\tMachine Code Start Adress", strAux });
                        }

                        listRet.Add(new String[2] { "\tReserved RAM for Machine Code", this.sArosProgramHeader.ReservedRAM.ToString() + " Bytes"});

                        switch (this.sArosProgramHeader.AutostartSpecification)
                        {
                            case 0:
                                strAux = "False";
                                break;

                            case 1:
                                strAux = "True";
                                break;

                            default:
                                strAux = String.Format("Unknown Autostart Specification {0}", this.sArosProgramHeader.AutostartSpecification).ToString();
                                break;
                        }
                        listRet.Add(new String[2] { "\tProgram Autostarts", strAux });

                        // Memory Chunk Specification
                        uint memChunks = (uint)this.sArosProgramHeader.MemoryChunkSpecification;
                        listRet.Add(new String[2] { "\tMemory Chunk Specification", "" });

                        // NOTE: Bits 4 must he set to 1 for proper execution.
                        for (int i = 0; i < 8; i++)
                        {
                            strAux = String.Format("\t\tChunk {0}: {1}", i, ((((memChunks & 1) == 0) ? "Present" : "Not Present"))).ToString();
                            listRet.Add(new String[2] { null, strAux });
                            memChunks = memChunks >> 1;
                        }
                    }
                }
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

            // Bank ID
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
            bool ignoreDockTypeDetection = false;
            bool ignoreDockProgramStartDetection = false;
            for (int i = 0; i < 8; i++)
            {
                memoryBaseStart = i * GlobalMemorySizeConstants.KB8;
                memoryBaseEnd = ((i + 1) * GlobalMemorySizeConstants.KB8) - 1;
                this.MemoryBanksChunks[i] = new TimexCartridge8KChunk();

                nextByte = (int)structDCK.MemoryBankChunkType[i];
                this.MemoryBanksChunks[i].IsRAM = ((nextByte & CHUNK_IS_RAM_FLAG) == CHUNK_IS_RAM_FLAG);
                this.MemoryBanksChunks[i].IsOnFile = ((nextByte & CHUNK_MEM_PRESENT_FLAG) == CHUNK_MEM_PRESENT_FLAG);
                if ((nextByte & CHUNK_RESERVED_MASK) == 0)
                {
                    switch ((TimexCartridge8KChunkType)nextByte)
                    {
                        case TimexCartridge8KChunkType.NON_EXISTENT:
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.NON_EXISTENT;
                            switch (this.MemoryBankID)
                            {
                                case (int)TimexCartridgeBankID.DOCK:
                                    // In this bank, when reading from non existing ROM/RAM, 255 (FFh) is returned
                                    this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\xFF', GlobalMemorySizeConstants.KB8));
                                    break;

                                case (int)TimexCartridgeBankID.EXROM:
                                    // This memory bank is only partialy decoded (memory bank chunk 2), so the Extension ROM is shadoed in 8 KB chunks
                                    // from adresses 16384 to 24575 ([4000h-5FFFh])
                                    this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\xFF', GlobalMemorySizeConstants.KB8));
                                    break;

                                case (int)TimexCartridgeBankID.HOME:
                                    // For this bank, if the memory chunk is not on the cartridge, the system 16 KB Home ROM and/or RAM are used
                                    //this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                                    break;

                                default:
                                    // FIXME: Is this OK? Not supported memory bank
                                    this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\xFF', GlobalMemorySizeConstants.KB8));
                                    break;
                            }
                            break;

                        case TimexCartridge8KChunkType.RAM_NOT_ON_FILE:
                            // Fills the memory bank RAM chunk with 8 KB of zeros
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.RAM_NOT_ON_FILE;
                            this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                            this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                            break;

                        case TimexCartridge8KChunkType.ROM_ON_FILE:
                            // Checks if this is Dock memory bank with a LROS or an AROS program
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.ROM_ON_FILE;
                            this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                            if (this.MemoryBankID == (int)TimexCartridgeBankID.DOCK)
                            {
                                if (!ignoreDockTypeDetection)
                                {
                                    if (i == 0)
                                    {
                                        // This is a LROS machine code program
                                        this.CartridgeDockType = TimexCartridgeDockType.LROS;
                                        ignoreDockTypeDetection = true;
                                    }
                                    else if (i == 4)
                                    {
                                        // This is an AROS BASIC program with an optional machin code program
                                        this.CartridgeDockType = TimexCartridgeDockType.AROS;
                                        ignoreDockTypeDetection = true;
                                    }
                                }
                            }
                            // Else is a Home bank 8 KB memory chunk, or some non suported extencion bank
                            ++NumberOf8KChunksInFile;
                            break;

                        case TimexCartridge8KChunkType.RAM_ON_FILE:
                            // This is a presinstent 8KB memory bank chunk
                            this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.RAM_ON_FILE;
                            this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                            ++NumberOf8KChunksInFile;
                            break;

                        default:
                            // FIXME: Bug? Should't be where
                            break;
                    }
                }
                else
                {
                    // TODO: Bug? Unknown memory bank type
                    this.MemoryBanksChunks[i].Type = TimexCartridge8KChunkType.Unknown;
                    if (this.MemoryBanksChunks[i].IsOnFile)
                    {
                        // ROM or RAM from file
                        this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                        ++NumberOf8KChunksInFile;
                    }
                    if (this.MemoryBanksChunks[i].IsRAM)
                    {
                        // RAM not on file
                        this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                    }
                }
                // Read the 8 KB memory chunk from stream
                if (this.MemoryBanksChunks[i].IsOnFile)
                {
                    StreamUtils.ReadBytes(streamIn, ref this.MemoryBanksChunks[i].MemoryChunk);
                    if (this.MemoryBankID == (int)TimexCartridgeBankID.EXROM)
                    {
                        // Fix the partial decoding 
                        // This memory bank is only partialy decoded (memory bank chunk 2), so the Extension ROM is shadoed in 8 KB chunks
                        // from adresses 16384 to 24575 ([4000h-5FFFh])
                        if (i == 2)
                        {
                            this.MemoryBanksChunks[0].MemoryChunk = this.MemoryBanksChunks[2].MemoryChunk;
                            this.MemoryBanksChunks[1].MemoryChunk = this.MemoryBanksChunks[2].MemoryChunk;
                        }
                        else
                        {
                            this.MemoryBanksChunks[i].MemoryChunk = this.MemoryBanksChunks[2].MemoryChunk;
                        }
                    }
                    else if (this.MemoryBankID == (int)TimexCartridgeBankID.DOCK)
                    {
                        if (ignoreDockTypeDetection && !ignoreDockProgramStartDetection)
                        {
                            if (i == 0)
                            {
                                // This is a LROS machine code program
                                Stream stream = new MemoryStream(this.MemoryBanksChunks[i].MemoryChunk);
                                this.sLrosProgramHeader = (DckLrosStruct)StreamUtils.ReadStructure<DckLrosStruct>(stream);
                                // Starting Address endian fix
                                this.sLrosProgramHeader.ProgStartingAddress = EndianUtils.ConvertWord16_LE(this.sLrosProgramHeader.ProgStartingAddress);
                                // LROS program header read completed
                                ignoreDockProgramStartDetection = true;
                            }
                            else if (i == 4)
                            {
                                // This is an AROS BASIC program with an optional machine code program
                                Stream stream = new MemoryStream(this.MemoryBanksChunks[i].MemoryChunk);
                                this.sArosProgramHeader = (DckArosStruct)StreamUtils.ReadStructure<DckArosStruct>(stream);
                                // Starting Address endian fix
                                this.sArosProgramHeader.ProgStartingAddress = EndianUtils.ConvertWord16_LE(this.sArosProgramHeader.ProgStartingAddress);
                                // Reserved Machine Code RAM endian fix
                                this.sArosProgramHeader.ReservedRAM = EndianUtils.ConvertWord16_LE(this.sArosProgramHeader.ReservedRAM);
                                // AROS program header read completed
                                ignoreDockProgramStartDetection = true;
                            }
                        }
                    }
                }
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
        List<TimexCartridgeBank> TCCBanks;
        #endregion // Class Properties

        #region Class Constructors
        public TimexCartridge()
        {
            TCCBanks = new List<TimexCartridgeBank>();
        }

        public TimexCartridge(String fileName)
        {
            TCCBanks = new List<TimexCartridgeBank>();
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
        public List<String[]> GetInfo()
        {
            List<String[]> retList = new List<string[]>();
            retList.Add(new String[2] { "File Name", this.FileName });
            retList.Add(new String[2] { "File Size", this.FileSize.ToString() + " Bytes" });
            foreach (TimexCartridgeBank TCCBank in this.TCCBanks)
            {
                try
                {
                    retList.AddRange(TCCBank.GetInfo());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return retList;
        }

        public void Read(Stream cartridgeFile)
        {
            while ((this.FileSize - cartridgeFile.Position) >= 9)
            {
                try
                {
                    TimexCartridgeBank TCCBank = new TimexCartridgeBank();
                    TCCBank.Read(cartridgeFile);
                    this.TCCBanks.Add(TCCBank);
                }
                catch(Exception ex)
                {
                    throw ex;
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
                this.FileName = fileName;
                this.FileSize = cartridgeFile.Length;
                this.Read(cartridgeFile);
                long fileSize = this.FileSize - cartridgeFile.Position;
                if (fileSize != 0)
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

