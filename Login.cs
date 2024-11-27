using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace AttendanceSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            string connectionString = ConfigurationManager.ConnectionStrings["AttendanceDB"].ConnectionString; using
    (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password =@password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtinput.Text);
                        cmd.Parameters.AddWithValue("@password", textpassword.Text);
                        int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                        {
                       
                        MessageBox.Show("Login Successful!");
                            AttendanceForm attendanceForm = new AttendanceForm();

                            this.Hide();
                            attendanceForm.Show();

                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password.");
                        txtinput.Clear();
                        textpassword.Clear();
                        txtinput.Focus();
                    }
                    }
                }
            }
           
    
     
            
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure , you want to exit?", "Confirm Exit",
                MessageBoxButtons.YesNo);
            if(result==DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textpassword.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}
