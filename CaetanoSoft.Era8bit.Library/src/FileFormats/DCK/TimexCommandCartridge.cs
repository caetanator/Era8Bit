/**
 * TimexCommandCartridge.cs
 *
 * PURPOSE
 *  This is a library to read and write the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *  This class read and writes from the .DCK media file format that stores Timex Command Cartridges for the 
 *  Timex Sinclair/Computer 2068 (TS2068/TC2068) and Unipolbrit Komputer 2086 (UK2086).
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSoft.Era8bit.MediaFormats" project:
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
 *  2021-04-23: Major re-wright.
 */

using System;
using System.IO;
using System.Collections.Generic;

using CaetanoSoft.Era8bit.MediaFormats;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
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
        0, for non-existent chunks (reading from such chunks must return default values for given bank; for example, #FF in DOCK bank, 
           and ghost images of 8 KB Timex EXROM in EXROM bank) 
        1, for RAM chunks, where initial RAM content is not given (in the emulator such chunks will be initially filled with zeros) 
        2, for ROM chunks 
        3, for RAM chunks where initial RAM content is given (this is need to allow saving content of expanded RAM; also this is 
           useful for emulating non-volatile battery-protected RAM expansions) 

        After the header, a pure image of each presented chunk is stored in DCK file. Some examples will help understanding of such organization. 
        16 KB long LROS program needs header 0,2,2,0,0,0,0,0,0 in front of pure binary image of this program. 24 KB long AROS program needs header
        255,0,0,0,0,2,2,2,0 in front of binary image of it to become a valid DCK file. 64 KB DOCK RAM disc cartridge (64K of empty RAM) may be 
        described as only 9-byte long DCK file with content 0,1,1,1,1,1,1,1,1. 32 KB EXROM RAM disc cartridge mapped at address 32768 may be 
        described also using 9-byte long DCK file with content 254,0,0,0,0,1,1,1,1. If you put a 9-byte header 255,2,2,0,0,0,0,0,0 in front of 
        binary image of standard ZX Spectrum ROM, you will get DCK file which will replace Timex HOME ROM with ordinary Spectrum ROM (e.g. you 
        will achieve Timex Sinclair 2048). At the last, if you put a header 255,3,3,0,0,0,0,0,0 in front of binary image of Timex HOME ROM, you 
        will allow writing in the HOME ROM! 

        That's all if only one bank is stored in DCK file. Else, after the memory image, a new 9-byte header for next bank follows, and so on. 
        */


    /// <summary>
    /// DCK file format reader and writer.
    /// <para>This file format is used to store images of Timex Command Cartridges (TCC), used by TS2068 and TC2068 computers.</para>
    /// <para>TCC actually add up to 64K of additional ROM and/or RAM memory to the computer.</para>
    /// <para>Timex made the TS1510 cartridge player to add the command cartridges to the TS1000 and TS1500 computer. These cartridges 
    /// are incompatible with the TS2068 cartridges.</para>
    /// <para>ZX Interface 2 (IF2) cartridges are also incompatible.</para>
    /// </summary>
    public class TimexCommandCartridge : IMediaFormat
    {
        #region Interface IMediaFormat
        public override MediaFormatTypeEnum MediaFormatType { get; protected set; } = MediaFormatTypeEnum.Cartridge;

        public override MediaDataTypeEnum MediaDataType { get; protected set; } = MediaDataTypeEnum.Memory;

        public override String[] Extensions { get; protected set;  } = { "dck" };

        public override string Description
        {
            get
            {
                return "Timex Command Cartridge (TCC)";
            }

            protected set { }
        }

        public override String FileName { get; protected set; } = "";

        public override long FileSize { get; protected set; } = 0;

        public override bool DataChanged { get; protected set; } = false;

        public override void Read(Stream cartridgeFile)
        {
            while ((this.FileSize - cartridgeFile.Position) >= 9)
            {
                try
                {
                    TimexCommandCartridgeBanks TCCBank = new TimexCommandCartridgeBanks();
                    TCCBank.Read(cartridgeFile);
                    this.TCCBanks.Add(TCCBank);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public override void Write(Stream streamOut)
        {
            throw new NotImplementedException();
        }

        public override void Load(String fileName)
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

        public override void Save(String fileName, uint fileVersion = 0)
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

        public override List<String[]> GetInfo()
        {
            List<String[]> retList = new List<string[]>();
            retList.Add(new String[2] { "File Name", this.FileName });
            retList.Add(new String[2] { "File Size", this.FileSize.ToString() + " Bytes" });
            retList.Add(new String[2] { "File Type Description", this.Description });
            foreach (TimexCommandCartridgeBanks TCCBank in this.TCCBanks)
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

        public override object GetData()
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        public override void SetData(object objData)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }
        #endregion // Interface IMediaFormat

        #region Class Properties
        List<TimexCommandCartridgeBanks> TCCBanks;
        #endregion // Class Properties

        #region Class Constructors
        public TimexCommandCartridge()
        {
            TCCBanks = new List<TimexCommandCartridgeBanks>();
        }

        public TimexCommandCartridge(String fileName)
        {
            TCCBanks = new List<TimexCommandCartridgeBanks>();
            this.Load(fileName);
        }
        #endregion // Class Constructors

        #region Class Methods

        #endregion // Class Methods
    }
}

