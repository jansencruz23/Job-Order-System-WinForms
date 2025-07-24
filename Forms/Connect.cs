using Job_Order_System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job_Order_System.Forms
{
    public partial class Connect : Form
    {
        public Connect()
        {
            InitializeComponent();
        }

        public string SelectedServer { get;set; }
        public string User { get;set; }
        public string Password { get;set; }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var serverIp = txtServerIp.Text.Trim();
            //var user = txtUser.Text.Trim();
            //var password = txtPassword.Text.Trim();

            //Database.SetConnectionString(serverIp, user, password
            Database.SetConnectionString(serverIp);
            var connectionString = Database.CONNECTION_STRING;

            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Hide();

                    SelectedServer = serverIp;
                    //User = user;
                    //Password = password;

                    new Login().Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
        }
    }
}
