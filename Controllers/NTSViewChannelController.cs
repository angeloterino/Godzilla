using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanDBLibray.Classes;
using StrawmanApp.Classes;

namespace StrawmanApp.Controllers
{
    public class NTSViewChannelController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetNTSView()
        {
            ViewBag.NTSData = (List<Models.StrawmanViewSTDModel>)GetData(NTSTables.WRK_NTS_VIEW_DATA);
            return PartialView(ViewPaths.NTSVIEW, (List<Models.MarketViewChannelModels>)GetData(StrawmanDataTables.v_WRK_CHANNEL_DATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetDataView()
        {
            return PartialView(ViewPaths.NTSVIEW, GetData(StrawmanDataTables.v_WRK_CHANNEL_DATA));
        }
        private dynamic GetDataPBP()
        {
            return GetDataByType(NTSTypes.PBP);
        }
        private dynamic GetDataLE()
        {
            return GetDataByType(NTSTypes.LE);
        }
        private dynamic GetDataINT()
        {
            return GetDataByType(NTSTypes.INT);
        }
        private dynamic GetDataTOTAL(int cad)
        {
            return GetDataByType(NTSTypes.TOTAL, cad);
        }
        private dynamic GetDataByType(string NTSType)
        {
            return GetDataByType(NTSType, null);
        }
        private dynamic GetDataByType(string NTSType, int? cad)
        {
            int _year = cad == null? 0:(int)cad;
            List<StrawmanDBLibray.Entities.GROUP_TYPES> grp = (List<StrawmanDBLibray.Entities.GROUP_TYPES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_TYPES, true);
            grp = grp.Where(m => m.ID == 20).Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> cfg = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG, true);
            cfg = cfg.Where(m => m.TYPE_ID == grp.First().ID).Distinct().Select(m => m).ToList();
            //List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA> data = (List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)GetSessionData(NTSTables.WRK_NTS_VIEW_DATA);
            List<Models.StrawmanViewSTDModel> data = (List<Models.StrawmanViewSTDModel>)new NTSViewController().GetDataByNTSType(NTSType, cad);
            //data = data
            //        .Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year - _year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.TYPE == NTSType)
            //        .ToList();
            List<Models.StrawmanViewSTDModel> aux = data
                    .Join(cfg, c => new { _brand = c.brand, _market = c.market }, d => new { _brand = d.BRAND, _market = d.MARKET }, (c, d) => new { c = c, d = d })
                    .Select(p => new Models.StrawmanViewSTDModel
                    {
                        vid = p.d.GROUP_ID,
                        vorder = p.c.vorder,
                        vgroup = p.c.vgroup,
                        market = p.c.market,
                        brand = p.c.brand,
                        channel = p.c.channel,
                        col1 = p.c.col1
                    })
                    .ToList();
            return aux
                    .GroupBy(m => new { m.vid }).Select(p => new Models.StrawmanViewSTDModel { col1 = p.Sum(w => w.col1), vorder = p.Max(w => w.vorder), vgroup = p.Max(w => w.vgroup), channel = p.Max(w => w.channel), vid = p.Key.vid }).ToList();
                    
        }
        private dynamic GetData(string key)
        {
            switch(key){
                case NTSTables.WRK_NTS_VIEW_DATA:
                    List<Models.StrawmanViewSTDModel> lst = null;
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.TWO_AGO), NTSColumns.COL1);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.LAST), NTSColumns.COL2);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.CURRENT), NTSColumns.COL3);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataINT(), NTSColumns.INT);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataLE(), NTSColumns.LE);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataPBP(), NTSColumns.PBP);
                    return lst;
                default:
                    List<StrawmanDBLibray.Entities.GROUP_MASTER> data = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER, true);
                    List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
                    var = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_CHANNELS_COLORS)
                            .Select(m => m).ToList();
                    List<Models.MarketViewChannelModels> aux = data
                        .GroupJoin(var, l => new { ID = l.ID }, v => new { ID = decimal.Parse(v.NAME) }, (l, v) => new { l = l, v = v })
                        .Where(m => m.l.TYPE == 20).Distinct()
                        .SelectMany(f => f.v.DefaultIfEmpty(), (l, v) => new { l = l.l, v = v }).ToList()
                        .Select(p => new Models.MarketViewChannelModels
                        {
                            name = p.l.NAME,
                            vorder = p.l.ID,
                            vid = p.l.ID,
                            vchannel = p.l.ID,
                            style = p.v == null ? "" : Helpers.StyleUtils.GetBGColor(p.v.VALUE, true)
                        }).ToList();
                    return aux;
                    //return data.Where(m => m.TYPE == 20).Distinct().Select(p => new Models.MarketViewChannelModels { name = p.NAME, vorder = p.ID, vid = p.ID, vchannel = p.ID }).ToList();
                    //List<StrawmanDBLibray.Entities.GROUP_TYPES> grp = (List<StrawmanDBLibray.Entities.GROUP_TYPES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_TYPES, true);
                    //grp = grp.Where(m => m.ID == 20).Select(m => m).ToList();
                    //List<StrawmanDBLibray.Entities.GROUP_CONFIG> data = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG, true);
                    //List<StrawmanDBLibray.Entities.GROUP_CONFIG> aux = data.Where(m => m.TYPE_ID == grp.First().ID).Where(m=>m.BRAND>9000).Distinct().Select(m => m).ToList();
                    //return data.Where(m => m.TYPE_ID == grp.First().ID).Distinct().Select(p => new Models.MarketViewChannelModels { name = p.BRAND_NAME, vid = (decimal)p.GROUP_ID, vchannel = p.BRAND }).ToList();
                //List<StrawmanDBLibray.Entities.v_WRK_CHANNEL_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_CHANNEL_DATA>)GetSessionData(StrawmanDataTables.v_WRK_CHANNEL_DATA);
                //return data.Select(p => new Models.MarketViewChannelModels
                //            { 
                //                name = p.NAME,
                //                vorder = p.ID,
                //                vid = p.ID,
                //                vchannel = p.ID,
                //                vgroup = p.ID
                //            }).ToList();
            }
        }

        private dynamic SetDataView(List<Models.StrawmanViewSTDModel> data, List<Models.StrawmanViewSTDModel> source, string type)
        {
            if (data == null)
            {
                data = source;
            }
            else
            {
                foreach (Models.StrawmanViewSTDModel item in source)
                {
                    Models.StrawmanViewSTDModel data_item = data.Find(m => m.vid == item.vid);
                    switch (type)
                    {
                        case NTSColumns.COL1:
                            data_item.col1 = item.col1;
                            break;
                        case NTSColumns.COL2:
                            data_item.col2 = item.col1;
                            break;
                        case NTSColumns.COL3:
                            data_item.col3 = item.col1;
                            break;
                        case NTSColumns.INT:
                            data_item._internal = item.col1;
                            break;
                        case NTSColumns.LE:
                            data_item._le = item.col1;
                            break;
                        case NTSColumns.PBP:
                            data_item._pbp = item.col1;
                            break;
                    }
                }
            }
            return data;
        }

        private dynamic GetSessionData(string key)
        {

            if (Helpers.Session.GetSession(key) == null)
            {
                var obj = StrawmanDBLibray.DBLibrary.GetNTSData(key);
                Helpers.Session.SetSession(key, obj);
                
            }
            return Helpers.Session.GetSession(key);
        }
        

        //
        // GET: /NTSView/

        public ActionResult Index()
        {
            return View();
        }

        #region Constants
        private partial class ViewPaths
        {
            public const string _PATH = "~/Views/NTSView/";
            public const string NTSVIEW = _PATH + "_NTSDataChannel.cshtml";
        }
        #endregion

    }
}
