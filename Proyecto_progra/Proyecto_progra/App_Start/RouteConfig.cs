﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Proyecto_progra
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
<<<<<<< Updated upstream
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
=======
                defaults: new { controller = "Main", action = "MainPage", id = UrlParameter.Optional }
>>>>>>> Stashed changes
            );
        }
    }
}
