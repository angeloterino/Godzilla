using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class ShareBoardController : Controller
    {
        #region Partial Views

        public ActionResult GetData()
        {
            ViewBag.MarketData = GetDataView();
            SetSessionObject("MarketData", ViewBag.MarketData);
            return PartialView(MARKET_DATA_VIEW, GetSessionObject("MarketData"));
        }

        public ActionResult GetDataChannel()
        {
            ViewBag.MarketViewChannelsData = GetDataViewChannel();
            SetSessionObject("MarketViewChannelsData", ViewBag.MarketViewChannelsData);
            return PartialView(SHAREBOARDDATAVIEWCHANNEL, GetSessionObject("MarketViewChannelsData"));
        }

        public ActionResult GetDataFranchise()
        {
            ViewBag.MarketViewFranchiseData = GetDataViewFranchise();
            SetSessionObject("MarketViewFranchiseData", ViewBag.MarketViewFranchiseData);
            return PartialView(SHAREBOARDDATAVIEWFRANCHISE, GetSessionObject("MarketViewFranchiseData"));
        }
        public ActionResult GetDataKeybrands()
        {
            ViewBag.MarketViewKeybrandsData = GetDataViewKeybrands();
            SetSessionObject("MarketViewKeybrandsData", ViewBag.MarketViewKeybrandsData);
            return PartialView(SHAREBOARDDATAVIEWKEYBRANDS, GetSessionObject("MarketViewKeybrandsData"));
        }

        public ActionResult GetMonth()
        {
            ViewBag.MarketMonth = GetMonthData();
            return PartialView(SHAREBOARDMONTH, GetSessionObject("MarketData"));
        }
        public ActionResult GetMonthChannel()
        {
            ViewBag.MarketViewChannels = GetSessionObject("MarketViewChannelsData");
            return PartialView(SHAREBOARDCHANNEL, GetMonthDataChannel());
        }
        public ActionResult GetMonthFranchise()
        {
            ViewBag.MarketViewFranchiseData = GetSessionObject("MarketViewFranchiseData");
            return PartialView(SHAREBOARDMONTHFRANCHISE, GetMonthDataFranchise());
        }
        public ActionResult GetMonthKeybrands()
        {
            ViewBag.MarketViewKeybrandsMonth = GetSessionObject("MarketViewKeybrandsData");
            return PartialView(SHAREBOARDMONTHKEYBRANDS, GetMonthDataKeybrands());
        }

        public ActionResult GetYTD()
        {
            ViewBag.MarketYTD = GetYTDData();
            return PartialView(SHAREBOARDYTD, GetSessionObject("MarketData"));
        }
        public ActionResult GetYTDChannel()
        {
            ViewBag.MarketViewChannels = GetSessionObject("MarketViewChannelsData");
            return PartialView(SHAREBOARDCHANNEL, GetYTDDataChannel());
        }
        public ActionResult GetYTDFranchise()
        {
            ViewBag.MarketViewFranchiseData = GetSessionObject("MarketViewFranchiseData");
            return PartialView(SHAREBOARDYTDFRANCHISE, GetYTDDataFranchise());
        }
        public ActionResult GetYTDKeybrands()
        {
            ViewBag.MarketViewKeybrandsYTD = GetSessionObject("MarketViewKeybrandsData");
            return PartialView(SHAREBOARDYTDKEYBRANDS, GetYTDDataKeybrands());
        }

        public ActionResult GetMAT()
        {
            ViewBag.MarketMAT = GetMATData();
            return PartialView(SHAREBOARDMAT, GetSessionObject("MarketData"));
        }
        public ActionResult GetMATChannel()
        {
            ViewBag.MarketViewChannels = GetSessionObject("MarketViewChannelsData");
            return PartialView(SHAREBOARDCHANNEL, GetMATDataChannel());
        }
        public ActionResult GetMATFranchise()
        {
            ViewBag.MarketViewFranchiseData = GetSessionObject("MarketViewFranchiseData");
            return PartialView(SHAREBOARDMATFRANCHISE, GetMATDataFranchise());
        }
        public ActionResult GetMATKeybrands()
        {
            ViewBag.MarketViewKeybrandsMAT = GetSessionObject("MarketViewKeybrandsData");
            return PartialView(SHAREBOARDMATKEYBRANDS, GetMATDataKeybrands());
        }
        #endregion

        #region DataView
        private dynamic GetDataView()
        {
            //List<Models.MarketDataModels> lst = null;
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> data = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_MARKET_DATA, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> wc_channels = var.Where(m => m.VIEW == Classes.Default.Variables.WC_CHANNELS).Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> colors = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_COLORS)
                    .Select(m => m).ToList();
            List<Models.MarketDataModels> aux = data.Where(m => m.STATUS == "A").AsEnumerable()
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
                    is_wc = wc_channels.Exists(m => m.VALUE == p.l.CHANNEL.ToString())
                }).ToList();
            return aux;
            //    var query = from p in table
            //                select new Models.MarketDataModels { market = (decimal)p.MARKET, brand = p.BRAND, channel= (decimal)p.CHANNEL, brand_name = p.BRAND_NAME, data = p.DATA, market_name = p.NAME, source = p.SOURCE, vgroup = p.GROUP, vorder = p.ORDER };
            //    lst = query.ToList();
            //return lst;
        }
        private dynamic GetDataViewChannel()
        {
            List<StrawmanDBLibray.Entities.GROUP_MASTER> lst = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> colors = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_CHANNELS_COLORS)
                    .Select(m => m).ToList();
            return lst
                    .GroupJoin(colors, l => new { ID = l.ID }, v => new { ID = decimal.Parse(v.NAME) }, (l, v) => new { l = l, v = v })
                    .Where(m => m.l.TYPE == 20).Distinct()
                    .SelectMany(f => f.v.DefaultIfEmpty(), (l, v) => new { l = l.l, v = v }).AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { name = p.l.NAME, vorder = p.l.ID, vid = p.l.ID, vchannel = p.l.ID, style = p.v == null ? "" : Helpers.StyleUtils.GetBGColor(p.v.VALUE, true), }).ToList();
            //List<Models.MarketViewChannelModels> lst = null;
            //using (StrawmanApp.Models.ChannelsDataClassesDataContext db = new StrawmanApp.Models.ChannelsDataClassesDataContext())
            //{
            //    var query = from p in db.v_WRK_CHANNEL_DATA
            //                select new Models.MarketViewChannelModels { name = p.NAME, vid = p.ID, vorder = p.ID, vchannel = p.ID };
            //    lst = query.ToList();
            //}
            //return lst;
        }

        private dynamic GetDataViewFranchise()
        {
            List<Models.MarketViewChannelModels> lst = null;
            using (StrawmanApp.Models.FranchiseDataClassesDataContext db = new StrawmanApp.Models.FranchiseDataClassesDataContext())
            {
                var query = from p in db.v_WRK_FRANCHISE_DATA
                            select new Models.MarketViewChannelModels { name = p.NAME, vorder = p.ORDER, vhas_child = p.HAS_CHILD, vparent = p.PARENT_ID, vid = p.ID };
                lst = query.ToList();
            }
            return lst;
        }

        private dynamic GetDataViewKeybrands()
        {   
            List<Models.MarketViewChannelModels> lst = null;
            using (StrawmanApp.Models.KeybrandsDataClassesDataContext db = new StrawmanApp.Models.KeybrandsDataClassesDataContext())
            {         
            var query = from p in db.v_WRK_KEYBRANDS_BASE
                        where p.DATA == "BRAND" && p.TYPE == "MAT"
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.MarketViewChannelModels { name = p.NAME, vid = (decimal)p.ID };
                lst = query.ToList();
            }
            return lst;
        }
        #endregion

        #region YTD Data
        private dynamic GetYTDData(string type)
        {
            switch(type){
                case "FRANCHISE":
                    return ((List<Entities.ShareBoardModel>)SetFranchiseData(new MarketViewFranchiseController().GetYTDFranchiseData(), new BrandViewFranchiseController().GetYTDFranchiseData()))
                        .Select(p => new Entities.ShareBoardModel
                                {
                                    ytd_col1 = p.col1,
                                    ytd_col2 = p.col2,
                                    ytd_col3 = p.col3,
                                    ytd_col4 = p.col4,
                                    ytd_col5 = p.col5,
                                    ytd_col6 = p.col6,
                                    order = p.order,
                                    group = p.group,
                                    channel = p.channel,
                                    id = p.id
                                }).ToList();
                case "KEYBRANDS":
                    return ((List<Entities.ShareBoardModel>)SetKeybrandsData(new MarketViewKeybrandsController().GetYTDKeybrandsData(), new BrandViewKeybrandsController().GetYTDKeybrandsData()))
                        .Select(p => new Entities.ShareBoardModel
                                {
                                    ytd_col1 = p.col1,
                                    ytd_col2 = p.col2,
                                    ytd_col3 = p.col3,
                                    ytd_col4 = p.col4,
                                    ytd_col5 = p.col5,
                                    ytd_col6 = p.col6,
                                    order = p.order,
                                    group = p.group,
                                    channel = p.channel,
                                    id = p.id
                                }).ToList();
                default:
                    return (List<Entities.ShareBoardModel>)GetStrawmanData("v_SHAREBOARD_YTD", type);
            }
            //List<Entities.ShareBoardModel> lst = null;
            //using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            //{
            //    db.CommandTimeout = 50000;
            //    var query = db.v_SHAREBOARD_YTD.Where(p=>p.TYPE == type
            //                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
            //                .Select(p => new Entities.ShareBoardModel { market = p.MARKET, brand = p.BRAND, order = p.GROUP_ORDER, group = p.GROUP, channel = p.CHANNEL, ytd_col1 = p.MARKET_GROUTH, ytd_col2 = p.BRAND_GROUTH, ytd_col3 = p.MARKET_SHARE, ytd_col4 = p.PT_PLUS_MINUS, ytd_col5 = (double?)p.MARKET_SIZE, ytd_col6 = (double?)p.BRAND_SIZE });
            //    lst = query.ToList();
            //}
        }
        private dynamic GetYTDData()
        {
            List<Entities.ShareBoardModel> lst = GetYTDData("DATA");
            return lst;
        }
        private dynamic GetYTDDataChannel()
        {
            List<Entities.ShareBoardModel> lst = SetChannelData(GetYTDData("DATA"));
            return lst;
        }
        private dynamic GetYTDDataFranchise()
        {
            List<Entities.ShareBoardModel> lst = GetYTDData("FRANCHISE");
            return lst;
        }
        private dynamic GetYTDDataKeybrands()
        {
            List<Entities.ShareBoardModel> lst = GetYTDData("KEYBRANDS");
            return lst;
        }
        #endregion

        #region Month Data
        private dynamic GetMonthData(string type)
        {
            switch (type)
            {
                case "FRANCHISE":
                    return ((List<Entities.ShareBoardModel>)SetFranchiseData(new MarketViewFranchiseController().GetMonthFranchiseData(), new BrandViewFranchiseController().GetMonthFranchiseData()))
                        .Select(p => new Entities.ShareBoardModel
                                {
                                    month_col1 = p.col1,
                                    month_col2 = p.col2,
                                    month_col3 = p.col3,
                                    month_col4 = p.col4,
                                    month_col5 = (decimal?)p.col5,
                                    month_col6 = (decimal?)p.col6,
                                    order = p.order,
                                    group = p.group,
                                    channel = p.channel,
                                    id = p.id
                                }).ToList();
                case "KEYBRANDS":
                    return ((List<Entities.ShareBoardModel>)SetKeybrandsData(new MarketViewKeybrandsController().GetMonthKeybrandsData(), new BrandViewKeybrandsController().GetMonthKeybrandsData()))
                        .Select(p => new Entities.ShareBoardModel
                        {
                            month_col1 = p.col1,
                            month_col2 = p.col2,
                            month_col3 = p.col3,
                            month_col4 = p.col4,
                            month_col5 = (decimal?)p.col5,
                            month_col6 = (decimal?)p.col6,
                            order = p.order,
                            group = p.group,
                            channel = p.channel,
                            id = p.id
                        }).ToList();
                default:
                    return (List<Entities.ShareBoardModel>)GetStrawmanData("v_SHAREBOARD_MONTH", type);
            }
            //List<Entities.ShareBoardModel> lst = null;
            //using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            //{
            //    db.CommandTimeout = 50000;
            //    var query = db.v_SHAREBOARD_MONTH.Where(p => p.TYPE == type
            //                &&(p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
            //                .Select(p => new Entities.ShareBoardModel { market = p.MARKET, brand = p.BRAND, channel = p.CHANNEL, order = p.GROUP_ORDER, group = p.GROUP, month_col1 = p.MARKET_GROUTH, month_col2 = p.BRAND_GROUTH, month_col3 = p.MARKET_SHARE, month_col4 = p.PT_PLUS_MINUS, month_col5 = p.MARKET_SIZE, month_col6 = p.BRAND_SIZE });
            //    lst = query.ToList();
            //}
        }
        private dynamic GetMonthData()
        {
            List<Entities.ShareBoardModel> lst = GetMonthData("DATA");
            return lst;
        }
        private dynamic GetMonthDataChannel()
        {
            List<Entities.ShareBoardModel> lst = SetChannelData(GetMonthData("DATA"));
            return lst;
        }
        private dynamic GetMonthDataFranchise()
        {
            List<Entities.ShareBoardModel> lst = GetMonthData("FRANCHISE");
            return lst;
        }
        private dynamic GetMonthDataKeybrands()
        {
            List < Entities.ShareBoardModel > lst = GetMonthData("KEYBRANDS"); ;
            return lst;
        }

        #endregion

        #region MAT Data
        private dynamic GetMATData(string type)
        {
            switch (type)
            {
                case "FRANCHISE":
                    return ((List<Entities.ShareBoardModel>)SetFranchiseData(new MarketViewFranchiseController().GetMATFranchiseData(), new BrandViewFranchiseController().GetMATFranchiseData()))
                        .Select(p => new Entities.ShareBoardModel
                        {
                            mat_col1 = p.col1,
                            mat_col2 = p.col2,
                            mat_col3 = p.col3,
                            mat_col4 = p.col4,
                            mat_col5 = p.col5,
                            mat_col6 = p.col6,
                            order = p.order,
                            group = p.group,
                            channel = p.channel,
                            id = p.id
                        }).ToList();
                case "KEYBRANDS":
                    return ((List<Entities.ShareBoardModel>)SetKeybrandsData(new MarketViewKeybrandsController().GetMATKeybrandsData(), new BrandViewKeybrandsController().GetMATKeybrandsData()))
                        .Select(p => new Entities.ShareBoardModel
                        {
                            mat_col1 = p.col1,
                            mat_col2 = p.col2,
                            mat_col3 = p.col3,
                            mat_col4 = p.col4,
                            mat_col5 = p.col5,
                            mat_col6 = p.col6,
                            order = p.order,
                            group = p.group,
                            channel = p.channel,
                            id = p.id
                        }).ToList();
                default:
                    return (List<Entities.ShareBoardModel>)GetStrawmanData("v_SHAREBOARD_MAT", type);
            }
            //List<Entities.ShareBoardModel> lst = null;
            //using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            //{
            //    db.CommandTimeout = 50000;
            //    var query = db.v_SHAREBOARD_MAT.Where(p => p.TYPE == type
            //                &&(p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
            //                .Select(p => new Entities.ShareBoardModel { market = p.MARKET, brand = p.BRAND, channel = p.CHANNEL, order = p.GROUP_ORDER, group = p.GROUP, mat_col1 = p.MARKET_GROUTH, mat_col2 = p.BRAND_GROUTH, mat_col3 = p.MARKET_SHARE, mat_col4 = p.PT_PLUS_MINUS, mat_col5 = (double?)p.MARKET_SIZE, mat_col6 = (double?)p.BRAND_SIZE });
            //    lst = query.ToList();
            //}
        }
        private dynamic GetMATData()
        {
            List<Entities.ShareBoardModel> lst = GetMATData("DATA");
            return lst;
        }
        private dynamic GetMATDataChannel()
        {
            List<Entities.ShareBoardModel> lst = SetChannelData(GetMATData("DATA"));
            return lst;
        }
        private dynamic GetMATDataFranchise()
        {
            List<Entities.ShareBoardModel> lst = GetMATData("FRANCHISE");
            return lst;
        }
        private dynamic GetMATDataKeybrands()
        {
            List<Entities.ShareBoardModel> lst = GetMATData("KEYBRANDS");
            return lst;
        }

        #endregion
        //
        // GET: /ShareBoard/

        public ActionResult Index()
        {
            ViewBag.MenuUrl = MENU_URL;
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            return View();
        }
        #region Default Functions
        //
        // GET: /ShareBoard/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ShareBoard/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShareBoard/Create

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
        // GET: /ShareBoard/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ShareBoard/Edit/5

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
        // GET: /ShareBoard/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ShareBoard/Delete/5

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

        #region Functions
        private void SetSessionObject(string key, object obj)
        {
            Helpers.Session.SetSession(key,obj);
        }

        private object GetSessionObject(string key)
        {
            return Helpers.Session.GetSession(key);
        }

        private object GetStrawmanData(string view, string type)
        {
            object lst = null;
            switch (view)
            {
                case "v_SHAREBOARD_YTD":
                    List<StrawmanDBLibray.Entities.v_SHAREBOARD_YTD> ytd_aux = (List<StrawmanDBLibray.Entities.v_SHAREBOARD_YTD>)Helpers.StrawmanDBLibrayData.Get(view, true);
                    lst = ytd_aux.Where(p => p.TYPE == type
                                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                        .Select(p => new Entities.ShareBoardModel { 
                                            market = p.MARKET, 
                                            brand = p.BRAND, 
                                            order = p.GROUP_ORDER, 
                                            group = p.GROUP, 
                                            channel = p.CHANNEL, 
                                            ytd_col1 = p.MARKET_GROUTH, 
                                            ytd_col2 = p.BRAND_GROUTH, 
                                            ytd_col3 = p.MARKET_SHARE, 
                                            ytd_col4 = p.PT_PLUS_MINUS, 
                                            ytd_col5 = (double?)p.MARKET_SIZE,
                                            ytd_col6 = (double?)p.BRAND_SIZE,
                                            col1 = p.MARKET_GROUTH,
                                            col2 = p.BRAND_GROUTH,
                                            col3 = p.MARKET_SHARE,
                                            col4 = p.PT_PLUS_MINUS,
                                            col5 = (double?)p.MARKET_SIZE,
                                            col6 = (double?)p.BRAND_SIZE,
                                            market_col1 = p.MARKET_LAST_YEAR,
                                            market_col2 = p.MARKET_THIS_YEAR,
                                            brand_col1 = p.BRAND_LAST_YEAR,
                                            brand_col2 = p.BRAND_THIS_YEAR
                                        })
                                        .ToList();
                    break;
                case "v_SHAREBOARD_MAT":
                    List<StrawmanDBLibray.Entities.v_SHAREBOARD_MAT> mat_aux = (List<StrawmanDBLibray.Entities.v_SHAREBOARD_MAT>)Helpers.StrawmanDBLibrayData.Get(view, true);
                    lst = mat_aux.Where(p => p.TYPE == type
                                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                        .Select(p => new Entities.ShareBoardModel { 
                                            market = p.MARKET, 
                                            brand = p.BRAND, 
                                            order = p.GROUP_ORDER, 
                                            group = p.GROUP, 
                                            channel = p.CHANNEL, 
                                            mat_col1 = p.MARKET_GROUTH, 
                                            mat_col2 = p.BRAND_GROUTH, 
                                            mat_col3 = p.MARKET_SHARE, 
                                            mat_col4 = p.PT_PLUS_MINUS, 
                                            mat_col5 = (double?)p.MARKET_SIZE,
                                            mat_col6 = (double?)p.BRAND_SIZE,
                                            col1 = p.MARKET_GROUTH,
                                            col2 = p.BRAND_GROUTH,
                                            col3 = p.MARKET_SHARE,
                                            col4 = p.PT_PLUS_MINUS,
                                            col5 = (double?)p.MARKET_SIZE,
                                            col6 = (double?)p.BRAND_SIZE,
                                            market_col1 = p.MARKET_LAST_YEAR,
                                            market_col2 = (decimal?)p.MARKET_THIS_YEAR,
                                            brand_col1 = p.BRAND_LAST_YEAR,
                                            brand_col2 = (decimal?)p.BRAND_THIS_YEAR
                                        })
                                        .ToList();
                    break;
                case "v_SHAREBOARD_MONTH":
                    List<StrawmanDBLibray.Entities.v_SHAREBOARD_MONTH> month_aux = (List<StrawmanDBLibray.Entities.v_SHAREBOARD_MONTH>)Helpers.StrawmanDBLibrayData.Get(view,true);
                    lst = month_aux.Where(p => p.TYPE == type
                                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                        .Select(p => new Entities.ShareBoardModel { 
                                            market = p.MARKET, 
                                            brand = p.BRAND, 
                                            order = p.GROUP_ORDER, 
                                            group = p.GROUP, 
                                            channel = p.CHANNEL, 
                                            month_col1 = p.MARKET_GROUTH, 
                                            month_col2 = p.BRAND_GROUTH, 
                                            month_col3 = p.MARKET_SHARE, 
                                            month_col4 = p.PT_PLUS_MINUS, 
                                            month_col5 = p.MARKET_SIZE,
                                            month_col6 = p.BRAND_SIZE,
                                            col1 = p.MARKET_GROUTH,
                                            col2 = p.BRAND_GROUTH,
                                            col3 = p.MARKET_SHARE,
                                            col4 = p.PT_PLUS_MINUS,
                                            col5 = (double?)p.MARKET_SIZE,
                                            col6 = (double?)p.BRAND_SIZE,
                                            market_col1 = p.MARKET_LAST_YEAR,
                                            market_col2 = p.MARKET_THIS_YEAR,
                                            brand_col1 = p.BRAND_LAST_YEAR,
                                            brand_col2 = p.BRAND_THIS_YEAR
                                        })
                                        .ToList();
                    break;
            }
            return lst;
        }

        private object SetChannelData(List<Entities.ShareBoardModel> lst)
        {
            List<StrawmanDBLibray.Entities.GROUP_TYPES> grp = (List<StrawmanDBLibray.Entities.GROUP_TYPES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_TYPES, true);
            grp = grp.Where(m => m.ID == 20).Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> cfg = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG, true);
            cfg = cfg.Where(m => m.TYPE_ID == grp.First().ID).Distinct().Select(m => m).ToList();
            List<Entities.ShareBoardModel> aux = lst
                    .Join(cfg, c => new { BRAND = c.brand, MARKET = c.market }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d })
                    .Select(p => new Entities.ShareBoardModel
                    {
                         id = (decimal)p.d.GROUP_ID,
                         market = p.c.market, 
                         brand = p.c.brand, 
                         order = p.c.order, 
                         group = p.c.group, 
                         channel = p.c.channel, 
                         market_col1 = p.c.market_col1,
                         market_col2 = p.c.market_col2,
                         brand_col1 = p.c.brand_col1,
                         brand_col2 = p.c.brand_col2
                    }).ToList();
            return aux.GroupBy(m=> new {m.id}).Select(p=> new Entities.ShareBoardModel
                                {   
                                    col1 = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.market_col1),p.Sum(w=>w.market_col2)),
                                    col2 = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.brand_col1), p.Sum(w => w.brand_col2)),
                                    col3 = Helpers.StrawmanCalcs.CalcShare(p.Sum(w => w.market_col2),p.Sum(w=>w.brand_col2)),
                                    col4 = Helpers.StrawmanCalcs.CalcShare(p.Sum(w => w.market_col2), p.Sum(w => w.brand_col2)) - Helpers.StrawmanCalcs.CalcShare(p.Sum(w => w.market_col1), p.Sum(w => w.brand_col1)),
                                    col5 = (double?)p.Sum(w => w.market_col2),
                                    col6 = (double?)p.Sum(w => w.brand_col2),
                                    order = p.Max(w => w.order), 
                                    group = p.Max(w => w.group), 
                                    channel = p.Max(w => w.channel), 
                                    id = p.Key.id 
                                })
                        .ToList();
        }

        private object SetFranchiseData(List<Models.MarketViewChannelModels> mrklst, List<Models.MarketViewChannelModels> brdlst)
        {
            var lst = mrklst.Join(brdlst, m => new { _id = m.vid }, b => new { _id = b.vid }, (m, b) => new { m = m, b = b }).ToList();
            //List<Models.MarketViewChannelModels> mst = (List<Models.MarketViewChannelModels>)GetDataViewFranchise();
            return lst.Select(p => new Entities.ShareBoardModel
            {
                col1 = Helpers.StrawmanCalcs.CalcPCVSPY(p.m.col1, p.m.col2),
                col2 = Helpers.StrawmanCalcs.CalcPCVSPY(p.b.col1, p.b.col2),
                col3 = Helpers.StrawmanCalcs.CalcShare(p.m.col2, p.b.col2),
                col4 = Helpers.StrawmanCalcs.CalcShare(p.m.col2, p.b.col2) - Helpers.StrawmanCalcs.CalcShare(p.m.col1, p.b.col1),
                col5 = (double?)p.m.col2,
                col6 = (double?)p.b.col2,
                order = p.m.vorder,
                group = p.m.vgroup,
                channel = p.m.vchannel,
                id = p.m.vid
            }).ToList();

            return null;
        }

        private object SetKeybrandsData(List<Models.StrawmanViewSTDModel> mrklst, List<Models.StrawmanViewSTDModel> brdlst)
        {
            List<Models.MarketViewChannelModels> mst = (List<Models.MarketViewChannelModels>)GetDataViewKeybrands();
            var lst = mrklst.Join(brdlst, m => new { _id = m.vid }, b => new { _id = b.vid }, (m, b) => new { m = m, b = b }).ToList();
            //List<Models.MarketViewChannelModels> mst = (List<Models.MarketViewChannelModels>)GetDataViewFranchise();
            return lst.Select(p => new Entities.ShareBoardModel
            {
                col1 = Helpers.StrawmanCalcs.CalcPCVSPY(p.m.col1, p.m.col2),
                col2 = Helpers.StrawmanCalcs.CalcPCVSPY(p.b.col1, p.b.col2),
                col3 = Helpers.StrawmanCalcs.CalcShare(p.m.col2, p.b.col2),
                col4 = Helpers.StrawmanCalcs.CalcShare(p.m.col2, p.b.col2) - Helpers.StrawmanCalcs.CalcShare(p.m.col1, p.b.col1),
                col5 = (double?)p.m.col2,
                col6 = (double?)p.b.col2,
                order = p.m.vorder,
                group = p.m.vgroup,
                channel = p.m.channel,
                id = (decimal)p.m.vid
            }).ToList();
            return null;
        }

        public dynamic GetShareBoardChannelData(string type)
        {
            switch (type)
            {
                case "YTD":
                    return GetYTDDataChannel();
                case "MAT":
                    return GetMATDataChannel();
                default:
                    return GetMonthDataChannel();
            }
        }
        #endregion

        #region Constants
        private const string SHAREBOARDDATAVIEW = "~/Views/ShareBoard/_DataView.cshtml";
        private const string MARKET_DATA_VIEW = "~/Views/MarketView/_MarketDataView.cshtml";
        private const string SHAREBOARDDATAVIEWCHANNEL = "~/Views/ShareBoard/_DataViewChannel.cshtml";
        private const string SHAREBOARDDATAVIEWFRANCHISE = "~/Views/ShareBoard/_DataViewFranchise.cshtml";
        private const string SHAREBOARDDATAVIEWKEYBRANDS = "~/Views/ShareBoard/_DataViewKeybrands.cshtml";
        private const string SHAREBOARDMONTH = "~/Views/ShareBoard/_ShareBoard_Month.cshtml";
        private const string SHAREBOARDMONTHCHANNEL = "~/Views/ShareBoard/_ShareBoard_MonthChannel.cshtml";
        private const string SHAREBOARDMONTHFRANCHISE = "~/Views/ShareBoard/_ShareBoard_MonthFranchise.cshtml";
        private const string SHAREBOARDMONTHKEYBRANDS = "~/Views/ShareBoard/_ShareBoard_MonthKeybrands.cshtml";
        private const string SHAREBOARDYTD = "~/Views/ShareBoard/_ShareBoard_YTD.cshtml";
        private const string SHAREBOARDYTDCHANNEL = "~/Views/ShareBoard/_ShareBoard_YTDChannel.cshtml";
        private const string SHAREBOARDYTDFRANCHISE = "~/Views/ShareBoard/_ShareBoard_YTDFranchise.cshtml";
        private const string SHAREBOARDYTDKEYBRANDS = "~/Views/ShareBoard/_ShareBoard_YTDKeybrands.cshtml";
        private const string SHAREBOARDMAT = "~/Views/ShareBoard/_ShareBoard_MAT.cshtml";
        private const string SHAREBOARDMATCHANNEL = "~/Views/ShareBoard/_ShareBoard_MATChannel.cshtml";
        private const string SHAREBOARDMATFRANCHISE = "~/Views/ShareBoard/_ShareBoard_MATFranchise.cshtml";
        private const string SHAREBOARDMATKEYBRANDS = "~/Views/ShareBoard/_ShareBoard_MATKeybrands.cshtml";
        private const string SHAREBOARDCHANNEL = "~/Views/ShareBoard/_ShareBoard_Channel.cshtml";
        private static string MENU_URL = "StrawmanApp";
        private static string CONTROLLER_NAME = "ShareBoard";
        #endregion

    }
}
