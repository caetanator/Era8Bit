using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    ///   <br />
    /// </summary>
    public enum MemoryBankChunkTypeEnum : byte
    {
        /// <summary>
        /// The ROM memory chunk isn't present on the file.
        /// <para>For non-existent chunks, reading from such chunks must return default values for given bank; for example:<br />
        /// &nbsp;&nbsp;&nbsp;&nbsp; * 255 (FFH) in DOCK bank;<br />
        /// &nbsp;&nbsp;&nbsp;&nbsp; * ghost images of the  8 KB Timex 2068 EXROM in EXROM bank.</para>
        /// </summary>
        ROM_NOT_ON_FILE = 0,

        /// <summary>
        /// The RAM memory chunk isn't present on the file.<br />
        /// Volatile RAM, allocate and fill with zeros (0).
        /// </summary>
        RAM_NOT_ON_FILE = 1,

        /// <summary>
        /// The ROM memory chunk is present on the file.<br />
        /// Allocate and load it from the file.
        /// </summary>
        ROM_ON_FILE = 2,

        /// <summary>
        /// The RAM memory chunk is present on the file.<br />
        /// Static RAM, allocate and load it from the file.
        /// </summary>
        RAM_ON_FILE = 3,
    };
}
