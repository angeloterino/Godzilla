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
            new Helpers.Session().CacheStatus = true;
            ViewBag.MarketViewKeybrandsPCVSPY = GetPCVSPYData();
            return PartialView(pcvspyview);
        }

        private dynamic GetPCVSPYData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> bdata = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY, true);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA> tdata = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_TOTAL, true);
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

            return GetGroupedData(tbase, mst, Classes.StrawmanViews.PCVSPY); 
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY() {
            ViewBag.MarketViewKeybrandsBOY = GetBOYData();             
            return PartialView(boyview);
        }

        private dynamic GetBOYData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return GetGroupedData(tbase, mst, Classes.StrawmanViews.BOY); 
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom() {
            ViewBag.MarketViewKeybrandsTotalCustom = GetTotalCustomData();               
            return PartialView(totalcustomview);
        }

        private dynamic GetTotalCustomData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_TOTAL);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return GetGroupedData(tbase, mst, Classes.StrawmanViews.TOTAL); 
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBTG() {
            ViewBag.MarketViewKeybrandsBTG = GetBTGData();
            return PartialView(btgview);
        }

        private dynamic GetBTGData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return GetGroupedData(tbase, mst, Classes.StrawmanViews.BTG); 
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetYTD()
        {
            ViewBag.MarketViewKeybrandsYTD = GetYTDData();
            return PartialView(ytdview);
        }
        
        private dynamic GetYTDData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return GetGroupedData(tbase, mst, Classes.StrawmanViews.YTD); 
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMonth()
        {
            ViewBag.MarketViewKeybrandsMonth = GetMonthData();
            return PartialView(monthview);
        }

        private dynamic GetMonthData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return GetGroupedData(tbase, mst, Classes.StrawmanViews.MONTH);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMAT()
        {
            ViewBag.MarketViewKeybrandsMAT = GetMATData();
            return PartialView(matview);
        }

        private dynamic GetMATData()
        {
            List<Models.StrawmanViewSTDModel> tbase = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT);
            List<Models.StrawmanViewSTDModel> mst = (List<Models.StrawmanViewSTDModel>)GetDataViewMaster();

            return GetGroupedData(tbase, mst, Classes.StrawmanViews.MAT);
        }

        public ActionResult GetDataView()
        {
            ViewBag.MarketViewKeybrandsData = GetSessionObject(KEYBRANDS_MASTER, GetDataViewData());
            return PartialView(dataview);
        }

        private dynamic GetDataViewData()
        {
            List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER> db = (List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_KEYBRANDS_MASTER, true);
            return db.Select(p => new Models.MarketViewChannelModels { name = p.NAME, vid = (decimal)p.ID }).Distinct().ToList();
        }
        private dynamic GetDataViewMaster()
        {
            List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER> db = (List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_KEYBRANDS_MASTER, true);
            return db.Select(p => new Models.StrawmanViewSTDModel { brand = p.BRAND, market = p.MARKET, channel = p.CHANNEL, vgroup = p.GROUP, vid = (decimal)p.ID, config = p.CONFIG_BRAND }).ToList();
        }

        //
        // GET: /MarketViewKeybrands/

        public ActionResult Index()
        {
            return View();
        }

        #region Default Functions
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
        #endregion

        #region Private Funcitions
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
                item.col1_wc = is_wc ? item.col1_wc : item.col1;
                item.col2_wc = is_wc ? item.col2_wc : item.col2;
                item.is_wc = is_wc;

            }
            return obj;
        }
        #endregion

        #region Public Functions
        public List<Models.StrawmanViewSTDModel> GetGroupedData(dynamic tbase, List<Models.StrawmanViewSTDModel> mst, string type)
        {
            var data = mst.GroupJoin(((List<Models.StrawmanViewSTDModel>)tbase).AsEnumerable(), m => new { m.market, m.brand, m.channel }, d => new { d.market, d.brand, d.channel }, (m, d) => new { m = m, d = d })
                                .SelectMany(f => f.d, (m, d) => new { m = m.m, d = d }).ToList()
                                .Select(p => new Models.StrawmanViewSTDModel
                                {
                                    col1 = (p.d.col1??0) * (p.m.config??1),
                                    col2 = (p.d.col2??0) * (p.m.config ?? 1),
                                    col3 = (p.d.col3??0) * (p.m.config ?? 1),
                                    col4 = (p.d.col4??0) * (p.m.config ?? 1),
                                    col5 = (p.d.col5??0) * (p.m.config ?? 1),
                                    col6 = (p.d.col6??0) * (p.m.config ?? 1),
                                    col1_wc = (p.d.col1_wc??0) * (p.m.config ?? 1),
                                    col2_wc = (p.d.col2_wc??0) * (p.m.config ?? 1),
                                    vid = (decimal)p.m.vid,
                                    channel = p.m.channel
                                });
            switch (type)
            {
                case Classes.StrawmanViews.PCVSPY:
                    
                    return SetWCChannels(data.ToList())
                            .AsEnumerable()
                            .GroupBy(m => new { _key = (decimal)m.vid })
                            .Select(m => new Models.StrawmanViewSTDModel
                            {
                                vid = m.Key._key,
                                pcvspy1 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col1), m.Sum(p => p.col2)),
                                pcvspy2 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col2), m.Sum(p => p.col3)),
                                pcvspy3 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col3), m.Sum(p => p.col4)),
                                pcvspy4 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col3), m.Sum(p => p.col5)),
                                pcvspy5 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col5), m.Sum(p => p.col6)),
                            }).ToList();
                
                case Classes.StrawmanViews.BOY:
                case Classes.StrawmanViews.TOTAL:

                    return SetWCChannels(data.ToList())
                            .AsEnumerable()
                            .GroupBy(m => new { _key = (decimal)m.vid })
                            .Select(m => new Models.StrawmanViewSTDModel
                            {
                                vid = m.Key._key,
                                col1 = m.Sum(p => p.col1),
                                col2 = m.Sum(p => p.col2),
                                col3 = m.Sum(p => p.col3),
                            }).ToList();
                
                default:

                    return SetWCChannels(data.ToList())
                            .AsEnumerable()
                            .GroupBy(m => new { _key = (decimal)m.vid })
                            .Select(m => new Models.StrawmanViewSTDModel
                            {
                                vid = m.Key._key,
                                col1 = m.Sum(p => p.col1),
                                col2 = m.Sum(p => p.col2),
                                col1_wc = m.Sum(p => p.col1_wc),
                                col2_wc = m.Sum(p => p.col2_wc),
                                pcvspy = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col1), m.Sum(p => p.col2)),
                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col1_wc), m.Sum(p => p.col2_wc)),
                            }).ToList();
            }
        }
        #endregion

        #region Constants
        private const string KEYBRANDS_MASTER = "KEYBRANDS_MASTER";
        #endregion

    }
}
