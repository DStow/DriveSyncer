using System;
using System.Collections.Generic;

namespace DirectorySyncer
{
    static class DirectoryLoader
    {
        public static List<string> LoadDirectory(string dir)
        {
            var results = new List<string>();

            foreach (var file in Directory.GetFiles(dir))
            {
                results.Add(file);
            }

            foreach (var subDir in Directory.GetDirectories(dir))
            {
                results.AddRange(LoadDirectory(subDir).ToArray());
            }

            return results;
        }
    }
}