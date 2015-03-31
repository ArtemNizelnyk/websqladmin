using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebSQLMan.Models;
using Ext.Net.MVC;
using Ext.Net;

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
        public ActionResult Connect(ConnectionParams cnParams)
        {



            SQL.Func.ConnectToSQLserver(cnParams.ServerName);
            return RedirectToAction("Index", "Main", cnParams);





        }


    }
}
