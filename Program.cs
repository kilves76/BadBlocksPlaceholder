using System;
using System.IO;
using System.Diagnostics;

namespace BadBlocksPlaceholder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: <program> [clean|drive] <directory|blockSize>");
                return;
            }

            string targetDir;
            if (args[0] == "clean")
            {
                if (!args[1].ToUpper().Contains("BADBLOCKPLACEHOLDERS"))
                {
                    Console.WriteLine("Only BadBlockPlaceholders folder is allowed to be cleaned");
                    return;
                }
                targetDir = args[1];
            }
            else
            {
                if (!int.TryParse(args[1], out int blockSize))
                {
                    Console.WriteLine("Block size must be a valid integer");
                    return;
                }
                var drive = new DriveInfo(args[0]);
                blockSize *= 1024;
                targetDir = CreateBlocks(drive, blockSize, args);
            }

            Validate(targetDir);
            Console.WriteLine("Done!");
        }

        private static int GetWriteThresholdTicks(string[] args)
        {
            return 250000 * (int.Parse(args[1]) / 1024);
        }

        private static string CreateBlocks(DriveInfo drive, int blockSize, string[] args)
        {
            var block = new byte[blockSize];
            var bbDir = Path.Combine(drive.RootDirectory.FullName, "BadBlockPlaceholders");
            var slowDir = Path.Combine(drive.RootDirectory.FullName, "SlowBlockPlaceholders");
            if (!Directory.Exists(bbDir))
                Directory.CreateDirectory(bbDir);
            if (!Directory.Exists(slowDir))
                Directory.CreateDirectory(slowDir);

            var targetDir = Path.Combine(bbDir, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            int index = 0;
            Stopwatch sw = new Stopwatch();
            int sw_write_treshold_ticks = GetWriteThresholdTicks(args);
            while (drive.AvailableFreeSpace > blockSize)
            {
                Console.WriteLine("Creating block #" + index);
                var filename = Path.Combine(targetDir, index + ".bad");
                ++index;
                sw.Restart();
                using (var filestream = File.OpenWrite(filename))
                {
                    filestream.Write(block, 0, blockSize);
                }
                sw.Stop();
                if (sw.ElapsedTicks > sw_write_treshold_ticks)
                {
                    System.IO.File.Move(filename, Path.Combine(slowDir, DateTime.Now.ToString("ddhhmmss") + index + ".bad"));
                    Console.WriteLine(filename + " was too slow. it took " + sw.ElapsedTicks.ToString() + " to write. Moved");
                }
            }
            return targetDir;
        }

        private static void Validate(string targetDir)
        {
            Console.WriteLine("Validating blocks");
            foreach (var file in Directory.GetFiles(targetDir))
            {
                try
                {
                    var res = File.ReadAllBytes(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                File.Delete(file);
            }
        }
    }
}
