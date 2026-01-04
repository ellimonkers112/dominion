using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Security
{
    public static class KernelDebuggerCheck
    {
        // NTSTATUS values
        private const int STATUS_SUCCESS = 0;
        private const int SystemKernelDebuggerInformation = 0x23; // 35 decimal

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_KERNEL_DEBUGGER_INFORMATION
        {
            [MarshalAs(UnmanagedType.U1)]
            public bool DebuggerEnabled;
            [MarshalAs(UnmanagedType.U1)]
            public bool DebuggerNotPresent;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQuerySystemInformation(
            int systemInformationClass,
            IntPtr systemInformation,
            int systemInformationLength,
            IntPtr returnLength
        );

        public static bool IsKernelDebuggerAttached()
        {
            try
            {
                int size = Marshal.SizeOf(typeof(SYSTEM_KERNEL_DEBUGGER_INFORMATION));
                IntPtr buffer = Marshal.AllocHGlobal(size);
                int status = NtQuerySystemInformation(SystemKernelDebuggerInformation, buffer, size, IntPtr.Zero);

                if (status == STATUS_SUCCESS)
                {
                    var info = Marshal.PtrToStructure<SYSTEM_KERNEL_DEBUGGER_INFORMATION>(buffer);
                    Marshal.FreeHGlobal(buffer);

                    // Kernel debugger is active only if debugger enabled AND not marked as not-present
                    return info.DebuggerEnabled && !info.DebuggerNotPresent;
                }

                Marshal.FreeHGlobal(buffer);
            }
            catch
            {
                // Fail-safe, assume not attached if API fails
            }
            return false;
        }
    }
}
