using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class MarketViewChannelController : Controller
    {

        private static string _path = "~/Views/MarketViewChannel/";
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
            ViewBag.MarketViewChannelsPCVSPY = GetPCVSPYData();
            return PartialView(pcvspyview, GetDataViewData());
        }

        private dynamic GetPCVSPYData()
        {
            return GetData(Classes.StrawmanViews.PCVSPY);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_PCVSPY_CHANNELs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.StrawmanViewSTDModel { pcvspy1 = (decimal?)p.PCVSPY_COL1, pcvspy2 = (decimal?)p.PCVSPY_COL2, pcvspy3 = (decimal?)p.PCVSPY_COL3, pcvspy4 = (decimal?)p.PCVSPY_COL4, pcvspy5 = (decimal?)p.PCVSPY_COL5, vorder = p.ORDER, vgroup = p.GROUP, channel = p.CHANNEL };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOY() {
            
            ViewBag.MarketViewChannelsBOY = GetBOYData();

            return PartialView(boyview, GetDataViewData());
        }

        private dynamic GetBOYData()
        {
            return GetData(Classes.StrawmanViews.BOY);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_BOY_CHANNELs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.StrawmanViewSTDModel { _internal = (decimal?)p.INTERNAL, _le = (decimal?)p.LE, _pbp = (decimal?)p.PBP, vorder = p.ORDER, vgroup = p.GROUP, channel = p.CHANNEL };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalCustom() {
            ViewBag.MarketViewChannelsTotalCustom = GetTotalCustomData();
            return PartialView(totalcustomview, GetDataViewData());
        }

        private dynamic GetTotalCustomData()
        {
            return GetData(Classes.StrawmanViews.TOTAL);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_TOTAL_CUSTOM_CHANNELs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.StrawmanViewSTDModel { col1 = p.THREE_AGO, col2 = p.TWO_AGO, col3 = p.LAST, vorder = p.GROUP_ORDER, vgroup = p.GROUP, channel = p.CHANNEL };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBTG() {
            ViewBag.MarketViewChannelsBTG = GetBTGData();
            return PartialView(btgview, GetDataViewData());
        }

        private dynamic GetBTGData()
        {
            return GetData(Classes.StrawmanViews.BTG);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_BTG_CHANNELs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.StrawmanViewSTDModel { col1 = p.BTG_COL1, col2 = (decimal?)p.BTG_COL2, pcvspy = p.PCVSPY, vorder = p.GROUP_ORDER, vgroup = p.GROUP, channel = p.CHANNEL };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetYTD()
        {
            ViewBag.MarketViewChannelsYTD = GetYTDData();
            return PartialView(ytdview, GetDataViewData());
        }

        private dynamic GetYTDData()
        {
            return GetData(Classes.StrawmanViews.YTD);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_YTD_CHANNELs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.StrawmanViewSTDModel { col1 = p.YTD_COL1, col2 = p.YTD_COL2, pcvspy = p.PCVSPY, vorder = p.GROUP_ORDER, vgroup = p.GROUP, channel = p.CHANNEL };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMonth()
        {
            ViewBag.MarketViewChannelsMonth = GetData(Classes.StrawmanViews.MONTH);
            return PartialView(monthview, GetDataViewData());
        }

        private dynamic GetMonthData()
        {
            return GetData(Classes.StrawmanViews.MONTH);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_MONTH_CHANNELs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.StrawmanViewSTDModel { col1 = p.MONTH_COL1, col2 = p.MONTH_COL2, pcvspy = p.PCVSPY, vorder = p.GROUP_ORDER, vgroup = p.GROUP, channel = p.CHANNEL };
            //    return query.ToList();
            //}
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetMAT()
        {
            ViewBag.MarketViewChannelsMAT = GetMATData();

            return PartialView(matview, GetDataViewData());
        }

        private dynamic GetMATData()
        {
            return GetData(Classes.StrawmanViews.MAT);
            //using (Models.GodzillaWRKDataContext db = new Models.GodzillaWRKDataContext())
            //{
            //    var query = from p in db.WRK_MARKET_MAT_CHANNELs
            //                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
            //                select new Models.StrawmanViewSTDModel { col1 = p.MAT_COL1, col2 = p.MAT_COL2, pcvspy = p.PCVSPY, vorder = p.GROUP_ORDER, vgroup = p.GROUP, channel = p.CHANNEL };
            //    return query.ToList();
            //}
        }

        public ActionResult GetDataView()
        {
            return PartialView(dataview, GetDataViewData());
        }

        private dynamic GetDataViewData()
        {

            List<StrawmanDBLibray.Entities.GROUP_MASTER> lst = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            var = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_CHANNELS_COLORS)
                    .Select(m => m).ToList();
            List<Models.MarketViewChannelModels> aux =  lst
                .GroupJoin(var,l=> new{ID = l.ID},v=>new{ID = decimal.Parse(v.NAME)},(l,v) =>new{l=l,v=v})
                .Where(m => m.l.TYPE == 20).Distinct()
                .SelectMany(f => f.v.DefaultIfEmpty(), (l, v) => new { l = l.l, v = v }).ToList()
                .Select(p => new Models.MarketViewChannelModels { 
                                    name = p.l.NAME, 
                                    vorder = p.l.ID, 
                                    vid = p.l.ID, 
                                    vchannel = p.l.ID,
                                    style = p.v == null ? "": Helpers.StyleUtils.GetBGColor(p.v.VALUE,true),
                                    }).ToList();
            return aux;
            //using (Models.ChannelsDataClassesDataContext db = new Models.ChannelsDataClassesDataContext())
            //{
            //    var query = from p in db.v_WRK_CHANNEL_DATA
            //                select new Models.MarketViewChannelModels { name = p.NAME, vorder = p.ID, vid = p.ID, vchannel = p.ID };
            //    return query.ToList();
            //}
        }


        private dynamic GetData(string view)
        {
            dynamic ret = null;
            List<StrawmanDBLibray.Entities.GROUP_TYPES> grp = (List<StrawmanDBLibray.Entities.GROUP_TYPES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_TYPES, true);
            grp = grp.Where(m => m.ID == 20).Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> cfg = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG, true);
            cfg = cfg.Where(m => m.TYPE_ID == grp.First().ID).Distinct().Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> wc_channels = var.Where(m => m.VIEW == Classes.Default.Variables.WC_CHANNELS).Select(m => m).ToList();
            switch (view)
            {
                case Classes.StrawmanViews.MONTH:
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA> month_lst = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH, false);
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA> month_lst_wc = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH, true);
                    var month_data = month_lst.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d })
                        .Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.MONTH_COL1, col2 = p.c.MONTH_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    var month_data_wc = month_lst_wc.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d })
                        .Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.MONTH_COL1, col2 = p.c.MONTH_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    return month_data
                            .Join(month_data_wc, d=> new {d.channel, d.market, d.brand, d.vid}, w=>new{w.channel,w.market, w.brand, w.vid},(d,w)=>new{d=d,w=w})
                            .GroupBy(m=> new { m.d.vid })
                            .Select(p => new Models.StrawmanViewSTDModel 
                                            { 
                                                col1 = p.Sum(w=>w.d.col1), 
                                                col2 = p.Sum(w=>w.d.col2),
                                                pcvspy = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.d.col1), p.Sum(w => w.d.col2)), 
                                                col1_wc = p.Sum(w => w.w.col1),
                                                col2_wc = p.Sum(w => w.w.col2),
                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.w.col1), p.Sum(w => w.w.col2)), 
                                                vorder = p.Max(w=>w.d.vorder), 
                                                vgroup = p.Max(w=>w.d.vgroup), 
                                                channel = p.Max(w => w.d.channel),
                                                is_wc = wc_channels.Exists(m => m.VALUE == p.Max(w => w.d.channel).ToString()),
                                                vid = p.Key.vid
                                            }).ToList();
                case Classes.StrawmanViews.MAT:
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA> mat_lst = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT, false);
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA> mat_lst_wc = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_MAT_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT, true);
                    var mat_data = mat_lst.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d }).Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.MAT_COL1, col2 = (decimal?)p.c.MAT_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    var mat_data_wc = mat_lst_wc.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d })
                        .Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.MAT_COL1, col2 = (decimal?)p.c.MAT_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    return mat_data
                            .Join(mat_data_wc, d => new { d.channel, d.market, d.brand, d.vid }, w => new { w.channel, w.market, w.brand, w.vid }, (d, w) => new { d = d, w = w })
                            .GroupBy(m=> new { m.d.vid })
                            .Select(p => new Models.StrawmanViewSTDModel 
                                            { 
                                                col1 = p.Sum(w => w.d.col1), 
                                                col2 = p.Sum(w => w.d.col2), 
                                                pcvspy = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.d.col1), p.Sum(w => w.d.col2)),
                                                col1_wc = p.Sum(w => w.w.col1),
                                                col2_wc = p.Sum(w => w.w.col2),
                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.w.col1), p.Sum(w => w.w.col2)), 
                                                vorder = p.Max(w => w.d.vorder), 
                                                vgroup = p.Max(w => w.d.vgroup), 
                                                channel = p.Max(w => w.d.channel),
                                                is_wc = wc_channels.Exists(m => m.VALUE == p.Max(w => w.d.channel).ToString()),
                                                vid = p.Key.vid 
                                            }).ToList();
                case Classes.StrawmanViews.YTD:
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA> ytd_lst = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD, false);
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA> ytd_lst_wc = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD, true);
                    var ytd_data = ytd_lst.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d }).Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.YTD_COL1, col2 = (decimal?)p.c.YTD_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    var ytd_data_wc = ytd_lst_wc.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d })
                        .Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.YTD_COL1, col2 = (decimal?)p.c.YTD_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    return ytd_data
                            .Join(ytd_data_wc, d => new { d.channel, d.market, d.brand, d.vid }, w => new { w.channel, w.market, w.brand, w.vid }, (d, w) => new { d = d, w = w })
                            .GroupBy(m => new { m.d.vid })
                            .Select(p => new Models.StrawmanViewSTDModel 
                                            { 
                                                col1 = p.Sum(w => w.d.col1), 
                                                col2 = p.Sum(w => w.d.col2),
                                                pcvspy = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.d.col1), p.Sum(w => w.d.col2)),
                                                col1_wc = p.Sum(w => w.w.col1), 
                                                col2_wc = p.Sum(w => w.w.col2), 
                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.w.col1),p.Sum(w => w.w.col2)), 
                                                vorder = p.Max(w => w.d.vorder), 
                                                vgroup = p.Max(w => w.d.vgroup), 
                                                channel = p.Max(w => w.d.channel),
                                                is_wc = wc_channels.Exists(m => m.VALUE == p.Max(w => w.d.channel).ToString()),
                                                vid = p.Key.vid 
                                            }).ToList();
                case Classes.StrawmanViews.TOTAL:
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA> total_lst = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_TOTAL_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_TOTAL, false);
                    var total_data = total_lst.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d }).Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.THREE_AGO, col2 = p.c.TWO_AGO, col3 = p.c.LAST, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    return total_data.GroupBy(m => new { m.vid }).Select(p => new Models.StrawmanViewSTDModel { col1 = p.Sum(w => w.col1), col2 = p.Sum(w => w.col2), col3 = p.Sum(w => w.col3), vorder = p.Max(w => w.vorder), vgroup = p.Max(w => w.vgroup), channel = p.Max(w => w.channel), vid = p.Key.vid }).ToList();
                case Classes.StrawmanViews.BTG:
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA> btg_lst = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG, false);
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA> btg_lst_wc = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG, true);
                    var btg_data = btg_lst.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d }).Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.BTG_COL1, col2 = (decimal?)p.c.BTG_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    var btg_data_wc = btg_lst_wc.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d })
                        .Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.BTG_COL1, col2 = (decimal?)p.c.BTG_COL2, pcvspy = p.c.PCVSPY, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    return btg_data
                            .Join(btg_data_wc, d => new { d.channel, d.market, d.brand, d.vid }, w => new { w.channel, w.market, w.brand, w.vid }, (d, w) => new { d = d, w = w })
                            .GroupBy(m => new { m.d.vid })
                            .Select(p => new Models.StrawmanViewSTDModel    
                                            { 
                                                col1 = p.Sum(w => w.d.col1), 
                                                col2 = p.Sum(w => w.d.col2),
                                                pcvspy = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.d.col1), p.Sum(w => w.d.col2)),
                                                col1_wc = p.Sum(w => w.w.col1),
                                                col2_wc = p.Sum(w => w.w.col2),
                                                pcvspy_wc = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.w.col1), p.Sum(w => w.w.col2)),
                                                vorder = p.Max(w => w.d.vorder), 
                                                vgroup = p.Max(w => w.d.vgroup), 
                                                channel = p.Max(w => w.d.channel),
                                                is_wc = wc_channels.Exists(m => m.VALUE == p.Max(w => w.d.channel).ToString()),
                                                vid = p.Key.vid 
                                            }).ToList();
                case Classes.StrawmanViews.BOY:
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> boy_lst = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY, false);
                    var boy_data = boy_lst.Join(cfg, c => new { c.BRAND, c.MARKET }, d => new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d }).Select(p =>
                         new Models.StrawmanViewSTDModel { _internal = (decimal?)p.c.INTERNAL, _le = (decimal?)p.c.LE, _pbp = (decimal?)p.c.PBP, vorder = p.c.GROUP_ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    return boy_data.GroupBy(m => new { m.vid }).Select(p => new Models.StrawmanViewSTDModel { _internal = p.Sum(w => w._internal), _le = p.Sum(w => w._le), _pbp = p.Sum(w => w._pbp), vorder = p.Max(w => w.vorder), vgroup = p.Max(w => w.vgroup), channel = p.Max(w => w.channel), vid = p.Key.vid }).ToList();
                case Classes.StrawmanViews.PCVSPY:
                    List<StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA> pcvspy_lst = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_PCVSPY_DATA>)Helpers.Session.GetSessionData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_PCVSPY, false);
                    var pcvspy_data = pcvspy_lst.Join(cfg, c => new { c.BRAND, c.MARKET }, d=> new { d.BRAND, d.MARKET }, (c, d) => new { c = c, d = d }).Select(p =>
                         new Models.StrawmanViewSTDModel { col1 = p.c.THREE_AGO, col2 = p.c.TWO_AGO, col3 = p.c.LAST, col4 = p.c.CURRENT, _internal = (decimal?)p.c.INTERNAL, _le = (decimal?)p.c.LE, _pbp = (decimal?)p.c.PBP, vorder = p.c.ORDER, vgroup = p.c.GROUP, channel = p.c.CHANNEL, vid = p.d.GROUP_ID }
                        ).ToList();
                    return pcvspy_data.GroupBy(m => new { m.vid }).Select(p => new Models.StrawmanViewSTDModel { 
                                                                                        pcvspy1 = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.col1),p.Sum(w => w.col2)), 
                                                                                        pcvspy2 = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.col2),p.Sum(w => w.col3)),
                                                                                        pcvspy3 = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.col4),p.Sum(w => w._internal)),
                                                                                        pcvspy4 = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w.col3),p.Sum(w => w._le)),
                                                                                        pcvspy5 = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(w => w._le),p.Sum(w => w._pbp)), 
                                                                                        vorder = p.Max(w => w.vorder), 
                                                                                        vgroup = p.Max(w => w.vgroup), 
                                                                                        channel = p.Max(w => w.channel), 
                                                                                        vid = p.Key.vid }).ToList();
            }
            return ret;
        }

        public dynamic GetDataViewChannel()
        {
            return GetDataViewData();
        }
        [ChildActionOnly]
        public List<Models.StrawmanViewSTDModel> GetMarketViewData(string type)
        {
            return this.GetData(type);
        }

        //
        // GET: /MarketViewChannel/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MarketViewChannel/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MarketViewChannel/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MarketViewChannel/Create

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
        // GET: /MarketViewChannel/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MarketViewChannel/Edit/5

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
        // GET: /MarketViewChannel/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MarketViewChannel/Delete/5

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
