namespace DirectorySyncer
{
    public static class Logger
    {
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);

            using(var sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " - " + message);
            }
        }
    }
}