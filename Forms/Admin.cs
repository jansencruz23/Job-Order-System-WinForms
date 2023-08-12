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
    public partial class Admin : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        DataTable dt;
        DataSet ds = new DataSet();
        public Admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'db_joborderDataSet12.tbl_user' table. You can move, or remove it, as needed.
            this.tbl_userTableAdapter1.Fill(this.db_joborderDataSet12.tbl_user);
            // TODO: This line of code loads data into the 'db_joborderDataSet5.tbl_user' table. You can move, or remove it, as needed.
            


            con.Open();
            OleDbCommand cm = new OleDbCommand("SELECT * FROM tbl_user WHERE Status = 1", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cm);
            dt = new DataTable();
            da.Fill(dt);
            datagrid.DataSource = dt;
            con.Close();

            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;

            DisplayTechnician();
        }

        private void DisplayTechnician()
        {
            con.Open();
            cmd = new OleDbCommand("SELECT Technician FROM tbl_technician", con);
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Technician", typeof(string));
            dt.Load(reader);
            cbTechnician.ValueMember = "Technician";
            cbTechnician.DataSource = dt;
            con.Close();
        }

        private void AddTechnician()
        {
            cmd = new OleDbCommand("SELECT * FROM tbl_technician WHERE Technician = '" + txtTechnician.Text + "' ", con);
            OleDbDataAdapter oda = new OleDbDataAdapter(cmd);
            oda.Fill(ds);
            int existing = ds.Tables[0].Rows.Count;
            if (existing > 0)
            {
                MessageBox.Show("Technician Already Exists. Try Again", "Technician Add Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                new Admin().Show();
            }
            else
            {
                con.Open();
                string addTechnician = "INSERT INTO tbl_technician (Technician) VALUES('" + txtTechnician.Text + "' )";
                cmd = new OleDbCommand(addTechnician, con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Technician added successfully!");
                this.Hide();
                new Admin().Show();
            }
            
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdminRegister().Show();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DeletedUser().Show();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DeletedJobOrder().Show();
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Restart();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Restart();
            }
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("LastName LIKE '%{0}%'", txtSearch.Text);
            datagrid.DataSource = dv;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddTechnician();
        }

        private void DeleteTechnician()
        {
            con.Open();
            string addTechnician = "DELETE FROM tbl_technician WHERE Technician = '" + txtTechnician.Text + "' ";
            cmd = new OleDbCommand(addTechnician, con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Technician deleted successfully!");
            this.Hide();
            new Admin().Show();
        }

        private void cbTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTechnician.Text = cbTechnician.SelectedValue.ToString();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DeleteTechnician();
        }
    }
}
