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
using Job_Order_System.Services;
using System.Runtime.Caching;
using Job_Order_System.Data;
using System.Data.SqlClient;

namespace Job_Order_System
{
    public partial class DeletedJobOrder : Form
    {
        SqlConnection con = new SqlConnection(Database.CONNECTION_STRING);
        DataTable dt;

        private MemoryCache cache = MemoryCache.Default;
        private const string CACHEKEY = "JobOrderDataDelete";

        public DeletedJobOrder()
        {
            InitializeComponent();
        }

        private void DeletedJobOrder_Load(object sender, EventArgs e)
        {
            DataTable cachedData = CacheService.Get<DataTable>(CACHEKEY);

            if (cachedData != null)
            {
                dt = cachedData;
            }
            else
            {
                // If data is not in the cache, load it from the database
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM joborder_winforms.tbl_joborder WHERE Status = 0 ORDER BY ID DESC", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    // Cache the data with a specific expiration time (e.g., 10 minutes)
                    CacheService.Add(CACHEKEY, dt, DateTimeOffset.Now.AddMinutes(10));
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Display the data in your DataGridView
            datagrid.DataSource = dt;
            datagrid.ClearSelection();
        }



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable cachedData = CacheService.Get<DataTable>(CACHEKEY);

            if (cachedData != null)
            {
                dt = cachedData;
            }
            else
            {
                // Load all data from the database into the DataTable
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT JobOrderNo, CustomerName, ItemDescription FROM joborder_winforms.tbl_joborder", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    con.Close();

                    // Cache the data with a specific expiration time (e.g., 10 minutes)
                    CacheService.Add(CACHEKEY, dt, DateTimeOffset.Now.AddMinutes(10));
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            try
            {
                datagrid.ClearSelection();

                // Filter the DataTable based on the search term
                DataRow[] filteredRows = dt.Select($"JobOrderNo LIKE '%{txtSearch.Text}%' OR CustomerName LIKE '%{txtSearch.Text}%' OR ItemDescription LIKE '%{txtSearch.Text}%'");

                // Take only the top 20 records from the filtered results
                DataTable filteredData = filteredRows.Take(20).CopyToDataTable();

                datagrid.DataSource = filteredData;
            }
            catch (Exception ex)
            {
                // Handle any exceptions as needed
                MessageBox.Show(ex.Message);
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
            string sql = "SELECT JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM joborder_winforms.tbl_joborder WHERE MONTH(DateReceived) = MONTH(NOW()) AND Status = 0 ORDER BY ID";
            loadData(sql);
        }

        private void loadData(string sql)
        {
            DataTable cachedData = CacheService.Get<DataTable>(CACHEKEY);

            if (cachedData != null)
            {
                dt = cachedData;
            }
            else
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);

                    // Store all data in the cache
                    // CacheService.Add(CACHEKEY, dt, DateTimeOffset.Now.AddMinutes(10));
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }

            try
            {
                datagrid.ClearSelection();

                // Take only the top 20 records
                DataTable top20Data = dt.AsEnumerable().Take(20).CopyToDataTable();
                datagrid.DataSource = top20Data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, Technician, ID FROM joborder_winforms.tbl_joborder WHERE Status = 0 ORDER BY ID DESC", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                datagrid.DataSource = dt;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void DeletedJobOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
