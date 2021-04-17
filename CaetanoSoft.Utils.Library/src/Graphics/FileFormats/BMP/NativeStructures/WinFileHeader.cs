
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP
{
    /// <summary>
    /// This is the Microsoft Windows BMP v2 and IBM OS/2 BMP v1 (and later) file header:
    /// BITMAPFILEHEADER. It contains information about the type, size, and layout of the contained DIB
    /// (Device Independent Bitmap).
    /// <para>Supported since Windows 2.0, Windows CE 2.0 and OS/2 1.0.</para>
    /// <para>Implemented on Microsoft Windows BMP v2 and IBM OS/2 BMP v1 format.</para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd183374(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    /// <remarks>
    /// Make shore that <c>sizeof(WinFileHeader)</c> returns the size of 12 bytes and is byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// <para>
    /// The DIB information header must follow the <c>WinFileHeader</c> structure, and consist of one of
    /// <see cref="OS2InfoHeaderV2"/>, <see cref="WinCoreHeader"/>, <see cref="WinInfoHeaderV5"/>, etc. structures.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14)]
    internal struct WinFileHeader
    {
        // ** Fields for Microsoft Windows BMP v2 and IBM OS/2 BMP v1 file header

        /// <summary>
        /// Specifies the BMP file type "Magic ID", must be "BM" in ASCII or 4D42h in hexadecimal.
        /// <para>
        /// OS/2 allows also this "Magic IDs" for other image file formats:
        /// <list type="table">
        /// <listheader>
        /// <term>ASCII/Hexadecimal Value</term>
        /// <description>Type of Image File</description>
        /// </listheader>
        /// <item>
        /// <term>"BA"/4142h</term>
        /// <description>Bitmap Array</description>
        /// </item>
        /// <item>
        /// <term>"CI"/4943h</term>
        /// <description>Color Icon</description>
        /// </item>
        /// <item>
        /// <term>"CP"/5043h</term>
        /// <description>Color Pointer</description>
        /// </item>
        /// <item>
        /// <term>"IC"/4349h</term>
        /// <description>Icon</description>
        /// </item>
        /// <item>
        /// <term>"PT"/5450h</term>
        /// <description>Pointer</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public ushort Magic;

        /// <summary>
        /// Specifies the size, in bytes, of the bitmap BMP file.
        /// <para>
        /// This value is calculated by the formula:
        /// <para><c>    FileHeaderSize + InfoHeaderSize + PaletteDataSize + GapOptional1 + ImageDataSize + GapOptional2 + IccProfileSize</c></para>
        /// <para><c>    PaletteDataSize = PaletteNumberOfEntries * PaletteElementSize</c></para>
        /// <para><c>    ImageDataSize = RowSize * modulus(ImageHeight)</c></para>
        /// <para><c>    RowSize = floor(((BitsPerPixel * ImageWidth) + 31) / 32) * 4</c></para>
        /// <para><c>    PaletteNumberOfEntries_Maximum = 1 &lt;&lt; BitsPerPixel</c></para>
        /// <para><c>    PaletteNumberOfEntries_Present = (WinFileHeader.PixelsOffset - FileHeaderSize - InfoHeaderSize) / PaletteElementSize</c></para>
        /// </para>
        /// </summary>
        public uint FileSize;

        /// <summary>
        /// Reserved for future use. Must be set to 0.
        /// <para>
        /// For OS/2 specific formats, this is the X coordinate of the central point of the hotspot for icons and pointers.
        /// </para>
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// Reserved for future use. Must be set to 0.
        /// <para>
        /// For OS/2 specific formats, this is the Y coordinate of the central point of the hotspot for icons and pointers.
        /// </para>
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// Specifies the offset, in bytes, from the beginning of the <c>WinFileHeader</c> structure to the bitmap pixels color bits/image data.
        /// <para>
        /// This value is obtained by this formula: <c>FileHeaderSize + InfoHeaderSize + PaletteDataSize + GapOptional1</c>
        /// </para>
        /// </summary>
        public uint PixelsOffset;
    }
}
