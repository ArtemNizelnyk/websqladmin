using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebSQLMan.Models;

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
