/*
                                     .DCK (Warajevo)
                                     ===============

DCK files keeps information about memory content of various Timex memory expansions, and information which 
chunks of extra memory are RAM chunks and which chunks are ROM chunks. Such files have relatively simple 
format. At the beginning of a DCK file, a nine-byte header is located. First byte is the bank ID which has 
the following meaning:

        0: DOCK bank (the most frequent variant)

  1 - 253: Reserved for expansions which allow more than three 64 KB
  
           * banks(currently not implemented) *
  
      254: EXROM bank (using this ID you may insert RAM or ROM chunks
           into EXROM bank; such hardware exists for the real TS2068)
      255: HOME bank(mainly useless, HOME content is typically stored in a Z80
           file); however, using this bank ID you may replace content of Timex
           HOME ROM, or turn Timex HOME ROM into RAM
This numbering of banks is in according to convention used in various routines from the TS2068 ROM.

After the first byte, following eight bytes corresponds to eight 8K chunks in the bank.
Organization of each byte is as follows:

    bit D0: 0 = read - only chunk, 1 = read / write chunk
    bit D1: 0 = memory image for corresponding chunk is not present in DCK file, 
            1 = memory image is present in DCK file
    bits D2 - D7: reserved (all zeros)

To be more precise, these bytes will have the following values:

   0, for non - existent chunks(reading from such chunks must return default values for given bank; for 
      example, #FF in DOCK bank, and ghost images of 8K Timex EXROM in EXROM bank).
   1, for RAM chunks, where initial RAM content is not given(in Warajevo, such chunks will be initially 
      filled with zeros)
   2, for ROM chunks
   3, for RAM chunks where initial RAM content is given(this is need to allow saving content of expanded 
      RAM; also this is useful for emulating non-volatile battery-protected RAM expansions).

After the header, a pure binary image of each chunk is stored in DCK file. That's all if only one bank is stored 
in DCK file. Else, after the memory image, a new 9-byte header for next bank follows, and so on.
 */


using System.Runtime.InteropServices;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// DCK file format is a simple little-endian structure that keeps information about memory content of various 
    /// Timex memory expansions, and information which chunks of extra memory are RAM chunks and which chunks are ROM chunks.
    /// <para>See <see href="https://rk.nvg.ntnu.no/sinclair/faq/fileform.html#DCK">Spectrum FAQ - File Formats</see> or 
    /// <see href="https://timex.comboios.info/tscart.html">Timex Computer World - Timex Command Cartridges</see> for more information.</para>
    /// <para><c>
    /// +---------------+-------------------------------------------------------------------+<br />
    /// |   Location    |                           Description                             |<br />
    /// +---------------+-------------------------------------------------------------------+<br />
    /// |               | Memory Bank ID:                                                   |<br />
    /// |       0       |     0     = DOCK bank (TCC ROM and/or RAM up to 64 KB)            |<br />
    /// |     (00H)     |   1 - 253 = Reserved banks (BEU RAM memory expansions of 64 KB)   |<br />
    /// |               |    254    = EXROM bank (Timex 2068 8 KB Extension ROM)            |<br />
    /// |               |    255    = HOME bank (Timex 2068 16 KB Home ROM + 48 KB RAM)     |<br />
    /// +---------------+-------------------------------------------------------------------+<br />
    /// |               | Memory Bank Chunk Type (8 bytes: 1 for each 8K chunk):            |<br />
    /// |      1-8      |   bit D0: 0=>ROM chunk; 1=>RAM chunk                              |<br />
    /// |   (01H-08H)   |   bit D1: 0=>Chunk present on file; 1=>Chunk not present on file  |<br />
    /// |               |   bits D2 - D7: Reserved (all zeros)                              |<br />
    /// +---------------+-------------------------------------------------------------------+<br />
    /// |               |                                                                   |<br />
    /// |      ...      | 8 KB memory chunks present on the file for this bank (see above)  |<br />
    /// |               |                                                                   |<br />
    /// +---------------+-------------------------------------------------------------------+<br />
    /// +---------------+-------------------------------------------------------------------+<br />
    /// |                   * Repeat the above for another bank if necessary *              |<br />
    /// +---------------+-------------------------------------------------------------------+<br />
    /// </c></para>
    /// </summary>
    /// <remarks>
    /// The 9 bytes little-endian structure starting at file location 0 (0000H), as the following representation:
    /// <para>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Location</term>
    ///         <description>Description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>0<br />(00H)<br /></term>
    ///         <description>
    ///			Memory Bank ID:<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>0</term>
    ///			        <description>DOCK bank (TCC ROM and/or RAM up to 64 KB)</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>1-253</term>
    ///			        <description>Reserved banks (BEU RAM memory expansions of 64 KB)</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>254</term>
    ///			        <description>EXROM bank (Timex 2068 8 KB Extension ROM)</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>255</term>
    ///			        <description>HOME bank (Timex 2068 16 KB Home ROM + 48 KB RAM)</description>
    ///			    </item>
    ///			</list>
    ///			</description>
    ///     </item>
    ///     <item>
    ///         <term>1-8<br />(01H-08H)<br /></term>
    ///         <description>
    ///			Memory Bank Chunk Type (8 bytes: 1 for each 8K chunk):<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>bit D0</term>
    ///			        <description>0=>ROM chunk; 1=>RAM chunk</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>bit D1</term>
    ///			        <description>0=>Chunk present on file; 1=>Chunk not present on file</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>bits D2 - D7</term>
    ///			        <description>Reserved (all zeros)</description>
    ///			    </item>
    ///			</list>
    ///			</description>
    ///     </item>
    ///     <item>
    ///			<term>...</term>
    ///			<description>8 KB memory chunks present on the file for this bank (see above)</description>
    ///	    </item>
    ///	    <item>
    ///			<term> </term>
    ///			<description> </description>
    ///	    </item>
    ///	    <item>
    ///			<term></term>
    ///			<description>* Repeat the above for another bank if necessary *</description>
    ///	    </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <copyright>(c) 2016-2021 by José Caetano Silva</copyright>
    /// <license type="GPL-3">See LICENSE for full terms</license>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 9)]
    public struct DckHeaderStruct
    {
        /// <summary>The memory bank identifier</summary>
        public byte MemoryBankID;

        /// <summary>The memory bank chunk type</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] MemoryBankChunkType;
    }
}
