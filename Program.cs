using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace DirectorySyncer
{
    public class Program
    {
        static public void Main(string[] args)
        {
            // Setup configuration files
            using IHost host = Host.CreateDefaultBuilder(args).Build();
            Config.LoadConfig();

            string originDir = Config.OriginDirectory;
            string destinationDIr = Config.DestinationDirectory;

            // Parse a list of all the files in the original dir
            var originFiles = DirectoryLoader.LoadDirectory(originDir, originDir);
            var destFiles = DirectoryLoader.LoadDirectory(destinationDIr, destinationDIr);

            Console.WriteLine("Origin files: " + originFiles.Count());

            // Find all missing files
            var missing = originFiles.Where(x => destFiles.Where(y => y.FilenameRelative == x.FilenameRelative).Count() == 0);

            Console.WriteLine("Missing files: " + missing.Count());

            // Find all modified files
            var modified = originFiles.Where(x => destFiles.Where(y => y.FilenameRelative == x.FilenameRelative && (x.ModifiedDate > y.ModifiedDate || x.FileSize != y.FileSize)).Count() > 0);

            Console.WriteLine("Modified files: " + modified.Count());

            int missingProgress = 0;

            var missingTask = new Task(() =>
            {
                foreach (var missingFile in missing)
                {
                    string originPath = originDir + missingFile.FilenameRelative;
                    string destPath = destinationDIr + missingFile.FilenameRelative;

                    var fileDir = missingFile.FilenameRelative.Substring(0, missingFile.FilenameRelative.LastIndexOf("\\"));

                    var destFileDir = destinationDIr + fileDir;
                    if (Directory.Exists(destFileDir) == false)
                        Directory.CreateDirectory(destFileDir);

                    Console.WriteLine(missingFile);

                    File.Copy(originPath, destPath);

                    missingProgress++;

                    if (missingProgress % 100 == 0)
                        Console.WriteLine(missingProgress);
                }

                Console.WriteLine("Missing done!");
            });

            var modifiedTask = new Task(() =>
            {
                foreach (var modifiedFile in modified)
                {
                    string originPath = originDir + modifiedFile.FilenameRelative;
                    string destPath = destinationDIr + modifiedFile.FilenameRelative;

                    File.Copy(originPath, destPath, true);

                    missingProgress++;

                    if (missingProgress % 100 == 0)
                        Console.WriteLine(missingProgress);
                }

                Console.WriteLine("Modified done!");
            });

            if (Config.SkipMoving == false)
            {
                System.Console.WriteLine();
                Console.WriteLine("Starting transfer...");
                missingTask.Start();
                modifiedTask.Start();

                missingTask.Wait();
                modifiedTask.Wait();
            }

            Console.WriteLine("Complete!");
        }

    }
}