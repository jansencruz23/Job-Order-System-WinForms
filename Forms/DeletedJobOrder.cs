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
    public partial class DeletedJobOrder : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        DataTable dt;
        public DeletedJobOrder()
        {
            InitializeComponent();
        }

        private void DeletedJobOrder_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'db_joborderDataSet10.tbl_joborder' table. You can move, or remove it, as needed.
            this.tbl_joborderTableAdapter.Fill(this.db_joborderDataSet10.tbl_joborder);
            // TODO: This line of code loads data into the 'db_joborderDataSet8.tbl_joborder' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'db_joborderDataSet6.tbl_joborder' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'db_joborderDataSet1.tbl_joborder' table. You can move, or remove it, as needed.

            con.Open();
            cmd = new OleDbCommand("SELECT JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, User, Technician FROM tbl_joborder WHERE Status = 0", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            datagrid.DataSource = dt;
            con.Close();

            datagrid.ClearSelection();

            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;
        }



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            datagrid.ClearSelection();
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("JobOrderNo LIKE '%{0}%' OR CustomerName LIKE '%{0}%' OR ItemDescription LIKE '%{0}%'", txtSearch.Text);
            datagrid.DataSource = dv;
            datagrid.ClearSelection();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdminRegister().Show();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Admin().Show();
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

        private void datagrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in datagrid.Rows)
            {
                if (row.Cells[0].Value.ToString() == "UNIT FIXED")
                {
                    row.Cells[0].Style.BackColor = Color.LightGreen;
                }
                else if (row.Cells[0].Value.ToString() == "RECEIVED PENDING")
                {
                    // row.DefaultCellStyle.BackColor = Color.LightSalmon; // Use it in order to colorize all cells of the row

                    row.Cells[0].Style.BackColor = Color.LightSalmon;
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM tbl_joborder WHERE MONTH(`DateReceived`) =MONTH(NOW()) AND Status = 0 ORDER BY ID";
            loadData(sql);

        }

        private void loadData(string sql)
        {
            OleDbDataAdapter da;
            try
            {
                con.Open();
                cmd = new OleDbCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                da = new OleDbDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataTable();
                da.Fill(dt);

                datagrid.DataSource = dt;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();

            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM tbl_joborder WHERE Status = 0 ORDER BY ID", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                datagrid.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();

            }
        }
    }
}
