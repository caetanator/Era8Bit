namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
  /// <summary>
  /// The <c>BMP_Constants</c> class contains useful constants defined by the Microsoft Windows and 
  /// IBM OS/2 API's and some created for this project.
  /// </summary>
  internal static class BMP_Constants
  {
    // ** Constants

    /// <summary>
    /// This flag as the same value as <c>BI_SRCPREROTATE</c> in the Windows CE/Mobile SDKs. You can use it on 
    /// Windows CE/Mobile version 5.0 + .NET 4 and later, to check if the source (camera) DIB section has a 
    /// different orientation angle (Portrait/Landscape) of the destination (screen).
    /// <para>
    /// If this flag is AND'ed, <c>if (((BMP_DIB_HeaderV3.biCompression & BMP_Constants.SourceIsPreRotatedMask) != 0) && 
    /// ((BMP_DIB_HeaderV3.biCompression & !BMP_Constants.SourceIsPreRotatedMask) <= BI_ALPHABITFIELDS))</c>, is <c>true</c>
    /// the BMP image have the same orientation than the destination device, otherwise it should be rotated 90 degrees 
    /// anti-clockwise for best view.
    /// </para>
    /// <para>This flag is used internally by Windows CE devices (specially by camera and display drivers), and 
    /// shouldn't be saved on the BMP file, so can be ignored if present.</para>
    /// <para>BITMAPINFOHEADER.biCompression must be either BI_RGB, BI_ALPHABITFIELDS or BI_BITFIELDS. 
    /// Pre-rotated DIBs cannot be compressed.</para>
    /// See this <seealso cref="https://msdn.microsoft.com/en-us/library/aa452495.aspx">MSDN</seealso> article, for
    /// more information.
    /// </summary>
    public const uint SourceIsPreRotatedMask = 0x8000;

    /// <summary>
    /// For Windows 95/NT and later, you can AND <c>BITMAPINFOHEADER.biHeight</c>, <c>BITMAPV4HEADER.bV4Height</c> or <c>BITMAPV5HEADER.bV5Height</c>
    /// with this flag value to see if this bitmap is a special top-down DIB (its origin is the upper-left corner) or a normal bottom-up DIB 
    /// (its origin is the lower-left corner).
    /// 
    /// If this flag is AND'ed, <c>if ((BMP_DIB_HeaderV3.biHeight & BMP_Constants.SourceIsTopDownMask) != 0)</c>, the BMP is 
    /// Top-Down (BMP_DIB_HeaderV3.biHeight < 0) instead of the normal Bottom-Up (BMP_DIB_HeaderV3.biHeight > 0).
    /// 
    /// BITMAPV5HEADER.bV3Compression (or equivalent) must be either BI_RGB, BI_ALPHABITFIELDS or BI_BITFIELDS. Top-down DIBs cannot be compressed.
    /// https://docs.microsoft.com/en-us/windows/desktop/api/Wingdi/ns-wingdi-bitmapv5header
    /// </summary>
    public const uint SourceIsTopDownMask = 0x80000000;
        
    /* Windows 1.0x constants stuff */
    // Microsoft Windows 1.x bitmap file Magic ID
    public const ushort BMP_WIN1_MAGIC_ID = 0;

    // Windows 1.0x BMP Resource Type ID (non-discardable)
    public const ushort RT_BITMAP_NDISCARDABLE = 0x0002;
    // Windows 1.0x BMP Resource Type ID (discardable)
    public const ushort RT_BITMAP_DISCARDABLE = (ushort)((int)RT_BITMAP_NDISCARDABLE | 0x0100);

    // Microsoft Windows 1.x DDB header size
    public const int BMP_DDB_HEADER_SIZE_V1 = 10;
    // Microsoft Windows 1.x bitmap file size
    public const int BMP_WIN1_FILE_HEADER_SIZE = 10;
    // Windows 1.0x BMP Resource Type leading bytes
    public const int RT_BITMAP_SIZE_START = 2;
    // Windows 1.0x BMP Resource Type trailing bytes
    public const int RT_BITMAP_SIZE_END = 4;

    /* OS/2 2.0x constants stuff */
    // IBM OS/2 2.x  (and all above) DIB header size
    public const int OS2_DIB_HEADER_SIZE_V2 = 64;
    public const int OS2_DIB_HEADER_SIZE_V2_MIN = 16;
    public const int OS2_DIB_HEADER_SIZE_V2_MAX = 64;    

    /* BMP Palette constants stuff */
    // Microsoft Windows 2.x and IBM OS/2 1.x palette size
    public const int BMP_PALETTE_SIZE_V2 = 3;
    // Microsoft Windows 3.xx, Windows NT 3.xx, Windows CE 1.xx and IBM OS/2 2.x  (and all above) palette size
    public const int BMP_PALETTE_SIZE_V3 = 4;

    /* BMP File Header constants stuff */
    // Microsoft Windows 2.x and IBM OS/2 1.x (and all above) bitmap file size
    public const int BMP_FILE_HEADER_SIZE_V1 = 14;

    /* BMP DIB Header constants stuff */
    // Microsoft Windows 2.x and IBM OS/2 1.x DIB header size
    public const int BMP_DIB_HEADER_SIZE_V2 = 12;
    // Microsoft Windows 3.xx, Windows NT 3.xx  and Windows CE 1.xx  (and all above) DIB header size
    public const int BMP_DIB_HEADER_SIZE_V3 = 40;
    // Microsoft Windows NT 3.xx (and all above) DIB header size
    public const int BMP_DIB_HEADER_SIZE_V3_RGB = 52;
    // Microsoft Windows CE 5.0 + .NET 4.0 (and all above) DIB header size
    public const int BMP_DIB_HEADER_SIZE_V3_RGBA = 56;
    // Microsoft Windows 95 and Windows NT 4.0 DIB size
    public const int BMP_DIB_HEADER_SIZE_V4 = 108;
    // Microsoft Windows 98 and 2000  DIB header size
    public const int BMP_DIB_HEADER_SIZE_V5 = 124;

    // Windows or OS/2 Bitmap Magic ID ("BM" = 0x4D42, 'B'=0x42; 'M'=0x4D)
    public const ushort BMP_MAGIC_ID = 0x4D42;
    // OS/2 Bitmap Array Magic ID ("BA" = 0x4142, 'B'=0x42; 'A'=0x41)
    public const ushort BFT_BITMAPARRAY = 0x4142;
    // OS/2 Monochrome Icon Magic ID ("IC" = 0x4349, 'I'=0x49; 'C'=0x43)
    public const ushort BFT_ICON = 0x4349;
    // OS/2 Color Icon Magic ID ("CI" = 0x4943, 'C'=0x43; 'I'=0x49)
    public const ushort BFT_COLORICON = 0x4943;
    // OS/2 Monochrome Pointer Magic ID ("PT" = 0x5450, 'P'=0x50; 'T'=0x54)
    public const ushort BFT_POINTER = 0x5450;
    // OS/2 Color Pointer Magic ID ("CP" = 0x5043, 'C'=0x43; 'I'=0x49)
    public const ushort BFT_COLORPOINTER = 0x5043;
  }
}
