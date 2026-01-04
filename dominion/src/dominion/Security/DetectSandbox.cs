using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectSandbox
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("[DEBUG] [SECURITY] [*] DetectSandbox.cs");

                int detectionFlags = 0;
                int threshold = 3;

                // 1. Check for known sandbox/VM processes
                string[] sandboxProcesses =
                {
                    "sandboxie", "cuckoo", "joebox", "anubis",
                    "vboxservice", "vmtoolsd", "vmsrvc", "vboxtray"
                };

                foreach (var process in Process.GetProcesses())
                {
                    try
                    {
                        string pname = process.ProcessName.ToLower();
                        if (sandboxProcesses.Any(p => pname.Contains(p)))
                            detectionFlags++;
                    }
                    catch { }
                }

                string[] registryKeys =
                {
                    @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Sandboxie",
                    @"SOFTWARE\\Sandboxie",
                    @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\VMware Tools",
                    @"SOFTWARE\\Oracle\\VirtualBox Guest Additions",
                    @"SOFTWARE\\VMware, Inc.\\VMware Tools"
                };

                foreach (string key in registryKeys)
                {
                    using (var k = Registry.LocalMachine.OpenSubKey(key))
                    {
                        if (k != null)
                            detectionFlags += 2;
                    }
                }

                using (var searcher = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem"))
                {
                    foreach (var item in searcher.Get())
                    {
                        string manufacturer = item["Manufacturer"]?.ToString().ToLower() ?? "";
                        string model = item["Model"]?.ToString().ToLower() ?? "";

                        if (manufacturer.Contains("vmware") ||
                            manufacturer.Contains("virtualbox") ||
                            manufacturer.Contains("kvm") ||
                            manufacturer.Contains("xen") ||
                            manufacturer.Contains("qemu") ||
                            (manufacturer.Contains("microsoft corporation") && model.Contains("virtual")))
                        {
                            detectionFlags += 3;
                        }
                    }
                }

                // 4. Environment indicators — CPU cores < 2 suggests VM
                if (Environment.ProcessorCount < 2)
                    detectionFlags++;

                // 5. Check total RAM size (via WMI)
                double ramInGB = GetTotalMemoryInGB();
                if (ramInGB < 2.0)
                    detectionFlags++;

                // 6. Check system uptime (under 3 minutes often means sandbox)
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();
                    Thread.Sleep(1000);
                    float systemUptime = uptime.NextValue();

                    if (systemUptime < 180 && detectionFlags > 0)
                        detectionFlags++;
                }

                // 7. Final action
                if (detectionFlags >= threshold)
                {
                    Console.WriteLine("[SECURITY] Sandbox detected — created warning files.");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] DetectSandbox: " + ex.Message);
            }
        }

        // Helper: Get total installed RAM in GB
        private static double GetTotalMemoryInGB()
        {
            double totalMemory = 0;
            using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            {
                foreach (var obj in searcher.Get())
                {
                    totalMemory += Convert.ToDouble(obj["Capacity"]);
                }
            }
            return totalMemory / (1024 * 1024 * 1024);
        }
    }
}
