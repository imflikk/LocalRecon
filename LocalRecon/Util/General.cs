using System;
using System.Diagnostics;

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
	}
}
