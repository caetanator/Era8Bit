/**
 * MediaViewer.cs
 *
 * PURPOSE
 *  Show the contents of 8-bit related files (.TZX, .TAP, .DCK, .IMG, etc.).
 *
 * CONTACTS
 *  E-mail regarding any portion of the "Era 8-bit Media Viewer" project:
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
using System.IO;

namespace CaetanoSof.Era8Bit.Programs.MediaViwer
{
    public class MediaViewer
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

        public static void ProcessFile(string mediaFilePath)
        {
            IMediaFormat mediaObject = MediaFactory.Instance.GetMediaHandler(mediaFilePath);
            if (mediaObject != null)
            {
                List<String[]> info = mediaObject.GetInfo();
                if ((info != null) && (info.Capacity > 0))
                {
                    foreach (var item in info)
                    {
                        WriteProperty(item[0], item[1]);
                    }
                }
                else
                {
                    FileStream mediaStream = null;
                    try
                    {
                        mediaStream = new FileStream(mediaFilePath, FileMode.Open, FileAccess.Read);
                    }
                    catch (Exception)
                    {
                        mediaStream = null;
                    }

                    WriteProperty("File Name", mediaFilePath);
                    WriteProperty("File Size", (mediaStream != null) ? (mediaStream.Length.ToString()) : ("File Not Found"));
                    WriteProperty("File Type Description", mediaObject.Description);
                }
            }
            else
            {
                FileStream mediaStream = null;
                try
                {
                    mediaStream = new FileStream(mediaFilePath, FileMode.Open, FileAccess.Read);
                }
                catch (Exception)
                {
                    mediaStream = null;
                }

                WriteProperty("File Name", mediaFilePath);
                WriteProperty("File Size", (mediaStream != null) ? (mediaStream.Length.ToString()) : ("File Not Found"));
                WriteProperty("File Type Description", "Unknown Media Format");
            }

            Console.WriteLine();
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory);
            }
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Usage: MediaViewer8Bit [file | directory] [file | directory] ...");
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            if(args.Length <= 0)
            {
                PrintUsage();
                return;
            }

            // Set console colors
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();

            // Prosses files
            try
            {
                foreach (string path in args)
                {
                    if (File.Exists(path))
                    {
                        // This path is a file
                        ProcessFile(path);
                    }
                    else if (Directory.Exists(path))
                    {
                        // This path is a directory
                        ProcessDirectory(path);
                    }
                    else
                    {
                        WriteError(String.Format("{0} is not a valid file or directory.", path));
                    }
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
