﻿/**
 * EndianAwareBinaryReader.cs
 *
 * PURPOSE
 *  Implements auxiliary methods that are big-endian/little-endian aware to handle reading of binary streams.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSoft.Utils.FileSystem.BinaryStreamUtils" project:
 *      José Caetano Silva, jcaetano@users.sourceforge.net
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2006-2021 José Caetano Silva
 *
 * HISTORY
 *  2006-02-01: Created.
 *  2017-01-30: Major rewrite.
 *  2021-04-16: Renamed and updated.
 *  2024-12-13: Added method "public void SeekRelativePosition(int pos)".
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CaetanoSoft.Utils.FileSystem.BinaryStreamUtils
{
    /// <summary>
    /// This class writes binary data (in the form of bytes) to a binary stream, aware of the endian of the system and 
    /// the stream.
    ///   <br />
    /// The bytes order of the data is swapped automatic.
    ///   <br />
    /// On strings, the code page is also converted, if necessary.
    /// </summary>
    public class EndianAwareBinaryReader : BinaryReader
    {
        // ** Variables

        /// <summary>Controls if the bytes must be swap because of unmatched endian.</summary>
        private bool swapBytesEndian = false;

        // ** Constructors

        /// <summary>Initializes a new instance of the <see cref="EndianAwareBinaryReader" /> class.</summary>
        /// <param name="input">The streamIn stream to read data from.</param>
        /// <param name="encoding">The strings code-page encoding on the stream.</param>
		/// <param name="leaveOpen">if set to <c>true</c> [leave the stream open after the BinaryReader object is disposed]. 
        /// if set to <c>false</c> [closes the stream when the BinaryReader object is disposed].</param>
        /// <param name="isLittleEndian">if set to <c>true</c> [the stream is little-endian]. if set to <c>false</c> [is big-endian].</param>
        public EndianAwareBinaryReader(Stream streamIn, Encoding encoding, bool leaveOpen, bool isLittleEndian) : base(streamIn, encoding, leaveOpen)
        {
            // If the system endian is equal to the stream endian, no byte swap is needed, otherwise, if 
            // one is big-endian and the other little-endian, then the bytes order must be swapped
            this.swapBytesEndian = (isLittleEndian != BitConverter.IsLittleEndian) ? true : false;
        }

        /// summary>Initializes a new instance of the<see cref="EndianAwareBinaryReader" /> class.
        ///   <br />
        /// Strings on the stream are assumed to the UTF-8 code-page encoding.
        /// </summary>
        /// <param name="streamIn">The streamIn stream to read data from.</param>
        /// <param name="isLittleEndian">if set to <c>true</c> [the stream is little-endian]. if set to <c>false</c> [is big-endian].</param>
        public EndianAwareBinaryReader(Stream streamIn, bool isLittleEndian) : this(streamIn, Encoding.UTF8, false, isLittleEndian)
        {
            // Do nothing
        }

        // Override methods of BinaryReader

        public override ushort ReadUInt16()
        {
            // Read the 16-bit unsigned integer from the stream
            ushort value = base.ReadUInt16();

            if (this.swapBytesEndian)
            {
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Converts back to 16-bit unsigned integer
                value = BitConverter.ToUInt16(buffer, 0);
            }

            return value;
        }

        public override short ReadInt16()
        {
            // Read the 16-bit signed integer from the stream
            short value = base.ReadInt16();

            if (this.swapBytesEndian)
            {
                // Convert 16-bit unsigned integer to array
                /*
                byte[] buffer = new byte[2];
                // LE
                buffer[1] = (byte)(value >> 8);
                buffer[0] = (byte)(value);
                //BE
                buffer[0] = (byte)(value >> 8);
                buffer[1] = (byte)(value);
                 */
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Converts back to 16-bit signed integer
                value = BitConverter.ToInt16(buffer, 0);
            }

            return value;
        }

        /// <summary>Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.</summary>
        /// <returns>A 4-byte unsigned integer read from this stream.</returns>
        public override uint ReadUInt32()
        {
            // Read the 32-bit unsigned integer from the stream
            uint value = base.ReadUInt32();

            if (this.swapBytesEndian)
            {
                // Convert 32-bit unsigned integer to array
                /*
                byte[] buffer = new byte[4];
                // LE
                buffer[3] = (byte)(value >> 24);
                buffer[2] = (byte)(value >> 16);
                buffer[1] = (byte)(value >> 8);
                buffer[0] = (byte)value;
                // BE
                buffer[0] = (byte)(value >> 24);
                buffer[1] = (byte)(value >> 16);
                buffer[2] = (byte)(value >> 8);
                buffer[3] = (byte)value;
                 */
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Converts back to 32-bit unsigned integer
                value = BitConverter.ToUInt32(buffer, 0);
            }

            return value;
        }

        /// <summary>Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.</summary>
        /// <returns>A 4-byte signed integer read from the current stream.</returns>
        public override int ReadInt32()
        {
            // Read the 32-bit integer from the stream
            int value = base.ReadInt32();

            if (this.swapBytesEndian)
            {
                // Convert 32-bit signed integer to array
                /*
                byte[] buffer = new byte[4];
                // LE
                buffer[3] = (byte)(value >> 24);
                buffer[2] = (byte)(value >> 16);
                buffer[1] = (byte)(value >> 8);
                buffer[0] = (byte)value;
                // BE
                buffer[0] = (byte)(value >> 24);
                buffer[1] = (byte)(value >> 16);
                buffer[2] = (byte)(value >> 8);
                buffer[3] = (byte)value;
                 */
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Converts back to 32-bit signed integer
                value = BitConverter.ToInt16(buffer, 0);
            }

            return value;
        }

        /// <summary>Reads an 8-byte unsigned integer from the current stream and advances the 
        /// position of the stream by eight bytes.</summary>
        /// <returns>An 8-byte unsigned integer read from this stream.</returns>
        public override ulong ReadUInt64()
        {
            // Read the 64-bit unsigned integer from the stream
            ulong value = base.ReadUInt64();

            if (this.swapBytesEndian)
            {
                // Convert 64-bit unsigned integer to array
                /*
                byte[] buffer = new byte[8];
                // LE
                buffer[7] = (byte)(value >> 56);
                buffer[6] = (byte)(value >> 48);
                buffer[5] = (byte)(value >> 40);
                buffer[4] = (byte)(value >> 32);
                buffer[3] = (byte)(value >> 24);
                buffer[2] = (byte)(value >> 16);
                buffer[1] = (byte)(value >> 8);
                buffer[0] = (byte)value;
                // BE
                buffer[0] = (byte)(value >> 56);
                buffer[1] = (byte)(value >> 48);
                buffer[2] = (byte)(value >> 40);
                buffer[3] = (byte)(value >> 32);
                buffer[4] = (byte)(value >> 24);
                buffer[5] = (byte)(value >> 16);
                buffer[6] = (byte)(value >> 8);
                buffer[7] = (byte)value;
                 */
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Converts back to 64-bit unsigned integer
                value = BitConverter.ToUInt64(buffer, 0);
            }

            return value;
        }

        /// <summary>Reads an 8-byte signed integer from the current stream and advances the current 
        /// position of the stream by eight bytes.</summary>
        /// <returns>An 8-byte signed integer read from the current stream.</returns>
        public override long ReadInt64()
        {
            // Read the 64-bit signed integer from the stream
            int value = base.ReadInt32();

            if (this.swapBytesEndian)
            {
                // Convert 64-bit signed integer to array
                /*
                byte[] buffer = new byte[8];
                // LE
                buffer[7] = (byte)(value >> 56);
                buffer[6] = (byte)(value >> 48);
                buffer[5] = (byte)(value >> 40);
                buffer[4] = (byte)(value >> 32);
                buffer[3] = (byte)(value >> 24);
                buffer[2] = (byte)(value >> 16);
                buffer[1] = (byte)(value >> 8);
                buffer[0] = (byte)value;
                // BE
                buffer[0] = (byte)(value >> 56);
                buffer[1] = (byte)(value >> 48);
                buffer[2] = (byte)(value >> 40);
                buffer[3] = (byte)(value >> 32);
                buffer[4] = (byte)(value >> 24);
                buffer[5] = (byte)(value >> 16);
                buffer[6] = (byte)(value >> 8);
                buffer[7] = (byte)value;
                 */
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Converts back to 32-bit signed integer
                value = BitConverter.ToInt16(buffer, 0);
            }

            return value;
        }

        /// <summary>Reads a 4-byte floating point value from the current stream and advances the current 
        /// position of the stream by four bytes.</summary>
        /// <returns>A 4-byte floating point value read from the current stream.</returns>
        public override float ReadSingle()
        {
            // Read the 32-bit floating-point from the stream
            float value = -0.0f;

            if (this.swapBytesEndian)
            {
                /*
                // LE
                buffer[3] = (byte)(value >> 24);
                buffer[2] = (byte)(value >> 16);
                buffer[1] = (byte)(value >> 8);
                buffer[0] = (byte)value;
                // BE
                buffer[0] = (byte)(value >> 24);
                buffer[1] = (byte)(value >> 16);
                buffer[2] = (byte)(value >> 8);
                buffer[3] = (byte)value;
                 */

                // Convert 32-bit floating-point to array
                byte[] buffer = new byte[sizeof(float)];
                this.ReadToBuffer(ref buffer);
                Array.Reverse(buffer);

                // Converts back to 32-bit floating-point
                value = BitConverter.ToSingle(buffer, 0);
            }

            return value;
        }

        /// <summary>Reads an 8-byte floating point value from the current stream and advances the current 
        /// position of the stream by eight bytes.</summary>
        /// <returns>An 8-byte floating point value read from the current stream.</returns>
        public override double ReadDouble()
        {
            // Read the 64-bit floating-point from the stream
            double value = -0.0d;

            if (this.swapBytesEndian)
            {
                /*
                // LE
                buffer[3] = (byte)(value >> 24);
                buffer[2] = (byte)(value >> 16);
                buffer[1] = (byte)(value >> 8);
                buffer[0] = (byte)value;
                // BE
                buffer[0] = (byte)(value >> 24);
                buffer[1] = (byte)(value >> 16);
                buffer[2] = (byte)(value >> 8);
                buffer[3] = (byte)value;
                 */

                // Convert 64-bit floating-point to array
                byte[] buffer = new byte[sizeof(double)];
                this.ReadToBuffer(ref buffer);
                Array.Reverse(buffer);

                // Converts back to 64-bit floating-point
                value = BitConverter.ToDouble(buffer, 0);
            }
            else
            {
                value = base.ReadDouble();
            }

            return value;
        }

        // ** Methods

        /// <summary>Reads to buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <exception cref="System.IO.EndOfStreamException">Attempted to read past the end of the stream.</exception>
        private void ReadToBuffer(ref byte[] buffer)
        {
            int offset = 0;
            int num_read = 0;

            if (buffer.Length == 1)
            {
                num_read = base.ReadByte();
                if (num_read == -1)
                {
                    throw new EndOfStreamException("Attempted to read past the end of the stream.");
                }
                buffer[0] = (byte)num_read;
            }
            else
            {
                do
                {
                    num_read = base.Read(buffer, offset, buffer.Length - offset);
                    if (num_read == 0)
                    {
                        throw new EndOfStreamException("Attempted to read past the end of the stream.");
                    }
                    offset += num_read;
                }
                while (offset < num_read);
            }
        }

        /*
         public static string ReadString(Stream stream, int size, Encoding encoding = null)
        {
            // Validates the string size
            if (size <= 0)
            {
                // Error: Invalid size of string to read
                throw new System.ArgumentException("Invalid string size to read!");
            }

            // Validates the streamIn stream
            if (stream == null)
            {
                // Error: Invalid stream o read from
                throw new System.ArgumentNullException("Input stream is NULL!");
            }

            if ((stream.Length - stream.Position) < size)
            {
                // Error: Not enough bytes to read from stream
                throw new System.IO.IOException("Not enough bytes on the streamIn stream!");
            }

            // Validates the string encoding
            if (encoding == null)
            {
                // The default string encoding is UTF8
                encoding = Encoding.UTF8;
            }

            // Read the string bytes from the stream
            byte[] buffer = new byte[size + 1];
            buffer[size + 1] = 0;
            int readBytes = stream.Read(buffer, 0, size);
            if (readBytes != size)
            {
                // Couldn't read all the bytes
                if(readBytes == 0)
                {
                    throw new System.IO.IOException("Stream is at EOF (End of File)!");
                }
                else
                {
                    throw new System.IO.IOException("Couldn't read all the bytes!");
                }
            }

            // Converts the string
            char[] chars = new char[size + 1];
            chars[size + 1] = '\0';
            Decoder decoder = encoding.GetDecoder();
            int charLen = decoder.GetChars(buffer, 0, buffer.Length, chars, 0);
            String StrUTF8 = new String(chars);

            // Return string
            return StrUTF8;
        }
         */

        private static void SwapStructFieldsEndianness(Type type, byte[] data, bool swapBytesEndian, int startOffset = 0)
		{
			// Swap big-endian/little-endian
			if (swapBytesEndian)
			{
				foreach (var field in type.GetFields())
				{
					var fieldType = field.FieldType;
					if (field.IsStatic)
					{
						// Don't process static fields
						continue;
					}
					
					if (fieldType == typeof(string))
					{
						// Don't swap bytes for strings
						continue;
					}

					var offset = Marshal.OffsetOf(type, field.Name).ToInt32();

					// Handle enums
					if (fieldType.IsEnum)
					{
						fieldType = Enum.GetUnderlyingType(fieldType);
					}

                    // Check for sub-fields to recurse if necessary
                    var subFields = fieldType.GetFields();
                    // .Where(subField => subField.IsStatic == false).ToArray();

					var effectiveOffset = startOffset + offset;

					if (subFields.Length == 0)
					{
						Array.Reverse(data, effectiveOffset, Marshal.SizeOf(fieldType));
					}
					else
					{
						// Recurse
						SwapStructFieldsEndianness(fieldType, data, swapBytesEndian, effectiveOffset);
					}
				}
			}
		}

		public T? ReadStructure<T>() where T : struct
        {          
            int size = Marshal.SizeOf<T>();
            byte[] rawData = new byte[size];
			
            if (base.Read(rawData, 0, size) != size)
            {
                // Error: Not enough bytes to read the structure
                return null;
            }

			// Swap big-endian/little-endian
			if (this.swapBytesEndian)
			{
				SwapStructFieldsEndianness(typeof(T), rawData, this.swapBytesEndian);
			}
			
            // Convert the structure to .NET
            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>Seeks the relative position.</summary>
        /// <param name="pos">The position.</param>
        public void SeekRelativePosition(int pos)
        {
            if (this.BaseStream.CanSeek)
                this.BaseStream.Seek(pos, SeekOrigin.Current);
            else
                throw new EndOfStreamException("Can't seek stream.");
        }
    }
}
