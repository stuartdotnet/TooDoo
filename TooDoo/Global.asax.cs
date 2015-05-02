using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using TooDoo.Repository;

namespace TooDoo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LocatorInitializationHandler.Initialize();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public static class LocatorInitializationHandler
    {
        public static void Initialize()
        {
            Database.SetInitializer(new CreateInitializer()); 

            using (var db = new ToDoContext())
            {
                {
                    db.Database.Initialize(true);
                }
            }
        }
    }
}
