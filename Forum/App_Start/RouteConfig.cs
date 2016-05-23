﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Forum
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Forum",
                url: "Forum/{id}",
                defaults: new { controller = "Forum", action = "Wyswietl", id = UrlParameter.Optional },
                constraints: new {productId = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Forum", action = "Lista", id = UrlParameter.Optional }
            );
        }
    }
}
