using System;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class Start : Form
    {
        private Main _mainForm;

        public Start(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;

            usernameBox.TextChanged += UsernameBox_TextChanged;
            computernameBox.KeyPress += ComputernameBox_KeyPress;
            nextButton.Enabled = false;
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            string sanitizedUsername = usernameBox.Text.Replace(" ", string.Empty);
            computernameBox.Text = $"{sanitizedUsername}-PC";
            nextButton.Enabled = !string.IsNullOrWhiteSpace(usernameBox.Text);
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
            Password passwordForm = new Password(_mainForm);
            _mainForm.LoadFormIntoPanel(passwordForm);
        }
    }
}
