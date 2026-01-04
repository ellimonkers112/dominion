using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class DetectInlineHook
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        private const int JMP_OPCODE = 0xE9;
        private const int CALL_OPCODE = 0xE8;
        private const int HOOK_CHECK_SIZE = 8;

        public static void Run()
        {
            try
            {
                Console.WriteLine("[DEBUG] [SECURITY] [*] DetectInlineHook.cs");

                string[] targets =
                {
                    "kernel32.dll!ReadProcessMemory",
                    "kernel32.dll!WriteProcessMemory",
                    "ntdll.dll!NtQueryInformationProcess",
                    "ntdll.dll!NtOpenProcess"
                };

                foreach (var target in targets)
                {
                    string[] parts = target.Split('!');
                    if (parts.Length != 2)
                        continue;

                    var module = GetModuleHandle(parts[0]);
                    if (module == IntPtr.Zero)
                        continue;

                    var functionPtr = GetProcAddress(module, parts[1]);
                    if (functionPtr == IntPtr.Zero)
                        continue;

                    if (IsHooked(functionPtr))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[SECURITY] Inline hook detected on {target}");
                        Console.ResetColor();
                        Environment.Exit(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SECURITY] Inline hook detection failed: " + ex.Message);
                Environment.Exit(1);
            }
        }

        private static bool IsHooked(IntPtr address)
        {
            try
            {
                byte[] buffer = new byte[HOOK_CHECK_SIZE];
                Marshal.Copy(address, buffer, 0, buffer.Length);

                // Inline jumps (JMP rel32 or CALL rel32) will have leading byte 0xE9 or 0xE8
                if (buffer[0] == JMP_OPCODE || buffer[0] == CALL_OPCODE)
                    return true;

                // Sometimes malicious hooks use near JMP (0xFF 0x25)
                if (buffer[0] == 0xFF && buffer[1] == 0x25)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
