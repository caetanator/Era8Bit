using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// 
    /// </summary>
    public class TimexCartridgeBank
    {
        #region Class Constants
        private const uint CHUNK_IS_RAM_FLAG = 0x01;
        private const uint CHUNK_MEM_PRESENT_FLAG = 0x02;
        private const uint CHUNK_RESERVED_MASK = 0xFC;
        #endregion // Class Constants

        #region Class Properties
        public bool DataChanged { get; private set; } = false;

        public int MemoryBankID { get; set; } = (int)Timex2068DefaultMemoryBanksIDsEnum.Unknown;

        public DockHeaderCartridgeTypeEnum CartridgeDockType { get; private set; } = DockHeaderCartridgeTypeEnum.Unknown;

        public DockHeaderLanguageTypeEnum CartridgeDockArosLanguage { get; private set; } = DockHeaderLanguageTypeEnum.Unknown;

        public int NumberOf8KChunksInFile { get; private set; } = 0;

        public MemoryBankChunk[] MemoryBanksChunks { get; private set; } = new MemoryBankChunk[8];

        public DockHeaderLrosStruct sLrosProgramHeader;
        public DockHeaderArosStruct sArosProgramHeader;
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
            return (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.HOME);
        }
        public bool IsExRomBank()
        {
            return (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM);
        }
        public bool IsDockBank()
        {
            return (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK);
        }

        public bool IsDockAROS()
        {
            return IsDockBank() && (CartridgeDockType == DockHeaderCartridgeTypeEnum.AROS);
        }
        public bool IsDockLROS()
        {
            return IsDockBank() && (CartridgeDockType == DockHeaderCartridgeTypeEnum.AROS);
        }

        public bool IsArosLangBASIC()
        {
            return IsDockAROS() && (CartridgeDockArosLanguage == DockHeaderLanguageTypeEnum.BASIC);
        }
        public bool IsArosLangMachineCode()
        {
            return IsDockAROS() && (CartridgeDockArosLanguage == DockHeaderLanguageTypeEnum.MACHINE_CODE);
        }

        public DockHeaderLrosStruct GetLrosProgramHeader()
        {
            return this.sLrosProgramHeader;
        }

        public DockHeaderArosStruct GetArosProgramHeader()
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
                    case (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK:
                        strAux = "DOCK Memory Bank";
                        break;

                    case (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM:
                        strAux = "EXROM Memory Bank";
                        break;

                    case (int)Timex2068DefaultMemoryBanksIDsEnum.HOME:
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
                        case DckHeaderMemoryBankChunkTypeEnum.NON_EXISTENT:
                            strAux = "System RAM/ROM";
                            break;

                        case DckHeaderMemoryBankChunkTypeEnum.RAM_NOT_ON_FILE:
                            strAux = "RAM (not on file)";
                            break;

                        case DckHeaderMemoryBankChunkTypeEnum.ROM_ON_FILE:
                            strAux = "ROM";
                            break;

                        case DckHeaderMemoryBankChunkTypeEnum.RAM_ON_FILE:
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
                if (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK)
                {
                    if (this.CartridgeDockType == DockHeaderCartridgeTypeEnum.LROS)
                    {
                        // This is a LROS machine code program
                        listRet.Add(new String[2] { "DOCK Memory Bank Type", "LROS" });

                        listRet.Add(new String[2] { "\tReserved (=0)", this.sLrosProgramHeader.LanguageType.ToString() });

                        if(this.sLrosProgramHeader.CartridgeType == (int)DockHeaderCartridgeTypeEnum.LROS)
                        {
                            strAux = "Machine Code";
                        }
                        else
                        {
                            strAux = String.Format("Unknown Language {0}", this.sLrosProgramHeader.CartridgeType).ToString();
                        }
                        listRet.Add(new String[2] { "\tPrograming Language", strAux });

                        strAux = String.Format("{0,5:####0} / {0:X4}h", this.sLrosProgramHeader.StartingCode).ToString();
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
                    else if (this.CartridgeDockType == DockHeaderCartridgeTypeEnum.AROS)
                    {
                        // This is an AROS BASIC program with an optional machine code program
                        listRet.Add(new String[2] { "DOCK Memory Bank Type", "AROS" });

                        if (this.sArosProgramHeader.CartridgeType == (int)DockHeaderCartridgeTypeEnum.AROS)
                        {
                            switch (this.sArosProgramHeader.LanguageType)
                            {
                                case (int)DockHeaderLanguageTypeEnum.BASIC:
                                    strAux = "BASIC";
                                    if (this.sArosProgramHeader.ReservedRAM > 0)
                                    {
                                        strAux = " + Machine Code";
                                    }
                                    break;

                                case (int)DockHeaderLanguageTypeEnum.MACHINE_CODE:
                                    strAux = "Machine Code";
                                    break;

                                default:
                                    strAux = String.Format("Unknown Language {0}", this.sArosProgramHeader.LanguageType).ToString();
                                    break;
                            }
                        }
                        else
                        {
                            strAux = String.Format("Unknown Type {0}", this.sArosProgramHeader.CartridgeType).ToString();
                        }
                        listRet.Add(new String[2] { "\tPrograming Language", strAux });

                        if (this.CartridgeDockArosLanguage == DockHeaderLanguageTypeEnum.BASIC)
                        {
                            // AROS BASIC
                            strAux = String.Format("{0,5:####0}", this.sArosProgramHeader.StartingCode).ToString();
                            listRet.Add(new String[2] { "\tBASIC Code Start Line", strAux });
                        }
                        else
                        {
                            // AROS Machine Code
                            strAux = String.Format("{0,5:####0} / {0:X4}h", this.sArosProgramHeader.StartingCode).ToString();
                            listRet.Add(new String[2] { "\tMachine Code Start Adress", strAux });
                        }

                        listRet.Add(new String[2] { "\tReserved RAM for Machine Code", this.sArosProgramHeader.ReservedRAM.ToString() + " Bytes"});

                        switch (this.sArosProgramHeader.AutoStartSpecification)
                        {
                            case 0:
                                strAux = "False";
                                break;

                            case 1:
                                strAux = "True";
                                break;

                            default:
                                strAux = String.Format("Unknown Autostart Specification {0}", this.sArosProgramHeader.AutoStartSpecification).ToString();
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
            //Assert.IsNull(streamIn);
            //Assert.IsTrue(streamIn.CanRead, nameof(streamIn), "Stream isn't readable");

            //// Read DCK File Header
            DckHeaderStruct structDCK = (DckHeaderStruct)StreamUtils.ReadStructure<DckHeaderStruct>(streamIn);

            // Bank ID
            int nextByte = (int)structDCK.MemoryBankID;
            if ((nextByte == (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK) || (nextByte == (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM) || (nextByte == (int)Timex2068DefaultMemoryBanksIDsEnum.HOME))
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
                this.MemoryBanksChunks[i] = new MemoryBankChunk();

                nextByte = (int)structDCK.MemoryBankChunkType[i];
                this.MemoryBanksChunks[i].IsRAM = ((nextByte & CHUNK_IS_RAM_FLAG) == CHUNK_IS_RAM_FLAG);
                this.MemoryBanksChunks[i].IsOnFile = ((nextByte & CHUNK_MEM_PRESENT_FLAG) == CHUNK_MEM_PRESENT_FLAG);
                if ((nextByte & CHUNK_RESERVED_MASK) == 0)
                {
                    switch ((DckHeaderMemoryBankChunkTypeEnum)nextByte)
                    {
                        case DckHeaderMemoryBankChunkTypeEnum.NON_EXISTENT:
                            this.MemoryBanksChunks[i].Type = DckHeaderMemoryBankChunkTypeEnum.NON_EXISTENT;
                            switch (this.MemoryBankID)
                            {
                                case (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK:
                                    // In this bank, when reading from non existing ROM/RAM, 255 (FFh) is returned
                                    this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\xFF', GlobalMemorySizeConstants.KB8));
                                    break;

                                case (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM:
                                    // This memory bank is only partialy decoded (memory bank chunk 2), so the Extension ROM is shadoed in 8 KB chunks
                                    // from adresses 16384 to 24575 ([4000h-5FFFh])
                                    this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                                    this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\xFF', GlobalMemorySizeConstants.KB8));
                                    break;

                                case (int)Timex2068DefaultMemoryBanksIDsEnum.HOME:
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

                        case DckHeaderMemoryBankChunkTypeEnum.RAM_NOT_ON_FILE:
                            // Fills the memory bank RAM chunk with 8 KB of zeros
                            this.MemoryBanksChunks[i].Type = DckHeaderMemoryBankChunkTypeEnum.RAM_NOT_ON_FILE;
                            this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                            this.MemoryBanksChunks[i].MemoryChunk = Encoding.ASCII.GetBytes(new string('\0', GlobalMemorySizeConstants.KB8));
                            break;

                        case DckHeaderMemoryBankChunkTypeEnum.ROM_ON_FILE:
                            // Checks if this is Dock memory bank with a LROS or an AROS program
                            this.MemoryBanksChunks[i].Type = DckHeaderMemoryBankChunkTypeEnum.ROM_ON_FILE;
                            this.MemoryBanksChunks[i].MemoryChunk = new byte[GlobalMemorySizeConstants.KB8];
                            if (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK)
                            {
                                if (!ignoreDockTypeDetection)
                                {
                                    if (i == 0)
                                    {
                                        // This is a LROS machine code program
                                        this.CartridgeDockType = DockHeaderCartridgeTypeEnum.LROS;
                                        ignoreDockTypeDetection = true;
                                    }
                                    else if (i == 4)
                                    {
                                        // This is an AROS BASIC program with an optional machin code program
                                        this.CartridgeDockType = DockHeaderCartridgeTypeEnum.AROS;
                                        ignoreDockTypeDetection = true;
                                    }
                                }
                            }
                            // Else is a Home bank 8 KB memory chunk, or some non suported extencion bank
                            ++NumberOf8KChunksInFile;
                            break;

                        case DckHeaderMemoryBankChunkTypeEnum.RAM_ON_FILE:
                            // This is a presinstent 8KB memory bank chunk
                            this.MemoryBanksChunks[i].Type = DckHeaderMemoryBankChunkTypeEnum.RAM_ON_FILE;
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
                    this.MemoryBanksChunks[i].Type = DckHeaderMemoryBankChunkTypeEnum.Unknown;
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
                    if (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM)
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
                    else if (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK)
                    {
                        if (ignoreDockTypeDetection && !ignoreDockProgramStartDetection)
                        {
                            if (i == 0)
                            {
                                // This is a LROS machine code program
                                Stream stream = new MemoryStream(this.MemoryBanksChunks[i].MemoryChunk);
                                this.sLrosProgramHeader = (DockHeaderLrosStruct)StreamUtils.ReadStructure<DockHeaderLrosStruct>(stream);
                                // Starting Address endian fix
                                this.sLrosProgramHeader.StartingCode = EndianUtils.ConvertWord16_LE(this.sLrosProgramHeader.StartingCode);
                                // LROS program header read completed
                                ignoreDockProgramStartDetection = true;
                            }
                            else if (i == 4)
                            {
                                // This is an AROS BASIC program with an optional machine code program
                                Stream stream = new MemoryStream(this.MemoryBanksChunks[i].MemoryChunk);
                                this.sArosProgramHeader = (DockHeaderArosStruct)StreamUtils.ReadStructure<DockHeaderArosStruct>(stream);
                                // Starting Address endian fix
                                this.sArosProgramHeader.StartingCode = EndianUtils.ConvertWord16_LE(this.sArosProgramHeader.StartingCode);
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
}
