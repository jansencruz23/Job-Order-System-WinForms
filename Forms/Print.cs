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
            // TODO: This line of code loads data into the 'joborder_winforms_sqlDataSet.tbl_joborder' table. You can move, or remove it, as needed.
            this.tbl_joborderTableAdapter.Fill(this.joborder_winforms_sqlDataSet.tbl_joborder);
            // TODO: This line of code loads data into the 'jobOrderDataSet.tbl_joborder' table. You can move, or remove it, as needed.
            //this.tbl_joborderTableAdapter.Fill(this.jobOrderDataSet.tbl_joborder);
            // TODO: This line of code loads data into the 'jobOrderDataSet.tbl_joborder' table. You can move, or remove it, as needed.
            //this.tbl_joborderTableAdapter1.Fill(this.jobOrderDataSet.tbl_joborder);
            // TODO: This line of code loads data into the 'db_joborderDataSet10.tbl_joborder' table. You can move, or remove it, as needed.
            //this.tbl_joborderTableAdapter.Fill(this.db_joborderDataSet11.tbl_joborder);
            // TODO: This line of code loads data into the 'db_joborderDataSet9.tbl_joborder' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'db_joborderDataSet8.tbl_joborder' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'db_joborderDataSet1.tbl_joborder' table. You can move, or remove it, as needed.


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

        private void tbl_joborderBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
