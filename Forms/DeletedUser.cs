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
    public partial class DeletedUser : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        DataTable dt;
        public DeletedUser()
        {
            InitializeComponent();
        }

        private void DeletedUser_Load(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cm = new OleDbCommand("SELECT * FROM tbl_user WHERE Status = 0", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cm);
            dt = new DataTable();
            da.Fill(dt);
            dt.Columns.Add("Pic", Type.GetType("System.Byte[]"));
            foreach (DataRow drow in dt.Rows)
            {
                drow["Pic"] = File.ReadAllBytes(drow["PicPath"].ToString());
            }
            datagrid.DataSource = dt;
            con.Close();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Admin().Show();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdminRegister().Show();
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
