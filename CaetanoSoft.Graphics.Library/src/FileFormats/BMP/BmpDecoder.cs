/**
 * BmpDecoder.cs
 *
 * PURPOSE
 *  This class encapsulates the properties and methods needed to decode an BMP bitmap image 
 *  from a streamIn.
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
 *  (C)2019-2024 José Caetano Silva
 *
 * HISTORY
 *  2009-09-15: Created.
 *  2017-04-13: Major rewrite.
 *  2024-12-10: Renamed and updated.
 */

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures;
using CaetanoSoft.Graphics.FileFormats.Common;
using CaetanoSoft.Graphics.PixelFormats;
using CaetanoSoft.Utils.FileSystem.BinaryStreamUtils;

namespace CaetanoSoft.Graphics.FileFormats.BMP
{
    /// <summary>
    /// Performs the BMP decoding operations.
    /// </summary>
    internal sealed class BmpDecoder : IImageDecoder
    {
        // ** Fields

        private bool isTopDown = false;

        private bool isSourcePreRotate = false;

        private bool isWin_v1 = false;

        private bool isOS2_v2 = false;

        /// <summary>
        /// The file name to decode from.
        /// </summary>
        private string bmpFileName;

        /// <summary>
        /// The streamIn to decode from.
        /// </summary>
        private Stream bmpStream;

        /// <summary>
        /// The file/streamIn to decode from size, in bytes.
        /// </summary>
        private long bmpStreamSize = 0;

        /// <summary>
        /// The BMP file header containing general information about the bitmap file DIB.
        /// </summary>
        private Win32FileHeader fileHeader = new(false);

        /// <summary>
        /// The BMP information header containing detailed information about the bitmap DIB.
        /// </summary>
        private Win32InfoHeaderV5 dibHeader = new(false);

        /// <summary>
        /// The BMP DIB information header containing extra information about the OS/2 v2 specific bitmap DIB.
        /// </summary>
        private OS2InfoHeaderV2_ExtraFields dibHeader_OS2;

        private uint scanLineSize = 0;

        // ** Properties

        public bool IsTopDown { get { return isTopDown; } }

        public bool IsSourcePreRotate { get { return isSourcePreRotate; } }

        public bool IsWin_v1 { get { return isWin_v1; } }

        public bool IsOS2_v2 { get { return isOS2_v2; } }

        // ** Constructors

        public BmpDecoder()
        {

        }

        public BmpDecoder(string fileName) : this()
        {
            this.Load(fileName);
        }

        public BmpDecoder(Stream stream) : this()
        {
            this.Read(stream);
        }

        // ** Methods
        public void Load(String fileName)
        {
            // Validate parameters
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            // Parameters OK
            this.bmpFileName = fileName;
            using (var bmpFile = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    this.bmpStreamSize = bmpFile.Length;
                    this.Read(bmpFile);
                    long fileSize = this.bmpStreamSize - bmpFile.Position;
                    if (fileSize != 0)
                    {
                        throw new Exception("Bad file size");
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Read(Stream stream)
        {
            // Validate parameters
            this.bmpStream = stream ?? throw new ArgumentNullException(nameof(stream));

            // Parameters OK
            this.bmpStreamSize = Math.Max(stream.Length, this.bmpStreamSize);
            Image<RGBA32> image this.Decode<RGBA32>(stream, null);
        }

        // ** Override methods of IImageDecoder

        public Image<TPixel> Decode<TPixel>(Stream streamIn, ImageConfiguration configuration) where TPixel : struct, PixelFormats.IPixel<TPixel>
        {
            // Validate parameters
            if (streamIn == null)
            {
                throw new ArgumentNullException(nameof(streamIn));
            }

            // Parameters OK
            try
            {
                long pos = 0;
                EndianAwareBinaryReader binaryReader = new EndianAwareBinaryReader(streamIn, true);
                this.bmpStreamSize = Math.Min(streamIn.Length, this.bmpStreamSize);
                // Check if the size of the file is > than Win BMP v1 (File Header + DDB Header) and (Win BMP v (File Header + DIB Header->Size))
                if (this.bmpStreamSize > Math.Max(
                    (BMP_Constants.RT_BITMAP_SIZE_START + BMP_Constants.BMP_DDB_HEADER_SIZE_V1 + BMP_Constants.RT_BITMAP_SIZE_END),
                    (long)EnumBmpNativeStructuresSizes.BITMAPFILEHEADER + Marshal.SizeOf(this.dibHeader.Size)))
                {
                    // Loads the bitmap File Header
                    this.fileHeader.Magic = binaryReader.ReadUInt16();
                    switch ((int)this.fileHeader.Magic)
                    {
                        case (int)BMP_Constants.BMP_MAGIC_ID:
                        case (int)BMP_Constants.BFT_BITMAPARRAY:
                        case (int)BMP_Constants.BFT_COLORPOINTER:
                        case (int)BMP_Constants.BFT_POINTER:
                        case (int)BMP_Constants.BFT_COLORICON:
                        case (int)BMP_Constants.BFT_ICON:
                            // OK to read BITMAPFILEHEADER structure
                            binaryReader.SeekRelativePosition(-Marshal.SizeOf(this.fileHeader.Magic));
                            this.ReadFileHeaderV1(ref binaryReader, ref pos, this.bmpStreamSize);
                            break;

                        case (int)BMP_Constants.RT_BITMAP_DISCARDABLE:
                        case (int)BMP_Constants.RT_BITMAP_NDISCARDABLE:
                            pos = Marshal.SizeOf(this.fileHeader.Magic);
                            this.fileHeader.Magic = binaryReader.ReadUInt16();
                            if ((int)this.fileHeader.Magic == (int)BMP_Constants.BMP_WIN1_MAGIC_ID)
                            {
                                // OK to read BITMAP16 structure
                                this.fileHeader.Magic = binaryReader.ReadUInt16();
                                pos += Marshal.SizeOf(this.fileHeader.Magic);
                                this.ReadFileHeaderV0(ref binaryReader, ref pos, this.bmpStreamSize);
                            }
                            this.fileHeader.PixelsOffset = binaryReader.ReadUInt32();
                            break;
                        case (int)BMP_Constants.BMP_WIN1_MAGIC_ID:
                            // OK to read BITMAP16 structure
                            pos = Marshal.SizeOf(this.fileHeader.Magic);
                            this.ReadFileHeaderV0(ref binaryReader, ref pos, this.bmpStreamSize);
                            break;
                        default:
                            throw new Exception(String.Format("BMP File Header type \"0x{0,4:X}\" format not supported!", this.fileHeader.Magic));
                    }
                    if (!ValidateFileHeaderV1())
                        throw new Exception(String.Format("BMP File Header type \"0x{0,4:X}\" invalid format!", this.fileHeader.Magic));

                    // Loads the bitmap DIB Header
                    if (this.isWin_v1)
                    {
                        // BITMAP16 structure includes DDB Header, calculate and validate DIB Header Fields
                        this.dibHeader.Size = BMP_Constants.BMP_DDB_HEADER_SIZE_V1;
                        if (scanLineSize == 0)
                        {
                            // If 0, auto detect scan line size
                            scanLineSize = (((uint)this.dibHeader.Width * this.dibHeader.BitsPerPixel) / 8) + 1;
                        }
                        this.dibHeader.ImageDataSize = (uint)this.dibHeader.Height * scanLineSize;
                        if (this.dibHeader.BitsPerPixel <= (uint)EnumBitsPerPixel.Palette256)
                        {
                            // Calculates the palette size
                            this.dibHeader.PaletteSize = (uint) (1 << this.dibHeader.BitsPerPixel);
                            this.dibHeader.PaletteImportant = this.dibHeader.PaletteSize;
                        }
                    }
                    else
                    {
                        switch ((int)this.fileHeader.Magic)
                        {
                            case (int)BMP_Constants.BMP_MAGIC_ID:
                                // OK to read BITMAPCOREHEADER/BITMAPINFOHEADER/BITMAPV4HEADER/BITMAPV5HEADER/OS21XBITMAPHEADER/OS22XBITMAPHEADER structure
                                this.dibHeader.Size = binaryReader.ReadUInt32();
                                if ((pos + this.dibHeader.Size) <= this.fileHeader.FileSize)
                                {
                                    pos += 4;
                                    switch (this.dibHeader.Size)
                                    {
                                        case (uint)EnumBmpNativeStructuresSizes.BITMAPCOREHEADER:
                                            // OK to read BITMAPCOREHEADER/OS21XBITMAPHEADER
                                            this.ReadDibHeaderV2(ref binaryReader, ref pos, this.dibHeader.Size);
                                            break;

                                        case (uint)EnumBmpNativeStructuresSizes.OS22XBITMAPHEADER_MIN:
                                            // OK to read OS22XBITMAPHEADER 16 Bytes
                                        case (uint)EnumBmpNativeStructuresSizes.BITMAPINFOHEADER:
                                            // OK to read BITMAPINFOHEADER
                                        case (uint)EnumBmpNativeStructuresSizes.BITMAPINFOHEADER_NT:
                                            // OK to read BITMAPINFOHEADER NT
                                        case (uint)EnumBmpNativeStructuresSizes.BITMAPINFOHEADER_CE:
                                            // OK to read BITMAPINFOHEADER CE
                                        case (uint)EnumBmpNativeStructuresSizes.BITMAPV4HEADER:
                                            // OK to read BITMAPV4HEADER
                                        case (uint)EnumBmpNativeStructuresSizes.BITMAPV5HEADER:
                                            // OK to read BITMAPV5HEADER
                                            this.ReadDibHeaderV5(ref binaryReader, ref pos, this.dibHeader.Size, this.dibHeader.Size);
                                            break;

                                        default:
                                            if ((this.dibHeader.Size > (uint)EnumBmpNativeStructuresSizes.OS22XBITMAPHEADER_MIN) &&
                                                (this.dibHeader.Size < (uint)EnumBmpNativeStructuresSizes.BITMAPINFOHEADER))
                                            {
                                                // OK to read OS22XBITMAPHEADER > 16 Bytes && < 40 Bytes
                                                this.ReadDibHeaderV5(ref binaryReader, ref pos, this.dibHeader.Size, this.dibHeader.Size);
                                            }
                                            else if ((this.dibHeader.Size > (uint)EnumBmpNativeStructuresSizes.BITMAPINFOHEADER) &&
                                                (this.dibHeader.Size <= (uint)EnumBmpNativeStructuresSizes.OS22XBITMAPHEADER_MAX))
                                            {
                                                // OK to read OS22XBITMAPHEADER > 40 Bytes && <= 64 Bytes
                                                this.ReadDibHeaderV5(ref binaryReader, ref pos, this.dibHeader.Size, (long)EnumBmpNativeStructuresSizes.BITMAPINFOHEADER);
                                                this.ReadDibOs2HeaderV2(ref binaryReader, ref pos, this.dibHeader.Size);
                                            }
                                            else
                                            {
                                                throw new Exception(String.Format("BMP 'BM' DIB Header size {0} format not supported!", this.dibHeader.Size));
                                            }
                                    }
                                    if (!ValidateDibHeaderV5())
                                        throw new Exception(String.Format("BMP 'BM' DIB Header size { 0 } invalid format!", this.dibHeader.Size));
                                }
                                else
                                {
                                    throw new Exception(String.Format("BMP 'BM'' File + DIB Header size {0} is to small to be valid!", (pos + this.dibHeader.Size)));
                                }
                                break;

                            case (int)BMP_Constants.BFT_BITMAPARRAY:
                            case (int)BMP_Constants.BFT_COLORPOINTER:
                            case (int)BMP_Constants.BFT_POINTER:
                            case (int)BMP_Constants.BFT_COLORICON:
                            case (int)BMP_Constants.BFT_ICON:
                                this.isOS2_v2 = true;
                                throw new Exception(String.Format("OS/2 BMP File Header type \"0x{0,4:X}\" format not supported!", this.fileHeader.Magic));
                                break;

                            default:
                                throw new Exception(String.Format("BMP File Header type \"0x{0,4:X}\" format not supported!", this.fileHeader.Magic));
                        }
                    }
                }
                else
                {
                    throw new Exception(String.Format("BMP File size {0} is to small to be valid!", this.bmpStreamSize));
                }
            }
            catch
            {
                throw;
            }
        }

        // ** Methods

        /// <summary>
        /// Reads the DDB File Header (BITMAP16) structure, of Windows 1.0.
        /// </summary>
        /// <param name="binaryReader">The binary reader.</param>
        /// <param name="pos">The position.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        private void ReadFileHeaderV0(ref EndianAwareBinaryReader binaryReader, ref long pos, long size)
        {
            this.isWin_v1 = true;

            this.dibHeader.Width = (int)binaryReader.ReadUInt16();
            pos += 2;
            this.dibHeader.Height = (int)binaryReader.ReadUInt16();
            pos += 2;
            scanLineSize = (uint)binaryReader.ReadUInt16();
            pos += 2;
            this.dibHeader.ColorPlanes = (ushort)binaryReader.ReadByte();
            pos += 1;
            this.dibHeader.BitsPerPixel = (ushort)binaryReader.ReadByte();
            pos += 1;
        }

        /// <summary>
        /// Reads the DIB File Header v1 (BITMAPFILEHEADER) structure, of Windows 2.0 (and newer) 
        /// and OS/2 1.0 (and newer).
        /// </summary>
        /// <param name="binaryReader">The binary reader.</param>
        /// <param name="pos">The position.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        private void ReadFileHeaderV1(ref EndianAwareBinaryReader binaryReader, ref long pos, long size)
        {
            this.fileHeader = (Win32FileHeader)binaryReader.ReadStructure<Win32FileHeader>();
            pos += Marshal.SizeOf(this.fileHeader);
        }

        /// <summary>
        /// Validates the BMP DIB File Header v1.
        /// </summary>
        /// <returns></returns>
        private bool ValidateFileHeaderV1()
        {
            if (this.fileHeader.FileSize == 0)
            {
                // If 0, auto detect file size
                this.fileHeader.FileSize = (uint)this.bmpStreamSize;
            }
            else if (this.fileHeader.FileSize != (uint)this.bmpStreamSize)
            {
                // Most BMP files have this value wrong, so fix it
                this.fileHeader.FileSize = (uint)this.bmpStreamSize;
            }

            return true;
        }

        private void ReadDibOs2HeaderV2(ref EndianAwareBinaryReader binaryReader, ref long pos, long size)
        {
            this.isOS2_v2 = true;

            this.dibHeader_OS2 = (OS2InfoHeaderV2_ExtraFields)binaryReader.ReadStructure<OS2InfoHeaderV2_ExtraFields>();
            pos += Marshal.SizeOf(this.dibHeader_OS2);
        }

        private void ReadDibHeaderV2(ref EndianAwareBinaryReader binaryReader, ref long pos, long size)
        {
            this.dibHeader.Width = (int)binaryReader.ReadUInt16();
            pos += 2;
            this.dibHeader.Height = (int)binaryReader.ReadUInt16();
            pos += 2;
            this.dibHeader.ColorPlanes = (ushort)binaryReader.ReadUInt16();
            pos += 2;
            this.dibHeader.BitsPerPixel = (ushort)binaryReader.ReadUInt16();
            pos += 2;
        }

        private void ReadDibHeaderV5(ref EndianAwareBinaryReader binaryReader, ref long pos, long size, long max)
        {
            uint posStop = (uint)pos + (uint)max;
            if (pos < posStop)
            {
                this.dibHeader.Width = (int)binaryReader.ReadInt32();
                pos += 4;
            }
            if (pos < posStop)
            {
                this.dibHeader.Height = (int)binaryReader.ReadInt32();
                pos += 4;
            }
            if (pos < posStop)
            {
                this.dibHeader.ColorPlanes = (ushort)binaryReader.ReadUInt16();
                pos += 2;
            }
            if (pos < posStop)
            {
                this.dibHeader.BitsPerPixel = (ushort)binaryReader.ReadUInt16();
                pos += 2;
            }
            if (pos < posStop)
            {
                this.dibHeader.Compression = (ushort)binaryReader.ReadUInt32();
                pos += 4;
            }
            if (pos < posStop)
            {
                this.dibHeader.ImageDataSize = (ushort)binaryReader.ReadUInt32();
                pos += 4;
            }
        }

        private bool ValidateDibHeaderV5()
        {
            if (this.dibHeader.Width < 0)
            {
                // DIB is Top-Down
                this.dibHeader.Width = -this.dibHeader.Width;
                this.isTopDown = true;
            }
            if ((this.dibHeader.Compression & (uint)BMP_Constants.SourceIsPreRotatedMask) != 0)
            {
                // DIB is source pre-rotated
                if (this.dibHeader.Compression <= (uint)EnumCompression.AlphaBitFields)
                {
                    // Protects against CMCYK and FourCC YUV Compressed Video compression types
                    this.dibHeader.Compression ^= (uint)BMP_Constants.SourceIsPreRotatedMask;
                    this.isSourcePreRotate = true;
                }
            }
            if (this.dibHeader.ImageDataSize == 0 && (this.dibHeader.Compression == (uint)EnumCompression.RGB))
            {
                // If 0, auto detect scan line size
                scanLineSize = 4 * ((((uint)this.dibHeader.Width * this.dibHeader.BitsPerPixel) + 31) / 32);
                this.dibHeader.ImageDataSize = (uint)this.dibHeader.Height * scanLineSize;
            }
            else
            {
                scanLineSize = (uint)this.dibHeader.ImageDataSize / (uint)this.dibHeader.Height);
            }
            
            if (this.dibHeader.BitsPerPixel <= (uint)EnumBitsPerPixel.Palette256)
            {
                // Calculates the palette size
                this.dibHeader.PaletteSize = (uint)(1 << this.dibHeader.BitsPerPixel);
                this.dibHeader.PaletteImportant = this.dibHeader.PaletteSize;
            }

            return true;
        }
    }
}
