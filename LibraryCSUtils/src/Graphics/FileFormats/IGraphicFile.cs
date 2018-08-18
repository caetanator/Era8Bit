using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace CaetanoSoft.Graphics.FileFormats
{
    public interface IGraphicFile
    {
        IEnumerable<string> GetFileExtentions();

        IEnumerable<string> GetFileMimeTypes();

        void GetFileVersion(out int major, out int minor);
        void SetFileVersion(int major, int minor);
        
        void GetPixelDimensions(out uint Width, out uint Height);
        void SetPixelDimensions(uint Width, uint Heighty);
	    
        void GetDPI(out int x, out int y);
        void SetDPI(int x, int y);

        float GetGama();
        void  SetGama(float gama);

        uint GetColorDeep();
        void SetColorDeep(uint bitsPerPixel);

        PixelFormat GetPixelFormat();
        void SetPixelFormat(PixelFormat pixelFormat);

        uint GetNumberOfImages();
        void GetNumberOfImages(uint nImages);

        uint GetCurrentImageIndex();
        void SetCurrentImageIndex(uint nImage);

        byte[] GetProfileICM();
        void   SetProfileICM(ref byte[] icm);
        void   SetProfileICM(string path);

        Dictionary<string, string> getExifData();
        void setExifData(ref Dictionary<string, string> hashTable);

        TColorEntryRGBA<byte> GetPixel(uint x, uint y);
        void SetPixel(int x, int y, ref TColorEntryRGBA<byte> color);

        void Load(string file);
        void Load(Stream stream);
        void Save(string file);
        void Save(Stream stream);
    }
}