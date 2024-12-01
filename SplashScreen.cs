using System;

using System.Windows.Forms;

namespace AttendanceSystem
{
    public partial class SplashScreen : MetroFramework.Forms.MetroForm
    {
        public SplashScreen()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            metroProgressBar1.Visible = true;
            Timer timer = new Timer();
            timer.Interval =5000; // Show splash screen for 3 seconds
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                this.Close(); // Close the splash screen
            };
            timer.Start();

        }
    }
}
