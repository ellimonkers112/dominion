using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectDebugger
    {
        public static void Run()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] DetectDebugger.cs");

            if (Debugger.IsAttached || Debugger.IsLogging())
            {
                Console.WriteLine("[SECURITY] Debugger attached — exiting safely.");
                Environment.Exit(1);
            }

            if (IsAnyDebuggerPresent())
            {
                Console.WriteLine("[SECURITY] Debugger detected via WinAPI — exiting safely.");
                Environment.Exit(1);
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool IsDebuggerPresent();

        private static bool IsAnyDebuggerPresent()
        {
            return IsDebuggerPresent();
        }
    }
}
