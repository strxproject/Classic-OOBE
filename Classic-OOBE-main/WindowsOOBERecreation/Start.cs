using System;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class Start : Form
    {
        private Main _mainForm;

        public string Username { get; private set; }
        public string ComputerName { get; private set; }

        public Start(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            usernameBox.TextChanged += UsernameBox_TextChanged;
            usernameBox.AutoSize = false;
            usernameBox.Height = 20;
            computernameBox.KeyPress += ComputernameBox_KeyPress;
            computernameBox.AutoSize = false;
            computernameBox.Height = 20;
            nextButton.Enabled = false;
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            string sanitizedUsername = usernameBox.Text.Replace(" ", string.Empty);
            computernameBox.Text = $"{sanitizedUsername}-PC";

            nextButton.Enabled = !string.IsNullOrWhiteSpace(usernameBox.Text);

            Username = sanitizedUsername;
            ComputerName = computernameBox.Text;
        }

        private void ComputernameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            _mainForm.Username = Username;
            _mainForm.ComputerName = ComputerName;

            _mainForm.LoadPasswordForm();
        }
    }
}
