using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class MarketViewKeybrandsController : Controller
    {
        private Models.GodzillaWRKDataContext db;
        private List<Models.WRK_KEYBRANDS_BASE> tbase;

        private static string _path = "~/Views/MarketViewKeybrands/";
        private string dataview = _path + "_MarketDataView.cshtml";
        private string matview = _path + "_Market_MAT.cshtml";
        private string monthview = _path + "_Market_Month.cshtml";
        private string ytdview = _path + "_Market_YTD.cshtml";
        private string btgview = _path + "_Market_BTG.cshtml";
        private string totalcustomview = _path + "_Market_TotalCustom.cshtml";
        private string boyview = _path + "_Market_BOY.cshtml";
        private string pcvspyview = _path + "_Market_PCVSPY.cshtml";

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetPCVSPY() {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                db.CommandTimeout = 500000;
                ViewBag.MarketViewKeybrandsPCVSPY = GetPCVSPYData();
            }
            return PartialView(pcvspyview);
        }

        private dynamic GetPCVSPYData()
        {
            var query = from p in db.WRK_KEYBRANDS_PCVSPies
                        where p.DATA == "MARKET" && p.TYPE == "MARKET" && p.ID != 999
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { pcvspy1 = (decimal?)p.COL1, pcvspy2 = (decimal?)p.COL2, pcvspy3 = (decimal?)p.COL3, pcvspy4 = (decimal?)p.COL4, pcvspy5 = (decimal?)p.COL5, vid = (decimal)p.ID }
                        ;
            return query.OrderBy(m=>m.vid).ToList();
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY() {
            using (db = new Models.GodzillaWRKDataContext())
            {
                db.CommandTimeout = 500000;
                ViewBag.MarketViewKeybrandsBOY = GetBOYData();             
            }
            return PartialView(boyview);
        }

        private dynamic GetBOYData()
        {
            var query = from p in db.WRK_KEYBRANDS_BOYs
                        where p.DATA == "MARKET" && p.TYPE == "MARKET" && p.ID != 999
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { _internal = (decimal?)p.COL1, _le = (decimal?)p.COL2, _pbp = (decimal?)p.COL3, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList();
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom() {
            using (db = new Models.GodzillaWRKDataContext())
            {
                db.CommandTimeout = 50000;
                ViewBag.MarketViewKeybrandsTotalCustom = GetTotalCustomData();               
            }
            return PartialView(totalcustomview);
        }

        private dynamic GetTotalCustomData()
        {
            tbase = Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") != null ? (List<Models.WRK_KEYBRANDS_BASE>)Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") : db.WRK_KEYBRANDS_BASEs.Select(m => m).ToList();
            var query = from p in tbase
                        where p.DATA == "MARKET" && p.TYPE == "TOTAL"
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { col1 = p.COL1, col2 = p.COL2, col3 = p.COL3, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBTG() {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                db.CommandTimeout = 500000;
                ViewBag.MarketViewKeybrandsBTG = GetBTGData();
            }
            return PartialView(btgview);
        }

        private dynamic GetBTGData()
        {

            var query = from p in db.WRK_KEYBRANDS_BTGs
                        where p.DATA == "MARKET" && p.TYPE == "MARKET" && p.ID != 999
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { col1 = (decimal?)p.COL1, col2 = (decimal?)p.COL2, pcvspy = (decimal?)p.PCVSPY, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetYTD()
        {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                ViewBag.MarketViewKeybrandsYTD = GetYTDData();
            }
            return PartialView(ytdview);
        }
        
        private dynamic GetYTDData()
        {
            tbase = Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") != null ? (List<Models.WRK_KEYBRANDS_BASE>)Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") : db.WRK_KEYBRANDS_BASEs.Select(m => m).ToList();
            var query = from p in tbase
                        where p.DATA == "MARKET" &&  p.TYPE == "YTD"
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { col1 = p.COL1, col2 = p.COL2, pcvspy = p.PCVSPY, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMonth()
        {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                ViewBag.MarketViewKeybrandsMonth = GetMonthData();
                db.CommandTimeout = 50000;
            }
            return PartialView(monthview);
        }

        private dynamic GetMonthData()
        {
            tbase = Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") != null ? (List<Models.WRK_KEYBRANDS_BASE>)Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") : db.WRK_KEYBRANDS_BASEs.Select(m => m).ToList();
            var query = from p in tbase
                        where p.DATA == "MARKET" && p.TYPE == "MONTH"
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { col1 = p.COL1, col2 = p.COL2, pcvspy = p.PCVSPY, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMAT()
        {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                ViewBag.MarketViewKeybrandsMAT = GetMATData();
            }
            return PartialView(matview);
        }

        private dynamic GetMATData()
        {
            tbase = Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") != null ? (List<Models.WRK_KEYBRANDS_BASE>)Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") : db.WRK_KEYBRANDS_BASEs.Select(m => m).ToList();
            var query = from p in tbase
                        where p.DATA == "MARKET" && p.TYPE == "MAT"
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { col1 = p.COL1, col2 = p.COL2, pcvspy = p.PCVSPY, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        public ActionResult GetDataView()
        {
            using (db = new Models.GodzillaWRKDataContext())
            {
                db.CommandTimeout = 500000;
                ViewBag.MarketViewKeybrandsData = GetDataViewData();
            }
            return PartialView(dataview);
        }

        private dynamic GetDataViewData()
        {            
            Helpers.Session.SetSession("v_WRK_KEYBRANDS_BASE", db.WRK_KEYBRANDS_BASEs.Where(p=>p.ID != 999).Select(m => m).ToList());
            tbase = Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") != null ? (List<Models.WRK_KEYBRANDS_BASE>) Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") : db.WRK_KEYBRANDS_BASEs.Select(m=>m).ToList();
            var query = from p in tbase
                        where p.DATA == "MARKET" && p.TYPE == "MAT" 
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.MarketViewChannelModels { name = p.NAME, vid = (decimal) p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        //
        // GET: /MarketViewKeybrands/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MarketViewKeybrands/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MarketViewKeybrands/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MarketViewKeybrands/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MarketViewKeybrands/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MarketViewKeybrands/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MarketViewKeybrands/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MarketViewKeybrands/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
    }
}
