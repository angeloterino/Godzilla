using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanDBLibray.Classes;
using StrawmanApp.Classes;

namespace StrawmanApp.Controllers
{
    public class NTSViewController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetNTSView()
        {

            ViewBag.NTSData = (List<Models.StrawmanViewSTDModel>)GetData(NTSTables.WRK_NTS_VIEW_DATA);
            return PartialView(ViewPaths.NTSVIEW, (List<Models.MarketDataModels>)GetData(StrawmanDataTables.v_STRWM_MARKET_DATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetDataView()
        {
            return PartialView(ViewPaths.NTSVIEW, GetData(StrawmanDataTables.v_STRWM_MARKET_DATA));
        }
        private dynamic GetDataPBP()
        {
            return GetDataByType(NTSTypes.PBP);
        }
        private dynamic GetDataLE()
        {
            return GetDataByType(NTSTypes.LE);

        }
        private dynamic GetDataLE_WC()
        {
            return GetDataByType(NTSTypes.LE + NTSTypes._WC);

        }
        private dynamic GetDataPBP_WC()
        {
            return GetDataByType(NTSTypes.PBP + NTSTypes._WC);

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
            int _year = cad == null ? 0 : (int)cad;
            List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA> data = (List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)GetSessionData(NTSTables.WRK_NTS_VIEW_DATA);
            //if (NTSType.EndsWith(NTSTypes._WC))
            //{
            //    string NTSTypeWC = NTSType.Replace(NTSTypes._WC, "");
            //    List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA> data_wc = (List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)GetSessionData(NTSTables.WRK_NTS_VIEW_DATA, true);
            //    data_wc.Where(m => (m.YEAR_PERIOD == Helpers.PeriodUtil.Year - _year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month) && m.TYPE == NTSTypeWC).ToList().ForEach(i => i = new StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA 
            //                        { 
            //                            AMOUNT = i.AMOUNT, 
            //                            BRAND = i.BRAND, 
            //                            CHANNEL = i.CHANNEL, 
            //                            GROUP = i.GROUP, 
            //                            GROUP_ORDER = i.GROUP_ORDER, 
            //                            ID = i.ID, 
            //                            MARKET = i.MARKET, 
            //                            TYPE = i.TYPE, 
            //                            MONTH_PERIOD = Helpers.PeriodUtil.Month, 
            //                            YEAR_PERIOD = Helpers.PeriodUtil.Year - _year 
            //                        });
            //    return data.Where(m => (m.YEAR_PERIOD == Helpers.PeriodUtil.Year - _year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month) && m.TYPE == NTSType)
            //            .Select(p => new Models.StrawmanViewSTDModel
            //            {
            //                market = p.MARKET,
            //                brand = p.BRAND,
            //                channel = p.CHANNEL,
            //                col1 = data_wc.Find(m => m.TYPE == p.TYPE && m.CHANNEL == p.CHANNEL && m.MARKET == p.MARKET && m.BRAND == p.BRAND && m.YEAR_PERIOD == p.YEAR_PERIOD && m.MONTH_PERIOD == p.MONTH_PERIOD).AMOUNT,
            //                vgroup = p.GROUP,
            //                vorder = p.GROUP_ORDER
            //            }).ToList();
            //}
            return data.Where(m => (m.YEAR_PERIOD == Helpers.PeriodUtil.Year - _year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month) && m.TYPE == NTSType)
                        .Select(p => new Models.StrawmanViewSTDModel
                        {
                            market = p.MARKET,
                            brand = p.BRAND,
                            channel = p.CHANNEL,
                            col1 = p.AMOUNT,
                            vgroup = p.GROUP,
                            vorder = p.GROUP_ORDER
                        })
                        .ToList();

        }
        private dynamic GetData(string key)
        {
            switch (key)
            {
                case NTSTables.WRK_NTS_VIEW_DATA:
                    List<Models.StrawmanViewSTDModel> lst = null;
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.TWO_AGO), NTSColumns.COL1);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.LAST), NTSColumns.COL2);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataTOTAL(NTSPeriod.CURRENT), NTSColumns.COL3);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataINT(), NTSColumns.INT);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataLE(), NTSColumns.LE);
                    //lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataLE_WC(), NTSColumns.LE_WC);
                    lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataPBP(), NTSColumns.PBP);
                    //lst = (List<Models.StrawmanViewSTDModel>)SetDataView(lst, (List<Models.StrawmanViewSTDModel>)GetDataPBP_WC(), NTSColumns.PBP_WC);
                    List<Models.StrawmanViewSTDModel> lst_st = lst.FindAll(m => m.brand > 9000);
                    foreach (Models.StrawmanViewSTDModel st in lst_st)
                    {
                        Models.StrawmanViewSTDModel t = st;
                    }

                    return lst;
                default:
                    List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> data = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_BRAND_DATA, true);
                    List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
                    var = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_COLORS)
                            .Select(m => m).ToList();
                    List<Models.MarketDataModels> aux = data
                        .GroupJoin(var, l => new { ID = "BRAND:" + l.BRAND.ToString() + ";MARKET:" + l.MARKET.ToString() }, v => new { ID = v.NAME }, (l, v) => new { l = l, v = v })
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
                            style = p.v == null ? "" : Helpers.StyleUtils.GetBGColor(p.v.VALUE, true)
                        }).ToList();
                    return aux;
                    //return data//.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month || (m.YEAR_PERIOD == null && m.MONTH_PERIOD == null) && !m.SOURCE.Contains("MARKET"))
                    //            .Select(p => new Models.MarketDataModels
                    //            {
                    //                market = (decimal)p.MARKET,
                    //                brand = (decimal)p.BRAND,
                    //                channel = (decimal)p.CHANNEL,
                    //                brand_name = p.BRAND_NAME,
                    //                data = p.DATA,
                    //                market_name = p.NAME,
                    //                source = p.SOURCE,
                    //                vgroup = p.GROUP,
                    //                vorder = p.ORDER
                    //            })
                    //            .ToList();
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
                    Models.StrawmanViewSTDModel aux = data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market);
                    if (aux == null)
                        data.Add(new Models.StrawmanViewSTDModel
                        {
                            brand = item.brand,
                            channel = item.channel,
                            market = item.market,
                            col1 = 0,
                            col2 = 0,
                            col3 = 0, col4 = 0,col5 = 0, col6 = 0,
                        });
                    switch (type)
                    {
                        case NTSColumns.COL1:
                            data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market).col1 = item.col1;
                            break;
                        case NTSColumns.COL2:
                            data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market).col2 = item.col1;
                            break;
                        case NTSColumns.COL3:
                            data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market).col3 = item.col1;
                            break;
                        case NTSColumns.INT:
                            data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market)._internal = item.col1;
                            break;
                        case NTSColumns.LE:
                            data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market)._le = item.col1;
                            break;
                        case NTSColumns.PBP:
                            data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market)._pbp = item.col1;
                            break;
                        //case NTSColumns.LE_WC:
                        //    data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market)._lewc = item.col1;
                        //    break;
                        //case NTSColumns.PBP_WC:
                        //    data.Find(m => m.channel == item.channel && m.brand == item.brand && m.market == item.market)._pbpwc = item.col1;
                        //    break;
                    }
                }
            }
            return data;
        }
        private dynamic GetSessionData(string key, bool wc)
        {
            return Helpers.Session.GetSessionData(key, wc);
        }
        private dynamic GetSessionData(string key)
        {
            return GetSessionData(key, false);
        }
        //private dynamic GetSessionData(string key)
        //{

        //    if (Helpers.Session.GetSession(key) == null)
        //    {
        //        var obj = StrawmanDBLibray.DBLibrary.GetNTSData(key);
        //        Helpers.Session.SetSession(key, obj);             
        //    }
        //    return Helpers.Session.GetSession(key);
        //}
        

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
            public const string NTSVIEW = _PATH + "_NTSData.cshtml";
        }
        #endregion


    }
}
