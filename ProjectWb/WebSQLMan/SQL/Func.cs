using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;

namespace WebSQLMan.SQL
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


        public static SqlConnection ConnectToSQLserver(string Server, bool IntegratedSecurity = true)
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
            try
            {
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
            }
            catch (SqlException)
            {

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
                    Tables.Add((string)reader[0]);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
            return Tables;
        }

        public static DataSet RunQuery(SqlCommand sqlQuery, string server)
        {
            DataSet resultsDataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source={0};Initial Catalog=master;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False", server)))
            {

                string query = sqlQuery.CommandText;
                //Build a command that will execute your SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);


                //Open your connection prior to executing the Command
                sqlConnection.Open();


                SqlDataAdapter dbAdapter = new SqlDataAdapter();
                dbAdapter.SelectCommand = sqlQuery;
                sqlQuery.Connection = sqlConnection;

                //заполняем DataSet
                dbAdapter.Fill(resultsDataSet);
            }

            return resultsDataSet;
        }

        public static DataSet RunQuery(SqlCommand sqlQuery, string server, string DB)
        {
            DataSet resultsDataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(String.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False", server, DB)))
            {

                string query = sqlQuery.CommandText;
                //Build a command that will execute your SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                try
                {
                    //Open your connection prior to executing the Command
                    sqlConnection.Open();


                    SqlDataAdapter dbAdapter = new SqlDataAdapter();
                    dbAdapter.SelectCommand = sqlQuery;
                    sqlQuery.Connection = sqlConnection;

                    //заполняем DataSet

                    dbAdapter.Fill(resultsDataSet);
                }
                catch (SqlException)
                {

                }
            }

            return resultsDataSet;
        }

        [WebMethod()]
        [System.Web.Script.Services.ScriptMethod()]
        public static DataSet Input(string sql, string server, string DB)
        {
            DataSet resultSet = new DataSet();


            SqlCommand sqlQuery = new SqlCommand(sql);


            resultSet = RunQuery(sqlQuery, server, DB);



            return resultSet;


        }

        public static List<string> GetSystemDatabases(string ServerName)
        {
            List<String> databases = new List<String>();
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = ServerName;
            //connection.UserID = //get username;
            // connection.Password = //get password;
            connection.IntegratedSecurity = true;
            String strConn = connection.ToString();
            //create connection
            using (SqlConnection sqlConn = new SqlConnection(strConn))
            {
                try
                {  //open connection
                    sqlConn.Open();
                    //get databases
                    SqlCommand Command = sqlConn.CreateCommand();
                    Command.CommandText = "SELECT * FROM sys.databases Where Name IN ('master','model','msdb','tempdb') OR Is_distributor = 1;";
                    SqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read())
                    {
                        databases.Add((string)Reader["Name"]);
                    }
                }
                catch(SqlException)
                {

                }
                return databases;
            }
        }

        public static List<string> GetUserDatabases(string ServerName)
        {
            List<String> databases = new List<String>();
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = ServerName;
            //connection.UserID = //get username;
            // connection.Password = //get password;
            connection.IntegratedSecurity = true;
            String strConn = connection.ToString();
            //create connection
            using (SqlConnection sqlConn = new SqlConnection(strConn))
            {
                try
                {//open connection
                    sqlConn.Open();
                    //get databases
                    SqlCommand Command = sqlConn.CreateCommand();
                    Command.CommandText = "SELECT * FROM sys.databases Where Name NOT IN ('master','model','msdb','tempdb') AND Is_distributor <> 1;";
                    SqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read())
                    {
                        databases.Add((string)Reader["Name"]);
                    }
                }
                catch   (SqlException)
                {

                }
                return databases;
            }
        }

        public static List<string> GetDBsnapshots(string ServerName)
        {
            List<String> databases = new List<String>();
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = ServerName;
            //connection.UserID = //get username;
            // connection.Password = //get password;
            connection.IntegratedSecurity = true;
            String strConn = connection.ToString();
            //create connection
            using (SqlConnection sqlConn = new SqlConnection(strConn))
            {
                //open connection
                sqlConn.Open();
                //get databases
                SqlCommand Command = sqlConn.CreateCommand();
                Command.CommandText = "SELECT * FROM sys.databases Where Name Like '%Snapshot%';";
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    databases.Add((string)Reader["Name"]);
                }

                return databases;
            }
        }

        public static List<string> GetDBtables(string Server, string DB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> Tables = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = "SELECT Name FROM dbo.sysobjects WHERE (xtype = 'U');";
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Tables.Add((string)Reader[0]);
                }
                return Tables;
            }
        }

        public static List<string> GetViews(string Server, string DB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> Views = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = "SELECT Name FROM dbo.sysobjects WHERE (xtype = 'V');";
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Views.Add((string)Reader[0]);
                }
                return Views;
            }
        }

        public static List<string> GetColumns(string Server, string DB, string TB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> Columns = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = String.Format("select Name " +
                                                        "FROM sys.columns " +
                                                        "where object_id = OBJECT_ID('dbo.{0}')", TB);
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Columns.Add((string)Reader[0]);
                }
                return Columns;
            }
        }

        public static List<string> GetConstraints(string Server, string DB, string TB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> Constraints = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = String.Format("SELECT OBJECT_NAME(object_id) AS ConstraintName, " +
                                                       "SCHEMA_NAME(schema_id) AS SchemaName, " +
                                                        "type_desc AS ConstraintType " +
                                                        "FROM sys.objects " +
                                                "WHERE type_desc LIKE '%CONSTRAINT' AND OBJECT_NAME(parent_object_id)='{0}'", TB);
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Constraints.Add((string)Reader["ConstraintName"]);
                }
                return Constraints;
            }
        }

    }
}