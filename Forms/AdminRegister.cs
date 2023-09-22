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
using Job_Order_System.Data;
using System.Data.SqlClient;

namespace Job_Order_System
{
    public partial class AdminRegister : Form
    {
        SqlConnection con = new SqlConnection(Database.CONNECTION_STRING);
        SqlCommand cmd;
        DataSet ds = new DataSet();

        private string path = Path.GetFullPath(@"sentrow.png");
        private int UID = 1;
        public AdminRegister()
        {
            InitializeComponent();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtLN.Text) || string.IsNullOrEmpty(txtFN.Text) || string.IsNullOrEmpty(txtUN.Text) || string.IsNullOrEmpty(txtPW.Text) || string.IsNullOrEmpty(txtCPW.Text))
                {
                    MessageBox.Show("Please fill out required fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtPW.Text != txtCPW.Text)
                {
                    MessageBox.Show("Passwords don't match", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM joborder_winforms.tbl_user WHERE Username = @Username", con);
                    cmd.Parameters.AddWithValue("@Username", txtUN.Text);

                    SqlDataAdapter oda = new SqlDataAdapter(cmd);
                    DataTable existingUsers = new DataTable();
                    oda.Fill(existingUsers);

                    int existing = existingUsers.Rows.Count;

                    if (existing > 0)
                    {
                        MessageBox.Show("Username Already Exists. Try Again", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Hide();
                        new AdminRegister().Show();
                    }
                    else
                    {
                        string addJobOrder = "INSERT INTO joborder_winforms.tbl_user (Username, Password, FirstName, LastName, Status) VALUES (@Username, @Password, @FirstName, @LastName, @Status)";
                        cmd = new SqlCommand(addJobOrder, con);
                        cmd.Parameters.AddWithValue("@Username", txtUN.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPW.Text);
                        cmd.Parameters.AddWithValue("@FirstName", txtFN.Text);
                        cmd.Parameters.AddWithValue("@LastName", txtLN.Text);
                        cmd.Parameters.AddWithValue("@Status", 1);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User Register Successfully", "User Register Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        new Admin().Show();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void AdminRegister_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT MAX(ID) FROM joborder_winforms.tbl_user", con);
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    UID = Convert.ToInt32(result) + 1;
                }
                else
                {
                    UID = 1; // Set UID to 1 if the table is empty
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            /*ofdPic.Filter = "Image files (*.jpg; *.jpeg; *.png;) | *.jpg; *.jpeg; *.png;";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                path = ofdPic.FileName;
                pbpic.Image = new Bitmap(ofdPic.FileName);
            }*/

        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Admin().Show();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DeletedUser().Show();
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Restart();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DeletedJobOrder().Show();
        }

        private void guna2CustomGradientPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AdminRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
