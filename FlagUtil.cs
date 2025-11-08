using System.Text.Json;

namespace PvZ_Replanter
{
    public class FlagUtil
    {
        public static string FlagChecker(string[] args, string configPath)
        {

            HashSet<string> flags = CommandLineHandler.GetFlags(args);

            if (File.Exists(configPath))
            {
                string jsonString = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<Config>(jsonString);

                if (config != null)
                {
                    if (flags.Contains("-spine"))
                    {
                        return config.spine_bundle_location;
                    }

                    if (flags.Contains("-preview"))
                    {
                        return config.preview_bundle_location;
                    }

                    if (flags.Contains("-music"))
                    {
                        return config.music_bundle_location;
                    }

                    if (flags.Contains("-almanac"))
                    {
                        return config.almanac_bundle_location;
                    }
                }
            }

            Console.WriteLine("No config file! Delete the config folder and restart the program");
            return "0";
        }
        class Config
        {
            public string spine_bundle_location { get; set; }
            public string preview_bundle_location { get; set; }
            public string music_bundle_location { get; set; }
            public string almanac_bundle_location { get; set; }
        }
    }
}
