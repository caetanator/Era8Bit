using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.Memory
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class MemoryBankChunk
    {
        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public MemoryBankChunkTypeEnum Type { get; private set; } = MemoryBankChunkTypeEnum.Unknown;

        public int Size { get; private set; } = -1;

        public bool IsROM { get { return (this.Type == MemoryBankChunkTypeEnum.ROM); } }

        public bool IsVolatilRAM { get { return (this.Type == MemoryBankChunkTypeEnum.RAM_Volatile); } }

        public bool IsStaticRAM { get { return (this.Type == MemoryBankChunkTypeEnum.RAM_Static); } }

        /// <summary>Gets or sets a value indicating whether this instance is RAM.</summary>
        /// <value>
        ///   <c>true</c> if this instance is ram; otherwise, <c>false</c>.</value>
        public bool IsRAM { get { return (this.Type == MemoryBankChunkTypeEnum.RAM_Volatile) || (this.Type == MemoryBankChunkTypeEnum.RAM_Static); } }

        private byte[] MemoryChunk = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBankChunk" /> class.<br />
        /// This creates a new memory bank chunk.<br />
        /// <para>
        /// If <c>type</c> is equal to <c>MemoryBankChunkTypeEnum.Unknown</c>, the memory bank chunk is not created.<br />
        /// If <c>size</c> is not between 1 byte and 512 MB, the memory bank chunk is not created.<br />
        /// If <c>initValue</c> is not between 0 and 255, the memory bank chunk is not initialized with the value.
        /// </para>
        /// </summary>
        /// <param name="type">The type of the chunk to one of the values of <see cref="MemoryBankChunkTypeEnum"/> except <c>MemoryBankChunkTypeEnum.Unknown</c>.</param>
        /// <param name="size">The size in bytes of the chunk (must be between 1 byte and 512 MB).</param>
        /// <param name="initValue">The initialize byte value for the memory bank chunk (must be between 0 and 255).</param>
        public MemoryBankChunk(MemoryBankChunkTypeEnum type, int size, int initValue = -1)
        {
            // Validate the memory bank chunk type
            if (type != MemoryBankChunkTypeEnum.Unknown)
            {
                // OK, it's ROM or RAM
                this.Type = type;

                // Validate the memory bank chunk size
                if ((size > 0) && (size <= MemorySizeConstants.KB512))
                {
                    // OK, size is more than 1 byte and less or equal to 512 MB
                    this.Size = size;
                    this.MemoryChunk = new byte[this.Size];

                    // Validate the memory bank chunk size
                    if ((initValue >= 0) && (initValue <= 255))
                    {
                        // OK, is between 0 and 255
                        for (int i = 0; i < this.MemoryChunk.Length; i++)
                        {
                            this.MemoryChunk[i] = (byte)initValue;
                        }
                    }
                }
                else
                {
                    // This memory bank chunk is invalid
                    this.Type = MemoryBankChunkTypeEnum.Unknown;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryBankChunk" /> class.<br />
        /// This creates a new memory bank chunk from a vector of bytes.<br />
        /// <para>
        /// If <c>type</c> is equal to <c>MemoryBankChunkTypeEnum.Unknown</c>, the memory bank chunk is not created.<br />
        /// If <c>chunk.Length</c> is not between 1 byte and 512 MB, the memory bank chunk is not created.<br />
        /// </para>
        /// </summary>
        /// <param name="type">The type of the chunk to one of the values of <see cref="MemoryBankChunkTypeEnum"/> except <c>MemoryBankChunkTypeEnum.Unknown</c>.</param>
        /// <param name="chunk">The vector bytes to be copied to the chunk.</param>
        public MemoryBankChunk(MemoryBankChunkTypeEnum type, byte [] chunk) : this(type, chunk.Length, -1)
        {
            // Validate the memory bank chunk type
            if (this.Type != MemoryBankChunkTypeEnum.Unknown)
            {
                // Validate the memory bank chunk size
                if ((this.Size >= 0) && (this.MemoryChunk.Length == chunk.Length))
                {
                    // OK, copy byte values
                    chunk.CopyTo(this.MemoryChunk, 0);
                }
            }
        }
     }
}
