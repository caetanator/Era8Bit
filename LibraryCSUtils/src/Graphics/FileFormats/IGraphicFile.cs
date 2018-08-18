using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CaetanoSoft.Graphics.FileFormats
{
    public interface IGraphicFile
    {
        IEnumerable<string> GetFileExtentions();

        IEnumerable<string> GetFileMimeTypes();

        void GetFileVersion(out int major, out int minor);
        void SetFileVersion(int major, int minor);
        
        void GetPixelDimensions(out uint x, out uint y);
        void SetPixelDimensions(uint x, uint y);
	    
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

        uint GetCurrentImagePosition();
        void SetCurrentImagePosition(uint nImage);

        byte[] GetProfileICM();
        void   SetProfileICM(ref byte[] icm);
        void   SetProfileICM(string path);

        Dictionary<string, string> getExifData();
        void setExifData(ref Dictionary<string, string> hashTable);

        ARGB rrr;

        ColorEntryRGBA<T> getPixel(uint x, uint y);
        void setPixel(int x, int y, ref ColorEntryRGBA<T> color);

        void Load(string file);
        void Save(string file);
    }
}