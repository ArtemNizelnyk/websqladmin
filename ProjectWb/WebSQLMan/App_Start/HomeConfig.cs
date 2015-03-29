using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebSQLMan.App_Start
{
   
    public class HomeConfig
    {
         [assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebSQLMan.App_Start.HomeConfig), "Start")]
        

        public static void RegisterRoutes(RouteCollection routes)
{
    // Указываем, что все ресурсы ext.net нужно игнорировать
    routes.IgnoreRoute("{extnet-root}/{extnet-file}/ext.axd");

    // настраиваем маршрут для контроллера
    routes.MapRoute(
        "Main", // Имя маршрута
        "{controller}/{action}/{id}", // Как будет выглядеть URL
        new { controller = "Main", action = "Index", id = UrlParameter.Optional } //Что вызывать по умолчанию
    );
}
        public static void Start()
{
  HomeConfig.RegisterRoutes(RouteTable.Routes);
}
    }
}