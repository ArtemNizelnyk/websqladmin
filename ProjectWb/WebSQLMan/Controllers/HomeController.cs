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


            return View();

        }

        public Ext.Net.MVC.PartialViewResult Test(string containerId)
        {


            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            DataTable dt = new DataTable("MyTable");
            dt.Columns.Add(new DataColumn("Col1", typeof(string)));
            dt.Columns.Add(new DataColumn("Col2", typeof(string)));
            dt.Columns.Add(new DataColumn("Col3", typeof(string)));

            for (int i = 0; i < 3; i++)
            {
                DataRow row = dt.NewRow();
                row["Col1"] = "col 1, row " + i;
                row["Col2"] = "col 2, row " + i;
                row["Col3"] = "col 3, row " + i;
                dt.Rows.Add(row);
            }

            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "Test",
                ContainerId = containerId,
                Model = dt, //passing the DataTable as my Model
                ClearContainer = true,
                RenderMode = RenderMode.AddTo
            };
        }
    }
}
