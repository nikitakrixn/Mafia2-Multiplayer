using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Mafia2_Launcher.Class
{
    internal static class ProcUtils
    {
        public static Process Process = null;
        private static readonly string clientLibrary = "m2o-client.dll";

        public static bool InjectDLL()
        {
            byte[] byLibName = Encoding.UTF8.GetBytes(GetClientLibraryPath());

            IntPtr hMemory = WinAPI.VirtualAllocEx(Process.Handle, IntPtr.Zero, byLibName.Length, WinAPI.AllocationType.Commit, WinAPI.MemoryProtection.ReadWrite);

            if (hMemory == IntPtr.Zero)
            {
                return false;
            }

            if (WinAPI.WriteProcessMemory(Process.Handle, hMemory, byLibName, byLibName.Length, out _) == 0)
            {
                return false;
            }

            IntPtr pLoadLibraryAddr = WinAPI.GetProcAddress(WinAPI.LoadLibrary("kernel32"), "LoadLibraryA");

            if (pLoadLibraryAddr == IntPtr.Zero)
            {
                return false;
            }

            IntPtr hThread = WinAPI.CreateRemoteThread(Process.Handle, 0, 0, pLoadLibraryAddr, hMemory, 0, out _);

            if (hThread == IntPtr.Zero)
            {
                return false;
            }

            if (WinAPI.WaitForSingleObject(hThread, -1) == 0 && WinAPI.GetExitCodeThread(hThread, out var dwAddr) != 0)
            {
                IntPtr hLib = WinAPI.LoadLibrary(GetClientLibraryPath());
                IntPtr hEnt = WinAPI.GetProcAddress(hLib, "Init");
                long iOff = dwAddr + (int)hEnt - (int)hLib;

                WinAPI.CreateRemoteThread(Process.Handle, 0, 0, (IntPtr)iOff, IntPtr.Zero, 0, out _);

                return hThread != IntPtr.Zero;
            }

            return false;
        }

        public static string GetClientLibraryPath()
        {
            return string.Format(Environment.CurrentDirectory + "\\{0}", GetClientLibrary());
        }

        public static bool ProcessIsRunning(string name)
        {
            return Process.GetProcessesByName(name).Length > 0;
        }

        public static void StartProcess(string path)
        {
            Process process = new Process
            {
                StartInfo = { FileName = path }
            };
            process.Start();
        }

        public static void CloseProcess(string name)
        {
            Process process = Process.GetProcessesByName(name).FirstOrDefault();
            process.Kill();
        }

        public static string GetClientLibrary()
        {
            return clientLibrary;
        }

        public static void FindMafia2()
        {
            while (Process == null)
            {
                Thread.Sleep(500);

                Process[] ProcList = Process.GetProcessesByName("mafia2");
                if (ProcList.Length < 1)
                {
                    continue;
                }

                Process = ProcList[0];
            }
        }
    }

    #region WIN32API
    internal static class WinAPI
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

        [DllImport("kernel32.dll")]
        public static extern int GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            int lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
    }
    #endregion
}
