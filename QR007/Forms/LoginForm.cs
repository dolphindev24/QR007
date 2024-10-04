using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;

namespace QR007.Forms
{
    public partial class LoginForm : Form
    {
        Connection connect = new Connection();

        string conn_MESPDB = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.31)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=topprod)));User ID=lelong;Password=lelong;";

        public LoginForm()
        {
            InitializeComponent();
            txtID.Text = Properties.Settings.Default.UserID;
            txbPassword.Text = Properties.Settings.Default.Password;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txbPassword.Text == "")
            {
                MessageBox.Show("User or Password invalid!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                using (Oracle.ManagedDataAccess.Client.OracleConnection conn = new Oracle.ManagedDataAccess.Client.OracleConnection(conn_MESPDB))
                {
                    conn.Open(); // Mở kết nối
                    string sqltr = "SELECT COUNT(*) AS NUMS FROM tc_qrh_file WHERE tc_qrh001 = '" + txtID.Text + "' AND tc_qrh002 = '" + txbPassword.Text + "' ";
                    Oracle.ManagedDataAccess.Client.OracleCommand cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(sqltr, conn);
                    Oracle.ManagedDataAccess.Client.OracleDataReader dr = cmd.ExecuteReader();
                    int strNum = 0;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            strNum = Convert.ToInt32(dr["NUMS"]);
                        }
                        if (strNum == 0)
                        {
                            MessageBox.Show("User or password invalid!"); //Tai khoan mat mã bị sai
                        }
                        else
                        {
                            // Lưu thông tin đăng nhập nếu checkbox được chọn
                            if (checkboxSaveAccount.Checked) // Thay 'chkRememberMe' bằng tên checkbox của bạn
                            {
                                Properties.Settings.Default.UserID = txtID.Text.Trim();
                                Properties.Settings.Default.Password = txbPassword.Text.Trim();
                                Properties.Settings.Default.Save();
                            }
                            else
                            {
                                // Xóa thông tin đăng nhập nếu checkbox không được chọn
                                Properties.Settings.Default.UserID = string.Empty;
                                Properties.Settings.Default.Password = string.Empty;
                                Properties.Settings.Default.Save();
                            } 

                            Helper.ID = txtID.Text.Trim();
                            ChuyenChungTuNhapKho cctnk = new ChuyenChungTuNhapKho();
                            cctnk.Show();
                            this.Hide();
                            //MessageBox.Show(Helper.ID);
                        }
                    }
                    else
                    {
                        MessageBox.Show("User or password invalid!"); //Tai khoan mat mã bị sai
                    }
                }
            }
        }
    }
}
