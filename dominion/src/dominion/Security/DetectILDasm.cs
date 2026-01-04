using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectILDasm
    {
        public static void Run()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] DetectILDasm.cs");

            string[] analysisTools = { "ildasm", "reflector", "dnspy", "ilspy" };

            var processes = Process.GetProcesses();
            foreach (var proc in processes)
            {
                string procName = "";
                try { procName = proc.ProcessName.ToLower(); } catch { continue; }
                if (analysisTools.Any(tool => procName.Contains(tool)))
                {
                    Console.WriteLine("[SECURITY] Reverse engineering tool detected: " + procName);
                    Environment.Exit(1);
                }
            }
        }
    }
}
