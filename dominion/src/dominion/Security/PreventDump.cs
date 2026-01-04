using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class PreventDump
    {
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        public static void Run()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] PreventDump.cs");

            try
            {
                // Purge working set, makes it harder for external dumper tools.
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
            catch { }
        }
    }
}
