using CaetanoSoft.Era8bit.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CaetanoSoft.Utils;
using CaetanoSoft.Utils.FileSystem.BinaryStreamUtils;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// 
    /// </summary>
    public class TimexCommandCartridgeBanks
    {
        #region Class Constants
        private const uint CHUNK_IS_RAM_FLAG = 0x01;
        private const uint CHUNK_MEM_PRESENT_FLAG = 0x02;
        private const uint CHUNK_RESERVED_MASK = 0xFC;
        #endregion // Class Constants

        #region Class Properties
        private DckFileHeaderStruct structDCK;

        public bool DataChanged { get; private set; } = false;

        public int MemoryBankID { get; set; } = (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK;

        public DockHeaderCartridgeTypeEnum CartridgeDockType { get; private set; } = 0;

        public DockHeaderLanguageTypeEnum CartridgeDockArosLanguage { get; private set; } = 0;

        public int NumberOf8KChunksInFile { get; private set; } = 0;

        public MemoryBankChunk[] MemoryBanksChunks { get; private set; } = new MemoryBankChunk[8];

        public DockHeaderLrosStruct sLrosProgramHeader;
        public DockHeaderArosStruct sArosProgramHeader;
        #endregion // Class Properties

        #region Class Constructors
        public TimexCommandCartridgeBanks()
        {
            // Do nothing
        }

        public TimexCommandCartridgeBanks(Stream streamIn)
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
            return IsDockBank() && (CartridgeDockType == DockHeaderCartridgeTypeEnum.LROS);
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
                        strAux = "Reserved REU Memory Bank ID " + this.MemoryBankID;
                        break;
                }
                listRet.Add(new String[2] { "Cartridge Memory Bank ID", strAux });

                // 8 KB memory chunks in the bank
                listRet.Add(new String[2] { "Cartridge 8 KB Memory Bank Chunk Type", null });
                int memoryBaseStart;
                int memoryBaseEnd;
                for (int i = 0; i < 8; i++)
                {
                    memoryBaseStart = i * MemorySizeConstants.KB8;
                    memoryBaseEnd = ((i + 1) * MemorySizeConstants.KB8) - 1;

                    if ((this.structDCK.MemoryBankChunkType[i] & CHUNK_MEM_PRESENT_FLAG) == CHUNK_MEM_PRESENT_FLAG)
                    {
                        if ((this.structDCK.MemoryBankChunkType[i] & CHUNK_IS_RAM_FLAG) == CHUNK_IS_RAM_FLAG)
                        {
                            strAux = "RAM (Static)";
                        }
                        else
                        {
                            strAux = "ROM";
                        }
                        
                    }
                    else
                    {
                        if ((this.structDCK.MemoryBankChunkType[i] & CHUNK_IS_RAM_FLAG) == CHUNK_IS_RAM_FLAG)
                        {
                            strAux = "RAM (Volatile)";
                        }
                        else
                        {
                            strAux = "ROM/RAM from HOME Bank";
                        }
                    }
                    uint reserved = this.structDCK.MemoryBankChunkType[i] & CHUNK_RESERVED_MASK;
                    if ((reserved) == 0)
                    {
                        // OK
                    }
                    strAux = String.Format("\t{0} - (Rb={4:X2}h) [{1,5:####0} - {2,5:####0}] / [{1:X4}h - {2:X4}h]: {3}", 
                                i, memoryBaseStart, memoryBaseEnd, strAux, reserved).ToString();
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
                        listRet.Add(new String[2] { "\tMachine Code Start Address", strAux });

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
                            listRet.Add(new String[2] { "\tMachine Code Start Address", strAux });
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

            //// 
            EndianAwareBinaryReader binaryReader = new EndianAwareBinaryReader(streamIn, true);
            //// Read DCK File Header
            structDCK = (DckFileHeaderStruct)binaryReader.ReadStructure<DckFileHeaderStruct>();

            // Bank ID
            int nextByte = (int)structDCK.MemoryBankID;
            if ((nextByte == (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK) || 
                 (nextByte == (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM) || 
                 (nextByte == (int)Timex2068DefaultMemoryBanksIDsEnum.HOME))
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
            for (int i = 0; i < structDCK.MemoryBankChunkType.Length; i++)
            {
                memoryBaseStart = i * MemorySizeConstants.KB8;
                memoryBaseEnd = ((i + 1) * MemorySizeConstants.KB8) - 1;
                nextByte = (int)structDCK.MemoryBankChunkType[i];
                if ((nextByte & CHUNK_IS_RAM_FLAG) == CHUNK_IS_RAM_FLAG)
                {

                }
                if ((nextByte & CHUNK_MEM_PRESENT_FLAG) == CHUNK_MEM_PRESENT_FLAG)
                {

                }
                if ((nextByte & CHUNK_RESERVED_MASK) == 0)
                {
                    switch ((DckFileHeaderMemoryBankChunkTypeEnum)nextByte)
                    {
                        case DckFileHeaderMemoryBankChunkTypeEnum.ROM_NOT_ON_FILE:
                            switch (this.MemoryBankID)
                            {
                                case (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK:
                                    // In this bank, when reading from non existing ROM/RAM, 255 (FFh) is returned
                                    this.MemoryBanksChunks[i] = new MemoryBankChunk((MemoryBankChunkTypeEnum)nextByte, MemorySizeConstants.KB8, 0xFF);
                                    break;

                                case (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM:
                                    // This memory bank is only partially decoded (memory bank chunk 2), so the Extension ROM is shadowed in 8 KB chunks
                                    // from addresses 16384 to 24575 ([4000h-5FFFh])
                                    this.MemoryBanksChunks[i] = new MemoryBankChunk((MemoryBankChunkTypeEnum)nextByte, MemorySizeConstants.KB8);
                                    // TODO: Copy EXROM
                                    break;

                                case (int)Timex2068DefaultMemoryBanksIDsEnum.HOME:
                                    // For this bank, if the memory chunk is not on the cartridge, the system 16 KB Home ROM and/or RAM are used
                                    this.MemoryBanksChunks[i] = new MemoryBankChunk((MemoryBankChunkTypeEnum)nextByte, MemorySizeConstants.KB8);
                                    // TODO: Copy ROM or RAM
                                    break;

                                default:
                                    // FIXME: Is this OK? Not supported memory bank
                                    this.MemoryBanksChunks[i] = new MemoryBankChunk((MemoryBankChunkTypeEnum)nextByte, MemorySizeConstants.KB8, 0x00);
                                    break;
                            }
                            break;

                        case DckFileHeaderMemoryBankChunkTypeEnum.RAM_NOT_ON_FILE:
                            // Fills the memory bank RAM chunk with 8 KB of zeros
                            this.MemoryBanksChunks[i] = new MemoryBankChunk((MemoryBankChunkTypeEnum)nextByte, MemorySizeConstants.KB8, 0x00);
                            break;

                        case DckFileHeaderMemoryBankChunkTypeEnum.ROM_ON_FILE:
                            // Checks if this is Dock memory bank with a LROS or an AROS program
                            this.MemoryBanksChunks[i] = new MemoryBankChunk((MemoryBankChunkTypeEnum)nextByte, MemorySizeConstants.KB8);
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
                                        // This is an AROS BASIC program with an optional machine code program
                                        this.CartridgeDockType = DockHeaderCartridgeTypeEnum.AROS;
                                        ignoreDockTypeDetection = true;
                                    }
                                }
                            }
                            // Else is a Home bank 8 KB memory chunk, or some non supported extension bank
                            ++this.NumberOf8KChunksInFile;
                            break;

                        case DckFileHeaderMemoryBankChunkTypeEnum.RAM_ON_FILE:
                            // This is a pressinstent 8KB memory bank chunk
                            this.MemoryBanksChunks[i] = new MemoryBankChunk((MemoryBankChunkTypeEnum)nextByte, MemorySizeConstants.KB8, 0x00);
                            ++this.NumberOf8KChunksInFile;
                            break;

                        default:
                            // FIXME: Bug? Shouldn't be where
                            break;
                    }
                }
                else
                {
                    // TODO: Bug? Unknown memory bank type
                    
                }
                // Read the 8 KB memory chunk from stream
                if ((structDCK.MemoryBankChunkType[i] & CHUNK_MEM_PRESENT_FLAG) == CHUNK_MEM_PRESENT_FLAG)
                {
                    this.MemoryBanksChunks[i].SetMemoryChunk(binaryReader.ReadBytes(MemorySizeConstants.KB8));

                    if (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.EXROM)
                    {
                        // Fix the partial decoding 
                        // This memory bank is only partialy decoded (memory bank chunk 2), so the Extension ROM is shadowed in 8 KB chunks
                        // from addresses 16384 to 24575 ([4000h-5FFFh])
                        if (i == 2)
                        {
                            this.MemoryBanksChunks[0].SetMemoryChunk(this.MemoryBanksChunks[2].GetMemoryChunk());
                            this.MemoryBanksChunks[1].SetMemoryChunk(this.MemoryBanksChunks[2].GetMemoryChunk());
                        }
                        else
                        {
                            this.MemoryBanksChunks[i].SetMemoryChunk(this.MemoryBanksChunks[2].GetMemoryChunk());
                        }
                    }
                    else if (this.MemoryBankID == (int)Timex2068DefaultMemoryBanksIDsEnum.DOCK)
                    {
                        if (ignoreDockTypeDetection && !ignoreDockProgramStartDetection)
                        {
                            if (i == 0)
                            {
                                // This is a LROS machine code program
                                Stream stream = new MemoryStream(this.MemoryBanksChunks[i].GetMemoryChunk());
                                EndianAwareBinaryReader binaryReaderBE = new EndianAwareBinaryReader(stream, false);
                                this.sLrosProgramHeader = (DockHeaderLrosStruct)binaryReaderBE.ReadStructure<DockHeaderLrosStruct>();
                                // LROS program header read completed
                                ignoreDockProgramStartDetection = true;
                            }
                            else if (i == 4)
                            {
                                // This is an AROS BASIC program with an optional machine code program
                                Stream stream = new MemoryStream(this.MemoryBanksChunks[i].GetMemoryChunk());
                                EndianAwareBinaryReader binaryReaderBE = new EndianAwareBinaryReader(stream, false);
                                this.sArosProgramHeader = (DockHeaderArosStruct)binaryReaderBE.ReadStructure<DockHeaderArosStruct>();
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
