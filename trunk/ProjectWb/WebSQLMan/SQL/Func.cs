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

        public static void ConnectToSQLserver(string Server, bool IntegratedSecurity = true, string login = "", string pass = "")
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = Server;
            connection.IntegratedSecurity = IntegratedSecurity;
            connection.UserID = login??String.Empty;
            connection.Password = pass??String.Empty;
            
            String strConn = connection.ToString();
            SqlConnection sqlConn;
            
            
                using (
                 sqlConn = new SqlConnection(strConn))
                {
                    sqlConn.Open();
                }
            
            
            
        }



        public static DataSet RunQuery(SqlCommand sqlQuery, string server, string login, string passWord)
        {
            try
            {
                DataSet resultsDataSet = new DataSet();
                SqlConnectionStringBuilder CnStrBuilder = new SqlConnectionStringBuilder();
                CnStrBuilder.ConnectTimeout = 15;
                CnStrBuilder.IntegratedSecurity = true;
                CnStrBuilder.DataSource = server;
                CnStrBuilder.Encrypt = false;
                CnStrBuilder.TrustServerCertificate = false;
                CnStrBuilder.Password = passWord ?? String.Empty;
                CnStrBuilder.UserID = login ?? String.Empty;
                using (SqlConnection sqlConnection = new SqlConnection(CnStrBuilder.ConnectionString))
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
            catch(SqlException ex)
            {
                throw ex;
            }

            }

        public static DataSet RunQuery(SqlCommand sqlQuery, string server, string DB, string login, string passWord)
        {
            try {
            DataSet resultsDataSet = new DataSet();
            SqlConnectionStringBuilder CnStrBuilder = new SqlConnectionStringBuilder();
            CnStrBuilder.ConnectTimeout = 15;
            CnStrBuilder.IntegratedSecurity = true;
            CnStrBuilder.DataSource = server;
            CnStrBuilder.InitialCatalog = DB;
            CnStrBuilder.Encrypt = false;
            CnStrBuilder.TrustServerCertificate = false;
            CnStrBuilder.Password = passWord ?? String.Empty;
            CnStrBuilder.UserID = login ?? String.Empty;

            using (SqlConnection sqlConnection = new SqlConnection(CnStrBuilder.ConnectionString))
            {

                string query = sqlQuery.CommandText;
                //Build a command that will execute your SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

              //  try
                //{
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
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        [WebMethod()]
        [System.Web.Script.Services.ScriptMethod()]
        public static DataSet Input(string sql, string server, string DB, string login = "", string pass = "")
        {
            DataSet resultSet = new DataSet();


            SqlCommand sqlQuery = new SqlCommand(sql);

            try
            {
                resultSet = RunQuery(sqlQuery, server, DB, login, pass);




                return resultSet;
            }
           
                catch(SqlException ex)
            {
                throw ex;
            }
            

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
                catch (SqlException ex)
                {
                    throw ex;
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
                catch (SqlException ex)
                {
                    throw ex;
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
                try
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
                }
                catch(SqlException ex)
                {
                    throw ex;
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

        public static List<string> GetStoredProcs(string Server, string DB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> StoredProcs = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = String.Format("SELECT name "+
                                                    "FROM dbo.sysobjects "+
                                                    "WHERE (type = 'P')" );
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    StoredProcs.Add((string)Reader[0]);
                }
                return StoredProcs;
            }
        }

        public static List<string> GetFuncs(string Server, string DB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> Funcs = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = String.Format("SELECT *"+
                                                    "FROM sys.objects "+
                                                    "WHERE type IN ('FN', 'IF', 'TF')");
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Funcs.Add((string)Reader[0]);
                }
                return Funcs;
            }
        }

        public static List<string> GetTrigs(string Server, string DB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> Trigs = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = String.Format("SELECT *" +
                                                    "FROM sys.objects " +
                                                    "WHERE type = 'TR'");
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Trigs.Add((string)Reader[0]);
                }
                return Trigs;
            }
        }

        public static List<string> GetRules(string Server, string DB)
        {
            SqlConnectionStringBuilder con = new SqlConnectionStringBuilder();
            con.IntegratedSecurity = true;
            con.DataSource = Server;
            con.InitialCatalog = DB;
            using (SqlConnection cn = new SqlConnection(con.ConnectionString))
            {
                cn.Open();
                List<string> Trigs = new List<string>();
                SqlCommand Command = cn.CreateCommand();
                Command.CommandText = String.Format("SELECT *" +
                                                    "FROM sys.objects " +
                                                    "WHERE type = 'TR'");
                SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Trigs.Add((string)Reader[0]);
                }
                return Trigs;
            }
        }
    }
}