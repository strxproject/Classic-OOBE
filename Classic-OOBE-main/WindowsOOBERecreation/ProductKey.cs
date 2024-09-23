using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class ProductKey : Form
    {
        private Main _mainForm;
        public ProductKey(Main mainForm)
        {
            _mainForm = mainForm;
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void ProductKey_Load(object sender, EventArgs e)
        {

        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            TimeAndDate timeAndDateForm = new TimeAndDate(_mainForm);
            _mainForm.LoadFormIntoPanel(timeAndDateForm);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeAndDate timeAndDateForm = new TimeAndDate(_mainForm);
            _mainForm.LoadFormIntoPanel(timeAndDateForm);
        }
    }
}
