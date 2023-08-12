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

namespace Job_Order_System
{
    public partial class Profile : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        public Profile()
        {
            InitializeComponent();
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            con.Open();
            cmd = new OleDbCommand("SELECT * FROM tbl_user WHERE ID = '" + Login.IDD + "' ", con);
            using (OleDbDataReader read = cmd.ExecuteReader())
            {
                while (read.Read())
                {
                    lblName.Text = ((read[3])).ToString() + " " + ((read[4])).ToString();
                    lblUsername.Text = ((read[1])).ToString();
     
                }
            }
            con.Close();
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
