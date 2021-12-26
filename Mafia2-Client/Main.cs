using Mafia2_Client.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafia2_Client
{
    public static class Main
    {
        [DllExport("Init")]
        public static void Init()
        {
            DebugHelper.ShowConsoleWindow();
            Console.WriteLine("oriv");
        }
    }
}
