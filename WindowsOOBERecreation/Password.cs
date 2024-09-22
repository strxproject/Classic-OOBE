using System;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class Password : Form
    {
        private Main _mainForm;

        public Password(Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            TimeAndDate timeAndDateForm = new TimeAndDate(_mainForm);
            _mainForm.LoadFormIntoPanel(timeAndDateForm);
        }
    }
}
