using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Linq;

namespace dominion.src.dominion.Security
{
    public class AntiVm
    {
        [DllImport("kernel32.dll")]
        private static extern bool IsProcessorFeaturePresent(uint feature);

        private const uint PF_VIRT_FIRMWARE_ENABLED = 21;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetFirmwareEnvironmentVariable(string lpName, string lpGuid, IntPtr lpBuffer, uint nSize);

        private static bool IsHypervisorPresent()
        {
            try
            {
                uint eax, ebx, ecx, edx;
                Cpuid(0x1, out eax, out ebx, out ecx, out edx);
                return (ecx & (1u << 31)) != 0;
            }
            catch
            {
                return false;
            }
        }

        private static void Cpuid(int level, out uint eax, out uint ebx, out uint ecx, out uint edx)
        {
            eax = ebx = ecx = edx = 0;

            if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                CpuidX64(level, out eax, out ebx, out ecx, out edx);
            }
            else
            {
                CpuidX86(level, out eax, out ebx, out ecx, out edx);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern void __cpuid(int[] cpuInfo, int level);

        private static void CpuidX86(int level, out uint eax, out uint ebx, out uint ecx, out uint edx)
        {
            int[] info = new int[4];
            __cpuid(info, level);
            eax = (uint)info[0];
            ebx = (uint)info[1];
            ecx = (uint)info[2];
            edx = (uint)info[3];
        }

        [DllImport("kernel32.dll")]
        private static extern void __cpuidex(int[] cpuInfo, int level, int ecxValue);

        private static void CpuidX64(int level, out uint eax, out uint ebx, out uint ecx, out uint edx)
        {
            int[] info = new int[4];
            __cpuidex(info, level, 0);
            eax = (uint)info[0];
            ebx = (uint)info[1];
            ecx = (uint)info[2];
            edx = (uint)info[3];
        }

        private static bool CheckWmiIndicators()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem");
                ManagementObjectCollection collection = searcher.Get();
                foreach (ManagementObject obj in collection)
                {
                    string manufacturer = (obj["Manufacturer"]?.ToString() ?? "").ToLowerInvariant();
                    string model = (obj["Model"]?.ToString() ?? "").ToLowerInvariant();

                    if (manufacturer.Contains("vmware") ||
                        (manufacturer.Contains("microsoft") && model.Contains("virtual")) ||
                        model.Contains("virtualbox") ||
                        model.Contains("kvm") ||
                        model.Contains("qemu"))
                    {
                        return true;
                    }
                }
                collection.Dispose();
                searcher.Dispose();

                ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("SELECT Product FROM Win32_BaseBoard");
                ManagementObjectCollection collection2 = searcher2.Get();
                foreach (ManagementObject obj in collection2)
                {
                    string product = (obj["Product"]?.ToString() ?? "").ToLowerInvariant();
                    if (product.Contains("virtual") || product.Contains("vmware"))
                    {
                        return true;
                    }
                }
                collection2.Dispose();
                searcher2.Dispose();
            }
            catch { }

            return false;
        }

        private static bool CheckRegistryIndicators()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System");
                if (key != null)
                {
                    string bios = (key.GetValue("SystemBiosVersion") as string ?? "").ToLowerInvariant();
                    string video = (key.GetValue("VideoBiosVersion") as string ?? "").ToLowerInvariant();
                    if (bios.Contains("vbox") || bios.Contains("vmware") || video.Contains("vmware"))
                    {
                        key.Close();
                        return true;
                    }
                    key.Close();
                }

                RegistryKey vboxKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Oracle\VirtualBox Guest Additions");
                if (vboxKey != null)
                {
                    vboxKey.Close();
                    return true;
                }

                RegistryKey acpi = Registry.LocalMachine.OpenSubKey(@"HARDWARE\ACPI\DSDT");
                if (acpi != null)
                {
                    string[] subKeys = acpi.GetSubKeyNames();
                    bool hasVbox = subKeys.Any(k => k.StartsWith("VBOX__", StringComparison.OrdinalIgnoreCase));
                    acpi.Close();
                    if (hasVbox) return true;
                }

                RegistryKey acpi2 = Registry.LocalMachine.OpenSubKey(@"HARDWARE\ACPI\FADT");
                if (acpi2 != null)
                {
                    string[] subKeys = acpi2.GetSubKeyNames();
                    bool hasVbox = subKeys.Any(k => k.StartsWith("VBOX__", StringComparison.OrdinalIgnoreCase));
                    acpi2.Close();
                    if (hasVbox) return true;
                }
            }
            catch { }

            return false;
        }

        public static void Execute()
        {
            if (IsHypervisorPresent() ||
                IsProcessorFeaturePresent(PF_VIRT_FIRMWARE_ENABLED) ||
                GetFirmwareEnvironmentVariable("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0) == 0 && Marshal.GetLastWin32Error() == 1 ||
                CheckWmiIndicators() ||
                CheckRegistryIndicators())
            {
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}