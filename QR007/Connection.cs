using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QR007
{
    public class Connection
    {
        private string conn_MESPDB = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.31)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=topprod)));User ID=lelong;Password=lelong;";
        private string conn_orclpdb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.16.40.12)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MESPDB)));User ID=lelong;Password=lelong;";
        //conn_orclpdb = Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.40.12)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = MESPDB))); User ID = lelong; Password=lelong;")
        private OracleConnection connection;

        public void OpenConnect(string connStr)
        {
            connection = new OracleConnection(connStr);
            
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }


        public void CloseConnect(string connStr)
        {
            connection = new OracleConnection(connStr);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        //excute query to get data from database
        public DataTable ExcuteQuery(string sql)
        {
            DataTable dt = new DataTable();
            using (OracleCommand cmd = new OracleCommand(sql, connection))
            {
                using ( OracleDataReader dr = cmd.ExecuteReader())
                {
                    dt.Load(dr);
                }
            }
            return dt;
        }
    }
}
