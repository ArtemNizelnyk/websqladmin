using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SqlAdmin.SQL
{
    public class Func
    {
        public static List<string> GetActualSQLServers()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            System.Data.DataTable table = instance.GetDataSources();
            List<string> servers = new List<string>();
            foreach (System.Data.DataRow row in table.Rows)
            {
                servers.Add((string)row[0]);
            }
            return servers;
        }

        public static SqlConnection ConnectToSQLserver( string Server, bool IntegratedSecurity = true) 
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = Server;
            connection.IntegratedSecurity = IntegratedSecurity;
            String strConn = connection.ToString();
            //create connection
            SqlConnection sqlConn = new SqlConnection(strConn);
            //open connection
            sqlConn.Open();
            return sqlConn;
        }

        public static List<string> GetDatabasesOnServer(string ServerName)
        {
            List<String> databases = new List<String>();
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = ServerName;
            //connection.UserID = //get username;
            // connection.Password = //get password;
            connection.IntegratedSecurity = true;
            String strConn = connection.ToString();
            //create connection
            SqlConnection sqlConn = new SqlConnection(strConn);
            //open connection
            sqlConn.Open();
            //get databases
            DataTable tblDatabases = sqlConn.GetSchema("Databases");
            //close connection
            sqlConn.Close();
            //add to list
            foreach (DataRow row in tblDatabases.Rows)
            {
                String strDatabaseName = row["database_name"].ToString();

                databases.Add(strDatabaseName);
            }

            return databases;
        }

        public static List<string> GetTablesOnDataBase(SqlConnection cn, string DataBase)
        {
            List<string> Tables = new List<string>();

            string queryString = String.Format("Use {0} SELECT Name FROM dbo.sysobjects WHERE (xtype = 'U');", DataBase);  
                SqlCommand command = new SqlCommand(
                    queryString, cn);
       
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Tables.Add( (string)reader[0] );
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
                return Tables;
        }
    }
}