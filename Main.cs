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
    public partial class Main : Form
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db_joborder.mdb");
        OleDbCommand cmd;
        DataTable dt;

        public string dateShort;
        private int JID = 1;
        public string dateLong;
        public bool fresh = true;
        public string name;
        public string iddd;

        public Guna2GradientButton mainBtnEdit{get { return btnEdit; }set { btnEdit = value; }}
        public Guna2GradientButton mainBtnDelete{get { return btnDelete; }set { btnDelete = value; }}
        public Guna2GradientButton mainBtnPrint{get { return btnPrint; }set { btnPrint = value; }}
        public Guna2TextBox maintxtJobNum{get { return txtJobNum; }set { txtJobNum = value; }}
        public Guna2TextBox maintxtJobNumEdit{get { return txtJobNumEdit; }set { txtJobNumEdit = value; }}
        public Guna2TextBox maintxtCName{get { return txtCName; }set { txtCName = value; }}
        public Guna2TextBox maintxtCNum{get { return txtCNum; }set { txtCNum = value; }}
        public Guna2TextBox maintxtCEmail{get { return txtEmail; }set { txtEmail = value; }}
        public Guna2TextBox maintxtCAddress{get { return txtCAddress; }set { txtCAddress = value; }}
        public Guna2TextBox mainORNo{get { return txtORNo; }set { txtORNo = value; }}
        public Guna2TextBox mainItemDesc{get { return txtItemDesc; }set { txtItemDesc = value; }}
        public Guna2TextBox mainItemBrand{get { return txtItemBrand; }set { txtItemBrand = value; }}
        public Guna2TextBox mainSerialNo{get { return txtSerialNo; }set { txtSerialNo = value; }}
        public Guna2TextBox mainProblem{get { return txtProb; }set { txtProb = value; }}
        public Guna2TextBox mainDiagErr{get { return txtDiagError; }set { txtDiagError = value; }}
        public Guna2TextBox mainPartsRep{get { return txtPartsReplaced; }set { txtPartsReplaced = value; }}
        public Guna2TextBox mainServiceFee{get { return txtServiceFee; }set { txtServiceFee = value; }}
        public Guna2TextBox mainAmountRep{get { return txtAmountReplaced; }set { txtAmountReplaced = value; }}
        public Guna2TextBox mainTotal{get { return txtTotal; }set { txtTotal = value; }}
        public DateTimePicker maindtp{get { return dtpDateRec; }set { dtpDateRec = value; }}
        public Guna2ComboBox maincbStatus{get { return cbStatus; }set { cbStatus = value; }}
        public Guna2ComboBox maintxtTech{get { return txtTechnician; }set { txtTechnician = value; }}
        public Guna2TextBox maintxtRemarks{get { return txtRemarks; }set { txtRemarks = value; }}

        private string ZeroJID;
        private int idz;
        private long first;
        private long second;
        private long zero = 0;
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                lblDate.Text = DateTime.Now.ToLongDateString() + " | " + DateTime.Now.ToLongTimeString();
                timer1.Start();



                txtServiceFee.Text = zero.ToString();
                txtAmountReplaced.Text = zero.ToString();

                dateShort = DateTime.Now.ToShortDateString().Replace("/", "");
                dateLong = DateTime.Now.ToShortDateString();
                cmd = new OleDbCommand("SELECT * FROM tbl_joborder ORDER BY ID ASC", con);
                using (OleDbDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        if (fresh == true)
                        {
                            JID = Convert.ToInt32((read[23]));
                            JID++;
                        }
                        else
                        {
                            JID = Convert.ToInt32((read[23]));
                        }


                    }

                }
                Zeros();
                if (fresh == true)
                {
                    txtJobNum.Text = "JO#" + dateShort + ZeroJID;
                }
                else
                {
                    idz = Convert.ToInt32(JID) - 1;
                    txtJobNum.Text = "JO#" + dateShort + idz;
                }



                DisplayTechnician();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Restart();
            }

            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;

        }

        private void DisplayTechnician()
        {

            cmd = new OleDbCommand("SELECT Technician FROM tbl_technician", con);
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Technician", typeof(string));
            dt.Load(reader);
            txtTechnician.ValueMember = "Technician";
            txtTechnician.DataSource = dt;
            con.Close();
        }

        private void Zeros()
        {
            if(JID.ToString().Length == 1)
            {
                ZeroJID = "000" + JID;
            }
            else if(JID.ToString().Length == 2)
            {
                ZeroJID = "00" + JID;
            }
            else if(JID.ToString().Length == 3)
            {
                ZeroJID = "0" + JID;
            }
            else
            {
                ZeroJID = JID.ToString() ;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToLongDateString() + " | " + DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (txtJobNum.Text == "" || txtCName.Text == "" || txtCNum.Text == "" || txtSerialNo.Text == "")
            {
                MessageBox.Show("Please fill out required fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                string addJobOrder = "INSERT INTO tbl_joborder (JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, [DateTime], [Status], [User], Technician, JID) VALUES('" + txtJobNum.Text + "', '" + txtCName.Text + "', '" + txtCNum.Text + "', '" + txtEmail.Text + "','" + txtCAddress.Text + "', '" + dtpDateRec.Value.ToShortDateString().ToString() + "', '" + txtORNo.Text + "', '" + txtItemDesc.Text + "', '" + txtItemBrand.Text + "', '" + txtSerialNo.Text + "' , '" + cbStatus.Text + "', '" + txtProb.Text + "', '" + txtDiagError.Text + "','" + txtPartsReplaced.Text + "', '" + txtRemarks.Text + "','" + txtServiceFee.Text + "', '" + txtAmountReplaced.Text + "', '" + txtTotal.Text + "', '" + lblDate.Text + "', ' 1 ', '" + name + "', '" + txtTechnician.Text + "', '"+ JID + "' )";
                cmd = new OleDbCommand(addJobOrder, con);
                cmd.ExecuteNonQuery();
                con.Close();
                this.Hide();
                Print print = new Print();
                print.Show();
            }

        }

        private void RefreshForm()
        {
            this.Hide();
            Main main = new Main();
            main.Show();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ViewTable().Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtJobNum.Text == "" || txtCName.Text == "" || txtCNum.Text == "" || txtSerialNo.Text == "")
            {
                MessageBox.Show("Please fill out required fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Open();
                string editJobOrder = "DELETE FROM tbl_joborder WHERE JobOrderNo = '" + txtJobNumEdit.Text + "'   ";
                cmd = new OleDbCommand(editJobOrder, con);
                cmd.ExecuteNonQuery();
                con.Close();
                
                con.Open();
                string addJobOrder = "INSERT INTO tbl_joborder (JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, [DateTime], [Status], [User], Technician, JID) VALUES('" + txtJobNumEdit.Text + "', '" + txtCName.Text + "', '" + txtCNum.Text + "', '" + txtEmail.Text + "','" + txtCAddress.Text + "', '" + dtpDateRec.Value.ToShortDateString().ToString() + "', '" + txtORNo.Text + "', '" + txtItemDesc.Text + "', '" + txtItemBrand.Text + "', '" + txtSerialNo.Text + "' , '" + cbStatus.Text + "', '" + txtProb.Text + "', '" + txtDiagError.Text + "','" + txtPartsReplaced.Text + "', '" + txtRemarks.Text + "','" + txtServiceFee.Text + "', '" + txtAmountReplaced.Text + "', '" + txtTotal.Text + "', '" + lblDate.Text + "', ' 1 ', '" + name + "', '" + txtTechnician.Text + "', '" + JID-- + "' )";
                cmd = new OleDbCommand(addJobOrder, con);
                cmd.ExecuteNonQuery();
                con.Close();


                DialogResult dialogResult = MessageBox.Show("Do you want to print this Job Order?", "Print Job Order?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    this.Hide();
                    Print print = new Print();
                    print.Show();
                }
                else if (dialogResult == DialogResult.No)
                {
                    RefreshForm();
                    return;
                }


            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this data?", "Delete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                con.Open();
                string editJobOrder = "UPDATE tbl_joborder SET Status = '0' WHERE JobOrderNo = '" + txtJobNumEdit.Text + "'";
                cmd = new OleDbCommand(editJobOrder, con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Job Order deleted successfully", "Job Order Deleted Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshForm();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
            
        }

        private void txtCNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == 22)
                e.Handled = true;
        }

        private void btnPic_Click(object sender, EventArgs e)
        {
            new Profile().ShowDialog();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatus.SelectedItem.Equals("WARRANTY IN"))
            {
                txtAmountReplaced.Enabled = false;
                txtServiceFee.Enabled = false;
            }
            else
            {
                txtAmountReplaced.Enabled = true;
                txtServiceFee.Enabled = true;
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CustomGradientPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtServiceFee_TextChanged(object sender, EventArgs e)
        {
            if(txtServiceFee.Text == "")
            {
                txtServiceFee.Text = zero.ToString();
            }
            else
            {
                first = Convert.ToInt64(txtServiceFee.Text);
                txtTotal.Text = (first + second).ToString();
            }

        }

        private void txtAmountReplaced_TextChanged(object sender, EventArgs e)
        {
            if (txtAmountReplaced.Text == "")
            {
                txtAmountReplaced.Text = zero.ToString();
            }
            else
            {
                second = Convert.ToInt64(txtAmountReplaced.Text);
                txtTotal.Text = (first + second).ToString();
            }

        }

        private void txtServiceFee_Click(object sender, EventArgs e)
        {
            txtServiceFee.SelectAll();
        }

        private void txtAmountReplaced_Click(object sender, EventArgs e)
        {
            txtAmountReplaced.SelectAll();
        }
    }
}
