using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsOOBERecreation
{
    public partial class Finalizing : Form
    {
        private Main _mainForm;

        public Finalizing(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;

            LogoutAfterDelay();
        }

        private async void LogoutAfterDelay()
        {
            await Task.Delay(30000);

            LogOut();
        }

        private async void LogOut()
        {
            string logFilePath = @"C:\Classic Files\oobe.log";

            try
            {
                string appDirectory = Application.StartupPath;

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WinLogon", true))
                {
                    key?.SetValue("AutoAdminLogon", "1", RegistryValueKind.String);
                    key?.SetValue("AutoLogonCount", "2", RegistryValueKind.DWord);
                    LogToFile(logFilePath, $"Set AutoAdminLogon to 1 and AutoLogonCount to 1");

                    key?.SetValue("DefaultUserName", Properties.Settings.Default.username, RegistryValueKind.String);
                    LogToFile(logFilePath, $"Set DefaultUserName to {Properties.Settings.Default.username}");

                    string password = Properties.Settings.Default.password ?? "";
                    using (var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WinLogon", true))
                    {
                        regKey?.SetValue("DefaultPassword", password, RegistryValueKind.String);
                        LogToFile(logFilePath, $"Set DefaultPassword to {(string.IsNullOrEmpty(password) ? "an empty string" : password)}");
                    }
                }
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\Setup", true))
                {
                    key?.SetValue("OOBEInProgress", 0, RegistryValueKind.DWord);
                    key?.SetValue("RestartSetup", 0, RegistryValueKind.DWord);
                    key?.SetValue("SetupPhase", 0, RegistryValueKind.DWord);
                    key?.SetValue("SetupSupported", 1, RegistryValueKind.DWord);
                    key?.SetValue("SetupType", 0, RegistryValueKind.DWord);
                    key?.SetValue("SystemSetupInProgress", 0, RegistryValueKind.DWord);

                    LogToFile(logFilePath, "Set OOBEInProgress, RestartSetup, SetupPhase, SetupSupported, SetupType, and SystemSetupInProgress to 0 or 1");
                }

                LogToFile(logFilePath, "Exiting now");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LogToFile(logFilePath, $"An error occurred: {ex.Message}");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void LogToFile(string filePath, string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log: {ex.Message}");
            }
        }
    }
}
