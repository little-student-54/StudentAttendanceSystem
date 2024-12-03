namespace AttendanceSystem
{
    partial class SplashScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroProgressBar1
            // 
            this.metroProgressBar1.Location = new System.Drawing.Point(2, 466);
            this.metroProgressBar1.MarqueeAnimationSpeed = 150;
            this.metroProgressBar1.Maximum = 150;
            this.metroProgressBar1.Name = "metroProgressBar1";
            this.metroProgressBar1.ProgressBarStyle = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.metroProgressBar1.Size = new System.Drawing.Size(851, 10);
            this.metroProgressBar1.Step = 15;
            this.metroProgressBar1.TabIndex = 1;
            this.metroProgressBar1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.pictureBox1.Image = global::AttendanceSystem.Properties.Resources.SplashBackground__1_;
            this.pictureBox1.Location = new System.Drawing.Point(2, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(851, 454);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(857, 482);
            this.ControlBox = false;
            this.Controls.Add(this.metroProgressBar1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreen";
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
    }
}