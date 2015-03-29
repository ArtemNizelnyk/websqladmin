using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebSQLMan.Models;

namespace WebSQLMan.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.servers = SQL.Func.GetActualSQLServers();

            return View();
        }

        public RedirectToRouteResult Dissconnect()
        {
            return RedirectToAction("Index", "Login");
        }

    }
}
