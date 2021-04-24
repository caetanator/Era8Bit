/**
 * MediaDataTypeEnum.cs
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
    /// Enumeration that identifies the type of media data returned.
    /// </summary>
    public enum MediaDataTypeEnum
    {
        /// <summary>
        /// Unknown media data type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Memory manager media data type.
        /// </summary>
        Memory,

        /// <summary>
        /// Image media data type.
        /// </summary>
        Image,

        /// <summary>
        /// Music media data type.
        /// </summary>
        Music,

        /// <summary>
        /// File stream media data type.
        /// </summary>
        FileStream
    };
}
