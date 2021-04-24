using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// 
    /// </summary>
    public class TimexCC8KChunk
    {
        public DckHeaderMemoryBankChunkTypeEnum Type { get; set; } = DckHeaderMemoryBankChunkTypeEnum.Unknown;
        public bool IsRAM { get; set; } = false;
        public bool IsOnFile { get; set; } = false;
        public byte[] MemoryChunk;

        public TimexCC8KChunk()
        {
            // Do nothing
        }
    }
}
