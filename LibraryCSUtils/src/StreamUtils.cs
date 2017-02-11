/**
 * StreamUtils.cs
 *
 * PURPOSE
 *  Implements auxiliary methods to handle streams.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSof.Utils" project:
 *      José Caetano Silva, jcaetano@users.sourceforge.net
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2006-2017 José Caetano Silva
 *
 * HISTORY
 *  2006-02-01: Created.
 *  2017-01-30: Major rewrite.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CaetanoSof.Utils.StreamUtils
{
    public static class StreamUtils
    {
        public static string ReadString(Stream stream, int size, Encoding encoding = null)
        {
            // Validates the string size
            if (size <= 0)
            {
                // Error: Invalid size of string to read
                throw new System.ArgumentException("Invalid string size to read!");
            }

            // Validates the input stream
            if (stream == null)
            {
                // Error: Invalid stream o read from
                throw new System.ArgumentNullException("Input stream is NULL!");
            }

            if ((stream.Length - stream.Position) < size)
            {
                // Error: Not enough bytes to read from stream
                throw new System.IO.IOException("Not enough bytes on the input stream!");
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

        public static void WriteString(Stream stream, ref string str, int size, Encoding encoding = null)
        {
            // Validates the string size
            if (size <= 0)
            {
                // Error: Invalid size of string to read
                throw new System.ArgumentException("Invalid string size to read!");
            }

            // Validates the output stream
            if (stream == null)
            {
                // Error: Invalid stream o write to
                throw new System.ArgumentNullException("Output stream is NULL!");
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

            // Wite the string bytes to the stream
            stream.Write(buffer, 0, size);
            //stream.Flush();
        }

        public static byte ReadByte(Stream stream)
        {
            // Validates the input stream
            if (stream == null)
            {
                // Error: Invalid stream o read from
                throw new System.ArgumentNullException("Input stream is NULL!");
            }

            int readByte = stream.ReadByte();
            if (readByte == -1)
            {
                throw new System.IO.IOException("Stream is at EOF (End of File)!");
            }

            return (byte)(readByte & 0xFF);
        }

        public static void WriteByte(Stream stream, byte b)
        {
            // Validates the output stream
            if (stream == null)
            {
                // Error: Invalid stream o write to
                throw new System.ArgumentNullException("Output stream is NULL!");
            }
    
            stream.WriteByte((byte)b);
            //stream.Flush();
        }

        public static void ReadBytes(Stream stream, ref byte[] buffer)
        {
            // Validates the input stream
            if (stream == null)
            {
                // Error: Invalid stream o read from
                throw new System.ArgumentNullException("Input stream is NULL!");
            }

            if ((stream.Length - stream.Position) < buffer.Length)
            {
                // Error: Not enough bytes to read from stream
                throw new System.IO.IOException("Not enough bytes on the input stream!");
            }

            int readBytes = stream.Read(buffer, 0, buffer.Length);
            if (readBytes != buffer.Length)
            {
                // Couldn't read all the bytes
                if (readBytes == 0)
                {
                    throw new System.IO.IOException("Stream is at EOF (End of File)!");
                }
                else
                {
                    throw new System.IO.IOException("Couldn't read all the bytes!");
                }
            }
        }

        public static void PutBytes(Stream stream, ref byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();
        }

        public static uint GetWord16_BE(Stream stream)
        {
            byte[] buffer = new byte[2];
            ushort word = 0;
            int ret;

            ret = stream.Read(buffer, 0, buffer.Length);
            if ((ret == -1) || (ret != buffer.Length))
                throw new System.IO.IOException("Stream is at EOF (End of File)!");

            word = (ushort)((buffer[0] << 8) | buffer[1]);

            return (uint)(word & 0xFFFF);
        }

        public static void PutWord16_BE(Stream stream, uint word)
        {
            byte[] buffer = new byte[2];

            if (word != (word & 0xFFFF))
                throw new ArgumentException("Value must be between 0 and 0xFFFF!");
            buffer[0] = (byte)(word >> 8);
            buffer[1] = (byte)(word);
            stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();
        }

        public static uint GetWord16_LE(Stream stream)
        {
            byte[] buffer = new byte[2];
            ushort word = 0;
            int ret;

            ret = stream.Read(buffer, 0, buffer.Length);
            if ((ret == -1) || (ret != buffer.Length))
                throw new System.IO.IOException("Stream is at EOF (End of File)!");

            word = (ushort)((buffer[1] << 8) | buffer[0]);

            return (uint)(word & 0xFFFF);
        }

        public static void PutWord16_LE(Stream stream, uint word)
        {
            byte[] buffer = new byte[2];

            if (word != (word & 0xFFFF))
                throw new ArgumentException("Value must be between 0 and 0xFFFF!");
            buffer[1] = (byte)(word >> 8);
            buffer[0] = (byte)(word);
            stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();
        }

        public static uint GetWord32_BE(Stream stream)
        {
            byte[] buffer = new byte[4];
            int word = 0;
            int ret;

            ret = stream.Read(buffer, 0, buffer.Length);
            if ((ret == -1) || (ret != buffer.Length))
                throw new System.IO.IOException("Stream is at EOF (End of File)!");

            word = (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3];
            return (uint)(word & 0xFFFFFFFF);
        }

        public static void PutWord32_BE(Stream stream, uint word)
        {
            byte[] buffer = new byte[4];

            buffer[0] = (byte)(word >> 24);
            buffer[1] = (byte)(word >> 16);
            buffer[2] = (byte)(word >> 8);
            buffer[3] = (byte)word;
            stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();
        }

        public static uint GetWord32_LE(Stream stream)
        {
            byte[] buffer = new byte[4];
            int word = 0;
            int ret;

            ret = stream.Read(buffer, 0, buffer.Length);
            if ((ret == -1) || (ret != buffer.Length))
                throw new System.IO.IOException("Stream is at EOF (End of File)!");

            word = (buffer[3] << 24) | (buffer[2] << 16) | (buffer[1] << 8) | buffer[0];
            return (uint)(word & 0xFFFFFFFF);
        }

        public static void PutWord32_LE(Stream stream, uint word)
        {
            byte[] buffer = new byte[4];

            buffer[3] = (byte)(word >> 24);
            buffer[2] = (byte)(word >> 16);
            buffer[1] = (byte)(word >> 8);
            buffer[0] = (byte)word;
            stream.Write(buffer, 0, buffer.Length);
            //stream.Flush();
        }

        public static T? ReadStructure<T>(Stream stream) where T : struct
        {
            if (stream == null)
            {
                // Error: Invalid stream o read from
                return null;
            }
            
            int size = Marshal.SizeOf<T>();
            byte[] bytes = new byte[size];
            if (stream.Read(bytes, 0, size) != size)
            {
                // Error: Not enough bytes to read the structure
                return null;
            }

            // Convert the structure to .NET
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
