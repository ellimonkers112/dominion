using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectHarmonyPatch
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("[DEBUG] [SECURITY] [*] DetectHarmonyPatch.cs");

                var harmonyAssembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == "0Harmony");

                if (harmonyAssembly != null)
                {
                    Console.WriteLine("[SECURITY] Detected Harmony patcher assembly — exiting.");
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
