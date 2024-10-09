using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace QR007.Forms
{
    public partial class ChuyenChungTuNhapKho : Form
    {
        private string conn_MESPDB = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.31)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=topprod)));User ID=lelong;Password=lelong;";
        private string conn_orclpdb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.12)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MESPDB)));User ID=lelong;Password=lelong;";
        private string sqltr1;

        Connection connect = new Connection();
        DataTable dt = new DataTable();
        public ChuyenChungTuNhapKho()
        {
            InitializeComponent();
            cbxLoaiDH.SelectedIndex = 0;
            tbxSoDong.Text = "0";
            tbxSoDong2.Text = "0";
            tbxSDDC.Text = "0";
            tbxTongSLDaChon.Text = "0";
            tbxSoLuongCoTheNKConLai.Text = "0";
            Helper.DoubleBufferded(dataGridView1, true);
            Helper.DoubleBufferded(dataGridView2, true);
        }

        //Update rowcount
        private void RowCountDgv1()
        {
            //dataGridView1
            if (dataGridView1.DataSource != null)
            {
                int rowCount = dataGridView1.Rows.Count;
                tbxSoDong.Text = rowCount.ToString();
            }
        }

        private void RowCountDgv2()
        {
            int rowCount = dataGridView2.Rows.Count;
            tbxSoDong2.Text = rowCount.ToString();
        }

        //Set column header dgv1
        private void SetColumnHeadersDgv1()
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
            //dataGridView1.Columns["oga02"].DataPropertyName = "oga02"; //Ngay xuat hang
            //dataGridView1.Columns["ogb12"].DataPropertyName = "ogb12"; //SL cont
            dataGridView1.Columns["oeb12"].DataPropertyName = "oeb12"; //SL dat hang
            dataGridView1.Columns["sfv09"].DataPropertyName = "qty"; //SL da nhap
        }

        //Set column header dgv2
        private void SetColumnHeadersDgv2()
        {
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns["codeqty_wno"].DataPropertyName = "codeqty_wno";
            dataGridView2.Columns["codeqty_workid"].DataPropertyName = "codeqty_workid";
            dataGridView2.Columns["workorder_item"].DataPropertyName = "workorder_item";
            dataGridView2.Columns["sfe09"].DataPropertyName = "sfe09";
            dataGridView2.Columns["lima021"].DataPropertyName = "workorder_vnitemspec";
            dataGridView2.Columns["sfb08"].DataPropertyName = "workorder_qty";
            dataGridView2.Columns["stockin_qtyN"].DataPropertyName = "stockin_qtyN";
            dataGridView2.Columns["qrc17N2"].DataPropertyName = "qrc17N2";
            dataGridView2.Columns["qrc17"].DataPropertyName = "stockin_qty";

        }

        //Clear field
        private void Clear()
        {
            tbxMaDH.Text = string.Empty;
            tbxQuyCach.Text = string.Empty;
            tbxBoMonCT.Text = string.Empty;
            tbxMVL.Text = string.Empty;
            tbxMaDonCong.Text = string.Empty;
            tbxSDDC.Text = string.Empty;
            tbxTongSLDaChon.Text = string.Empty;
            tbxSoLuongCoTheNKConLai.Text = string.Empty;
        }

        private void ChuyenChungTuNhapKho_Load(object sender, EventArgs e)
        {
            txtID.Text = Helper.ID;
            laytennv();

            dt.Columns.Add("clk", typeof(string));
            dt.Columns.Add("codeqty_wno", typeof(string));
            dt.Columns.Add("codeqty_workid", typeof(string));
            dt.Columns.Add("workorder_item", typeof(string));
            dt.Columns.Add("sfe09", typeof(string));
            dt.Columns.Add("lima021", typeof(string));
            dt.Columns.Add("sfb08", typeof(Double));
            dt.Columns.Add("stockin_qtyN", typeof(Double));
            dt.Columns.Add("qrc17N2", typeof(Double));
            dt.Columns.Add("qrc17", typeof(Double));
        }

        //TraCuu Button event
        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            dt.Rows.Clear();
            dataGridView1.DataSource = dt;
            switch (cbxLoaiDH.SelectedIndex)
            {
                case 0: //Xuat khau
                    try
                    {
                        connect.OpenConnect(conn_MESPDB);
                        dataGridView1.DataSource = null;
                        dataGridView2.DataSource = null;
                        tbxSoDong.Text = "0";
                        cbxVitri.SelectedIndex = 0;

                        string sql = "SELECT oeb01,oeb03,'',oea04,tc_oxf002,ima01, " +
                        "CASE WHEN LENGTH(TRIM(TRANSLATE(SUBSTR(ima01, -2, 2), '0123456789', ' '))) " +
                        "IS NULL THEN '' ELSE SUBSTR(ima01,-2,2) END ima01N, ima021,    (SELECT NVL(SUM(ogb12), 0) FROM ogb_file, oga_file WHERE ogb01 = oga01 " +
                        "AND ogaconf <> 'X' AND OGB31 = oeb01 AND ogb32 = oeb03 ) ogb12 ,'', TO_CHAR (oea02,'yyyy/mm/dd') oga02,oeb12,  " +
                        "(SELECT NVL(SUM(sfv09), 0) FROM sfv_file, sfu_file WHERE sfvud01 = oeb01 AND sfvud10 = oeb03 AND sfu01 = sfv01 AND sfuconf<> 'X'  ) qty " +
                        "FROM oeb_file,tc_oxf_file,ima_file,oea_file " +
                        "WHERE YEAR(oea02) >= 2021 AND " +
                        "ta_oeb001 = tc_oxf001(+) " +
                        "AND oeb04 = ima01 AND oea01 = oeb01 AND(oeb70 = 'N' OR oeb70 IS NULL)";

                        if (tbxMaDH.Text.Length > 0)
                        {
                            string searchByMDH = tbxMaDH.Text.Trim();
                            searchByMDH = searchByMDH.Replace('*', '%');
                            sql += " AND oeb01 LIKE '" + searchByMDH + "'";
                        }

                        if (tbxQuyCach.Text.Length > 0)
                        {
                            string serachByQC = tbxQuyCach.Text.Trim();
                            serachByQC = serachByQC.Replace('*', '%');
                            sql += " AND ima021 LIKE '" + serachByQC + "'";
                        }

                        if (tbxBoMonCT.Text.Length > 0)
                        {
                            string searchByBMCT = tbxBoMonCT.Text.Trim();
                            searchByBMCT = searchByBMCT.Replace('*', '%');
                            sql += " AND sfb82 LIKE'" + searchByBMCT + "'";
                        }

                        if (tbxMVL.Text.Length > 0)
                        {
                            string searchByMVL = tbxMVL.Text.Trim();
                            searchByMVL = searchByMVL.Replace('*', '%');
                            sql += " AND ima01 LIKE'" + searchByMVL + "'";
                        }

                        OracleDataAdapter dta = new OracleDataAdapter(sql, conn_MESPDB);
                        dta.Fill(dt);
                        // Gọi hàm lọc DataTable
                        dt = FilterDataTable(dt);

                        // Thiết lập dữ liệu đã lọc cho DataGridView
                        SetColumnHeadersDgv1();

                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.NewRow();
                            row["clk"] = "False";
                        }


                        dataGridView1.DataSource = dt;

                        RowCountDgv1();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connect.CloseConnect(conn_MESPDB);
                    }
                    break;
                case 1: //Noi dia
                    try
                    {
                        connect.OpenConnect(conn_MESPDB);
                        dataGridView1.DataSource = null;
                        dataGridView2.DataSource = null;
                        tbxSoDong.Text = "0";
                        cbxVitri.SelectedIndex = 1;

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
                       "AND sfb87 <> 'X' AND sfb01 IS NOT NULL AND oea04 LIKE 'VNM%' ";

                        if (tbxMaDH.Text.Length > 0)
                        {
                            string searchByMDH = tbxMaDH.Text.Trim();
                            sqltr1 = " AND oeb01 LIKE '" + searchByMDH + "' ";

                            searchByMDH = searchByMDH.Replace('*', '%');
                        }
                        sqltr += sqltr1;

                        sqltr += "GROUP BY oeb01, oeb03, oea04, tc_oxf002, ima01, ima021, oga02, oeb12 " +
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

                        if (tbxMaDH.Text.Length > 0)
                        {
                            string searchByMDH = tbxMaDH.Text.Trim();
                            sqltr += " AND ksf01 LIKE '" + searchByMDH + "' ";

                            searchByMDH = searchByMDH.Replace('*', '%');
                        }

                        if (tbxQuyCach.Text.Length > 0)
                        {
                            string serachByQC = tbxQuyCach.Text.Trim();
                            serachByQC = serachByQC.Replace('*', '%');
                            sqltr += " AND ima021 LIKE '" + serachByQC + "'";
                        }

                        if (tbxBoMonCT.Text.Length > 0)
                        {
                            string searchByBMCT = tbxBoMonCT.Text.Trim();
                            searchByBMCT = searchByBMCT.Replace('*', '%');
                            sqltr += " AND sfb82 LIKE'" + searchByBMCT + "'";
                        }

                        if (tbxMVL.Text.Length > 0)
                        {
                            string searchByMVL = tbxMVL.Text.Trim();
                            searchByMVL = searchByMVL.Replace('*', '%');
                            sqltr += " AND ima01 LIKE'" + searchByMVL + "'";
                        }

                        OracleDataAdapter dta = new OracleDataAdapter(sqltr, conn_MESPDB);
                        dta.Fill(dt);

                        dt = FilterDataTable(dt);

                        SetColumnHeadersDgv1();

                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.NewRow();
                            row["clk"] = "False";
                        }

                        dataGridView1.DataSource = dt;

                        RowCountDgv1(); //Update rowcount
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connect.CloseConnect(conn_MESPDB);
                    }
                    break;
                case 2: //Khac
                    try
                    {
                        connect.OpenConnect(conn_orclpdb);
                        dataGridView1.DataSource = null;
                        dataGridView2.DataSource = null;
                        tbxSoDong.Text = "0";
                        cbxVitri.SelectedIndex = 2;

                        string sqltr = "SELECT * FROM ( " +
                          "SELECT UNIQUE codeqty_wno, codeqty_workid, workorder_item, workorder_vnitemspec, workorder_qty, " +
                          "NVL(codeqty_dqty - workorder_stockinqty, 0) stockin_qtyN, NVL(codeqty_dqty - workorder_stockinqty, 0) qrc17N2, " +
                          "NVL(workorder_stockinqty, 0) stockin_qty " +
                          "FROM QR_CODEQTY, QR_WORKORDER " +
                          "WHERE codeqty_workid = (SELECT MAX(codeqty_workid) FROM QR_CODEQTY a WHERE a.codeqty_wno = workorder_wno) " +
                          "AND workorder_wno = codeqty_wno AND workorder_status <> 8 " +
                          ") WHERE stockin_qtyN > 0";

                        if (tbxMaDonCong.Text.Length > 0)
                        {
                            string searchByMDC = tbxMaDonCong.Text.Trim();
                            searchByMDC = searchByMDC.Replace('*', '%');
                            sqltr += " AND codeqty_wno LIKE '" + searchByMDC + "'";
                        }

                        if (tbxMaVatLieu1.Text.Length > 0)
                        {
                            string searchByMaVL1 = tbxMaVatLieu1.Text.Trim();
                            searchByMaVL1 = searchByMaVL1.Replace('*', '%');
                            sqltr += " AND workorder_item LIKE '" + searchByMaVL1 + "'";
                        }

                        OracleDataAdapter dta = new OracleDataAdapter(sqltr, conn_orclpdb);
                        dta.Fill(dt);

                        SetColumnHeadersDgv2();

                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.NewRow();
                            row["clk"] = "False";

                        }

                        dataGridView2.DataSource = dt;

                        //get save location
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells["codeqty_wno"].Value != null)
                            {
                                string doncong = row.Cells["codeqty_wno"].Value.ToString();
                                string vitriLuu = check_add_vitriluu1(doncong);
                                row.Cells["sfe09"].Value = vitriLuu;
                            }
                        }

                        RowCountDgv2();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connect.CloseConnect(conn_orclpdb);
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

        //Calculator so luong nhap kho con lai 
        private void CalculatorSLNKCL(DataGridViewCellEventArgs e)
        {
            //So luong dat hang | oeb12
            int quantityOrdered = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["oeb12"].Value);
            //So luong da nhap kho | sfv09
            int quantityInStock = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["sfv09"].Value);

            int result = quantityOrdered - quantityInStock;
            tbxSoLuongCoTheNKConLai.Text = result.ToString();
        }

        //Checkbox checked
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["dt00"].Index && e.RowIndex >= 0)
            {
                try
                {
                    connect.OpenConnect(conn_orclpdb);
                    // connect.OpenConnect(conn_MESPDB);

                    // Get the current value of the checkbox
                    bool isChecked = Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["dt00"].Value);

                    // Uncheck other checkboxes
                    UncheckOtherCheckboxes(e.RowIndex, dataGridView1, "dt00");

                    // Update the value of the current checkbox
                    dataGridView1.Rows[e.RowIndex].Cells["dt00"].Value = !isChecked;

                    // Get oeb01 value from current row
                    var ima01Value = dataGridView1.Rows[e.RowIndex].Cells["ima01"].Value.ToString();

                    // Check the checkbox status after updating
                    if (!isChecked) // If the new checkbox is selected
                    {
                        LoadDataToDatagridview2(ima01Value);

                        CalculatorSLNKCL(e);

                        RowCountDgv2();
                    }
                    else
                    {
                        //dataGridView2.DataSource = null;
                        dataGridView2.Rows.Clear();
                        RowCountDgv2();
                    }

                    // Uncheck all checkboxes in DataGridView2
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells["clk"].Value = false; // Uncheck all checkboxes in DataGridView2
                    }

                    // Reset TextBox to 0 after unchecking all checkboxes in DataGridView2
                    tbxSDDC.Text = "0";
                    tbxTongSLDaChon.Text = "0";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connect.CloseConnect(conn_orclpdb);
                    // connect.CloseConnect(conn_MESPDB);
                }
            }
        }

        //Check vi tri
        private string check_add_vitriluu1(string doncong)
        {
            string checkViTriLuu = " ";

            try
            {
                connect.OpenConnect(conn_MESPDB);

                if (doncong.Substring(0, 5) == "BC511" || doncong.Substring(0, 5) == "BB511" ||
                    doncong.Substring(0, 5) == "BC512" || doncong.Substring(0, 5) == "BB512" ||
                    doncong.Substring(0, 5) == "BC510" || doncong.Substring(0, 5) == "BC51E")
                {
                    checkViTriLuu = "X";
                }

                if (doncong.Substring(0, 5) == "BB51A" || doncong.Substring(0, 5) == "BC51A" ||
                    doncong.Substring(0, 5) == "BC51B" || doncong.Substring(0, 5) == "BB51B" ||
                    doncong.Substring(0, 5) == "BC51C")
                {
                    checkViTriLuu = "T";
                }

                if (doncong.Substring(0, 5) == "BC521" || doncong.Substring(0, 5) == "BB521")
                {
                    checkViTriLuu = "T";
                }

                if (doncong.Substring(0, 5) == "BC526" || doncong.Substring(0, 5) == "BB526" ||
                    doncong.Substring(0, 5) == "BC52A" || doncong.Substring(0, 5) == "BB52A")
                {
                    string sqltr1 = "SELECT UNIQUE sfe09 FROM sfe_file WHERE sfe01='" + doncong + "' " +
                                    "AND regexp_like(SUBSTR(sfe07,1,1), '^[A-Z]') AND regexp_like(SUBSTR(sfe07,2,1), '^[0-9]') ";

                    using (OracleConnection conn = new OracleConnection(conn_MESPDB))
                    {
                        conn.Open();
                        OracleCommand cmd = new OracleCommand(sqltr1,conn);
                        OracleDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal("sfe09")))
                            {
                                checkViTriLuu = dr["sfe09"].ToString();
                            }
                            else
                            {
                                checkViTriLuu = "Null";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connect.CloseConnect(conn_MESPDB);
            }
            return checkViTriLuu;
        }

        //check if oeb12 = qty => remove row
        private DataTable FilterDataTable(DataTable dt)
        {
            // Tạo một DataTable mới để chứa các hàng đã lọc
            DataTable filteredTable = dt.Clone(); // Clone cấu trúc của DataTable

            foreach (DataRow row in dt.Rows)
            {
                object ogb12Value = row["oeb12"];
                object qtyValue = row["qty"];

                // Kiểm tra giá trị null và loại bỏ hàng nếu oeb12 = qty
                if (ogb12Value != DBNull.Value && qtyValue != DBNull.Value)
                {
                    double oeb12 = Convert.ToDouble(ogb12Value);
                    decimal qty = Convert.ToDecimal(qtyValue);

                    if (oeb12 != (double)qty)
                    {
                        filteredTable.ImportRow(row); // Thêm hàng vào DataTable đã lọc
                    }
                }
                else
                {
                    filteredTable.ImportRow(row); // Giữ lại hàng nếu một trong hai giá trị là null
                }
            }
            return filteredTable;
        }

        //Uncheck checkbox
        private void UncheckOtherCheckboxes(int currentRowIndex, DataGridView dgv, string checkboxName)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Index != currentRowIndex && row.Cells[checkboxName].Value != null && (bool)row.Cells[checkboxName].Value)
                {
                    row.Cells[checkboxName].Value = false; // Bỏ chọn checkbox
                }
            }
        }

        //Load data from database to datagridview
        private void LoadDataToDatagridview2(string ima01Value)
        {
            dataGridView2.Rows.Clear();
            string sqltr = "SELECT * FROM ( " +
               "SELECT UNIQUE codeqty_wno, codeqty_workid, workorder_item, '', workorder_vnitemspec, workorder_qty, " +
               "NVL(codeqty_dqty - workorder_stockinqty, 0) stockin_qtyN, NVL(workorder_stockinqty, 0) stockin_qty " +
               "FROM QR_CODEQTY, QR_WORKORDER " +
               "WHERE codeqty_workid = (SELECT MAX(codeqty_workid) FROM QR_CODEQTY a WHERE a.codeqty_wno = workorder_wno) " +
               "AND workorder_wno = codeqty_wno AND workorder_status <> 8 " +
               ") WHERE stockin_qtyN > 0 and workorder_item = '" + ima01Value + "'";

            // Mở kết nối Oracle trước khi thực thi câu lệnh
            using (OracleConnection connection = new OracleConnection(conn_orclpdb))
            {
                connection.Open(); // Mở kết nối

                using (OracleCommand sql_ora = new OracleCommand(sqltr, connection))
                {
                    using (OracleDataReader dt_qrc = sql_ora.ExecuteReader())
                    {
                        while (dt_qrc.Read())
                        {
                            string checkViTriLuu = check_add_vitriluu1(dt_qrc["codeqty_wno"].ToString());

                            //check cbxViTri value
                            if (cbxVitri.SelectedIndex == 0 && checkViTriLuu == "X" ||
                                (cbxVitri.SelectedIndex == 1 && checkViTriLuu == "T") ||
                                (cbxVitri.SelectedIndex == 2 && (checkViTriLuu == "X" || checkViTriLuu == "T")))
                            {
                                dataGridView2.Rows.Add("False",
                                                   dt_qrc["codeqty_wno"].ToString(),
                                                   dt_qrc["codeqty_workid"].ToString(),
                                                   dt_qrc["workorder_item"].ToString(),
                                                   checkViTriLuu,
                                                   dt_qrc["workorder_vnitemspec"].ToString(),
                                                   dt_qrc["workorder_qty"].ToString(),
                                                   dt_qrc["stockin_qtyN"].ToString(),
                                                   dt_qrc["stockin_qtyN"].ToString(),
                                                   dt_qrc["stockin_qty"].ToString());
                            }
                        }
                    }
                }
            }
            //RowCountDgv2();
        }

        private void btnHuyChon_Click(object sender, EventArgs e)
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

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra chỉ số hàng và cột
            if (e.RowIndex >= 0) // Đảm bảo là hàng hợp lệ
            {
                // Nếu cột hiện tại là qrc17N2
                if (dataGridView2.Columns[e.ColumnIndex].Name == "qrc17N2")
                {
                    e.CellStyle.BackColor = Color.LightGoldenrodYellow; // Tô màu đỏ cho cột qrc17N2
                }
                else if (e.RowIndex % 2 == 0) // Nếu hàng là hàng chẵn
                {
                    e.CellStyle.BackColor = Color.LightGray; // Tô màu xám nhạt cho hàng
                }
            }
        }

        private void cbxLoaiDH_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = cbxLoaiDH.SelectedIndex;
            if (selected == 0)
            {
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                tbxSoDong2.Text = "0";
                tbxSoDong.Text = "0";
                tbxSDDC.Text = "0";
                tbxTongSLDaChon.Text = "0";
                tbxSoLuongCoTheNKConLai.Text = "0";
                Clear();
            }
            else if (selected == 1)
            {
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                tbxSoDong2.Text = "0";
                tbxSoDong.Text = "0";
                tbxSDDC.Text = "0";
                tbxTongSLDaChon.Text = "0";
                tbxSoLuongCoTheNKConLai.Text = "0";
                Clear();
            }
            else if (selected == 2)
            {
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                tbxSoDong2.Text = "0";
                tbxSoDong.Text = "0";
                tbxSDDC.Text = "0";
                tbxTongSLDaChon.Text = "0";
                tbxSoLuongCoTheNKConLai.Text = "0";
                Clear();
            }
        }

        private void cbxVitri_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["dt00"].Value != null && (bool)row.Cells["dt00"].Value == true)
                {

                    //row.Cells["dt00"].Value = false;
                    //dataGridView2.Rows.Clear();

                    // Lấy giá trị từ cột 'ima01' của hàng hiện tại
                    string ima01Value = row.Cells["ima01"].Value.ToString();

                    // Xóa các hàng trong dataGridView2
                    dataGridView2.Rows.Clear();

                    // Load lại dữ liệu cho dataGridView2 dựa trên điều kiện mới từ cbxVitri
                    LoadDataToDatagridview2(ima01Value);
                    RowCountDgv2();
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //int DEm = 0;
            if (e.ColumnIndex == 0)
            {
                dataGridView2.EndEdit();
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int l_sodongdaimport = 0;
            int DEm = 0;
            double total = 0;

            // Đếm số checkbox đã được chọn
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                l_sodongdaimport++;
                // Kiểm tra nếu checkbox được check
              
                if (dataGridView2.Rows[i].Cells["clk"].Value.ToString() == "True")
                {
                    DEm++; // Đếm số checkbox được chọn
                    total += Convert.ToDouble(dataGridView2.Rows[i].Cells["qrc17N2"].Value);
                }
            }

            // Hiển thị số lượng checkbox đã được check vào tbxSDDC
            tbxSDDC.Text = DEm.ToString();

            // Hiển thị tổng số lượng đã chọn
            tbxTongSLDaChon.Text = total.ToString();

            // Kiểm tra nếu tổng số lượng đã chọn lớn hơn số lượng có thể nhập kho còn lại
            if (!String.IsNullOrEmpty(tbxSoLuongCoTheNKConLai.Text) && total > Convert.ToDouble(tbxSoLuongCoTheNKConLai.Text))
            {
                MessageBox.Show("Số lượng nhập kho dự tính không thể lớn hơn số lượng nhập kho còn lại.");
                dataGridView2.Rows[e.RowIndex].Cells["clk"].Value = "False"; // Uncheck checkbox nếu vượt quá
                tbxSDDC.Text = (DEm - 1).ToString(); // Giảm số lượng đã chọn
            }

            // Kiểm tra nếu số checkbox được chọn vượt quá 5
            if (DEm > 5)
            {
                MessageBox.Show("Số dòng được chọn vượt quá giới hạn 5", "Thông báo", MessageBoxButtons.OK);
                dataGridView2.Rows[e.RowIndex].Cells["clk"].Value = "False"; // Uncheck checkbox nếu vượt quá
                tbxSDDC.Text = (DEm - 1).ToString(); // Giảm số lượng đã chọn sau khi uncheck
            }
        }

        private void ChuyenChungTuNhapKho_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
