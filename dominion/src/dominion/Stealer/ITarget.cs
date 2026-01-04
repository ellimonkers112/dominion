using dominion.src.dominion.Dependencies.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Stealer
{
    public interface ITarget
    {
        void Collect(InMemoryZip zip, Counter counter);
    }
}
