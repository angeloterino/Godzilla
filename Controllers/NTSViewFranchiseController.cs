using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanDBLibray.Classes;
using StrawmanApp.Classes;

namespace StrawmanApp.Controllers
{
    public class NTSViewFranchiseController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetNTSView()
        {
            ViewBag.NTSData = (List<Models.StrawmanViewSTDModel>)GetData(NTSTables.v_WRK_NTS_DATA_FRANCHISE);
            return PartialView(ViewPaths.NTSVIEW, (List<Models.MarketViewChannelModels>)GetData(StrawmanDataTables.v_WRK_FRANCHISE_DATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetDataView()
        {
            return PartialView(ViewPaths.NTSVIEW, GetData(StrawmanDataTables.v_WRK_FRANCHISE_DATA));
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
            List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA> data = (List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)GetSessionData(NTSTables.WRK_NTS_VIEW_DATA);
            List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER> mster = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_FRANCHISE_MASTER, true);
            var q = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year - _year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.BRAND < 9000 && m.MARKET < 9000 && m.TYPE == NTSType).AsEnumerable()
                    .Join(mster.Where(m => m.TYPE == Classes.StrawmanViews.MARKET).AsEnumerable()
                    , d => new { _channel = d.CHANNEL, _market = d.MARKET, _brand = d.BRAND }, m => new { _channel = m.CHANNEL, _market = m.MARKET, _brand = (decimal?)m.BRAND }, (d, m) => new { d = d, m = m })
                    .AsEnumerable()
                    .Select(p => new Models.MarketViewChannelModels { col1 = (decimal?)p.d.AMOUNT * p.m.CFG, col2 = 0, col3 = 0, col4 = 0, col5 = 0, col6 = 0, vid = (decimal)p.m.FRANCHISE_ID, vparent = p.m.PARENT_ID })
                    .AsEnumerable();
            var fdata =  new MarketViewFranchiseController().GetFormatedData(q, Classes.StrawmanColumns.NTS);
            //List<StrawmanDBLibray.Entities.v_WRK_NTS_DATA_FRANCHISE> data = (List<StrawmanDBLibray.Entities.v_WRK_NTS_DATA_FRANCHISE>)GetSessionData(NTSTables.v_WRK_NTS_DATA_FRANCHISE);
            return fdata
                        .Select(p => new Models.StrawmanViewSTDModel
                        {
                            vid = p.vid,
                            vorder = p.vorder,
                            vparent = p.vparent,
                            col1 = p.col1
                        })
                        .ToList();
        }
        private dynamic GetData(string key)
        {
            switch (key)
            {
                case NTSTables.v_WRK_NTS_DATA_FRANCHISE:
                    List<Models.StrawmanViewSTDModel> lst = null;
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.TWO_AGO), NTSColumns.COL1);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.LAST), NTSColumns.COL2);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.CURRENT), NTSColumns.COL3);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataINT(), NTSColumns.INT);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataLE(), NTSColumns.LE);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataPBP(), NTSColumns.PBP);
                    return lst;
                default:
                    List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_DATA>)GetSessionData(StrawmanDataTables.v_WRK_FRANCHISE_DATA);
                    return data.Select(p => new Models.MarketViewChannelModels { name = p.NAME, vorder = p.ORDER, vhas_child = p.HAS_CHILD, vparent = p.PARENT_ID, vid = (decimal)p.ID }).ToList();
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
                    Models.StrawmanViewSTDModel data_item = data.Find(m => m.vorder == item.vorder && m.vid == item.vid && m.vparent == item.vparent);
                    if (data_item == null) data_item = new Models.StrawmanViewSTDModel { brand = item.brand, market = item.market, channel = item.channel, vorder = item.vorder, vid = item.vid, vparent = item.vparent }; 
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
            public const string NTSVIEW = _PATH + "_NTSDataFranchise.cshtml";
        }
        #endregion

    }
}
