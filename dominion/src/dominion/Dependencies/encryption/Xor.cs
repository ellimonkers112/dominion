using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public static class Xor
    {
        // Token: 0x06000320 RID: 800 RVA: 0x00018BF0 File Offset: 0x00016DF0
        public static string DecryptString(string input, byte key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            for (int i = 0; i < bytes.Length; i++)
            {
                byte[] array = bytes;
                int num = i;
                array[num] ^= key;
            }
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
