using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class BrandViewFranchiseController : Controller
    {

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetPCVSPY() {
            
            ViewBag.ViewFranchisePCVSPY = GetPCVSPYData();
            
            return PartialView(PCVSPYVIEW, GetSessionObject(BRAND_VIEW_FRANCHISE_DATA));
        }

        private dynamic GetPCVSPYData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA> bdata = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY, true);
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_TOTAL_DATA> tdata = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_TOTAL_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_TOTAL, true);
            IEnumerable<StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA> data =
                bdata.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000)
                    .AsEnumerable()
                    .Join(tdata.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                            , b => new { _channel = b.CHANNEL, _market = b.MARKET, _brand = b.BRAND }
                            , t => new { _channel = t.CHANNEL, _market = t.MARKET, _brand = t.BRAND }
                            , (b, t) => new { b = b, t = t })
                    .AsEnumerable()
                    .Select(m => new StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA
                    {
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
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
                    , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { col1 = p.d.THREE_AGO * p.m.CFG, col2 = p.d.TWO_AGO * p.m.CFG, col3 = p.d.LAST * p.m.CFG, col4 = (decimal?)p.d.INTERNAL * p.m.CFG, col5 = (decimal?)p.d.LE * p.m.CFG, col6 = (decimal?)p.d.PBP * p.m.CFG, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
                    .AsEnumerable();
            return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.PCVSPY);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_BRAND_PCVSPY_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { pcvspy1 = (decimal?)p.PCVSPY_COL1, pcvspy2 = (decimal?)p.PCVSPY_COL2, pcvspy3 = (decimal?)p.PCVSPY_COL3, pcvspy4 = (decimal?)p.PCVSPY_COL4, pcvspy5 = (decimal?)p.PCVSPY_COL5, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY() {
            
            ViewBag.ViewFranchiseBOY = GetBOYData();
            
            return PartialView(BOYVIEW, GetSessionObject(BRAND_VIEW_FRANCHISE_DATA));
        }

        private dynamic GetBOYData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
                    , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { col1 = (decimal?)p.d.INTERNAL * p.m.CFG, col2 = (decimal?)p.d.LE * p.m.CFG, col3 = (decimal?)p.d.PBP * p.m.CFG, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
                    .AsEnumerable();
            return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.BOY);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_BRAND_BOY_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { _internal = (decimal?)p.COL1, _le = (decimal?)p.COL2, _pbp = (decimal?)p.COL3, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom() {
            
            ViewBag.ViewFranchiseTotalCustom = GetTotalCustomData();
            
            return PartialView(TOTALCUSTOMVIEW, GetSessionObject(BRAND_VIEW_FRANCHISE_DATA));
        }

        private dynamic GetTotalCustomData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_TOTAL_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_TOTAL_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_TOTAL, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
                    , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { col1 = p.d.THREE_AGO * p.m.CFG, col2 = (decimal?)p.d.TWO_AGO * p.m.CFG, col3 = (decimal?)p.d.LAST * p.m.CFG, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
                    .AsEnumerable();
            return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.TOTAL);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_BRAND_TOTAL_CUSTOM_FRANCHISEs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.MarketViewChannelModels { col1 = p.COL1, col2 = p.COL2, col3 = p.COL3, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBTG() {
                        
            ViewBag.ViewFranchiseBTG = GetBTGData();
            
            return PartialView(BTGVIEW, GetSessionObject(BRAND_VIEW_FRANCHISE_DATA));
        }

        //private dynamic GetBTGData()
        //{
        //    List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BTG, true);
        //    List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
        //    var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
        //            .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
        //            , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
        //            .AsEnumerable()
        //            .Select(p => new Models.MarketViewChannelModels { col1 = p.d.BTG_COL1 * p.m.CFG, col2 = (decimal?)p.d.BTG_COL2 * p.m.CFG, col3 = 0, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
        //            .AsEnumerable();
        //    return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.BTG);
        //    //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
        //    //{
        //    //    var query = from p in db.WRK_BRAND_BTG_FRANCHISEs
        //    //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
        //    //                select new Models.MarketViewChannelModels { col1 = p.COL1, col2 = (decimal?)p.COL2, pcvspy = p.COL3, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
        //    //    return query.ToList();
        //    //}
        //}
        private dynamic GetBTGData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BTG, false);
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BTG, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _BRAND = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _BRAND = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels
                                        {
                                            col1 = p.d.m.BTG_COL1 * p.m.CFG,
                                            col2 = (decimal?)p.d.m.BTG_COL2 * p.m.CFG,
                                            col1_wc = (GetChannelAsWC(p.m.CHANNEL) ? p.d.w.BTG_COL1 : p.d.m.BTG_COL1) * p.m.CFG,
                                            col2_wc = (decimal?)(GetChannelAsWC(p.m.CHANNEL) ? p.d.w.BTG_COL2 : p.d.m.BTG_COL2) * p.m.CFG,
                                            col3 = 0,
                                            col4 = 0,
                                            col5 = 0,
                                            col6 = 0,
                                            vid = (decimal)p.m.FRANCHISE_ID,
                                            vparent = p.m.PARENT_ID
                                        })
                    .AsEnumerable();
            return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.BTG);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetYTD()
        {
            
            ViewBag.ViewFranchiseYTD = GetYTDData();
            
            return PartialView(YTDVIEW, GetSessionObject(BRAND_VIEW_FRANCHISE_DATA));
        }

        //private dynamic GetYTDData()
        //{
        //    List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_YTD, true);
        //    List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
        //    var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
        //            .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
        //            , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
        //            .AsEnumerable()
        //            .Select(p => new Models.MarketViewChannelModels { col1 = p.d.YTD_COL1 * p.m.CFG, col2 = (decimal?)p.d.YTD_COL2 * p.m.CFG, col3 = 0, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
        //            .AsEnumerable();
        //    return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.YTD);
        //    //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
        //    //{
        //    //    var query = from p in db.WRK_BRAND_YTD_FRANCHISEs
        //    //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
        //    //                select new Models.MarketViewChannelModels { col1 = p.COL1, col2 = p.COL2, pcvspy = p.COL3, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
        //    //    return query.ToList();
        //    //}
        //}
        private dynamic GetYTDData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_YTD, false);
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_YTD, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _BRAND = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _BRAND = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels
                                            {
                                                col1 = p.d.m.YTD_COL1 * p.m.CFG,
                                                col2 = (decimal?)p.d.m.YTD_COL2 * p.m.CFG,
                                                col1_wc = (GetChannelAsWC(p.m.CHANNEL) ? p.d.w.YTD_COL1 : p.d.m.YTD_COL1) * p.m.CFG,
                                                col2_wc = (decimal?)(GetChannelAsWC(p.m.CHANNEL) ? p.d.w.YTD_COL2 : p.d.m.YTD_COL2) * p.m.CFG,
                                                col3 = 0,
                                                col4 = 0,
                                                col5 = 0,
                                                col6 = 0,
                                                vid = (decimal)p.m.FRANCHISE_ID,
                                                vparent = p.m.PARENT_ID
                                            })
                    .AsEnumerable();
            return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.YTD);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMonth()
        {
            
            ViewBag.ViewFranchiseMonth = GetMonthData();
            
            return PartialView(MONTHVIEW, GetSessionObject(BRAND_VIEW_FRANCHISE_DATA));
        }

        //private dynamic GetMonthData()
        //{
        //    List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MONTH, true);
        //    List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
        //    var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
        //            .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
        //            , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
        //            .AsEnumerable()
        //            .Select(p => new Models.MarketViewChannelModels { col1 = p.d.MONTH_COL1 * p.m.CFG, col2 = (decimal?)p.d.MONTH_COL2 * p.m.CFG, col3 = 0, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
        //            .AsEnumerable();
        //    return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.MONTH);
        //    //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
        //    //{
        //    //    var query = from p in db.WRK_BRAND_MONTH_FRANCHISEs
        //    //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
        //    //                select new Models.MarketViewChannelModels { col1 = p.COL1, col2 = p.COL2, pcvspy = p.COL3, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
        //    //    return query.ToList();
        //    //}
        //}
        private dynamic GetMonthData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MONTH, false);
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MONTH, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _BRAND = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _BRAND = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels
                    {
                        col1 = p.d.m.MONTH_COL1 * p.m.CFG,
                        col2 = (decimal?)p.d.m.MONTH_COL2 * p.m.CFG,
                        col1_wc = (GetChannelAsWC(p.m.CHANNEL) ? p.d.w.MONTH_COL1 : p.d.m.MONTH_COL1) * p.m.CFG,
                        col2_wc = (decimal?)(GetChannelAsWC(p.m.CHANNEL) ? p.d.w.MONTH_COL2 : p.d.m.MONTH_COL2) * p.m.CFG,
                        col3 = 0,
                        col4 = 0,
                        col5 = 0,
                        col6 = 0,
                        vid = (decimal)p.m.FRANCHISE_ID,
                        vparent = p.m.PARENT_ID
                    })
                    .AsEnumerable();
            return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.MONTH);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMAT()
        {
            
            ViewBag.ViewFranchiseMAT = GetMATData();
            
            return PartialView(MATVIEW, GetSessionObject(BRAND_VIEW_FRANCHISE_DATA));
        }

        //private dynamic GetMATData()
        //{
        //    List<StrawmanDBLibray.Entities.v_WRK_BRAND_MAT_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_MAT_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MAT, true);
        //    List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
        //    var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
        //            .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
        //            , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
        //            .AsEnumerable()
        //            .Select(p => new Models.MarketViewChannelModels { col1 = p.d.MAT_COL1 * p.m.CFG, col2 = (decimal?)p.d.MAT_COL2 * p.m.CFG, col3 = 0, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
        //            .AsEnumerable();// A partir de aquí, llamar a función pública que realiza el sumatorio de todas las columnas. Debe tener en cuenta:
        //    // - Tipo de Master (MARKET - BRAND)
        //    // - Tipo de columna: amount-amount-pc, amount-amount-amount o pc-pc-pc-pc-pc
        //    return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.MAT);
        //    //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
        //    //{
        //    //    var query = from p in db.WRK_BRAND_MAT_FRANCHISEs
        //    //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
        //    //                select new Models.MarketViewChannelModels { col1 = p.COL1, col2 = p.COL2, pcvspy = p.COL3, vorder = p.ORDER, vparent = p.PARENT_ID, vid = (decimal)p.ID };
        //    //    return query.ToList();
        //    //}
        //}
        private dynamic GetMATData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_MAT_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_MAT_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MAT, false);
            List<StrawmanDBLibray.Entities.v_WRK_BRAND_MAT_DATA> dataWC = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_MAT_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MAT, true);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable()
                    .Join(dataWC.Where(m => m.BRAND < 9000 && m.MARKET < 9000).AsEnumerable(), m => new { m.BRAND, m.MARKET, m.CHANNEL }, w => new { w.BRAND, w.MARKET, w.CHANNEL }, (m, w) => new { m = m, w = w })
                    .AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.BRAND).AsEnumerable()
                    , d => new { _channel = d.m.CHANNEL, _BRAND = d.m.MARKET, _brand = d.m.BRAND }, m => new { _channel = m.CHANNEL, _BRAND = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
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
                        vparent = p.m.PARENT_ID
                    })
                    .AsEnumerable();// A partir de aquí, llamar a función pública que realiza el sumatorio de todas las columnas. Debe tener en cuenta:
            // - Tipo de Master (MARKET - BRAND)
            // - Tipo de columna: amount-amount-pc, amount-amount-amount o pc-pc-pc-pc-pc
            return new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.MAT);
        }
        public ActionResult GetDataView()
        {
            ViewBag.ViewFranchiseData = GetSessionObject(BRAND_VIEW_FRANCHISE_DATA, GetDataViewData());
            return PartialView(DATAVIEW);
        }

        private dynamic GetDataViewData()
        {
            using (Models.FranchiseDataClassesDataContext db = new Models.FranchiseDataClassesDataContext())
            {
                var query = from p in db.v_WRK_FRANCHISE_DATA
                            select new Models.MarketViewChannelModels { name = p.NAME, vorder = p.ORDER, vhas_child = p.HAS_CHILD, vparent = p.PARENT_ID, vid = (decimal)p.ID };
                return query.ToList();
            }
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

        private object GetSessionObject(string key, object obj)
        {
        
            if(Helpers.Session.GetSession(key) == null && obj != null)
                SetSessionObject(key, obj);
            return Helpers.Session.GetSession(key);
        }

        private object GetSessionObject(string key)
        {
            return GetSessionObject(key, null);
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
        public List<Models.MarketViewChannelModels> GetBrandViewData(string type)
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

        #region Default Functions

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
        #endregion  

        #region Constants
        private const string _PATH = "~/Views/BrandViewFranchise/";
        private const string DATAVIEW = _PATH + "_BrandDataView.cshtml";
        private const string MATVIEW = _PATH + "_Brand_MAT.cshtml";
        private const string MONTHVIEW = _PATH + "_Brand_Month.cshtml";
        private const string YTDVIEW = _PATH + "_Brand_YTD.cshtml";
        private const string BTGVIEW = _PATH + "_Brand_BTG.cshtml";
        private const string TOTALCUSTOMVIEW = _PATH + "_Brand_TotalCustom.cshtml";
        private const string BOYVIEW = _PATH + "_Brand_BOY.cshtml";
        private const string PCVSPYVIEW = _PATH + "_Brand_PCVSPY.cshtml";
        private const string BRAND_VIEW_FRANCHISE_DATA = "BRAND_VIEW_FRANCHISE_DATA";
        #endregion

    }
}
