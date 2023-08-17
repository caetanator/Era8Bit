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
        /// If <c>type</c> is equal to <c>DckFileHeaderMemoryBankChunkTypeEnum.Unknown</c>, the memory bank chunk is not created.<br />
        /// If <c>size</c> is not between 1 byte and 512 MB, the memory bank chunk is not created.<br />
        /// If <c>initValue</c> is not between 0 and 255, the memory bank chunk is not initialized with the value.
        /// </para>
        /// </summary>
        /// <param name="id">The type of the chunk to one of the values of <see cref="MemoryBankChunkTypeEnum"/> except <c>DckFileHeaderMemoryBankChunkTypeEnum.Unknown</c>.</param>
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
            if ((theByte >= 0) && (theByte <= 255))
            {
                // Byte OK
                if ((location >= 0) && (location < this.BankSize))
                {
                    // Memory Location OK
                    int chunkID = location % this.ChunkSize;
                    if ((chunkID >= 0) && (chunkID < this.BankChunks.Length))
                    {
                        // Memory Chunk ID OK
                        if (this.BankChunks != null)
                        {
                            // Memory Chunk Exists
                            MemoryBankChunk chunk = this.BankChunks[chunkID];
                            if (chunk != null)
                            {
                                int chunkPosition = location - (chunkID * this.ChunkSize);
                                if ((chunkPosition >= 0) && (chunkPosition < chunk.Size))
                                {
                                    // Memory Chunk Position OK
                                    if (chunk.IsRAM)
                                    {
                                        // Is RAM
                                        theByte = chunk.WriteByte(chunkPosition, theByte);
                                    }
                                    else
                                    {
                                        // Can't write to ROM
                                        return -1;
                                    }
                                }
                                else
                                {
                                    // Memory Chunk Position is invalid
                                    return -1;
                                }
                            }
                            else
                            {
                                // Memory Chunk Position Doesn't Exists
                                return -1;
                            }
                        }
                        else
                        {
                            // Memory Chunk isn't valid
                            return -1;
                        }
                    }
                    else
                    {
                        // Memory Chunk Doesn't Exists
                        return -1;
                    }
                }
                else
                {
                    // Memory Location invalid
                    return -1;
                }
            }
            else
            {
                // Byte invalid
                return -1;
            }

            return theByte;
        }

        /// <summary>Gets the memory bank chunk.</summary>
        /// <returns>
        /// An vector with the bytes of this chunk, if is valid;
        /// <para><c>null</c> otherwise.</para>
        /// </returns>
        public byte[] GetMemoryChunk(int chunkID)
        {
            if ((chunkID >= 0) && (chunkID < this.BankChunks.Length))
            {
                return (byte[])this.BankChunks[chunkID].GetMemoryChunk().Clone();
            }
            //else
            //{
                // Invalid memory bank chunk
                return null;
            //}
        }

        public bool SetMemoryChunk(int chunkID, byte[] chunk)
        {
#if DEBUG
            Assert.IsNotNull(chunk, "'chunk' can't be null!");
            Assert.IsFalse((chunk.Length >= 0), "'chunk.Length' must be 1 or more!");
            Assert.IsFalse(this.BankSize != chunk.Length, "'chunk.Length' must be equal to 'this.BankSize'!");
#endif
            if ((chunkID >= 0) && (chunkID < this.BankChunks.Length))
            {
                this.BankChunks[chunkID].SetMemoryChunk(chunk);
                return true;
            }
            //else
            //{
            // Invalid memory bank chunk
            return false;
            //}
        }
        #endregion // Class Methods
    }
}
