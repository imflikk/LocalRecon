﻿using System;
using System.IO;
using System.Net.NetworkInformation;
using LocalRecon.Util;

namespace LocalRecon
{
	class LocalRecon
	{
		static void Main(string[] args)
		{
			//Console.BackgroundColor = ConsoleColor.Black;
			//Console.ForegroundColor = ConsoleColor.White;
			//Console.Clear();

			RunChecks();

			// Debugging statement to stop immediate exit
			Console.ReadLine();
		}


		// Run all checks at once for now.  Will split up later to be called based on command-line args
		static void RunChecks()
		{

			Console.WriteLine("[*] Starting all checks...");
			string separator = "==========================";

            Console.WriteLine(separator);
			UserEnum.GetCurrentUserInfo();
            Console.WriteLine(separator);
			UserEnum.GetUserGroups();
            Console.WriteLine(separator);
            UserEnum.GetUsers();
            Console.WriteLine(separator);
            NetworkEnum.GetListeningPorts();
            Console.WriteLine(separator);
            ServiceEnum.GetLocalServices();
            Console.WriteLine(separator);
			General.GetMappedDrives();

        }



		// Keep this in case I want the interactive prompt later
		/*static void PromptUser()
		{
			bool running = true;
			string separator = "==========================";

			UserEnum.GetCurrentUserInfo();

			while (running)
			{
				Console.WriteLine(separator);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Choose an option:");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine("\t1 - Run system command");
				Console.WriteLine("\t2 - Show local users/administrators");
				Console.WriteLine("\t3 - Show listening ports/services");
				Console.WriteLine("\t4 - Show local services");
				Console.WriteLine("\t99 - Run all checks");
				Console.WriteLine("\n\t0 - Exit program");
				Console.WriteLine(separator);

				Console.Write("=> ");

				// Check if entered value is a number
				int reconChoice;
				if (!(int.TryParse(Console.ReadLine(), out reconChoice)))
				{
					Console.WriteLine("Please enter a number.");
					continue;
				}

				switch (reconChoice)
				{
					case 0:
						Console.WriteLine("[*] Exiting!");
						running = false;
						break;
					case 1:
						Console.WriteLine(separator);
						Console.Write("Command => ");
						General.RunCommand(Console.ReadLine());
						break;
					case 2:
						Console.WriteLine(separator);
						UserEnum.GetUsers();
						break;
					case 3:
						Console.WriteLine(separator);
						NetworkEnum.GetListeningPorts();
						break;
					case 4:
						Console.WriteLine(separator);
						ServiceEnum.GetLocalServices();
						break;
					case 99:
						Console.WriteLine(separator);
						UserEnum.GetUsers();
						NetworkEnum.GetListeningPorts();
						ServiceEnum.GetLocalServices();
						break;
					default:
						Console.WriteLine("Unknown command, try again.");
						break;

				}
			}
		}*/

	}
}
