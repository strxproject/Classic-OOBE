using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void LogOut()
        {
            try
            {
                //Process.Start("shutdown", "/l");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to log out: {ex.Message}");
            }
        }
    }
}
