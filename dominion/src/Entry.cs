using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src
{
    internal class Entry
    {
        public static void Run()
        {
            
        }

        private static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        private static void LogSuccess(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [+] {message}");
        }

        private static void LogError(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [*] {message}");
        }

        private static void LogWarning(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [!] {message}");
        }
    }
}
