using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuplicateFileChecker
{
    class Program
    {
        /// <summary>
        /// Takes a list of hardcoded paths and searches for duplicate files by comparing size, extension and 64 bytes from the middle of the file.
        /// Automatically removes duplicates afterwards, so set a breakpoint if you're not sure what to do
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Enter all paths to be searched here
            string[] allPaths = new string[] { @"E:\Google Drive\Large Data\Alte Bilder - Wenn ein Bild verloren geht hier suchen" };
            List<string> allFilesList = new List<string>();
            foreach (var path in allPaths)
            {
                allFilesList.AddRange(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories));
            }

            //List of all the files in all listed directories and sub-directories
            string[] allfiles = allFilesList.Distinct().ToArray();
            Dictionary<(string, long, string), List<string>> duplicateFiles = new Dictionary<(string, long, string), List<string>>();
            Console.WriteLine($"Found {allfiles.Length} files.");
            double totalFiles = allfiles.Length;
            double currentFileIndex = 0;

            //Iterate through all files
            foreach (var file in allfiles)
            {
                if ((int)currentFileIndex % 1000 == 0)
                {
                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {(currentFileIndex / totalFiles * 100):n2}% finished.");
                }
                try
                {
                    //Get the file, its size, its extension and the middle 64 bytes as a semi-solid way to determine duplicates
                    var fi = new FileInfo(file);
                    long size = fi.Length;
                    string extension = fi.Extension.ToLower();
                    byte[] buffer = new byte[64];
                    using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open)))
                    {
                        reader.BaseStream.Seek(size / 2, SeekOrigin.Begin);
                        reader.Read(buffer, 0, 64);
                    }
                    string firstBytes = new string(buffer.Select(b => b.ToString()).SelectMany(x => x).ToArray());
                    var key = (extension, size, firstBytes);

                    //Add file to list of duplicates
                    if (duplicateFiles.ContainsKey(key))
                    {
                        //A file with the same key has been found already, add it to the group
                        //Console.WriteLine($"Found a duplicate: {file}");
                        duplicateFiles[key].Add(file);
                    }
                    else
                    {
                        //The key is unique at this point, add a new group
                        duplicateFiles.Add(key, new List<string>() { file });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Encountered error: {e}.\nFile in question: {file}");
                }
                currentFileIndex++;
            }

            //Get all groups that are actually duplicates
            var duplicates = duplicateFiles.Where(f => f.Value.Count > 1);

            //Go over all duplicate groups and remove them
            foreach (var duplicate in duplicates)
            {
                Console.WriteLine("New Group: " + duplicate.Key.Item2);
                foreach (var file in duplicate.Value)
                {
                    Console.WriteLine(file);
                }
                Console.WriteLine("-----------------");
                File.Delete(duplicate.Value.First());
            }
        }
    }
}
