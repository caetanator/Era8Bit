/**
 * IMediaFormat.cs
 *
 * PURPOSE
 *  This is a library to read and write the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *  This interface defines the methods necessary for all the media file types.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSoft.Era8bit.MediaFormat" project:
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

using System;
using System.IO;
using System.Collections.Generic;

namespace CaetanoSoft.Era8bit.MediaFormats
{
    /// <summary>
    ///   <br />
    /// </summary>
    public abstract class IMediaFormat
    {
        /// <summary>Gets or sets the type of the media file.<br />
        /// See <see cref="MediaFormatTypeEnum"/>.</summary>
        /// <value>The type of media file.</value>
        public abstract MediaFormatTypeEnum MediaFormatType { get; protected set; }

        /// <summary>Gets or sets the type of data on the media.<br />
        /// See <see cref="MediaDataTypeEnum"/>.</summary>
        /// <value>The type of data used.</value>
        public abstract MediaDataTypeEnum MediaDataType { get; protected set; }

        /// <summary>Gets or sets the extensions.</summary>
        /// <value>The extensions.</value>
        public abstract String[] Extensions { get; protected set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public abstract String Description { get; protected set; }

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public abstract String FileName { get; protected set; }

        /// <summary>Gets or sets the size of the file.</summary>
        /// <value>The size of the file.</value>
        public abstract long FileSize { get; protected set; }


        /// <summary>Gets or sets a value indicating whether [data changed].</summary>
        /// <value>
        ///   <c>true</c> if [data changed]; otherwise, <c>false</c>.</value>
        public abstract bool DataChanged { get; protected set; }


        /// <summary>Reads the specified stream in.</summary>
        /// <param name="streamIn">The stream in.</param>
        public abstract void Read(Stream streamIn);

        /// <summary>Writes the specified stream out.</summary>
        /// <param name="streamOut">The stream out.</param>
        public abstract void Write(Stream streamOut);

        /// <summary>Loads the specified file name.</summary>
        /// <param name="fileName">Name of the file.</param>
        public abstract void Load(String fileName);

        /// <summary>Saves the specified file name.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileVersion">The file version.</param>
        public abstract void Save(String fileName, uint fileVersion = 0);

        /// <summary>Gets the information.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public abstract List<String[]> GetInfo();

        /// <summary>Gets the data.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public abstract Object GetData();

        /// <summary>Sets the data.</summary>
        /// <param name="objData">The object data.</param>
        public abstract void SetData(Object objData);
    }
}
