using AssetsTools.NET;
using AssetsTools.NET.Extra;
using PvZ_Replanter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Replanter
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                CommandLineHandler.CLHMain(args);
            }
            else
            {
                CommandLineHandler.PrintHelp();
            }
        }
    }
}