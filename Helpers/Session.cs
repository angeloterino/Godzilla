using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Helpers
{
    public class Session
    {
        internal static object GetSession(string key)
        {
            return System.Web.HttpContext.Current.Session[key];
        }

        internal static void SetSession(string key, object obj)
        {
            System.Web.HttpContext.Current.Session.Add(key, obj);
        }
        internal static dynamic GetSessionData(string key, bool wc)
        {
            string view = Classes.StrawmanViews.MARKET;
            if (key.Contains(Classes.StrawmanViews.BRAND))
                view = Classes.StrawmanViews.BRAND;
            else if (key.Contains(Classes.StrawmanViews.NTS))
                view = Classes.StrawmanViews.NTS;
            int _year = PeriodUtil.Year;
            int _month = PeriodUtil.Month;
            string session_key = key;
            //Comprobar si queremos datos WC
            if (wc)
            {
                //Marcamos el objeto como WC
                session_key = key + "_WC";
                //Buscamos datos del mes pasado
                _month = _month - 1;
                //Si el mes es 0 (es Enero), cambiamos a diciembre del año anterior
                if (_month <= 0)
                {
                    _month = 12;
                    _year = _year - 1;
                    //Si no es MONTH y buscamos Diciembre, tenemos que devolver solo el mes de Diciembre.
                    //TODO: Comprobar si para LE es necesario.
                    if (key != StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH)
                        key = StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH;
                }

            }
            if (!Session.CacheStatus) SetSession(session_key, null);
            if (GetSession(session_key) == null)
            {
                var obj = new object();
                switch (view)
                {
                    case Classes.StrawmanViews.BRAND:
                        obj = StrawmanDBLibray.DBLibrary.GetBrandData(key, _year, _month);
                        if (wc && PeriodUtil.Year == _year && key != StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MONTH)
                        {
                            var tmp = StrawmanDBLibray.DBLibrary.GetBrandData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MONTH, Helpers.PeriodUtil.Year - 1, 12);
                            //TODO:Sumar al obj el mes de diciembre del año anterior (tmp)
                            //YTD
                            switch(key){
                                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_YTD:
                                    ((List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA>)obj).ForEach(i => {
                                        i.YTD_COL2 = i.YTD_COL2 + (((List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA>)tmp).Where(s => s.MARKET == i.MARKET && s.CHANNEL == i.CHANNEL && s.BRAND == i.BRAND).FirstOrDefault().MONTH_COL2);
                                        i.YTD_COL1 = i.YTD_COL1 + (((List<StrawmanDBLibray.Entities.v_WRK_BRAND_MONTH_DATA>)tmp).Where(s => s.MARKET == i.MARKET && s.CHANNEL == i.CHANNEL && s.BRAND == i.BRAND).FirstOrDefault().MONTH_COL1);
                                    });
                                break;
                                //case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY:
                                //    List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA> ytd = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_YTD_DATA>)GetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_YTD + "_WC");
                                //    List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA> pbp = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA>)GetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY);
                                //    List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA> btg = (List<StrawmanDBLibray.Entities.v_WRK_BRAND_BTG_DATA>)GetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BTG);
                                //    ((List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA>)obj).ForEach(i => i.LE = 
                                //        (double?)ytd.Find(m=>m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).YTD_COL2
                                //        + btg.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).BTG_COL2
                                //        );
                                //    ((List<StrawmanDBLibray.Entities.v_WRK_BRAND_BOY_DATA>)obj).ForEach(i => i.PBP =
                                //        (double?)i.LE * (
                                //         pbp.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).LE == 0 ? 0 :
                                //         pbp.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).PBP /
                                //         pbp.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).LE)
                                //        );
                                //break;
                            }
                        }
                        break;
                    case Classes.StrawmanViews.NTS:
                        obj = StrawmanDBLibray.DBLibrary.GetNTSData(key);
                    //    if (wc)
                    //    {
                    //        List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA> nts = (List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)GetSession(StrawmanDBLibray.Classes.NTSTables.WRK_NTS_VIEW_DATA);
                    //        if (nts != null)
                    //        {
                    //            List<StrawmanDBLibray.Entities.WRK_BOY_DATA> boy = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BOY_DATA, true);
                    //            List<StrawmanDBLibray.Entities.WRK_BOY_DATA> ytd = boy.Where(m => m.YEAR_PERIOD == _year && m.MONTH_PERIOD == _month && m.TYPE == Classes.BOYTypes.YTD).ToList();
                    //            List<StrawmanDBLibray.Entities.WRK_BOY_DATA> pbp = boy.Where(m => m.YEAR_PERIOD == _year && m.MONTH_PERIOD == _month && m.TYPE == Classes.BOYTypes.PBP).ToList();
                    //            List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA> aux = ((List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)obj).Where(m => m.YEAR_PERIOD == PeriodUtil.Year && m.MONTH_PERIOD == PeriodUtil.Month && m.TYPE == Classes.NTSTypes.LE).ToList();
                    //            ((List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)obj).Where(m => m.YEAR_PERIOD == PeriodUtil.Year && m.MONTH_PERIOD == PeriodUtil.Month && m.TYPE == Classes.NTSTypes.LE).ToList()
                    //                .ForEach(i => i.AMOUNT =
                    //                    (ytd.Exists(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL)
                    //                     && pbp.Exists(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL))?
                    //                    ytd.Find(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL ).SELLIN_COL2
                    //                    + pbp.Find(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL).SELLIN_COL2: i.AMOUNT
                    //                );
                    //            List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>le = ((List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)obj).Where(m => m.YEAR_PERIOD == _year && m.MONTH_PERIOD == _month && m.TYPE == Classes.NTSTypes.LE).ToList();
                    //            List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA> aux2 = ((List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)obj).Where(m => m.YEAR_PERIOD == _year && m.MONTH_PERIOD == _month && m.TYPE == Classes.NTSTypes.PBP).ToList();
                    //            ((List<StrawmanDBLibray.Entities.WRK_NTS_VIEW_DATA>)obj).Where(m => m.YEAR_PERIOD == _year && m.MONTH_PERIOD == _month && m.TYPE == Classes.NTSTypes.PBP).ToList()
                    //                .ForEach(i => i.AMOUNT =
                    //                    (le.Exists(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL)
                    //                    && pbp.Exists(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL)) ?
                    //                    le.Find(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL).AMOUNT
                    //                    * pbp.Find(m => m.BRAND == i.BRAND && m.MARKET == i.MARKET && m.CHANNEL == i.CHANNEL).SELLIN_PC:i.AMOUNT
                    //                );
                    //        }
                    //    }
                        break;
                    default:
                        obj = StrawmanDBLibray.DBLibrary.GetMarketData(key, _year, _month);
                        //Para MARKET
                        //Si es WC y el año es el mismo que el actual, y no es MONTH, tenemos que añadir al resultado los datos de MONTH de Diciembre del año anterior
                        //TODO: Comprobar para LE si es necesario
                        //TODO: para datos anteriores al último año con datos debe devolver 0, no nulo.
                        if (wc && PeriodUtil.Year == _year && key != StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH)
                        {
                            var tmp = StrawmanDBLibray.DBLibrary.GetMarketData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH, Helpers.PeriodUtil.Year - 1, 12);
                            //TODO:Sumar al obj el mes de diciembre del año anterior (tmp)
                            //YTD
                            switch(key){
                                case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD:
                                    ((List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)obj).ForEach(i => { 
                                        i.YTD_COL2 = i.YTD_COL2 + (((List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)tmp).Where(s => s.MARKET == i.MARKET && s.CHANNEL == i.CHANNEL && s.BRAND == i.BRAND).FirstOrDefault().MONTH_COL2);
                                        i.YTD_COL1 = i.YTD_COL1 + (((List<StrawmanDBLibray.Entities.v_WRK_MARKET_MONTH_DATA>)tmp).Where(s => s.MARKET == i.MARKET && s.CHANNEL == i.CHANNEL && s.BRAND == i.BRAND).FirstOrDefault().MONTH_COL1);
                                    });
                                break;
                                //case StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY:
                                //    List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA> ytd = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_YTD_DATA>)GetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD + "_WC");
                                //    List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA> pbp = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)GetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY);
                                //    List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA> btg = (List<StrawmanDBLibray.Entities.v_WRK_MARKET_BTG_DATA>)GetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG);
                                //    ((List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)obj).ForEach(i => i.LE = 
                                //        (double?)ytd.Find(m=>m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).YTD_COL2
                                //        + btg.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).BTG_COL2
                                //        );
                                //    ((List<StrawmanDBLibray.Entities.v_WRK_MARKET_BOY_DATA>)obj).ForEach(i => i.PBP =
                                //        (double?)i.LE * (
                                //         pbp.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).LE == 0?0:
                                //         pbp.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).PBP /
                                //         pbp.Find(m => m.CHANNEL == i.CHANNEL && m.BRAND == i.BRAND && i.MARKET == i.MARKET).LE)
                                //        );
                                //break;
                            }
                        }
                        break;
                }
                SetSession(session_key, obj);
            }
            return GetSession(session_key);
        }

        public static bool CacheStatus
        {
            get { return cache_status; }
            set { cache_status = value; }
        }

        private static bool cache_status = true;
    }
}