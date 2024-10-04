using System;
using System.Data;
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
            tbxSoDong.Text = "0";
            Helper.DoubleBufferded(dataGridView1, true);
        }

        //Line
        private void RowCount()
        {
            if (dataGridView1.DataSource != null)
            {
                int rowCount = dataGridView1.Rows.Count - 1;
                tbxSoDong.Text = rowCount.ToString();
            }
        }

        //Add checkbox
        private void AddCheckboxToDatagridview()
        {
                dataGridView1.Columns.Clear();
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "Select";
                checkBoxColumn.Name = "checkBoxColumn";
                checkBoxColumn.Width = 50;
                checkBoxColumn.ReadOnly = false;
                // Thêm cột Checkbox vào DataGridView
                dataGridView1.Columns.Add(checkBoxColumn);
        }

        private void ChuyenChungTuNhapKho_Load(object sender, EventArgs e)
        {
            txtID.Text = Helper.ID;
            laytennv();
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            switch(cbxLoaiDH.SelectedIndex)
            {
                case 0:
                    try
                    {
                        dataGridView1.DataSource = null;
                        connect.OpenConnect();

                        string sql = "SELECT oeb01,oeb03,'',oea04,tc_oxf002,ima01, " +
                        "CASE WHEN LENGTH(TRIM(TRANSLATE(SUBSTR(ima01, -2, 2), '0123456789', ' '))) " +
                        "IS NULL THEN '' ELSE SUBSTR(ima01,-2,2) END ima01N, ima021,    (SELECT NVL(SUM(ogb12), 0) FROM ogb_file, oga_file WHERE ogb01 = oga01 " +
                        "AND ogaconf <> 'X' AND OGB31 = oeb01 AND ogb32 = oeb03 ) ogb12 ,'', " +
                        "(SELECT NVL(SUM(sfv09), 0) FROM sfv_file, sfu_file WHERE sfvud01 = oeb01 AND sfvud10 = oeb03 AND sfu01 = sfv01 AND sfuconf<> 'X'  ) qty " +
                        "FROM oeb_file,tc_oxf_file,ima_file,oea_file " +
                        "WHERE YEAR(oea02) >= 2021 AND " +
                        "ta_oeb001 = tc_oxf001(+) " +
                        "AND oeb04 = ima01 AND oea01 = oeb01 AND(oeb70 = 'N' OR oeb70 IS NULL)";

                        DataTable dt = new DataTable();
                        AddCheckboxToDatagridview();
                        dt = connect.ExcuteQuery(sql);
                        dataGridView1.DataSource = dt;
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
                case 1:
                    try
                    {
                        dataGridView1.DataSource = null;
                        connect.OpenConnect();

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
                       "ORDER BY oga02 ASC, oeb01 ASC, oeb03 ASC ) WHERE qty < oeb12";

                        DataTable dt = new DataTable();
                        AddCheckboxToDatagridview();
                        dt = connect.ExcuteQuery(sqltr);
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

                case 2:
                    try
                    {
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        connect.OpenConnect();

                        string sqltr = "";

                        DataTable dt = new DataTable();
                        AddCheckboxToDatagridview();
                        dt = connect.ExcuteQuery(sqltr);
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
