using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.Memory
{
    /// <summary>
    /// This class implements a type of memory chunk, that can be ROM, volatile RAM or static RAM.
    /// </summary>
    public class MemoryBankChunk
    {
        #region Class Properties
        /// <summary>
        /// Gets or sets the type of the memory chunk. See <see cref="MemoryBankChunkTypeEnum" /> enumeration for more information.<br />
        /// If this value is <c>MemoryBankChunkTypeEnum.Unknown</c>, them it's content is invalid. </summary>
        /// <value>The memory chunk type.</value>
        public MemoryBankChunkTypeEnum Type { get; private set; } = MemoryBankChunkTypeEnum.Unknown;

        /// <summary>Gets the size.</summary>
        /// <value>The size.</value>
        public int Size { get; private set; } = 0;

        /// <summary>The memory chunk</summary>
        private byte[] MemoryChunk = null;

        /// <summary>Gets a value indicating whether the data in this memory chunk as changed.</summary>
        /// <value><c>true</c> if the data changed; otherwise, <c>false</c>.</value>
        public bool DataChanged { get; private set; } = false;

        /// <summary>Gets a value indicating whether this instance is ROM.</summary>
        /// <value><c>true</c> if this instance is ROM; otherwise, <c>false</c>.</value>
        public bool IsROM { get { return (this.Type == MemoryBankChunkTypeEnum.ROM); } }

        /// <summary>Gets a value indicating whether this instance is volatile RAM.</summary>
        /// <value><c>true</c> if this instance is volatile RAM; otherwise, <c>false</c>.</value>
        public bool IsVolatilRAM { get { return (this.Type == MemoryBankChunkTypeEnum.RAM_Volatile); } }

        /// <summary>Gets a value indicating whether this instance is static RAM.</summary>
        /// <value><c>true</c> if this instance is static RAM; otherwise, <c>false</c>.</value>
        public bool IsStaticRAM { get { return (this.Type == MemoryBankChunkTypeEnum.RAM_Static); } }

        /// <summary>Gets or sets a value indicating whether this instance is RAM.</summary>
        /// <value><c>true</c> if this instance is RAM; otherwise, <c>false</c>.</value>
        public bool IsRAM { get { return (this.Type == MemoryBankChunkTypeEnum.RAM_Volatile) || (this.Type == MemoryBankChunkTypeEnum.RAM_Static); } }
        #endregion // Class Properties

        #region Class Constructors
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
#if DEBUG
            Assert.IsFalse((type != MemoryBankChunkTypeEnum.Unknown) && 
                            ((type == MemoryBankChunkTypeEnum.ROM) || (type == MemoryBankChunkTypeEnum.RAM_Static) || 
                             (type == MemoryBankChunkTypeEnum.RAM_Volatile)), "'type' must be RAM or ROM!");
            Assert.IsFalse((size >= MemorySizeConstants.KB1) && (size <= MemorySizeConstants.KB64) && ((size % MemorySizeConstants.KB1) == 0), "'size' must be 1 KB to 64 KB!");
            Assert.IsFalse((initValue >= 0) && (initValue <= 255), "'initValue' must be between 0 and 255!");
#endif
            // Validate the memory bank chunk type
            if (type != MemoryBankChunkTypeEnum.Unknown)
            {
                // OK, it's ROM or RAM
                this.Type = type;

                // Validate the memory bank chunk size
                if ((size > MemorySizeConstants.KB1) && (size <= MemorySizeConstants.KB64) && ((size % MemorySizeConstants.KB1) == 0))
                {
                    // OK, size is more or equal to 1 KB and less or equal to 64 KB, in multiples of 1 KB
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
                    else
                    {
                        // This memory bank chunk is invalid
                        this.Type = MemoryBankChunkTypeEnum.Unknown;
                        this.Size = 0;
                        this.MemoryChunk = null;
                    }
                }
                else
                {
                    // This memory bank chunk is invalid
                    this.Type = MemoryBankChunkTypeEnum.Unknown;
                    this.Size = 0;
                    this.MemoryChunk = null;
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
#if DEBUG
            Assert.IsNotNull(chunk, "'chunk' can't be null!");
            Assert.IsFalse((type != MemoryBankChunkTypeEnum.Unknown) &&
                            ((type == MemoryBankChunkTypeEnum.ROM) || (type == MemoryBankChunkTypeEnum.RAM_Static) ||
                             (type == MemoryBankChunkTypeEnum.RAM_Volatile)), "'type' must be RAM or ROM!");
            Assert.IsFalse((chunk.Length >= MemorySizeConstants.KB1) && (chunk.Length <= MemorySizeConstants.KB64) && ((chunk.Length % MemorySizeConstants.KB1) == 0), "'chunk.Length' must be 1 KB to 64 KB!");
#endif
            // Validate the memory bank chunk type
            if (this.Type != MemoryBankChunkTypeEnum.Unknown)
            {
                // Validate the memory bank chunk size
                if ((chunk.Length >= MemorySizeConstants.KB1) && (chunk.Length <= MemorySizeConstants.KB64) && ((chunk.Length % MemorySizeConstants.KB1) == 0))
                {
                    // OK, copy byte values
                    this.Size = chunk.Length;
                    this.MemoryChunk = new byte[this.Size];
                    chunk.CopyTo(this.MemoryChunk, 0);
                }
                else
                {
                    // This memory bank chunk is invalid
                    this.Type = MemoryBankChunkTypeEnum.Unknown;
                    this.Size = 0;
                    this.MemoryChunk = null;
                }
            }
        }
        #endregion // Class Constructors

        #region Class Methods
        /// <summary>
        /// Reads the byte at memory bank chunk <c>location</c>.
        /// <para>Valid values are [0-<c>BankSize</c>].</para>
        /// </summary>
        /// <param name="location">The location to read the byte.</param>
        /// <returns>
        /// The byte at memory <c>location</c> [0-255], if the <c>location</c> is on the valid range;
        /// <para>-1, if <c>location</c> isn't valid</para>
        /// </returns>
        public int ReadByte(int location)
        {
            if ((location >= 0) && (location < this.Size))
            {
                return (int)this.MemoryChunk[location];
            }
            //else
            //{
            // Invalid byte location
            return -1;
            //}
        }

        /// <summary>
        /// Writes the byte <c>theByte</c> at memory bank chunk <c>location</c>.
        /// <para>Valid values for <c>location</c> are [0-<c>BankSize</c>].</para>
        /// <para>Valid values for <c>theByte</c> are [0-255].</para>
        /// </summary>
        /// <param name="location">The location to write the byte.</param>
        /// <param name="theByte">The byte to be written.</param>
        /// <returns>
        /// The previous byte at memory <c>location</c> [0-255], if the <c>location</c> and the <c>theByte</c> 
        /// are on the valid range;
        /// <para>-1, if <c>location</c> or <c>theByte</c> aren't valid.</para>
        /// </returns>
        public int WriteByte(int location, int theByte)
        {
            if (this.IsRAM)
            {
                if ((location >= 0) && (location < this.Size))
                {
                    if ((theByte >= 0) && (theByte <= 255))
                    {
                        int ret = (int)this.MemoryChunk[location];
                        this.MemoryChunk[location] = (byte)theByte;
                        this.DataChanged = true;
                        return ret;
                    }
                    //else
                    //{
                    // Invalid byte
                    return -1;
                    //}
                }
                //else
                //{
                    // Invalid byte location
                    return -1;
                //}
            }
            //else
            //{
                // Can't write to ROM
                return -1;
            //}
        }

        /// <summary>Gets the memory bank chunk.</summary>
        /// <returns>
        /// An vector with the bytes of this chunk, if is valid;
        /// <para><c>null</c> otherwise.</para>
        /// </returns>
        public byte[] GetMemoryChunk()
        {
            if (this.Type != MemoryBankChunkTypeEnum.Unknown)
            {
                return (byte[])this.MemoryChunk.Clone();
            }
            //else
            //{
                // Invalid memory bank chunk
                return null;
            //}
        }

        public bool SetMemoryChunk(byte[] chunk)
        {
#if DEBUG
            Assert.IsNotNull(chunk, "'chunk' can't be null!");
            Assert.IsFalse((chunk.Length >= 0), "'chunk.Length' must be 1 or more!");
            Assert.IsFalse(this.Size != chunk.Length, "'chunk.Length' must be equal to 'this.BankSize'!");
#endif
            if (this.IsRAM)
            {
                if (this.Type != MemoryBankChunkTypeEnum.Unknown)
                {
                    if ((this.Size >= 0) && (this.MemoryChunk.Length == chunk.Length))
                    {
                        // OK, copy byte values
                        chunk.CopyTo(this.MemoryChunk, 0);
                        this.DataChanged = true;
                        return true;
                    }
                    //else
                    //{
                        // Invalid byte
                        return false;
                    //}
                }
                //else
                //{
                    // Invalid memory bank chunk
                    return false;
                //}
            }
            //else
            //{
                // Can't write to ROM
                return false;
            //}
        }
        #endregion // Class Methods
    }
}
