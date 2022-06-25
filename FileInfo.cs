namespace DirectorySyncer
{
    public class FileInfo
    {
        public string FilenameRelative { get; set; } = "";
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long FileSize{get;set;}

        public override string ToString()
        {
            return FilenameRelative + " - " + CreateDate.ToString("yyyy-MM-dd HH:mm") + " - " + ModifiedDate.ToString("yyyy-MM-dd HH:mm");
        }
    }
}