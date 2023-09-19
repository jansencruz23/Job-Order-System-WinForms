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
using MySql.Data.MySqlClient;

namespace Job_Order_System
{
    public partial class Profile : Form
    {
        MySqlConnection con = new MySqlConnection(Database.CONNECTION_STRING);
        MySqlCommand cmd;
        public Profile()
        {
            InitializeComponent();
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT FirstName, LastName, Username FROM tbl_user WHERE ID = @UserID", con);
                cmd.Parameters.AddWithValue("@UserID", Login.IDD);

                using (MySqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        lblName.Text = read.GetString(0) + " " + read.GetString(1);
                        lblUsername.Text = read.GetString(2);
                    }
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
