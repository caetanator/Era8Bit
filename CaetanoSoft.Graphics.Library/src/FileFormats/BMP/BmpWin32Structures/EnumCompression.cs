
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// The compression method used on the Microsoft Windows BMP v2 and IBM OS/2 BMP v1 (and later versions) image DIB or file.
    /// <para>
    /// Defines how the compression type of the image data
    /// in the bitmap file.
    /// </para>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for more information.</seealso>
    /// </summary>
    internal enum EnumCompression : uint
    {
        // ** Values for Microsoft Windows BMP v3 and IBM OS/2 BMP v2

        /// <summary>
        /// Uncompressed format. The pixels are in plain RGB/RGBA.
        /// Same as <c>BI_RGB</c> macro of the Windows 3.0 and above SDK.
        /// <para>
        /// Each image row has a multiple of four elements. If the
        /// row has less elements, zeros will be added at the right side.
        /// The format depends on the number of bits, stored in the info header.
        /// If the number of bits are one, four or eight each pixel data is
        /// a index to the palette. If the number of bits are sixteen,
        /// twenty-four or thirty-two each pixel contains a color.
        /// </para>
        /// <para>Supported by Windows 3.0 and OS/2 2.0 and later.</para>
        /// <para>Implemented on Microsoft Windows BMP v3 and IBM OS/2 BMP v2 format.</para>
        /// </summary>
        RGB = 0,

        /// <summary>
        /// Compressed format using Run-Length Encoded (RLE) 8-bit/pixel. Only for 8 bpp bitmaps.
        /// Same as <c>BI_RLE8</c> macro of the Windows 3.0 and above SDK.
        /// <para>
        /// Two bytes are one data record. If the first byte is not zero, the
        /// next byte will be repeated as much as the value of the first byte.
        /// If the first byte is zero, the record has different meanings, depending
        /// on the second byte. If the second byte is zero, it is the end of the row,
        /// if it is one, it is the end of the image.
        /// </para>
        /// <para>Supported by Windows 2.0 and OS/2 2.0 and later.</para>
        /// <para>Implemented on Microsoft Windows BMP v2 and IBM OS/2 BMP v1 format.</para>
        /// </summary>
        RLE8 = 1,

        /// <summary>
        /// Compressed format using Run-Length Encoded (RLE) 4-bit/pixel. Only for 4 bpp bitmaps.
        /// Same as <c>BI_RLE4</c> macro of the Windows 3.0 and above SDK.
        /// <para>
        /// Two bytes are one data record. If the first byte is not zero, the
        /// next byte will be repeated as much as the value of the first byte.
        /// If the first byte is zero, the record has different meanings, depending
        /// on the second byte. If the second byte is zero, it is the end of the row,
        /// if it is one, it is the end of the image.
        /// </para>
        /// <para>Supported by Windows 2.0 and OS/2 2.0 and later.</para>
        /// <para>Implemented on Microsoft Windows BMP v2 and IBM OS/2 BMP v1 format.</para>
        /// </summary>
        RLE_4 = 2,

        /// <summary>
        /// Uncompressed format. Uses bitfields masks. Same as <c>BI_BITFIELDS</c> macro of the Windows NT 3.5 
        /// and Windows 98 BMP v4 and above SDKs.
        /// For Microsoft Windows NT 3.5 BMP v3.2 and above only, the color table consists of three DWORD color bitfields 
        /// masks that specify the red, green, and blue components, respectively, of each pixel.
        /// <para>
        /// Each image row has a multiple of four elements. If the
        /// row has less elements, zeros will be added at the right side.
        /// </para>
        /// <para>This is valid when used with 16 and 32 bpp bitmaps.</para>
        /// <para>Supported by Windows 98/Windows NT 3.5/Windows CE 2.0 and later.</para>
        /// <para>Implemented on Microsoft Windows BMP v3 for Windows NT 3.5 and Microsoft Windows BMP v4.</para>
        /// </summary>
        BitFields = 3,

        /// <summary>
        /// Compressed format using Huffman 1D. Only for 1 bpp bitmaps. Same as <c>BCA_HUFFMAN1D</c> macro 
        /// of the OS/2 BMP v2 SDK.
        /// <para>
        /// The bitmap contains a Huffman 1D encoded image.
        /// </para>
        /// <para>Supported only by OS/2 v2.0 and later.</para>
        /// <para>Implemented on IBM OS/2 BMP v2.</para>
        /// </summary>
        Huffman_1D = 3,

        /// <summary>
        /// The bitmap contains a JPG compressed image. Same as <c>BI_JPEG</c> macro of the 
        /// Windows 98/2000 and above BMP v4 SDKs.
        /// <para>
        /// The bitmap contains a JPG image.
        /// </para>
        /// <para>Supported by Windows 98/Windows 2000 and later. Only used internally by GDI for 
        /// printers drivers.</para>
        /// <para>Implemented on Microsoft Windows BMP v4.</para>
        /// </summary>
        JPEG = 4,

        /// <summary>
        /// Compressed format using Run-Length Encoded (RLE) 24-bit/pixel. Only for 24 bpp bitmaps.
        /// Same as <c>BCA_RLE24</c> macro of the OS/2 BMP v2 SDK.
        /// <para>
        /// The bitmap contains a RLE 24 encoded image.
        /// </para>
        /// <para>Supported only by OS/2 v2.0 and later.</para>
        /// <para>Implemented on IBM OS/2 BMP v2.</para>
        /// </summary>
        RLE_24 = 4,

        /// <summary>
        /// The bitmap contains a PNG compressed image. Same as <c>BI_PNG</c> macro of the 
        /// Windows 98/2000 and above BMP v4 SDKs.
        /// <para>
        /// The bitmap contains a PNG image.
        /// Not supported at the moment.
        /// </para>
        /// <para>Supported by Windows 98/Windows 2000 and later. Only used internally by GDI for 
        /// printers drivers.</para>
        /// <para>Implemented on Microsoft Windows BMP v4.</para>
        /// </summary>
        PNG = 5,

        /// <summary>
        /// Uncompressed format. Uses bitfields masks. Same as <c>BI_ALPHABITFIELDS</c> macro of the 
        /// Windows CE 5 + .NET 4.0 and above BMP v3.3 SDKs.
        /// For Microsoft Windows CE 5 and later BMP v3 only, the color table consists of four DWORD color bitfield 
        /// masks that specify the red, green, blue, and alpha components, respectively, of each pixel.
        /// 
        /// <para>This is valid when used with 16 and 32 bpp bitmaps on Windows CE only.</para>
        /// <para>Supported by Windows CE 5 + .NET and later.</para>
        /// <para>Implemented on Microsoft Windows BMP v3.</para>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/aa452885.aspx">See this MSDN link for more information.</seealso>
        /// </summary>
        AlphaBitFields = 6,

        // ** Windows Metafile Specific

        /// <summary>
        /// Uncompressed format using CMYK color scheme instead of RGBA.
        /// Same as <c>BI_CMYK</c> macro of the Windows NT 3.1 and Windows 3.0 BMP v3 and above SDKs
        /// <para>Windows Metafile CMYK only.</para>
        /// </summary>
        CMYK = 11,

        /// <summary>
        /// Compressed format using Run-Length Encoded (RLE) 8-bit/pixel. Only for 8 bpp bitmaps using CMYK color scheme instead of RGBA.
        /// Same as <c>BI_CMYKRLE8</c> macro of the Windows NT 3.1 and Windows 3.0 BMP v3 and above SDKs
        /// <para>Windows Metafile CMYK only.</para>
        /// </summary>
        CMYK_RLE8 = 12,

        /// <summary>
        /// Compressed format using Run-Length Encoded (RLE) 4-bit/pixel. Only for 4 bpp bitmaps using CMYK color scheme instead of RGBA.
        /// Same as <c>BI_CMYKRLE4</c> macro of the Windows NT 3.1 and Windows 3.0 BMP v3 and above SDKs.
        /// <para>Windows Metafile CMYK only.</para>
        /// </summary>
        CMYK_RLE4 = 13
    }
}
