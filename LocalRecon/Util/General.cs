using System;
using System.Diagnostics;
using System.IO;

namespace LocalRecon.Util
{
	class General
	{
		public static void RunCommand(string command)
		{
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = "cmd.exe";
			p.StartInfo.Arguments = "/q/c" + command;
			p.Start();
			string output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("-------------- Command Output --------------");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(output);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("-------------------------------------------");
			Console.ForegroundColor = ConsoleColor.White;
		}

        public static void GetMappedDrives()
        {
            PrintSectionHeader("Mapped Network Drives");

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                try
                {
                    Console.WriteLine(drive.Name + " - " + drive.VolumeLabel);
                }
                catch
                {
                    Console.WriteLine(drive.Name);

                }
            }

            PrintSectionFooter();
        }

        public static void PrintSectionHeader(string header)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"-------------- {header} --------------");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintSectionFooter()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
