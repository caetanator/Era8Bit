
Test images at:

- BMP Suite v1.0: http://www.hassconsult.co.ke/dompdf/www/test/image_bmp.html
- BMP Suite v2.5: http://entropymine.com/jason/bmpsuite/bmpsuite/html/bmpsuite.html
- Microsoft Windows Bitmap Sample Files: http://www.fileformat.info/format/bmp/sample/index.htm
- Charles Petzold Books and Source Code: https://www.charlespetzold.com/books.html

File format description and documentation:

https://msdn.microsoft.com/en-us/library/windows/desktop/dd183391(v=vs.85).aspx
https://msdn.microsoft.com/en-us/library/windows/desktop/ms536393(v=vs.85).aspx
https://msdn.microsoft.com/en-us/library/windows/desktop/dd183386(v=vs.85).aspx
https://msdn.microsoft.com/en-us/library/aa452885.aspx
https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-bitmapv5header
https://learn.microsoft.com/en-us/previous-versions/windows/embedded/aa452885(v=msdn.10)
https://en.wikipedia.org/wiki/BMP_file_format
http://www.fileformat.info/format/bmp/egff.htm
http://www.fileformat.info/format/os2bmp/egff.htm
http://fileformats.archiveteam.org/wiki/Windows_DDB
https://komh.github.io/os2books/os2tk45/gpi3/027_L2H_BITMAPINFO2FieldulCo.html
https://github.com/bitwiseworks/os2tk45/blob/master/h/pmbitmap.h
https://flylib.com/books/en/4.267.1.73/1/


# Supported BMP File Formats

## Loading/Decoding


### IBM OS/2 v1

> **See [Microsoft Windows v2](#microsoft-windows-v2)**


### IBM OS/2 v2

**Monochrome** (1 bit-per-pixel)

- [X] Uncompressed
- [ ] Huffman 1D Compressed
  - [ ] No Halftoning
  - [ ] Error Diffusion Halftoning
  - [ ] PANDA Halftoning
  - [ ] Super-Circle Halftoning

**16 Colors Palette** (4 bits-per-pixel)

- [X] Uncompressed
- [X] RLE-4 Compressed

**256 Colors Palette** (8 bits-per-pixel)

- [X] Uncompressed
- [X] RLE-8 Compressed

**24-bit RGB** (8 bits-per-pixel color)

- [X] Uncompressed - BGR888
- [ ] RLE-24 Compressed


### Microsoft Windows v1

**Monochrome/MGA/Hercules** (1 bit-per-pixel)

- [ ] Uncompressed

**CGA/EGA** (4 bit-per-pixel)

- [ ] Uncompressed

**VGA** (8 bit-per-pixel)

- [ ] Uncompressed*

### Microsoft Windows v2

**Monochrome** (1 bit-per-pixel)

- [X] Uncompressed

**16 Colors Palette** (4 bits-per-pixel)

- [X] Uncompressed
- [X] RLE-4 Compressed

**256 Colors Palette** (8 bits-per-pixel)

- [X] Uncompressed
- [X] RLE-8 Compressed

**24-bit RGB** (8 bits-per-pixel color)

- [X] Uncompressed - BGR888

### Microsoft Windows v3 to v5

**Extra Features**

- [X] Top-down DIBs for uncompressed files
- [ ]  90º degrees rotated DIBs for Windows CE files
- [X] Bitfield masks on v3 DIBs for Windows NT and Windows CE files
- [X] Gamma correction for v4 and v5 DIBs
- [X] ICC color spaces profiles for v5 DIBs

**PNG**

- [ ] PNG Compressed

**JPEG**

- [ ] JPEG Compressed

**Monochrome** (1 bits-per-pixel)

- [X] Uncompressed

**4 Colors Palette** (2 bits-per-pixel)

- [X] Uncompressed

**16 Colors Palette** (4 bits-per-pixel)

- [X] Uncompressed
- [X] RLE-4 Compressed

**256 Colors Palette** (8 bits-per-pixel)

- [X] Uncompressed
- [X] RLE-8 Compressed

**16-bit RGB** (5 bits-per-pixel color)

- [X] Uncompressed - BGR555
- [X] Bitfields

**24-bit RGB** (8 bits-per-pixel color)

- [X] Uncompressed - BGR888

**32-bit RGBA** (8 bits-per-pixel color + 8-bit Alpha)

- [X] Uncompressed - BGRA8880
- [X] Bitfields

## Saving/Encoding

### IBM OS/2 v1

> **See [Microsoft Windows v2](#microsoft-windows-v2-1)**

### IBM OS/2 v2

**Monochrome** (1 bit-per-pixel)

- [ ] Uncompressed
- [ ] Huffman 1D Compressed
  - [ ] No Halftoning
  - [ ] Error Diffusion Halftoning
  - [ ] PANDA Halftoning
  - [ ] Super Circle Halftoning

**16 Colors Palette** (4 bits-per-pixel)

- [ ] Uncompressed
- [ ] RLE-4 Compressed

**256 Colors Palette** (8 bits-per-pixel)

- [ ] Uncompressed
- [ ] RLE-8 Compressed

**24-bit RGB** (8 bits-per-pixel color)

- [ ] Uncompressed - BGR888
- [ ] RLE-24 Compressed

### Microsoft Windows v1

>**Not supported** - Uses a different FileHeader and DDB (Device-Dependent Bitmap) instead of DIB (Device-Independent Bitmap)

### Microsoft Windows v2

**Monochrome** (1 bit-per-pixel)

- [ ] Uncompressed

**16 Colors Palette** (4 bits-per-pixel)

- [ ] Uncompressed
- [ ] RLE-4 Compressed

**256 Colors Palette** (8 bits-per-pixel)

- [ ] Uncompressed
- [ ] RLE-8 Compressed

**24-bit RGB** (8 bits-per-pixel color)

- [ ] Uncompressed - BGR888

### Microsoft Windows v3 to v5

**Extra Features**

- [X] Top-down DIBs for uncompressed files
- [ ] 90º degrees rotated DIBs for Windows CE files
- [X] Bitfield masks on v3 DIBs for Windows NT and Windows CE files
- [X] Gamma correction for v4 and v5 DIBs
- [X] ICC color spaces profiles for v5 DIBs

**PNG**

- [ ] PNG Compressed

**JPEG**

- [ ] JPEG Compressed

**Monochrome** (1 bits-per-pixel)

- [ ] Uncompressed

**4 Colors Palette** (2 bits-per-pixel)

- [ ] Uncompressed

**16 Colors Palette** (4 bits-per-pixel)

- [ ] Uncompressed
- [ ] RLE-4 Compressed

**256 Colors Palette** (8 bits-per-pixel)

- [ ] Uncompressed
- [ ] RLE-8 Compressed

**16-bit RGB** (5 bits-per-pixel color)

- [ ] Uncompressed - BGR555
- [ ] Bitfields

**24-bit RGB** (8 bits-per-pixel color)

- [X] Uncompressed - BGR888

**32-bit RGBA** (8 bits-per-pixel color + 8-bit Alpha)

- [X] Uncompressed - BGRA8880
- [ ] Bitfields

> [!NOTE]
> * 90º degrees rotated DIBs for Windows CE files, are probably for internal use of the Windows Win32 APIs. Not to be used on a file saved on a disk.
> * JPEG and PNG cpmpression, are for internal use on the Windows Win32 Printing APIs. Not to be used on a file saved on a disk.

> [!IMPORTANT]
> Information for IBM OS/2 1-bpp "Huffman 1D" compression needed. Please help.