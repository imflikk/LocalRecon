using System;
using System.Management;
using System.ServiceProcess;


namespace LocalRecon.Util
{
	class ServiceEnum
	{
		static ManagementObjectCollection GetServices()
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
			return searcher.Get();
		}
		
		public static void GetLocalServices()
		{
			ManagementObjectCollection collection = GetServices();

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("-------------- All Services --------------");
			Console.ForegroundColor = ConsoleColor.White;

			foreach (ManagementObject obj in collection)
			{
				string name = obj["Name"].ToString().PadRight(40);
				string pathName;

				if (obj["PathName"] != null)
				{
					pathName = obj["PathName"].ToString().PadRight(60);
				}
				else
				{
					pathName = "N/A".PadRight(60);
				}
				
				Console.WriteLine($"{name}{pathName}");
				
			}
		}

		public static void GetUnquotedServices()
		{
			ManagementObjectCollection collection = GetServices();

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("-------------- Unquoted Services --------------");
			Console.ForegroundColor = ConsoleColor.White;

			foreach (ManagementObject obj in collection)
			{
				string name = obj["Name"].ToString().PadRight(40);
				string pathName;

				if (obj["PathName"] != null)
				{
					pathName = obj["PathName"].ToString().PadRight(60);

					if (pathName[0] != '"' && !pathName.Contains("Windows") && !pathName.Contains("windows") && pathName.Contains(" "))
					{
						Console.WriteLine($"{name}{pathName}");
					}
				}

			}
		}
	}
}
