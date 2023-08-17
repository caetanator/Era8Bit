/**
 * MediaFormatTypeEnum.cs
 *
 * PURPOSE
 *  This is a library to read and write the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *  This interface defines the methods necessary for all the media file types.
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
 *  2016-12-20: Created.
 *  2021-04-23: Major re-wright.
 */

namespace CaetanoSoft.Era8bit.MediaFormats
{
    /// <summary>
    /// Enumeration that identifies the type of media.
    /// </summary>
    public enum MediaFormatTypeEnum
    {
        /// <summary>
        /// Unknown media format type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Tape/Cassette media format type.
        /// </summary>
        Tape,

        /// <summary>
        /// Floppy Disk/Diskette media format type.
		/// <para>Used by  ZX Spectrum +3, +3B, Timex FDD 3 and 3000, etc..</para>
        /// </summary>
        FloppyDisk,

        /// <summary>
        /// Hard Disk media format type.
        /// </summary>
        HardDisk,

        /// <summary>
        /// Microdrive tape cartridge media format type.
        /// <para>Used by Sinclair ZX Interface I and Sinclair QL Microdrives.</para>
        /// </summary>
        MicrodriveCartridge,

        /// <summary>
        /// ROM and/or RAM memory cartridge media format type.
		/// <para>Used by Sinclair ZX Interface II and Timex TS/TC 2068.</para>
        /// </summary>
        Cartridge,

        /// <summary>
        /// Screen Dump image media format type.
        /// </summary>
        ScreenDump
    };
}
