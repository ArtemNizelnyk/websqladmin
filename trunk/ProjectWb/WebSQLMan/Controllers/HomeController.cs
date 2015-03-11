using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using Ext.Net;

namespace WebSQLMan.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Message = "Type your query here";

            return View();

        }


        public Ext.Net.MVC.PartialViewResult Run(string query, string containerId)
        {
            DataTable dt = new DataTable();

            DataSet ds = SQL.Func.Input(query);

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
    }
}
