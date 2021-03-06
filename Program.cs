using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace DirectorySyncer
{
    public class Program
    {
        static public void Main(string[] args)
        {
            try
            {
                // Setup configuration files
                using IHost host = Host.CreateDefaultBuilder(args).Build();
                Config.LoadConfig();

                string originDir = Config.OriginDirectory;
                string destinationDir = Config.DestinationDirectory;

                // Parse a list of all the files in the original dir
                var originFiles = DirectoryLoader.LoadDirectory(originDir, originDir);
                var destFiles = DirectoryLoader.LoadDirectory(destinationDir, destinationDir);

                Logger.WriteLine("Origin files: " + originFiles.Count());

                // Find all missing files
                var missing = originFiles.Where(x => destFiles.Where(y => y.FilenameRelative == x.FilenameRelative).Count() == 0);

                Logger.WriteLine("Missing files: " + missing.Count());

                // Find all modified files
                var modified = originFiles.Where(x => destFiles.Where(y => y.FilenameRelative == x.FilenameRelative && (x.ModifiedDate > y.ModifiedDate || x.FileSize != y.FileSize)).Count() > 0);

                Logger.WriteLine("Modified files: " + modified.Count());

                int missingProgress = 0;

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var missingTask = new Task(() =>
                {
                    foreach (var missingFile in missing)
                    {
                        string originPath = originDir + missingFile.FilenameRelative;
                        string destPath = destinationDir + missingFile.FilenameRelative;


                        int extensionIndex = missingFile.FilenameRelative.LastIndexOf("\\");
                        if (extensionIndex == -1)
                        {
                            extensionIndex = 0;
                        }
                        var fileDir = missingFile.FilenameRelative.Substring(0, extensionIndex);

                        var destFileDir = destinationDir + fileDir;
                        if (Directory.Exists(destFileDir) == false)
                            Directory.CreateDirectory(destFileDir);

                        Logger.WriteLine(missingFile.ToString());

                        File.Copy(originPath, destPath);

                        missingProgress++;

                        if (missingProgress % 100 == 0)
                            Logger.WriteLine(missingProgress.ToString());

                        if (stopwatch.Elapsed.Minutes > Config.RuntimeMinutes)
                        {
                            Logger.WriteLine("Missing has hit the runtime limit. Exiting");
                            break;
                        }
                    }

                    Logger.WriteLine("Missing done!");
                });

                var modifiedTask = new Task(() =>
                {
                    foreach (var modifiedFile in modified)
                    {
                        string originPath = originDir + modifiedFile.FilenameRelative;
                        string destPath = destinationDir + modifiedFile.FilenameRelative;

                        File.Copy(originPath, destPath, true);

                        missingProgress++;

                        if (missingProgress % 100 == 0)
                            Logger.WriteLine(missingProgress.ToString());

                        if (stopwatch.Elapsed.Minutes > Config.RuntimeMinutes)
                        {
                            Logger.WriteLine("Modified has hit the runtime limit. Exiting");
                            break;
                        }
                    }

                    Logger.WriteLine("Modified done!");
                });

                if (Config.SkipMoving == false)
                {
                    Logger.WriteLine("");
                    Logger.WriteLine("Starting transfer...");
                    missingTask.Start();
                    modifiedTask.Start();

                    missingTask.Wait();
                    modifiedTask.Wait();
                }

                Logger.WriteLine("Complete!");
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
                Logger.WriteLine(ex.StackTrace.ToString());
            }
        }
    }
}