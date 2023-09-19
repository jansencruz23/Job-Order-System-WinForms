using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using MySql.Data.MySqlClient;
using Job_Order_System.Data;

namespace Job_Order_System
{
    public partial class Admin : Form
    {
        MySqlConnection con = new MySqlConnection(Database.CONNECTION_STRING);
        MySqlCommand cmd;
        DataTable dt;
        DataSet ds = new DataSet();
        public Admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            try
            {
                // Open the MySQL connection
                con.Open();

                // Select data from the MySQL database
                MySqlCommand cm = new MySqlCommand("SELECT * FROM tbl_user WHERE Status = 1", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cm);
                dt = new DataTable();
                da.Fill(dt);
                datagrid.DataSource = dt;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Close the MySQL connection
                con.Close();
            }

            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;

            DisplayTechnician();
        }

        private void DisplayTechnician()
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Technician FROM tbl_technician", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("Technician", typeof(string));
                dt.Load(reader);
                cbTechnician.ValueMember = "Technician";
                cbTechnician.DataSource = dt;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void AddTechnician()
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tbl_technician WHERE Technician = @Technician", con);
                cmd.Parameters.AddWithValue("@Technician", txtTechnician.Text);

                MySqlDataAdapter oda = new MySqlDataAdapter(cmd);
                DataTable existingTechnicians = new DataTable();
                oda.Fill(existingTechnicians);

                int existing = existingTechnicians.Rows.Count;

                if (existing > 0)
                {
                    MessageBox.Show("Technician Already Exists. Try Again", "Technician Add Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string addTechnician = "INSERT INTO tbl_technician (Technician) VALUES (@Technician)";
                    cmd = new MySqlCommand(addTechnician, con);
                    cmd.Parameters.AddWithValue("@Technician", txtTechnician.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Technician added successfully!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            // Optionally, you may want to refresh the Technician list after adding a new one.
            DisplayTechnician();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdminRegister().Show();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DeletedUser().Show();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DeletedJobOrder().Show();
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Restart();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Restart();
            }
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("LastName LIKE '%{0}%'", txtSearch.Text);
            datagrid.DataSource = dv;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddTechnician();
        }

        private void DeleteTechnician()
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM tbl_technician WHERE Technician = @Technician", con);
                cmd.Parameters.AddWithValue("@Technician", txtTechnician.Text);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Technician deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Technician not found or couldn't be deleted.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void cbTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTechnician.Text = cbTechnician.SelectedValue.ToString();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DeleteTechnician();
        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}