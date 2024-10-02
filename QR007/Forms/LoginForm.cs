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
        Helper helper = new Helper();   

        public LoginForm()
        {
            InitializeComponent();
            txbUsername.Text = "H23275";
            txbPassword.Text = "it@H23275";
        }
        string conn_MESPDB = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.31)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=topprod)));User ID=lelong;Password=lelong;";


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txbUsername.Text == "" || txbPassword.Text == "")
            {
                MessageBox.Show("User or Password invalid!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                using (Oracle.ManagedDataAccess.Client.OracleConnection conn = new Oracle.ManagedDataAccess.Client.OracleConnection(conn_MESPDB))
                {
                    conn.Open(); // Mở kết nối
                    string sqltr = "SELECT COUNT(*) AS NUMS FROM tc_qrh_file WHERE tc_qrh001 = '" + txbUsername.Text + "' AND tc_qrh002 = '" + txbPassword.Text + "' ";
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
                            ChuyenChungTuNhapKho cctnk = new ChuyenChungTuNhapKho();
                            cctnk.Show();
                            this.Hide();
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
