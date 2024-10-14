/**
 * EndianUtils.cs
 *
 * PURPOSE
 *  Implements auxiliary methods to handle CPU endianness.
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
 *  2021-04-16: fixes and updates.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaetanoSoft.Utils.EndianUtils
{
    /// <summary>
    /// Big and little endian utils for helping with the hardware architecture/CPU.
    /// 
    /// Endianism refers to the way in which data is stored, and defines 
    /// how bytes are addressed in integral and floating point data types.
    /// 
    /// Little-endian means that the least significant byte is stored at 
    /// the lowest memory address and the most significant byte is stored 
    /// at the highest memory address (LSB/MSB).
    /// This is also know as the "Intel convention".
    /// Used in Intel 8086, Intel IA32, DEC Alpha and by Zylog Z80.
    ///
    /// Big-endian means that the most significant byte is stored at the 
    /// lowest memory address and the least significant byte is stored 
    /// at the highest memory address (MSB/LSB).
    /// This is also know as the "Motorola convention" and the "Network order".
    /// Used in Motorola 6800, Motorola 68000, SUN SPARK and by IBM PowerPC.
    /// </summary>
    public sealed class EndianUtils
    {
        private static readonly bool m_isBigEndian = ((unchecked(((byte)0x12345678)) == 0x12) ? true : false);
        private static readonly bool m_isLittleEndian = ((unchecked(((byte)0x12345678)) == 0x78) ? true : false);

        private static readonly Lazy<EndianUtils> m_lazy = new Lazy<EndianUtils>(() => new EndianUtils());

        public static EndianUtils Instance { get { return m_lazy.Value; } }

        private EndianUtils()
        {
            // Singleton pattern objects doesn't have public constructors
        }

        /// <summary>
        /// Checks if the platform is Big-Endian.
        /// </summary>
        /// <returns>True if it is, false otherwise.</returns>
        public static bool IsBigEndian
        {
            get
            {
                return m_isBigEndian;
                /*
                int i = 0x12345678;
                if ((byte)i == 0x12)
                {
                    return true;
                }

                return false;
                 */
            }

            private set { }
        }

        /// <summary>
        /// Checks if the platform is Little-Endian.
        /// </summary>
        /// <returns>True if it is, false otherwise.</returns>
        public static bool IsLittleEndian
        {
            get
            {
                return m_isLittleEndian;
                /*
                int i = 0x12345678;
                if ((byte)i == 0x78)
                {
                    return true;
                }

                return false;
                 */
            }

            private set { }
        }

        /// <summary>Converts, if necessary, an 16-bit unsigned integer to big-endian.</summary>
        /// <param name="word16">The 16-bit unsigned integer to be converted to big-endian.</param>
        /// <returns>
        ///   The 16-bit unsigned integer to be converted to big-endian.
        /// </returns>
        public static ushort ConvertUInt16_BE(ushort word16)
        {
            if(EndianUtils.IsBigEndian)
            {
                return word16;
            }

            // Convert 16-bit unsigned integer to array
            byte[] buffer = new byte[2];
            buffer[1] = (byte)(word16 >> 8);
            buffer[0] = (byte)(word16);

            return (ushort)(((buffer[0] << 8) | buffer[1]) & 0xFFFF);
        }

        /// <summary>Converts, if necessary, an 16-bit unsigned integer to little-endian.</summary>
        /// <param name="word16">The 16-bit unsigned integer to be converted to little-endian.</param>
        /// <returns>
        ///   The 16-bit unsigned integer to be converted to little-endian.
        /// </returns>
        public static ushort ConvertUInt16_LE(ushort word16)
        {
            if (EndianUtils.IsLittleEndian)
            {
                return word16;
            }

            // Convert 16-bit unsigned integer to array
            byte[] buffer = new byte[2];
            buffer[1] = (byte)(word16 >> 8);
            buffer[0] = (byte)(word16);

            return (ushort)(((buffer[0] << 8) | buffer[1]) & 0xFFFF);
        }

        /// <summary>Converts, if necessary, an 16-bit signed integer to big-endian.</summary>
        /// <param name="int16">The 16-bit signed integer to be converted to big-endian.</param>
        /// <returns>
        ///   The 16-bit signed integer to be converted to big-endian.
        /// </returns>
        public static short ConvertSInt16_BE(short int16)
        {
            if (EndianUtils.IsBigEndian)
            {
                return int16;
            }

            // Convert 16-bit unsigned integer to array
            byte[] buffer = new byte[2];
            buffer[1] = (byte)(int16 >> 8);
            buffer[0] = (byte)(int16);

            return (short)(((buffer[0] << 8) | buffer[1]) & 0xFFFF);
        }

        /// <summary>Converts, if necessary, an 16-bit signed integer to little-endian.</summary>
        /// <param name="int16">The 16-bit signed integer to be converted to little-endian.</param>
        /// <returns>
        ///   The 16-bit signed integer to be converted to little-endian.
        /// </returns>
        public static short ConvertSInt16_LE(short int16)
        {
            if (EndianUtils.IsLittleEndian)
            {
                return int16;
            }

            // Convert 16-bit unsigned integer to array
            byte[] buffer = new byte[2];
            buffer[1] = (byte)(int16 >> 8);
            buffer[0] = (byte)(int16);

            return (short)(((buffer[0] << 8) | buffer[1]) & 0xFFFF);
        }
    }
}
