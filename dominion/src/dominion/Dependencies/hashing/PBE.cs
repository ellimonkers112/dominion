using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Hashing
{
    public class PBE
    {
        // Token: 0x1700002F RID: 47
        // (get) Token: 0x060002A7 RID: 679 RVA: 0x00014C34 File Offset: 0x00012E34
        private byte[] Ciphertext { get; }

        // Token: 0x17000030 RID: 48
        // (get) Token: 0x060002A8 RID: 680 RVA: 0x00014C3C File Offset: 0x00012E3C
        private byte[] GlobalSalt { get; }

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x060002A9 RID: 681 RVA: 0x00014C44 File Offset: 0x00012E44
        private byte[] MasterPass { get; }

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x060002AA RID: 682 RVA: 0x00014C4C File Offset: 0x00012E4C
        private byte[] EntrySalt { get; }

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x060002AB RID: 683 RVA: 0x00014C54 File Offset: 0x00012E54
        private byte[] PartIv { get; }

        // Token: 0x060002AC RID: 684 RVA: 0x00014C5C File Offset: 0x00012E5C
        public PBE(byte[] ciphertext, byte[] globalSalt, byte[] masterPassword, byte[] entrySalt, byte[] partIv)
        {
            this.Ciphertext = ciphertext;
            this.GlobalSalt = globalSalt;
            this.MasterPass = masterPassword;
            this.EntrySalt = entrySalt;
            this.PartIv = partIv;
        }

        // Token: 0x060002AD RID: 685 RVA: 0x00014C8C File Offset: 0x00012E8C
        public byte[] Compute()
        {
            byte[] array = new byte[this.GlobalSalt.Length + this.MasterPass.Length];
            Buffer.BlockCopy(this.GlobalSalt, 0, array, 0, this.GlobalSalt.Length);
            Buffer.BlockCopy(this.MasterPass, 0, array, this.GlobalSalt.Length, this.MasterPass.Length);
            byte[] password = new SHA1Managed().ComputeHash(array);
            byte[] array2 = new byte[]
            {
                4,
                14
            };
            byte[] array3 = new byte[array2.Length + this.PartIv.Length];
            Buffer.BlockCopy(array2, 0, array3, 0, array2.Length);
            Buffer.BlockCopy(this.PartIv, 0, array3, array2.Length, this.PartIv.Length);
            byte[] bytes = new Rfc2898DeriveBytes(password, this.EntrySalt, 1, HashAlgorithmName.SHA256).GetBytes(32);
            return new AesManaged
            {
                Mode = CipherMode.CBC,
                BlockSize = 128,
                KeySize = 256,
                Padding = PaddingMode.Zeros
            }.CreateDecryptor(bytes, array3).TransformFinalBlock(this.Ciphertext, 0, this.Ciphertext.Length);
        }
    }
}
