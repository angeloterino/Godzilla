using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace StrawmanApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }

        protected void Session_Start() {
            Helpers.PeriodUtil.Year = DateTime.Now.Year;
            Helpers.PeriodUtil.Month = DateTime.Now.Month;
            using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            {
                var q = db.PERIOD_TABLE.OrderBy(m =>new { m.YEAR_PERIOD, m.MONTH_PERIOD}).ToList().Last();
                if (q != null)
                {
                    Helpers.PeriodUtil.Year = (int) q.YEAR_PERIOD;
                    Helpers.PeriodUtil.Month =(int) q.MONTH_PERIOD;
                }
            }
        }

        protected void Application_PreRequestHandlerExecute()
        {
            if (Request.QueryString.Count > 0) 
            {
                if (Request.QueryString["menu_id"] != null) Helpers.Session.SetSession("menu_id", Request.QueryString["menu_id"]);
                if (Request.QueryString["cache"] != null) Helpers.Session.CacheStatus = bool.Parse(Request.QueryString["cache"] ?? "true");
            }
        }
    }
}