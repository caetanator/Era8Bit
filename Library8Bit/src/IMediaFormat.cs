/**
 * IMediaFormat.cs
 *
 * PURPOSE
 *  This is a library to read and write the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *  This interface defines the methods necessary for all the media file types.
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
 *  2016-12-20: Created.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaetanoSof.Era8Bit.Library8Bit.MediaFormats
{
    public enum MediaFormatType
    {
        /// <summary>
        /// Unknown media format type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Tape/Cassette media format type.
        /// </summary>
        TAPE,

        /// <summary>
        /// Floppy Disk/Diskette media format type.
        /// </summary>
        DISK,

        /// <summary>
        /// Microdrive Cartridge media format type.
        /// <para>Used by Sinclair ZX Interface I and Sinclair QL.</para>
        /// </summary>
        MICRODRIVE_CARTRIDGE,

        /// <summary>
        /// Cartridge media format type.
        /// </summary>
        CARTRIDGE,

        /// <summary>
        /// Screen dump image media format type.
        /// </summary>
        SCREEN_DUMP
    };

    interface IMediaFormat
    {
        MediaFormatType Type { get; }
        String[] Extensions { get; }
        String Description { get; }

        String FileName { get; }
        long FileSize { get; }

        bool DataChanged { get; }

        void Read(Stream streamIn);
        void Write(Stream streamOut);

        void Load(String fileName);
        void Save(String fileName, uint fileVersion = 0);
    }
}
