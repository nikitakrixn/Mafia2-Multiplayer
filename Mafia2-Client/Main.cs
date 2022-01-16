using Mafia2_Client.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2_Client
{
    public static class Main
    {
        [DllExport("Init")]
        public static void Init()
        {
            string nameWindow = string.Format("{0} - {1}", Settings.Name, Settings.Version);
            DebugHelper.RenameWindowName(nameWindow);
            if (Settings.DEBUG)
            {
                DebugHelper.ShowConsoleWindow();
            }
            Logger.WriteToConsole("Privet", Logger.LogLevel.INFO);
        }
    }
}
