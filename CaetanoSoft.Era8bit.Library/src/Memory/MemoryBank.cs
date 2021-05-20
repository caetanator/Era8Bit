using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.Memory
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class MemoryBank
    {
        #region Class Properties
        /// <summary>Gets the memory bank identifier. Must be between 0 and 255.</summary>
        /// <value>The memory bank identifier.</value>
        public int BankID { get; private set; } = -1;

        /// <summary>Gets the memory bank size. Must be between 1 KB and 64 KB.</summary>
        /// <value>The memory bank size.</value>
        public int BankSize { get; private set; } = 0;

        /// <summary>Gets the number of chunks.</summary>
        /// <value>The number of chunks.</value>
        public int NumberOfChunks { get; private set; } = 0;

        /// <summary>Gets the size of the memory chunk. Must be between 1 KB and 64 KB</summary>
        /// <value>The size of the memory chunk.</value>
        public int ChunkSize { get; private set; } = 0;

        /// <summary>The memory bank chunks</summary>
        private MemoryBankChunk[] BankChunks = null;

        /// <summary>Gets a value indicating whether the data changed.</summary>
        /// <value>
        ///   <c>true</c> if data changed; otherwise, <c>false</c>.</value>
        public bool DataChanged { get; private set; } = false;
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
        /// <param name="id">The type of the chunk to one of the values of <see cref="MemoryBankChunkTypeEnum"/> except <c>MemoryBankChunkTypeEnum.Unknown</c>.</param>
        /// <param name="size">The size in bytes of the chunk (must be between 1 byte and 512 MB).</param>
        /// <param name="maxChunks">The initialize byte value for the memory bank chunk (must be between 0 and 255).</param>
        public MemoryBank(int id, int size, int numChunks)
        {
#if DEBUG
            Assert.IsFalse((id >= 0) && (id <= 255), "'id' must be between 0 and 255!");
            Assert.IsFalse((size >= MemorySizeConstants.KB1) && (size <= MemorySizeConstants.KB64), "'size' must be between 1 KB and 64 KB!");
            Assert.IsFalse((numChunks >= 1) && (numChunks <= 64), "'numChunks' must be between 0 and 64!");
#endif
            // Validate the memory bank chunk type
            if ((id >= 0) && (id <= 255))
            {
                // OK, bank ID is valid
                this.BankID = id;

                // Validate the memory bank chunk size
                if ((size >= MemorySizeConstants.KB1) && (size <= MemorySizeConstants.KB64))
                {
                    // OK, size is between than 1 KB and less or equal to 64 KB
                    this.BankSize = size;

                    // Validate the memory bank chunk size
                    if ((numChunks >= 1) && (numChunks <= 64))
                    {
                        // OK, is between 1 and 64 memory chunks
                        this.NumberOfChunks = numChunks;
                        this.ChunkSize = this.BankSize / this.NumberOfChunks;

                        // Allocate the memory vector
                        this.BankChunks = new MemoryBankChunk[this.NumberOfChunks];
                        for (int i = 0; i < this.NumberOfChunks; i++)
                        {
                            this.BankChunks[i] = null;
                        }
                    }
                }
                else
                {
                    // This memory bank is invalid
                    this.BankID = -1;
                    this.NumberOfChunks = 0;
                    this.ChunkSize = 0;
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
        /// The byte at memory <c>location</c>, if it's on the valid range.
        /// <para>-1, if <c>location</c> isn't valid; [0-255] otherwise.</para>
        /// </returns>
        public int ReadByte(int location)
        {
            int theByte = -1;

            if ((location >= 0) && (location < this.BankSize))
            {
                int chunkID = location % this.ChunkSize;
                int chunkPosition = location - (chunkID * this.ChunkSize);
                if (this.BankChunks != null)
                {
                    MemoryBankChunk chunk = this.BankChunks[location];
                    if (chunk != null)
                    {
                        theByte = chunk.ReadByte(chunkPosition);
                    }
                    // else
                    theByte = -1;
                }
                // else
                theByte = -1;
            }
            // else
            return theByte;
        }

        /// <summary>
        /// Writes the byte <c>theByte</c> at memory bank chunk <c>location</c>.
        /// <para>Valid values for <c>location</c> are [0-<c>BankSize</c>].</para>
        /// <para>Valid values for <c>theByte</c> are [0-255].</para>
        /// </summary>
        /// <param name="location">The location to write the byte.</param>
        /// <param name="theByte">The byte to be written.</param>
        /// <returns>
        /// The <c>theByte</c> at memory <c>location</c>, if it's on the valid range.
        /// <para>-1, if <c>location</c> isn't valid; [0-255] otherwise</para>
        /// </returns>
        public int WriteByte(int location, int theByte)
        {
            if ((location >= 0) && (location < this.BankSize))
            {
                int chunkID = location % this.ChunkSize;
                int chunkPosition = location - (chunkID * this.ChunkSize);
                if (this.BankChunks != null)
                {
                    MemoryBankChunk chunk = this.BankChunks[location];
                    if (chunk != null)
                    {
                        theByte = chunk.ReadByte(chunkPosition);
                    }
                    // else
                    theByte = -1;
                }
                // else
                theByte = -1;
            }
            // else
            return theByte;
            if (this.IsRAM)
            {
                if ((location >= 0) && (location < this.BankSize))
                {
                    if ((theByte >= 0) && (theByte <= 255))
                    {
                        int ret = (int)this.MemoryChunk[location];
                        this.MemoryChunk[location] = (byte)theByte;
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
                    // Invalid location
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
            Assert.IsFalse(this.BankSize != chunk.Length, "'chunk.Length' must be equal to 'this.BankSize'!");
#endif
            if (this.IsRAM)
            {
                if (this.Type != MemoryBankChunkTypeEnum.Unknown)
                {
                    if ((this.BankSize >= 0) && (this.MemoryChunk.Length == chunk.Length))
                    {
                        // OK, copy byte values
                        chunk.CopyTo(this.MemoryChunk, 0);
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
