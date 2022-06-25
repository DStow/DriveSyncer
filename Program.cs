
namespace DirectorySyncer
{
    public class Program
    {
        static public void Main(string[] args)
        {
            string originDir = "C:\\Code\\DriveSyncer\\original\\";
            string destinationDIr = "C:\\Code\\DriveSyncer\\destination\\";

            // Parse a list of all the files in the original dir
            var originFiles = DirectoryLoader.LoadDirectory(originDir, originDir);
            var destFiles = DirectoryLoader.LoadDirectory(destinationDIr, destinationDIr);
            
        }

    }
}