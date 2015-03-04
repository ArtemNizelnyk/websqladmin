using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlAdmin.Models;
using System.Data.SqlClient;

namespace SqlAdmin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.servers = SQL.Func.GetActualSQLServers();

            return View();
        }
        [HttpPost]
        public ActionResult Connect( ConnectionParams cnParams )
        {
            SqlConnection cn = SQL.Func.ConnectToSQLserver(cnParams.ServerName);
            List<string> CurTables = SQL.Func.GetTablesOnDataBase(cn, cnParams.DataBase);
            return View();
        }

    }
}
