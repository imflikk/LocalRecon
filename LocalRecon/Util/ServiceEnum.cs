using System;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using static LocalRecon.Native.Win32;

namespace LocalRecon.Util
{
    class ServiceEnum
    {


        // This isn't used anymore, but keeping it for now just in case
        static ManagementObjectCollection GetServices()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
            return searcher.Get();
        }

        public static void GetLocalServices()
        {
            ManagementObjectCollection collection = GetServices();

            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            var GetServiceHandle = typeof(System.ServiceProcess.ServiceController).GetMethod("GetServiceHandle", BindingFlags.Instance | BindingFlags.NonPublic);

            object[] readRights = { 0x00020000 };

            ServiceAccessRights[] ModifyRights =
            {
                ServiceAccessRights.ChangeConfig,
                ServiceAccessRights.WriteDac,
                ServiceAccessRights.WriteOwner,
                ServiceAccessRights.GenericAll,
                ServiceAccessRights.GenericWrite,
                ServiceAccessRights.AllAccess
            };

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------------- All Services --------------");
            Console.ForegroundColor = ConsoleColor.White;

            string name = "";
            string pathName = "";
            string unquoted = " ";
            string canRestart = "";

            //foreach (ManagementObject obj in collection)
            //{
            //    name = obj["Name"].ToString().PadRight(40);
            //    unquoted = "Not Vulnerable".PadLeft(((20 - "Not Vulnerable".Length) / 2) + "Not Vulnerable".Length).PadRight(20);
            //    access = "";

            //    if (obj["PathName"] != null)
            //    {
            //        pathName = obj["PathName"].ToString().PadRight(60);

            //        if (pathName[0] != '"' && !pathName.Contains("Windows") && !pathName.Contains("windows") && pathName.Contains(" "))
            //        {
            //            unquoted = "Vulnerable (unquoted)".PadLeft(((20 - unquoted.Length) / 2) + unquoted.Length).PadRight(20);
            //        }
            //    }
            //    else
            //    {
            //        pathName = "N/A".PadRight(60);
            //    }
            //}


            // This portion taken mostly from SharpUp because I wasn't sure how to read the service DACL
            // Reference: https://github.com/GhostPack/SharpUp/blob/master/SharpUp/Checks/ModifiableServices.cs
            // 
            foreach (ServiceController sc in scServices)
            {
                name = sc.ServiceName;
                unquoted = " ".PadLeft(((20 - " ".Length) / 2) + " ".Length).PadRight(25);
                canRestart = "".PadLeft(((12 - "".Length) / 2) + "".Length).PadRight(12);
                pathName = "";

                using (ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + name + "'"))
                {
                    wmiService.Get();
                    if (wmiService["PathName"] != null)
                        pathName = wmiService["PathName"].ToString();
					else
					{
                        pathName = "N/A";
					}
                    name = sc.ServiceName.PadRight(40);
                }

                if (pathName[0] != '"' && !pathName.Contains("Windows") && !pathName.Contains("windows") && pathName.Contains(" "))
                {
                    unquoted = "Vulnerable (unquoted)".PadLeft(((20 - unquoted.Length) / 2) + unquoted.Length).PadRight(25);
                }


                try
                {
                    IntPtr handle = (IntPtr)GetServiceHandle.Invoke(sc, readRights);
                    ServiceControllerStatus status = sc.Status;
                    byte[] psd = new byte[0];
                    uint bufSizeNeeded;
                    bool ok = QueryServiceObjectSecurity(handle, SecurityInfos.DiscretionaryAcl, psd, 0, out bufSizeNeeded);

                    if (!ok)
                    {
                        int err = Marshal.GetLastWin32Error();
                        if (err == 122 || err == 0)
                        { // ERROR_INSUFFICIENT_BUFFER
                            // expected; now we know bufsize
                            psd = new byte[bufSizeNeeded];
                            ok = QueryServiceObjectSecurity(handle, SecurityInfos.DiscretionaryAcl, psd, bufSizeNeeded, out bufSizeNeeded);
                        }
                        else
                        {
                            //throw new ApplicationException("error calling QueryServiceObjectSecurity() to get DACL for " + _name + ": error code=" + err);
                            continue;
                        }
                    }
                    if (!ok)
                    {
                        //throw new ApplicationException("error calling QueryServiceObjectSecurity(2) to get DACL for " + _name + ": error code=" + Marshal.GetLastWin32Error());
                        continue;
                    }

                    // get security descriptor via raw into DACL form so ACE ordering checks are done for us.
                    RawSecurityDescriptor rsd = new RawSecurityDescriptor(psd, 0);
                    RawAcl racl = rsd.DiscretionaryAcl;
                    DiscretionaryAcl dacl = new DiscretionaryAcl(false, false, racl);

                    WindowsIdentity identity = WindowsIdentity.GetCurrent();

                    foreach (System.Security.AccessControl.CommonAce ace in dacl)
                    {
                        if ((identity.Groups.Contains(ace.SecurityIdentifier) || ace.SecurityIdentifier == identity.User) &&
                            ace.AceType == AceType.AccessAllowed)
                        {
                            ServiceAccessRights serviceRights = (ServiceAccessRights)ace.AccessMask;
                            foreach (ServiceAccessRights ModifyRight in ModifyRights)
                            {
                                if ((ModifyRight & serviceRights) == ModifyRight)
                                {
                                    ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\cimv2", String.Format("SELECT * FROM win32_service WHERE Name LIKE '{0}'", sc.ServiceName));
                                    ManagementObjectCollection data = wmiData.Get();

                                    foreach (ManagementObject result in data)
                                    {
                                        //_isVulnerable = true;
                                        //_details.Add($"Service '{result["Name"]}' (State: {result["State"]}, StartMode: {result["StartMode"]})");
                                        canRestart = "Can Restart".PadLeft(((12 - "Can Restart".Length) / 2) + "Can Restart".Length).PadRight(12);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    continue;
                }


                if (unquoted.Contains("Vulnerable") || canRestart.Contains("Can Restart"))
				{
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{name}|{unquoted}|{canRestart}|{pathName}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
				else
				{
                    Console.WriteLine($"{name}|{unquoted}|{canRestart}|{pathName}");
                }
                

            }


        }
    }
}