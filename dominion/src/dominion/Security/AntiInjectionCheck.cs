using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class AntiInjectionCheck
    {
        public static void RunChecks()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] AntiInjectionCheck.cs");

            Thread monitorThread = new Thread(MonitorLoop)
            {
                IsBackground = true
            };
            monitorThread.Start();
        }

        private static void MonitorLoop()
        {
            while (true)
            {
                if (IsInjectedDllPresent() || AreSuspiciousModulesLoaded())
                {
                    Environment.Exit(0);
                }
                Thread.Sleep(3000);
            }
        }

        // Detects suspiciously loaded DLLs (tightened detection, less broad)
        private static bool IsInjectedDllPresent()
        {
            string[] bannedDlls = {
                // Only specific cheat/hacking DLLs, NOT generic names!
                "cheatengine", "modmenu", "dnlib", "easyhook",
                "hacking.dll", "scylla.dll", "dbghelp.dll", "libde4dot"
            };

            try
            {
                var currentProcess = Process.GetCurrentProcess();
                foreach (ProcessModule module in currentProcess.Modules)
                {
                    string moduleName = module.ModuleName.ToLower();
                    if (bannedDlls.Any(bad => moduleName == bad || moduleName.StartsWith(bad)))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        // Detect only known suspicious non-.NET assemblies injected at runtime
        private static bool AreSuspiciousModulesLoaded()
        {
            string[] bannedAssemblies = {
                "cheatengine", "mono.cecil", "dnspy", "de4dot", "reflexil"
            };

            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in assemblies)
                {
                    string name = asm.FullName.ToLower();
                    if (bannedAssemblies.Any(bad => name.Contains(bad)))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
