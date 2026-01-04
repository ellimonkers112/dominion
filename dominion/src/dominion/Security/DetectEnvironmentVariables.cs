using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectEnvironmentVariables
    {
        public static void Run()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] DetectEnvironmentVariables.cs");

            string[] suspiciousVars = {
                "COR_ENABLE_PROFILING", "COR_PROFILER", "COMPLUS_",
                "DOTNET_", "PROCESS_DUMP", "DEVKNOWLEDGE"
            };
            foreach (var var in suspiciousVars)
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(var)))
                {
                    Console.WriteLine($"[SECURITY] Suspicious environment variable detected: {var}");
                    Environment.Exit(1);
                }
            }
        }
    }
}
