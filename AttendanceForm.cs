using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Data;

namespace AttendanceSystem
{
    public partial class AttendanceForm : Form
    {
        public AttendanceForm()
        {
            InitializeComponent();
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            // Configure dtpTime for date and time display
            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Displays date and time
            dtpTime.ShowUpDown = true; // Allows scrolling to change time
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the connection string from configuration
                string connectionString = ConfigurationManager.ConnectionStrings["AttendanceDB"].ConnectionString;

                // Insert attendance data into the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Attendance (StudentName, RollNumber, Date, Time, Status) " +
                                   "VALUES(@name, @roll, @date, @time, @status)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@name", txtStudentName.Text);
                        cmd.Parameters.AddWithValue("@roll", txtRollNumber.Text);
                        cmd.Parameters.AddWithValue("@date", dtpDate.Value.Date); // Date only
                        cmd.Parameters.AddWithValue("@time", dtpTime.Value.ToString("HH:mm:ss")); // Time only
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem?.ToString() ?? ""); // Check if selected

                        // Execute the query
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Attendance Saved Successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewAttendance_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a status is selected
                if (cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please select a status from the dropdown before viewing attendance.",
                                    "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbStatus.Focus();
                    return;
                }

                // Get the selected status
                string selectedStatus = cmbStatus.SelectedItem.ToString();
                MessageBox.Show($"Selected Status: {selectedStatus}");

                // Code to query the database and populate DataGridView can be added here
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
            try
            {
                // Get the connection string from the configuration
                string connectionString = ConfigurationManager.ConnectionStrings["AttendanceDB"].ConnectionString;

                // SQL query to fetch attendance records based on status
                string query = "SELECT * FROM Attendance WHERE Status = @Status";

                // Create a DataTable to hold the data
                DataTable dataTable = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Get the selected status from a ComboBox or similar control
                        string selectedStatus = cmbStatus.SelectedItem.ToString(); // Assuming a ComboBox named cmbStatus

                        // Add the parameter to the SQL query
                        cmd.Parameters.AddWithValue("@Status", selectedStatus);

                        // Use SqlDataAdapter to fill the DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);
                    }
                }

                // Bind the DataTable to the DataGridView
                dgvAttendance.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            // Ensure the dtpTime is configured properly (optional; usually set in Form_Load)
            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Date and time
        }

        private void exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure , you want to exit?", "Confirm Exit",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AttendanceManage v = new AttendanceManage();
            this.Hide();
            v.Show();
        }
    }
}
