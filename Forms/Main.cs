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
using Job_Order_System.Services;
using System.Runtime.Caching;
using Job_Order_System.Data;
using System.Data.SqlClient;

namespace Job_Order_System
{
    public partial class Main : Form
    {
        SqlConnection con = new SqlConnection(Database.CONNECTION_STRING);
        SqlCommand cmd;
        DataTable dt;

        private MemoryCache cache = MemoryCache.Default;
        private const string CACHEKEY = "JobOrderData";

        public string dateShort;
        private int JID = 1;
        public string dateLong;
        public bool fresh = true;
        public string name;
        public string iddd;

        public Guna2GradientButton mainBtnEdit { get { return btnEdit; } set { btnEdit = value; } }
        public Guna2GradientButton mainBtnDelete { get { return btnDelete; } set { btnDelete = value; } }
        public Guna2GradientButton mainBtnPrint { get { return btnPrint; } set { btnPrint = value; } }
        public Guna2TextBox maintxtJobNum { get { return txtJobNum; } set { txtJobNum = value; } }
        public Guna2TextBox maintxtJobNumEdit { get { return txtJobNumEdit; } set { txtJobNumEdit = value; } }
        public Guna2TextBox maintxtCName { get { return txtCName; } set { txtCName = value; } }
        public Guna2TextBox maintxtCNum { get { return txtCNum; } set { txtCNum = value; } }
        public Guna2TextBox maintxtCEmail { get { return txtEmail; } set { txtEmail = value; } }
        public Guna2TextBox maintxtCAddress { get { return txtCAddress; } set { txtCAddress = value; } }
        public Guna2TextBox mainORNo { get { return txtORNo; } set { txtORNo = value; } }
        public Guna2TextBox mainItemDesc { get { return txtItemDesc; } set { txtItemDesc = value; } }
        public Guna2TextBox mainItemBrand { get { return txtItemBrand; } set { txtItemBrand = value; } }
        public Guna2TextBox mainSerialNo { get { return txtSerialNo; } set { txtSerialNo = value; } }
        public Guna2TextBox mainProblem { get { return txtProb; } set { txtProb = value; } }
        public Guna2TextBox mainDiagErr { get { return txtDiagError; } set { txtDiagError = value; } }
        public Guna2TextBox mainPartsRep { get { return txtPartsReplaced; } set { txtPartsReplaced = value; } }
        public Guna2TextBox mainServiceFee { get { return txtServiceFee; } set { txtServiceFee = value; } }
        public Guna2TextBox mainAmountRep { get { return txtAmountReplaced; } set { txtAmountReplaced = value; } }
        public Guna2TextBox mainTotal { get { return txtTotal; } set { txtTotal = value; } }
        public DateTimePicker maindtp { get { return dtpDateRec; } set { dtpDateRec = value; } }
        public Guna2ComboBox maincbStatus { get { return cbStatus; } set { cbStatus = value; } }
        public Guna2ComboBox maintxtTech { get { return txtTechnician; } set { txtTechnician = value; } }
        public Guna2TextBox maintxtRemarks { get { return txtRemarks; } set { txtRemarks = value; } }

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
                SqlCommand cmd = new SqlCommand("SELECT ID FROM joborder_winforms.tbl_joborder ORDER BY ID ASC", con);
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        if (fresh == true)
                        {
                            JID = Convert.ToInt32((read[0]));
                            JID++;
                        }
                        else
                        {
                            JID = Convert.ToInt32((read[0]));
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
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Restart();
            }
            finally
            {
                con.Close();
            }

            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            WindowState = FormWindowState.Maximized;

        }

        private void DisplayTechnician()
        {

            try
            {
                //con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Technician FROM joborder_winforms.tbl_technician", con);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("Technician", typeof(string));
                dt.Load(reader);
                txtTechnician.ValueMember = "Technician";
                txtTechnician.DataSource = dt;
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

        private void Zeros()
        {
            if (JID.ToString().Length == 1)
            {
                ZeroJID = "000" + JID;
            }
            else if (JID.ToString().Length == 2)
            {
                ZeroJID = "00" + JID;
            }
            else if (JID.ToString().Length == 3)
            {
                ZeroJID = "0" + JID;
            }
            else
            {
                ZeroJID = JID.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToLongDateString() + " | " + DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtJobNum.Text) || string.IsNullOrEmpty(txtCName.Text) || string.IsNullOrEmpty(txtCNum.Text) || string.IsNullOrEmpty(txtSerialNo.Text))
            {
                MessageBox.Show("Please fill out required fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO joborder_winforms.tbl_joborder (JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, DateTime, Status, [User], Technician, JID) VALUES(@JobOrderNo, @CustomerName, @ContactNo, @EmailAddress, @Address, @DateReceived, @ORNo, @ItemDescription, @ItemBrand, @SerialNo, @JOStatus, @Problem, @DiagnoseError, @PartsReplaced, @Remarks, @ServiceFee, @AmountReplaced, @Total, @DateTime, @Status, @User, @Technician, @JID)", con);
                    cmd.Parameters.AddWithValue("@JobOrderNo", txtJobNum.Text);
                    cmd.Parameters.AddWithValue("@CustomerName", txtCName.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", txtCNum.Text);
                    cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Address", txtCAddress.Text);
                    cmd.Parameters.AddWithValue("@DateReceived", dtpDateRec.Value.ToShortDateString());
                    cmd.Parameters.AddWithValue("@ORNo", txtORNo.Text);
                    cmd.Parameters.AddWithValue("@ItemDescription", txtItemDesc.Text);
                    cmd.Parameters.AddWithValue("@ItemBrand", txtItemBrand.Text);
                    cmd.Parameters.AddWithValue("@SerialNo", txtSerialNo.Text);
                    cmd.Parameters.AddWithValue("@JOStatus", cbStatus.Text);
                    cmd.Parameters.AddWithValue("@Problem", txtProb.Text);
                    cmd.Parameters.AddWithValue("@DiagnoseError", txtDiagError.Text);
                    cmd.Parameters.AddWithValue("@PartsReplaced", txtPartsReplaced.Text);
                    cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@ServiceFee", txtServiceFee.Text);
                    cmd.Parameters.AddWithValue("@AmountReplaced", txtAmountReplaced.Text);
                    cmd.Parameters.AddWithValue("@Total", txtTotal.Text);
                    cmd.Parameters.AddWithValue("@DateTime", lblDate.Text);
                    cmd.Parameters.AddWithValue("@Status", 1);
                    cmd.Parameters.AddWithValue("@User", name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Technician", txtTechnician.Text);
                    cmd.Parameters.AddWithValue("@JID", JID);

                    cmd.ExecuteNonQuery();

                    con.Close();
                

                this.Dispose();
                Print print = new Print();
                print.Show();
                CacheService.Remove(CACHEKEY);
            }
        }

        private void RefreshForm()
        {
            this.Dispose();
            Main main = new Main();
            main.Show();
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new ViewTable().Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtJobNum.Text) || string.IsNullOrEmpty(txtCName.Text) || string.IsNullOrEmpty(txtCNum.Text) || string.IsNullOrEmpty(txtSerialNo.Text))
            {
                MessageBox.Show("Please fill out required fields", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    // Delete the existing job order based on the JobOrderNo
                    SqlCommand deleteJobOrderCmd = new SqlCommand("DELETE FROM joborder_winforms.tbl_joborder WHERE JobOrderNo = @JobOrderNo", con);
                    deleteJobOrderCmd.Parameters.AddWithValue("@JobOrderNo", txtJobNumEdit.Text);
                    deleteJobOrderCmd.ExecuteNonQuery();

                    // Insert the updated job order
                    SqlCommand addJobOrderCmd = new SqlCommand("INSERT INTO joborder_winforms.tbl_joborder (JobOrderNo, CustomerName, ContactNo, EmailAddress, Address, DateReceived, ORNo, ItemDescription, ItemBrand, SerialNo, JOStatus, Problem, DiagnoseError, PartsReplaced, Remarks, ServiceFee, AmountReplaced, Total, DateTime, Status, [User], Technician, JID) VALUES(@JobOrderNo, @CustomerName, @ContactNo, @EmailAddress, @Address, @DateReceived, @ORNo, @ItemDescription, @ItemBrand, @SerialNo, @JOStatus, @Problem, @DiagnoseError, @PartsReplaced, @Remarks, @ServiceFee, @AmountReplaced, @Total, @DateTime, @Status, @User, @Technician, @JID)", con);

                    addJobOrderCmd.Parameters.AddWithValue("@JobOrderNo", txtJobNumEdit.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@CustomerName", txtCName.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@ContactNo", txtCNum.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@Address", txtCAddress.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@DateReceived", dtpDateRec.Value.ToShortDateString());
                    addJobOrderCmd.Parameters.AddWithValue("@ORNo", txtORNo.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@ItemDescription", txtItemDesc.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@ItemBrand", txtItemBrand.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@SerialNo", txtSerialNo.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@JOStatus", cbStatus.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@Problem", txtProb.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@DiagnoseError", txtDiagError.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@PartsReplaced", txtPartsReplaced.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@ServiceFee", txtServiceFee.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@AmountReplaced", txtAmountReplaced.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@Total", txtTotal.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@DateTime", lblDate.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@Status", 1);
                    addJobOrderCmd.Parameters.AddWithValue("@User", name ?? (object)DBNull.Value);
                    addJobOrderCmd.Parameters.AddWithValue("@Technician", txtTechnician.Text);
                    addJobOrderCmd.Parameters.AddWithValue("@JID", JID - 1); // Decrease JID by 1

                    addJobOrderCmd.ExecuteNonQuery();

                    CacheService.Remove(CACHEKEY);

                    DialogResult dialogResult = MessageBox.Show("Do you want to print this Job Order?", "Print Job Order?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.Dispose();
                        Print print = new Print();
                        print.Show();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        RefreshForm();
                        return;
                    }
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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this data?", "Delete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    SqlCommand editJobOrderCmd = new SqlCommand("UPDATE joborder_winforms.tbl_joborder SET Status = '0' WHERE JobOrderNo = @JobOrderNo", con);
                    editJobOrderCmd.Parameters.AddWithValue("@JobOrderNo", txtJobNumEdit.Text);
                    editJobOrderCmd.ExecuteNonQuery();
                    MessageBox.Show("Job Order deleted successfully", "Job Order Deleted Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CacheService.Remove(CACHEKEY);
                    RefreshForm();
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
            if (txtServiceFee.Text == "")
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

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void UpdateCache(DataTable updatedData)
        {
            CacheItemPolicy cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) // Adjust the cache expiration time as needed
            };
            cache.Set(CACHEKEY, updatedData, cachePolicy);
        }

        private void btnCheckCustomer_Click(object sender, EventArgs e)
        {
            string customerName = txtCName.Text.Trim();

            if (string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Please enter a customer name.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                // Assuming you have a table called "Customers" with columns "CustomerName", "PhoneNumber", and "Email"
                string query = "SELECT TOP 1 * FROM joborder_winforms.tbl_joborder WHERE CustomerName = @customerName " +
                    "ORDER BY ID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@customerName", customerName);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Customer exists, retrieve values and display them
                    string phoneNumber = reader["ContactNo"].ToString();
                    string email = reader["EmailAddress"].ToString();
                    string address = reader["Address"].ToString();
                    string orNo = reader["OrNo"].ToString();
                    string itemDesc = reader["ItemDescription"].ToString();
                    string itemBrand = reader["ItemBrand"].ToString();
                    string serialNo = reader["SerialNo"].ToString();
                    string joStatus = reader["JOStatus"].ToString();
                    string problem = reader["Problem"].ToString();
                    string diagnoseError = reader["DiagnoseError"].ToString();
                    string partsReplaced = reader["PartsReplaced"].ToString();
                    string remarks = reader["Remarks"].ToString();
                    string serviceFee = reader["ServiceFee"].ToString();
                    string amountReplaced = reader["AmountReplaced"].ToString();
                    string total = reader["Total"].ToString();
                    string technician = reader["Technician"].ToString();

                    // Populate text boxes with retrieved values
                    txtCNum.Text = phoneNumber;
                    txtEmail.Text = email;
                    txtCAddress.Text = address;
                    txtORNo.Text = orNo;
                    txtItemDesc.Text = itemDesc;
                    txtItemBrand.Text = itemBrand;
                    txtSerialNo.Text = serialNo;
                    cbStatus.SelectedItem = joStatus;
                    txtProb.Text = problem;
                    txtDiagError.Text = diagnoseError;
                    txtPartsReplaced.Text = partsReplaced;
                    txtRemarks.Text = remarks;
                    txtServiceFee.Text = serviceFee;
                    txtAmountReplaced.Text = amountReplaced;
                    txtTotal.Text = total;
                    txtTechnician.SelectedItem = technician;

                    MessageBox.Show("Customer found.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Customer does not exist
                    MessageBox.Show("Customer not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // You can clear the text boxes here if needed
                    txtCNum.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                }

                reader.Close();
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
    }
}