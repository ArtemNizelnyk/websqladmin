using Ext.Net;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using WebSQLMan.Models;
using WebSQLMan.SQL;
using Ext.Net.MVC;
using System;

namespace WebSQLMan.Controllers
{

    public class MainController : Controller
    {
        // GET: Home

        public ActionResult Index(ConnectionParams cnParams)
        {


            HttpContext.Cache["CnInfo"] = cnParams;

            return View();

        }
        public RedirectToRouteResult Dissconnect()
        {
            return RedirectToAction("Index", "Login");
        }


        public Ext.Net.MVC.PartialViewResult Run(string query, string containerId)
        {
            //DataTable dt = new DataTable();
            ConnectionParams cnP = (ConnectionParams)HttpContext.Cache["CnInfo"];
            string server = cnP.ServerName;
            string db = (string)HttpContext.Cache["CurDB"];
            DataSet ds = SQL.Func.Input(query, server, db, cnP.Login, cnP.Password);



            return new Ext.Net.MVC.PartialViewResult
{

    ViewName = "Run",

    ContainerId = containerId,
    Model = ds, //passing the DataTable as my Model
    RenderMode = RenderMode.AddTo

};

        }

        public ActionResult AddTab(int index)
        {

            index++;
            this.GetCmp<Hidden>("HiddenNumber").Text = index.ToString();
            HttpContext.Cache["CurTab"] = index.ToString();

            Panel pan = new Panel
            {
                ID = "pan" + index,
                Title = "NewQuery" + index,
                Closable = true,
                Height = 250,
                MaxHeight = 400,
                Border = false,
                Items =
                {
                    new Container
                    {
                        ID="query"+ index,
                    Loader = new ComponentLoader
                    {
                        Url = Url.Action("Query", "Main"),
                        Mode = LoadMode.Script

                    }
                    }
                }

            };

            pan.AddTo(this.GetCmp<TabPanel>("SQLcommandTab"));


            this.GetCmp<TabPanel>("SQLcommandTab").SetActiveTab("pan" + index);


            return this.Direct();
        }

        public Ext.Net.MVC.PartialViewResult Query()
        {

            var ind = HttpContext.Cache["CurTab"].ToString();

            return new Ext.Net.MVC.PartialViewResult
             {
                 ViewName = "Query",
                 Model = "textAreaId" + ind,
                 ContainerId = "query" + ind,
                 WrapByScriptTag = false,
                 RenderMode = RenderMode.Replace
             };
        }




        public Ext.Net.MVC.PartialViewResult ContextMenu(string NodeText, string containerId)
        {
            DataTable dt = new DataTable();

            string querystring = string.Format("Select * From {0}", NodeText);
            ConnectionParams cnP = (ConnectionParams)HttpContext.Cache["CnInfo"];
            string server = cnP.ServerName;
            string db = NodeText;

            DataSet ds = SQL.Func.Input(querystring, server, db);

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Run",
                ContainerId = containerId,
                ClearContainer = true,
                Model = dt, //passing the DataTable as my Model
                RenderMode = RenderMode.AddTo,
                WrapByScriptTag = false

            };
        }




        public Ext.Net.MVC.PartialViewResult BasesTree(string containerId)
        {

            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "_BasesTree",
                ContainerId = containerId,
                WrapByScriptTag = false,
                ClearContainer = true,
                RenderMode = RenderMode.RenderTo
            };
        }

        [HttpGet]
        public JsonResult CashDB(string NodeData)
        {
            HttpContext.Cache["CurDB"] = NodeData;
            return new JsonResult();
        }

        public string GetRootNodes()
        {
            return "<ul> <li class=\"jstree-closed\">System Databases</li>" +
                        "<li class=\"jstree-closed\">User Databases</li>" +
                        "<li class=\"jstree-closed\">Database Snapshots</li> " +
                   "</ul>";
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetChildren(string id, string NodeData, string NodeText)
        {
            ConnectionParams CnP = (ConnectionParams)HttpContext.Cache["CnInfo"];

            string Server = CnP.ServerName;

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
                        Node.data = "Service Broker " + "(DB)" + NodeText + "(-DB)";
                        ;
                        Node.text = "Service Broker";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Storage " + "(DB)" + NodeText + "(-DB)";
                        ;
                        Node.text = "Storage";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Security " + "(DB)" + NodeText + "(-DB)";
                        ;
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
                    case "Programmability":
                        string db = ParseDB(NodeData);
                        #region ProgrammabilityNodeCONTENT
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "StoredProcs " + "(DB)" + db + "(-DB)";
                        Node.text = "Stored Procedures";
                        Node.state = "closed";
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Functions " + "(DB)" + db + "(-DB)";
                        Node.text = "Functions";
                        Node.state = "closed";

                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Triggers " + "(DB)" + db + "(-DB)";
                        Node.text = "Database Triggers";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);
                        Node = new G_JSTree();
                        Node.children = true;
                        Node.data = "Rules " + "(DB)" + db + "(-DB)";
                        Node.text = "Rules";
                        Node.state = "closed";
                        //Node.attr = new G_JsTreeAttribute(){ id = NodeText, selected=false };
                        Nodes.Add(Node);

                        #endregion
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "StoredProcs":
                        db = ParseDB(NodeData);
                        List<string> procs = Func.GetStoredProcs(Server, db);
                        foreach (string proc in procs)
                        {
                            Node = new G_JSTree();
                            Node.children = false;
                            Node.data = "Proc " + NodeData;

                            Node.text = proc;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "Functions":
                        db = ParseDB(NodeData);
                        List<string> funcs = Func.GetFuncs(Server, db);
                        foreach (string func in funcs)
                        {
                            Node = new G_JSTree();
                            Node.children = false;
                            Node.data = "Func " + NodeData;

                            Node.text = func;
                            Node.state = "closed";
                            Nodes.Add(Node);
                        }
                        return Json(Nodes, JsonRequestBehavior.AllowGet);
                    case "Triggers":
                        db = ParseDB(NodeData);
                        List<string> trigs = Func.GetTrigs(Server, db);
                        foreach (string trig in trigs)
                        {
                            Node = new G_JSTree();
                            Node.children = false;
                            Node.data = "Func " + NodeData;

                            Node.text = trig;
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
            if (str == null)
                return "";
            if (str.IndexOf(" ") < 0)
                return str;
            return str.Substring(0, str.IndexOf(" "));
        }

        string ParseDB(string str)
        {
            if (str == null)
                return "";
            int i = str.IndexOf("(DB)");
            int j = str.IndexOf("(-DB)");

            if (i == -1 || j == -1)
                return "";
            return str.Substring(i + 4, j - i - 4);
        }

        string ParseTbl(string str)
        {
            if (str == null)
                return "";
            int i = str.IndexOf("(Tbl)");
            int j = str.IndexOf("(-Tbl)");

            if (i == -1 || j == -1)
                return "";

            return str.Substring(i + 5, j - i - 5);
        }

    }
}
