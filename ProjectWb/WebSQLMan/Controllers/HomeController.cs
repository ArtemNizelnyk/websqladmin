﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using Ext.Net;
using System.Web.Script.Services;
using TestTree.Models;
using WebSQLMan.SQL;
using System.Data.SqlClient;
using WebSQLMan.Models;

namespace WebSQLMan.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index( ConnectionParams cnParams )
        {
            ViewBag.Message = "Type your query here";

            return View(cnParams);  // В дальнейшем [закешировать, а не передавать] cnParams!

        }


        public Ext.Net.MVC.PartialViewResult Run(string query, string containerId, string server)
        {
            DataTable dt = new DataTable();

            DataSet ds = SQL.Func.Input(query, server);

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }


            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Run",
                ContainerId = containerId,
                Model = dt, //passing the DataTable as my Model
                ClearContainer = true,
                RenderMode = RenderMode.AddTo

            };
        }

        public Ext.Net.MVC.PartialViewResult BasesTree(string containerId, string server)
        {
            ViewBag.server = server;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "_BasesTree",
                ContainerId = containerId,
                ClearContainer = true,
                //RenderMode = RenderMode.RenderTo
            };
        }

        public string GetRootNodes()
        {
            return "<ul> <li class=\"jstree-closed\">System Databases</li>" +
                        "<li class=\"jstree-closed\">User Databases</li>" +
                        "<li class=\"jstree-closed\">Database Snapshots</li> " +
                   "</ul>";
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetChildren(string id, string NodeData, string NodeText, string server)
        {
            string Server = server;

            SqlConnectionStringBuilder cnBuilder = new SqlConnectionStringBuilder();
            cnBuilder.DataSource = Server;
            cnBuilder.InitialCatalog = "master";
            cnBuilder.IntegratedSecurity = true;
            using (SqlConnection CurrConn = new SqlConnection(cnBuilder.ConnectionString))  // Захардкодил!!  В дальнейшем достать конекшн из КЄША
            {
                //CurrConn.Open();
                List<G_JSTree> Nodes = new List<G_JSTree>();

                G_JSTree Node = new G_JSTree();

                switch (id)
                {
                    case "j1_1":
                        List<string> SystemDBs = Func.GetSystemDatabases(Server);
                        foreach (string SysDB in SystemDBs)
                        {
                            Node = new G_JSTree();
                            Node.children = true;
                            Node.data = "DB";
                            Node.text = SysDB;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "j1_2":
                        List<string> UserDBs = Func.GetUserDatabases(Server);
                        foreach (string UserDB in UserDBs)
                        {
                            Node = new G_JSTree();
                            Node.children = true;
                            Node.data = "DB";  //  Все узлы баз получают аттрибут DB для дальнейшей идентификации
                            Node.text = UserDB;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "j1_3":
                        List<string> DBsnapshots = Func.GetDBsnapshots(Server);
                        foreach (string DBsnapshot in DBsnapshots)
                        {
                            Node = new G_JSTree();
                            Node.children = true;
                            Node.data = "DB";
                            Node.text = DBsnapshot;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                };

                switch (GetFirstWord(NodeData))
                {
                    case "DB":
                        #region DBnodeCONTENT
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Tables " + "(DB)" + NodeText + "(-DB)";
                        Node.text = "Tables";
                        Node.state = "closed";
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Views " + "(DB)" + NodeText + "(-DB)";
                        Node.text = "Views";
                        Node.state = "closed";

                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Synonyms " + "(DB)" + NodeText + "(-DB)";
                        Node.text = "Synonyms";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Programmability " + "(DB)" + NodeText + "(-DB)";
                        Node.text = "Programmability";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Service Broker " + "(DB)" + NodeText + "(-DB)"; ;
                        Node.text = "Service Broker";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Storage " + "(DB)" + NodeText + "(-DB)"; ;
                        Node.text = "Storage";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Security " + "(DB)" + NodeText + "(-DB)"; ;
                        Node.text = "Security";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        #endregion
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "Tables":
                        string DB = ParseDB(NodeData);
                        List<string> tables = Func.GetDBtables(Server, DB);
                        foreach (string table in tables)
                        {
                            Node = new G_JSTree();
                            Node.children = true;
                            Node.data = "Table " + NodeData;

                            Node.text = table;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "Views":
                        DB = ParseDB(NodeData);
                        List<string> views = Func.GetViews(Server, DB);
                        foreach (string view in views)
                        {
                            Node = new G_JSTree();
                            Node.children = true;
                            Node.data = "View " + NodeData;

                            Node.text = view;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    /* Раскрытие конкретной табл*/
                    case "Table":
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Columns " + "(Tbl)" + NodeText + "(-Tbl)" + NodeData;
                        Node.text = "Columns";
                        Node.state = "closed";
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Constraints " + "(Tbl)" + NodeText + "(-Tbl)" + NodeData;
                        Node.text = "Constraints";
                        Node.state = "closed";

                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Keys " + "(Tbl)" + NodeText + "(-Tbl)" + NodeData;
                        Node.text = "Keys";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Indexes " + "(Tbl)" + NodeText + "(-Tbl)" + NodeData;
                        Node.text = "Indexes";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "statistics " + "(Tbl)" + NodeText + "(-Tbl)" + NodeData;
                        Node.text = "statistics";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "Constraints":
                        string Tb = ParseTbl(NodeData);
                        string Db = ParseDB(NodeData);
                        List<string> constraints = Func.GetConstraints(Server, Db, Tb);
                        foreach (string constraint in constraints)
                        {
                            Node = new G_JSTree();
                            Node.children = true;
                            Node.data = "Constraints " + NodeData;

                            Node.text = constraint;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "Columns":
                        Tb = ParseTbl(NodeData);
                        Db = ParseDB(NodeData);
                        List<string> columns = Func.GetColumns(Server, Db, Tb);
                        foreach (string column in columns)
                        {
                            Node = new G_JSTree();
                            Node.children = false;
                            Node.data = "Column " + NodeData;

                            Node.text = column;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                }

                return new JsonResult();
            }

        }

        string GetFirstWord(string str)
        {
            if (str.IndexOf(" ") < 0)
                return str;
            return str.Substring(0, str.IndexOf(" "));
        }

        string ParseDB(string str)
        {
            int i = str.IndexOf("(DB)");
            int j = str.IndexOf("(-DB)");

            return str.Substring(i + 4, j - i - 4);
        }

        string ParseTbl(string str)
        {
            int i = str.IndexOf("(Tbl)");
            int j = str.IndexOf("(-Tbl)");

            return str.Substring(i + 5, j - i - 5);
        }

    }
}