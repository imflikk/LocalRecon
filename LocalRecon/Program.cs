﻿using System;
using LocalRecon.Util;

namespace LocalRecon
{
	class LocalRecon
	{
		static void Main(string[] args)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Clear();

			PromptUser();
		}

		static void PromptUser()
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
						Console.Write("Command => ");
						General.RunCommand(Console.ReadLine());
						break;
					case 2:
						Console.WriteLine(separator);
						UserEnum.GetUsers();
						break;
					case 3:
						NetworkEnum.GetListeningPorts();
						break;
					default:
						Console.WriteLine("Unknown command, try again.");
						break;

				}
			}
		}

	}
}
