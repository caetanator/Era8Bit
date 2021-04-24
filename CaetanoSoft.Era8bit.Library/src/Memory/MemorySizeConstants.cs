/**
 * MemorySizeConstants.cs
 *
 * PURPOSE
 *  This is a library to read and write the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *  This class defines global constants to be used by this library and all programs that depends on it.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSof.Era8Bit.Library8Bit.MediaFormats" project:
 *      José Caetano Silva, jcaetano@users.sourceforge.net
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2016-2017 José Caetano Silva
 *
 * HISTORY
 *  2016-12-20: Created.
 *  2021-04-23: Major re-wright.
 */

using System;

namespace CaetanoSoft.Era8bit.Memory
{
    /// <summary>
    /// This class implements a Singleton Pattern object, that contains constants to help the calculation of 
    /// different memory sizes in bytes.
    /// </summary>
    public sealed class MemorySizeConstants
    {
        /// <summary>The size in bytes for 1 KB (1024 bytes).</summary>
        public const int KB1 = 1024;

        /// <summary>The size in bytes for 4 KB.</summary>
        public const int KB4 = 4 * KB1;

        /// <summary>The size in bytes for 8 KB.</summary>
        public const int KB8 = 8 * KB1;

        /// <summary>The size in bytes for 16 KB.</summary>
        public const int KB16 = 16 * KB1;

        /// <summary>The size in bytes for 24 KB.</summary>
        public const int KB24 = 24 * KB1;

        /// <summary>The size in bytes for 32 KB.</summary>
        public const int KB32 = 32 * KB1;

        /// <summary>The size in bytes for 64 KB.</summary>
        public const int KB64 = 64 * KB1;

        /// <summary>The size in bytes for 128 KB.</summary>
        public const int KB128 = 128 * KB1;

        /// <summary>The size in bytes for 256 KB.</summary>
        public const int KB256 = 256 * KB1;

        /// <summary>The size in bytes for 512 KB.</summary>
        public const int KB512 = 512 * KB1;

        /// <summary>The size in bytes for 1 MB (1024 KB).</summary>
        public const int MB1 = MemorySizeConstants.KB1 * 1024;

        /// <summary>The size in bytes for 1 MB (1024 KB).</summary>
        public const int MB4 = MemorySizeConstants.MB1 * 4;

        /// <summary>The size in bytes for 1 TB (1024 MB).</summary>
        public const int TB1 = MemorySizeConstants.MB1 * 1024;

        /// <summary>
        /// Stores the unique instance of this Singleton Pattern object. The object instance is created lazy 
        /// (only on first instantiation).
        /// </summary>
        private static readonly Lazy<MemorySizeConstants> m_lazyInstance = new Lazy<MemorySizeConstants>(() => new MemorySizeConstants());

        /// <summary>Gets the unique instance of the this Singleton Pattern object.</summary>
        /// <value>The unique instance.</value>
        public static MemorySizeConstants Instance { get { return m_lazyInstance.Value; } }

        /// <summary>
        /// Prevents a default instance of the <see cref="MemorySizeConstants" /> class from being created.
        /// <para>This is a Singleton Pattern object, so can't be instantiated more than one time.</para>
        /// </summary>
        private MemorySizeConstants()
        {
            // Singleton pattern objects doesn't have public constructors
        }
    }
}
