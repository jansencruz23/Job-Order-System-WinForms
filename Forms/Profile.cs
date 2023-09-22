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
using Job_Order_System.Data;
using System.Data.SqlClient;

namespace Job_Order_System
{
    public partial class Profile : Form
    {
        SqlConnection con = new SqlConnection(Database.CONNECTION_STRING);
        SqlCommand cmd;
        public Profile()
        {
            InitializeComponent();
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT FirstName, LastName, Username FROM joborder_winforms.tbl_user WHERE ID = @UserID", con);
                cmd.Parameters.AddWithValue("@UserID", Login.IDD);

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        lblName.Text = read.GetString(0) + " " + read.GetString(1);
                        lblUsername.Text = read.GetString(2);
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Restart();
            }
            catch(Exception ex)
            {
                Application.Restart();
            }
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Profile_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
