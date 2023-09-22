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
using System.Data.SqlClient;
using Job_Order_System.Data;

namespace Job_Order_System
{
    public partial class Login : Form
    {
        SqlConnection con = new SqlConnection(Database.CONNECTION_STRING);
       // OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= db_joborder.mdb");
        OleDbCommand cmd = new OleDbCommand();
        public static string IDD;
        public static string password;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            con.Open();
            string login = "SELECT * FROM joborder_winforms.tbl_user WHERE Username = @Username AND Password = @Password";
            SqlCommand cmd = new SqlCommand(login, con);
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
            SqlDataReader read = cmd.ExecuteReader();
            if (read.Read())
            {
                IDD = read[0].ToString();
                this.Hide();
                Main main = new Main();
                main.Show();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password, Please Try Again", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtUsername.Focus();
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Hide();
        }

        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            new LoginAdmin().Show();
            this.Hide();
        }

        private void cbPW_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPW.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
