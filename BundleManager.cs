using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PvZ_Replanter
{
    public class BundleManager
    {
        public static AssetBundleFile DecompressBundle(string file)
        {
            string file_name = Path.GetFileName(file);
            FileSystemCheck($"./unpacked/{file_name}");
            string decompFile_name = $"{file_name}.decomp";

            AssetBundleFile bun = new AssetBundleFile();
            Stream original_file = File.OpenRead(file);
            AssetsFileReader reader_p = new AssetsFileReader(original_file);

            bun.Read(reader_p);

            if (bun.Header.GetCompressionType() != 0)
            {
                if (!File.Exists($"./backup/{file_name}"))
                {
                    File.Copy(file, $"./backup/{file_name}");
                }

                Stream new_file;
                new_file = File.Open($"./unpacked/{file_name}/{decompFile_name}", FileMode.Create, FileAccess.ReadWrite);
                AssetsFileWriter writter_p = new AssetsFileWriter(new_file);
                bun.Unpack(writter_p);

                new_file.Position = 0;
                original_file.Close();

                original_file = new_file;
                reader_p = new AssetsFileReader(original_file);

                bun = new AssetBundleFile();
                bun.Read(reader_p);
            }

            return bun;
        }

        public static void FileSystemCheck(string folder)
        {
            bool folder_exists = System.IO.Directory.Exists(folder);
            if (!folder_exists)
            {
                System.IO.Directory.CreateDirectory(folder);
            }
        }

        public static void UnpackBundle(string file)
        {
            string unpack_path = $"./unpacked/{Path.GetFileName(file)}/";
            string unpacked_path = unpack_path + $"{Path.GetFileName(file)}.decomp";

            if (!File.Exists(unpacked_path))
            {
                Console.WriteLine("No File to Unpack!");
                return;
            }

            AssetBundleFile bun = new AssetBundleFile();
            Stream decomp_file = File.OpenRead(unpacked_path);
            AssetsFileReader reader_p = new AssetsFileReader(decomp_file);
            bun.Read(reader_p);


            Console.WriteLine(unpack_path);
            Console.WriteLine(unpacked_path);

            
            int entryCount = bun.BlockAndDirInfo.DirectoryInfos.Length;
            for (int i = 0; i < entryCount; i++)
            {
                string name = bun.BlockAndDirInfo.DirectoryInfos[i].Name;
                byte[] data = BundleHelper.LoadAssetDataFromBundle(bun, i);

                string outName = Path.Combine(unpack_path, name);

                Console.WriteLine($"Unpacking CAB {i+1}... " );
                File.WriteAllBytes(outName, data);
            }

            bun.Close();
        }

        public static void PackAPunch (string file, string args)
        {

            string rootDir = $"./unpacked/{Path.GetFileName(file)}";
            string emipFile = args;

            if (Directory.Exists(rootDir)) {

                if (!File.Exists(emipFile))
                {
                    Console.WriteLine($"Mod File {Path.GetFileName(emipFile)} does not exist!");
                    return;
                }

                Console.WriteLine("Starting Patcher!");
                EmipPatcher(emipFile, rootDir);
                return; 

            }
            Console.WriteLine("Error!");
        }

        public static void EmipPatcher(string emipFile, string rootDir)
        {
            if (!File.Exists(emipFile))
            {
                Console.WriteLine($"File {emipFile} does not exist!");
                return;
            }

            InstallerPackageFile instPkg = new InstallerPackageFile();
            FileStream fs = File.OpenRead(emipFile);
            AssetsFileReader r = new AssetsFileReader(fs);
            instPkg.Read(r, true);

            Console.WriteLine($"Installing emip...");
            Console.WriteLine($"\n{instPkg.modName} by {instPkg.modCreators}\n");
            Console.WriteLine(instPkg.modDescription);

            foreach (var affectedFile in instPkg.affectedFiles)
            {
                string affectedFileName = Path.GetFileName(affectedFile.path);
                string affectedFilePath = Path.Combine(rootDir, affectedFile.path);

                string modFile = $"{affectedFilePath}.mod";
                string bakFile = GetNextBackup(affectedFilePath);

                if (bakFile == null)
                        return;

                FileStream afs = File.OpenRead(affectedFilePath);
                AssetsFileReader ar = new AssetsFileReader(afs);
                AssetsFile assets = new AssetsFile();
                assets.Read(ar);
                List<AssetsReplacer> reps = new List<AssetsReplacer>();

                foreach (var rep in affectedFile.replacers)
                {
                    var assetsReplacer = (AssetsReplacer)rep;
                    reps.Add(assetsReplacer);
                }

                Console.WriteLine($"Writing {modFile}...");
                FileStream mfs = File.Open(modFile, FileMode.Create);
                AssetsFileWriter mw = new AssetsFileWriter(mfs);
                assets.Write(mw, 0, reps, instPkg.addedTypes);

                mfs.Close();
                ar.Close();

                Console.WriteLine($"Swapping mod file...");
                File.Move(affectedFilePath, bakFile);
                File.Move(modFile, affectedFilePath);

                File.Delete(bakFile);

                Console.WriteLine($"Done!");
            }

            return;
        }

        public static string GetNextBackup(string affectedFilePath)
        {
            for (int i = 0; i < 10000; i++)
            {
                string bakName = $"{affectedFilePath}.bak{i.ToString().PadLeft(4, '0')}";
                if (!File.Exists(bakName))
                {
                    return bakName;
                }
            }

            Console.WriteLine("Too many backups, exiting for your safety.");
            return null;
        }
    }
}
