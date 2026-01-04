using System;
using System.Runtime.InteropServices;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public static class DpApi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct DataBlob
        {
            public int cbData;
            public IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CryptprotectPromptstruct
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }

        [DllImport("crypt32.dll", SetLastError = true)]
        private static extern bool CryptUnprotectData(
            ref DataBlob pDataIn,
            ref string ppszDataDescr,
            ref DataBlob pOptionalEntropy,
            IntPtr pvReserved,
            ref CryptprotectPromptstruct pPromptStruct,
            int dwFlags,
            ref DataBlob pDataOut);

        public static byte[] Decrypt(byte[] bCipher)
        {
            DataBlob dataBlob = default(DataBlob);
            DataBlob dataBlob2 = default(DataBlob);
            DataBlob dataBlob3 = default(DataBlob);
            string empty = string.Empty;
            GCHandle gchandle = GCHandle.Alloc(bCipher, GCHandleType.Pinned);
            dataBlob.cbData = bCipher.Length;
            dataBlob.pbData = gchandle.AddrOfPinnedObject();
            byte[] result;
            try
            {
                if (!CryptUnprotectData(ref dataBlob, ref empty, ref dataBlob3, IntPtr.Zero, ref Prompt, 0, ref dataBlob2) || dataBlob2.cbData == 0)
                {
                    result = null;
                }
                else
                {
                    byte[] array = new byte[dataBlob2.cbData];
                    Marshal.Copy(dataBlob2.pbData, array, 0, dataBlob2.cbData);
                    result = array;
                }
            }
            finally
            {
                gchandle.Free();
                if (dataBlob2.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(dataBlob2.pbData);
                }
                if (dataBlob3.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(dataBlob3.pbData);
                }
            }
            return result;
        }

        private static CryptprotectPromptstruct Prompt = new CryptprotectPromptstruct
        {
            cbSize = Marshal.SizeOf(typeof(CryptprotectPromptstruct)),
            dwPromptFlags = 0,
            hwndApp = IntPtr.Zero,
            szPrompt = null
        };
    }
}