using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSQLMan.Models
{
    public class SiteMapModel
    {
        //dynamic node creation
        public static Node CreateNodeWithOutChildren(SiteMapNode siteMapNode)
        {
            Node treeNode;

            if (siteMapNode.ChildNodes != null && siteMapNode.ChildNodes.Count > 0)
            {
                treeNode = new Node();
            }
            else
            {
                treeNode = new Node();
                treeNode.Leaf = true;
            }

            if (!string.IsNullOrEmpty(siteMapNode.Url))
            {
                treeNode.Href = siteMapNode.Url.StartsWith("~/") ? siteMapNode.Url.Replace("~/", "http://examples.ext.net/") : ("http://examples.ext.net" + siteMapNode.Url);
            }

            treeNode.NodeID = siteMapNode.Key;
            treeNode.Text = siteMapNode.Title;
            treeNode.Qtip = siteMapNode.Description;

            return treeNode;
        }

    }
}