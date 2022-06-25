using System;
using System.Collections.Generic;

namespace DirectorySyncer
{
    static class DirectoryLoader
    {
        public static List<FileInfo> LoadDirectory(string dir, string originDir)
        {
            var results = new List<FileInfo>();

            foreach (var file in Directory.GetFiles(dir))
            {
                var fileInfo = new System.IO.FileInfo(file);

                results.Add(new FileInfo(){
                    FilenameRelative = file.Replace(originDir, ""),
                    CreateDate = fileInfo.CreationTimeUtc,
                    ModifiedDate = fileInfo.LastWriteTimeUtc,
                    FileSize = fileInfo.Length
                });
            }

            foreach (var subDir in Directory.GetDirectories(dir))
            {
                results.AddRange(LoadDirectory(subDir, originDir).ToArray());
            }

            return results;
        }
    }
}