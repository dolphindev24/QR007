using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace QR007.Forms
{
    public partial class ChuyenChungTuNhapKho : Form
    {
        string conn_MESPDB = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.31)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=topprod)));User ID=lelong;Password=lelong;";

        Connection connect = new Connection();
        public ChuyenChungTuNhapKho()
        {
            InitializeComponent();
            cbxLoaiDH.SelectedIndex = 0;
        }

        private void ChuyenChungTuNhapKho_Load(object sender, EventArgs e)
        {
            txtID.Text = Helper.ID;
            laytennv();
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            if (cbxLoaiDH.SelectedIndex == 0)
            {

                using (OracleConnection conn = new OracleConnection(conn_MESPDB))
                {
                    conn.Open();

                    //string sqltr = "SELECT oeb01, oeb03, '', oea04, tc_oxf002, ima01, CASE WHEN LENGTH(TRIM(TRANSLATE(SUBSTR(ima01, -2, 2), '0123456789', ' '))) IS NULL THEN '' ELSE SUBSTR(ima01, -2, 2) END AS ima01N, ima021, ( SELECT NVL(SUM(ogb12), 0) FROM ogb_file, oga_file WHERE ogb01 = oga01 AND ogaconf <> 'X' AND OGB31 = oeb01 AND ogb32 = oeb03 ) AS ogb12, '', ( SELECT NVL(SUM(sfv09), 0) FROM sfv_file, sfu_file WHERE sfvud01 = oeb01 AND sfvud10 = oeb03 AND sfu01 = sfv01 AND sfuconf <> 'X' ) AS qty FROM oeb_file, tc_oxf_file, ima_file, oea_file WHERE YEAR(oea02) >= 2021 AND ta_oeb001 = tc_oxf001(+) AND oeb04 = ima01 AND oea01 = oeb01 AND (oeb70 = 'N' OR oeb70 IS NULL";
                    string sql = "SELECT oeb01,oeb03,'',oea04,tc_oxf002,ima01, " +
                        "CASE WHEN LENGTH(TRIM(TRANSLATE(SUBSTR(ima01, -2, 2), '0123456789', ' '))) " +
                        "IS NULL THEN '' ELSE SUBSTR(ima01,-2,2) END ima01N, ima021,    (SELECT NVL(SUM(ogb12), 0) FROM ogb_file, oga_file WHERE ogb01 = oga01 " +
                        "AND ogaconf <> 'X' AND OGB31 = oeb01 AND ogb32 = oeb03 ) ogb12 ,'', " +
                        "(SELECT NVL(SUM(sfv09), 0) FROM sfv_file, sfu_file WHERE sfvud01 = oeb01 AND sfvud10 = oeb03 AND sfu01 = sfv01 AND sfuconf<> 'X'  ) qty " +
                        "FROM oeb_file,tc_oxf_file,ima_file,oea_file " +
                        "WHERE YEAR(oea02) >= 2021 AND " +
                        "ta_oeb001 = tc_oxf001(+) " +
                        "AND oeb04 = ima01 AND oea01 = oeb01 AND(oeb70 = 'N' OR oeb70 IS NULL)";

                    OracleCommand cmd = new OracleCommand(sql, conn);
                    OracleDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dataGridView1.DataSource = dt;
                }
            }
            else if (cbxLoaiDH.SelectedIndex == 1)
            {

            }
            else if(cbxLoaiDH.SelectedIndex == 2)
            {

            }
        }

        //Get username by ID
        private void laytennv()
        {
            using (OracleConnection conn = new OracleConnection(conn_MESPDB))
            {
                conn.Open(); // Mở kết nối
                string sqltr = "SELECT ta_cpf001 FROM cpf_file WHERE cpf01 = '" + txtID.Text + "' AND  cpf35 is null ";
                OracleCommand cmd = new OracleCommand(sqltr, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        txt_name.Text = dr["ta_cpf001"].ToString();
                    }

                }
            }

        }

    }
    
}
