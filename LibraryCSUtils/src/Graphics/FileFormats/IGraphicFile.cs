using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CaetanoSof.Utils.Drawing
{
    public enum RTPixelFormat
    {
        Unknow = 0,
        RGB050505 = 1, 
        RGB050605 = 2,
        RGB080808 = 3,
        RGB101010 = 4,
        RGB161616 = 5,
        RGBA05050501 = 6,
        RGBA05050505 = 7,
        RGBA08080808 = 8,
        RGBA16161616 = 9
    };

    public interface IGraphicFile
    {
        string[] getFileExtention();
        
        bool isTopDown();

        void getFileVersion(out int major, out int minor);
        void setFileVersion(int major, int minor);
        
        void getSize(out uint x, out uint y);
        void setSize(uint x, uint y);
	    
        void getDPI(out int x, out int y);
        void setDPI(int x, int y);

        float getGama();
        void  setGama(float gama);

        uint getColorDeep();
        void setColorDeep(uint bitsPerPixel);

        RTPixelFormat getPixelFormat();
        void setPixelFormat(RTPixelFormat pixelFormat);

        uint getNumberOfImages();
        void setNumberOfImages(uint nImages);

        uint getCurrentImage();
        void setCurrentImage(uint nImage);

        byte[] getProfileICM();
        void   setProfileICM(ref byte[] icm);
        void   setProfileICM(string path);

        Dictionary<string, string> getExifData();
        void setExifData(ref Dictionary<string, string> hashTable);

        ColorEntryRGBA<T> getPixel(int x, int y);
        void setPixel(int x, int y, ref ColorEntryRGBA<T> color);

        void Load(string file);
        void Save(string file);
    }
}