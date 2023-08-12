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

namespace Job_Order_System
{
    public partial class AdminRegister : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        DataSet ds = new DataSet();

        private string path = Path.GetFullPath(@"sentrow.png");
        private int UID = 1;
        public AdminRegister()
        {
            InitializeComponent();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            if (txtLN.Text == "" || txtFN.Text == "" || txtUN.Text == "" || txtPW.Text == "" || txtCPW.Text == "")
            {
                MessageBox.Show("Please fill out required fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(txtPW.Text != txtCPW.Text)
            {
                MessageBox.Show("Passwords don't match", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                cmd = new OleDbCommand("SELECT * FROM tbl_user WHERE Username = '" + txtUN.Text + "' ", con);
                OleDbDataAdapter oda = new OleDbDataAdapter(cmd);
                oda.Fill(ds);
                int existing = ds.Tables[0].Rows.Count;
                if (existing > 0)
                {
                    MessageBox.Show("Username Already Exists. Try Again", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Hide();
                    new AdminRegister().Show();
                }
                else
                {
                    con.Open();
                    string addJobOrder = "INSERT INTO tbl_user VALUES('" + UID.ToString() + "', '" + txtUN.Text + "', '" + txtPW.Text + "', '" + txtFN.Text + "', '" + txtLN.Text + "' , '" + 1 + "' )";
                    cmd = new OleDbCommand(addJobOrder, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Register Successfully", "User Register Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    new Admin().Show();
                    con.Close();
                }

            }
        }

        private void AdminRegister_Load(object sender, EventArgs e)
        {
            con.Open();
            cmd = new OleDbCommand("SELECT * FROM tbl_user", con);
            using (OleDbDataReader read = cmd.ExecuteReader())
            {
                while (read.Read())
                {
                    UID = Convert.ToInt32((read[0]));
                    UID++;
                }
            }
            con.Close();
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
    }
}
