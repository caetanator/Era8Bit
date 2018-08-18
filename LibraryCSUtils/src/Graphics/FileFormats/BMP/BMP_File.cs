/**
 * BMP_File.cs
 *
 * PURPOSE
 *  Implements the MS Windows and IBM/OS2 BMP file format.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "Ray Tracer" project:
 *      caetanator@hotmail.com
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C) 2006-2017   José Caetano Silva
 *
 * HISTORY
 *  2006-02-01: Created.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace CaetanoSof.Graphics.FileFormats.BMP
{
    /// <summary>
    /// The compression method used on the IBM OS/2 BMP image.
    /// <para>See: http://www.fileformat.info/format/os2bmp/egff.htm </para>
    /// </summary>
    public enum OS2_BitmapCompressionType : uint
    {
        /// <summary>
        /// The image is uncompressed, the pixels are in plain RGB/RGBA.
        /// <para>Needs OS/2 1.0 or above.</para>
        /// <para>From OS/2 BMP version 1 and above.</para>
        /// </summary>
        None = 0,
        /// <summary>
        /// Run-length encoded (RLE) 8-bit/pixel. Only for 8 bpp bitmaps.
        /// <para>Needs OS/2 2.0 or above.</para>
        /// <para>From OS/2 BMP version 2 and above.</para>
        /// </summary>
        RLE_8 = 1,
        /// <summary>
        /// Run-length encoded (RLE) 4-bit/pixel. Only for 4 bpp bitmaps.
        /// <para>Needs OS/2 2.0 or above.</para>
        /// <para>From OS/2 BMP version 2 and above.</para>
        /// </summary>
        RLE_4 = 2,
        /// <summary>
        /// Huffman 1D algorithm for monochrome bitmaps with 1 bpp.
        /// <para>Needs OS/2 2.0 or above.</para>
        /// <para>From OS/2 BMP version 2 and above.</para>
        /// </summary>
        Huffman_1D = 3,
        /// <summary>
        /// Run-length encoded (RLE) 24-bit/pixel. Only for 24 bpp bitmaps.
        /// <para>Needs OS/2 2.0 or above.</para>
        /// <para>From OS/2 BMP version 2 and above.</para>
        /// </summary>
        RLE_24 = 4
    }

    /// <summary>
    /// The compression method used on the Microsoft Windows BMP image.
    /// <para>See: https://en.wikipedia.org/wiki/BMP_file_format </para>
    /// </summary>
    public enum BitmapCompressionType : uint
    {
        /// <summary>
        /// Uncompressed format. The pixels are in plain RGB/RGBA.
        /// <para>Needs Windows 2.0 or above.</para>
        /// <para>From Windows BMP version 2 and above.</para>
        /// </summary>
        None = 0,
        /// <summary>
        /// Run-length encoded (RLE) 8-bit/pixel. Only for 8 bpp bitmaps.
        /// <para>Needs Windows 2.0 or above.</para>
        /// <para>From Windows BMP version 2 and above.</para>
        /// </summary>
        RLE_8 = 1,
        /// <summary>
        /// Run-length encoded (RLE) 4-bit/pixel. Only for 4 bpp bitmaps.
        /// <para>Needs Windows 2.0 or above.</para>
        /// <para>From Windows BMP version 2 and above.</para>
        /// </summary>
        RLE_4 = 2,
        /// <summary>
        /// Uncompressed format that the color table consists of three DWORD color masks
        /// that specify the red, green, and blue components, respectively, of each pixel. 
        /// <para>This is valid when used with 16 and 32 bpp bitmaps.</para>
        /// <para>Needs Windows 95/Windows NT 4 or above.</para>
        /// <para>From Windows BMP version 3 and above.</para>
        /// </summary>
        BitFields = 3,
        /// <summary>
        /// The bitmap contains a JPG image.
        /// <para>Needs Windows 98/Windows 2000 or above.</para>
        /// <para>From Windows BMP version 4 and above.</para>
        /// </summary>
        JPEG = 4,
        /// <summary>
        /// The bitmap contains a PNG image. 
        /// <para>Needs Windows 98/Windows 2000 or above.</para>
        /// <para>From Windows BMP version 4 and above.</para>
        /// </summary>
        PNG = 5,
        /// <summary>
        /// Uncompressed format that the color table consists of four DWORD color masks
        /// that specify the red, green, blue, and alpha components, respectively, of each pixel. 
        /// <para>This is valid when used with 16 and 32 bpp bitmaps on Windows CE only.</para>
        /// <para>Needs Windows CE .NET 4.0 or later.</para>
        /// <para>From Windows BMP version 3 and above.</para>
        /// </summary>
        /// <see cref="https://msdn.microsoft.com/en-us/library/aa452885.aspx"/>
        Alpha_BitFields = 6,
        /// <summary>
        /// Uncompressed format.
        /// <para>Windows Metafile CMYK only.</para>
        /// </summary>
        CMYK_None = 11,
        /// <summary>
        /// Run-length encoded (RLE) 8-bit/pixel. Only for 8 bpp bitmaps.
        /// <para>Windows Metafile CMYK only.</para>
        /// </summary>
        CMYK_RLE_8 = 12,
        /// <summary>
        /// Run-length encoded (RLE) 4-bit/pixel. Only for 4 bpp bitmaps.
        /// <para>Windows Metafile CMYK only.</para>
        /// </summary>
        CMYK_RLE_4 = 13,

        /// <summary>
        /// For Windows Mobile version 5.0 and later, you can OR any of the values BI_RGB, BI_BITFIELDS and BI_ALPHABITFIELDS with
        /// BI_SRCPREROTATE to specify that the source DIB section has the same rotation angle as the destination.
        /// Otherwise, the image can be rotated 90 degrees anti-clockwise (Landscape/Portrait).
        /// https://msdn.microsoft.com/en-us/library/aa452495.aspx
        /// </summary>
        BI_SRCPREROTATE = 0x8000
    }

    /// <summary>
    /// The number of bits-per-pixel (bpp) used in the Microsoft Windows BMP image.
    /// </summary>
    public enum BitmapBitsPerPixel : ushort
    {
        /// <summary>
        /// The number of bits-per-pixel is specified or is implied by the JPEG or PNG format.
        /// <para>Needs Windows 98/Windows 2000 or above.</para>
        /// <para>From Windows BMP version 4 and above.</para>
        /// </summary>
        JPEG_PNG = 0,
        /// <summary>
        /// The bitmap is monochrome, and the palette contains 2 entries (2 colors palette).
        /// <para>Each bit in the bitmap array represents a pixel.
        /// If the bit is clear, the pixel is displayed with the color of the first entry in the palette table;
        /// if the bit is set, the pixel has the color of the second entry in the table.</para>
        /// <para>Needs Windows 2.0 or above.</para>
        /// <para>From Windows BMP version 2 and above.</para>
        /// </summary>
        MonoChrome = 1,
        /// <summary>
        /// The bitmap has a maximum of 16 colors, and the palette contains up to 16 entries (16 colors palette).
        /// <para>Each 4 bits in the bitmap array represents a pixel (the index for the palette).
        /// For example, if the first byte in the bitmap is 0x1F, the byte represents two pixels. 
        /// The first pixel contains the color in the second table entry, and the second pixel contains the 
        /// color in the sixteenth table entry.</para>
        /// <para>Needs Windows 2.0 or above.</para>
        /// <para>From Windows BMP version 2 and above.</para>
        /// </summary>
        Palette_16 = 4,
        /// <summary>
        /// The bitmap has a maximum of 256 colors, and the palette contains up to 256 entries (256 colors palette).
        /// <para>Each 8 bits (1 byte) in the bitmap array represents a pixel (the index for the palette).</para>
        /// <para>Needs Windows 2.0 or above.</para>
        /// <para>From Windows BMP version 2 and above.</para>
        /// </summary>
        Palette_256 = 8,
        /// <summary>
        /// The bitmap has a maximum of 2^16 colors. An optional 256 colors palette can be given, for optimizing the display on palette-based devices.
        /// <para>Each WORD (2 bytes) in the bitmap array represents a single pixel. The relative intensities of red, green, and blue are represented with 5 bits 
        /// for each color component.
        /// The value for blue is in the least significant 5 bits, followed by 5 bits each for green and red. The most significant bit is not used.</para>
        /// <para>Needs Windows 95/Windows NT 4 or above.</para>
        /// <para>From Windows BMP version 4 and above.</para>
        /// </summary>
        RGB_16 = 16,
        /// <summary>
        /// The bitmap has a maximum of 2^24 colors. An optional 256 colors palette can be given, for optimizing the display on palette-based devices.
        /// <para>Each pixel is made by 3 bytes that specifies the relative intensities of blue, green, and red color components respectively.</para>
        /// <para>Needs Windows 3.1/Windows NT 3.1 or above.</para>
        /// <para>From Windows BMP version 3 and above.</para>
        /// </summary>
        RGB_24 = 24,
        /// <summary>
        /// The bitmap has a maximum of 2^32 colors. An optional 256 colors palette can be given, for optimizing the display on palette-based devices.
        /// <para>Each DWORD (4 bytes) in the bitmap array represents a single pixel. 
        /// The relative intensities of red, green, and blue are represented with 8 bits for each color component.
        /// The value for blue is in the least significant 8 bits, followed by 8 bits each for green and red. The most significant byte is not used.</para>
        /// <para>Needs Windows 95/Windows NT 4 or above.</para>
        /// <para>From Windows BMP version 4 and above.</para>
        /// </summary>
        RGB_32 = 32
    }

    /// <summary>
    /// The color space used on the Microsoft Windows BMP image.
    /// </summary>
	public enum BitmapColorSpaceType : uint
    {

        /// <list type="bullet">
        /// <item>
        /// <term>Calibrated_RBG</term>
        /// <description></description>
        /// </item>
        /// <item>
        /// <term>sRGB</term>
        /// <description></description>
        /// </item>
        /// <item>
        /// <term>WindowsColorSpace</term>
        /// <description></description>
        /// </item>
        /// <item>
        /// <term>ProfileLinked</term>
        /// <description></description>
        /// </item>
        /// <item>
        /// <term>ProfileEmbedded</term>
        /// <description></description>
        /// </item>
        /// </list>
        /// 
        /// <summary>
        /// The endpoints and gamma values are given in the appropriate fields.
        /// </summary>
        Calibrated_RBG = 0,
        /// <summary>
        /// The bitmap is in sRGB color space.
        /// </summary>
		sRGB = 1,
        /// <summary>
        /// The bitmap is in the system default color space: sRGB.
        /// </summary>
		WindowsColorSpace = 2,
        /// <summary>
        /// This value indicates that <b>ProfileOffset</b> points to the ICC color space file name of the profile to use (gamma and endpoints values are ignored).
        /// </summary>
		ProfileLinked = 3,
        /// <summary>
        /// Indicates that <b>ProfileOffset</b> points to a memory buffer that contains the profile to be used (gamma and endpoints values are ignored).
        /// </summary>
        ProfileEmbedded = 4
    }
	
	/// <summary>
    /// This is the BMP v5 and above, rendering intent for bitmap..
    /// </summary>
    /// <list type="bullet">
    /// <item>
    /// <term>Business</term>
    /// <description>Graphic. Saturation Maintains. saturation.
	/// Used for business charts and other situations in which undithered colors are required.</description>
    /// </item>
	/// <item>
    /// <term>Graphics</term>
    /// <description>Proof. Relative Colorimetric. Maintains colorimetric match.
	/// Used for graphic designs and named colors.</description>
    /// </item>
	/// <item>
    /// <term>Images</term>
    /// <description>Picture. Perceptual. Maintains contrast.
	/// Used for photographs and natural images.</description>
    /// </item>
	/// <item>
    /// <term>ABS_ColoriMetric</term>
    /// <description>Match. Absolute Colorimetric. Maintains the white point.
	/// Matches the colors to their nearest color in the destination gamut.</description>
    /// </item>
	/// </list>
	public enum BitmapIntentV5 : int
    { 
        Business            = 1,	// Saturation
		Graphics            = 2,	// Relative
		Images              = 4,	// Perceptual
		ABS_ColoriMetric    = 8		// Absolute
    }

    /// <summary>
    /// This is the Windows BMP v2 and OS/2 BMP v1 palette entry.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(RGBTRIPLE) returns the size of 3 bytes and is byte aligned.
    /// </remarks>
    /// <see cref="https://msdn.microsoft.com/en-us/library/dd162939(v=vs.85).aspx"/>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
    public struct PaletteEntryV2
    {
        /// <summary>
        /// Specifies the intensity of blue in the color in the range 0 to 255.
        /// </summary>
        public byte Blue;
        /// <summary>
        /// Specifies the intensity of green in the color in the range 0 to 255.
        /// </summary>
        public byte Green;
        /// <summary>
        /// Specifies the intensity of red in the color in the range 0 to 255.
        /// </summary>
        public byte Red;
    }

    /// <summary>
    /// This is the Windows BMP v3 and OS/2 BMP v2 palette entry.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(RGBQUAD) returns the size of 4 bytes and is byte aligned.
    /// </remarks>
    /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/dd162939(v=vs.85).aspx"/>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct PaletteEntryV3
    {
        /// <summary>
        /// Specifies the intensity of blue in the color in the range 0 to 255.
        /// </summary>
        public byte Blue;
        /// <summary>
        /// Specifies the intensity of green in the color in the range 0 to 255.
        /// </summary>
        public byte Green;
        /// <summary>
        /// Specifies the intensity of red in the color in the range 0 to 255.
        /// </summary>
        public byte Red;
        /// <summary>
        /// Not used on BMP v3 (reserved and must be 0). Alpha color chanel on BMP v4 and above.
        /// </summary>
        public byte Alpha;
    }

    /// <summary>
    /// This is the BMP v2 (and above) bmpFile header.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(BITMAPFILEHEADER) returns 12 (WORD aligned, the real structure size)
    /// insted of 16 (DWORD aligned, the default for VC++ 32 bits).
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>BMP_MAGIC_ID</term>
    /// <description>Specifies the bmpFile type, must be "BM".</description>
    /// </item>
    /// <item>
    /// <term>FileSize</term>
    /// <description>Specifies the size, in bytes, of the bitmap bmpFile.</description>
    /// </item>
    /// <item>
    /// <term>Reserved1</term>
    /// <description>Reserved; must be zero.</description>
    /// </item>
    /// <item>
    /// <term>Reserved2</term>
    /// <description>Reserved; must be zero.</description>
    /// </item>
    /// <item>
    /// <term>PixelsOffset</term>
    /// <description>Specifies the offset, in bytes, from the beginning of the BITMAPFILEHEADER structure to the bitmap bits.</description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14)]
    public struct BitmapFileHeaderV2
    { 
	    public ushort	Magic; 
	    public uint	    FileSize;
	    public ushort	Reserved1; 
	    public ushort	Reserved2; 
	    public uint	    PixelsOffset; 
    }
    
	/// <summary>
    /// This is the BMP v4 (and above) CIE XYZ color correction.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(CIEXYZ) returns 12 (WORD aligned, the real structure size)
    /// insted of 16 (DWORD aligned, the default for VC++ 32 bits).
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>cX</term>
    /// <description>The x coordinate in fix point (2.30).</description>
    /// </item>
    /// <item>
    /// <term>Y</term>
    /// <description>The y coordinate in fix point (2.30).</description>
    /// </item>
	/// <item>
    /// <term>Z</term>
    /// <description>The z coordinate in fix point (2.30).</description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    public struct CIEXYZ_V4
    { 
	    public int X;
	    public int Y;
        public int Z;
    }
	
	/// <summary>
    /// This is the BMP v4 (and above) CIE XYZ RGB color correction.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(CIEXYZTRIPLE) returns 36 (DWORD aligned, the real structure size)
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>Red</term>
    /// <description>The xyz coordinates of red endpoint.</description>
    /// </item>
    /// <item>
    /// <term>Green</term>
    /// <description>The xyz coordinates of green endpoint.</description>
    /// </item>
	/// <item>
    /// <term>Blue</term>
    /// <description>The xyz coordinates of blue endpoint.</description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 36)]
    public struct CIEXYZ_TripleV4
    { 
	    public CIEXYZ_V4 Red; 
	    public CIEXYZ_V4 Green;
	    public CIEXYZ_V4 Blue; 
    }

    /// <summary>
    /// This is the Windows 2.x BMP v2 and OS/2 1.x BMP v1 header.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(BITMAPCOREHEADER) returns 12 (WORD aligned, the real structure size)
    /// insted of 16 (DWORD aligned, the default for VC++ 32 bits).
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>HeaderSize</term>
    /// <description>Specifies the size, in bytes, of the bitmap header v2 struct (12 bytes).
	/// Applications should use this member to determine which bitmap information header structure is being used.</description>
    /// </item>
    /// <item>
    /// <term>Width</term>
    /// <description>Specifies the width of the bitmap in pixels.</description>
    /// </item>
    /// <item>
    /// <term>Height</term>
    /// <description>Specifies the height of the bitmap in pixels.</description>
    /// </item>
    /// <item>
    /// <term>Planes</term>
    /// <description>Specifies the number of planes for the target device. This value must be 1.</description>
    /// </item>
    /// <item>
    /// <term>BitsPerPixel</term>
    /// <description>Specifies the number of bits per pixel. This value must be 1, 4, 8 or 24. <see>BitmapCompressionType</see></description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    public struct BitmapHeaderV2
    {
        public uint     HeaderSize;
        public ushort   Width;
        public ushort   Height;
        public ushort   Planes;
        public ushort   BitsPerPixel;
    }

    /// <summary>
    /// This is the Windows 3.x and Windows NT 3.xx BMP v3 header.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(BITMAPINFOHEADER) returns 40 (DWORD aligned, the default for VC++ 32 bits).
	/// Each scan line must be zero-padded to end on a DWORD (4 bytes) boundary.
	/// The colors in the palette table should appear in order of importance.
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>HeaderSize</term>
    /// <description>Specifies the size, in bytes, of the bitmap header v3 struct (40 bytes).
	/// Applications should use this member to determine which bitmap information header structure is being used.</description>
    /// </item>
    /// <item>
    /// <term>Width</term>
    /// <description>Specifies the width of the bitmap in pixels.
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the Width member specifies the width of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Height</term>
    /// <description>Specifies the height of the bitmap in pixels.
    /// If <b>Height</b> is positive, the bitmap is a bottom-up DIB and its origin is the lower-left corner.
    /// If <b>Height</b> is negative, the bitmap is a top-down DIB and its origin is the upper-left corner.
    /// Top-down DIBs cannot be compressed: <b>Compression</b> must be either <b>None</b> or <b>BitFields</b>.
    /// </description>
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the Width member specifies the height of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Planes</term>
    /// <description>Specifies the number of planes for the target device. This value must be 1.</description>
    /// </item>
    /// <item>
    /// <term>BitsPerPixel</term>
    /// <description>Specifies the number of bits per pixel. This value must be 0, 1, 4, 8, 16, 24 or 32. <see>BitmapCompression</see></description>
    /// </item>
	/// <item>
    /// <term>Compression</term>
    /// <description>Specifies the type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).  <see>BitmapCompressionType</see></description>
    /// </item>
	/// <item>
    /// <term>ImageSize</term>
    /// <description>Specifies the size, in bytes, of the image. This may be set to zero for <b>RGB</b> bitmaps.
	///
	/// If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>, biSizeImage indicates the size of the JPEG or PNG image buffer, respectively.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterX</term>
    /// <description>Specifies the horizontal resolution, in pixels-per-meter, of the target device for the bitmap. 
	/// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current device.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterY</term>
    /// <description>Specifies the vertical resolution, in pixels-per-meter, of the target device for the bitmap.</description>
    /// </item>
	/// <item>
    /// <term>PaletteColors</term>
    /// <description>Specifies the number of color indexes in the color palette that are actually used by the bitmap. 
	/// If this value is 0, the bitmap uses the maximum number of colors corresponding to the value of the <b>BitsPerPixel</b> member for the compression mode specified by <b>Compression</b>.
	/// If is nonzero and the <b>BitsPerPixel</b> member is less than 16, the <b>PaletteColors</b> member specifies the actual number of colors the graphics engine or device driver accesses.
	/// If <b>BitsPerPixel</b> is 16 or greater, the <b>PaletteColors</b> member specifies the size of the color table used to optimize performance of the system color palettes. 
	/// If <b>BitsPerPixel</b> equals 16 or 32, the optimal color palette starts immediately following the three DWORD masks. 
	/// When the bitmap array immediately follows the BITMAPINFO structure, it is a packed bitmap. 
	/// Packed bitmaps are referenced by a single pointer. 
	/// Packed bitmaps require that the <b>PaletteColors</b> member must be either zero or the actual size of the color table.</description>
    /// </item>
	/// <item>
    /// <term>PaletteImportant</term>
    /// <description>Specifies the number of color indexes that are required for displaying the bitmap. 
	/// If this value is 0, all colors are required.</description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 40)]
    public struct BitmapHeaderV3
    {
        public uint     HeaderSize;
        public int      Width;
        public int      Height;
        public ushort   Planes;
        public ushort   BitsPerPixel;
        public uint     Compression;
        public uint     ImageSize;
        public int      PixelsPerMeterX;
        public int      PixelsPerMeterY;
        public uint     PaletteColors;
        public uint     PaletteImportant;
    }

    /// <summary>
    /// This is the OS/2 2.x BMP v2 header.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(OS22XBITMAPHEADER) returns 64 (DWORD aligned, the default for VC++ 32 bits).
	/// Each scan line must be zero-padded to end on a DWORD (4 bytes) boundary.
	/// The colors in the palette table should appear in order of importance.
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>HeaderSize</term>
    /// <description>Specifies the size, in bytes, of the OS/2 bitmap header v2 struct (must be betew 16 and 64 bytes).
    /// Check the size to read only the fields sp
	/// Applications should use this member to determine which bitmap information header structure is being used.</description>
    /// </item>
    /// <item>
    /// <term>Width</term>
    /// <description>Specifies the width of the bitmap in pixels.
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the Width member specifies the width of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Height</term>
    /// <description>Specifies the height of the bitmap in pixels.
    /// If <b>Height</b> is positive, the bitmap is a bottom-up DIB and its origin is the lower-left corner.
    /// If <b>Height</b> is negative, the bitmap is a top-down DIB and its origin is the upper-left corner.
    /// Top-down DIBs cannot be compressed: <b>Compression</b> must be either <b>None</b> or <b>BitFields</b>.
    /// </description>
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the Width member specifies the height of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Planes</term>
    /// <description>Specifies the number of planes for the target device. This value must be 1.</description>
    /// </item>
    /// <item>
    /// <term>BitsPerPixel</term>
    /// <description>Specifies the number of bits per pixel. This value must be 0, 1, 4, 8, 16, 24 or 32. <see>BitmapCompression</see></description>
    /// </item>
	/// <item>
    /// <term>Compression</term>
    /// <description>Specifies the type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed). 
    /// 0 indicates that the data is uncompressed; 
    /// 1 indicates that the 8-bit RLE algorithm was used; 
    /// 2 indicates that the 4-bit RLE algorithm was used; 
    /// 3 indicates that the Huffman 1D algorithm was used; 
    /// and 4 indicates that the 24-bit RLE algorithm was used.</description>
    /// </item>
	/// <item>
    /// <term>ImageSize</term>
    /// <description>Specifies the size, in bytes, of the image. This may be set to zero for <b>RGB</b> bitmaps.
	/// If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>, <b>ImageSize</b> indicates the size of the JPEG or PNG image buffer, respectively.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterX</term>
    /// <description>Specifies the horizontal resolution, in pixels-per-meter, of the target device for the bitmap. 
	/// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current device.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterY</term>
    /// <description>Specifies the vertical resolution, in pixels-per-meter, of the target device for the bitmap.</description>
    /// </item>
	/// <item>
    /// <term>PaletteColors</term>
    /// <description>Specifies the number of color indexes in the color palette that are actually used by the bitmap. 
	/// If this value is 0, the bitmap uses the maximum number of colors corresponding to the value of the <b>BitsPerPixel</b> member for the compression mode specified by <b>Compression</b>.
	/// If is nonzero and the <b>BitsPerPixel</b> member is less than 16, the <b>PaletteColors</b> member specifies the actual number of colors the graphics engine or device driver accesses.
	/// If <b>BitsPerPixel</b> is 16 or greater, the <b>PaletteColors</b> member specifies the size of the color table used to optimize performance of the system color palettes. 
	/// If <b>BitsPerPixel</b> equals 16 or 32, the optimal color palette starts immediately following the three DWORD masks. 
	/// When the bitmap array immediately follows the BITMAPINFO structure, it is a packed bitmap. 
	/// Packed bitmaps are referenced by a single pointer. 
	/// Packed bitmaps require that the <b>PaletteColors</b> member must be either zero or the actual size of the color table.</description>
    /// </item>
	/// <item>
    /// <term>PaletteImportant</term>
    /// <description>Specifies the number of color indexes that are required for displaying the bitmap. 
	/// If this value is 0, all colors are required.</description>
    /// </item>
    /// <item>
    /// <term>Units</term>
    /// <description>Indicates the type of units used to interpret the values of the XResolution and YResolution fields. 
    /// The only valid value is 0, indicating pixels per meter.</description>
    /// </item>
    /// <item>
    /// <term>Reserved</term>
    /// <description>Unused and is always set to a value of zero. 
    /// Pad structure to 4-byte boundary.</description>
    /// </item>
    /// <item>
    /// <term>Recording</term>
    /// <description>Specifies how the bitmap scan lines are stored. 
    /// The only valid value for this field is 0, 
    /// indicating that the bitmap is stored from left to right and from the bottom up, 
    /// with the origin being in the lower-left corner of the display.</description>
    /// </item>
    /// <item>
    /// <term>Rendering</term>
    /// <description>Specifies the halftoning algorithm used on the bitmap data. 
    /// A value of 0 indicates that no halftoning algorithm was used; 
    /// 1 indicates error diffusion halftoning; 
    /// 2 indicates Processing Algorithm for Noncoded Document Acquisition (PANDA); 
    /// and 3 indicates super-circle halftoning.</description>
    /// </item>
    /// <item>
    /// <term>Size1</term>
    /// <description><b>Size1</b> and <b>Size2</b> are reserved fields used only by the halftoning algorithm. 
    /// If error diffusion halftoning is used, Size1 is the error damping as a percentage in the range 0 through 100. 
    /// A value of 100 percent indicates no damping, and a value of 0 percent indicates that any errors are not diffused. 
    /// Size2 is not used by error diffusion halftoning. 
    /// If PANDA or super-circle halftoning is specified, <b>Size1</b> is the X dimension and <b>Size2</b> 
    /// is the Y dimension of the pattern used in pixels.</description>
    /// </item>
    /// <item>
    /// <term>Size2</term>
    /// <description>See <b>Size1</b>.</description>
    /// </item>
    /// <item>
    /// <term>ColorEncoding</term>
    /// <description>Color model used to describe the bitmap data. 
    /// The only valid value is 0, indicating the None encoding scheme.</description>
    /// </item>
    /// <item>
    /// <term>Identifier</term>
    /// <description>Reserved for application use and may 
    /// contain an application-specific value.</description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
    public struct OS2_BitmapHeaderV2
    {
        public uint     HeaderSize;
        public int      Width;
        public int      Height;
        public ushort   Planes;
        public ushort   BitsPerPixel;
        public uint     Compression;
        public uint     ImageSize;
        public int      PixelsPerMeterX;
        public int      PixelsPerMeterY;
        public uint     PaletteColors;
        public uint     PaletteImportant;
        public ushort   Units;
        public ushort   Reserved;
        public ushort   Recording;
        public ushort   Rendering;
        public uint     Size1;
        public uint     Size2;
        public uint     ColorEncoding;
        public uint     Identifier;
    }
	
	/// <summary>
    /// This is the BMP v4 header.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(BITMAPV4HEADER) returns 108 (DWORD aligned, the default for VC++ 32 bits).
	/// Each scan line must be zero-padded to end on a DWORD (4 bytes) boundary.
	/// The colors in the palette table should appear in order of importance.
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>HeaderSize</term>
    /// <description>Specifies the size, in bytes, of the bitmap header v4 struct (108 bytes).
	/// Applications should use this member to determine which bitmap information header structure is being used.</description>
    /// </item>
    /// <item>
    /// <term>Width</term>
    /// <description>Specifies the width of the bitmap in pixels.
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the <b>Width</b> member specifies the width of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Height</term>
    /// <description>Specifies the height of the bitmap in pixels.
    /// If <b>Height</b> is positive, the bitmap is a bottom-up DIB and its origin is the lower-left corner.
    /// If <b>Height</b> is negative, the bitmap is a top-down DIB and its origin is the upper-left corner.
    /// Top-down DIBs cannot be compressed: <b>Compression</b> must be either <b>None</b> or <b>BitFields</b>.
    /// </description>
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the <b>Height</b> member specifies the height of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Planes</term>
    /// <description>Specifies the number of planes for the target device. This value must be 1.</description>
    /// </item>
    /// <item>
    /// <term>BitsPerPixel</term>
    /// <description>Specifies the number of bits per pixel. This value must be 0, 1, 4, 8, 16, 24 or 32. <see>BitmapCompression</see></description>
    /// </item>
	/// <item>
    /// <term>Compression</term>
    /// <description>Specifies the type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).  <see>BitmapCompressionType</see></description>
    /// </item>
	/// <item>
    /// <term>ImageSize</term>
    /// <description>Specifies the size, in bytes, of the image. This may be set to zero for <b>RGB</b> bitmaps.
	///
	/// If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>, <b>ImageSize</b> indicates the size of the JPEG or PNG image buffer, respectively.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterX</term>
    /// <description>Specifies the horizontal resolution, in pixels-per-meter, of the target device for the bitmap. 
	/// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current device.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterY</term>
    /// <description>Specifies the vertical resolution, in pixels-per-meter, of the target device for the bitmap.</description>
    /// </item>
	/// <item>
    /// <term>PaletteColors</term>
    /// <description>Specifies the number of color indexes in the color palette that are actually used by the bitmap. 
	/// If this value is 0, the bitmap uses the maximum number of colors corresponding to the value of the <b>BitsPerPixel</b> member for the compression mode specified by <b>Compression</b>.
	/// If is nonzero and the <b>BitsPerPixel</b> member is less than 16, the <b>PaletteColors</b> member specifies the actual number of colors the graphics engine or device driver accesses.
	/// If <b>BitsPerPixel</b> is 16 or greater, the <b>PaletteColors</b> member specifies the number of colors in the palette table used to optimize performance of the system color palettes. 
	/// If <b>BitsPerPixel</b> equals 16 or 32, the optimal color palette starts immediately following the three DWORD masks. 
	/// When the bitmap array immediately follows the BITMAPINFO structure, it is a packed bitmap. 
	/// Packed bitmaps are referenced by a single pointer. 
	/// Packed bitmaps require that the <b>PaletteColors</b> member must be either zero or the actual size of the color table.</description>
    /// </item>
	/// <item>
    /// <term>PaletteImportant</term>
    /// <description>Specifies the number of color indexes that are required for displaying the bitmap. 
	/// If this value is 0, all colors are required.</description>
    /// </item>
	/// <item>
    /// <term>MaskRed</term>
    /// <description>Color mask that specifies the red component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>MaskGreen</term>
    /// <description>Color mask that specifies the green component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>MaskBlue</term>
    /// <description>Color mask that specifies the blue component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>MaskAlpha</term>
    /// <description>Color mask that specifies the alpha (transparencie) component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceType</term>
    /// <description>Specifies the color space of the DIB. <see>BitmapColorSpaceType</see></description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceEndPoints</term>
    /// <description>A structure that specifies the x, y and z coordinates of the three colors that correspond to the 
	/// red, green and blue endpoints for the logical color space associated with the bitmap. <see>CIEXYZ_TripleV4</see>.
    /// This member is ignored unless the <b>ColorSpaceType</b> member specifies <b>Calibrated_RBG</b>.
	/// <para><b>Note:>/byte1>  A color space is a model for representing color numerically in terms of three or more coordinates. 
	/// For example, the RGB color space represents colors in terms of the red, green and blue coordinates.</para></description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceGammaRed</term>
    /// <description>Toned response curve for red. 
    /// This member is ignored unless color values are calibrated RGB values and <b>ColorSpaceType</b> is set to <b>Calibrated_RBG</b>.
    /// Specified in 16^16 format.</description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceGammaGreen</term>
    /// <description>Toned response curve for green. 
    /// This member is ignored unless color values are calibrated RGB values and <b>ColorSpaceType</b> is set to <b>Calibrated_RBG</b>.
    /// Specified in 16^16 format.</description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceGammaBlue</term>
    /// <description>Toned response curve for blue. 
    /// This member is ignored unless color values are calibrated RGB values and <b>ColorSpaceType</b> is set to <b>Calibrated_RBG</b>.
    /// Specified in 16^16 format.</description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 108)]
    public struct BitmapHeaderV4
    {
        public uint             HeaderSize;
        public int              Width;
        public int              Height;
        public ushort           Planes;
        public ushort           BitsPerPixel;
        public uint             Compression;
        public uint             ImageSize;
        public int              PixelsPerMeterX;
        public int              PixelsPerMeterY;
        public uint             PaletteColors;
        public uint             PaletteImportant;
        public uint             MaskRed;
        public uint             MaskGreen;
        public uint             MaskBlue;
        public uint             MaskAlpha;
        public uint             ColorSpaceType;
        public CIEXYZ_TripleV4  ColorSpaceEndPoints;
        public uint             ColorSpaceGammaRed;
        public uint             ColorSpaceGammaGreen;
        public uint             ColorSpaceGammaBlue;
    }
	
	/// <summary>
    /// This is the BMP v5 header.
    /// </summary>
    /// <remarks>
    /// Make shore that sizeof(BITMAPV5HEADER) returns 124 (DWORD aligned, the default for VC++ 32 bits).
	/// Each scan line must be zero-padded to end on a DWORD (4 bytes) boundary.
	/// The colors in the palette table should appear in order of importance.
    /// </remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>HeaderSize</term>
    /// <description>Specifies the size, in bytes, of the bitmap header v5 struct (124 bytes).
	/// Applications should use this member to determine which bitmap information header structure is being used.</description>
    /// </item>
    /// <item>
    /// <term>Width</term>
    /// <description>Specifies the width of the bitmap in pixels.
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the <b>Width</b> member specifies the width of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Height</term>
    /// <description>Specifies the height of the bitmap in pixels.
    /// If <b>Height</b> is positive, the bitmap is a bottom-up DIB and its origin is the lower-left corner.
    /// If <b>Height</b> is negative, the bitmap is a top-down DIB and its origin is the upper-left corner.
    /// Top-down DIBs cannot be compressed: <b>Compression</b> must be either <b>None</b> or <b>BitFields</b>.
    /// </description>
    /// <para>
    /// Windows 98/Me, Windows 2000 or above: If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>,
    /// the <b>Height</b> member specifies the height of the decompressed JPEG or PNG image bmpFile, respectively.</para></description>
    /// </item>
    /// <item>
    /// <term>Planes</term>
    /// <description>Specifies the number of planes for the target device. This value must be 1.</description>
    /// </item>
    /// <item>
    /// <term>BitsPerPixel</term>
    /// <description>Specifies the number of bits per pixel. This value must be 0, 1, 4, 8, 16, 24 or 32. <see>BitmapCompression</see></description>
    /// </item>
	/// <item>
    /// <term>Compression</term>
    /// <description>Specifies the type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).  <see>BitmapCompressionType</see></description>
    /// </item>
	/// <item>
    /// <term>ImageSize</term>
    /// <description>Specifies the size, in bytes, of the image. This may be set to zero for <b>RGB</b> bitmaps.
	///
	/// If <b>Compression</b> is <b>JPEG</b> or <b>PNG</b>, <b>ImageSize</b> indicates the size of the JPEG or PNG image buffer, respectively.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterX</term>
    /// <description>Specifies the horizontal resolution, in pixels-per-meter, of the target device for the bitmap. 
	/// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current device.</description>
    /// </item>
	/// <item>
    /// <term>PixelsPerMeterY</term>
    /// <description>Specifies the vertical resolution, in pixels-per-meter, of the target device for the bitmap.</description>
    /// </item>
	/// <item>
    /// <term>PaletteColors</term>
    /// <description>Specifies the number of color indexes in the color palette that are actually used by the bitmap. 
	/// If this value is 0, the bitmap uses the maximum number of colors corresponding to the value of the <b>BitsPerPixel</b> member for the compression mode specified by <b>Compression</b>.
	/// If is nonzero and the <b>BitsPerPixel</b> member is less than 16, the <b>PaletteColors</b> member specifies the actual number of colors the graphics engine or device driver accesses.
	/// If <b>BitsPerPixel</b> is 16 or greater, the <b>PaletteColors</b> member specifies the size of the color table used to optimize performance of the system color palettes. 
	/// If <b>BitsPerPixel</b> equals 16 or 32, the optimal color palette starts immediately following the three DWORD masks. 
	/// When the bitmap array immediately follows the BITMAPINFO structure, it is a packed bitmap. 
	/// Packed bitmaps are referenced by a single pointer. 
	/// Packed bitmaps require that the <b>PaletteColors</b> member must be either zero or the actual size of the color table.</description>
    /// </item>
	/// <item>
    /// <term>PaletteImportant</term>
    /// <description>Specifies the number of color indexes that are required for displaying the bitmap. 
	/// If this value is 0, all colors are required.</description>
    /// </item>
	/// <item>
    /// <term>MaskRed</term>
    /// <description>Color mask that specifies the red component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>MaskGreen</term>
    /// <description>Color mask that specifies the green component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>MaskBlue</term>
    /// <description>Color mask that specifies the blue component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>MaskAlpha</term>
    /// <description>Color mask that specifies the alpha (transparencie) component of each pixel, valid only if <b>Compression</b> is set to <b>BitFields</b>.</description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceType</term>
    /// <description>Specifies the color space of the DIB. <see>BitmapColorSpaceType</see></description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceEndPoints</term>
    /// <description>A structure that specifies the x, y and z coordinates of the three colors that correspond to the 
	/// red, green and blue endpoints for the logical color space associated with the bitmap. <see>CIEXYZ_TripleV4</see>.
    /// This member is ignored unless the <b>ColorSpaceType</b> member specifies <b>Calibrated_RBG</b>.
	/// <para><b>Note:>/byte1>  A color space is a model for representing color numerically in terms of three or more coordinates. 
	/// For example, the RGB color space represents colors in terms of the red, green and blue coordinates.</para></description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceGammaRed</term>
    /// <description>Toned response curve for red. 
    /// This member is ignored unless color values are calibrated RGB values and <b>ColorSpaceType</b> is set to <b>Calibrated_RBG</b>.
    /// Specified in 16^16 format.</description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceGammaGreen</term>
    /// <description>Toned response curve for green. 
    /// This member is ignored unless color values are calibrated RGB values and <b>ColorSpaceType</b> is set to <b>Calibrated_RBG</b>.
    /// Specified in 16^16 format.</description>
    /// </item>
	/// <item>
    /// <term>ColorSpaceGammaBlue</term>
    /// <description>Toned response curve for blue. 
    /// This member is ignored unless color values are calibrated RGB values and <b>ColorSpaceType</b> is set to <b>Calibrated_RBG</b>.
    /// Specified in 16^16 format.</description>
    /// </item>
	/// <item>
    /// <term>RenderingIntent</term>
    /// <description>Rendering intent for bitmap. <see>BitmapIntentV5</see>
	/// <para>The Independent Color Management interface (ICM) 2.0 allows International Color Consortium (ICC) color profiles to be linked or embedded in DIBs.</para></description>
    /// </item>
	/// <item>
    /// <term>ProfileOffset</term>
    /// <description>The offset, in bytes, from the beginning of the <b>BitmapHeaderV5</b> structure to the start of the profile data.
	/// This member is ignored unless <b>ColorSpaceType</b> is set to <b>ProfileLinked</b> or <b>ProfileEmbedded</b>.
	/// <para>If the profile is embedded, profile data is the actual ICM 2.0 profile.</para>
	/// <para>If the profile is linked,  profile data is the null-terminated bmpFile name of the ICM 2.0 profile or 
	/// the fully qualified path (including a network path) of the profile that can be opened using the CreateFile function.
	/// It must be composed exclusively of characters from the Windows character set (code page 1252). This cannot be a Unicode string.</para></description>
	/// </item>
	/// <item>
    /// <term>ProfileSize</term>
    /// <description>Size, in bytes, of embedded profile data.
	/// This member is ignored unless <b>ColorSpaceType</b> is set to <b>ProfileLinked</b> or <b>ProfileEmbedded</b>.
	/// <para>The profile data (if present) should follow the color table.<para></description>
	/// <para>For packed DIBs, the profile data should follow the bitmap bits similar to the bmpFile format.<para></description>
    /// </item>
	/// <item>
    /// <term>Reserved</term>
    /// <description>This member has been reserved for future use. Its value should be set to zero.</description>
    /// </item>
    /// </list>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 124)]
    public struct BitmapHeaderV5
    {
        public uint    		    HeaderSize; 
	    public int	    	    Width; 
	    public int    		    Height;
        public ushort           Planes;
        public ushort           BitsPerPixel;
        public uint             Compression;
        public uint             ImageSize;
        public int              PixelsPerMeterX;
        public int              PixelsPerMeterY;
        public uint             PaletteColors;
        public uint             PaletteImportant;
        public uint             MaskRed;
        public uint             MaskGreen;
        public uint             MaskBlue;
        public uint             MaskAlpha;
        public uint             ColorSpaceType;
        public CIEXYZ_TripleV4  ColorSpaceEndPoints;
        public uint             ColorSpaceGammaRed;
        public uint             ColorSpaceGammaGreen;
        public uint             ColorSpaceGammaBlue;
        public uint             RenderingIntent;
        public uint             ProfileOffset;
        public uint             ProfileSize;
        public uint             Reserved; 
    }
	
    public class BMP_File : IGraphicFile
    {
        // BMP File extention
        private static string[] FileExt = {"bmp"};
        
        // BMP Magic ID ("BM" = 0x4D42, 'B'=0x42; 'M'=0x4D)
        private const ushort BMP_MAGIC_ID = 0x4D42;

        // Microsoft Windows 2.x and IBM OS/2 Presentation Manager versions 1.x bitmaps
        protected const int BMP_FILE_HEADER_SIZE_V2 = 14;     

        // Microsoft Windows 2.x and IBM OS/2 Presentation Manager versions 1.x bitmaps
        protected const int BMP_PALETTE_SIZE_V2 = 3;

        // Microsoft Windows 3.xx and Windows NT 3.xx
        protected const int BMP_PALETTE_SIZE_V3 = 4;
        
        // IBM OS/2 Presentation Manager versions 1.x bitmaps (same as Microsoft Windows 2.x)
        protected const int BMP_HEADER_OS2_SIZE_V1 =  12;
        // IBM OS/2 Presentation Manager versions 2.x bitmaps
        protected const int BMP_HEADER_OS2_SIZE_V2 = 64;

        // Microsoft Windows 2.x (same as IBM OS/2 Presentation Manager versions 1.x)
        protected const int BMP_HEADER_WIN_SIZE_V2 =  12;
        // Microsoft Windows 3.xx and Windows NT 3.xx
        protected const int BMP_HEADER_WIN_SIZE_V3 =  40;
        // Windows 95 and Windows NT 4.0
        protected const int BMP_HEADER_WIN_SIZE_V4 = 108;
        // Windows 98, ME, 2000, XP and 2003 Server
        protected const int BMP_HEADER_WIN_SIZE_V5 = 124;

        private BitmapFileHeaderV2 BitmapFileHeader;
        private BitmapHeaderV5 BitmapHeader;
        private OS2_BitmapHeaderV2 BitmapHeaderOS2;
        private ColorPalette Palette = new ColorPalette();

        private ColorEntryRGBA<byte>[] Pixels = null;

        private byte[] ProfileICM = null;

        /// <summary>
        /// This is <b>true</b> if the bmpFile loaded from a bmpFile and <b>false</b> otherwise.
        /// 
        /// If <b>OpenForReadding</b> is true, the properties can only be read and we can't modify or save the bitmap.
        /// </summary>
        private bool OpenForReadding = false;

        public BMP_File()
        {
            // Bitmap File Header
            this.BitmapFileHeader.Magic = BMP_MAGIC_ID;
            this.BitmapFileHeader.FileSize = 0;
            this.BitmapFileHeader.Reserved1 = 0;
            this.BitmapFileHeader.Reserved2 = 0;
            this.BitmapFileHeader.PixelsOffset = 0;

            // Bitmap v5 Header
            this.BitmapHeader.HeaderSize       = BMP_HEADER_WIN_SIZE_V3; 
	        this.BitmapHeader.Width            = 0; 
	        this.BitmapHeader.Height           = 0;
            this.BitmapHeader.Planes           = 1;
            this.BitmapHeader.BitsPerPixel     = (ushort)BitmapBitsPerPixel.RGB_24;
            this.BitmapHeader.Compression      = (uint)BitmapCompressionType.None;
            this.BitmapHeader.ImageSize        = 0;
            this.BitmapHeader.PixelsPerMeterX  = 0;
            this.BitmapHeader.PixelsPerMeterY  = 0;
            this.BitmapHeader.PaletteColors    = 0;
            this.BitmapHeader.PaletteImportant = 0;
            this.BitmapHeader.MaskRed          = 0;
            this.BitmapHeader.MaskGreen        = 0;
            this.BitmapHeader.MaskBlue         = 0;
            this.BitmapHeader.MaskAlpha        = 0;
            this.BitmapHeader.ColorSpaceType   = (uint)BitmapColorSpaceType.sRGB;
            this.BitmapHeader.ColorSpaceEndPoints.Red.X = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Red.Y = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Red.Z = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Green.X = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Green.Y = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Green.Z = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Blue.X = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Blue.Y = 0;
            this.BitmapHeader.ColorSpaceEndPoints.Blue.Z = 0;
            this.BitmapHeader.ColorSpaceGammaRed = 0;
            this.BitmapHeader.ColorSpaceGammaGreen = 0;
            this.BitmapHeader.ColorSpaceGammaBlue = 0;
            this.BitmapHeader.RenderingIntent  = (uint)BitmapIntentV5.Graphics;
            this.BitmapHeader.ProfileOffset    = 0;
            this.BitmapHeader.ProfileSize      = 0;
            this.BitmapHeader.Reserved         = 0;
            
            this.setFileVersion(3, 0);
            this.setColorDeep(24);
        }

        //------
        public string[] getFileExtention()
        {
            return FileExt;
        }

        public bool isTopDown()
        {
            if (this.BitmapHeader.Height < 0)
                return true;
            else
                return false;
        }
        
        public void getFileVersion(out int major, out int minor)
        {
            switch (this.BitmapHeader.HeaderSize)
            { 
                //case BMP_HEADER_OS2_SIZE_V1:
                case BMP_HEADER_WIN_SIZE_V2:
                    // Microsoft Windows v2 & IBM OS/2 v1 (Windows 2.0 and OS/2 1.0)
                    major = 2;
                    break;
                case BMP_HEADER_WIN_SIZE_V3:
                    // Microsoft Windows v3 (Windows 3.xx)
                    major = 3;
                    break;
                case BMP_HEADER_WIN_SIZE_V4:
                    // Microsoft Windows v4 (Windows 95, NT 3.1)
                    major = 4;
                    break;
                case BMP_HEADER_WIN_SIZE_V5:
                    // Microsoft Windows v5 (Windows 98, 2000)
                    major = 5;
                    break;
                default:
                    if((this.BitmapHeader.HeaderSize > BMP_HEADER_OS2_SIZE_V1) && (this.BitmapHeader.HeaderSize <= BMP_HEADER_OS2_SIZE_V2))
                    {
                        // IBM OS/2 v2
                        major = 3;
                    }
                    else
                    {
                        // Unknown
                        major = 0;
                    }
                    break;
            }
            minor = 0;
        }

        public void setFileVersion(int major, int minor)
        {
            if (this.OpenForReadding)
                return;

            switch (major)
            {
                //case 2:
                //    this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V2;
                //    break;
                case 3:
                    this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V3;
                    break;
                case 4:
                    this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V4;
                    break;
                case 5:
                    this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V5;
                    break;
                default:
                    this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V3;
                    break;
            }
        }

        public void getSize(out uint x, out uint y)
        {
            x = (uint)this.BitmapHeader.Width;
            y = (uint)Math.Abs(this.BitmapHeader.Height);
        }

        public void setSize(uint x, uint y)
        {
            if (this.OpenForReadding)
                return;

            this.BitmapHeader.Width = (int)x;
            this.BitmapHeader.Height = (int)y;

            Pixels = new ColorEntryRGBA<byte>[y, x];
        }

        public void getDPI(out int x, out int y)
        {
            x = (int)((double)this.BitmapHeader.PixelsPerMeterX * 0.0254);
            y = (int)((double)this.BitmapHeader.PixelsPerMeterY * 0.0254);
        }

        public void setDPI(int x, int y)
        {
            if (this.OpenForReadding)
                return;

            /*
             *                     Desired DPI     1 inch      100 cm     Desired DPI
             * Pixels Per Meter = ------------- x --------- x -------- = -------------
             *                       1 inch        2.54 cm      1 m         0.0254 m
             * Exemlpe:
             *  96 DPI = 3780 ppm
             * 
             */
            this.BitmapHeader.PixelsPerMeterX = (int)((double)x / 0.0254) + 1;
            this.BitmapHeader.PixelsPerMeterY = (int)((double)y / 0.0254) + 1;
        }

        public float getGama()
        {
            uint r = this.BitmapHeader.ColorSpaceGammaRed   / 0xFFFF;
            uint g = this.BitmapHeader.ColorSpaceGammaGreen / 0xFFFF;
            uint b = this.BitmapHeader.ColorSpaceGammaBlue  / 0xFFFF;

            double sum = ((double)r + (double)g + (double)b) / 3.0;

            return (float)sum;
        }

        public void setGama(float gama)
        {
            if (this.OpenForReadding)
                return;
            
            double sum = (double)gama * 3.0;
            
            uint r = (uint)sum * 0xFFFF;
            uint g = (uint)sum * 0xFFFF;
            uint b = (uint)sum * 0xFFFF;

            this.BitmapHeader.ColorSpaceGammaRed = r;
            this.BitmapHeader.ColorSpaceGammaGreen = g;
            this.BitmapHeader.ColorSpaceGammaBlue = b;

            this.BitmapHeader.ColorSpaceType = (uint)BitmapColorSpaceType.Calibrated_RBG;

            if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V4)
                this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V4;
        }

        public uint getColorDeep()
        {
            return this.BitmapHeader.BitsPerPixel;
        }
        
        public void setColorDeep(uint bitsPerPixel)
        {
            if (this.OpenForReadding)
                return;

            switch (this.BitmapHeader.HeaderSize)
            {
                //case BMP_HEADER_OS2_SIZE_V1:
                case BMP_HEADER_OS2_SIZE_V2:
                case BMP_HEADER_WIN_SIZE_V2:
                    this.BitmapHeader.BitsPerPixel = (ushort)((BitsPerPixel)bitsPerPixel);
                    break;
                case BMP_HEADER_WIN_SIZE_V3:
                case BMP_HEADER_WIN_SIZE_V4:
                case BMP_HEADER_WIN_SIZE_V5:
                    this.BitmapHeader.BitsPerPixel = (ushort)((BitmapBitsPerPixel)bitsPerPixel);
                    break;
                default:
                    // This should never happen.
                    this.BitmapHeader.BitsPerPixel = (ushort)((BitmapBitsPerPixel)bitsPerPixel);
                    break;
            }

            if ((bitsPerPixel > 0) && (bitsPerPixel < 16))
            {
                uint nColors = 1;
                for (int i = 0; i < bitsPerPixel; i++)
                    nColors = nColors << 1;
                Palette.NumberOfEntries = nColors;
            }
        }

        public RTPixelFormat getPixelFormat()
        {
            RTPixelFormat pixelFormat;

            switch ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel)
            {
                case BitmapBitsPerPixel.JPEG_PNG:
                    // No info for this format
                    throw new Exception("JPEG and PNG compressed BMP is not supported!");
                    //break;
                case BitmapBitsPerPixel.MonoChrome:
                case BitmapBitsPerPixel.Palette_16:
                case BitmapBitsPerPixel.Palette_256:
                    pixelFormat = RTPixelFormat.RGB080808;
                    break;
                case BitmapBitsPerPixel.RGB_16:
                    pixelFormat = RTPixelFormat.Unknow;
                    if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.BitFields)
                    {
                        if (this.BitmapHeader.MaskAlpha == 0)
                        {
                            if ((this.BitmapHeader.MaskRed == (uint)0x00007C00) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x000003E0) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x0000001F))
                                pixelFormat = RTPixelFormat.RGB050505;
                            if ((this.BitmapHeader.MaskRed == (uint)0x0000F800) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x000007E0) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x0000001F))
                                pixelFormat = RTPixelFormat.RGB050605;
                        }
                    }
                    break;
                case BitmapBitsPerPixel.RGB_24:
                    if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None)
                        pixelFormat = RTPixelFormat.RGB080808;
                    else
                        pixelFormat = RTPixelFormat.Unknow;
                    break;
                case BitmapBitsPerPixel.RGB_32:
                    pixelFormat = RTPixelFormat.Unknow;
                    if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.BitFields)
                    {
                        if (this.BitmapHeader.MaskAlpha == 0)
                        {
                            if ((this.BitmapHeader.MaskRed == (uint)0x00007C00) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x000003E0) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x0000001F))
                                pixelFormat = RTPixelFormat.RGB050505;
                            if ((this.BitmapHeader.MaskRed == (uint)0x0000F800) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x000007E0) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x0000001F))
                                pixelFormat = RTPixelFormat.RGB050605;
                            if ((this.BitmapHeader.MaskRed == (uint)0x00FF0000) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x0000FF00) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x000000FF))
                                pixelFormat = RTPixelFormat.RGB080808;
                            if ((this.BitmapHeader.MaskRed == (uint)0x3FF00000) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x000FFC00) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x000003FF))
                                pixelFormat = RTPixelFormat.RGB101010;
                        }
                        if (this.BitmapHeader.MaskAlpha == (uint)0x001F0000)
                        {
                            if ((this.BitmapHeader.MaskRed == (uint)0x00007C00) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x000003E0) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x0000001F))
                                pixelFormat = RTPixelFormat.RGBA05050501;
                            if ((this.BitmapHeader.MaskRed == (uint)0x0000F800) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x000007E0) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x0000001F))
                                pixelFormat = RTPixelFormat.RGBA05050505;
                        }
                        if (this.BitmapHeader.MaskAlpha == (uint)0xFF000000)
                        {
                            if ((this.BitmapHeader.MaskRed == (uint)0x00FF0000) &&
                                 (this.BitmapHeader.MaskGreen == (uint)0x0000FF00) &&
                                 (this.BitmapHeader.MaskBlue == (uint)0x000000FF))
                                pixelFormat = RTPixelFormat.RGBA08080808;
                        }
                    }
                    if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None)
                        pixelFormat = RTPixelFormat.RGB080808;
                    break;
                default:
                    // This should never happen.
                    throw new Exception("Unknown Bits per Pixel!");
                    //break;
            }

            return pixelFormat;
        }

        public void setPixelFormat(RTPixelFormat pixelFormat)
        {
            if (this.OpenForReadding)
                return;

            switch (pixelFormat)
            {
                case RTPixelFormat.RGB050505:
                    this.BitmapHeader.MaskRed = (uint)0x00007C00;     /* 0000 0000 0000 0000 0111 1100 0000 0000 */
                    this.BitmapHeader.MaskGreen = (uint)0x000003E0;     /* 0000 0000 0000 0000 0000 0011 1110 0000 */
                    this.BitmapHeader.MaskBlue = (uint)0x0000001F;     /* 0000 0000 0000 0000 0000 0000 0001 1111 */
                    this.BitmapHeader.MaskAlpha = (uint)0x00000000;     /* 0000 0000 0000 0000 0000 0000 0000 0000 */
                    
                    if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V3)
                        this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V3;
                    if (this.BitmapHeader.Compression != (uint)BitmapCompressionType.BitFields)
                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.BitFields;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_16)
                        this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_16;
                    break;
                case RTPixelFormat.RGB050605:
                    this.BitmapHeader.MaskRed = (uint)0x0000F800;     /* 0000 0000 0000 0000 1111 1000 0000 0000 */
                    this.BitmapHeader.MaskGreen = (uint)0x000007E0;     /* 0000 0000 0000 0000 0000 0111 1110 0000 */
                    this.BitmapHeader.MaskBlue = (uint)0x0000001F;     /* 0000 0000 0000 0000 0000 0000 0001 1111 */
                    this.BitmapHeader.MaskAlpha = (uint)0x00000000;     /* 0000 0000 0000 0000 0000 0000 0000 0000 */

                    if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V3)
                        this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V3;
                    if (this.BitmapHeader.Compression != (uint)BitmapCompressionType.BitFields)
                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.BitFields;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_16)
                        this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_16;
                    break;
                case RTPixelFormat.RGB080808:
                    this.BitmapHeader.MaskRed = (uint)0x00FF0000;     /* 0000 0000 1111 1111 0000 0000 0000 0000 */
                    this.BitmapHeader.MaskGreen = (uint)0x0000FF00;     /* 0000 0000 0000 0000 1111 1111 0000 0000 */
                    this.BitmapHeader.MaskBlue = (uint)0x000000FF;     /* 0000 0000 0000 0000 0000 0000 1111 1111 */
                    this.BitmapHeader.MaskAlpha = (uint)0x00000000;     /* 0000 0000 0000 0000 0000 0000 0000 0000 */

                    if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V3)
                        this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V3;
                    if ((this.BitmapHeader.Compression != (uint)BitmapCompressionType.BitFields) &&
                        (this.BitmapHeader.Compression != (uint)BitmapCompressionType.None))
                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.None;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_24)
                    {
                        if (this.BitmapHeader.Compression != (uint)BitmapCompressionType.None)
                            this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_24;
                        else
                            this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_32;
                    }
                    break;
                case RTPixelFormat.RGB101010:
                    this.BitmapHeader.MaskRed = (uint)0x3FF00000;     /* 0011 1111 1111 0000 0000 0000 0000 0000 */
                    this.BitmapHeader.MaskGreen = (uint)0x000FFC00;     /* 0000 0000 0000 1111 1111 1100 0000 0000 */
                    this.BitmapHeader.MaskBlue = (uint)0x000003FF;     /* 0000 0000 0000 0000 0000 0011 1111 1111 */
                    this.BitmapHeader.MaskAlpha = (uint)0x00000000;     /* 0000 1000 0000 0000 0000 0000 0000 0000 */

                    if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V4)
                        this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V4;
                    if (this.BitmapHeader.Compression != (uint)BitmapCompressionType.BitFields)
                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.BitFields;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_32)
                        this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_32;
                    break;
                case RTPixelFormat.RGBA05050501:
                    this.BitmapHeader.MaskRed = (uint)0x00007C00;     /* 0000 0000 0000 0000 0111 1100 0000 0000 */
                    this.BitmapHeader.MaskGreen = (uint)0x000003E0;     /* 0000 0000 0000 0000 0000 0011 1110 0000 */
                    this.BitmapHeader.MaskBlue = (uint)0x0000001F;     /* 0000 0000 0000 0000 0000 0000 0001 1111 */
                    this.BitmapHeader.MaskAlpha = (uint)0x00008000;     /* 0000 0000 0000 0000 1000 0000 0000 0000 */

                    if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V4)
                        this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V4;
                    if (this.BitmapHeader.Compression != (uint)BitmapCompressionType.BitFields)
                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.BitFields;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_32)
                        this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_32;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_32)
                        this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_32;
                    break;
                case RTPixelFormat.RGBA05050505:
                    this.BitmapHeader.MaskRed = (uint)0x0000F800;     /* 0000 0000 0000 0000 1111 1000 0000 0000 */
                    this.BitmapHeader.MaskGreen = (uint)0x000007E0;     /* 0000 0000 0000 0000 0000 0111 1110 0000 */
                    this.BitmapHeader.MaskBlue = (uint)0x0000001F;     /* 0000 0000 0000 0000 0000 0000 0001 1111 */
                    this.BitmapHeader.MaskAlpha = (uint)0x001F0000;     /* 0000 0000 0001 1111 0000 0000 0000 0000 */

                    if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V4)
                        this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V4;
                    if (this.BitmapHeader.Compression != (uint)BitmapCompressionType.BitFields)
                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.BitFields;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_32)
                        this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_32;
                    break;
                case RTPixelFormat.RGBA08080808:
                default:
                    this.BitmapHeader.MaskRed = (uint)0x00FF0000;     /* 0000 0000 1111 1111 0000 0000 0000 0000 */
                    this.BitmapHeader.MaskGreen = (uint)0x0000FF00;     /* 0000 0000 0000 0000 1111 1111 0000 0000 */
                    this.BitmapHeader.MaskBlue = (uint)0x000000FF;     /* 0000 0000 0000 0000 0000 0000 1111 1111 */
                    this.BitmapHeader.MaskAlpha = (uint)0xFF000000;     /* 1111 1111 0000 0000 0000 0000 0000 0000 */

                    if (this.BitmapHeader.HeaderSize < BMP_HEADER_WIN_SIZE_V4)
                        this.BitmapHeader.HeaderSize = BMP_HEADER_WIN_SIZE_V4;
                    if (this.BitmapHeader.Compression != (uint)BitmapCompressionType.BitFields)
                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.BitFields;
                    if (this.BitmapHeader.BitsPerPixel < (ushort)BitmapBitsPerPixel.RGB_32)
                        this.BitmapHeader.BitsPerPixel = (ushort)BitmapBitsPerPixel.RGB_32;
                    break;
            }
        }

        public uint getNumberOfImages()
        {
            return 1;
        }

        public void setNumberOfImages(uint nImages)
        {
            return;
        }

        public uint getCurrentImage()
        {
            return 1;
        }

        public void setCurrentImage(uint nImage)
        {
            return;
        }

        public byte[] getProfileICM()
        {
            return ProfileICM;
        }

        public void setProfileICM(ref byte[] icm)
        {
            if ((icm == null) || (icm.Length == 0))
            {
                this.BitmapHeader.ProfileSize = 0;
                ProfileICM = null;
            }
            else
            {
                ProfileICM = icm;
                this.BitmapHeader.ProfileSize = (uint)ProfileICM.Length;
                this.BitmapHeader.ColorSpaceType = (uint)BitmapColorSpaceType.ProfileEmbedded;
            }
        }

        public void setProfileICM(string path)
        {
            if ((path == null) || (path.Length == 0))
            {
                this.BitmapHeader.ProfileSize = 0;
                ProfileICM = null;
            }
            else
            {
                byte[] buffer = new byte[2 * path.Length];
                char[] chars = path.ToCharArray();
                int charLen = 0;

                Encoder encoder = Encoding.GetEncoding("Windows-1252").GetEncoder();
                charLen = encoder.GetBytes(chars, 0, chars.Length, buffer, 0, true);

                ProfileICM = new byte[charLen];
                Array.Copy(buffer, ProfileICM, charLen);
                this.BitmapHeader.ProfileSize = (uint)ProfileICM.Length;
                this.BitmapHeader.ColorSpaceType = (uint)BitmapColorSpaceType.ProfileLinked;
            }
        }

        public Dictionary<string, string> getExifData()
        {
            return new Dictionary<string, string>();
        }

        public void setExifData(ref Dictionary<string, string> hashTable)
        {
            if ((hashTable == null) || (hashTable.Count <= 0))
                return;
        }

        public ColorEntryRGBA<T> getPixel(int x, int y)
        {
            ColorEntryRGBA<T> color;
            int swap;
            if (this.BitmapHeader.Height > 0)
            {
                swap = this.BitmapHeader.Height - y - 1;
                color = Pixels[swap, x];
            }
            else
                color = Pixels[y, x];
            return color;
        }

        public void setPixel(int x, int y, ref ColorEntryRGBA<T> color)
        {
            if (this.OpenForReadding)
                return;

            int swap;
            if (this.BitmapHeader.Height > 0)
            {
                swap = this.BitmapHeader.Height - y - 1;
                Pixels[swap, x].Alpha = color.Alpha;
                Pixels[swap, x].Red = color.Red;
                Pixels[swap, x].Green = color.Green;
                Pixels[swap, x].Blue = color.Blue;
            }
            else
            {
                Pixels[y, x].Alpha = color.Alpha;
                Pixels[y, x].Red = color.Red;
                Pixels[y, x].Green = color.Green;
                Pixels[y, x].Blue = color.Blue;
            }
            
        }

        public void Load(string file)
        {
            if (this.OpenForReadding)
                return;
            try
            {
                FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);

                // Loads the bitmap file Header
                this.BitmapFileHeader.Magic = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                this.BitmapFileHeader.FileSize = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                this.BitmapFileHeader.Reserved1 = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                this.BitmapFileHeader.Reserved2 = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                this.BitmapFileHeader.PixelsOffset = (uint)RTStreamEndianUtils.GetWord32_LE(stream);

                // Loads the bitmap DIB Header
                this.BitmapHeader.HeaderSize = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                // Some BMP software calculate "BitmapFileHeader.FileSize" wrong, so we can't validate it
                //if ((long)this.BitmapFileHeader.FileSize > stream.Length)
                //{
                //    stream.Close();
                //    throw new Exception("Not a BMP file or the file is corrupted!");
                //}
                //else
                    this.BitmapFileHeader.FileSize = (uint)stream.Length;
                if ((this.BitmapFileHeader.Magic != BMP_MAGIC_ID) || ((
                    (this.BitmapHeader.HeaderSize != BMP_HEADER_WIN_SIZE_V2) &&
                    (this.BitmapHeader.HeaderSize != BMP_HEADER_WIN_SIZE_V3) &&
                    (this.BitmapHeader.HeaderSize != BMP_HEADER_WIN_SIZE_V4) &&
                    (this.BitmapHeader.HeaderSize != BMP_HEADER_WIN_SIZE_V5) &&
                    !((this.BitmapHeader.HeaderSize > BMP_HEADER_OS2_SIZE_V1) && 
                     (this.BitmapHeader.HeaderSize <= BMP_HEADER_OS2_SIZE_V2)))))
                {
                    stream.Close();
                    throw new Exception("Not a BMP file or version not supported!");
                }
                // OS/2 v2.x DIB
                if ((this.BitmapHeader.HeaderSize > BMP_HEADER_OS2_SIZE_V1) && 
                    (this.BitmapHeader.HeaderSize <= BMP_HEADER_OS2_SIZE_V2) &&
                    (this.BitmapHeader.HeaderSize != BMP_HEADER_WIN_SIZE_V3))
                {
                    // Static fields (16 bytes)
                    this.BitmapHeader.Width = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.Height = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.Planes = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    this.BitmapHeader.BitsPerPixel = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    // Optional fields (same as Windows v3.x)
                    if (this.BitmapHeader.HeaderSize >= (16 + 4))
                        this.BitmapHeader.Compression = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeader.Compression = (uint)0;
                    if (this.BitmapHeader.HeaderSize >= (16 + 4))
                        this.BitmapHeader.ImageSize = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeader.ImageSize = (uint)0;
                    if (this.BitmapHeader.HeaderSize >= (16 + 8))
                        this.BitmapHeader.PixelsPerMeterX = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeader.PixelsPerMeterX = (int)0;
                    if (this.BitmapHeader.HeaderSize >= (16 + 16))
                        this.BitmapHeader.PixelsPerMeterY = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeader.PixelsPerMeterY = (int)0;
                    if (this.BitmapHeader.HeaderSize >= (16 + 24))
                        this.BitmapHeader.PaletteColors = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeader.PaletteColors = (uint)0;
                    if (this.BitmapHeader.HeaderSize >= (16 + 32))
                        this.BitmapHeader.PaletteImportant = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeader.PaletteImportant = (uint)0;
                    // Optional fields (OS/2 v2.x spcific)
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 2))
                        this.BitmapHeaderOS2.Units = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    else
                        this.BitmapHeaderOS2.Units = (ushort)0;
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 4))
                        this.BitmapHeaderOS2.Reserved = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    else
                        this.BitmapHeaderOS2.Reserved = (ushort)0;
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 6))
                        this.BitmapHeaderOS2.Recording = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    else
                        this.BitmapHeaderOS2.Recording = (ushort)0;
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 8))
                        this.BitmapHeaderOS2.Rendering = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    else
                        this.BitmapHeaderOS2.Rendering = (ushort)0;
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 12))
                        this.BitmapHeaderOS2.Size1 = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeaderOS2.Size1 = (uint)0;
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 16))
                        this.BitmapHeaderOS2.Size2 = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeaderOS2.Size2 = (uint)0;
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 20))
                        this.BitmapHeaderOS2.ColorEncoding = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeaderOS2.ColorEncoding = (uint)0;
                    if (this.BitmapHeader.HeaderSize >= (BMP_HEADER_WIN_SIZE_V3 + 24))
                        this.BitmapHeaderOS2.Identifier = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    else
                        this.BitmapHeaderOS2.Identifier = (uint)0;
                    
                    // Mark as an OS/2 bitmap
                    this.BitmapHeader.HeaderSize = BMP_HEADER_OS2_SIZE_V2;
                }
                if (this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V2)
                {
                    this.BitmapHeader.Width = (int)RTStreamEndianUtils.GetWord16_LE(stream);
                    this.BitmapHeader.Height = (int)RTStreamEndianUtils.GetWord16_LE(stream);
                    this.BitmapHeader.Planes = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    this.BitmapHeader.BitsPerPixel = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    // Calculates the number of entries in the palette
                    this.BitmapHeader.PaletteColors = (this.BitmapFileHeader.PixelsOffset -
                                                       (uint)BMP_FILE_HEADER_SIZE_V2 - BMP_HEADER_OS2_SIZE_V1) / 
                                                       (uint)BMP_PALETTE_SIZE_V2;
                    this.BitmapHeader.PaletteImportant = this.BitmapHeader.PaletteColors;
                }
                if ((this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V3) && (this.BitmapHeader.HeaderSize != BMP_HEADER_OS2_SIZE_V2))
                {
                    this.BitmapHeader.Width = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.Height = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.Planes = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    this.BitmapHeader.BitsPerPixel = (ushort)RTStreamEndianUtils.GetWord16_LE(stream);
                    this.BitmapHeader.Compression = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ImageSize = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.PixelsPerMeterX = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.PixelsPerMeterY = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.PaletteColors = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.PaletteImportant = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                }
                if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V4)
                {
                    this.BitmapHeader.MaskRed          = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.MaskGreen        = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.MaskBlue         = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.MaskAlpha        = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceType   = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Red.X = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Red.Y = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Red.Z = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Green.X = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Green.Y = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Green.Z = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Blue.X = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Blue.Y = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceEndPoints.Blue.Z = (int)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceGammaRed = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceGammaGreen = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ColorSpaceGammaBlue = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                }
                if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V5)
                {
                    this.BitmapHeader.RenderingIntent = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ProfileOffset = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.ProfileSize = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.Reserved = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                }
                if (this.BitmapHeader.Width < 0)
                {
                    stream.Close();
					throw new Exception("The BMP with can't be negative (x < 0)!");
                }

                // Checks unsupported OS/2 BMP v2.x compression
                if (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V2)
                {
                    if ((this.BitmapHeader.Compression == (uint)OS2_BitmapCompressionType.Huffman_1D) ||
                        (this.BitmapHeader.Compression == (uint)OS2_BitmapCompressionType.RLE_24))
                    {
                        stream.Close();
						throw new Exception("OS/2 BMP v2.x compression can't be 'Huffman 1D' or 'RLE 24'!");
                    }
                }

                // Loads the palette
                if (this.BitmapHeader.BitsPerPixel < 16)
                {
                    uint maxColors = 1;
                    for (int i = 0; i < this.BitmapHeader.BitsPerPixel; i++)
                        maxColors = maxColors << 1;
                    if (this.BitmapHeader.PaletteColors == 0)
                    {
                        this.BitmapHeader.PaletteColors = maxColors;
                        this.BitmapHeader.PaletteImportant = maxColors;
                    }
                    if (maxColors != this.BitmapHeader.PaletteColors)
                    {
                        if (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V1)
                        {
                            this.BitmapHeader.PaletteColors = Math.Min(maxColors, this.BitmapHeader.PaletteColors);
                            this.BitmapHeader.PaletteImportant = this.BitmapHeader.PaletteColors;
                        }
                        else
                        {
                            stream.Close();
                            throw new Exception("Not a BMP file or the file is corrupted!");
                        }
                    }
                    Palette.NumberOfEntries = (uint)Math.Min(this.BitmapHeader.PaletteColors, maxColors);
                    ColorEntryRGBA<T> color;
                    for (int i = 0; i < Palette.NumberOfEntries; i++)
                    {
                        if ((this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V1) || 
                            (this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V2))
                        {
                            color.Blue = (byte)RTStreamEndianUtils.GetByte(stream);
                            color.Green = (byte)RTStreamEndianUtils.GetByte(stream);
                            color.Red = (byte)RTStreamEndianUtils.GetByte(stream);
                            color.Alpha = (byte)0;
                        }
                        else
                        {
                            color.Blue = (byte)RTStreamEndianUtils.GetByte(stream);
                            color.Green = (byte)RTStreamEndianUtils.GetByte(stream);
                            color.Red = (byte)RTStreamEndianUtils.GetByte(stream);
                            color.Alpha = (byte)RTStreamEndianUtils.GetByte(stream);
                        }
                        Palette.setEntry(i, ref color);
                    }
                }

                // Loads the colors masks for Header v3
                if ((this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V3) &&
                    ((BitmapCompressionType)this.BitmapHeader.Compression == BitmapCompressionType.BitFields) &&
                    (((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_16) ||
                    ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_32)))
                {
                    this.BitmapHeader.MaskRed = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.MaskGreen = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                    this.BitmapHeader.MaskBlue = (uint)RTStreamEndianUtils.GetWord32_LE(stream);
                }

                // Loads the ICM profile for paletted DIBs
                if (this.BitmapHeader.ProfileSize == 0)
                    ProfileICM = null;
                else
                    ProfileICM = new byte[this.BitmapHeader.ProfileSize];

                if ((Palette.NumberOfEntries > 0) && (this.BitmapHeader.ProfileSize > 0))
                {
                    RTStreamEndianUtils.GetBytes(stream, ref ProfileICM);
                }

                // Loads the pixels
                if (stream.Position != (long)this.BitmapFileHeader.PixelsOffset)
                {
                    if (stream.Position > (long)this.BitmapFileHeader.PixelsOffset)
                    {
                        stream.Close();
                        throw new Exception("Not a BMP file or the file is corrupted!");
                    }
                    else
                    {
                        stream.Position = (long)this.BitmapFileHeader.PixelsOffset;
                    }
                }

                int sizeOfPixel = (int)this.BitmapHeader.BitsPerPixel / 8;
                if (sizeOfPixel < 1)
                    sizeOfPixel = 1;
                
                if (this.BitmapHeader.ImageSize == 0)
                {
                    switch ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel)
                    {
                        case BitmapBitsPerPixel.JPEG_PNG:
                            throw new Exception("JPEG and PNG compressed BMP is not supported!");
                            //break;
                        case BitmapBitsPerPixel.MonoChrome:
                        case BitmapBitsPerPixel.Palette_16:
                        case BitmapBitsPerPixel.Palette_256:
                        case BitmapBitsPerPixel.RGB_24:
                            this.BitmapHeader.ImageSize = (uint)(4 * (((this.BitmapHeader.Width *
                                                          this.BitmapHeader.BitsPerPixel) + 31) / 32) *
                                                          Math.Abs(this.BitmapHeader.Height));
                            break;
                        case BitmapBitsPerPixel.RGB_16:
                            if (this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V3)
                                this.BitmapHeader.ImageSize = (uint)(2 * ((this.BitmapHeader.Width % 2) + this.BitmapHeader.Width) *
                                                           Math.Abs(this.BitmapHeader.Height));
                            else
                                this.BitmapHeader.ImageSize = (uint)(4 * this.BitmapHeader.Width * Math.Abs(this.BitmapHeader.Height));
                            break;
                        case BitmapBitsPerPixel.RGB_32:
                            this.BitmapHeader.ImageSize = (uint)(4 * this.BitmapHeader.Width * Math.Abs(this.BitmapHeader.Height));
                            break;
                        default:
                            // This should never happen.
                            throw new Exception("Unknown Bits per Pixel!");
                            //break;
                    }
                }
                int pad = ((int)this.BitmapHeader.ImageSize / Math.Abs(this.BitmapHeader.Height)) - (sizeOfPixel * this.BitmapHeader.Width);
                if (pad < 0)
                    pad = 0;
                byte b;

                Pixels = new ColorEntryRGBA<T>[Math.Abs(this.BitmapHeader.Height), this.BitmapHeader.Width];

                // Pixels
                switch ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel)
                {
                    case BitmapBitsPerPixel.JPEG_PNG:
                        // No info for this format
                        throw new Exception("JPEG and PNG compressed BMP is not supported!");
                        //break;
                    case BitmapBitsPerPixel.MonoChrome:
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None)
                        {
                            int bytesPerLine = (int)this.BitmapHeader.ImageSize / (int)Math.Abs(this.BitmapHeader.Height);
                            int x;
                            int y;

                            byte[] byteN = new byte[bytesPerLine];
                            for (y = 0; y < Math.Abs(this.BitmapHeader.Height); y++)
                            {
                                RTStreamEndianUtils.GetBytes(stream, ref byteN);
                                int pos = 0;
                                for (x = 0; x < (this.BitmapHeader.Width - 8); x +=8)
                                {
                                    b = byteN[pos];
                                    ++pos;
                                    for (int i = 0; i < 8; i++)
                                    {
                                        int p = (int)((b & (0x80 >> i)) != 0 ? 0x01 : 0x00);
                                        this.Pixels[y, x + i] = Palette.getEntry(p);
                                    }
                                }
                                if ((this.BitmapHeader.Width % 8) != 0)
                                {
                                    b = byteN[pos];
                                    ++pos;
                                    for (int i = 0; i < (this.BitmapHeader.Width % 8); i++)
                                    {
                                        int p = (int)((b & (0x80 >> i)) != 0 ? 0x01 : 0x00);
                                        this.Pixels[y, x + i] = Palette.getEntry(p);
                                    }
                                }
                            }
                            break;
                        }
                        else
                        {
                            throw new Exception("This compression of 8 bits BMP is not supported!");
                        }
                        //break;
                    case BitmapBitsPerPixel.Palette_16:
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None)
                        {
                            int bytesPerLine = (int)this.BitmapHeader.ImageSize / (int)Math.Abs(this.BitmapHeader.Height);
                            int x;
                            int y;
                            int p;

                            byte[] byteN = new byte[bytesPerLine];
                            int x1;
                            for (y = 0; y < Math.Abs(this.BitmapHeader.Height); y++)
                            {
                                RTStreamEndianUtils.GetBytes(stream, ref byteN);
                                int pos = 0;
                                for (x = 0; x < this.BitmapHeader.Width; x +=2)
                                {
                                    b = byteN[pos];
                                    p = ((int)b & 0x000000F0) >> 4;
                                    this.Pixels[y, x] = Palette.getEntry(p);
                                    x1 = x + 1;
                                    if (x1 < this.BitmapHeader.Width)
                                    {
                                        p = (int)b & 0x0000000F;
                                        this.Pixels[y, x1] = Palette.getEntry(p);
                                        ++pos;
                                    }
                                }
                            }
                            break;
                        }
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.RLE_4)
                        {
                            DecodeRLE4(stream, ref this.Pixels, ref this.Palette);
                            break;
                        }
                        else
                        {
                            throw new Exception("This compression of 8 bits BMP is not supported!");
                        }
                        //break;
                    case BitmapBitsPerPixel.Palette_256:
                         if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None)
                         {
                            for (int y = 0; y < Math.Abs(this.BitmapHeader.Height); y++)
                            {
                                for (int x = 0; x < this.BitmapHeader.Width; x++)
                                {
                                    b = (byte)RTStreamEndianUtils.GetByte(stream);
                                    this.Pixels[y, x] = Palette.getEntry(b);
                                }
                                for (int i = 0; i < pad; i++)
                                    b = (byte)RTStreamEndianUtils.GetByte(stream);
                             }
                             break;
                        }
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.RLE_8)
                        {
                            DecodeRLE8(stream, ref this.Pixels, ref this.Palette);
                            break;
                        }
                        else
                        {
                            throw new Exception("This compression of 8 bits BMP is not supported!");
                        }
                        //break;
                    case BitmapBitsPerPixel.RGB_16:
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.BitFields ||
                            (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None))
                        {
                            if ((this.BitmapHeader.MaskRed == (uint)0) && (this.BitmapHeader.MaskGreen == (uint)0) &&
								(this.BitmapHeader.MaskBlue == (uint)0) && (this.BitmapHeader.MaskAlpha == (uint)0))
							{
                                this.BitmapHeader.MaskRed = (uint)0x00007C00;   /* 0000 0000 0000 0000 0111 1100 0000 0000 */
                                this.BitmapHeader.MaskGreen = (uint)0x000003E0; /* 0000 0000 0000 0000 0000 0011 1110 0000 */
                                this.BitmapHeader.MaskBlue = (uint)0x0000001F;  /* 0000 0000 0000 0000 0000 0000 0001 1111 */
								this.BitmapHeader.MaskAlpha = (uint)0x00000000; /* 0000 0000 0000 0000 0000 0000 0000 0000 */
							}
                   
							uint pixel = 0;
							int shiftRed = 0;
							int shiftGreen = 0;
							int shiftBlue = 0;
							int shiftAlpha = 0;
							int quantumRed = 0;
							int quantumGreen = 0;
							int quantumBlue = 0;
							int quantumAlpha = 0;
							int Red = 0;
							int Green = 0;
							int Blue = 0;
							int Alpha = 0;
							
							// Calculate shift bits info from bitfield masks
							if (this.BitmapHeader.MaskRed != 0)
								while (((this.BitmapHeader.MaskRed << shiftRed) & 0x80000000UL) == 0)
									shiftRed++;
							if (this.BitmapHeader.MaskGreen != 0)
								while (((this.BitmapHeader.MaskGreen << shiftGreen) & 0x80000000UL) == 0)
									shiftGreen++;
							if (this.BitmapHeader.MaskBlue != 0)
								while (((this.BitmapHeader.MaskBlue << shiftBlue) & 0x80000000UL) == 0)
									shiftBlue++;
							if (this.BitmapHeader.MaskAlpha != 0)
								while (((this.BitmapHeader.MaskAlpha << shiftAlpha) & 0x80000000UL) == 0)
									shiftAlpha++;
                            // Calculate quantum bits info from bitfield masks	
							quantumRed = shiftRed;
							while (((this.BitmapHeader.MaskRed << quantumRed) & 0x80000000UL) != 0)
								quantumRed++;
							quantumRed = quantumRed - shiftRed;
							quantumGreen = shiftGreen;
							while (((this.BitmapHeader.MaskGreen << quantumGreen) & 0x80000000UL) != 0)
								quantumGreen++;
							quantumGreen = quantumGreen - shiftGreen;
							quantumBlue = shiftBlue;
							while (((this.BitmapHeader.MaskBlue << quantumBlue) & 0x80000000UL) != 0)
								quantumBlue++;
							quantumBlue = quantumBlue - shiftBlue;
							quantumAlpha = shiftAlpha;
							while (((this.BitmapHeader.MaskAlpha << quantumAlpha) & 0x80000000UL) != 0)
								quantumAlpha++;
							quantumAlpha = quantumAlpha - shiftAlpha;
							
							// Read the pixels
							for (int y = 0; y < Math.Abs(this.BitmapHeader.Height); y++)
                            {
								for (int x = 0; x < this.BitmapHeader.Width; x++)
                                {
									pixel = (uint)RTStreamEndianUtils.GetWord16_LE(stream);
									
									Red = (int)((pixel & this.BitmapHeader.MaskRed) << shiftRed) >> 16;
									if (quantumRed == 5)
										Red |= ((Red & 0xE000) >> 5);
									if (quantumRed <= 8)
										Red |= ((Red & 0xFF00) >> 8);
									this.Pixels[y, x].Red = (byte)Red;
									Green = (int)((pixel & this.BitmapHeader.MaskGreen) << shiftGreen) >> 16;
									if (quantumGreen == 5)
										Green |= ((Green & 0xE000) >> 5);
									if (quantumGreen == 6)
										Green |= ((Green & 0xC000) >> 6);
									if (quantumRed <= 8)
										Green |= ((Green & 0xFF00) >> 8);
									this.Pixels[y, x].Green = (byte)Green;
									Blue = (int)((pixel & this.BitmapHeader.MaskBlue) << shiftBlue) >> 16;
									if (quantumBlue == 5)
										Blue |= ((Blue & 0xE000) >> 5);
									if (quantumBlue <= 8)
										Blue |= ((Blue & 0xFF00) >> 8);
									this.Pixels[y, x].Blue = (byte)Blue;
									Alpha = (int)((pixel & this.BitmapHeader.MaskAlpha) << shiftAlpha) >> 16;
									if (quantumAlpha <= 8)
										Alpha |= ((Alpha & 0xFF00) >> 8);
									this.Pixels[y, x].Alpha = (byte)Alpha;
                                }
								
                                for (int i = 0; i < pad; i++)
                                    b = (byte)RTStreamEndianUtils.GetByte(stream);
                            }
							break;
                        }
                        else
                        {
                            throw new Exception("This compression of 16 bits BMP is not supported!");
                        }
                        //break;
                    case BitmapBitsPerPixel.RGB_24:
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None)
                        {
                            for (int y = 0; y < Math.Abs(this.BitmapHeader.Height); y++)
                            {
                                for (int x = 0; x < this.BitmapHeader.Width; x++)
                                {
                                    this.Pixels[y, x].Blue = (byte)RTStreamEndianUtils.GetByte(stream);
                                    this.Pixels[y, x].Green = (byte)RTStreamEndianUtils.GetByte(stream);
                                    this.Pixels[y, x].Red = (byte)RTStreamEndianUtils.GetByte(stream);
                                    this.Pixels[y, x].Alpha = (byte)0;
                                }
                                for (int i = 0; i < pad; i++)
                                    b = (byte)RTStreamEndianUtils.GetByte(stream);
                             }
                             break;
                        }
                        else
                        {
                            throw new Exception("This compression of 24 bits BMP is not supported!");
                        }
                        //break;
                    case BitmapBitsPerPixel.RGB_32:
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.None)
                        {
                            for (int y = 0; y < Math.Abs(this.BitmapHeader.Height); y++)
                            {
                                for (int x = 0; x < this.BitmapHeader.Width; x++)
                                {
                                    this.Pixels[y, x].Blue = (byte)RTStreamEndianUtils.GetByte(stream);
                                    this.Pixels[y, x].Green = (byte)RTStreamEndianUtils.GetByte(stream);
                                    this.Pixels[y, x].Red = (byte)RTStreamEndianUtils.GetByte(stream);
                                    this.Pixels[y, x].Alpha = (byte)RTStreamEndianUtils.GetByte(stream);
                                    // Some transparent bitmaps are badly identifyed
                                    if ((this.BitmapHeader.Compression == (uint)BitmapCompressionType.None) && (this.Pixels[y, x].Alpha != 0))
                                    {
                                        this.BitmapHeader.Compression = (uint)BitmapCompressionType.BitFields;
                                        this.BitmapHeader.MaskRed =     (uint)0x00FF0000; /* 0000 0000 1111 1111 0000 0000 0000 0000 */
                                        this.BitmapHeader.MaskGreen =   (uint)0x0000FF00; /* 0000 0000 0000 0000 1111 1111 0000 0000 */
                                        this.BitmapHeader.MaskBlue =    (uint)0x000000FF; /* 0000 0000 0000 0000 0000 0000 1111 1111 */
                                        this.BitmapHeader.MaskAlpha =   (uint)0xFF000000; /* 1111 1111 0000 0000 0000 0000 0000 0000 */
                                    }
                                }
                                for (int i = 0; i < pad; i++)
                                    b = (byte)RTStreamEndianUtils.GetByte(stream);
                             }
                             break;
                        }
                        if (this.BitmapHeader.Compression == (uint)BitmapCompressionType.BitFields)
                        {
                            if ((this.BitmapHeader.MaskRed == (uint)0) && (this.BitmapHeader.MaskGreen == (uint)0) &&
								(this.BitmapHeader.MaskBlue == (uint)0) && (this.BitmapHeader.MaskAlpha == (uint)0))
							{
                                this.BitmapHeader.MaskRed =   (uint)0x00FF0000; /* 0000 0000 1111 1111 0000 0000 0000 0000 */
                                this.BitmapHeader.MaskGreen = (uint)0x0000FF00; /* 0000 0000 0000 0000 1111 1111 0000 0000 */
                                this.BitmapHeader.MaskBlue =  (uint)0x000000FF; /* 0000 0000 0000 0000 0000 0000 1111 1111 */
								this.BitmapHeader.MaskAlpha = (uint)0x00000000; /* 0000 0000 0000 0000 0000 0000 0000 0000 */
							}
                   
							uint pixel = 0;
							int shiftRed = 0;
							int shiftGreen = 0;
							int shiftBlue = 0;
							int shiftAlpha = 0;
							int quantumRed = 0;
							int quantumGreen = 0;
							int quantumBlue = 0;
							int quantumAlpha = 0;
							int Red = 0;
							int Green = 0;
							int Blue = 0;
							int Alpha = 0;
							
							// Calculate shift and quantum bits info from bitfield masks
							if (this.BitmapHeader.MaskRed != 0)
								while (((this.BitmapHeader.MaskRed << shiftRed) & 0x80000000UL) == 0)
									shiftRed++;
							if (this.BitmapHeader.MaskGreen != 0)
								while (((this.BitmapHeader.MaskGreen << shiftGreen) & 0x80000000UL) == 0)
									shiftGreen++;
							if (this.BitmapHeader.MaskBlue != 0)
								while (((this.BitmapHeader.MaskBlue << shiftBlue) & 0x80000000UL) == 0)
									shiftBlue++;
							if (this.BitmapHeader.MaskAlpha != 0)
								while (((this.BitmapHeader.MaskAlpha << shiftAlpha) & 0x80000000UL) == 0)
									shiftAlpha++;
								
							quantumRed = shiftRed;
							while (((this.BitmapHeader.MaskRed << quantumRed) & 0x80000000UL) != 0)
								quantumRed++;
							quantumRed = quantumRed - shiftRed;
							quantumGreen = shiftGreen;
							while (((this.BitmapHeader.MaskGreen << quantumGreen) & 0x80000000UL) != 0)
								quantumGreen++;
							quantumGreen = quantumGreen - shiftGreen;
							quantumBlue = shiftBlue;
							while (((this.BitmapHeader.MaskBlue << quantumBlue) & 0x80000000UL) != 0)
								quantumBlue++;
							quantumBlue = quantumBlue - shiftBlue;
							quantumAlpha = shiftAlpha;
							while (((this.BitmapHeader.MaskAlpha << quantumAlpha) & 0x80000000UL) != 0)
								quantumAlpha++;
							quantumAlpha = quantumAlpha - shiftAlpha;
							
							// Read the pixels
							for (int y = 0; y < Math.Abs(this.BitmapHeader.Height); y++)
                            {
								for (int x = 0; x < this.BitmapHeader.Width; x++)
                                {
									pixel = (uint)RTStreamEndianUtils.GetWord32_LE(stream);

                                    Red = (int)((pixel & this.BitmapHeader.MaskRed) << shiftRed) >> 16;
									if (quantumRed == 8)
										Red |= (Red >> 8);
									this.Pixels[y, x].Red = (byte)Red;
                                    Green = (int)((pixel & this.BitmapHeader.MaskGreen) << shiftGreen) >> 16;
									if (quantumGreen == 8)
										Green |= (Green >> 8);
									this.Pixels[y, x].Green = (byte)Green;
                                    Blue = (int)((pixel & this.BitmapHeader.MaskBlue) << shiftBlue) >> 16;
									if (quantumBlue == 8)
										Blue |= (Blue >> 8);
									this.Pixels[y, x].Blue = (byte)Blue;
                                    Alpha = (int)((pixel & this.BitmapHeader.MaskAlpha) << shiftAlpha) >> 16;
									if (quantumAlpha == 8)
										Alpha |= (Alpha >> 8);
									this.Pixels[y, x].Alpha = (byte)Alpha;
                                }
								
                                for (int i = 0; i < pad; i++)
                                    b = (byte)RTStreamEndianUtils.GetByte(stream);
                            }
							break;
                        }
                        else
                        {
                            throw new Exception("This compression of 32 bits BMP is not supported!");
                        }
                        //break;
                    default:
                        // This should never happen.
                        throw new Exception("Unknown Bits per Pixel!");
                        //break;
                }

                // Loads the ICM profile for packed DIBs
                if ((Palette.NumberOfEntries == 0) && (this.BitmapHeader.ProfileSize > 0))
                {
                    // This is wrong? Its more logical to be before the pixels...
                    RTStreamEndianUtils.GetBytes(stream, ref ProfileICM);
                }

                // Close stream
                stream.Close();
                OpenForReadding = true;
            }
            catch
            {
                throw;
            }
        }

        public void Save(string file)
        {
            if (this.OpenForReadding)
                return;

            try
            {
                FileStream stream = new FileStream(file, FileMode.Create, FileAccess.Write);

                // Bitmap v5 Header
                int sizeOfPixel = (int)this.BitmapHeader.BitsPerPixel / 8;
                if (sizeOfPixel < 1)
                    sizeOfPixel = 1;

                // Top-down BMP, flip it
                if (this.BitmapHeader.Height < 0)
                    this.BitmapHeader.Height = -this.BitmapHeader.Height;

                switch ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel)
                {
                    case BitmapBitsPerPixel.JPEG_PNG:
                        // No info for this format
                        throw new Exception("JPEG and PNG compressed BMP is not supported!");
                        //break;
                    case BitmapBitsPerPixel.MonoChrome:
                    case BitmapBitsPerPixel.Palette_16:
                    case BitmapBitsPerPixel.Palette_256:
                    case BitmapBitsPerPixel.RGB_24:
                        this.BitmapHeader.ImageSize = (uint)(4 * (((this.BitmapHeader.Width * 
                                                      this.BitmapHeader.BitsPerPixel) + 31) / 32) *
                                                      this.BitmapHeader.Height);
                        break;
                    case BitmapBitsPerPixel.RGB_16:
                        if (this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V3)
                            this.BitmapHeader.ImageSize = (uint)(2 * ((this.BitmapHeader.Width % 2) + this.BitmapHeader.Width) *
                                                       this.BitmapHeader.Height);
                        else
                            this.BitmapHeader.ImageSize = (uint)(4 * this.BitmapHeader.Width * this.BitmapHeader.Height);
                        break;
                    case BitmapBitsPerPixel.RGB_32:
                        this.BitmapHeader.ImageSize = (uint)(4 * this.BitmapHeader.Width * this.BitmapHeader.Height);
                        break;
                    default:
                        // This should never happen.
                        throw new Exception("Unknown Bits per Pixel!");
                        //break;
                }  

                this.BitmapHeader.PaletteColors = this.Palette.NumberOfEntries;
                this.BitmapHeader.PaletteImportant = this.Palette.NumberOfEntries;

                int pad = ((int)this.BitmapHeader.ImageSize / this.BitmapHeader.Height) - (sizeOfPixel * this.BitmapHeader.Width);
                if (pad < 0)
                    pad = 0;
                                
                // Bitmap File Header
                if ((this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V1) ||
                    (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V2))
                {
                    // Size of File Header + DIB Header + Palette v2
                    this.BitmapFileHeader.PixelsOffset = (uint)BMP_FILE_HEADER_SIZE_V2 + this.BitmapHeader.HeaderSize +
                                                        ((uint)this.BitmapHeader.PaletteColors * (uint)BMP_PALETTE_SIZE_V2);
                }
                else
                {
                    // Size of File Header + DIB Header + Palette v3
                    this.BitmapFileHeader.PixelsOffset = (uint)BMP_FILE_HEADER_SIZE_V2 + this.BitmapHeader.HeaderSize +
                                                        ((uint)this.BitmapHeader.PaletteColors * (uint)BMP_PALETTE_SIZE_V3);
                    // Size of 3 RGB Masks
                    if ((this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V3) &&
                        ((BitmapCompressionType)this.BitmapHeader.Compression == BitmapCompressionType.BitFields) &&
                        (((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_16) ||
                        ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_32)))
                    {
                        this.BitmapFileHeader.PixelsOffset += sizeof(uint) * 3;
                    }
                    // Size of ICM Profile
                    if ((this.BitmapHeader.ProfileSize > 0) &&
                        (this.BitmapHeader.BitsPerPixel < 16))
                    {
                        this.BitmapHeader.ProfileOffset = this.BitmapFileHeader.PixelsOffset;
                        this.BitmapFileHeader.PixelsOffset += this.BitmapHeader.ProfileSize;
                        this.BitmapFileHeader.FileSize = (uint)(BMP_FILE_HEADER_SIZE_V2 + this.BitmapHeader.HeaderSize +
                                                         this.BitmapHeader.ImageSize);
                    }
                    else
                    {
                        this.BitmapFileHeader.FileSize = (uint)(BMP_FILE_HEADER_SIZE_V2 + this.BitmapHeader.HeaderSize +
                                                         this.BitmapHeader.ImageSize);
                        this.BitmapHeader.ProfileOffset = this.BitmapFileHeader.PixelsOffset;
                        this.BitmapFileHeader.FileSize += this.BitmapHeader.ProfileSize;
                    }
                }

                // Saves the bitmap file Header
                RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapFileHeader.Magic);
                RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapFileHeader.FileSize);
                RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapFileHeader.Reserved1);
                RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapFileHeader.Reserved2);
                RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapFileHeader.PixelsOffset);
                
                // Saves the bitmap DIB Header
                if (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V1)
                {
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.HeaderSize);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)(ushort)this.BitmapHeader.Width);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)(ushort)this.BitmapHeader.Height);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)(ushort)this.BitmapHeader.Planes);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)(ushort)this.BitmapHeader.BitsPerPixel);
                }
                if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V3)
                {
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.HeaderSize);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.Width);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.Height);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapHeader.Planes);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapHeader.BitsPerPixel);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.Compression);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ImageSize);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.PixelsPerMeterX);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.PixelsPerMeterY);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.PaletteColors);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.PaletteImportant);
                }
                if (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V2)
                {
                    // Optional fields (OS/2 v2.x specific)
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapHeaderOS2.Units);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapHeaderOS2.Reserved);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapHeaderOS2.Recording);
                    RTStreamEndianUtils.PutWord16_LE(stream, (uint)this.BitmapHeaderOS2.Rendering);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeaderOS2.Size1);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeaderOS2.Size2);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeaderOS2.ColorEncoding);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeaderOS2.Identifier);
                }
                if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V4)
                {
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.MaskRed);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.MaskGreen);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.MaskBlue);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.MaskAlpha);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceType);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Red.X);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Red.Y);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Red.Z);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Green.X);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Green.Y);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Green.Z);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Blue.X);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Blue.Y);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceEndPoints.Blue.Z);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceGammaRed);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceGammaGreen);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ColorSpaceGammaBlue);
                }
                if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V5)
                {
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.RenderingIntent);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ProfileOffset);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.ProfileSize);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.Reserved);
                }

                // Saves the palette
                if (Palette.NumberOfEntries > 0)
                {
                    ColorEntryRGBA<T> color;
                    for (int i = 0; i < Palette.NumberOfEntries; i++)
                    {
                        color = Palette.getEntry(i);
                        if ((this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V1) ||
                            (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V2))
                        {
                            RTStreamEndianUtils.PutByte(stream, (uint)color.Blue);
                            RTStreamEndianUtils.PutByte(stream, (uint)color.Green);
                            RTStreamEndianUtils.PutByte(stream, (uint)color.Red);
                        }
                        else
                        {
                            RTStreamEndianUtils.PutByte(stream, (uint)color.Blue);
                            RTStreamEndianUtils.PutByte(stream, (uint)color.Green);
                            RTStreamEndianUtils.PutByte(stream, (uint)color.Red);
                            RTStreamEndianUtils.PutByte(stream, (uint)color.Alpha);
                        }
                    }
                }

                // Saves the colors masks for Header v3
                if ((this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V3) &&
                    ((BitmapCompressionType)this.BitmapHeader.Compression == BitmapCompressionType.BitFields) &&
                    (((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_16) || 
                    ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_32)))
                {
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.MaskRed);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.MaskGreen);
                    RTStreamEndianUtils.PutWord32_LE(stream, (uint)this.BitmapHeader.MaskBlue);
                }
                
                // Saves the ICM profile for paletted DIBs
                if ((Palette.NumberOfEntries > 0) && (this.BitmapHeader.ProfileSize > 0))
                {
                    RTStreamEndianUtils.PutBytes(stream, ref ProfileICM);
                }
                
                // Saves the pixels
                for (int y = this.BitmapHeader.Height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < this.BitmapHeader.Width; x++)
                    {
                        switch ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel)
                        {
                            case BitmapBitsPerPixel.JPEG_PNG:
                                // No info for this format
                                throw new Exception("JPEG and PNG compressed BMP is not supported!");
                                //break;
                            case BitmapBitsPerPixel.MonoChrome:
                                break;
                            case BitmapBitsPerPixel.Palette_16:
                                break;
                            case BitmapBitsPerPixel.Palette_256:
                                int i = this.Palette.getColorIndex(this.Pixels[y, x]);
                                RTStreamEndianUtils.PutByte(stream, (uint)i);
                                break;
                            case BitmapBitsPerPixel.RGB_16:
                                break;
                            case BitmapBitsPerPixel.RGB_24:
                                RTStreamEndianUtils.PutByte(stream, (uint)this.Pixels[y, x].Blue);
                                RTStreamEndianUtils.PutByte(stream, (uint)this.Pixels[y, x].Green);
                                RTStreamEndianUtils.PutByte(stream, (uint)this.Pixels[y, x].Red);
                                break;
                            case BitmapBitsPerPixel.RGB_32:
                                RTStreamEndianUtils.PutByte(stream, (uint)this.Pixels[y, x].Blue);
                                RTStreamEndianUtils.PutByte(stream, (uint)this.Pixels[y, x].Green);
                                RTStreamEndianUtils.PutByte(stream, (uint)this.Pixels[y, x].Red);
                                RTStreamEndianUtils.PutByte(stream, (uint)this.Pixels[y, x].Alpha);
                                break;
                            default:
                                // This should never happen.
                                throw new Exception("Unknown Bits per Pixel!");
                                //break;
                        }  
                    }
                    for (int i = 0; i < pad; i++)
                        RTStreamEndianUtils.PutByte(stream, 0);
                }

                // Saves the ICM profile for packed DIBs
                if ((Palette.NumberOfEntries == 0) && (this.BitmapHeader.ProfileSize > 0))
                {
                    RTStreamEndianUtils.PutBytes(stream, ref ProfileICM);
                }

                // Close stream
                stream.Flush();
                stream.Close();
            }
            catch
            {
                throw;
            }
        }

        public override string ToString()
        {
            // Create a StringBuilder that expects to hold 512 characters.
            StringBuilder sb = new StringBuilder("", 512);

            // Bitmap File Header.
            sb.Append("Bitmap File Header:");
            sb.AppendLine();
            sb.AppendFormat("\tMagic:\t\t0x{0:X8} - \"{1}{2}\"", this.BitmapFileHeader.Magic,
                            (char)(this.BitmapFileHeader.Magic & 0x00FF), (char)((this.BitmapFileHeader.Magic & 0xFF00) >> 8));
            sb.AppendLine();
            sb.AppendFormat("\tFile Size:\t{0} Bytes", this.BitmapFileHeader.FileSize);
            sb.AppendLine();
            sb.AppendFormat("\tReserved 1:\t0x{0:X8}", this.BitmapFileHeader.Reserved1);
            sb.AppendLine();
            sb.AppendFormat("\tReserved 2:\t0x{0:X8}", this.BitmapFileHeader.Reserved2);
            sb.AppendLine();
            sb.AppendFormat("\tOffset Bytes:\t{0} Bytes", this.BitmapFileHeader.PixelsOffset);
            sb.AppendLine();
            
            // Bitmap DIB Header
            sb.Append("Bitmap DIB Header:");
            sb.AppendLine();
            uint version = 0;
            switch (this.BitmapHeader.HeaderSize)
            { 
                case BMP_HEADER_OS2_SIZE_V1:
                    version = 2;
                    break;
                case BMP_HEADER_OS2_SIZE_V2:
                case BMP_HEADER_WIN_SIZE_V3:
                    version = 3;
                    break;
                case BMP_HEADER_WIN_SIZE_V4:
                    version = 4;
                    break;
                case BMP_HEADER_WIN_SIZE_V5:
                    version = 5;
                    break;
                default:
                    version = 0;
                    break;
            }
            sb.Append("\t*** Header v2 Members (OS/2 v1.x and Windows 2.x)");
            sb.AppendLine();
            sb.AppendFormat("\tVersion:\t{0} ({1} Bytes)", version, this.BitmapHeader.HeaderSize);
            sb.AppendLine();
            sb.AppendFormat("\tWidth:\t\t{0} pixels", this.BitmapHeader.Width);
            sb.AppendLine();
            sb.AppendFormat("\tHeight:\t\t{0} pixels (", Math.Abs(this.BitmapHeader.Height));
            if (this.BitmapHeader.Height < 0)
                sb.AppendFormat("Top-Down)");
            else
                sb.AppendFormat("Bottom-up)");
            sb.AppendLine();
            sb.AppendFormat("\tPlanes:\t\t{0}", this.BitmapHeader.Planes);
            sb.AppendLine();
            sb.AppendFormat("\tBits Per Pixel:\t{0}", this.BitmapHeader.BitsPerPixel);
            sb.AppendLine();
            
            if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V3)
            {
                sb.Append("\t*** Header v3 Members (Windows 3.x)");
                sb.AppendLine();
                string str = "";
                switch ((BitmapCompressionType)this.BitmapHeader.Compression)
                {
                    case BitmapCompressionType.None:
                        str = "None (Plain RGB)";
                        break;
                    case BitmapCompressionType.RLE_8:
                        str = "RLE 8";
                        break;
                    case BitmapCompressionType.RLE_4:
                        str = "RLE 4";
                        break;
                    case BitmapCompressionType.BitFields:
                        str = "None (RGBA Bit Fields)";
                        break;
                    case BitmapCompressionType.JPEG:
                        str = "JPEG";
                        break;
                    case BitmapCompressionType.PNG:
                        str = "PNG";
                        break;
                    default:
                        str = "Unknow";
                        break;
                }
                sb.AppendFormat("\tCompression:\t\t{0} - {1}", this.BitmapHeader.Compression, str);
                sb.AppendLine();
                sb.AppendFormat("\tImage Size:\t\t{0} Bytes", this.BitmapHeader.ImageSize);
                sb.AppendLine();
                int dpiX = 0;
                int dpiY = 0;
                this.getDPI(out dpiX, out dpiY);
                sb.AppendFormat("\tPixels Per Meter X:\t{0} ({1} DPI)", this.BitmapHeader.PixelsPerMeterX, dpiX);
                sb.AppendLine();
                sb.AppendFormat("\tPixels Per Meter Y:\t{0} ({1} DPI)", this.BitmapHeader.PixelsPerMeterY, dpiY);
                sb.AppendLine();
                sb.AppendFormat("\tPalette Colors:\t\t{0}", this.BitmapHeader.PaletteColors);
                sb.AppendLine();
                sb.AppendFormat("\tPalette Important:\t{0}", this.BitmapHeader.PaletteImportant);
                sb.AppendLine();
            }
            if ((this.BitmapHeader.HeaderSize == BMP_HEADER_WIN_SIZE_V3) &&
                    ((BitmapCompressionType)this.BitmapHeader.Compression == BitmapCompressionType.BitFields) &&
                    (((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_16) ||
                    ((BitmapBitsPerPixel)this.BitmapHeader.BitsPerPixel == BitmapBitsPerPixel.RGB_32)))
            {
                sb.AppendFormat("\tMask Red:\t0x{0:X8}", this.BitmapHeader.MaskRed);
                sb.AppendLine();
                sb.AppendFormat("\tMask Green:\t0x{0:X8}", this.BitmapHeader.MaskGreen);
                sb.AppendLine();
                sb.AppendFormat("\tMask Blue:\t0x{0:X8}", this.BitmapHeader.MaskBlue);
                sb.AppendLine();
            }
            if (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V2)
            {
                // Optional fields (OS/2 v2.x spcific)
                sb.Append("\tOS/2 v2.x Extra Members:");
                sb.AppendLine();
                sb.AppendFormat("\t\tUnits (0=pixels per meter):\t0x{0:X4}", this.BitmapHeaderOS2.Units);
                sb.AppendLine();
                sb.AppendFormat("\t\tReserved:\t0x{0:X4}", this.BitmapHeaderOS2.Reserved);
                sb.AppendLine();
                sb.AppendFormat("\t\tRecording:\t0x{0:X4}", this.BitmapHeaderOS2.Recording);
                sb.AppendLine();
                sb.AppendFormat("\t\tRendering:\t0x{0:X4}", this.BitmapHeaderOS2.Rendering);
                sb.AppendLine();
                sb.AppendFormat("\t\tSize 1:\t{0} (0x{0:X8})", this.BitmapHeaderOS2.Size1);
                sb.AppendLine();
                sb.AppendFormat("\t\tSize 2:\t{0} (0x{0:X8})", this.BitmapHeaderOS2.Size2);
                sb.AppendLine();
                sb.AppendFormat("\t\tColor Encoding (0=RGB):\t0x{0:X8}", this.BitmapHeaderOS2.ColorEncoding);
                sb.AppendLine();
                sb.AppendFormat("\t\tIdentifier:\t0x{0:X8}", this.BitmapHeaderOS2.Identifier);
                sb.AppendLine();
            }
            if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V4)
            {
                sb.Append("\t*** Header v4 Members (Windows 95/98/ME and Windows NT 4 or above)");
                sb.AppendLine();
                sb.AppendFormat("\tMask Red:\t\t0x{0:X8}", this.BitmapHeader.MaskRed);
                sb.AppendLine();
                sb.AppendFormat("\tMask Green:\t\t0x{0:X8}", this.BitmapHeader.MaskGreen);
                sb.AppendLine();
                sb.AppendFormat("\tMask Blue:\t\t0x{0:X8}", this.BitmapHeader.MaskBlue);
                sb.AppendLine();
                sb.AppendFormat("\tMask Alpha:\t\t0x{0:X8}", this.BitmapHeader.MaskAlpha);
                sb.AppendLine();
                string str = "";
                switch ((BitmapColorSpaceType)this.BitmapHeader.ColorSpaceType)
                {
                    case BitmapColorSpaceType.Calibrated_RBG:
                        str = "Calibrated RBG";
                        break;
                    case BitmapColorSpaceType.sRGB:
                        str = "sRGB";
                        break;
                    case BitmapColorSpaceType.WindowsColorSpace:
                        str = "System Default";
                        break;
                    case BitmapColorSpaceType.ProfileEmbedded:
                        str = "Prolile Embedded";
                        break;
                    case BitmapColorSpaceType.ProfileLinked:
                        str = "Prolile Linked";
                        break;
                    default:
                        str = "Unknow";
                        break;
                }
                sb.AppendFormat("\tColor Space Type:\t{0} - {1}", this.BitmapHeader.ColorSpaceType, str);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Red X:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Red.X);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Red Y:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Red.Y);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Red Z:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Red.Z);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Green X:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Green.X);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Green Y:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Green.Y);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Green Z:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Green.Z);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Blue X:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Blue.X);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Blue Y:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Blue.Y);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Blue Z:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceEndPoints.Blue.Z);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Red Gamma:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceGammaRed);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Green Gamma:{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceGammaGreen);
                sb.AppendLine();
                sb.AppendFormat("\tColor Space Blue Gamma:\t{0} (0x{0:X8})", this.BitmapHeader.ColorSpaceGammaBlue);
                sb.AppendLine();
                sb.AppendFormat("\t\tComputed Gamma (check):\t{0}", this.getGama());
                sb.AppendLine();
            }
            if (this.BitmapHeader.HeaderSize >= BMP_HEADER_WIN_SIZE_V5)
            {
                sb.Append("\t*** Header v5 Members (Windows 98/ME and Windows 2000 or above)");
                sb.AppendLine();
                string str = "";
                switch ((BitmapIntentV5)this.BitmapHeader.RenderingIntent)
                {
                    case BitmapIntentV5.Business:
                        str = "Business";
                        break;
                    case BitmapIntentV5.Graphics:
                        str = "Graphics";
                        break;
                    case BitmapIntentV5.Images:
                        str = "Images";
                        break;
                    case BitmapIntentV5.ABS_ColoriMetric:
                        str = "ABS ColoriMetric";
                        break;
                    default:
                        str = "Unknow";
                        break;
                }
                sb.AppendFormat("\tRendering Intent:\t{0} - {1}", this.BitmapHeader.RenderingIntent, str);
                sb.AppendLine();
                sb.AppendFormat("\tProfile Data Offset:\t{0} Bytes", this.BitmapHeader.ProfileOffset);
                sb.AppendLine();
                sb.AppendFormat("\tProfile Size:\t\t{0} Bytes", this.BitmapHeader.ProfileSize);
                sb.AppendLine();
                sb.AppendFormat("\tReserved:\t\t0x{0:X8}", this.BitmapHeader.Reserved);
                sb.AppendLine();
                if (this.BitmapHeader.ProfileSize > 0)
                {
                    sb.AppendFormat("\t\tProfile:\t");
                    foreach (Byte b in ProfileICM)
                    {
                        if (this.BitmapHeader.ColorSpaceType == (uint)BitmapColorSpaceType.ProfileEmbedded)
                        {
                            sb.AppendFormat("{0} ", b.ToString());
                        }
                        else
                        {
                            // BitmapColorSpaceType.ProfileLinked
                            sb.AppendFormat("{0}", (char)b);
                        }
                    }
                    sb.AppendLine();
                }
            }

            // Palette
            if (Palette.NumberOfEntries > 0)
            {
                sb.Append("Palette:");
                sb.AppendLine();
                ColorEntryRGBA<T> color;
                for (int i = 0; i < Palette.NumberOfEntries; i++)
                {
                    color = Palette.getEntry(i);
                    if ((this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V1) ||
                        (this.BitmapHeader.HeaderSize == BMP_HEADER_OS2_SIZE_V2))
                    {
                        sb.AppendFormat("\tColor[{0,3}] - R={1,3} G={2,3} B={3,3}", i, color.Red, color.Green, color.Blue);
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendFormat("\tColor[{0,3}] - A={1,3} R={2,3} G={3,3} B={4,3}", i, color.Alpha, color.Red, color.Green, color.Blue);
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();   
        }

        /// <description></para></description>
        /// </item>
        /// <item>
        /// <term>RLE_4</term>
        /// <description></description>
        /// </item>
        


        /// <summary>
        /// A run-length encoded (RLE) drcoder for bitmaps with 8 bpp. 
        /// This format may be compressed in either of two modes:
        /// 	- Encoded;
        /// 	- Absolute.
        /// Both modes can occur anywhere throughout a single bitmap.
        /// <para>Encoded mode consists of two bytes:  
        /// the first byte specifies the number of consecutive pixels to be drawn using the color index contained in the second byte.</para></description>
        /// <para>Absolute mode consists of two bytes, where the first byte of the pair is set to 0 and the second byte as this meaning:
        /// 	0 - End of Line;
        /// 	1 - End of Bitmap;
        /// 	2 - Delta: The next 2 bytes contain unsigned values indicating the horizontal and vertical  offset of the next pixel from the current position;
        /// 	3 to 255 - The follow WORD aligned bytes represents the number of bytes which follow, each of which contains the color index of a single pixel.</para>
        ///  <para>Example:
        /// 	Compressed:
        /// 		03 04 
        /// 		05 06 
        /// 		00 03 45 56 67 00 
        /// 		02 78 
        /// 		00 02 05 01 
        /// 		02 78 
        /// 		00 00 
        /// 		09 1E 
        /// 		00 01
        /// 	Expanded:
        /// 		04 04 04 
        /// 		06 06 06 06 06 
        /// 		45 56 67 
        /// 		78 78 
        ///		Move 5 Coluns right and Line down 
        /// 		78 78 
        /// 		End of Line 
        /// 		1E 1E 1E 1E 1E 1E 1E 1E 1E 
        /// 		End of RLE Bitmap</para>
        /// </summary>
        /// <param name="stream">The RLE 8 stream to read from.</param>
        /// <param name="pixels">The color buffer that recives the decoded pixels.</param>
        /// <param name="palette">The palette to use.</param>
        /// <exception cref="System.Exception">
        /// Throws <b>Exception</b> when the palette as more than 256 entries.
        /// </exception>
        public void DecodeRLE8(Stream stream, ref ColorEntryRGBA<T>[,] pixels, ref ColorPalette palette)
        {
            if (pixels == null)
                throw new Exception("Invalid pixels!");
            if (palette == null)
                throw new Exception("Invalid palette!");
            if (palette.NumberOfEntries > 256)
                throw new Exception("The palette has more than 256 colors!");
            
            bool exit = false;
            int x = 0;
            int y = 0;
            int maxX = pixels.GetLength(1);
            int maxY = pixels.GetLength(0);
            long count = 0;
            int byte1;
            int byte2;
            int byte3;
            int byte4;
            ColorEntryRGBA<T> color;
            
            do
            {
                byte1 = (int)RTStreamEndianUtils.GetByte(stream);
                ++count;
                if (stream.Position < stream.Length)
                {
                    byte2 = (int)RTStreamEndianUtils.GetByte(stream);
                    ++count;
                    if (byte1 == 0)
                    {
                        // Encoded mode
                        switch (byte2)
                        {
                            case 0:
                                // End of Line
                                x = 0;
                                ++y;
                                if (y == maxY)
                                {
                                    // Is this a bug?
                                    y = 0;
                                }
                                break;
                            case 1:
                                // End of Bitmap
                                exit = true;
                                break;
                            case 2:
                                // Delta
                                if (stream.Position < stream.Length)
                                {
                                    // Read the number of coluns to shift right
                                    byte3 = (int)RTStreamEndianUtils.GetByte(stream);
                                    ++count;
                                    for (int i = 0; i < (int)byte3; i++)
                                    {
                                        ++x;
                                        if (x == maxX)
                                        {
                                            x = 0;
                                            ++y;
                                        }
                                        if (y == maxY)
                                        {
                                            // Is this a bug?
                                            y = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    throw new IOException("Stream is at EOF (End of File)!");
                                }
                                if (stream.Position < stream.Length)
                                {
                                    // Read the number of lines to shift down
                                    byte4 = (int)RTStreamEndianUtils.GetByte(stream);
                                    ++count;
                                    for (int i = 0; i < (int)byte4; i++)
                                    {
                                        ++y;
                                        if (y == maxY)
                                        {
                                            // Is this a bug?
                                            y = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    throw new IOException("Stream is at EOF (End of File)!");
                                }
                                break;
                            default:
                                // Word16 mode
                                if (stream.Position < stream.Length)
                                {
                                    // If needed, adjust to a Word 16 boundary, to read the extra dummy byte
                                    byte3 = byte2 + (byte2 % 2);
                                    byte[] byteN = new byte[byte3];
                                    RTStreamEndianUtils.GetBytes(stream, ref byteN);
                                    count += byte3;
                                    for (int i = 0; i < (int)byte2; i++)
                                    {
                                        color = palette.getEntry((int)byteN[i]);
                                        //pixels[maxY - y - 1, x] = color;
                                        pixels[y, x] = color;
                                        ++x;
                                        if (x == maxX)
                                            x = 0;
                                    }
                                    break;
                                }
                                else
                                {
                                    throw new IOException("Stream is at EOF (End of File)!");
                                }
                                //break;
                        }
                    }
                    else
                    {
                        // Absolute mode
                        for (int i = 0; i < (int)byte1; i++)
                        {
                            color = palette.getEntry((int)byte2);
                            //pixels[maxY - y - 1, x] = color;
                            pixels[y, x] = color;
                            ++x;
                            if (x == maxX)
                                x = 0;
                        }
                    }
                }
                else
                {
                    throw new System.IO.IOException("Stream is at EOF (End of File)!");
                }
            } while (exit == false);
        }

        /// <summary>
        /// A run-length encoded (RLE) encoder for bitmaps with 8 bpp. 
        /// </summary>
        /// <param name="stream">The RLE 8 stream to write to.</param>
        /// <param name="pixels">The color buffer olds the original pixels.</param>
        /// <param name="palette">The palette to use.</param>
        /// <exception cref="System.Exception">
        /// Throws <b>Exception</b> when the palette as more than 256 entries.
        /// </exception>
        public void EncodeRLE8(Stream stream, ref ColorEntryRGBA<T>[,] pixels, ref ColorPalette palette)
        {
            if (pixels == null)
                throw new Exception("Invalid pixels!");
            if (palette == null)
                throw new Exception("Invalid palette!");
			if (palette.NumberOfEntries > 256)
                throw new Exception("The palette has more than 256 colors!");
            throw new Exception("RLE 8 encoding not implented!");
        }

        /// <summary>
        /// A run-length encoded (RLE) decoder for bitmaps with 4 bpp. 
        /// The compression format is a 2-byte format consisting of a count byte followed by two 4 bits color indexes.
        /// <para>In encoded mode, the  high-order nibble (that is, its low-order four bits) and one in its low-order nibble. 
        /// The first of the pixels is drawn using the color specified by the high-order nibble, the second is drawn using the color in the low-order nibble.</para>
        /// <para>In absolute mode, the first byte contains zero, the second byte contains 
        /// the number of color indexes that follow, and subsequent bytes contain 
        /// color indexes in their high- and low-order nibbles, one color index for 
        /// each pixel. Each run must be aligned on a word boundary. The end-of-line, end-of-bitmap, and delta escapes also apply to.</para>
        /// <para>Example:
        /// 	Compressed:
        /// 		03 04 
        /// 		05 06 
        /// 		00 06 45 56 67 00 
        /// 		04 78
        /// 		00 02 05 01 
        /// 		04 78 
        /// 		00 00 
        /// 		09 1E 
        /// 		00 01
        /// 	Expanded:
        /// 		0 4 0 
        /// 		0 6 0 6 0 
        /// 		4 5 5 6 6 7 
        /// 		7 8 7 8  
        ///		Move 5 Coluns right and Line down 
        /// 		7 8 7 8
        /// 		End of Line 
        /// 		1 E 1 E 1 E 1 E 1
        /// 		End of RLE Bitmap</para>
        /// </summary>
        /// <param name="stream">The RLE 4 stream to read from.</param>
        /// <param name="pixels">The color buffer that recives the decoded pixels.</param>
        /// <param name="palette">The palette to use.</param>
        /// <exception cref="System.Exception">
        /// Throws <b>Exception</b> when the palette as more than 16 entries.
        /// </exception>
        public void DecodeRLE4(Stream stream, ref ColorEntryRGBA<T>[,] pixels, ref ColorPalette palette)
        {
            if (pixels == null)
                throw new Exception("Invalid pixels!");
            if (palette == null)
                throw new Exception("Invalid palette!");
			if (palette.NumberOfEntries > 16)
                throw new Exception("The palette has more 16 than  colors!");

            bool exit = false;
            int x = 0;
            int y = 0;
            int maxX = pixels.GetLength(1);
            int maxY = pixels.GetLength(0);
            long count = 0;
            int byte1 = 0;
            int byte2 = 0;
            int byte3 = 0;
            int byte4 = 0;
            ColorEntryRGBA<T> color;

            do
            {
                byte1 = (int)RTStreamEndianUtils.GetByte(stream);
                ++count;
                if (stream.Position < stream.Length)
                {
                    byte2 = (int)RTStreamEndianUtils.GetByte(stream);
                    ++count;
                    if (byte1 == 0)
                    {
                        // Encoded mode
                        switch (byte2)
                        {
                            case 0:
                                // End of Line
                                x = 0;
                                ++y;
                                if (y == maxY)
                                {
                                    // Is this a bug?
                                    y = 0;
                                }
                                break;
                            case 1:
                                // End of Bitmap
                                exit = true;
                                break;
                            case 2:
                                // Delta
                                if (stream.Position < stream.Length)
                                {
                                    // Read the number of coluns to shift right
                                    byte3 = (int)RTStreamEndianUtils.GetByte(stream);
                                    ++count;
                                    for (int i = 0; i < (int)byte3; i++)
                                    {
                                        ++x;
                                        if (x == maxX)
                                        {
                                            x = 0;
                                            ++y;
                                        }
                                        if (y == maxY)
                                        {
                                            // Is this a bug?
                                            y = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    throw new IOException("Stream is at EOF (End of File)!");
                                }
                                if (stream.Position < stream.Length)
                                {
                                    // Read the number of lines to shift down
                                    byte4 = (int)RTStreamEndianUtils.GetByte(stream);
                                    ++count;
                                    for (int i = 0; i < (int)byte4; i++)
                                    {
                                        ++y;
                                        if (y == maxY)
                                        {
                                            // Is this a bug?
                                            y = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    throw new IOException("Stream is at EOF (End of File)!");
                                }
                                break;
                            default:
                                // Word16 mode
                                if (stream.Position < stream.Length)
                                {
                                    // If needed, adjust to a Word 16 boundary, to read the extra dummy byte
                                    byte3 = (byte2 / 2) + ((byte2 /2) % 2);
                                    byte[] byteN = new byte[byte3];
                                    RTStreamEndianUtils.GetBytes(stream, ref byteN);
                                    count += byte3;
                                    int pos = 0;
                                    for (int i = 0; i < (int)byte2; ++i)
                                    {
                                        if ((i % 2) == 0)
                                            color = palette.getEntry(((int)byteN[pos] & 0x000000F0) >> 4);
                                        else
                                        {
                                            color = palette.getEntry((int)byteN[pos] & 0x0000000F);
                                            ++pos;
                                        }
                                        //pixels[maxY - y - 1, x] = color;
                                        pixels[y, x] = color;
                                        ++x;
                                        if (x == maxX)
                                            x = 0;
                                    }
                                    break;
                                }
                                else
                                {
                                    throw new IOException("Stream is at EOF (End of File)!");
                                }
                                //break;
                        }
                    }
                    else
                    {
                        // Absolute mode
                        byte3 = (byte2 & 0xF0) >> 4;
                        byte4 = byte2 & 0x0F;
                        for (int i = 0; i < (int)byte1; ++i)
                        {
                            if ((i % 2) == 0)
                                color = palette.getEntry((int)byte3);
                            else
                                color = palette.getEntry((int)byte4);
                            //pixels[maxY - y - 1, x] = color;
                            pixels[y, x] = color;
                            ++x;
                            if (x == maxX)
                                x = 0;
                        }
                    }
                }
                else
                {
                    throw new System.IO.IOException("Stream is at EOF (End of File)!");
                }
            } while (exit == false);
        }

        /// <summary>
        /// A run-length encoded (RLE) encoder for bitmaps with 4 bpp. 
        /// </summary>
        /// <param name="stream">The RLE 4 stream to write to.</param>
        /// <param name="pixels">The color buffer olds the original pixels.</param>
        /// <param name="palette">The palette to use.</param>
        /// <exception cref="System.Exception">
        /// Throws <b>Exception</b> when the palette as more than 16 entries.
        /// </exception>
        public void EncodeRLE4(Stream stream, ref ColorEntryRGBA<T>[,] pixels, ref ColorPalette palette)
        {
            if (pixels == null)
                throw new Exception("Invalid pixels!");
            if (palette == null)
                throw new Exception("Invalid palette!");
			if (palette.NumberOfEntries > 16)
                throw new Exception("The palette has more than 16 colors!");
            throw new Exception("RLE 4 encoding not implented!");
        }
    }
}
