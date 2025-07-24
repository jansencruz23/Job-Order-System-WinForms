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

namespace Job_Order_System
{
    public partial class Print : Form
    {
        public Print()
        {
            InitializeComponent();
        }

        private void print_Load(object sender, EventArgs e)
        {
            string connStr = Database.CONNECTION_STRING;
            var newConn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            this.tbl_joborderTableAdapter.Connection = newConn;
            this.tbl_joborderTableAdapter.Fill(this.jobOrderDataSet.tbl_joborder);

            this.reportViewer1.RefreshReport();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Main().Show();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;   
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2CustomGradientPanel2_SizeChanged(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
