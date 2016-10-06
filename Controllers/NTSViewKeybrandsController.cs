using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanDBLibray.Classes;
using StrawmanApp.Classes;

namespace StrawmanApp.Controllers
{
    public class NTSViewKeybrandsController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetNTSView()
        {
            ViewBag.NTSData = (List<Models.StrawmanViewSTDModel>)GetData(NTSTables.v_WRK_NTS_DATA_KEYBRANDS);
            return PartialView(ViewPaths.NTSVIEW, (List<Models.MarketViewChannelModels>)GetData(StrawmanDataTables.v_ENTITY_WRK_KEYBRANDS_BASE));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetDataView()
        {
            return PartialView(ViewPaths.NTSVIEW, GetData(StrawmanDataTables.v_ENTITY_WRK_KEYBRANDS_BASE));
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
            List<StrawmanDBLibray.Entities.v_WRK_NTS_DATA_KEYBRANDS> data = (List<StrawmanDBLibray.Entities.v_WRK_NTS_DATA_KEYBRANDS>)GetSessionData(NTSTables.v_WRK_NTS_DATA_KEYBRANDS);
            List<StrawmanDBLibray.Entities.v_WRK_NTS_DATA_CHANNEL> channel = (List<StrawmanDBLibray.Entities.v_WRK_NTS_DATA_CHANNEL>)GetSessionData(NTSTables.v_WRK_NTS_DATA_CHANNEL);
            data = data.Union(channel.Where(m=>m.ID>= 9).Select(m=>new StrawmanDBLibray.Entities.v_WRK_NTS_DATA_KEYBRANDS{
                 ROW_ID = long.Parse(m.ID.ToString()),
                 ID = m.ID,
                 GROUP = null,
                 AMOUNT = m.AMOUNT,
                 YEAR_PERIOD = (decimal)m.YEAR_PERIOD,
                 MONTH_PERIOD = (decimal)m.MONTH_PERIOD,
                 TYPE = m.TYPE,
            })).ToList();
            return data.Where(m => (m.YEAR_PERIOD == Helpers.PeriodUtil.Year - _year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month) && m.TYPE == NTSType)
                        .Select(p => new Models.StrawmanViewSTDModel
                        {
                            vid = p.ID,
                            vorder = p.ID,
                            vgroup = 0,
                            col1 = p.AMOUNT,
                            market = 0
                        })
                        .ToList();
        }
        private dynamic GetData(string key)
        {
            switch (key)
            {
                case NTSTables.v_WRK_NTS_DATA_KEYBRANDS:
                    List<Models.StrawmanViewSTDModel> lst = null;
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.TWO_AGO), NTSColumns.COL1);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.LAST), NTSColumns.COL2);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.CURRENT), NTSColumns.COL3);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataINT(), NTSColumns.INT);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataLE(), NTSColumns.LE);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataPBP(), NTSColumns.PBP);
                    return lst;
                default:
                    List<StrawmanDBLibray.Entities.v_ENTITY_WRK_KEYBRANDS_BASE> data = (List<StrawmanDBLibray.Entities.v_ENTITY_WRK_KEYBRANDS_BASE>)GetSessionData(StrawmanDataTables.v_ENTITY_WRK_KEYBRANDS_BASE);
                    return data.Where(p => p.DATA == StrawmanViews.MARKET && p.TYPE == StrawmanColumns.MAT && p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                                .Select(p => new Models.MarketViewChannelModels { name = p.NAME, vgroup = 0, vorder = (decimal)p.ID, vid = (decimal)p.ID }).ToList();
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
                    Models.StrawmanViewSTDModel data_item = data.Find(m => m.vgroup == item.vgroup && m.vid == item.vid);
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
            public const string NTSVIEW = _PATH + "_NTSDataKeybrands.cshtml";
        }
        #endregion

    }
}
