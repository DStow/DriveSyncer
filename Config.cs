using Microsoft.Extensions.Configuration;

namespace DirectorySyncer
{
    public static class Config
    {
        public static int RuntimeMinutes { get; private set; }
        public static string[]? IgnoredPaths { get; private set; } = null;
        public static string? OriginDirectory { get; set; } = "";
        public static string? DestinationDirectory { get; set; } = "";
        public static bool SkipMoving { get; set; } = false;

        public static async void LoadConfig()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

            // Get values from the config given their key and their target type.
            OriginDirectory = config.GetValue<string>("OriginDirectory");
            DestinationDirectory = config.GetValue<string>("DestinationDirectory");
            IgnoredPaths = config.GetSection("IgnoredPaths").Get<string[]>();
            RuntimeMinutes = config.GetValue<int>("Runtime");
            SkipMoving = config.GetValue<bool>("SkipMoving");

            // Sanitize these into upper
            for (int i = 0; i < IgnoredPaths.Length; i++)
            {
                IgnoredPaths[i] = IgnoredPaths[i].ToUpper();
            }

            Logger.WriteLine("--CONFIGURATION--");
            Logger.WriteLine("Origin: " + OriginDirectory);
            Logger.WriteLine("Destination: " + DestinationDirectory);
            Logger.WriteLine("Skipped Folders: " + IgnoredPaths.Length);
            foreach (var path in IgnoredPaths) { Logger.WriteLine("\t" + path); }
            Logger.WriteLine("Runtime: " + RuntimeMinutes);
            Logger.WriteLine("Skip move: " + SkipMoving);
            Logger.WriteLine("-----------------");
            Logger.WriteLine("");
        }
    }
}