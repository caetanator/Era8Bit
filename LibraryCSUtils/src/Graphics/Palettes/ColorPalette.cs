using System;
using System.Collections.Generic;
using System.Text;

using ColorEntryByteRGB = CaetanoSoft.Graphics.FileFormats.TColorEntryRGB<byte>;
using ColorEntryFloatRGB = CaetanoSoft.Graphics.FileFormats.TColorEntryRGB<float>;
using ColorEntryByteRGBA = CaetanoSoft.Graphics.FileFormats.TColorEntryRGBA<byte>;
using ColorEntryFloatRGBA = CaetanoSoft.Graphics.FileFormats.TColorEntryRGBA<float>;

namespace CaetanoSoft.Graphics.FileFormats
{
    /// <summary>
    /// Implementes a color palette.
    /// </summary>
    /// <list type = "bullet" >
    /// <item>
    /// <term>ColorEntryByte</term>
    /// <description>A palette here each color component is a byte in the interval 0 to 255.</description>
    /// </item>
    /// <item>
    /// <term>ColorEntryFloat</term>
    /// <description>A palette here each color component is a float in the interval 0.0 to 1.0.</description>
    /// </item>
    /// </list>
    public class ColorPalette
    {
        /// <summary>
        /// The palette minimum size
        /// </summary>
        public const int PALETTE_MIN_SIZE = 2;
        /// <summary>
        /// The palette maximum size
        /// </summary>
        public const int PALETTE_MAX_SIZE = 256;

        // Number of colors in this palette (minimum = 2, maximum = 256)
        private int paletteColors = 0;

        // Palette index for the color entry that defines the 100% transparency
        private int transparencyIndex = -1;

        // Palette color entries
        private ColorEntryByteRGBA[] palette = null;

        // Dictionary to speed search of colors in the palette
        private Dictionary<uint, int> hashTable = new Dictionary<uint, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPalette"/> class.
        /// </summary>
        public ColorPalette() : this(2)
        {
            // Executes: ColorPalette(2)
        }

        public ColorPalette(int maxColors)
        {
            this.NumberOfEntries = maxColors;
        }

        /// <summary>
        /// Gets the number of color entries on the palette.
        /// </summary>
        /// <value>
        /// The number of entries.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of entries on the palette must be between 2 and 256</exception>
        public int NumberOfEntries
        {
            get
            {
                return this.paletteColors;
            }

            set 
            {
                if ((value >= PALETTE_MIN_SIZE) && (value <= PALETTE_MAX_SIZE))
                {
                    this.paletteColors = value;
                    this.transparencyIndex = -1;
                    this.hashTable.Clear();
                    this.palette = new ColorEntryByteRGBA[this.NumberOfEntries];
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException("The number of entries on the palette must be between 2 and 256");
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the transparency color.
        /// </summary>
        /// <value>
        /// The index of the transparency.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">The index must be -1 (no transparency) or between 0 and 'paletteColors'</exception>
        public int TransparencyIndex
        {
            get
            {
                return this.transparencyIndex;
            }
            set
            {
                if ((value >= -1) && (value < this.paletteColors))
                {
                    this.transparencyIndex = value;
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException("The index must be -1 (no transparency) or between 0 and 'paletteColors'");
                }
            }
        }

        /// <summary>
        /// Gets the color entry for the given index.
        /// </summary>
        /// <param name="index">The palette index.</param>
        /// <returns>The ColorEntryByteRGBA color at index</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The index must be between 0 and 'paletteColors'</exception>
        public ColorEntryByteRGBA GetEntry(int index)
        {
            if ((index >= 0) && (index < this.paletteColors))
            {
                return this.palette[index];
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("The index must be between 0 and 'paletteColors'");
            }
        }

        /// <summary>
        /// Sets the color entry for the given index, to the given color.
        /// </summary>
        /// <param name="index">The palette index.</param>
        /// <param name="color">The ColorEntryByteRGBA color.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The index must be between 0 and 'paletteColors'</exception>
        public void SetEntry(int index, ref ColorEntryByteRGBA color)
        {
            if ((index >= 0) && (index < this.paletteColors))
            {
                this.palette[index].Alpha = color.Alpha;
                this.palette[index].Red = color.Red;
                this.palette[index].Green = color.Green;
                this.palette[index].Blue = color.Blue;

                 this.hashTable.Clear();
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("The index must be between 0 and 'paletteColors'");
            }
        }

        private void FillZeros()
        {
            for (int i = 0; i < this.paletteColors; i++)
            {
                this.palette[i].Alpha = 0;
                this.palette[i].Red = 0;
                this.palette[i].Green = 0;
                this.palette[i].Blue = 0;
            }
        }

        private void FillGray()
        {
            int step = 256 / (this.paletteColors);
            for (int i = 0; i < this.paletteColors; i++)
            {
                byte v = (byte)((i == 0) ? 0 : (((i + 1) * step)) - 1);
                this.palette[i].Alpha = 0;
                this.palette[i].Red = v;
                this.palette[i].Green = v;
                this.palette[i].Blue = v;
            }
        }

        /// <sumary>
        /// Creates a palette with 256 shades of gray.
        /// </sumary>
        public void Create256GrayScalePalette()
        {
            this.NumberOfEntries = 256;
            this. FillGray();
        }

        /// <sumary>
        /// Creates a palette with 64 shades of gray.
        /// </sumary>
        public void Create64GrayScalePalette()
        {
            this.NumberOfEntries = 64;
            this.FillGray();
        }

        /// <sumary>
        /// Creates a palette with 32 shades of gray.
        /// </sumary>
        public void Create32GrayScalePalette()
        {
            this.NumberOfEntries = 32;
            this.FillGray();
        }

        /// <sumary>
        /// Creates a palette with 16 shades of gray.
        /// </sumary>
        public void Create16GrayScalePalette()
        {
            this.NumberOfEntries = 16;
            this.FillGray();
        }

        /// <sumary>
        /// Creates a palette with 8 shades of gray.
        /// </sumary>
        public void Create8GrayScalePalette()
        {
            this.NumberOfEntries = 8;
            this.FillGray();
        }

        /// <sumary>
        /// Creates a palette with 4 shades of gray.
        /// </sumary>
        public void Create4GrayScalePalette()
        {
            this.NumberOfEntries = 4;
            this.FillGray();
        }

        /// <summary>
        /// Creates a monochrome palette (2 colors) with black & white.
        /// </summary>
        private void CreateMonochromePalette()
        {
            this.NumberOfEntries = 2;
            this.FillGray();
        }

        /*
        /// <sumary>
        /// Creates a 16 entries EGA default color palette.
        /// </sumary>
        public void CreateEGAPal()
        {
            this.paletteColors = 16;
            this.transparencyIndex = -1;
            this.hashTable.Clear();

            this.palette[0].Alpha = 0;
            this.palette[0].Red = 0;
            this.palette[0].Green= 0;
            this.palette[0].Blue = 0;
            this.palette[1].Alpha = 0;
            this.palette[1].Red = 0;
            this.palette[1].Green= 0;
            this.palette[1].Blue = (T)(Object)0xAA;
            this.palette[2].Alpha = 0;
            this.palette[2].Red = 0;
            this.palette[2].Green = (T)(Object)0xAA;
            this.palette[2].Blue = 0;
            this.palette[3].Alpha = 0;
            this.palette[3].Red = 0;
            this.palette[3].Green = (T)(Object)0xAA;
            this.palette[3].Blue = (T)(Object)0xAA;
            this.palette[4].Alpha = 0;
            this.palette[4].Red = (T)(Object)0xAA;
            this.palette[4].Green= 0;
            this.palette[4].Blue = 0;
            this.palette[5].Alpha = 0;
            this.palette[5].Red = (T)(Object)0xAA;
            this.palette[5].Green= 0;
            this.palette[5].Blue = (T)(Object)0xAA;
            this.palette[6].Alpha = 0;
            this.palette[6].Red = (T)(Object)0xAA;
            this.palette[6].Green = (T)(Object)0x55;
            this.palette[6].Blue = 0;
            this.palette[7].Alpha = 0;
            this.palette[7].Red = (T)(Object)0xAA;
            this.palette[7].Green = (T)(Object)0xAA;
            this.palette[7].Blue = (T)(Object)0xAA;
            this.palette[8].Alpha = 0;
            this.palette[8].Red = (T)(Object)0x55;
            this.palette[8].Green = (T)(Object)0x55;
            this.palette[8].Blue = (T)(Object)0x55;
            this.palette[9].Alpha = 0;
            this.palette[9].Red = 0;
            this.palette[9].Green= 0;
            this.palette[9].Blue = 255;
            this.palette[10].Alpha = 0;
            this.palette[10].Red = 0;
            this.palette[10].Green = 255;
            this.palette[10].Blue = 0;
            this.palette[11].Alpha = 0;
            this.palette[11].Red = 0;
            this.palette[11].Green = 255;
            this.palette[11].Blue = 255;
            this.palette[12].Alpha = 0;
            this.palette[12].Red = 255;
            this.palette[12].Green= 0;
            this.palette[12].Blue = 0;
            this.palette[13].Alpha = 0;
            this.palette[13].Red = 255;
            this.palette[13].Green= 0;
            this.palette[13].Blue = 255;
            this.palette[14].Alpha = 0;
            this.palette[14].Red = 255;
            this.palette[14].Green = 255;
            this.palette[14].Blue = 0;
            this.palette[15].Alpha = 0;
            this.palette[15].Red = 255;
            this.palette[15].Green = 255;
            this.palette[15].Blue = 255;
        }

        /// <sumary>
        /// Creates a 16 entries Windows default color palette.
        /// </sumary>
        public void CreateWin16Pal()
        {
            this.paletteColors = 16;
            this.transparencyIndex = -1;
            this.hashTable.Clear();

            this.palette[0].Alpha = 0;
            this.palette[0].Red = 0;
            this.palette[0].Green= 0;
            this.palette[0].Blue = 0;
            this.palette[1].Alpha = 0;
            this.palette[1].Red = (T)(Object)0x80;
            this.palette[1].Green= 0;
            this.palette[1].Blue = 0;
            this.palette[2].Alpha = 0;
            this.palette[2].Red = 0;
            this.palette[2].Green = (T)(Object)0x80;
            this.palette[2].Blue = 0;
            this.palette[3].Alpha = 0;
            this.palette[3].Red = (T)(Object)0x80;
            this.palette[3].Green = (T)(Object)0x80;
            this.palette[3].Blue = 0;
            this.palette[4].Alpha = 0;
            this.palette[4].Red = 0;
            this.palette[4].Green= 0;
            this.palette[4].Blue = (T)(Object)0x80;
            this.palette[5].Alpha = 0;
            this.palette[5].Red = (T)(Object)0x80;
            this.palette[5].Green= 0;
            this.palette[5].Blue = (T)(Object)0x80;
            this.palette[6].Alpha = 0;
            this.palette[6].Red = 0;
            this.palette[6].Green = (T)(Object)0x80;
            this.palette[6].Blue = (T)(Object)0x80;
            this.palette[7].Alpha = 0;
            this.palette[7].Red = (T)(Object)0xC0;
            this.palette[7].Green = (T)(Object)0xC0;
            this.palette[7].Blue = (T)(Object)0xC0;
            this.palette[8].Alpha = 0;
            this.palette[8].Red = (T)(Object)0x80;
            this.palette[8].Green = (T)(Object)0x80;
            this.palette[8].Blue = (T)(Object)0x80;
            this.palette[9].Alpha = 0;
            this.palette[9].Red = 255;
            this.palette[9].Green= 0;
            this.palette[9].Blue = 0;
            this.palette[10].Alpha = 0;
            this.palette[10].Red = 255;
            this.palette[10].Green= 0;
            this.palette[10].Blue = 255;
            this.palette[11].Alpha = 0;
            this.palette[11].Red = 255;
            this.palette[11].Green = 255;
            this.palette[11].Blue = 0;
            this.palette[12].Alpha = 0;
            this.palette[12].Red = 0;
            this.palette[12].Green= 0;
            this.palette[12].Blue = 255;
            this.palette[13].Alpha = 0;
            this.palette[13].Red = 255;
            this.palette[13].Green= 0;
            this.palette[13].Blue = 255;
            this.palette[14].Alpha = 0;
            this.palette[14].Red = 0;
            this.palette[14].Green = 255;
            this.palette[14].Blue = 255;
            this.palette[15].Alpha = 0;
            this.palette[15].Red = 255;
            this.palette[15].Green = 255;
            this.palette[15].Blue = 255;
        }

        /// <sumary>
        /// Creates a 256 entries Windows default color palette.
        /// Indexes [0;9] and [246;255] (20 colors) hare used by Windows, the others 
        /// can be set by the application.
        /// </sumary>
        public void CreateWin256Pal()
        {
            this.paletteColors = 256;
            this.transparencyIndex = -1;
            this.hashTable.Clear();

            this.palette[0].Alpha = 0;
            this.palette[0].Red = 0;
            this.palette[0].Green= 0;
            this.palette[0].Blue = 0;
            this.palette[1].Alpha = 0;
            this.palette[1].Red = (T)(Object)0x80;
            this.palette[1].Green= 0;
            this.palette[1].Blue = 0;
            this.palette[2].Alpha = 0;
            this.palette[2].Red = 0;
            this.palette[2].Green = (T)(Object)0x80;
            this.palette[2].Blue = 0;
            this.palette[3].Alpha = 0;
            this.palette[3].Red = (T)(Object)0x80;
            this.palette[3].Green = (T)(Object)0x80;
            this.palette[3].Blue = 0;
            this.palette[4].Alpha = 0;
            this.palette[4].Red = 0;
            this.palette[4].Green= 0;
            this.palette[4].Blue = (T)(Object)0x80;
            this.palette[5].Alpha = 0;
            this.palette[5].Red = (T)(Object)0x80;
            this.palette[5].Green= 0;
            this.palette[5].Blue = (T)(Object)0x80;
            this.palette[6].Alpha = 0;
            this.palette[6].Red = 0;
            this.palette[6].Green = (T)(Object)0x80;
            this.palette[6].Blue = (T)(Object)0x80;
            this.palette[7].Alpha = 0;
            this.palette[7].Red = (T)(Object)0xC0;
            this.palette[7].Green = (T)(Object)0xC0;
            this.palette[7].Blue = (T)(Object)0xC0;
            this.palette[8].Alpha = 0;
            this.palette[8].Red = (T)(Object)0xC0;
            this.palette[8].Green = (T)(Object)0xDC;
            this.palette[8].Blue = (T)(Object)0xC0;
            this.palette[9].Alpha = 0;
            this.palette[9].Red = 0xA6;
            this.palette[9].Green = 0xCA;
            this.palette[9].Blue = 0xF0;
            this.palette[246].Alpha = 0;
            this.palette[246].Red = 255;
            this.palette[246].Green = 0xFB;
            this.palette[246].Blue = 0xF0;
            this.palette[247].Alpha = 0;
            this.palette[247].Red = 0xA0;
            this.palette[247].Green = 0xA0;
            this.palette[246].Blue = 0xA4;
            this.palette[248].Alpha = 0;
            this.palette[248].Red = (T)(Object)0x80;
            this.palette[248].Green = (T)(Object)0x80;
            this.palette[248].Blue = (T)(Object)0x80;
            this.palette[249].Alpha = 0;
            this.palette[249].Red = 255;
            this.palette[249].Green= 0;
            this.palette[249].Blue = 0;
            this.palette[250].Alpha = 0;
            this.palette[250].Red = 255;
            this.palette[250].Green= 0;
            this.palette[250].Blue = 255;
            this.palette[251].Alpha = 0;
            this.palette[251].Red = 255;
            this.palette[251].Green = 255;
            this.palette[251].Blue = 0;
            this.palette[252].Alpha = 0;
            this.palette[252].Red = 0;
            this.palette[252].Green= 0;
            this.palette[252].Blue = 255;
            this.palette[253].Alpha = 0;
            this.palette[253].Red = 255;
            this.palette[253].Green= 0;
            this.palette[253].Blue = 255;
            this.palette[254].Alpha = 0;
            this.palette[254].Red = 0;
            this.palette[254].Green = 255;
            this.palette[254].Blue = 255;
            this.palette[255].Alpha = 0;
            this.palette[255].Red = 255;
            this.palette[255].Green = 255;
            this.palette[255].Blue = 255;

            for (int i = 10; i <= 245; i++)
            {
                this.palette[0].Alpha = 0;
                this.palette[i].Red = (T)(Object)i;
                this.palette[i].Green = (T)(Object)i;
                this.palette[i].Blue = (T)(Object)i;
            }
        }
        */

        /**
         * Color number	Binary value	BRIGHT 0 (RGB)	BRIGHT 1 (RGB)	Color name
0	000	#000000	#000000	black
1	001	#0000D7	#0000FF	blue
2	010	#D70000	#FF0000	red
3	011	#D700D7	#FF00FF	magenta
4	100	#00D700	#00FF00	green
5	101	#00D7D7	#00FFFF	cyan
6	110	#D7D700	#FFFF00	yellow
7	111	#D7D7D7	#FFFFFF	white
         */
        /// <sumary>
        /// Creates a 256 entries Windows default color palette.
        /// Indexes [0;9] and [246;255] (20 colors) hare used by Windows, the others 
        /// can be set by the application.
        /// </sumary>
        public void CreateZxSpectrumPal()
        {
            this.NumberOfEntries = 16;

            // Black #000000
            this.palette[0].Alpha = 0;
            this.palette[0].Red = 0;
            this.palette[0].Green = 0;
            this.palette[0].Blue = 0;

            // Dark Blue #0000D7
            this.palette[1].Alpha = 0;
            this.palette[1].Red = 0;
            this.palette[1].Green = 0;
            this.palette[1].Blue = 0xD7;

            // Dark Red #D70000
            this.palette[2].Alpha = 0;
            this.palette[2].Red = 0xD7;
            this.palette[2].Green = 0;
            this.palette[2].Blue = 0;

            // Dark Magenta #D700D7
            this.palette[3].Alpha = 0;
            this.palette[3].Red = 0xD7;
            this.palette[3].Green = 0;
            this.palette[3].Blue = 0xD7;

            // Dark Green #00D700
            this.palette[4].Alpha = 0;
            this.palette[4].Red = 0;
            this.palette[4].Green = 0xD7;
            this.palette[4].Blue = 0;

            // Dark Cyan #00D7D7
            this.palette[5].Alpha = 0;
            this.palette[5].Red = 0;
            this.palette[5].Green = 0xD7;
            this.palette[5].Blue = 0xD7;

            // Dark Yellow #D7D700
            this.palette[6].Alpha = 0;
            this.palette[6].Red = 0xD7;
            this.palette[6].Green = 0xD7;
            this.palette[6].Blue = 0;

            // Gray #D7D7D7
            this.palette[7].Alpha = 0;
            this.palette[7].Red = 0xD7;
            this.palette[7].Green = 0xD7;
            this.palette[7].Blue = 0xD7;

            // Black #000000
            this.palette[8].Alpha = 0;
            this.palette[8].Red = 0;
            this.palette[8].Green = 0;
            this.palette[8].Blue = 0;

            // Blue #0000FF
            this.palette[9].Alpha = 0;
            this.palette[9].Red = 0;
            this.palette[9].Green = 0;
            this.palette[9].Blue = 255;

            // Red #FF0000
            this.palette[10].Alpha = 0;
            this.palette[10].Red = 255;
            this.palette[10].Green = 0;
            this.palette[10].Blue = 0;

            // Magenta #FF00FF
            this.palette[11].Alpha = 0;
            this.palette[11].Red = 255;
            this.palette[11].Green = 0;
            this.palette[11].Blue = 255;

            // Green #00FF00
            this.palette[12].Alpha = 0;
            this.palette[12].Red = 0;
            this.palette[12].Green = 255;
            this.palette[12].Blue = 0;

            // Cyan #00FFFF
            this.palette[13].Alpha = 0;
            this.palette[13].Red = 0;
            this.palette[13].Green = 255;
            this.palette[13].Blue = 255;

            // Yellow #FFFF00
            this.palette[14].Alpha = 0;
            this.palette[14].Red = 255;
            this.palette[14].Green = 255;
            this.palette[14].Blue = 0;

            // White #FFFFFF
            this.palette[15].Alpha = 0;
            this.palette[15].Red = 255;
            this.palette[15].Green = 255;
            this.palette[15].Blue = 255;
        }

        public int GetColorIndex(ColorEntryByteRGBA color)
        {
            uint key = 0;
            int index;
            
            if (this.hashTable.Count <= 0)
            {
                
                for (int i = 0; i < this.paletteColors; i++)
                {
                    key = ((uint)(Object)this.palette[i].Alpha << 24) | 
                           ((uint)(Object)this.palette[i].Red << 16) |
                           ((uint)(Object)this.palette[i].Green << 8) |
                           (uint)(Object)this.palette[i].Blue;

                    // The Add method throws an exception if the new key is 
                    // already in the dictionary.
                    try
                    {
                        this.hashTable.Add((uint)key, i);
                    }
                    catch (ArgumentException)
                    {
                        // No problem.
                        // If the palette have repeated colors, we use the first one.
                    }
                }
            }

            key = ((uint)(Object)color.Alpha << 24) | 
                   ((uint)(Object)color.Red << 16) | ((uint)color.Green << 8)|
                   (uint)(Object)color.Blue;
            if (this.hashTable.TryGetValue((uint)key, out index))
                return (int)index;
            else
            {
                uint min = uint.MaxValue;
                uint max = 0;

                foreach (KeyValuePair<uint, int> kvp in this.hashTable)
                {
                    if (kvp.Key < key)
                        min = kvp.Key;
                    if (kvp.Key > key)
                        max = kvp.Key;
                }

                if ((max - key) < (key - min))
                    this.hashTable.TryGetValue(max, out index);
                else
                    this.hashTable.TryGetValue(min, out index);
            }
            
            return index;
        }
    }
}
