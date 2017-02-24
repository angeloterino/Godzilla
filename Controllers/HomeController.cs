using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanApp.Helpers;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private StrawmanApp.Models.DataClasses1DataContext db;
        
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ViewBag.MenuUrl = ViewPaths.MENU_URL;
            ViewBag.TabUrl = ViewPaths.CONTROLLER_NAME + "/StrawmanApp";
            //return View("~/Views/Account/Login.cshtml");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult StrawmanApp()
        {
            ViewBag.Message = "Your StrawmanApp page.";
            Helpers.Session.SetSession("MenuUrl","StrawmanApp");
            ViewBag.MenuUrl = ViewPaths.MENU_URL;
            ViewBag.TabUrl = ViewPaths.CONTROLLER_NAME + "/StrawmanApp";

            //Set page labels
            ViewBag.MarketTitle = TableTitles.MARKET_TITLE;
            ViewBag.BrandTitle = TableTitles.BRAND_TITLE;
            ViewBag.DataTitle = TableTitles.DATA_TITLE;
            ViewBag.SourceTitle = TableTitles.SOURCE_TITLE;
            ViewBag.JJBrandTitle = TableTitles.JJ_BRAND_TITLE;
            ViewBag.PCVSPYTitle = TableTitles.PCVSPY_TITLE;
            ViewBag.PCVSPYTitleMarket = TableTitles.MARKET_TITLE + " " +TableTitles.PCVSPY_TITLE;
            ViewBag.PCVSPYTitleBrand = TableTitles.BRAND_TITLE + " " + TableTitles.PCVSPY_TITLE;
            //Month
            ViewBag.MonthTitleMarket = TableTitles.GENERIC_TITLE.Replace("{0}",TableTitles.MARKET_TITLE);
            ViewBag.MonthTitleBrand = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.BRAND_TITLE);
            ViewBag.MonthLastTitle = TableTitles._ESPACE + TableTitles.MONTH_TITLE;
            ViewBag.MonthCurrentTitle = TableTitles._ESPACE + TableTitles.MONTH_TITLE;
            //YTD
            ViewBag.YTDTitleMarket = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.MARKET_TITLE);
            ViewBag.YTDTitleBrand = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.BRAND_TITLE);
            ViewBag.YTDLastTitle = TableTitles._ESPACE + TableTitles.YTD_TITLE;
            ViewBag.YTDCurrentTitle = TableTitles._ESPACE + TableTitles.YTD_TITLE;
            //MAT
            ViewBag.MATTitleMarket = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.MARKET_TITLE);
            ViewBag.MATTitleBrand = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.BRAND_TITLE);
            ViewBag.MATLastTitle = TableTitles._ESPACE + TableTitles.MAT_TITLE;
            ViewBag.MATCurrentTitle = TableTitles._ESPACE + TableTitles.MAT_TITLE;
            //BTG
            ViewBag.BTGTitleMarket = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.MARKET_TITLE);
            ViewBag.BTGTitleBrand = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.BRAND_TITLE);
            ViewBag.BTGLastTitle = TableTitles._ESPACE + TableTitles.BTG_TITLE;
            ViewBag.BTGCurrentTitle = TableTitles._ESPACE + TableTitles.BTG_TITLE;
            //Total
            ViewBag.TotalTitleMarket = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.MARKET_TITLE);
            ViewBag.TotalTitleBrand = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.BRAND_TITLE);
            ViewBag.TotalThreeTitle = "";
            ViewBag.TotalTwoTitle = "";
            ViewBag.TotalOneTitle = "";
            //BOY
            ViewBag.BOYTitleMarket = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.MARKET_TITLE);
            ViewBag.BOYTitleBrand = TableTitles.GENERIC_TITLE.Replace("{0}", TableTitles.BRAND_TITLE);
            ViewBag.BOYINTTitle = TableTitles._ESPACE + TableTitles.INT_TITLE;
            ViewBag.BOYLETitle = TableTitles._ESPACE + TableTitles.LE_TITLE;
            ViewBag.BOYPBPTitle = TableTitles._ESPACE + TableTitles.PBP_TITLE;
            //NTS
            ViewBag.NTSTitle = TableTitles.NTS_TITLE;


            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month,1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            
            return View();
        }
        public ActionResult BoyMassMarket()
        {
            ViewBag.Message = "Your BoyMassMarket page.";
            ViewBag.MenuUrl = ViewPaths.MENU_URL;
            ViewBag.TabUrl = ViewPaths.CONTROLLER_NAME + "/BoyMassMarket";
            //StrawmanApp.Models.BOYDataClassesDataContext dbb = new Models.BOYDataClassesDataContext();
            //dbb.CommandTimeout = 50000;
            //var variables = from p in dbb.v_WRK_BOY_BASIC_DATA
            //                where p.TYPE == "YTD"
            //                select new Models.BOYVariablesModels { brand = (int)p.BRAND, market = (int)p.MARKET, rate = (p.CONVERSION_RATE1 != null), sellin = (p.SELLIN_COL1 != null), sellout = (p.SELLOUT_COL1 != null), share = (p.SHARE_COL1 != null) };
                                            
            //return View(variables.ToList());
            ViewBag.Title = "BoyMassMarket";
            ViewBag.Channel = StrawmanConstants.CHANNEL_MASS;
            ViewBag.BoyMasterData = GetMasterData(StrawmanConstants.CHANNEL_MASS);
            return View(ViewPaths.BOYHOME_PATH);
        }
        public ActionResult BoyBeauty()
        {
            ViewBag.MenuUrl = ViewPaths.MENU_URL;
            ViewBag.TabUrl = ViewPaths.CONTROLLER_NAME + "/BoyBeauty";
            ViewBag.Message = "Your BoyMassMarket page.";
            ViewBag.Title = "BoyBeauty";
            ViewBag.Channel = StrawmanConstants.CHANNEL_BEAUTY;
            ViewBag.BoyMasterData = GetMasterData(StrawmanConstants.CHANNEL_BEAUTY);

            return View(ViewPaths.BOYHOME_PATH);
        }
        public ActionResult BoyOTC()
        {
            ViewBag.MenuUrl = ViewPaths.MENU_URL;
            ViewBag.TabUrl = ViewPaths.CONTROLLER_NAME + "/BoyOTC";
            ViewBag.Message = "Your BoyMassMarket page.";
            ViewBag.Title = "BoyOTC";
            ViewBag.Channel = StrawmanConstants.CHANNEL_OTC;
            ViewBag.BoyMasterData = GetMasterData(StrawmanConstants.CHANNEL_OTC);

            return View(ViewPaths.BOYHOME_PATH);
        }

        private List<Models.BoyMassMarketModels> GetMasterData(string channel)
        {
            Controllers.BoyMassMarketController cont = new Controllers.BoyMassMarketController();
            List<Models.BoyMassMarketModels> list = cont.GetMasterData(channel);
            cont.Dispose();
            return list;
        }

        public ActionResult AsyncView()
        {
            ViewBag.Message = "Your Async page.";

            return View();
        }
        
        public ActionResult MarketView()
        {
            return RedirectToAction("StrawmanApp");
            //db = new Models.DataClasses1DataContext();
            //db.CommandTimeout = 500000;
            //ViewBag.Message = "Your MarketView page.";
            //ViewBag.MarketData = GetMarketData().AsEnumerable();
            //ViewBag.MarketYTD = GetMarketYTD();
            //ViewBag.MarketBTG = GetMarketBTG();
            //ViewBag.MarketTotalCustom = GetMarketTotalCustom();
            //ViewBag.MarketGroupTotal = GetMarketGroupTotal();
            //ViewBag.MarketMonth = GetMarketMonth();
            //ViewBag.MarketBOY = GetMarketBOY();
            //ViewBag.MarketMAT = GetMarketMAT();
            //ViewBag.MarketPCVSPY = GetMarketPCVSPY();
            //db.Dispose();
            //return View();
        }
        public ActionResult BrandView()
        {
            using (db = new Models.DataClasses1DataContext())
            {
                db.CommandTimeout = 500000;
                ViewBag.Message = "Your MarketView page.";
                ViewBag.BrandData = GetBrandData();
                ViewBag.BrandYTD = GetBrandYTD();
                ViewBag.BrandBTG = GetBrandBTG();
                ViewBag.BrandTotalCustom = GetBrandTotalCustom();
                ViewBag.BrandGroupTotal = GetBrandGroupTotal();
                ViewBag.BrandMonth = GetBrandMonth();
                ViewBag.BrandBOY = GetBrandBOY();
                ViewBag.BrandMAT = GetBrandMAT();
                ViewBag.BrandPCVSPY = GetBrandPCVSPY();
            }
            return View();
        }

        private dynamic GetBrandPCVSPY()
        {
            var query = from p in db.v_WRK_BRAND_PCVSPY
                        select new Models.Market_PCVSPYModels { market = p.MARKET, brand = p.BRAND, vgroup = p.GROUP, vorder = p.ORDER, pcvspycol1 = p.PCVSPY_COL1, pcvspycol2 = p.PCVSPY_COL2, pcvspycol3 = (double?)p.PCVSPY_COL3, pcvspycol4 = (double?)p.PCVSPY_COL4, pcvspycol5 = (double?)p.PCVSPY_COL5 };
            return query.ToList(); 
        }

        private dynamic GetBrandMAT()
        {
            var query = from p in db.v_WRK_BRAND_MAT
                        select new Models.Market_MATModels { market = p.MARKET, brand = p.BRAND, mat_col1 = (double?)(p.MAT_COL1), mat_col2 = (double?)(p.MAT_COL2), pc_vs_py = p.PCVSPY, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetBrandBOY()
        {
            var query = from p in db.v_WRK_BRAND_BOY
                        select new Models.Market_BOYModels { market = p.MARKET, brand = p.BRAND, _internal = p.INTERNAL, _le = p.LE, _pbp = p.PBP, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetBrandMonth()
        {
            var query = from p in db.v_WRK_BRAND_MONTHs
                        select new Models.Market_MonthModels { market = p.MARKET, brand = p.BRAND, month_2014 = p.MONTH_COL1, month_2015 = p.MONTH_COL2, pc_vs_py = p.PCVSPY, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetBrandGroupTotal()
        {
            return null; 
        }

        private dynamic GetBrandTotalCustom()
        {
            var query = from p in db.v_WRK_BRAND_TOTAL_CUSTOM
                        select new Models.Market_TotalCustomModels { market = p.MARKET, brand = p.BRAND, vgroup = p.GROUP, v2012 = p.COL1, v2013 = p.COL2, v2014 = p.COL3, v2015 = p.COL4, vorder = p.ORDER };
            return query.ToList(); 
        }

        private dynamic GetBrandBTG()
        {
            var query = from p in db.v_WRK_BRAND_BTGs
                        select new Models.Market_BTGModels { market = p.MARKET, brand = p.BRAND, vgroup = p.GROUP, btg_col1 = (double?)p.COL1, btg_col2 = (double?)p.COL2, pc_vs_py = p.COL3, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetBrandYTD()
        {
            var query = from p in db.v_WRK_BRAND_YTD
                        select new Models.Market_YTDModels { market = p.MARKET, brand = p.BRAND, ytd_2013 = p.YTD_COL1, ytd_2014 = p.YTD_COL2, pc_vs_py = p.PCVSPY, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetBrandData()
        {
            return null;      
        }

        private dynamic GetMarketPCVSPY()
        {
            var query = from p in db.v_WRK_MARKET_PCVSPY
                        select new Models.Market_PCVSPYModels { market = p.MARKET, brand = p.BRAND, vgroup = p.GROUP, vorder = p.ORDER, pcvspycol1 = p.PCVSPY_COL1, pcvspycol2 = p.PCVSPY_COL2, pcvspycol3 = (double?)p.PCVSPY_COL3, pcvspycol4 = (double?)p.PCVSPY_COL4, pcvspycol5 = (double?)p.PCVSPY_COL5 };
            return query.ToList(); 
        }

        private dynamic GetMarketMAT()
        {
            var query = from p in db.v_WRK_MARKET_MAT
                        select new Models.Market_MATModels { market = p.MARKET, brand = p.BRAND, mat_col1 = double.Parse(p.MAT_COL1.ToString()), mat_col2 = double.Parse(p.MAT_COL2.ToString()), pc_vs_py = p.PCVSPY, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetMarketBOY()
        {
            var query = from p in db.v_WRK_MARKET_BOY
                        select new Models.Market_BOYModels { market = p.MARKET, brand = p.BRAND, _internal = p.INTERNAL, _le = p.LE, _pbp= p.PBP, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetMarketMonth()
        {
            var query = from p in db.v_WRK_MARKET_MONTH
                        select new Models.Market_MonthModels { market = p.MARKET, brand = p.BRAND, month_2014 = p.MONTH_COL1, month_2015 = p.MONTH_COL2, pc_vs_py = p.PCVSPY, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetMarketTotalCustom()
        {
            var query = from p in db.v_WRK_MARKET_TOTAL_CUSTOM
                        select new Models.Market_TotalCustomModels { market = p.MARKET, brand = p.BRAND, vgroup = p.GROUP, v2012 = p.THREE_AGO, v2013 = p.TWO_AGO, v2014 = p.LAST, v2015=p.CURRENT, vorder = p.ORDER };
            return query.ToList(); 
        }

        private dynamic GetMarketBTG()
        {
            var query = from p in db.v_WRK_MARKET_BTG
                        select new Models.Market_BTGModels { market = p.MARKET, brand = p.BRAND, vgroup = p.GROUP, btg_col1 = double.Parse(p.BTG_COL1.ToString()), btg_col2 = p.BTG_COL2, pc_vs_py = p.PCVSPY, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }

        private dynamic GetMarketGroupTotal()
        {            
            var query = from p in db.v_MARKET_GROUPS_TOTAL
                        select new Models.MarketTotalModels { market = p.MARKET, brand = p.BRAND, vgroup =p.GROUP, ytd = p.YTD, year = p.YEAR, brand_name = p.BRAND_NAME, group_name = p.GROUP_NAME, market_name = p.MARKET_NAME};
            return query.ToList(); 
        }

        private dynamic GetMarketYTD()
        {            
            var query = from p in db.v_WRK_MARKET_YTD
                        select new Models.Market_YTDModels { market = p.MARKET, brand = p.BRAND, ytd_2013 = p.YTD_COL1, ytd_2014 = p.YTD_COL2, pc_vs_py = p.PCVSPY, vgroup = p.GROUP, vorder = p.GROUP_ORDER };
            return query.ToList(); 
        }
        public List<StrawmanApp.Models.MarketDataModels> GetMarketData()
        {            
            return null;            
        }

        #region Constants
        private partial class ViewPaths
        {
            public static string MENU_URL = "StrawmanApp";
            public static string CONTROLLER_NAME = "Home";
            public static string VIEWS_PATH = "~/Views";
            public static string BOY_PATH = VIEWS_PATH + "/BoyMassMarket";
            public static string BOYHOME_PATH = BOY_PATH + "/Index.cshtml";
        }
        private partial class TableTitles
        {
            public static string MARKET_TITLE = "Market";
            public static string BRAND_TITLE = "Brand";
            public static string DATA_TITLE = "Data";
            public static string SOURCE_TITLE = "Source";
            public static string JJ_BRAND_TITLE = "J&J Brand";
            public static string PCVSPY_TITLE = "% Vs PY";
            public static string GENERIC_TITLE = "52 w {0} Size (€)";
            public static string _ESPACE = " ";
            public static string MONTH_TITLE = "Month";
            public static string YTD_TITLE = "YTD";
            public static string MAT_TITLE = "MAT";
            public static string BTG_TITLE = "BTG";
            public static string INT_TITLE = "INTERNAL";
            public static string LE_TITLE = "LE";
            public static string PBP_TITLE = "PBP";
            public static string NTS_TITLE = "J & J NTS";
        }
        #endregion
    }
}
