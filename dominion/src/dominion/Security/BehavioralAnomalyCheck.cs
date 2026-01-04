using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class BehavioralAnomalyCheck
    {
        private static int anomalyScore = 0;
        private const int threshold = 3;

        public static void RunChecks()
        {
            Console.WriteLine("[DEBUG] [SECURITY] [*] BehavioralAnomalyCheck.cs");

            Thread monitorThread = new Thread(MonitorLoop)
            {
                IsBackground = true
            };
            monitorThread.Start();
        }

        private static void MonitorLoop()
        {
            while (true)
            {
                anomalyScore = 0;

                // 1. Rapid process/window spawns (sandbox/automation indicator)
                if (Process.GetProcesses().Length > 150) // More than typical desktop
                    anomalyScore++;

                // 2. Unusual network activity (sandbox or analysis platform)
                int suspiciousConnCount = NetworkInterface.GetAllNetworkInterfaces()
                    .Count(ni => ni.OperationalStatus == OperationalStatus.Up
                                 && (ni.Name.ToLower().Contains("virtual") || ni.Description.ToLower().Contains("vmware")));

                if (suspiciousConnCount > 0)
                    anomalyScore++;

                // 3. Unusual writes to temp or suspicious folders (analysis tools)
                string tempPath = Path.GetTempPath();
                var recentTempFiles = Directory.GetFiles(tempPath)
                    .Count(f => File.GetCreationTime(f) > DateTime.Now.AddMinutes(-2));
                if (recentTempFiles > 50)
                    anomalyScore++;

                if (anomalyScore >= threshold)
                {
                    Environment.Exit(0);
                }

                Thread.Sleep(3000);
            }
        }
    }
}
