using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLMan.Models;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebSQLMan.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.servers = SQL.Func.GetActualSQLServers();

            return View();
        }
        [HttpPost]
        public ActionResult Connect(ConnectionParams cnParams)
        {
           
            SqlConnection cn = SQL.Func.ConnectToSQLserver(cnParams.ServerName);
            List<string> CurTables = SQL.Func.GetTablesOnDataBase(cn, cnParams.DataBase);
            return View();
        }

    }
}
