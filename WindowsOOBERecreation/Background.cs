﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class Background : Form
    {
        public Background()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.Enabled = false;

            SetBackgroundImageForResolution();
        }

        private void SetBackgroundImageForResolution()
        {
            var resolutions = new List<(Size Resolution, Bitmap Image)>
            {
                (new Size(1024, 768), Properties.Resources._1024768),
                (new Size(768, 1280), Properties.Resources._7681280),
                (new Size(1280, 1024), Properties.Resources._12801024),
                (new Size(1600, 1200), Properties.Resources._16001200),
                (new Size(1920, 1200), Properties.Resources._19201200)
            };
            var currentResolution = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            var closestResolution = resolutions
                .OrderBy(r => Math.Sqrt(Math.Pow(r.Resolution.Width - currentResolution.Width, 2) +
                                        Math.Pow(r.Resolution.Height - currentResolution.Height, 2)))
                .First();

            this.BackgroundImage = closestResolution.Image;
            this.BackgroundImageLayout = ImageLayout.Center;

            Console.WriteLine($"Screen Resolution: {currentResolution.Width}x{currentResolution.Height}");
            Console.WriteLine($"Selected Background Resolution: {closestResolution.Resolution.Width}x{closestResolution.Resolution.Height}");
        }
    }
}
