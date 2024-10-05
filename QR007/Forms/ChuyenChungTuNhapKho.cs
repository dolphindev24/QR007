using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;

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
            tbxSoDong.Text = "0";
            Helper.DoubleBufferded(dataGridView1, true); 
            Helper.DoubleBufferded(dataGridView2, true);
        }

        //Update rowcount
        private void RowCount()
        {
            if (dataGridView1.DataSource != null)
            {
                int rowCount = dataGridView1.Rows.Count - 1;
                tbxSoDong.Text = rowCount.ToString();
            }
        }

        //Set column header
        private void SetColumnHeaders()
        {
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns["ogb31"].DataPropertyName = "oeb01"; //Ma DDh
            dataGridView1.Columns["ogb32"].DataPropertyName = "oeb03"; //Hang muc
            dataGridView1.Columns["ogaud12"].DataPropertyName = "''"; //So cont
            dataGridView1.Columns["oea03"].DataPropertyName = "oea04"; //Ten KH
            dataGridView1.Columns["tc_oxf002"].DataPropertyName = "tc_oxf002"; //Logo
            dataGridView1.Columns["dt06"].DataPropertyName = "ima01n"; //Mau sac
            dataGridView1.Columns["ima01"].DataPropertyName = "ima01"; //MVL
            dataGridView1.Columns["ima021"].DataPropertyName = "ima021"; //Quy cach
            dataGridView1.Columns["tc_oge001"].DataPropertyName = "''1"; //Xuong
            dataGridView1.Columns["oga02"].DataPropertyName = "oga02"; //Ngay xuat hang
            dataGridView1.Columns["ogb12"].DataPropertyName = "ogb12"; //SL cont
            dataGridView1.Columns["oeb12"].DataPropertyName = "oeb12"; //SL dat hang
            dataGridView1.Columns["sfv09"].DataPropertyName = "qty"; //SL da nhap
        }

        private void ChuyenChungTuNhapKho_Load(object sender, EventArgs e)
        {
            txtID.Text = Helper.ID;
            laytennv();
        }

        //TraCuu Button event
        private void btnTraCuu_Click(object sender, EventArgs e)
            {
            switch(cbxLoaiDH.SelectedIndex)
            {
                case 0: //Xuat khau
                    try
                    {
                        dataGridView1.DataSource = null;
                        tbxSoDong.Text = "0";

                        connect.OpenConnect();

                        string sql = "SELECT oeb01,oeb03,'',oea04,tc_oxf002,ima01, " +
                        "CASE WHEN LENGTH(TRIM(TRANSLATE(SUBSTR(ima01, -2, 2), '0123456789', ' '))) " +
                        "IS NULL THEN '' ELSE SUBSTR(ima01,-2,2) END ima01N, ima021,    (SELECT NVL(SUM(ogb12), 0) FROM ogb_file, oga_file WHERE ogb01 = oga01 " +
                        "AND ogaconf <> 'X' AND OGB31 = oeb01 AND ogb32 = oeb03 ) ogb12 ,'', TO_CHAR (oea02,'yyyy/mm/dd') oga02,oeb12,  " +
                        "(SELECT NVL(SUM(sfv09), 0) FROM sfv_file, sfu_file WHERE sfvud01 = oeb01 AND sfvud10 = oeb03 AND sfu01 = sfv01 AND sfuconf<> 'X'  ) qty " +
                        "FROM oeb_file,tc_oxf_file,ima_file,oea_file " +
                        "WHERE YEAR(oea02) >= 2021 AND " +
                        "ta_oeb001 = tc_oxf001(+) " +
                        "AND oeb04 = ima01 AND oea01 = oeb01 AND(oeb70 = 'N' OR oeb70 IS NULL)";

                        DataTable dt = new DataTable(); 

                        dt = connect.ExcuteQuery(sql);
                        SetColumnHeaders();

                        dataGridView1.DataSource = dt;
                        //SetColumnHeaders();

                        //Update rows
                        RowCount();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connect.CloseConnect();
                    }
                    break;
                case 1: //Noi dia
                    try
                    {
                        dataGridView1.DataSource = null;
                        tbxSoDong.Text = "0";

                        connect.OpenConnect(); //Open connect

                        string sqltr = "SELECT * FROM ( " +
                       "SELECT oeb01, oeb03, oea04, tc_oxf002, ima01, " +
                       "CASE WHEN LENGTH(TRIM(TRANSLATE(SUBSTR(ima01, -2, 2), '0123456789', ' '))) IS NULL THEN '' ELSE SUBSTR(ima01, -2, 2) END AS ima01N, ima021, " +
                       "SUM(ogb12) AS ogb12, TO_CHAR(oga02, 'yyyy/mm/dd') AS oga02, oeb12, " +
                       "(SELECT NVL(SUM(sfv09), 0) FROM sfv_file, sfu_file " +
                       " WHERE sfvud01 = a.oeb01 AND sfvud10 = a.oeb03 AND sfu01 = sfv01 AND sfuconf <> 'X') AS qty " +
                       "FROM oeb_file a, oea_file, sfb_file, ogb_file, oga_file, tc_oxf_file, ima_file " +
                       "WHERE oea01 = oeb01 AND oeaconf = 'Y' AND oeb70 = 'N' AND sfb22 = oeb01 " +
                       "AND oeb03 = sfb221 AND oeb01 = ogb31 AND oeb03 = ogb32 " +
                       "AND ta_oeb001 = tc_oxf001(+) AND oga01 = ogb01 AND ima01 = oeb04 " +
                       "AND sfb87 <> 'X' AND sfb01 IS NOT NULL AND oea04 LIKE 'VNM%' " +
                       "GROUP BY oeb01, oeb03, oea04, tc_oxf002, ima01, ima021, oga02, oeb12 " +
                       "ORDER BY oga02 ASC, oeb01 ASC, oeb03 ASC ) WHERE qty < oeb12 ";

                        sqltr += "UNION " +
                            "SELECT  UNIQUE ksf01 AS oeb01,ksg02 AS oeb03,ta_ksf001 AS oea04,ta_ksg001,ima01,CASE WHEN LENGTH(TRIM(TRANSLATE(SUBSTR(ima01,-2,2), '0123456789',' '))) IS NULL THEN '' " +
                            "ELSE SUBSTR(ima01,-2,2) END ima01N,ima021,0,'',ksg05 AS oeb12,     (SELECT NVL(SUM(sfv09),0) FROM sfv_file,sfu_file  WHERE sfvud01=a.ksf01 " +
                            "AND sfvud10=a.ksg02 " +
                            "AND sfu01=sfv01 " +
                            "AND sfuconf <> 'X') qty " +
                            "FROM ksg_file@vn_top a, ksf_file@vn_top a, ima_file@vn_top, occ_file@vn_top   WHERE ksg01 = ksf01 " +
                            "AND occ01= ta_ksf001  " +
                            "AND ksg03=ima01 " +
                            "AND ksfconf='Y'  " +
                            "AND SUBSTR(ksf01,1,5) IN ('BB571') " +
                            "AND (regexp_like(SUBSTR(ksg03,1,2),'[A-Z][0-9]') OR regexp_like(SUBSTR(ksg03,1,2),'[1-9][A-Z]'))";

                        DataTable dt = new DataTable();
                        dt = connect.ExcuteQuery(sqltr);
                        SetColumnHeaders();
                        dataGridView1.DataSource = dt;

                        RowCount(); //Update rowcount
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connect.CloseConnect();
                    }
                    break;
                case 2: //Khac
                    try
                    {
                        dataGridView1.DataSource = null;
                        tbxSoDong.Text = "0";
                        connect.OpenConnect();

                        string sqltr = "";

                        DataTable dt = new DataTable();
                        dt = connect.ExcuteQuery(sqltr);

                        SetColumnHeaders();
                        dataGridView1.DataSource = dt;

                        RowCount();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connect.CloseConnect();
                    }
                    break;

            }
        }

        //Get username by UserID
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex % 2 == 0)
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
        }
    }
}
