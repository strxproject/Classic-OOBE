using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class Password : Form
    {
        private Main _mainForm;
        private string _username;
        private string _computerName;

        [DllImport("kernel32.dll")]
        static extern bool SetComputerName(string lpComputerName);

        public Password(Main mainForm, string username, string computerName)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _username = username;
            _computerName = computerName;

            passwordBox.TextChanged += ValidateInput;
            confpasswordBox.TextChanged += ValidateInput;
            passwordHintBox.TextChanged += ValidateInput;
            nextButton.Enabled = true;
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            string password = passwordBox.Text;
            string confirmPassword = confpasswordBox.Text;
            string passwordHint = passwordHintBox.Text;

            if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(confirmPassword) && string.IsNullOrEmpty(passwordHint))
            {
                nextButton.Enabled = true;
            }
            else if (!string.IsNullOrEmpty(password) && password == confirmPassword && !string.IsNullOrEmpty(passwordHint))
            {
                nextButton.Enabled = true;
            }
            else
            {
                nextButton.Enabled = false;
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            string password = passwordBox.Text;
            string confirmPassword = confpasswordBox.Text;

            try
            {
                if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(confirmPassword))
                {
                    ExecuteCommand($"net user \"{_username}\" /add");
                }
                else if (password == confirmPassword)
                {
                    ExecuteCommand($"net user \"{_username}\" \"{password}\" /add");
                }

                ExecuteCommand($"net localgroup Administrators /add \"{_username}\"");
                ChangeComputerName(_computerName);

                Properties.Settings.Default.username = _username;
                Properties.Settings.Default.password = password;
                Properties.Settings.Default.Save();

                ProductKey ProductKeyForm = new ProductKey(_mainForm);
                _mainForm.LoadFormIntoPanel(ProductKeyForm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during account creation: {ex.Message}");

                ProductKey ProductKeyForm = new ProductKey(_mainForm);
                _mainForm.LoadFormIntoPanel(ProductKeyForm);
            }
        }

        private void ExecuteCommand(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
            }
        }

        public static bool ChangeComputerName(string newComputerName)
        {
            bool isChanged = SetComputerName(newComputerName);

            if (isChanged)
            {
                Console.WriteLine("Computer name changed successfully to: " + newComputerName);
            }
            else
            {
                Console.WriteLine("Failed to change the computer name.");
            }

            return isChanged;
        }
    }
}
