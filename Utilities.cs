using AssetsTools.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PvZ_Replanter
{
    public class Utilities
    {
        public static void InitialSetup(string args)
        {
            string configPath = "./config/config.json";

            BundleManager.FileSystemCheck("mods");
            BundleManager.FileSystemCheck("config");
            BundleManager.FileSystemCheck("backup");
            BundleManager.FileSystemCheck("mods");

            string game_location_read = args;
            Console.WriteLine("Alright, so it is " + game_location_read + ", lemme write that down");
            string spine_bundle_read = game_location_read + "\\Replanted_Data\\StreamingAssets\\aa\\StandaloneWindows64\\spineassets_assets_assets\\art\\characters\\spine.bundle";
            string preview_bundle_read = game_location_read + "\\Replanted_Data\\StreamingAssets\\aa\\StandaloneWindows64\\defaultlocalgroup_assets_assets\\art\\characters\\previews_1c8b07f2ed4b49e58563b64b05c4c5fb.bundle";
            string music_bundle_read = game_location_read + "\\Replanted_Data\\StreamingAssets\\aa\\StandaloneWindows64\\music_assets_all_c79897e4dc1bedcab76c0bb920650d5d.bundle";
            string almanac_bundle_read = game_location_read + "\\Replanted_Data\\StreamingAssets\\aa\\StandaloneWindows64\\uiatlasalmanac_assets_all.bundle";

            var config = new
            {
                spine_bundle_location = spine_bundle_read,
                preview_bundle_location = preview_bundle_read,
                music_bundle_location = music_bundle_read,
                almanac_bundle_location = almanac_bundle_read
            };

            string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, jsonString);

            Console.WriteLine("There, done!");
        }

        public static void Decompresser(string file)
        {

            if (file != null)
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine("There is no file to decompress! ");
                    return;
                }

                Console.WriteLine("Starting Decompresser!");
                AssetBundleFile bun_spine = BundleManager.DecompressBundle(file);
                bun_spine.Close();
                Console.WriteLine($"The bundle {Path.GetFileName(file)} has been decompressed successfully!");
                return;
            }
            Console.WriteLine("Error!");
        }

        public static void Unpacker(string file)
        {
            if (file != null)
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine("There is no bundle to unpack!");
                    return;
                }

                Console.WriteLine("Starting Unpacker");
                BundleManager.UnpackBundle(file);
                Console.WriteLine($"CAB files for {Path.GetFileName(file)} have been unpacked successfully!");
                return;
            }
            Console.WriteLine("Error!");
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
