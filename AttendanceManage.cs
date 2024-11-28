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
    public partial class AttendanceManage : MetroFramework.Forms.MetroForm
    {
        public AttendanceManage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
                string query = "SELECT * FROM Attendance WHERE Status = @Status OR Date = @Date";

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
                        cmd.Parameters.AddWithValue("@Date", dptDate.Value.ToString("yyyy-MM-dd"));
                       
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

        private void exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure , you want to exit?", "Confirm Exit",
               MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AttendanceForm c = new AttendanceForm();
            this.Hide();
            c.Show();
        }

        private void AttendanceManage_Load(object sender, EventArgs e)
        {

        }
        private void dgvAttendance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow row = dgvAttendance.Rows[e.RowIndex];

                // Populate input fields with the selected row's data
                txtStudentName.Text = row.Cells["StudentName"].Value?.ToString();
                txtRollNumber.Text = row.Cells["RollNumber"].Value?.ToString();
                dptDate.Value = Convert.ToDateTime(row.Cells["Date"].Value);
                dtpTime.Value = DateTime.Parse(row.Cells["Time"].Value.ToString());
                cmbStatus.SelectedItem = row.Cells["Status"].Value?.ToString();
                attendanceID.Text = row.Cells["ID"].Value?.ToString(); // Use the correct column name
            }
        }


        private void LoadData()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AttendanceDB"].ConnectionString;
                string query = "SELECT ID, StudentName, RollNumber, Date, Time, Status FROM Attendance";

                DataTable dataTable = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dataTable);
                    }
                }

                dgvAttendance.DataSource = dataTable;

                // Hide the ID column if not required for display
                dgvAttendance.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAttendance.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected row's ID
                int id = Convert.ToInt32(dgvAttendance.SelectedRows[0].Cells["ID"].Value);

                string connectionString = ConfigurationManager.ConnectionStrings["AttendanceDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Attendance SET StudentName = @StudentName, RollNumber = @RollNumber, " +
                                   "Date = @Date, Time = @Time, Status = @Status WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Update parameters with input field values
                        cmd.Parameters.AddWithValue("@StudentName", txtStudentName.Text);
                        cmd.Parameters.AddWithValue("@RollNumber", txtRollNumber.Text);
                        cmd.Parameters.AddWithValue("@Date", dptDate.Value.Date);
                        cmd.Parameters.AddWithValue("@Time", dtpTime.Value.ToString("HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@ID", id); // Use correct parameter

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Updated Successfully!");
                    }
                }
                ClearInputs();
                // Refresh DataGridView after updating
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearInputs()
        {
            // Clear all textboxes
            txtStudentName.Clear();
            txtRollNumber.Clear();

            // Reset DateTimePickers to current date and time
            dptDate.Value = DateTime.Now;
            dtpTime.Value = DateTime.Now;

            // Reset ComboBox
            cmbStatus.SelectedIndex = -1;

            // Clear hidden Attendance ID field
            attendanceID.Clear();
        }


        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "yyyy-MM-dd HH:mm:ss"; // Date and time
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a row is selected
                if (dgvAttendance.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm the deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete the selected record?",
                                                      "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }

                // Get the selected row's ID
                int id = Convert.ToInt32(dgvAttendance.SelectedRows[0].Cells["ID"].Value);

                // Get the connection string
                string connectionString = ConfigurationManager.ConnectionStrings["AttendanceDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Attendance WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record Deleted Successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the record. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                // Refresh the DataGridView and clear inputs
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
