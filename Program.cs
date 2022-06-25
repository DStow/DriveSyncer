
namespace DirectorySyncer
{
    public class Program
    {
        static public void Main(string[] args)
        {
            string originDir = "P:\\";
            string destinationDIr = "C:\\Code\\DriveSyncer\\destination\\";

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
        }

    }
}