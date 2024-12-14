/**
 * OS2InfoHeaderV2_ExtraFields.cs
 *
 * PURPOSE
 *  This structure represents an IBM OS/2 BMP v2 DIB (Device Independent Bitmap) header structure (OS22XBITMAPHEADER) of a BMP bitmap.
 *   
 *  This structure range size is between 16 and 64 bytes. Common sizes are 16, 40 and 64.
 *   
 *  If a field is not present on the image file, all other fields that follow aren't present to, and 
 *  their values are assumed to be zero (0).
 *   
 *  This structure, only implements the last extra 24 bytes of fields that aren't included and not supported on the Microsoft Windows API and BMP v3 
 *  BITMAPINFOHEADER and newer, 40 bytes fields. So, the BITMAPINFOHEADER plus this structure makes the 64 bytes total of the OS22XBITMAPHEADER structure.
 *  Introduced in IBM's OS/2 2.0.
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
    /// This structure represents an IBM OS/2 BMP v2 DIB (Device Independent Bitmap) header structure (<c>OS22XBITMAPHEADER</c>) of a BMP bitmap.
    /// <para>This structure range size is between 16 and 64 bytes. Common sizes are 16, 40 and 64.</para>
    /// <para>If a field is not present on the image file, all other fields that follow aren't present to, and 
    /// their values are assumed to be zero (0).</para>
    /// <para>
    /// This structure, only implements the last extra 24 bytes of fields that aren't included and not supported on the Microsoft Windows API and BMP v3 
    /// <seealso cref="Win32RgbQuadruple"><c>BITMAPINFOHEADER</c></seealso> and newer, 40 bytes fields. So, the <c>BITMAPINFOHEADER</c> plus this 
    /// structure makes the 64 bytes total of the <c>OS22XBITMAPHEADER</c> structure.
    /// </para>
    /// <para>Introduced in IBM's OS/2 2.0.</para>
    /// <para>Supported by the OS/2 OS API since OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// </summary>
    /// <remarks>
    /// The <c>sizeof(OS2InfoHeaderV2_ExtraFields)</c> returns 24 bytes and is sequential and byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// <para>The color table (if present) must immediately follow this structure and stored in an vector array of 
    /// <seealso cref="Win32RgbQuadruple"/> with most important colors at top, up to the maximum palette size 
    /// dictated by the <seealso cref="BitsPerPixel"/> field.</para>
    /// <para>Each bitmap scan line of the pixels data, must be zero-padded to end on a DWORD (4 bytes) boundary.</para>
    /// <para>
    /// When the bitmap array immediately follows the <c>OS2InfoHeaderV2</c> structure plus the palette array (if needed), it is a packed bitmap.
    /// Packed bitmaps are referenced by a single pointer.
    /// Packed bitmaps require that the <seealso cref="PaletteSize"/> field must 
    /// be either 0 or the actual size of the color table.
    /// </para>
    /// </remarks>
    //[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 24)]
    internal struct OS2InfoHeaderV2_ExtraFields
    {
        // ** Fields - Upgraded from Microsoft Windows BMP v2 and IBM OS/2 BMP v1 DIB headers and compatible with Microsoft Windows BMP v3

        /// <summary>
        /// The size required to store this structure, in bytes. Always 24.
        /// <para>
        /// Also used to determine the version of the BMP DIB, as:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Version</description>
        /// </listheader>
        /// <item>
        /// <term>12</term>
        /// <description>OS/2 BMP DIB v1 (<seealso cref="Win32CoreHeader"><c>OS21XBITMAPHEADER</c></seealso>) or Windows BMP DIB v2 (<seealso cref="Win32CoreHeader"><c>BITMAPCOREHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>16 to 40</term>
        /// <description>OS/2 BMP DIB v2 [compatible with Windows DIB v3] (<seealso cref="Win32InfoHeaderV5"><c>BITMAPINFOHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>42 to 64</term>
        /// <description>OS/2 BMP DIB v2 [not compatible with Windows DIBs] (<seealso cref="OS2InfoHeaderV2_ExtraFields"><c>OS22XBITMAPHEADER</c></seealso>)</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        //public uint Size;

        /// <summary>
        /// Specifies the width of the bitmap image, in pixels.
        /// <para>Must be bigger that zero (<c>Width &gt; 0</c>).</para>
        /// </summary>
        /// <remarks>
        /// On Windows BMP v3 and above: This field is a signed 32-bits integer.
        /// </remarks>
        //public uint Width;

        /// <summary>
        /// Specifies the height of the bitmap image, in pixels.
        /// <para>Must be bigger that zero (<c>Height &gt; 0</c>).</para>
        /// </summary>
        /// <remarks>
        /// On Windows BMP v3 and above: This field is a signed 32-bits integer.
        /// </remarks>
        //public uint Height;

        /// <summary>
        /// The number of color planes for the target device: Always 1.
        /// </summary>
        //public ushort ColorPlanes;

        /// <summary>
        /// The number of bits-per-pixel (bpp) for each pixel in the image data. This value must be one of: 1, 4, 8 or 24.
        /// See <seealso cref="EnumBitsPerPixel"/> and <see cref="Compression"/> for more information.
        /// </summary>
        //public ushort BitsPerPixel;

        // **  Fields - Optional - Added for IBM OS/2 BMP v2 DIB header and compatible with Microsoft Windows BMP v3

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
        /// <description>Uncompressed, uses RGB</description>
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
        /// <term>3 - <see cref="EnumCompression.Huffman1D"/></term>
        /// <description>1-bit Huffman 1D Halftoning (only valid for 1-bpp images)</description>
        /// </item>
        /// <item>
        /// <term>4 - <see cref="EnumCompression.RLE24"/></term>
        /// <description>24-bit RLE (only valid for 24 bpp)</description>
        /// </item>
        /// </list>
        /// </para>
        /// See <seealso cref="BitsPerPixel"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        //public uint Compression;

        /// <summary>
        /// Specifies the size of the image data/pixels (including the zero-pads, if needed), in bytes.
        /// <para>This may be set to 0 for <seealso cref="EnumCompression.RGB"/> bitmaps.</para>
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        //public uint ImageDataSize;

        /// <summary>
        /// Specifies the horizontal resolution, in pixels-per-meter (ppm), of the destination device for the bitmap image.
        /// <para>
        /// If this field is 0, the target device ppm is used.
        /// </para>
        /// <para>
        /// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current output device.
        /// </para>
        /// See also <seealso cref="ResolutionUnit"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        //public uint PixelsPerMeterX;

        /// <summary>
        /// Specifies the vertical resolution, in pixels-per-meter (ppm), of the destination device for the bitmap image.
        /// <para>
        /// If this field is 0, the target device ppm is used.
        /// </para>
        /// <para>
        /// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current output device.
        /// </para>
        /// See also <seealso cref="ResolutionUnit"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        //public uint PixelsPerMeterY;

        /// <summary>
        /// Specifies the number of color indexes in the color palette (the palette size) used by the bitmap image. Most important colors must be at the top.
        /// <para>
        /// If this value is 0 (<c>PaletteSize == 0</c>) and the <see cref="BitsPerPixel"/> field is less than or equal to 8 (<c>BitsPerPixel &lt;= 8</c>), 
        /// the maximum number of colors corresponding to the value of <see cref="BitsPerPixel"/> is used.
        /// </para>
        /// <para>
        /// 24-bpp, by default, don't have a palette, but if one is provided for optimizing the system palette of the paletted devices, 
        /// this field can't be 0 and must be set to the total number of the palette color items.
        /// </para>
        /// See <seealso cref="PaletteImportant"/>, for more information.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        //public uint PaletteSize;

        /// <summary>
        /// Specifies the number of important color indexes from the color palette for displaying the bitmap image.
        /// <para>If this field is 0 (<c>PaletteSize == 0</c>), all colors are required to display the bitmap image correctly.</para>
        /// <para>If this field bigger than <seealso cref="PaletteSize"/> 0 (<c>PaletteImportant > PaletteSize</c>), 
        /// <seealso cref="PaletteSize"/> value is used.</para>
        /// <para>If the bitmap image doesn't contains a pallet, this value is ignored.</para>
        /// See <seealso cref="PaletteSize"/>, for more information.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        //public uint PaletteImportant;

        // ** Fields - Optional - Added exclusive for IBM OS/2 BMP v2 DIB header and not compatible with any Microsoft Windows BMP

        /// <summary>
        /// Indicates the type of units used in the resolution of the image to interpret the values of the <c>OS2InfoHeaderV2.PixelsPerMeterX</c> and
        /// <c>OS2InfoHeaderV2.PixelsPerMeterY</c> fields.
        /// <para>The only valid value is 0, indicating pixels-per-meter.</para>
        /// See <see cref="PixelsPerMeterX"/>,
        /// <see cref="PixelsPerMeterY"/> and 
        /// <see cref="EnumOS2EnumOS2ResolutionUnits.PPM"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public ushort ResolutionUnit;

        /// <summary>
        /// Reserved for future use. Must be set to 0.
        /// <para>Pad structure to 4-byte boundary.</para>
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public ushort Reserved;

        /// <summary>
        /// Specifies how the bitmap scan lines are stored.
        /// <para>The only valid value for this field is 0,
        /// indicating that the bitmap is stored from left to right and from the bottom up,
        /// with the origin being in the lower-left corner of the display.</para>
        /// See <see cref="EnumOS2EnumOS2RecordingAlgorithm.BottomUp"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public ushort RecordingDirection;

        /// <summary>
        /// Specifies the halftoning algorithm used when compressing the bitmap data.
        /// <para>
        /// This value must be one of:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Halftoning Algorithm</description>
        /// </listheader>
        /// <item>
        /// <term>0 - <see cref="EnumOS2Huffman1DHalftoning.NoHalftoning"/></term>
        /// <description>No halftoning algorithm was used</description>
        /// </item>
        /// <item>
        /// <term>1 - <see cref="EnumOS2Huffman1DHalftoning.ErrorDiffusion"/></term>
        /// <description>Error Diffusion</description>
        /// </item>
        /// <item>
        /// <term>2 - <see cref="EnumOS2Huffman1DHalftoning.PANDA"/></term>
        /// <description>Processing Algorithm for Non-coded Document Acquisition (PANDA)</description>
        /// </item>
        /// <item>
        /// <term>3 - <see cref="EnumOS2Huffman1DHalftoning.SuperCircle"/></term>
        /// <description>Super-Circle</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public ushort HalftoningMethod;

        /// <summary>
        /// Reserved field used only by the halftoning algorithm.
        /// <para>If Error Diffusion halftoning is used, this is the error damping as a percentage in the range 0 through 100.
        /// A value of 100% indicates no damping, and a value of 0% indicates that any errors are not diffused.</para>
        /// <para>If PANDA or Super-Circle halftoning is specified, this is the X dimension of the pattern used in pixels.</para>
        /// See <see cref="HalftoningMethod"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public uint HalftoningParameter1;

        /// <summary>
        /// Reserved field used only by the halftoning algorithm.
        /// <para>If Error Diffusion halftoning is used, this field is not used by error diffusion halftoning.</para>
        /// <para>If PANDA or Super-Circle halftoning is specified, this is the Y dimension of the pattern used in pixels.</para>
        /// See <see cref="HalftoningMethod"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public uint HalftoningParameter2;

        /// <summary>
        /// Color model used to describe the bitmap data.
        /// <para>The only valid value is 0, indicating the RGB encoding scheme,  
        /// but 0xFFFFh was reserved for Paletted encoding</para>
        /// See <see cref="EnumOS2EnumOS2ColorEncoding.RGB"/>.
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public uint ColorEncoding;

        /// <summary>
        /// Reserved for application use and may contain an application-specific value.
        /// <para>Normally is set to 0.</para>
        /// </summary>
        /// <remarks>
        /// If this field is not present on the BMP file, 0 is assumed as its value.
        /// </remarks>
        public uint ApplicationIdentifier;

        // ** Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OS2InfoHeaderV2_ExtraFields"/> struct 
        /// with the default values in all the structure fields.
        /// </summary>
        /// <param name="initialize">If set to <c>true</c> the structure is initialized.</param>
        public OS2InfoHeaderV2_ExtraFields(bool initialize=true) : this()
        {
            if (initialize)
            {
                //this.Size = (uint)Marshal.SizeOf(this);
                //this.ColorPlanes = 1;
                //this.Compression = (uint)EnumCompression.RGB;
                this.ResolutionUnit = (ushort)EnumOS2EnumOS2ResolutionUnits.PPM;
                this.RecordingDirection = (ushort)EnumOS2EnumOS2RecordingAlgorithm.BottomUp;
                this.HalftoningMethod = (ushort)EnumOS2Huffman1DHalftoning.NoHalftoning;
                this.ColorEncoding = (uint)EnumOS2EnumOS2ColorEncoding.RGB;
            }
        }
    }
}
