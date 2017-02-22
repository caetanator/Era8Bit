/**
 * MediaViwer.cs
 *
 * PURPOSE
 *  Show the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *
 * CONTACTS
 *  E-mail regarding any portion of the "Era 8-bit Media Viwer" project:
 *      José Caetano Silva, jcaetano@users.sourceforge.net
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2016-2017 José Caetano Silva
 *
 * HISTORY
 *  2016-12-01: Created.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CaetanoSof.Era8Bit.Library8Bit.MediaFormats;

namespace CaetanoSof.Era8Bit.Programs.MediaViwer
{
    public class MediaViwer
    {
        private static void WriteError(String strErrorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(strErrorMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void WriteProperty(String strProperty, String strValue)
        {
            if(strProperty != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("{0}: ", strProperty);
            }

            Console.ForegroundColor = ConsoleColor.White;
            if (strValue != null)
            {
                Console.Write(strValue);
            }
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            // Set console colors
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();

            // 
            try
            {
                String[] cartridges = {
                    @"C:\Users\JCaetano\Desktop\Emulators\Sinclair\Programs\Timex TC2068\Cartridges\Timex Dock\OS\Spectrum Emulator\TS2048.DCK",
                    @"C:\Users\JCaetano\Desktop\Emulators\Sinclair\Programs\Timex TC2068\Cartridges\Timex Dock\OS\Spectrum Emulator\SPECEMU.dck",
                    @"C:\Users\JCaetano\Desktop\Emulators\Sinclair\Programs\Timex TC2068\Cartridges\Timex Dock\OS\TC2048 Emulator\Emulator.dck",
                    @"C:\Users\JCaetano\Desktop\Emulators\Sinclair\Programs\Timex TC2068\Cartridges\Timex Dock\Office\Time Word\Time Word.dck",
                    @"C:\Users\JCaetano\Desktop\Emulators\Sinclair\Programs\Timex TC2068\Cartridges\Timex Dock\Games\CrazyBugs\CrazyBugs.dck",
                    @"C:\Users\JCaetano\Desktop\Emulators\Sinclair\Programs\Timex TC2068\Cartridges\Timex Dock\Games\Chess\Chess.dck" 
                };
                foreach (var cartridge in cartridges)
                {
                    TimexCartridge timexCartridge = new TimexCartridge(cartridge);
                    List<String[]> info = timexCartridge.GetInfo();
                    foreach (var item in info)
                    {
                        WriteProperty(item[0], item[1]);
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                WriteError("Exception Error: ");
                WriteError(ex.Message);
            }
            
            Console.WriteLine();

            Console.ResetColor();
            String str = Console.ReadLine();
        }
    }
}
