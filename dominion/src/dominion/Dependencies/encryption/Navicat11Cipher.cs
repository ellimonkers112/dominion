using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public class Navicat11Cipher
    {
        // Token: 0x060002F6 RID: 758 RVA: 0x00017780 File Offset: 0x00015980
        protected byte[] StringToByteArray(string hex)
        {
            return (from x in Enumerable.Range(0, hex.Length)
                    where x % 2 == 0
                    select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray<byte>();
        }

        // Token: 0x060002F7 RID: 759 RVA: 0x000177E8 File Offset: 0x000159E8
        protected void XorBytes(byte[] a, byte[] b, int len)
        {
            for (int i = 0; i < len; i++)
            {
                int num = i;
                a[num] ^= b[i];
            }
        }

        // Token: 0x060002F8 RID: 760 RVA: 0x00017810 File Offset: 0x00015A10
        public Navicat11Cipher()
        {
            byte[] array = new byte[]
            {
                51,
                68,
                67,
                53,
                67,
                65,
                51,
                57
            };
            using (SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider())
            {
                sha1CryptoServiceProvider.TransformFinalBlock(array, 0, array.Length);
                this.blowfishCipher = new Blowfish(sha1CryptoServiceProvider.Hash);
            }
        }

        // Token: 0x060002F9 RID: 761 RVA: 0x00017874 File Offset: 0x00015A74
        public string DecryptString(string ciphertext)
        {
            byte[] array = this.StringToByteArray(ciphertext);
            byte[] array2 = Enumerable.Repeat<byte>(byte.MaxValue, this.blowfishCipher.BlockSize).ToArray<byte>();
            this.blowfishCipher.Encrypt(array2, Blowfish.Endian.Big);
            byte[] array3 = new byte[0];
            int num = array.Length / this.blowfishCipher.BlockSize;
            int num2 = array.Length % this.blowfishCipher.BlockSize;
            byte[] array4 = new byte[this.blowfishCipher.BlockSize];
            byte[] array5 = new byte[this.blowfishCipher.BlockSize];
            for (int i = 0; i < num; i++)
            {
                Array.Copy(array, this.blowfishCipher.BlockSize * i, array4, 0, this.blowfishCipher.BlockSize);
                Array.Copy(array4, array5, this.blowfishCipher.BlockSize);
                this.blowfishCipher.Decrypt(array4, Blowfish.Endian.Big);
                this.XorBytes(array4, array2, this.blowfishCipher.BlockSize);
                array3 = array3.Concat(array4).ToArray<byte>();
                this.XorBytes(array2, array5, this.blowfishCipher.BlockSize);
            }
            if (num2 != 0)
            {
                Array.Clear(array4, 0, array4.Length);
                Array.Copy(array, this.blowfishCipher.BlockSize * num, array4, 0, num2);
                this.blowfishCipher.Encrypt(array2, Blowfish.Endian.Big);
                this.XorBytes(array4, array2, this.blowfishCipher.BlockSize);
                array3 = array3.Concat(array4.Take(num2).ToArray<byte>()).ToArray<byte>();
            }
            return Encoding.UTF8.GetString(array3);
        }

        // Token: 0x040001D4 RID: 468
        private Blowfish blowfishCipher;
    }
}
