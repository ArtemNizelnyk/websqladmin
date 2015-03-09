using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSQLMan.Models;
using Ext.Net;

namespace WebSQLMan.Controllers
{
    public class SiteMapController : Controller
    {
        public ActionResult Index()
        {
            return View(SiteMapModel.CreateNodeWithOutChildren(SiteMap.RootNode));
        }

        public ActionResult LoadPages(string node)
        {
            NodeCollection result = null;
            if (node == "_root")
            {
                result = SiteMapModel.CreateNodeWithOutChildren(SiteMap.RootNode).Children;
            }
            else
            {
                SiteMapNode siteMapNode = SiteMap.Provider.FindSiteMapNodeFromKey(node);
                SiteMapNodeCollection children = siteMapNode.ChildNodes;
                result = new NodeCollection();

                if (children != null && children.Count > 0)
                {
                    foreach (SiteMapNode mapNode in siteMapNode.ChildNodes)
                    {
                        result.Add(SiteMapModel.CreateNodeWithOutChildren(mapNode));
                    }
                }
            }

            return this.Json(result);
        }
    }
}
    