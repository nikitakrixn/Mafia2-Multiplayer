using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafia2_Client.Utils
{
    public class Logger
    {
        public enum LogLevel
        {
            DEBUG,
            INFO,
            ERROR,
            WARNING
        }

        private static readonly string logPath = Directory.GetCurrentDirectory() + "\\debug.log";

        public static void LogMessage(string strMessage, LogLevel enLevel)
        {
            try
            {
                if (strMessage == null) return;
                string strLog = string.Empty;
                StringBuilder sbLog = new StringBuilder();
                StreamWriter swLog = new StreamWriter(logPath, true);
                DateTime dtNow = DateTime.Now;
                string strDate = string.Format("{0:d.M.yyyy}", dtNow);
                switch (enLevel)
                {
                    case LogLevel.DEBUG:
                        sbLog.Append("[DEBUG]");
                        sbLog.Append("  ");
                        break;
                    case LogLevel.INFO:
                        sbLog.Append("[INFO]");
                        sbLog.Append("   ");
                        break;
                    case LogLevel.ERROR:
                        sbLog.Append("[ERROR]");
                        sbLog.Append("  ");
                        break;
                    case LogLevel.WARNING:
                        sbLog.Append("[WARNING]");
                        sbLog.Append(" ");
                        break;
                    default:
                        sbLog.Append("[UNKNOWN]");
                        break;
                }
                sbLog.Append(" -> [" + strDate + "] :: ");
                sbLog.Append(strMessage);
                strLog = sbLog.ToString();
                swLog.WriteLine(strLog);
                swLog.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Logger::LogMessage()] -> Logging error : " + ex.Message);
            }

        }

        public static void WriteToConsole(string strText, LogLevel enLevel)
        {
            if (strText == null) return;
            switch (enLevel)
            {
                case LogLevel.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[DEBUG]   : ");
                    break;
                case LogLevel.INFO:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[INFO]    : ");
                    break;
                case LogLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[ERROR]   : ");
                    break;
                case LogLevel.WARNING:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("[WARNING] : ");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[UNKNOWN] : ");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(strText);
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void DebugToLog(string strText)
        {
            if (strText == null) return;
            WriteToConsole(strText, LogLevel.DEBUG);
            LogMessage(strText, LogLevel.DEBUG);
        }
    }
}
