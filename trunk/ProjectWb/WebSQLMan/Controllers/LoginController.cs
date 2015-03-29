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

            return View();
        }
        [HttpPost]
        public RedirectToRouteResult Connect(ConnectionParams cnParams)
        {
           
            SqlConnection cn = SQL.Func.ConnectToSQLserver(cnParams.ServerName);
            return RedirectToAction("Index", "Main", cnParams);
        }

        
    }
}
