/**
 * EndianAwareBinaryWriter.cs
 *
 * PURPOSE
 *  Implements auxiliary methods that are big-endian/little-endian aware to handle writing to binary streams.
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
 */

using System;
using System.IO;
using System.Text;

namespace CaetanoSoft.Utils.FileSystem.BinaryStreamUtils
{
    /// <summary>
    /// This class writes binary data (in the form of bytes) to a binary stream, aware of the endian of the system 
    /// and the stream.
    /// <para>The bytes order of the data is swapped automatic.</para>
    /// <para>On strings, the code page is also converted, if necessary.</para>
    /// </summary>
    public class EndianAwareBinaryWriter : BinaryWriter
    {
        /// <summary>Controls if the bytes must be swap because of unmatched endian.</summary>
        private bool swapBytesEndian = false;

        /// <summary>Initializes a new instance of the <see cref="EndianAwareBinaryWriter" /> class.</summary>
        /// <param name="output">The output stream to write data to.</param>
        /// <param name="encoding">The strings code-page encoding on the stream.</param>
        /// <param name="isLittleEndian">if set to <c>true</c> [the stream is little-endian]. 
        /// if set to <c>false</c> [is big-endian].</param>
        public EndianAwareBinaryWriter(Stream output, Encoding encoding, bool isLittleEndian) : base(output, encoding)
        {
            // If the system endian is equal to the stream endian, no byte swap is needed, otherwise,
            // if one is big-endian and the other little-endian, then the bytes order must be swapped
            this.swapBytesEndian = (isLittleEndian != BitConverter.IsLittleEndian) ? true : false;
        }

        /// <summary>Initializes a new instance of the <see cref="EndianAwareBinaryWriter" /> class.
        /// <para>Strings on the stream are assumed to the UTF-8 code-page encoding.</para>
        /// </summary>
        /// <param name="output">The output stream to write data to.</param>
        /// <param name="isLittleEndian">if set to <c>true</c> [the stream is little-endian]. 
        /// if set to <c>false</c> [is big-endian].</param>
        public EndianAwareBinaryWriter(Stream output, bool isLittleEndian) : this(output, Encoding.UTF8, isLittleEndian)
        {
            // Do nothing
        }

        /// <summary>Writes a two-byte unsigned integer to the current stream and advances the stream position by 
        /// two bytes.</summary>
        /// <param name="value">The two-byte unsigned integer to write.</param>
        /// <exception cref="ArgumentException">Value must be between 0 and 0xFFFF!</exception>
        public override void Write(ushort value)
        {
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

                // Write the 16-bit unsigned integer to the stream
                base.Write(buffer, 0, sizeof(ushort));
            }
            else
            {
                // Write the 16-bit unsigned integer to the stream
                base.Write(value);
            }
        }


        /// <summary>Writes a two-byte signed integer to the current stream and advances the stream position 
        /// by two bytes.</summary>
        /// <param name="value">The two-byte signed integer to write.</param>
        public override void Write(short value)
        {
            this.Write((ushort)value);
        }


        /// <summary>Writes a four-byte unsigned integer to the current stream and advances the stream position
        /// by four bytes.</summary>
        /// <param name="value">The four-byte unsigned integer to write.</param>
        public override void Write(uint value)
        {
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

                // Write the 32-bit unsigned integer to the stream
                base.Write(buffer, 0, sizeof(uint));
            }
            else
            {
                // Write the 32-bit unsigned integer to the stream
                base.Write(value);
            }
        }


        /// <summary>Writes a four-byte signed integer to the current stream and advances the stream position by 
        /// four bytes.</summary>
        /// <param name="value">The four-byte signed integer to write.</param>
        public override void Write(int value)
        {
            this.Write((uint)value);
        }


        /// <summary>Writes an eight-byte unsigned integer to the current stream and advances the stream position by 
        /// eight bytes.</summary>
        /// <param name="value">The eight-byte unsigned integer to write.</param>
        public override void Write(ulong value)
        {
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

                // Write the 64-bit unsigned integer to the stream
                base.Write(buffer, 0, sizeof(ulong));
            }
            else
            {
                // Write the 64-bit unsigned integer to the stream
                base.Write(value);
            }
        }


        /// <summary>Writes an eight-byte signed integer to the current stream and advances the stream position by 
        /// eight bytes.</summary>
        /// <param name="value">The eight-byte signed integer to write.</param>
        public override void Write(long value)
        {
            this.Write((ulong)value);
        }

        /// <summary>Writes a four-byte floating-point value to the current stream and advances the stream position by 
        /// four bytes.</summary>
        /// <param name="value">The four-byte floating-point value to write.</param>
        public override void Write(float value)
        {
            if (this.swapBytesEndian)
            {
                // Convert 32-bit floating-point to array
                /*
                byte[] buffer = new byte[4];
                uint num = *((uint*)&value);
                // LE
                buffer[3] = (byte)(num >> 24);
                buffer[2] = (byte)(num >> 16);
                buffer[1] = (byte)(num >> 8);
                buffer[0] = (byte)num;
                // BE
                buffer[0] = (byte)(num >> 24);
                buffer[1] = (byte)(num >> 16);
                buffer[2] = (byte)(num >> 8);
                buffer[3] = (byte)num;
                 */
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Write the 32-bit floating-point to the stream
                base.Write(buffer, 0, sizeof(float));
            }
            else
            {
                // Write the 32-bit floating-point to the stream
                base.Write(value);
            } 
        }

        /// <summary>Writes an eight-byte floating-point value to the current stream and advances the stream 
        /// position by eight bytes.</summary>
        /// <param name="value">The eight-byte floating-point value to write.</param>
        public override void Write(double value)
        {
            if (this.swapBytesEndian)
            {
                // Convert 64-bit floating-point to array
                /*
                byte[] buffer = new byte[8];
                ulong num = *((ulong*)&value);
                // LE
                buffer[7] = (byte)(num >> 56);
                buffer[6] = (byte)(num >> 48);
                buffer[5] = (byte)(num >> 40);
                buffer[4] = (byte)(num >> 32);
                buffer[3] = (byte)(num >> 24);
                buffer[2] = (byte)(num >> 16);
                buffer[1] = (byte)(num >> 8);
                buffer[0] = (byte)num;
                // BE
                buffer[0] = (byte)(num >> 56);
                buffer[1] = (byte)(num >> 48);
                buffer[2] = (byte)(num >> 40);
                buffer[3] = (byte)(num >> 32);
                buffer[4] = (byte)(num >> 24);
                buffer[5] = (byte)(num >> 16);
                buffer[6] = (byte)(num >> 8);
                buffer[7] = (byte)num;
                 */
                byte[] buffer = BitConverter.GetBytes(value);
                Array.Reverse(buffer);

                // Write the 64-bit floating-point to the stream
                base.Write(buffer, 0, sizeof(double));
            }
            else
            {
                // Write the 64-bit floating-point to the stream
                base.Write(value);
            }
        }

        /*
        /// <summary>Writes the string.</summary>
        /// <param name="str">The string.</param>
        /// <param name="size">The size.</param>
        /// <param name="encoding">The encoding.</param>
        /// <exception cref="System.ArgumentException">Invalid string size to write!</exception>
        public void WriteString(ref string str, int size, Encoding encoding = null)
        {
            // Validates the string size
            if (size <= 0)
            {
                // Error: Invalid size of string to write
                throw new System.ArgumentException("Invalid string size to write!");
            }

            // Validates the string encoding
            if (encoding == null)
            {
                // The default string encoding is UTF8
                encoding = Encoding.UTF8;
            }

            // Buffer to write to stream
            byte[] buffer = new byte[size + 1];
            buffer[size + 1] = 0;

            // Converts the string
            char[] chars = str.ToCharArray();
            Encoder encoder = encoding.GetEncoder();
            int charLen = encoder.GetBytes(chars, 0, chars.Length, buffer, 0, true);

            // Checks the size
            if (charLen > size)
            {
        #if DEBUG
                Trace.TraceError("charLen > size");
        #endif
                // TODO: What is the correct action? Throw an exception, ignore, ...
            }

            // Write the string bytes to the BinaryWriter
            this.WriteBytes(ref buffer, 0, size);
        }
        */
    }
}
