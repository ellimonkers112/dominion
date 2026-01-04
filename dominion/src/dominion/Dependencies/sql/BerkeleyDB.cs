using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Sql
{
    public class BerkeleyDB
    {
        // Token: 0x1700002E RID: 46
        // (get) Token: 0x06000298 RID: 664 RVA: 0x00013C21 File Offset: 0x00011E21
        public List<KeyValuePair<string, string>> Keys { get; }

        // Token: 0x06000299 RID: 665 RVA: 0x00013C2C File Offset: 0x00011E2C
        public BerkeleyDB(byte[] file)
        {
            List<byte> list = new List<byte>();
            this.Keys = new List<KeyValuePair<string, string>>();
            using (MemoryStream memoryStream = new MemoryStream(file))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    int i = 0;
                    int num = (int)binaryReader.BaseStream.Length;
                    while (i < num)
                    {
                        list.Add(binaryReader.ReadByte());
                        i++;
                    }
                }
            }
            string text = BitConverter.ToString(BerkeleyDB.Extract(list.ToArray(), 0, 4, false)).Replace("-", "");
            int num2 = BitConverter.ToInt32(BerkeleyDB.Extract(list.ToArray(), 12, 4, true), 0);
            if (!text.Equals("00061561"))
            {
                return;
            }
            int num3 = int.Parse(BitConverter.ToString(BerkeleyDB.Extract(list.ToArray(), 56, 4, false)).Replace("-", ""));
            int num4 = 1;
            while (this.Keys.Count < num3)
            {
                string[] array = new string[(num3 - this.Keys.Count) * 2];
                for (int j = 0; j < (num3 - this.Keys.Count) * 2; j++)
                {
                    array[j] = BitConverter.ToString(BerkeleyDB.Extract(list.ToArray(), num2 * num4 + 2 + j * 2, 2, true)).Replace("-", "");
                }
                Array.Sort<string>(array);
                for (int k = 0; k < array.Length; k += 2)
                {
                    int num5 = Convert.ToInt32(array[k], 16) + num2 * num4;
                    int num6 = Convert.ToInt32(array[k + 1], 16) + num2 * num4;
                    int num7 = (k + 2 >= array.Length) ? (num2 + num2 * num4) : (Convert.ToInt32(array[k + 2], 16) + num2 * num4);
                    string @string = Encoding.ASCII.GetString(BerkeleyDB.Extract(list.ToArray(), num6, num7 - num6, false));
                    string value = BitConverter.ToString(BerkeleyDB.Extract(list.ToArray(), num5, num6 - num5, false));
                    if (!string.IsNullOrWhiteSpace(@string))
                    {
                        this.Keys.Add(new KeyValuePair<string, string>(@string, value));
                    }
                }
                num4++;
            }
        }

        // Token: 0x0600029A RID: 666 RVA: 0x00013E74 File Offset: 0x00012074
        private static byte[] Extract(byte[] source, int start, int length, bool littleEndian)
        {
            byte[] array = new byte[length];
            int num = 0;
            for (int i = start; i < start + length; i++)
            {
                array[num] = source[i];
                num++;
            }
            if (littleEndian)
            {
                Array.Reverse(array);
            }
            return array;
        }
    }
}
