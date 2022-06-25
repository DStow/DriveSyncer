
namespace DirectorySyncer
{
    public class Program
    {
        static public void Main(string[] args)
        {
            string originDir = "P:\\";
            string destinationDIr = "\\destination\\";

            // Parse a list of all the files in the original dir
            var originFiles = DirectoryLoader.LoadDirectory(originDir);

            Console.WriteLine(originFiles.Count);
        }

    }
}