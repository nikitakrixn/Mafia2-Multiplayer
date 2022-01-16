using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafia2_Client
{
    internal class Settings
    {
        public static string Name => "Mafia2 Multiplayer";
        public static string Version => "v0.0.0.1";
#if DEBUG
        public static bool DEBUG = true;
#else
        public static bool DEBUG = false;
#endif
    }
}
