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
using Guna.UI2.WinForms;

namespace Job_Order_System
{
    public partial class ViewTable : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        DataTable dt;
        public ViewTable()
        {
            InitializeComponent();
        }

        private void ViewTable_Load(object sender, EventArgs e)
        {
            this.tbl_joborderTableAdapter.Fill(this.db_joborderDataSet10.tbl_joborder);
            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT TOP 20 JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM tbl_joborder WHERE Status = 1 ORDER BY ID DESC", con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                datagrid.DataSource = dt;
                con.Close();

                datagrid.ClearSelection();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;
        }

        private void datagrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            Main main = new Main();
            main.mainBtnEdit.Visible = true;
            main.mainBtnDelete.Visible = true;
            main.mainBtnPrint.Visible = false;
            main.maintxtJobNum.Visible = false;
            main.maintxtJobNumEdit.Visible = true;
            main.maintxtTech.Text = datagrid.CurrentRow.Cells[1].Value.ToString();
            main.maintxtJobNumEdit.Text = datagrid.CurrentRow.Cells[2].Value.ToString();
            main.maintxtCName.Text = datagrid.CurrentRow.Cells[3].Value.ToString();
            main.maintxtCNum.Text = datagrid.CurrentRow.Cells[4].Value.ToString();
            main.fresh = false;
            main.iddd = datagrid.CurrentRow.Cells[0].Value.ToString();
            main.maintxtCEmail.Text = datagrid.CurrentRow.Cells[5].Value.ToString();
            main.maintxtCAddress.Text = datagrid.CurrentRow.Cells[6].Value.ToString();
            main.maindtp.Value = DateTime.Parse(datagrid.CurrentRow.Cells[7].Value.ToString());
            main.mainORNo.Text = datagrid.CurrentRow.Cells[8].Value.ToString();
            main.mainItemDesc.Text = datagrid.CurrentRow.Cells[9].Value.ToString();
            main.mainItemBrand.Text = datagrid.CurrentRow.Cells[10].Value.ToString();
            main.mainSerialNo.Text = datagrid.CurrentRow.Cells[11].Value.ToString();
            main.maincbStatus.Text = datagrid.CurrentRow.Cells[0].Value.ToString();
            main.mainProblem.Text = datagrid.CurrentRow.Cells[12].Value.ToString();
            main.mainDiagErr.Text = datagrid.CurrentRow.Cells[13].Value.ToString();
            main.mainPartsRep.Text = datagrid.CurrentRow.Cells[14].Value.ToString();
            main.maintxtRemarks.Text = datagrid.CurrentRow.Cells[15].Value.ToString();
            main.mainServiceFee.Text = datagrid.CurrentRow.Cells[16].Value.ToString();
            main.mainAmountRep.Text = datagrid.CurrentRow.Cells[17].Value.ToString();
            main.mainTotal.Text = datagrid.CurrentRow.Cells[18].Value.ToString();

            this.Dispose();
            main.Show();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Main().Show();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                datagrid.ClearSelection();

                con.Open();
                cmd = new OleDbCommand("SELECT TOP 20 JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM tbl_joborder WHERE Status = 1 " +
                    "AND (JobOrderNo LIKE '%' + @SearchTerm + '%' OR CustomerName LIKE '%' + @SearchTerm + '%' OR ItemDescription LIKE '%' + @SearchTerm + '%') ORDER BY ID DESC", con);
                cmd.Parameters.AddWithValue("@SearchTerm", txtSearch.Text);

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                datagrid.DataSource = dt;
                con.Close();

                datagrid.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //datagrid.ClearSelection();

            //DataView dv = new DataView(dt);
            //dv.RowFilter = string.Format("JobOrderNo LIKE '%{0}%' OR CustomerName LIKE '%{0}%' OR ItemDescription LIKE '%{0}%'", txtSearch.Text);
            //datagrid.DataSource = dv;

            //datagrid.ClearSelection();
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

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
                    row.Cells[0].Style.BackColor = Color.LightPink;
                }
                else if (row.Cells[0].Value.ToString() == "UNIT RELEASED")
                {
                    row.Cells[0].Style.BackColor = Color.LightBlue;
                }
                else if (row.Cells[0].Value.ToString() == "WARRANTY IN")
                {
                    row.Cells[0].Style.BackColor = Color.LightYellow;
                }
                else if (row.Cells[0].Value.ToString() == "WARRANTY OUT")
                {
                    row.Cells[0].Style.BackColor = Color.Violet;
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            string sql = "SELECT JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM tbl_joborder WHERE MONTH(`DateReceived`) =MONTH(NOW()) AND Status = 1 ORDER BY ID";
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
                cmd = new OleDbCommand("SELECT TOP 20 JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM tbl_joborder WHERE Status = 1 ORDER BY ID DESC", con);
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

        private void ViewTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}