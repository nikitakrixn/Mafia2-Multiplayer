using System;
using System.Diagnostics;

namespace Mafia2_Client.Utils
{
    public static class Memory
    {
        public static IntPtr BaseAddress
        {
            get { return Process.GetCurrentProcess().MainModule.BaseAddress; }
        }

        public static int ModuleSize
        {
            get { return Process.GetCurrentProcess().MainModule.ModuleMemorySize; }
        }

        public static IntPtr MainWindow
        {
            get { return Process.GetCurrentProcess().MainWindowHandle; }
        }
    }
}