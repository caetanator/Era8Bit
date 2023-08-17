using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.Memory
{
    /// <summary>
    ///   <br />
    /// </summary>
    public enum MemoryBankChunkTypeEnum
    {
        /// <summary>
        /// Unknown memory chunk.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// ROM memory chunk, not present, use the system memory bank ROM or RAM.
        /// </summary>
        SYS_BANK_MEM = 0,

        /// <summary>
        /// Volatile RAM memory chunk.<br />
        /// RAM will be lost on power-down or reboot.
        /// </summary>
        RAM_Volatile = 1,

        /// <summary>
        /// ROM memory chunk.<br />
        /// Chunk can only be read.
        /// </summary>
        ROM = 2,

        /// <summary>
        /// Static RAM memory chunk.<br />
        /// RAM must be saved to not be lost on power-down or reboot.
        /// </summary>
        RAM_Static = 3,
    };
}
