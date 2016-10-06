using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class BrandViewKeybrandsController : Controller
    {
        private Models.GodzillaWRKDataContext db;

        List<Models.WRK_KEYBRANDS_BASE> tbase;

        private static string _path = "~/Views/BrandViewKeybrands/";
        private string dataview = _path + "_BrandDataView.cshtml";
        private string matview = _path + "_Brand_MAT.cshtml";
        private string monthview = _path + "_Brand_Month.cshtml";
        private string ytdview = _path + "_Brand_YTD.cshtml";
        private string btgview = _path + "_Brand_BTG.cshtml";
        private string totalcustomview = _path + "_Brand_TotalCustom.cshtml";
        private string boyview = _path + "_Brand_BOY.cshtml";
        private string pcvspyview = _path + "_Brand_PCVSPY.cshtml";

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetPCVSPY() {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                db.CommandTimeout = 500000;
                ViewBag.BrandViewKeybrandsPCVSPY = GetPCVSPYData();
            }
            return PartialView(pcvspyview);
        }

        private dynamic GetPCVSPYData()
        {
            var query = from p in db.WRK_KEYBRANDS_PCVSPies
                        where p.DATA == "BRAND" && p.TYPE == "SELL OUT" && p.ID != 999
                        where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { pcvspy1 = (decimal?)p.COL1, pcvspy2 = (decimal?)p.COL2, pcvspy3 = (decimal?)p.COL3, pcvspy4 = (decimal?)p.COL4, pcvspy5 = (decimal?)p.COL5, vid = (decimal)p.ID }
                        ;
            return query.OrderBy(m=>m.vid).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY() {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                db.CommandTimeout = 500000;
                ViewBag.BrandViewKeybrandsBOY = GetBOYData();
            }
            return PartialView(boyview);
        }

        private dynamic GetBOYData()
        {
            var query = from p in db.WRK_KEYBRANDS_BOYs
                        where p.DATA == "BRAND" && p.TYPE == "SELL OUT" && p.ID != 999
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { _internal = (decimal?)p.COL1, _le = (decimal?)p.COL2, _pbp = (decimal?)p.COL3, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom() {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                ViewBag.BrandViewKeybrandsTotalCustom = GetTotalCustomData();
            }
            return PartialView(totalcustomview);
        }

        private dynamic GetTotalCustomData()
        {
            tbase = GetSessionBaseData();
            var query = from p in tbase
                        where p.DATA == "BRAND" && p.TYPE == "TOTAL"
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
                ViewBag.BrandViewKeybrandsBTG = GetBTGData();
            }
            return PartialView(btgview);
        }

        private dynamic GetBTGData()
        {

            var query = from p in db.WRK_KEYBRANDS_BTGs
                        where p.DATA == "BRAND" && p.TYPE == "SELL OUT" && p.ID != 999
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
                ViewBag.BrandViewKeybrandsYTD = GetYTDData();
            }
            return PartialView(ytdview);
        }

        private dynamic GetYTDData()
        {
            tbase = GetSessionBaseData();
            var query = from p in tbase
                        where p.DATA == "BRAND" &&  p.TYPE == "YTD"
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
                ViewBag.BrandViewKeybrandsMonth = GetMonthData();
            }
            return PartialView(monthview);
        }

        private dynamic GetMonthData()
        {
            tbase = GetSessionBaseData();
            var query = from p in tbase
                        where p.DATA == "BRAND" && p.TYPE == "MONTH"
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
                ViewBag.BrandViewKeybrandsMAT = GetMATData();
            }
            return PartialView(matview);
        }

        private dynamic GetMATData()
        {
            tbase = GetSessionBaseData();
            var query = from p in tbase
                        where p.DATA == "BRAND" && p.TYPE == "MAT"
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.StrawmanViewSTDModel { col1 = p.COL1, col2 = p.COL2, pcvspy = p.PCVSPY, vid = (decimal)p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        public ActionResult GetDataView()
        {
            using (
            db = new Models.GodzillaWRKDataContext())
            {
                ViewBag.BrandViewKeybrandsData = GetDataViewData();
            }
            return PartialView(dataview);
        }

        private dynamic GetDataViewData()
        {
            Helpers.Session.SetSession("v_WRK_KEYBRANDS_BASE", db.WRK_KEYBRANDS_BASEs.Where(p=> p.ID != 999).Select(m => m).ToList());
            tbase = GetSessionBaseData();
            var query = from p in tbase
                        where p.DATA == "BRAND" && p.TYPE == "MAT" 
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.MarketViewChannelModels { name = p.NAME, vid = (decimal) p.ID };
            return query.OrderBy(m=>m.vid).ToList(); 
        }

        //
        // GET: /BrandViewKeybrands/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /BrandViewKeybrands/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /BrandViewKeybrands/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BrandViewKeybrands/Create

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
        // GET: /BrandViewKeybrands/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /BrandViewKeybrands/Edit/5

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
        // GET: /BrandViewKeybrands/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /BrandViewKeybrands/Delete/5

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

        #region Custom Functions
        private List<Models.WRK_KEYBRANDS_BASE> GetSessionBaseData()
        {
            return Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") != null ? (List<Models.WRK_KEYBRANDS_BASE>)Helpers.Session.GetSession("v_WRK_KEYBRANDS_BASE") : db.WRK_KEYBRANDS_BASEs.Where(p=> p.ID != 999).Select(m => m).ToList();
        }
        #endregion

    }
}
