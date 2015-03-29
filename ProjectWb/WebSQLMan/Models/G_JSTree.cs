using System;

namespace WebSQLMan.Models
{
    public class G_JSTree
    {
        public G_JsTreeAttribute attr;
        public string text
        {
            get;
            set;
        }
        public Boolean children
        { 
            get;
            set; 
        }
        public object data
        {
            get;
            set;
        }
        public int IdServerUse
        {
            get;
            set;
        }
        public string icons
        {
            get;
            set;
        }
        public string state
        {
            get;
            set;
        }
    }

    public class G_JsTreeAttribute
    {
        public string id;
        public bool selected;
    }
}