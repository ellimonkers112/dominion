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
    public static class ProcessNameCheck
    {
        private static readonly string[] SuspiciousNames = {
            "ollydbg", "x64dbg", "x32dbg", "ida", "immunitydebugger",
            "wireshark", "fiddler", "burp", "cheatengine", "artmoney", "scylla", "peid",
            "dnspy", "ilspy", "dotpeek", "justdecompile", "de4dot", "processhacker"
        };
        private static bool isRunning = true;
        public static bool DebugOverride = false; // Set true to disable exits for dev/testing

        public static void RunChecks()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] ProcessNameCheck.cs");
            if (DebugOverride)
            {
                Console.WriteLine("[SECURITY] DebugOverride is enabled! No exit will occur.");
            }

            Thread monitorThread = new Thread(MonitorLoop)
            {
                IsBackground = true
            };
            monitorThread.Start();
            StartWmiProcessStartWatcher();
        }

        private static void MonitorLoop()
        {
            int currentId = Process.GetCurrentProcess().Id;
            while (isRunning)
            {
                try
                {
                    foreach (var process in Process.GetProcesses())
                    {
                        try
                        {
                            if (process.Id == currentId)
                                continue; // Skip self

                            string name = process.ProcessName.ToLowerInvariant();
                            if (SuspiciousNames.Any(s => name == s)) // Exact name match only
                            {
                                Console.WriteLine($"[SECURITY] Suspicious process DETECTED: {name}");
                                if (!DebugOverride)
                                {
                                    Environment.Exit(1); // Only exit if not debugging
                                }
                            }
                        }
                        catch { /* ignore process access denied errors */ }
                    }
                }
                catch { /* ignore enumerate exceptions */ }
                Thread.Sleep(5000);
            }
        }

        private static void StartWmiProcessStartWatcher()
        {
            try
            {
                var startWatcher = new ManagementEventWatcher(
                    new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
                startWatcher.EventArrived += (sender, args) =>
                {
                    string name = args.NewEvent["ProcessName"].ToString().ToLowerInvariant();
                    if (SuspiciousNames.Any(s => name == s)) // Exact name match
                    {
                        Console.WriteLine($"[SECURITY] Suspicious process STARTED: {name}");
                        if (!DebugOverride)
                        {
                            Environment.Exit(1);
                        }
                    }
                };
                startWatcher.Start();
            }
            catch
            {
                // Silently ignore any WMI issues
            }
        }

        public static void Stop()
        {
            isRunning = false;
        }
    }
}
