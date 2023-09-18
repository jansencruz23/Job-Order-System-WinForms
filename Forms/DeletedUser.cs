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
using System.Runtime.Caching;
using Job_Order_System.Services;

namespace Job_Order_System
{
    public partial class DeletedUser : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        DataTable dt;

        private MemoryCache cache = MemoryCache.Default;
        private const string CACHEKEY = "JobOrderData";

        public DeletedUser()
        {
            InitializeComponent();
        }

        private void DeletedUser_Load(object sender, EventArgs e)
        {
            DataTable cachedData = CacheService.Get<DataTable>(CACHEKEY);

            if (cachedData != null)
            {
                dt = cachedData;
                datagrid.DataSource = dt;
            }
            else
            {
                con.Open();
                OleDbCommand cm = new OleDbCommand("SELECT * FROM tbl_user WHERE Status = 0 ORDER BY ID DESC", con); // Fetch only the top 20 records
                OleDbDataAdapter da = new OleDbDataAdapter(cm);
                dt = new DataTable();
                da.Fill(dt);
                dt.Columns.Add("Pic", Type.GetType("System.Byte[]"));
                foreach (DataRow drow in dt.Rows)
                {
                    drow["Pic"] = File.ReadAllBytes(drow["PicPath"].ToString());
                }
                // Store all data in the cache
                CacheService.Add(CACHEKEY, dt, DateTimeOffset.Now.AddMinutes(10));

                datagrid.DataSource = dt;
                con.Close();
            }
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

        private void DeletedUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
