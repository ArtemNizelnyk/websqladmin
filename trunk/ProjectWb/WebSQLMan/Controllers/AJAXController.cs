using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSQLMan.Controllers
{
    public class AJAXController : Controller
    {
        //
        // GET: /AJAX/
        [HttpGet]
        public string Index(string server)
        {
            List<string> databases = SQL.Func.GetDatabasesOnServer(server);
            string html = "";

            foreach (string item in databases)
            {
                html += "<option>";
                html += item.ToString();
                html += "</option>";
            }
            return html;
        }

    }
}
