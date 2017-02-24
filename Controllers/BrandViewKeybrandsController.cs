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
            ViewBag.BrandViewKeybrandsPCVSPY = GetPCVSPYData();
            return PartialView(pcvspyview);
        }

        private dynamic GetPCVSPYData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA> bdata = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY, true);
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_TOTAL_DATA> tdata = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_TOTAL_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_TOTAL, true);
            List<Models.StrawmanViewSTDModel> tbase =
                bdata.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000)
                    .AsEnumerable()
                    .Join(tdata.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                            , b => new { _channel = b.CHANNEL, _market = b.MARKET, _brand = b.BRAND }
                            , t => new { _channel = t.CHANNEL, _market = t.MARKET, _brand = t.BRAND }
                            , (b, t) => new { b = b, t = t })
                    .AsEnumerable()
                    .Select(m => new Models.StrawmanViewSTDModel
                    {
                        channel = m.b.CHANNEL,
                        market = m.b.MARKET,
                        brand = m.b.BRAND,
                        col1 = m.t.THREE_AGO,
                        col2 = m.t.TWO_AGO,
                        col3 = m.t.LAST,
                        col4 = (decimal?)m.b.INTERNAL,
                        col5 = (decimal?)m.b.LE,
                        col6 = (decimal?)m.b.PBP,
                    }).ToList();
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return new MarketViewKeybrandsController().GetGroupedData(tbase, mst, Classes.StrawmanViews.PCVSPY)
                .Union(new MarketViewKeybrandsController().GetGroupedByConfig(tbase, KEYBRANDS_VIEW, Classes.StrawmanViews.PCVSPY)).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY() {
            ViewBag.BrandViewKeybrandsBOY = GetBOYData();
            return PartialView(boyview);
        }

        private dynamic GetBOYData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();
            foreach (Models.StrawmanViewSTDModel item in tbase)
            {
                item.col1 = item._internal;
                item.col2 = item._le;
                item.col3 = item._pbp;
            }

            return new MarketViewKeybrandsController().GetGroupedData(tbase, mst, Classes.StrawmanViews.BOY)
                .Union(new MarketViewKeybrandsController().GetGroupedByConfig(tbase, KEYBRANDS_VIEW, Classes.StrawmanViews.BOY)).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom() {
            ViewBag.BrandViewKeybrandsTotalCustom = GetTotalCustomData();
            return PartialView(totalcustomview);
        }

        private dynamic GetTotalCustomData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_TOTAL);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return new MarketViewKeybrandsController().GetGroupedData(tbase, mst, Classes.StrawmanViews.TOTAL)
                .Union(new MarketViewKeybrandsController().GetGroupedByConfig(tbase, KEYBRANDS_VIEW, Classes.StrawmanViews.TOTAL)).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBTG() {
            ViewBag.BrandViewKeybrandsBTG = GetBTGData();
            return PartialView(btgview);
        }

        private dynamic GetBTGData()
        {

            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BTG);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return new MarketViewKeybrandsController().GetGroupedData(tbase, mst, Classes.StrawmanViews.BTG)
                .Union(new MarketViewKeybrandsController().GetGroupedByConfig(tbase, KEYBRANDS_VIEW, Classes.StrawmanViews.BTG)).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetYTD()
        {
            ViewBag.BrandViewKeybrandsYTD = GetYTDData();
            return PartialView(ytdview);
        }

        private dynamic GetYTDData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_YTD);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return new MarketViewKeybrandsController().GetGroupedData(tbase, mst, Classes.StrawmanViews.YTD)
                .Union(new MarketViewKeybrandsController().GetGroupedByConfig(tbase, KEYBRANDS_VIEW, Classes.StrawmanViews.YTD)).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMonth()
        {
            ViewBag.BrandViewKeybrandsMonth = GetMonthData();
            return PartialView(monthview);
        }

        private dynamic GetMonthData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MONTH);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return new MarketViewKeybrandsController().GetGroupedData(tbase, mst, Classes.StrawmanViews.MONTH)
                .Union(new MarketViewKeybrandsController().GetGroupedByConfig(tbase, KEYBRANDS_VIEW, Classes.StrawmanViews.MONTH)).ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMAT()
        {
            ViewBag.BrandViewKeybrandsMAT = GetMATData();
            return PartialView(matview);
        }

        private dynamic GetMATData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>) new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MAT);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>) GetDataViewMaster();

            return new MarketViewKeybrandsController().GetGroupedData(tbase, mst, Classes.StrawmanViews.MAT)
                .Union(new MarketViewKeybrandsController().GetGroupedByConfig(tbase, KEYBRANDS_VIEW, Classes.StrawmanViews.MAT)).ToList();
        }

        public ActionResult GetDataView()
        {
            ViewBag.BrandViewKeybrandsData = GetSessionObject(KEYBRANDS_MASTER, GetDataViewData());
            return PartialView(dataview);
        }

        private dynamic GetDataViewData()
        {
            List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER> db = (List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_KEYBRANDS_MASTER, true);
            return db.Select(p => new { _name = p.NAME, _vid = (decimal)p.ID }).AsEnumerable()
                   .GroupBy(m => new { _name = m._name, _vid = m._vid }).AsEnumerable()
                   .ToList().Select(m => new Models.MarketViewChannelModels { name = m.Key._name, vid = m.Key._vid }).Union(new  MarketViewKeybrandsController().GetGroupedByConfigView(KEYBRANDS_VIEW)).ToList();
        }
        private dynamic GetDataViewMaster()
        {
            List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER> db = (List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_KEYBRANDS_MASTER, true);
            return db.Select(p=> new Models.StrawmanViewSTDModel { brand = p.BRAND, market = p.MARKET, channel = p.CHANNEL, vgroup = p.GROUP, vid = (decimal) p.ID, config = p.CONFIG_BRAND }).ToList();
        }

        //
        // GET: /BrandViewKeybrands/

        public ActionResult Index()
        {
            return View();
        }

        #region Default Functions
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
        #endregion
        
        #region Public Functions
        [ChildActionOnly]
        public List<Models.StrawmanViewSTDModel> GetMATKeybrandsData()
        {
            return this.GetMATData();
        }
        [ChildActionOnly]
        public List<Models.StrawmanViewSTDModel> GetMonthKeybrandsData()
        {
            return this.GetMonthData();
        }
        [ChildActionOnly]
        public List<Models.StrawmanViewSTDModel> GetYTDKeybrandsData()
        {
            return this.GetYTDData();
        }
        [ChildActionOnly]
        public List<Models.StrawmanViewSTDModel> GetBrandViewData(string type)
        {
            switch (type)
            {
                case Classes.StrawmanViews.MONTH:
                    return this.GetMonthData();
                case Classes.StrawmanViews.YTD:
                    return this.GetYTDData();
                case Classes.StrawmanViews.MAT:
                    return this.GetMATData();
                case Classes.StrawmanViews.BTG:
                    return this.GetBTGData();
                case Classes.StrawmanViews.TOTAL:
                    return this.GetTotalCustomData();
                case Classes.StrawmanViews.BOY:
                    return this.GetBOYData();
                default:
                    return null;
            }
        }
        #endregion

        #region Custom Functions

        private void SetSessionObject(string key, object obj)
        {
            Helpers.Session.SetSession(key, obj);
        }
        private object GetSessionObject(string key, object obj)
        {
            object ret = Helpers.Session.GetSession(key);
            if (obj != null && ret == null)
            {
                ret = obj;
                SetSessionObject(key, ret);
            }
            return ret;
        }
        private object GetSessionObject(string key)
        {
            return GetSessionObject(key, null);
        }

        private dynamic GetSessionData(string key, bool wc)
        {
            return Helpers.Session.GetSessionData(key, wc);
        }

        private dynamic GetSessionData(string key)
        {
            return GetSessionData(key, false);
        }

        private List<Models.StrawmanViewSTDModel> SetWCChannels(List<Models.StrawmanViewSTDModel> obj)
        {
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> wc_channels = var.Where(m => m.VIEW == Classes.Default.Variables.WC_CHANNELS).Select(m => m).ToList();
            foreach (Models.StrawmanViewSTDModel item in obj)
            {
                bool is_wc = wc_channels.Exists(m => m.VALUE == item.channel.ToString());
                item.col1_wc = is_wc?item.col1_wc: item.col1;
                item.col2_wc = is_wc ? item.col2_wc : item.col2;
                item.is_wc = is_wc;

            }
            return obj;
        }

        #endregion
        #region Constants
        private const string KEYBRANDS_MASTER = "KEYBRANDS_MASTER";
        private const string KEYBRANDS_VIEW = "KEYBRANDS_BRAND_VIEW";
        #endregion
    }
}
