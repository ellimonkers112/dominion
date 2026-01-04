using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectAnyrunMac
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("[DEBUG] [SECURITY] [*] DetectAnyrunMac.cs");

                const string anyrunmac = "52:54:00:4A:04:AF";
                List<string> macAddresses = new List<string>();

                var searcher = new ManagementClass("Win32_NetworkAdapterConfiguration");
                foreach (ManagementObject obj in searcher.GetInstances())
                {
                    try
                    {
                        if (obj["IPEnabled"] is bool ipEnabled && ipEnabled &&
                            obj["MacAddress"] is string mac)
                        {
                            macAddresses.Add(mac);
                        }
                    }
                    catch { }
                }

                foreach (var mac in macAddresses)
                {
                    if (string.Equals(mac, anyrunmac, StringComparison.OrdinalIgnoreCase))
                    {
                        Environment.Exit(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SECURITY] Error in DetectAnyrunMac: " + ex.Message);
            }
        }
    }
}
