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


            try
            {
                SQL.Func.ConnectToSQLserver(cnParams.ServerName, IntegratedSecurity: (cnParams.Authentification == ConnectionParams.Auth.WindowsAuthentication) ? true : false,
                    login: cnParams.Login, pass: cnParams.Password);
            }
            catch (SqlException ex)
            {
                string Errors = "";
                foreach (SqlError sqlError in ex.Errors)
                    Errors += sqlError.Message + "\n";
                HttpContext.Cache["Errors"] = Errors;
                return RedirectToAction("Index", "Login", cnParams);
            }
            return RedirectToAction("Index", "Main", cnParams);





        }


    }
}
