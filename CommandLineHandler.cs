namespace PvZ_Replanter
{
    public class CommandLineHandler
    {
        public static void PrintHelp()
        {
            Console.WriteLine("-------  PvZ Replanter CLI  -------");
            Console.WriteLine("------- No Input Detected!  -------");
            Console.WriteLine("-------------- Usage --------------");
            Console.WriteLine("Commands:\n");
            Console.WriteLine("setup <Game Path>");
            Console.WriteLine("decompress -bunfle_flag");
            Console.WriteLine("unpack -bundle_flag");
            Console.WriteLine("patch -bundle_flag <.emip Mod Location>");
        }

        public static HashSet<string> GetFlags(string[] args)
        {
            HashSet<string> flags = new HashSet<string>();
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                    flags.Add(args[i]);
            }
            return flags;
        }

        public static void CLHMain(string[] args)
        {
            if (args.Length < 1)
            {
                PrintHelp();
                return;
            }

            string configPath = "./config/config.json";
            string command = args[0];

            if (command == "setup")
            {
                Utilities.InitialSetup(args[1]);
            }

            if (command == "decompress")
            {
                string file = FlagUtil.FlagChecker(args, configPath);
                Utilities.Decompresser(file);
            }

            if (command == "unpack")
            {
                string file = FlagUtil.FlagChecker(args, configPath);
                Utilities.Unpacker(file);
            }
            if (command == "patch")
            {
                string file = FlagUtil.FlagChecker(args, configPath);
                BundleManager.PackAPunch(file, args[2]);
            }
            if (command == "flag")
            {
                FlagUtil.FlagChecker(args, configPath);
                PrintHelp();
            }
        }
    }
}
