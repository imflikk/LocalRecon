using System;
using System.Net;
using System.Net.NetworkInformation;

namespace LocalRecon.Util
{
	class NetworkEnum
	{
		public static void GetNetworkInterfaces()
		{
			Console.WriteLine("Network Information:");
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
				{
					foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
					{
						if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
						{
							Console.WriteLine($"\t{ip.Address}\t({ni.Name})");
						}
					}
				}
			}
		}

		public static void GetListeningPorts()
		{
			General.PrintSectionHeader("Listening Ports");

			IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
			IPEndPoint[] endPoints = properties.GetActiveTcpListeners();
			foreach (IPEndPoint e in endPoints)
			{
				if (e.Address.ToString() == "127.0.0.1")
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"\t{e}\t(Localhost only)");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.WriteLine($"\t{e}");
				}
				
			}

            General.PrintSectionFooter();
        }
	}
}
