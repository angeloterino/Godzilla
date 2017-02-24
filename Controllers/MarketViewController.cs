using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class MarketViewController : Controller
    {
        //
        // GET: /MarketView/
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetPCVSPY(string cache) {
            cache_status = bool.Parse(cache);
            ViewBag.MarketPCVSPY = GetDataPCVSPY();
            return PartialView(marketPCVSPY, GetData());
        }

        private dynamic GetDataPCVSPY()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA>)GetSessionData(WRK_MARKET_PCVSPY);
            return data//.Join(dataWC, m=> new {m.BRAND, m.MARKET, m.CHANNEL}, n=>new{n.BRAND, n.MARKET, n.CHANNEL},(m,n)=>new{m=m,n=n})
                .Select(p => new Models.StrawmanViewSTDModel 
                                        { 
                                            market = p.MARKET, brand = p.BRAND, channel =p.CHANNEL,vgroup = p.GROUP, vorder = p.ORDER, 
                                            pcvspy1 = p.PCVSPY_COL1, pcvspy2 = p.PCVSPY_COL2, pcvspy3 = p.PCVSPY_COL3, pcvspy4 = p.PCVSPY_COL4, pcvspy5 = p.PCVSPY_COL5
                                        })
                .ToList();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY(string cache) {
            cache_status = bool.Parse(cache);
            ViewBag.MarketBOY = GetDataBOY();
            return PartialView(marketBOY,GetData());
        }

        private dynamic GetDataBOY()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)GetSessionData(WRK_MARKET_BOY);
            return data//.Join(dataWC, m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                .Select(p => new Models.StrawmanViewSTDModel 
                                        { 
                                            market = p.MARKET, brand = p.BRAND, channel = p.CHANNEL, 
                                            _internal = (decimal?)p.INTERNAL, _le = (decimal?)p.LE, _pbp = (decimal?)p.PBP, vgroup = p.GROUP, 
                                            vorder = p.GROUP_ORDER
                                        })
                .ToList();
       
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom(string cache) {
            cache_status = bool.Parse(cache);
            ViewBag.MarketTotalCustom = GetDataTotalCustom();
            return PartialView(marketTotalCustom, GetData());
        }

        private dynamic GetDataTotalCustom()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA>)GetSessionData(WRK_MARKET_TOTAL_CUSTOM);
            return data.Select(p=>new Models.StrawmanViewSTDModel { market = p.MARKET, brand = p.BRAND, channel =p.CHANNEL,vgroup = p.GROUP, col1 = p.THREE_AGO, col2 = p.TWO_AGO, col3 = p.LAST, col4 = p.CURRENT, vorder = p.GROUP_ORDER })
                        .ToList();

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBTG(string cache) {
            cache_status = bool.Parse(cache);
            ViewBag.MarketBTG = GetDataBTG();
            return PartialView(marketBTG, GetData());
        }

        private dynamic GetDataBTG()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA>)GetSessionData(WRK_MARKET_BTG);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA>)GetSessionData(WRK_MARKET_BTG, true);
            return data.Join(dataWC, m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                .Select(p => new Models.StrawmanViewSTDModel 
                                            { 
                                                market = p.m.MARKET, brand = p.m.BRAND, channel = p.m.CHANNEL, 
                                                col1 = p.m.BTG_COL1, col2 = (decimal?)p.m.BTG_COL2, pcvspy = p.m.PCVSPY,
                                                col1_wc = (decimal?)p.w.BTG_COL1, 
                                                col2_wc = (decimal?)p.w.BTG_COL2,
                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal?)p.w.BTG_COL1, (decimal?)p.w.BTG_COL2),
                                                vgroup = p.m.GROUP, vorder = p.m.GROUP_ORDER, 
                                            })
                .ToList();

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMAT(string cache){
            cache_status = bool.Parse(cache);
            ViewBag.MarketMAT = GetDataMAT();
            return PartialView(marketMAT, GetData());
        }

        private dynamic GetDataMAT()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA>)GetSessionData(WRK_MARKET_MAT);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA>)GetSessionData(WRK_MARKET_MAT, true);
            return data.Join(dataWC, m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                .Select(p => new Models.StrawmanViewSTDModel {  market = p.m.MARKET, brand = p.m.BRAND, channel = p.m.CHANNEL, 
                                                                col1 = p.m.MAT_COL1, col2 = (decimal?)p.m.MAT_COL2, pcvspy = p.m.PCVSPY,
                                                                col1_wc = (decimal?)p.w.MAT_COL1, 
                                                                col2_wc = (decimal?)p.w.MAT_COL2, 
                                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(p.w.MAT_COL1, (decimal?)p.w.MAT_COL2),
                                                                vgroup = p.m.GROUP, vorder = p.m.GROUP_ORDER
                                                                })
                .ToList();

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetYTD(string cache)
        {
            cache_status = bool.Parse(cache);
            ViewBag.MarketYTD = GetDataYTD();
            return PartialView(marketYTD, GetData());
        }

        private dynamic GetDataYTD()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)GetSessionData(WRK_MARKET_YTD);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)GetSessionData(WRK_MARKET_YTD,true);
            return data.Join(dataWC, m=> new {m.BRAND, m.MARKET, m.CHANNEL}, w=> new{w.BRAND, w.MARKET, w.CHANNEL},(m,w) =>new {m=m,w=w})
                .Select(p => new Models.StrawmanViewSTDModel {  market = p.m.MARKET, brand = p.m.BRAND, channel = p.m.CHANNEL, 
                                                                col1 = p.m.YTD_COL1, col2 = p.m.YTD_COL2, pcvspy = p.m.PCVSPY,
                                                                col1_wc = p.w.YTD_COL1,
                                                                col2_wc = p.w.YTD_COL2,
                                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(p.w.YTD_COL1, p.w.YTD_COL2), 
                                                                vgroup = p.m.GROUP, vorder = p.m.GROUP_ORDER }).ToList();

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMonth(string cache)
        {
            cache_status = bool.Parse(cache);
            ViewBag.MarketMonth = GetDataMonth();
            return PartialView(marketMonth, GetData());
        }

        private dynamic GetDataMonth()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)GetSessionData(WRK_MARKET_MONTH);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)GetSessionData(WRK_MARKET_MONTH, true);
            return data.Join(dataWC, m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                .Select(p => new Models.StrawmanViewSTDModel {  market = p.m.MARKET, brand = p.m.BRAND, channel = p.m.CHANNEL, 
                                                                col1 = p.m.MONTH_COL1, col2 = p.m.MONTH_COL2, pcvspy = p.m.PCVSPY,
                                                                col1_wc = p.w.MONTH_COL1,
                                                                col2_wc = p.w.MONTH_COL2, 
                                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(p.w.MONTH_COL1, p.w.MONTH_COL2),
                                                                vgroup = p.m.GROUP,
                                                                vorder = p.m.GROUP_ORDER,}).ToList();

        }

        public ActionResult GetDataView(string cache)
        {
            cache_status = bool.Parse(cache);
            ViewBag.MarketData = GetData();
            return PartialView(marketDataView);            
        }

        private dynamic GetData()
        {
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> data = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)GetSessionData(v_STRWM_MARKET_DATA);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> wc_channels = var.Where(m => m.VIEW == Classes.Default.Variables.WC_CHANNELS).Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> colors = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_COLORS)
                    .Select(m => m).ToList();
            List<Models.MarketDataModels> aux = data.Where(m=>m.STATUS == "A").AsEnumerable()
                .GroupJoin(colors, l => new { ID = "BRAND:" + l.BRAND.ToString() + ";MARKET:" + l.MARKET.ToString() }, v => new { ID = v.NAME }, (l, v) => new { l = l, v = v })
                .SelectMany(f => f.v.DefaultIfEmpty(), (l, v) => new { l = l.l, v = v }).ToList()
                .Select(p => new Models.MarketDataModels
                {
                    market = (decimal)p.l.MARKET,
                    brand = (decimal)p.l.BRAND,
                    channel = (decimal)p.l.CHANNEL,
                    brand_name = p.l.BRAND_NAME,
                    market_name = p.l.NAME,
                    data = p.l.DATA,
                    source = p.l.SOURCE,
                    vgroup = p.l.GROUP,
                    vorder = p.l.ORDER,
                    vgorder = p.l.GROUP_ORDER,
                    style = p.v == null ? "" : Helpers.StyleUtils.GetBGColor(p.v.VALUE, true),
                    is_wc = wc_channels.Exists(m=>m.VALUE == p.l.CHANNEL.ToString())
                }).ToList();
            return aux;
            //return data
            //        .Select(p => new Models.MarketDataModels { market = (decimal)p.MARKET, brand = (decimal) p.BRAND, channel = (decimal)p.CHANNEL, brand_name = p.BRAND_NAME, data = p.DATA, market_name = p.NAME, source = p.SOURCE, vgroup = p.GROUP, vorder = p.ORDER }).ToList();
        }

        private dynamic GetSessionData(string key, bool wc)
        {
            //int _year = Helpers.PeriodUtil.Year;
            //int _month = Helpers.PeriodUtil.Month;
            //string session_key = key;
            //if (wc)
            //{
            //    session_key = key + "_WC";
            //    _month = _month - 1;
            //    if (_month <= 0) { _month = 12; _year = _year - 1; }
            //}
            //if (!cache_status) Helpers.Session.SetSession(session_key, null);
            //if (Helpers.Session.GetSession(session_key) == null)
            //{
            //    var obj = StrawmanDBLibray.DBLibrary.GetMarketData(key, _year, _month);
            //    Helpers.Session.SetSession(session_key, obj);
            //}
            return Helpers.Session.GetSessionData(key,wc);
        }
        private dynamic GetSessionData(string key)
        {
            return GetSessionData(key, false);
        }

        //private void GetPCVSPYWC(ref List<StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA> data)
        //{
        //    List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> boy = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)GetSessionData(WRK_MARKET_BOY);
        //    List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> bwc = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)GetSessionData(WRK_MARKET_BOY, true);
        //    List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA> tot = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA>)GetSessionData(WRK_MARKET_TOTAL_CUSTOM);
        //    data.ForEach(i=>i.PCVSPY_COL4 = Helpers.StrawmanCalcs.CalcPCVSPY(
        //        tot.Find(m=>m.BRAND == i.BRAND && m.CHANNEL == i.CHANNEL && m.MARKET == i.MARKET).LAST,
        //        (decimal?)(bwc.Find(m => m.BRAND == i.BRAND && m.CHANNEL == i.CHANNEL && m.MARKET == i.MARKET).LE)));
        //    data.ForEach(i => i.PCVSPY_COL5 = Helpers.StrawmanCalcs.CalcPCVSPY(
        //        (decimal?)(bwc.Find(m => m.BRAND == i.BRAND && m.CHANNEL == i.CHANNEL && m.MARKET == i.MARKET).LE)
        //        ,(decimal?)(bwc.Find(m => m.BRAND == i.BRAND && m.CHANNEL == i.CHANNEL && m.MARKET == i.MARKET).PBP)));
        //}
        
        //
        // GET: /MarketView/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        #region Default Functions
        //
        // GET: /MarketView/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MarketView/Create

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
        // GET: /MarketView/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MarketView/Edit/5

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
        // GET: /MarketView/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MarketView/Delete/5

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
        public object GetMarketViewData(string type)
        {
            switch (type)
            {
                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT:
                    return this.GetDataMAT();
                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH:
                    return this.GetDataMonth();
                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD:
                    return this.GetDataYTD();
                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG:
                    return this.GetDataBTG();
                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY:
                    return this.GetDataBOY();
                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_TOTAL:
                    return this.GetDataTotalCustom();
                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_PCVSPY:
                    return this.GetDataPCVSPY();
            }
            return null;
        }
        #endregion
        private bool cache_status = true;

        private const string marketDataView = "~/Views/MarketView/_MarketDataView.cshtml";
        private const string marketMonth = "~/Views/MarketView/_Market_Month.cshtml";
        private const string marketYTD = "~/Views/MarketView/_Market_YTD.cshtml";
        private const string marketMAT = "~/Views/MarketView/_Market_MAT.cshtml";
        private const string marketBTG = "~/Views/MarketView/_Market_BTG.cshtml";
        private const string marketTotalCustom = "~/Views/MarketView/_Market_TotalCustom.cshtml";
        private const string marketBOY = "~/Views/MarketView/_Market_BOY.cshtml";
        private const string marketPCVSPY = "~/Views/MarketView/_Market_PCVSPY.cshtml";

        #region Tables Definitions
        private const string WRK_MARKET_BOY = "WRK_MARKET_BOY";
        private const string v_STRWM_MARKET_DATA = "v_STRWM_MARKET_DATA";
        private const string WRK_MARKET_PCVSPY = "WRK_MARKET_PCVSPY";
        private const string WRK_MARKET_TOTAL_CUSTOM = "WRK_MARKET_TOTAL_CUSTOM";
        private const string WRK_MARKET_BTG = "WRK_MARKET_BTG";
        private const string WRK_MARKET_YTD = "WRK_MARKET_YTD";
        private const string WRK_MARKET_MONTH = "WRK_MARKET_MONTH";
        private const string WRK_MARKET_MAT = "WRK_MARKET_MAT";
        #endregion
    }
}
