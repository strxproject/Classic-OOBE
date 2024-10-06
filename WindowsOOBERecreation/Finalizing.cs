using System;
using System.Diagnostics;
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
            try
            {
                string appDirectory = Application.StartupPath;
                string regFilePath1 = System.IO.Path.Combine(appDirectory, "1.reg");
                string regFilePath2 = System.IO.Path.Combine(appDirectory, "2.reg");

                await Task.Run(() =>
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "regedit.exe",
                        Arguments = $"/s \"{regFilePath1}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    })?.WaitForExit();
                });

                await Task.Run(() =>
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "regedit.exe",
                        Arguments = $"/s \"{regFilePath2}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    })?.WaitForExit();
                });

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WinLogon", true))
                {
                    key?.SetValue("AutoAdminLogon", "1", RegistryValueKind.String);
                    key?.SetValue("AutoLogonCount", 1, RegistryValueKind.DWord);
                    key?.SetValue("DefaultUserName", Properties.Settings.Default.username, RegistryValueKind.String);
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.password))
                    {
                        using (var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\WinLogon", true))
                        {
                            regKey?.SetValue("DefaultPassword", Properties.Settings.Default.password, RegistryValueKind.String);
                        }
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
                }

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true))
                {
                    if (key != null)
                    {
                        key.SetValue("EnableLUA", 1, RegistryValueKind.DWord);
                    }
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = "shutdown",
                    Arguments = "/r /t 0",
                    UseShellExecute = true,
                    Verb = "runas"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
