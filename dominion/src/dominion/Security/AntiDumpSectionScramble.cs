using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class AntiDumpSectionScramble
    {
        public static void Run()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] AntiDumpSectionScramble.cs");

            try
            {
                // Find all static fields named "key", "secret", "password" and zero them if they're arrays
                var assembly = Assembly.GetExecutingAssembly();

                foreach (var type in assembly.GetTypes())
                {
                    foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        string name = field.Name.ToLower();
                        if (name.Contains("key") || name.Contains("secret") || name.Contains("password"))
                        {
                            if (field.FieldType == typeof(byte[]))
                            {
                                byte[] arr = (byte[])field.GetValue(null);
                                if (arr != null)
                                    Array.Clear(arr, 0, arr.Length);
                                Console.WriteLine($"[SECURITY] Zeroed static array field '{field.Name}' in '{type.Name}'");
                            }
                            if (field.FieldType == typeof(string))
                            {
                                field.SetValue(null, string.Empty);
                                Console.WriteLine($"[SECURITY] Cleared static string field '{field.Name}' in '{type.Name}'");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SECURITY] AntiDumpSectionScramble error: {ex.Message}");
            }
        }
    }
}
