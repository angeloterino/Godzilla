using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class MarketViewFranchiseController : Controller
    {

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetPCVSPY() {
            ViewBag.MarketViewFranchisePCVSPY = GetPCVSPYData();
            return PartialView(PCVSPYVIEW, (List<Models.MarketViewChannelModels>)GetDataViewData());
        }

        private dynamic GetPCVSPYData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> bdata = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY, true);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA> tdata = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_TOTAL, true);
            IEnumerable<StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA> data =
                bdata.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000)
                    .AsEnumerable()
                    .Join(  tdata.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                            ,b=>new { _channel = b.CHANNEL, _market = b.MARKET, _brand = b.BRAND}
                            ,t=>new { _channel = t.CHANNEL, _market = t.MARKET, _brand = t.BRAND}
                            ,(b,t) => new {b= b, t= t})
                    .AsEnumerable()
                    .Select(m=>new StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA{
                        CHANNEL = m.b.CHANNEL,
                        MARKET = m.b.MARKET,
                        BRAND = m.b.BRAND,
                        INTERNAL = m.b.INTERNAL,
                        LE = m.b.LE,
                        PBP = m.b.PBP,
                        THREE_AGO = m.t.THREE_AGO,
                        TWO_AGO = m.t.TWO_AGO,
                        LAST = m.t.LAST,
                        CURRENT = m.t.CURRENT
                    }).AsEnumerable();

            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable()
                    , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { col1 = p.d.THREE_AGO * p.m.CFG, col2 = p.d.TWO_AGO * p.m.CFG, col3 = p.d.LAST * p.m.CFG, col4 = (decimal?)p.d.INTERNAL * p.m.CFG, col5 = (decimal?)p.d.LE * p.m.CFG, col6 = (decimal?)p.d.PBP * p.m.CFG, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
                    .AsEnumerable();
            return GetFormatedData(q, Classes.StrawmanColumns.PCVSPY);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_PCVSPY_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { pcvspy1 = (decimal?)p.PCVSPY_COL1, pcvspy2 = (decimal?)p.PCVSPY_COL2, pcvspy3 = (decimal?)p.PCVSPY_COL3, pcvspy4 = (decimal?)p.PCVSPY_COL4, pcvspy5 = (decimal?)p.PCVSPY_COL5, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal) p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY() {
            ViewBag.MarketViewFranchiseBOY = GetBOYData();
            return PartialView(BOYVIEW, (List<Models.MarketViewChannelModels>)GetDataViewData());
        }

        private dynamic GetBOYData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable()
                    , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { col1 = (decimal?)p.d.INTERNAL * p.m.CFG, col2 = (decimal?)p.d.LE * p.m.CFG, col3 = (decimal?)p.d.PBP * p.m.CFG, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
                    .AsEnumerable();
            return GetFormatedData(q, Classes.StrawmanColumns.BOY);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_BOY_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { _internal = (decimal?)p.INTERNAL, _le = (decimal?)p.LE, _pbp = (decimal?)p.PBP, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom() {
            ViewBag.MarketViewFranchiseTotalCustom = GetTotalCustomData();
            return PartialView(TOTALCUSTOMVIEW, (List<Models.MarketViewChannelModels>)GetDataViewData());
        }

        private dynamic GetTotalCustomData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_TOTAL, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER,true);
            var ds = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable();
            var ms = mster.Where(m => m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable();
            var q = ds
                    .Join(ms
                    , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { col1 = p.d.THREE_AGO * p.m.CFG, col2 = (decimal?)p.d.TWO_AGO * p.m.CFG, col3 = (decimal?)p.d.LAST * p.m.CFG, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
                    .AsEnumerable();
            return GetFormatedData(q, Classes.StrawmanColumns.TOTAL);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_TOTAL_CUSTOM_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { col1 = p.THREE_AGO, col2 = p.TWO_AGO, col3 = p.LAST, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBTG() {
            ViewBag.MarketViewFranchiseBTG = GetBTGData();
            return PartialView(BTGVIEW, (List<Models.MarketViewChannelModels>)GetDataViewData());
        }

        private dynamic GetBTGData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG, false);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _market = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels 
                                        { 
                                            col1 = p.d.m.BTG_COL1 * p.m.CFG,
                                            col2 = (decimal?)p.d.m.BTG_COL2 * p.m.CFG,
                                            col1_wc = (GetChannelAsWC(p.m.CHANNEL) ? p.d.w.BTG_COL1 : p.d.m.BTG_COL1) * p.m.CFG,
                                            col2_wc = (decimal?)(GetChannelAsWC(p.m.CHANNEL) ? p.d.w.BTG_COL2 : p.d.m.BTG_COL2) * p.m.CFG, 
                                            col3 = 0, col4 = 0, col5 = 0, col6 = 0, 
                                            vid = (decimal)p.m.FRANCHISE_ID, 
                                            vparent = p.m.PARENT_ID 
                                        })
                    .AsEnumerable();
            return GetFormatedData(q, Classes.StrawmanColumns.BTG);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_BTG_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { col1 = (decimal?)p.COL1, col2 = (decimal?)p.COL2, pcvspy = (decimal?)p.PCVSPY, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetYTD()
        {
            ViewBag.MarketViewFranchiseYTD = GetYTDData();
            return PartialView(YTDVIEW, (List<Models.MarketViewChannelModels>)GetDataViewData());
        }

        private dynamic GetYTDData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD, false);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _market = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels 
                                            { 
                                                col1 = p.d.m.YTD_COL1 * p.m.CFG,
                                                col2 = (decimal?)p.d.m.YTD_COL2 * p.m.CFG,
                                                col1_wc = (GetChannelAsWC(p.m.CHANNEL) ? p.d.w.YTD_COL1 : p.d.m.YTD_COL1) * p.m.CFG,
                                                col2_wc = (decimal?)(GetChannelAsWC(p.m.CHANNEL) ? p.d.w.YTD_COL2 : p.d.m.YTD_COL2) * p.m.CFG, 
                                                col3 = 0, col4 = 0, col5 = 0, col6 = 0, 
                                                vid = (decimal)p.m.FRANCHISE_ID, 
                                                vparent = p.m.PARENT_ID 
                                            })
                    .AsEnumerable();
            return GetFormatedData(q, Classes.StrawmanColumns.YTD);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_YTD_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { col1 = p.COL1, col2 = p.COL2, pcvspy = p.PCVSPY, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMonth()
        {
            ViewBag.MarketViewFranchiseMonth = GetMonthData();
            List<Models.MarketViewChannelModels> model = (List<Models.MarketViewChannelModels>)GetDataViewData();
            return PartialView(MONTHVIEW, model);
        }

        private dynamic GetMonthData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH, false);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m=>m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _market = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels 
                                        { 
                                            col1 = p.d.m.MONTH_COL1 * p.m.CFG, 
                                            col2 = (decimal?)p.d.m.MONTH_COL2 * p.m.CFG,
                                            col1_wc = (GetChannelAsWC(p.m.CHANNEL) ? p.d.w.MONTH_COL1 : p.d.m.MONTH_COL1) * p.m.CFG,
                                            col2_wc = (decimal?)(GetChannelAsWC(p.m.CHANNEL) ? p.d.w.MONTH_COL2 : p.d.m.MONTH_COL2) * p.m.CFG, 
                                            col3 = 0, col4 = 0, col5 = 0, col6 = 0, 
                                            vid = (decimal)p.m.FRANCHISE_ID, 
                                            vparent = p.m.PARENT_ID 
                                        })
                    .AsEnumerable();
            return GetFormatedData(q, Classes.StrawmanColumns.MONTH);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_MONTH_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { col1 = p.COL1, col2 = p.COL2, pcvspy = p.PCVSPY, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMAT()
        {
            ViewBag.MarketViewFranchiseMAT = GetMATData();
            return PartialView(MATVIEW, (List<Models.MarketViewChannelModels>)GetDataViewData());
        }

        private dynamic GetMATData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT, false);
            List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m=>m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _market = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels 
                                        { 
                                            col1 = p.d.m.MAT_COL1 * p.m.CFG, 
                                            col2 = (decimal?)p.d.m.MAT_COL2 * p.m.CFG,
                                            col1_wc = (GetChannelAsWC(p.m.CHANNEL) ? p.d.w.MAT_COL1 : p.d.m.MAT_COL1) * p.m.CFG,
                                            col2_wc = (decimal?)(GetChannelAsWC(p.m.CHANNEL) ? p.d.w.MAT_COL2 : p.d.m.MAT_COL2) * p.m.CFG, 
                                            col3 = 0, 
                                            col4 = 0, 
                                            col5 = 0, 
                                            col6 = 0, 
                                            vid = (decimal)p.m.FRANCHISE_ID,
                                            vparent = p.m.PARENT_ID })
                    .AsEnumerable();// A partir de aquí, llamar a función pública que realiza el sumatorio de todas las columnas. Debe tener en cuenta:
                                   // - Tipo de Master (MARKET - BRAND)
                                   // - Tipo de columna: amount-amount-pc, amount-amount-amount o pc-pc-pc-pc-pc
            return GetFormatedData(q, Classes.StrawmanColumns.MAT);
        }

        public ActionResult GetDataView()
        {
            ViewBag.MarketViewFranchiseData = (List<Models.MarketViewChannelModels>)GetDataViewData();
            return PartialView(DATAVIEW);
        }

        private dynamic GetDataViewData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_DATA, true);
            return data.Select(p => new Models.MarketViewChannelModels { name = p.NAME, vorder = p.ORDER, vhas_child = p.HAS_CHILD, vparent = p.PARENT_ID, vid = (decimal)p.ID}).ToList();

        }


        #region Custom Functions

        private bool GetChannelAsWC(decimal? channel)
        {

            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> wc_channels = var.Where(m => m.VIEW == Classes.Default.Variables.WC_CHANNELS).Select(m => m).ToList();
            return wc_channels.Exists(w => w.VALUE == channel.ToString());
        }

        private void SetSessionObject(string key, object obj)
        {
            Helpers.Session.SetSession(key, obj);

        }

        private object GetSessionObject(string key)
        {
            return Helpers.Session.GetSession(key);
        }

        public List<Models.MarketViewChannelModels> GetFormatedData(object obj, string column) 
        { 
            var q = 
                ((IEnumerable<Models.MarketViewChannelModels>) obj)
                .GroupBy(m => new { m.vid })
                    .Select(m => new Models.MarketViewChannelModels
                    {
                        vparent = m.Max(p=>p.vparent),
                        vid = m.Key.vid,
                        col1 = m.Sum(p=>p.col1),
                        col2 = m.Sum(p=>p.col2),
                        col3 = m.Sum(p => p.col3),
                        col4 = m.Sum(p => p.col4),
                        col5 = m.Sum(p => p.col5),
                        col6 = m.Sum(p => p.col6),
                        col1_wc = m.Sum(p=>p.col1_wc),
                        col2_wc = m.Sum(p => p.col2_wc),
                    }).ToList();

            List<Models.MarketViewChannelModels> mdata = (List<Models.MarketViewChannelModels>)GetDataViewData();
            var cd = mdata
                        .Where(m => m.vparent != 0 && m.vhas_child != "Y").AsEnumerable();
            var g1 = cd
                        .Join(q, m => new { _id = (decimal)m.vid }, p => new { _id = (decimal)p.vid }, (m, p) => new { m = m, p = p })
                        .AsEnumerable()
                        .Select(m=>new Models.MarketViewChannelModels{
                            vparent = m.m.vparent,
                            vid = m.m.vid,
                            col1 = m.p.col1,
                            col2 = m.p.col2,
                            col3 = m.p.col3,
                            col4 = m.p.col4,
                            col5 = m.p.col5,
                            col6 = m.p.col6,
                            col1_wc = m.p.col1_wc,
                            col2_wc = m.p.col2_wc
                        }).AsEnumerable()
                        .GroupBy(m=>new {vid = (decimal)m.vparent})
                        .Select(m => new Models.MarketViewChannelModels
                        {
                            vparent = m.Key.vid,
                            vid = m.Key.vid,
                            col1 = m.Sum(p => p.col1),
                            col2 = m.Sum(p => p.col2),
                            col3 = m.Sum(p => p.col3),
                            col4 = m.Sum(p => p.col4),
                            col5 = m.Sum(p => p.col5),
                            col6 = m.Sum(p => p.col6),
                            col1_wc = m.Sum(p => p.col1_wc),
                            col2_wc = m.Sum(p => p.col2_wc),
                        }).ToList();
            var cd2 = mdata
                        .Where(m => m.vparent != 0 && m.vhas_child != "N").AsEnumerable();
            var g2 = cd2
                        .Join(g1, m => new { _id = m.vid }, p => new { _id = (decimal)p.vparent }, (m, p) => new { m = m, p = p })
                        .AsEnumerable()
                        .Select(m => new Models.MarketViewChannelModels
                        {
                            vparent = m.m.vparent,
                            vid = m.m.vid,
                            col1 = m.p.col1,
                            col2 = m.p.col2,
                            col3 = m.p.col3,
                            col4 = m.p.col4,
                            col5 = m.p.col5,
                            col6 = m.p.col6,
                            col1_wc = m.p.col1_wc,
                            col2_wc = m.p.col2_wc
                        }).AsEnumerable()
                        .GroupBy(m => new { vid = (decimal)m.vparent })
                        .Select(m => new Models.MarketViewChannelModels
                        {
                            vid = m.Key.vid,
                            vparent = m.Key.vid,
                            col1 = m.Sum(p => p.col1),
                            col2 = m.Sum(p => p.col2),
                            col3 = m.Sum(p => p.col3),
                            col4 = m.Sum(p => p.col4),
                            col5 = m.Sum(p => p.col5),
                            col6 = m.Sum(p => p.col6),
                            col1_wc = m.Sum(p => p.col1_wc),
                            col2_wc = m.Sum(p => p.col2_wc),
                        }).ToList();
            var cd3 = mdata
                        .Where(m => m.vparent == 0 && m.vhas_child != "N").AsEnumerable();
            var g3 = cd3
                        .Join(g2.Union(g1).AsEnumerable(), m => new { _id = m.vid }, p => new { _id = (decimal)p.vparent }, (m, p) => new { m = m, p = p })
                        .AsEnumerable()
                        .Select(m => new Models.MarketViewChannelModels
                        {
                            vparent = m.m.vparent,
                            vid = m.m.vid,
                            col1 = m.p.col1,
                            col2 = m.p.col2,
                            col3 = m.p.col3,
                            col4 = m.p.col4,
                            col5 = m.p.col5,
                            col6 = m.p.col6,
                            col1_wc = m.p.col1_wc,
                            col2_wc = m.p.col2_wc
                        }).AsEnumerable()
                        .GroupBy(m => new { vid = (decimal)m.vparent })
                        .Select(m => new Models.MarketViewChannelModels
                        {
                            vparent = m.Key.vid,
                            vid = m.Key.vid,
                            col1 = m.Sum(p => p.col1),
                            col2 = m.Sum(p => p.col2),
                            col3 = m.Sum(p => p.col3),
                            col4 = m.Sum(p => p.col4),
                            col5 = m.Sum(p => p.col5),
                            col6 = m.Sum(p => p.col6),
                            col1_wc = m.Sum(p => p.col1_wc),
                            col2_wc = m.Sum(p => p.col2_wc),
                        }).ToList();

            return GetDataForColumn(g3.Union(q).Union(g1).Union(g2).AsEnumerable(), column);
        }
        private List<Models.MarketViewChannelModels> GetDataForColumn(object obj, string column)
        {
            switch (column)
            {
                case Classes.StrawmanColumns.TOTAL:
                    return ((IEnumerable<Models.MarketViewChannelModels>)obj)
                            .GroupBy(m => new { vid = m.vid })
                                    .Select(m => new Models.MarketViewChannelModels
                                    {
                                        vid = m.Key.vid,
                                        vparent = m.Key.vid,
                                        col1 = m.Sum(p => p.col1),
                                        col2 = m.Sum(p => p.col2),
                                        col3 = m.Sum(p => p.col3),

                                    }).ToList();
                case Classes.StrawmanColumns.BOY:
                    return ((IEnumerable<Models.MarketViewChannelModels>)obj)
                            .GroupBy(m => new { vid = m.vid })
                                    .Select(m => new Models.MarketViewChannelModels
                                    {
                                        vid = m.Key.vid,
                                        vparent = m.Key.vid,
                                        _internal = m.Sum(p => p.col1),
                                        _le = m.Sum(p => p.col2),
                                        _pbp = m.Sum(p => p.col3),

                                    }).ToList();
                case Classes.StrawmanColumns.NTS:
                    return ((IEnumerable<Models.MarketViewChannelModels>)obj)
                            .GroupBy(m => new { vid = m.vid })
                                    .Select(m => new Models.MarketViewChannelModels
                                    {
                                        vid = m.Key.vid,
                                        vparent = m.Key.vid,
                                        col1 = m.Sum(p => p.col1),
                                        col2 = m.Sum(p => p.col2),
                                        col3 = m.Sum(p => p.col3),
                                        col4 = m.Sum(p => p.col4),
                                        col5 = m.Sum(p => p.col5),
                                        col6 = m.Sum(p => p.col6),
                                    }).ToList();
                case Classes.StrawmanColumns.PCVSPY:
                    //col1 = THREE AGO, col2 = TWO AGO, col3 = LAST, col4 = INTERNAL, col5 = LE, col6 = PBP
                    return ((IEnumerable<Models.MarketViewChannelModels>)obj)
                            .GroupBy(m => new { vid = m.vid })
                                    .Select(m => new Models.MarketViewChannelModels
                                    {
                                        vid = m.Key.vid,
                                        vparent = m.Key.vid,
                                        pcvspy1 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col1), m.Sum(p => p.col2)),
                                        pcvspy2 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col2), m.Sum(p => p.col3)),
                                        pcvspy3 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col3), m.Sum(p => p.col4)),
                                        pcvspy4 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col3), m.Sum(p => p.col5)),
                                        pcvspy5 = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col5), m.Sum(p => p.col6)),
                                    }).ToList();
                default:
                    //Classes.StrawmanColumns.MAT:
                    //Classes.StrawmanColumns.MONTH:
                    //Classes.StrawmanColumns.YTD:
                    //Classes.StrawmanColumns.BTG:
                    return
                        ((IEnumerable<Models.MarketViewChannelModels>)obj)
                            .GroupBy(m => new { vid = m.vid })
                                    .Select(m => new Models.MarketViewChannelModels
                                    {
                                        vid = m.Key.vid,
                                        vparent = m.Key.vid,
                                        col1 = m.Sum(p => p.col1),
                                        col2 = m.Sum(p => p.col2),
                                        pcvspy = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col1), m.Sum(p => p.col2)),
                                        col1_wc = m.Sum(p => p.col1_wc),
                                        col2_wc = m.Sum(p => p.col2_wc),
                                        pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(m.Sum(p => p.col1_wc), m.Sum(p => p.col2_wc)),

                                    }).ToList();
            }
        }

        #endregion
        #region Public Functions
        [ChildActionOnly]
        public List<Models.MarketViewChannelModels> GetMATFranchiseData()
        {
            return this.GetMATData();
        }
        [ChildActionOnly]
        public List<Models.MarketViewChannelModels> GetMonthFranchiseData()
        {
            return this.GetMonthData();
        }
        [ChildActionOnly]
        public List<Models.MarketViewChannelModels> GetYTDFranchiseData()
        {
            return this.GetYTDData();
        }
        [ChildActionOnly]
        public List<Models.MarketViewChannelModels> GetMarketViewData(string type)
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
        //
        // GET: /MarketViewFranchise/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MarketViewFranchise/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MarketViewFranchise/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MarketViewFranchise/Create

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
        // GET: /MarketViewFranchise/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MarketViewFranchise/Edit/5

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
        // GET: /MarketViewFranchise/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MarketViewFranchise/Delete/5

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
        private static string _PATH = "~/Views/MarketViewFranchise/";
        private string DATAVIEW = _PATH + "_MarketDataView.cshtml";
        private string MATVIEW = _PATH + "_Market_MAT.cshtml";
        private string MONTHVIEW = _PATH + "_Market_Month.cshtml";
        private string YTDVIEW = _PATH + "_Market_YTD.cshtml";
        private string BTGVIEW = _PATH + "_Market_BTG.cshtml";
        private string TOTALCUSTOMVIEW = _PATH + "_Market_TotalCustom.cshtml";
        private string BOYVIEW = _PATH + "_Market_BOY.cshtml";
        private string PCVSPYVIEW = _PATH + "_Market_PCVSPY.cshtml";
        private const string MARKET_VIEW_FRANCHISE_DATA = "MARKET_VIEW_FRANCHISE_DATA";
        
    }
}
