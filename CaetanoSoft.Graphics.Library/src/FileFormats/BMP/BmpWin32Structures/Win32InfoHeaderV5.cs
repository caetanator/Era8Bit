/**
 * Win32InfoHeaderV5.cs
 *
 * PURPOSE
 *   This structure represents a Microsoft Windows BMP v5 DIB (Device Independent Bitmap) header structure (BITMAPV5HEADER) of a BMP bitmap.
 *   Introduced in Microsoft's Windows 98 and Windows 2000.
 *
 * CONTACTS
 *  For any question or bug report, regarding any portion of the "CaetanoSoft.Graphics.FileFormats.BMP.BmpWin32Structures" project:
 *      https://github.com/caetanator/Era8Bit
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2009-2024 José Caetano Silva
 *
 * HISTORY
 *  2009-09-15: Created.
 *  2017-04-13: Major rewrite.
 *  2023-09-16: Renamed and updated.
 *  2024-12-10: More documentation updates.
 */

using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This structure represents a Microsoft Windows BMP v5 DIB (Device Independent Bitmap) header structure (<c>BITMAPV5HEADER</c>) of a BMP bitmap.
    /// <para>Introduced in Microsoft's Windows 98, Windows 2000 and Windows CE 1.0 (only version 3 and 3.3 for Windows CE 5 or later).</para>
    /// <para>Supported by the Windows OS API since Windows 98 and Windows 2000.</para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd183381(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    /// <remarks>
    /// The <c>sizeof(Win32InfoHeaderV5)</c> returns 124 bytes and is sequential and byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// <para>The color table (if present) must immediately follow this structure and stored in an vector array of 
    /// <seealso cref="Win32RgbQuadruple"/> with most important colors at top, up to the maximum palette size 
    /// dictated by the <seealso cref="BitsPerPixel"/> field.</para>
    /// <para>Each bitmap scan line of the pixels data, must be zero-padded to end on a DWORD (4 bytes) boundary.</para>
    /// <para>
    /// When the bitmap array immediately follows the <c>Win32InfoHeaderV5</c> structure plus the palette array (if needed), it is a packed bitmap.
    /// Packed bitmaps are referenced by a single pointer.
    /// Packed bitmaps require that the <seealso cref="PaletteSize"/> field must 
    /// be either 0 or the actual size of the color table.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 124)]
    internal struct Win32InfoHeaderV5
    {
        // ** Fields upgraded from Microsoft Windows BMP v2 and IBM OS/2 BMP v1 DIB headers

        /// <summary>
        /// The size required to store this structure, in bytes. Always 124.
        /// <para>
        /// Also used to determine the version of the BMP DIB, as:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Version</description>
        /// </listheader>
        /// <item>
        /// <term>12</term>
        /// <description>Windows BMP DIB v2 (<seealso cref="Win32CoreHeader"><c>BITMAPCOREHEADER</c></seealso>) or OS/2 BMP DIB v1 (<seealso cref="Win32CoreHeader"><c>OS21XBITMAPHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>40</term>
        /// <description>Windows BMP DIB v3 (<seealso cref="Win32InfoHeaderV5"><c>BITMAPINFOHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>52</term>
        /// <description>Windows BMP DIB v3.2 [only Windows NT 3.5 and above] (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV2INFOHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>56</term>
        /// <description>Windows BMP DIB v3.3 [only Windows CE 5 and above] (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV3INFOHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>108</term>
        /// <description>Windows BMP DIB v4 (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV4HEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>124</term>
        /// <description>Windows BMP DIB v5 (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV5HEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>16 to 64</term>
        /// <description>OS/2 BMP DIB v2 [only OS/2 2.0 and above] (<seealso cref="OS2InfoHeaderV2_ExtraFields"><c>OS22XBITMAPHEADER</c></seealso>)</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public uint Size;

        /// <summary>
        /// Specifies the width of the bitmap image, in pixels.
        /// <para>Must be bigger that zero (<c>Width &gt; 0</c>).</para>
        /// </summary>
        /// <remarks>
        /// On Windows BMP v4 and above: If <see cref="Compression"/> is <see cref="EnumCompression.JPEG"/> or <see cref="EnumCompression.PNG"/>,
        /// this field specifies the width of the decompressed JPEG or PNG image file, respectively.
        /// </remarks>
        public int Width;

        /// <summary>
        /// Specifies the height of the bitmap image, in pixels.
        /// <para>If this field is positive (<c>Height &gt; 0</c>), the bitmap is a bottom-up DIB and its origin is the lower-left corner.</para>
        /// <para>If this field is negative (<c>Height &lt; 0</c>), the bitmap is a top-down DIB and its origin is the upper-left corner.</para>
        /// <para>This field can't be zero (<c>Height == 0</c>).</para>
        /// <para>Top-down DIBs cannot be compressed: <see cref="Compression"/> must be either <see cref="EnumCompression.RGB"/>, 
        /// <see cref="EnumCompression.BitFields"/> or <see cref="EnumCompression.AlphaBitFields"/>.</para>
        /// </summary>
        /// <remarks>
        /// On Windows BMP v4 and above: If <see cref="Compression"/> is <see cref="EnumCompression.JPEG"/> or <see cref="EnumCompression.PNG"/>,
        /// this field specifies the height of the decompressed JPEG or PNG image file, respectively.
        /// </remarks>
        public int Height;

        /// <summary>
        /// The number of color planes for the target device: Always 1.
        /// </summary>
        public ushort ColorPlanes;

        /// <summary>
        /// The number of bits-per-pixel (bpp) for each pixel in the image data. This value must be one of: 0, 1, 2, 4, 8, 16, 24 or 32. The GDI+ framework supports 64-bpp images, internally.
        /// <para>If this field is 0 (<c>BitsPerPixel == 0</c>), <see cref="Compression"/> must be either <see cref="EnumCompression.JPEG"/> or
        /// <see cref="EnumCompression.PNG"/>.</para>
        /// <para>If this field is 2 (<c>BitsPerPixel == 2</c>), the bitmap is Windows CE 1.0 and above specific and only supported by the 
        /// Windows CE platform APIs.</para>
        /// See <seealso cref="EnumBitsPerPixel"/> and <see cref="Compression"/> for more information.
        /// </summary>
        public ushort BitsPerPixel;

        // ** Fields added for Microsoft Windows BMP v3 DIB header and compatible with IBM OS/2 BMP v2 DIB header

        /// <summary>
        /// Specifies the type of compression scheme used for compressing a bottom-up bitmap image data/pixels (top-down DIBs cannot be compressed).
        /// <para>
        /// This value must be one of:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Compression Scheme</description>
        /// </listheader>
        /// <item>
        /// <term>0 - <see cref="EnumCompression.RGB"/></term>
        /// <description>Uncompressed, uses RGB or RGBA</description>
        /// </item>
        /// <item>
        /// <term>1 - <see cref="EnumCompression.RLE8"/></term>
        /// <description>8-bit RLE (only valid for 8-bpp)</description>
        /// </item>
        /// <item>
        /// <term>2 - <see cref="EnumCompression.RLE4"/></term>
        /// <description>4-bit RLE (only valid for 4-bpp)</description>
        /// </item>
        /// <item>
        /// <term>3 - <see cref="EnumCompression.BitFields"/></term>
        /// <description>Only valid for 16 and 32-bpp bitmap images. On Windows BMP v3, only the Windows NT 3.5 (and above) OS platform APIs supported it
        /// and in this case, three <c>DWORD</c> bitfields masks components (RGB) are given immediately following the end of the <c>BITMAPINFOHEADER</c> 
        /// structure and before the color palette (if needed).</description>
        /// </item>
        /// <item>
        /// <term>4 - <see cref="EnumCompression.JPEG"/></term>
        /// <description>The file contains a JPEG image (only valid for Windows BMP v4 and above)</description>
        /// </item>
        /// <item>
        /// <term>5 - <see cref="EnumCompression.PNG"/></term>
        /// <description>The file contains a PNG image (only valid for Windows BMP v4 and above)</description>
        /// </item>
        /// <item>
        /// <term>6 - <see cref="EnumCompression.AlphaBitFields"/></term>
        /// <description>Only valid for 16 and 32-bpp bitmap images in Windows BMP v3 format for Windows CE 5 (and above). Only this OS platform APIs supported it
        /// and in this case, four <c>DWORD</c> bitfields masks components (RGBA) are given immediately following the end of the <c>BITMAPINFOHEADER</c> 
        /// structure and before the color palette (if needed).</description>
        /// </item>
        /// <item>
        /// <term>11 - <see cref="EnumCompression.CMYK"/></term>
        /// <description>Uncompressed, uses CMYK (only valid on Windows Metafile)</description>
        /// </item>
        /// <item>
        /// <term>12 - <see cref="EnumCompression.CMYK_RLE8"/></term>
        /// <description>8-bit RLE (only valid for 8-bpp on Windows Metafile)</description>
        /// </item>
        /// <item>
        /// <term>13 - <see cref="EnumCompression.CMYK_RLE4"/></term>
        /// <description>4-bit RLE (only valid for 4-bpp on Windows Metafile)</description>
        /// </item>
        /// </list>
        /// </para>
        /// See <seealso cref="BitsPerPixel"/>.
        /// </summary>
        /// <remarks>
        /// For Windows CE 5.0 and later, this value can be <c>OR</c> with <c>BI_SRCPREROTATE</c> (8000h in hexadecimal)
        /// to specify that the source (camera) DIB section has the same rotation angle (Landscape/Portrait) as the destination (screen). 
        /// Used internally by Windows CE APIs, not to be saved on file.
        /// </remarks>
        public uint Compression;

        /// <summary>
        /// Specifies the size of the image data/pixels (including the zero-pads, if needed), in bytes.
        /// <para>This may be set to 0 for <seealso cref="EnumCompression.RGB"/> bitmaps.</para>
        /// </summary>
        /// <remarks>
        /// If <see cref="Compression"/> is <see cref="EnumCompression.JPEG"/> or <see cref="EnumCompression.PNG"/>, 
        /// this field is the size of the JPEG or PNG image buffer.
        /// </remarks>
        public uint ImageDataSize;

        /// <summary>
        /// Specifies the horizontal resolution, in pixels-per-meter (ppm), of the destination device for the bitmap image.
        /// <para>
        /// If this field is 0, the target device ppm is used.
        /// </para>
        /// <para>
        /// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current output device.
        /// </para>
        /// </summary>
        public int PixelsPerMeterX;

        /// <summary>
        /// Specifies the vertical resolution, in pixels-per-meter (ppm), of the destination device for the bitmap image.
        /// <para>
        /// If this field is 0, the target device ppm is used.
        /// </para>
        /// <para>
        /// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current output device.
        /// </para>
        /// </summary>
        public int PixelsPerMeterY;

        /// <summary>
        /// Specifies the number of color indexes in the color palette (the palette size) used by the bitmap image. Most important colors must be at the top.
        /// <para>
        /// If this value is 0 (<c>PaletteSize == 0</c>) and the <see cref="BitsPerPixel"/> field is less than or equal to 8 (<c>BitsPerPixel &lt;= 8</c>), 
        /// the maximum number of colors corresponding to the value of <see cref="BitsPerPixel"/> is used.
        /// </para>
        /// <para>
        /// 16-bpp, 24-bpp and 32-bpp, by default, don't have a palette, but if one is provided for optimizing the system palette of the paletted devices, 
        /// this field can't be 0 and must be set to the total number of the palette color items.
        /// </para>
        /// See <seealso cref="PaletteImportant"/>, for more information.
        /// </summary>
        public uint PaletteSize;

        /// <summary>
        /// Specifies the number of important color indexes from the color palette for displaying the bitmap image.
        /// <para>If this field is 0 (<c>PaletteSize == 0</c>), all colors are required to display the bitmap image correctly.</para>
        /// <para>If this field bigger than <seealso cref="PaletteSize"/> 0 (<c>PaletteImportant > PaletteSize</c>), 
        /// <seealso cref="PaletteSize"/> value is used.</para>
        /// <para>If the bitmap image doesn't contains a pallet, this value is ignored.</para>
        /// See <seealso cref="PaletteSize"/>, for more information.
        /// </summary>
        public uint PaletteImportant;

        // ** Fields added for Microsoft Windows BMP v4 DIB header
        // ** Fields added for Microsoft Windows BMP v3.3 DIB header - Optional Fields Start
        // ** Fields added for Microsoft Windows BMP v3.2 DIB header - Optional Fields Start

        /// <summary>
        /// Specifies the bitfields color mask for the red component of each pixel in bitmap image data.
        /// <para>Only valid if <see cref="Compression"/> is set to 
        /// <see cref="EnumCompression.BitFields"/> or <see cref="EnumCompression.AlphaBitFields"/>.</para>
        /// </summary>
        public uint RedMask;

        /// <summary>
        /// Specifies the bitfields color mask for the green component of each pixel in bitmap image data.
        /// <para>Only valid if <see cref="Compression"/> is set to 
        /// <see cref="EnumCompression.BitFields"/> or <see cref="EnumCompression.AlphaBitFields"/>.</para>
        /// </summary>
        public uint GreenMask;

        /// <summary>
        /// Specifies the bitfields color mask for the blue component of each pixel in bitmap image data.
        /// <para>Only valid if <see cref="Compression"/> is set to 
        /// <see cref="EnumCompression.BitFields"/> or <see cref="EnumCompression.AlphaBitFields"/>.</para>
        /// </summary>
        public uint BlueMask;

        // ** Fields added for Microsoft Windows BMP v3.2 DIB header - Optional Fields End

        /// <summary>
        /// Specifies the bitfields color mask for the alpha/transparency component of each pixel in bitmap image data.
        /// <para>Only valid if <see cref="Compression"/> is set to 
        /// <see cref="EnumCompression.BitFields"/> or <see cref="EnumCompression.AlphaBitFields"/>.</para>
        /// </summary>
        public uint AlphaMask;

        // ** Fields added for Microsoft Windows BMP v3.3 DIB header - Optional Fields End

        /// <summary>
        /// Specifies the color space of the ICC (International Color Consortium) color profile
        /// used by the bitmap image data/pixels. Used for color correction.
        /// <para>
        /// Valid values for Windows BMP v4 are: 0 and the discontinued 1 (to use destination device 
        /// RGB and ignore endpoints and gammas) and 2 (to use destination device 
        /// CMYK and ignore endpoints and gammas). For Windows BMP v5, these values are valid:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Type</description>
        /// </listheader>
        /// <item>
        /// <term>00000000h - <c>LCS_CALIBRATED_RGB</c></term>
        /// <description>Specifies that the bitmap image color values are calibrated RGB values. 
        /// This implies that the values given in fields 
        /// <see cref="Endpoints"/>, <see cref="GammaRed"/>, <see cref="GammaGreen"/> and <see cref="GammaBlue"/>,
        /// are used by the ICM (Independent Color Management) version 2.0 interface to translate them to the destination device 
        /// color space (normally sRGB).</description>
        /// </item>
        /// <item>
        /// <term>73524742h - <c>LCS_sRGB</c></term>
        /// <description>Specifies that the bitmap image color values are calibrated sRGB values. 
        /// This implies that the values given in fields 
        /// <see cref="Endpoints"/>, <see cref="GammaRed"/>, <see cref="GammaGreen"/> and <see cref="GammaBlue"/>,
        /// are ignored and not used, because the image is already calibrated in the sRGB color space, so doesn't
        /// need to be translated before be passed to the destination device.</description>
        /// </item>
        /// <item>
        /// <term>57696E20h - <c>LCS_WINDOWS_COLOR_SPACE</c></term>
        /// <description>Specifies that the bitmap image color values are calibrated on the current Windows OS
        /// color space (normally sRGB). 
        /// This implies that the values given in fields 
        /// <see cref="Endpoints"/>, <see cref="GammaRed"/>, <see cref="GammaGreen"/> and <see cref="GammaBlue"/>,
        /// are ignored and not used, because the image is already calibrated in the sRGB color space, or if not, 
        /// the current Windows color space values are used instead.</description>
        /// </item>
        /// <item>
        /// <term>4C494E4Bh - <c>PROFILE_LINKED</c></term>
        /// <description>Specifies that the bitmap image color values are calibrated on the linked
        /// color space. The <see cref="ProfileDataOffset"/> points to an ASCII string 
        /// (encoded on Code Page 1252) with the size of <see cref="ProfileSize"/> bytes.
        /// This implies that the values given in fields 
        /// <see cref="Endpoints"/>, <see cref="GammaRed"/>, <see cref="GammaGreen"/> and <see cref="GammaBlue"/>,
        /// are ignored and not used, because the linked ICC color space values are used instead.</description>
        /// </item>
        /// <item>
        /// <term>4D424544h - <c>PROFILE_EMBEDDED</c></term>
        /// <description>Specifies that the bitmap image color values are calibrated on the embedded
        /// color space. The <see cref="ProfileDataOffset"/> points to a binary buffer 
        /// with the size of <see cref="ProfileSize"/> bytes.
        /// This implies that the values given in fields 
        /// <see cref="Endpoints"/>, <see cref="GammaRed"/>, <see cref="GammaGreen"/> and <see cref="GammaBlue"/>,
        /// are ignored and not used, because the embedded ICC color space values are used instead.</description>
        /// </item>
        /// </list>
        /// </para>
        /// See also <seealso cref="EnumColorSpace"/>.
        /// </summary>
        public uint ColorSpaceType;

        /// <summary>
        /// A structure that specifies the x, y and z coordinates of the three colors that correspond to the
        /// red, green and blue endpoints for the logical color space associated with the bitmap image data. See <see cref="Win32CieXyzTriple"/>.
        /// <para>
        /// This member is ignored unless the <seealso cref="ColorSpaceType"/> field specifies <see cref="EnumColorSpace.Calibrated_RGB"/>.
        /// </para>
        /// <para>
        /// <b>Note:</b> A color space is a model for representing color numerically in terms of three or more coordinates.
        /// For example, the RGB color space represents colors in terms of the red, green and blue coordinates.
        /// </para>
        /// </summary>
        public Win32CieXyzTriple Endpoints;

        /// <summary>
        /// Tone response curve for red.
        /// <para>
        /// This field is ignored unless the <seealso cref="ColorSpaceType"/> field is <see cref="EnumColorSpace.Calibrated_RGB"/>.
        /// </para>
        /// <para>
		/// The required <c>DWORD</c> bit format for the <c>GammaRed</c> is an 8.8 fixed point integer left-shifted
		/// by 8 bits. This means 8 integer bits are followed by 8 fraction bits. Taking the bit shift into account, the required format
		/// of the 32-bit DWORD is:
		///        0x00000000nnnnnnnnffffffff00000000
        /// </para>
		/// <remarks>Specified in fixed point 16.16 format. The upper 16 bits are the unsigned integer value. The lower 16 bits are the fractional part.</remarks>
		/// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-logcolorspacea">See this Microsoft link for more information.</seealso>
        /// </summary>
        public uint GammaRed;

        /// <summary>
        /// Tone response curve for green.
        /// <para>
        /// This field is ignored unless the <seealso cref="ColorSpaceType"/> field is <see cref="EnumColorSpace.Calibrated_RGB"/>.
        /// </para>
        /// <para>
		/// The required <c>DWORD</c> bit format for the <c>GammaGreen</c> is an 8.8 fixed point integer left-shifted
		/// by 8 bits. This means 8 integer bits are followed by 8 fraction bits. Taking the bit shift into account, the required format
		/// of the 32-bit DWORD is:
		///        0x00000000nnnnnnnnffffffff00000000
        /// </para>
		/// <remarks>Specified in fixed point 16.16 format. The upper 16 bits are the unsigned integer value. The lower 16 bits are the fractional part.</remarks>
		/// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-logcolorspacea">See this Microsoft link for more information.</seealso>
        /// </summary>
        public uint GammaGreen;

        /// <summary>
        /// Tone response curve for blue.
        /// <para>
        /// This field is ignored unless the <seealso cref="ColorSpaceType"/> field is <see cref="EnumColorSpace.Calibrated_RGB"/>.
        /// </para>
        /// <para>
		/// The required <c>DWORD</c> bit format for the <c>GammaBlue</c> is an 8.8 fixed point integer left-shifted
		/// by 8 bits. This means 8 integer bits are followed by 8 fraction bits. Taking the bit shift into account, the required format
		/// of the 32-bit DWORD is:
		///        0x00000000nnnnnnnnffffffff00000000
        /// </para>
		/// <remarks>Specified in fixed point 16.16 format. The upper 16 bits are the unsigned integer value. The lower 16 bits are the fractional part.</remarks>
		/// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-logcolorspacea">See this Microsoft link for more information.</seealso>
        /// </summary>
        public uint GammaBlue;

        // ** Fields added for Microsoft Windows BMP v5 DIB header

        /// <summary>
        /// Rendering intent for the bitmap image data/pixels.
        /// See <see cref="EnumColorSpaceIntent"/>.
        /// <para>
        /// Valid values for Windows BMP v5 are:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Type</description>
        /// </listheader>
        /// <item>
        /// <term>1 - <c>LCS_GM_BUSINESS</c></term>
        /// <description>ICC name: Saturation. Specifies that the bitmap image color values have  
        /// the intent to be used for graphics. Maintains saturation. Used for business charts and other situations in which undithered colors are required.</description>
        /// </item>
        /// <item>
        /// <term>2 - <c>LCS_GM_GRAPHICS</c></term>
        /// <description>ICC name: Relative Colorimetric. Specifies that the bitmap image color values have  
        /// the intent to be used for professional graphic software. Maintains colorimetric match. Used for graphic designs and named colors.</description>
        /// </item>
        /// <item>
        /// <term>4 - <c>LCS_GM_IMAGES</c></term>
        /// <description>ICC name: Perceptual. Specifies that the bitmap image color values have  
        /// the intent to be used for images. Maintains contrast. Used for photographs and natural images.</description>
        /// </item>
        /// <item>
        /// <term>8 - <c>LCS_GM_ABS_COLORIMETRIC</c></term>
        /// <description>ICC name: Absolute Colorimetric. Specifies that the bitmap image color values have  
        /// the intent to be used for absolute match. Maintains the white point. Matches the colors to their nearest color in the destination gamut.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public uint Intent;

        /// <summary>
        /// The offset, in bytes, from the beginning of the <c>Win32InfoHeaderV5</c> structure to the start of the profile data.
        /// This member is ignored unless <c>Win32InfoHeaderV5.ColorSpaceType</c> is set to <see cref="EnumColorSpace.ProfileLinked"/> or
        /// <see cref="EnumColorSpace.ProfileEmbedded"/>.
        /// <para>If the profile is embedded, profile data is the actual ICM 2.0 profile.</para>
        /// <para>If the profile is linked,  profile data is the null-terminated file name of the ICM 2.0 profile or
        /// the fully qualified path (including a network path) of the profile used by the DIB.
        /// It must be composed exclusively of characters from the Windows character set (code page 1252). This cannot be a Unicode string.</para>
        /// </summary>
        public uint ProfileDataOffset;

        /// <summary>
        /// Size, in bytes, of embedded profile data.
        /// This member is ignored unless <c>Win32InfoHeaderV5.ColorSpaceType</c> is set to <see cref="EnumColorSpace.ProfileLinked"/> or
        /// <see cref="EnumColorSpace.ProfileEmbedded"/>.
        /// <para>The profile data (if present) should follow the color table.</para>
        /// <para>For packed DIBs, the profile data should follow the bitmap bits similar to the file format.</para>
        /// </summary>
        public uint ProfileSize;

        /// <summary>
        /// This member has been reserved for future use. Its value must be set to 0.
        /// </summary>
        public uint Reserved;
    }
}
