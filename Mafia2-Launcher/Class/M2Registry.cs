using System;
using System.IO;
using Microsoft.Win32;

namespace Mafia2_Launcher.Class
{
    internal class M2Registry
    {

        public static string PathName { get; set; }
        private static readonly string subKey = @"Software\\EmpireBay-Multiplayer";
        private static readonly string subKeyValue = "mafia2exepath";

        public static bool Check()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(subKey))
                {
                    if (rk == null)
                    {
                        return false;
                    }

                    if (rk.GetValue(subKeyValue, null) == null)
                    {
                        return false;
                    }

                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Valid()
        {
            object value;

            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(subKey))
                {
                    if (rk == null)
                    {
                        return false;
                    }

                    value = rk.GetValue(subKeyValue, null);

                    if (value == null)
                    {
                        throw new ArgumentNullException();
                    }

                    if (!File.Exists(Convert.ToString(value)))
                    {
                        throw new FileNotFoundException();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool Create()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(subKey))
                {
                    PathName = FolderGamePath();
                    if (PathName == string.Empty)
                    {
                        return false;
                    }
                    rk.SetValue(subKeyValue, PathName, RegistryValueKind.String);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Read()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(subKey))
                {
                    PathName = (string)rk.GetValue(subKeyValue);
                    if (PathName == null)
                    {
                        throw new ArgumentNullException();
                    }

                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Write()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(subKey, true))
                {
                    rk.SetValue(subKeyValue, PathName, RegistryValueKind.String);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string FolderGamePath()
        {
            string exePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "mafia2.exe file|Mafia2.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                exePath = openFileDialog.FileName;
            }

            return exePath;
        }
    }
}
