/**
 * DCK_InfoViewer.cs
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
using System.IO;

using CaetanoSoft.Era8bit;
using CaetanoSoft.Era8bit.MediaFormats;

namespace CaetanoSof.Era8Bit.Programs.DCK_InfoViewer
{
    public class DCK_InfoViewer
    {
        private static void WriteError(String strErrorMessage)
        {
            if (Console.IsOutputRedirected == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(strErrorMessage);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Error.WriteLine(strErrorMessage);
            }
        }

        private static void WriteProperty(String strProperty, String strValue)
        {
            if(strProperty != null)
            {
                if (Console.IsOutputRedirected == false)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.Write("{0}: ", strProperty);
            }

            if (Console.IsOutputRedirected == false)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (strValue != null)
            {
                Console.Write(strValue);
            }
            Console.WriteLine();
        }

        public static void ProcessFile(string mediaFilePath)
        {
            IMediaFormat mediaObject = MediaFormatFactory.Instance.GetMediaHandler(mediaFilePath);
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
            //string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            //foreach (string subdirectory in subdirectoryEntries)
            //{
            //    ProcessDirectory(subdirectory);
            //}
        }

        public static string[] ExpandFilePaths(string[] args)
        {
            var fileList = new List<string>();

            foreach (var arg in args)
            {
                var substitutedArg = System.Environment.ExpandEnvironmentVariables(arg);

                var dirPart = Path.GetDirectoryName(substitutedArg);
                if (dirPart.Length == 0)
                    dirPart = ".";

                var filePart = Path.GetFileName(substitutedArg);

                foreach (var filepath in Directory.GetFiles(dirPart, filePart))
                    fileList.Add(filepath);
            }

            return fileList.ToArray();
        }

        /// <summary>Prints the usage, aka arguments for the "DCK_InfoViewer" program.</summary>
        public static void PrintUsage()
        {
            Console.WriteLine(".DCK Info Viewer - (c) 2016-2023 CaetanoSoft/José Caetano Silva");
            Console.WriteLine("Usage: DCK_InfoViewer [file | directory] [file | directory] ...");
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            if(args.Length <= 0)
            {
                // Error, no arguments passed
                PrintUsage();
                return;
            }

            // Expand file paths
            var fileList = ExpandFilePaths(args);

            // Set console colors and clear the screen
            if (Console.IsOutputRedirected == false)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
            }
            // Write the program name and copyright
            Console.WriteLine(".DCK Info Viewer - (c) 2016-2023 CaetanoSoft/José Caetano Silva");
            Console.WriteLine();
            // Change foreground color
            if (Console.IsOutputRedirected == false)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            // Process files
            try
            {
                foreach (string path in fileList)
                {
                    if (File.Exists(path))
                    {
                        // This path is a file
                        ProcessFile(path);
                    }
                    else if (Directory.Exists(path))
                    {
                        // This path is a directory
                        //ProcessDirectory(path);
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
            // Write a blank line
            Console.WriteLine();
            // Reset the console colors
            if (Console.IsOutputRedirected == false)
            {
                Console.ResetColor();
            }
#if DEBUG
            String str = Console.ReadLine();
#endif
        }
    }
}
