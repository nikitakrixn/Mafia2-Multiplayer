using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Threading;
using Mafia2_Launcher.Class;

namespace Mafia2_Launcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string captionName = "Mafia2 Multiplayer - Launcher";
        private readonly string processName = "Mafia2";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void LaunchGame_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Injector.GetClientLibraryPath()))
            {
                if (Injector.ProcessIsRunning(processName))
                {
                    MessageBoxResult result = MessageBox.Show("У вас уже запущена игра Mafia2. Хотите закрыть её?", captionName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        Injector.CloseProcess(processName);
                    }
                    else
                    {
                        Environment.Exit(1);
                    }
                }
                else
                {
                    try
                    {
                        Injector.StartProcess(M2Registry.PathName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, captionName, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    finally
                    {
                        Thread.Sleep(2000);
                        Injector.InjectDLL();
                    }

                }
            }
            else
            {
                MessageBox.Show("Файл " + Injector.GetClientLibrary() + " должен находится рядом с " + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, captionName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (M2Registry.Valid() && M2Registry.Read())
            {
                /* Надо допилить */
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("У вас есть установленная игра Mafia2?", captionName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (!M2Registry.Create())
                    {
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Environment.Exit(1);
                }
            }
            PathName.Content = M2Registry.PathName;
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            if (Injector.ProcessIsRunning(processName))
            {
                Injector.CloseProcess(processName);
            }
        }
    }
}
