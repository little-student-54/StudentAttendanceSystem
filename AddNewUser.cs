using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttendanceSystem
{
    public partial class AddNewUser : MetroFramework.Forms.MetroForm
    {
        public AddNewUser()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            password.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void back_Click(object sender, EventArgs e)
        {
            Login r=new Login();
            this.Hide();
            r.Show();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connection string
            string connectionString = ConfigurationManager.ConnectionStrings["AttendanceDB"].ConnectionString;

                // SQL query to insert data
                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

            try
            {
                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create a SqlCommand object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL Injection
                        command.Parameters.AddWithValue("@Username", name.Text.Trim());
                        command.Parameters.AddWithValue("@Password", password.Text.Trim());

                        // Open connection and execute query
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Show a message box with the entered username and password
                            DialogResult result = MessageBox.Show(
                                $"Here is your username and password:\n\nUsername: {name.Text}\nPassword: {password.Text}\n\nClick OK to go to the login form.",
                                "Confirmation",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Information);

                            // If user clicks OK, navigate to the login form
                            if (result == DialogResult.OK)
                            {
                                // Navigate to the login form
                                Login loginForm = new Login();
                                this.Hide(); // Hide the current form
                                loginForm.Show(); // Show the login form
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to add user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
