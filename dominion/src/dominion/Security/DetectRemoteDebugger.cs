using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectRemoteDebugger
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        public static void Run()
        {
            try
            {
                Console.WriteLine("[DEBUG] [SECURITY] [*] DetectRemoteDebugger.cs");

                bool isDebuggerPresent = false;
                CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
                if (isDebuggerPresent || Debugger.IsAttached)
                {
                    Console.WriteLine("[SECURITY] Remote debugger detected! Exiting.");
                    Environment.Exit(1);
                }
            }
            catch
            {
                // Silent fail
            }
        }
    }
}
