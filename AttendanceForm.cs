using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Linq;

namespace AttendanceSystem
{
    public partial class AttendanceForm : MetroFramework.Forms.MetroForm
    {
        private string comboBox1FilePath = "Student.txt";
        private string comboBox2FilePath = "RollNo.txt";

        public AttendanceForm()
        {
            InitializeComponent();
            LoadComboBoxData();
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
                        cmd.Parameters.AddWithValue("@name", txtStudentName.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@roll", txtRollNumber.SelectedItem?.ToString() ?? "");
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

        private void add_Click(object sender, EventArgs e)
        {
            string studentName = name.Text.Trim();
            string rollNumber = rollno.Text.Trim();

            if (!string.IsNullOrEmpty(studentName) && !string.IsNullOrEmpty(rollNumber))
            {
                if (!txtStudentName.Items.Contains(studentName) && !txtRollNumber.Items.Contains(rollNumber))
                {
                    // Add data to ComboBoxes
                    txtStudentName.Items.Add(studentName);
                    txtRollNumber.Items.Add(rollNumber);

                    // Save data to files
                    SaveComboBoxData();

                    // Clear TextBoxes
                    name.Clear();
                    rollno.Clear();

                    MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("This data already exists in the ComboBoxes.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please fill both fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string studentName = name.Text.Trim();
            string rollNumber = rollno.Text.Trim();

            int nameIndex = txtStudentName.Items.IndexOf(studentName);
            int rollNumberIndex = txtRollNumber.Items.IndexOf(rollNumber);

            if (nameIndex >= 0 && rollNumberIndex >= 0 && nameIndex == rollNumberIndex)
            {
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete:\n\nName: {studentName}\nRoll Number: {rollNumber}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    txtStudentName.Items.RemoveAt(nameIndex);
                    txtRollNumber.Items.RemoveAt(rollNumberIndex);

                    SaveComboBoxData(); // Save changes to file
                    name.Clear();
                    rollno.Clear();

                    MessageBox.Show("Data deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No matching data found in ComboBoxes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void SaveComboBoxData()
        {
            File.WriteAllLines(comboBox1FilePath, txtStudentName.Items.Cast<string>());
            File.WriteAllLines(comboBox2FilePath, txtRollNumber.Items.Cast<string>());
        }

        private void LoadComboBoxData()
        {
            if (File.Exists(comboBox1FilePath))
            {
                txtStudentName.Items.AddRange(File.ReadAllLines(comboBox1FilePath));
            }

            if (File.Exists(comboBox2FilePath))
            {
                txtRollNumber.Items.AddRange(File.ReadAllLines(comboBox2FilePath));
            }
        }
    }
}
    

