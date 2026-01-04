using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public class RC4Crypt
    {
        // Token: 0x0600030F RID: 783 RVA: 0x00018424 File Offset: 0x00016624
        public static byte[] Decrypt(byte[] key, byte[] data)
        {
            if (key == null)
            {
                return null;
            }
            if (key.Length == 0)
            {
                return null;
            }
            if (data == null)
            {
                return null;
            }
            byte[] array = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                array[i] = (byte)i;
            }
            int num = 0;
            for (int j = 0; j < 256; j++)
            {
                num = (num + (int)array[j] + (int)key[j % key.Length] & 255);
                RC4Crypt.Swap(array, j, num);
            }
            byte[] array2 = new byte[data.Length];
            int num2 = 0;
            num = 0;
            for (int k = 0; k < data.Length; k++)
            {
                num2 = (num2 + 1 & 255);
                num = (num + (int)array[num2] & 255);
                RC4Crypt.Swap(array, num2, num);
                byte b = array[(int)(array[num2] + array[num] & byte.MaxValue)];
                array2[k] = (byte)(data[k] ^ b);
            }
            return array2;
        }

        // Token: 0x06000310 RID: 784 RVA: 0x000184F8 File Offset: 0x000166F8
        private static void Swap(byte[] arr, int a, int b)
        {
            byte b2 = arr[a];
            arr[a] = arr[b];
            arr[b] = b2;
        }
    }
}
