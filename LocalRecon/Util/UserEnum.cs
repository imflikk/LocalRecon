using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Management;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace LocalRecon.Util
{
	class UserEnum
	{
		public static void GetCurrentUserInfo()
		{
			string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			string domainName;
			bool domainJoined = false;
			try
			{
				domainName = Domain.GetCurrentDomain().Name;
				domainJoined = true;
            }
			catch {
				domainName = Environment.MachineName;

            }
			

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------------- Overview --------------");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"Current user: \t{userName}");
			Console.WriteLine($"Hostname: \t{Environment.MachineName}");
			Console.WriteLine($"Domain joined: \t{domainJoined}");

			NetworkEnum.GetNetworkInterfaces();

            General.PrintSectionFooter();
        }

		// Uses this as a reference for users and groups.  Uses WMI
		// https://chakkaradeep.wordpress.com/2007/10/14/enumerating-users-using-wminet-and-c/
		public static void GetUsers()
		{
			SelectQuery sQuery = new SelectQuery("Win32_UserAccount", $"Domain='{Environment.MachineName}'");

			List<string> localAdmins = GetLocalAdmins();

			General.PrintSectionHeader("User Accounts");

			try
			{
				ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(sQuery);

				foreach (ManagementObject mObject in mSearcher.Get())
				{
					if (localAdmins.Contains(mObject["Name"].ToString()))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"(Admin)\t{mObject["Name"]}");
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						Console.WriteLine($"\t{mObject["Name"]}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

            General.PrintSectionFooter();
        }


		// Reference for getting names of local admins, though needed to use Regex to get just the name (there's probably a better way)
		// https://stackoverflow.com/questions/20583959/find-available-data-in-an-managementobject
		static List<string> GetLocalAdmins()
		{
			SelectQuery sQuery = new SelectQuery("Win32_GroupUser", $"GroupComponent=\"Win32_Group.Domain='{Environment.MachineName}',Name='Administrators'\"");

			List<string> localAdmins = new List<string>();

			try
			{
				ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(sQuery);

				Console.ForegroundColor = ConsoleColor.Green;
				//Console.WriteLine("-------------- Local Administrators --------------");
				Console.ForegroundColor = ConsoleColor.White;

				// Get the WMI class
				ManagementClass processClass =
					new ManagementClass("Win32_GroupUser");
				processClass.Options.UseAmendedQualifiers = true;

				foreach (ManagementObject mObject in mSearcher.Get())
				{
					foreach (PropertyData prop in mObject.Properties)
					{
						if (prop.Name == "PartComponent")
						{
							var pattern = ".*Name=\"(.*)\".*";
							var match = Regex.Match(prop.Value.ToString(), pattern);

							localAdmins.Add(match.Groups[1].ToString());
							//Console.WriteLine($"\t{match.Groups[1]}");
						}


					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			Console.WriteLine();

			return localAdmins;

		}

		public static void GetUserGroups()
		{
			General.PrintSectionHeader("Current User's Groups");

			List<string> userGroups = new List<string>();

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity != null)
            {
				// Add each group to a list
                foreach (IdentityReference group in identity.Groups)
                {
					userGroups.Add(group.Translate(typeof(NTAccount)).Value);
                }

				// Sort the list alphabetically and then print each one
				userGroups.Sort();
				foreach (string group in userGroups)
				{
                    Console.WriteLine(group);
                }
            }

            General.PrintSectionFooter();
        }
	}
}
