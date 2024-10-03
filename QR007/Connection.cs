using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QR007
{
    public class Connection
    {
        string conn_MESPDB = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.31)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=topprod)));User ID=lelong;Password=lelong;";
        public void OpenConnect(string sqltr)
        {
            using (OracleConnection conn = new OracleConnection(conn_MESPDB))
            {
                conn.Open(); // Mở kết nối
                OracleCommand cmd = new OracleCommand(sqltr, conn);
                OracleDataReader dr = cmd.ExecuteReader();
            }
        }
    }
}
