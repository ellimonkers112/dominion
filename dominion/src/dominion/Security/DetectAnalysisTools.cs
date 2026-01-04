using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectAnalysisTools
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("[DEBUG] [SECURITY] [*] DetectAnalysisTools.cs");

                string[] blacklisted =
                {
                    "ollydbg", "x64dbg", "x32dbg", "idaq", "ida64", "wireshark",
                    "procmon", "processhacker", "httpanalyzer", "fiddler",
                    "charles", "burpsuite", "dnspy", "de4dot", "ilspy",
                    "reflector", "simpleassembly", "pebrowse", "resourcehacker", "cheatengine", "hxd"
                };

                var processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    try
                    {
                        string pname = process.ProcessName.ToLower();
                        if (blacklisted.Any(b => pname.Contains(b)))
                        {
                            Console.WriteLine($"[SECURITY] Blacklisted analysis tool detected: {pname} — exiting.");
                            Environment.Exit(1);
                        }
                    }
                    catch { }
                }
            }
            catch { }

        }
    }
}
