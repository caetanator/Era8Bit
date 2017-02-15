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
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaetanoSof.Utils.EndianUtils
{
    /// <summary>
    /// Big and little endian utilsfor helpig with the hardware architecture/CPU.
    /// 
    /// Endianism refers to the way in which data is stored, and defines 
    /// how bytes are addressed in integral and floating point data types.
    /// 
    /// Little-endian means that the least significant byte is stored at 
    /// the lowest memory address and the most significant byte is stored 
    /// at the highest memory address.
    /// This is also know as the "Intel convention".
    /// Used in Intel 8086, Intel IA32, DEC Alpha and by Zylog Z80.
    ///
    /// Big-endian means that the most significant byte is stored at the 
    /// lowest memory address and the least significant byte is stored 
    /// at the highest memory address.
    /// This is also know as the "Motorola convention" and the "Network order".
    /// Used in Motorola 6800, Motorola 68000, SUN SPARK and by IBM PowerPC.
    /// </summary>
    public static class EndianUtils
    {
        /// <summary>
        /// Checks if the platform is Big-Endian.
        /// </summary>
        /// <returns>True if it is, false otherwise.</returns>
        public static bool IsBigEndian
        {
            get
            {
                return !(BitConverter.IsLittleEndian);
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
                return BitConverter.IsLittleEndian;
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

        public static ushort ConvertWord16_BE(ushort word16)
        {
            if(EndianUtils.IsBigEndian)
            {
                return word16;
            }
            
            byte[] buffer = BitConverter.GetBytes(word16);
            byte b0 = buffer[0];
            byte b1 = buffer[1];

            return (ushort)(((buffer[0] << 8) | buffer[1]) & 0xFFFF);
        }

        public static ushort ConvertWord16_LE(ushort word16)
        {
            if (EndianUtils.IsLittleEndian)
            {
                return word16;
            }

            byte[] buffer = BitConverter.GetBytes(word16);
            byte b0 = buffer[0];
            byte b1 = buffer[1];

            return (ushort)(((buffer[1] << 8) | buffer[0]) & 0xFFFF);
        }
    }
}
