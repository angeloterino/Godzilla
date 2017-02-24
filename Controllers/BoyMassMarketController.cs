using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanApp.Helpers;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class BoyMassMarketController : Controller
    {
        [Authorize]
        public JsonResult GetBOYJJByChannelVars()
        {
            var _paths = "BOYJJByChannel";
            var _viewId = "BoyViews";
            var _control = "GetTotalByChannelType"; 
            string[] _pathboys = {@"/BoyMassMarket/"};
            string[] _controls = {"GetBoyData", "GetBoyYTD", "GetBoyTOGO", "GetBoyTotals", "GetBoyINT", "GetBoyLE", "GetBoyPBP"};
            string[] _viewsids = { "_BoyData", "_BoyYTD", "_BoyTOGO", "_BoyTotals", "_BoyINT", "_BoyLE", "_BoyPBP" };
            string[] _channel = {"MASS", "OTC", "BEAUTY", "TOTAL"};
            //Vamos a enviar los canales configurados por el grupo BOYJJByChannel (tipo 22);
            List<StrawmanDBLibray.Entities.GROUP_MASTER> channels = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER,true);
            decimal? _type = StrawmanCalcs.GetGroupTypeByView(BY_CHANNEL_CONTROLLER);
            if (_type != null)
            {
                _channel = channels.Where(m => m.TYPE == _type).Select(m => m.ID.ToString()).ToArray();
            }

            return Json(new {_paths = _paths, _viewId = _viewId, _control = _control, _pathboys = _pathboys, _controls = _controls, _viewsids = _viewsids, _channel = _channel }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetTotalByChannelType(string _channel, string _type)
        {
            ViewBag.BoyData = GetTotalData(_type, _channel);
            ViewBag.BoyCalc = ViewBag.BoyData;
            ViewBag.BoyYTD = ViewBag.BoyData;
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            string view = BOYDATA;
            if (_type != "GetBoyData")
            {
                switch (GetDataType(_type))
                {
                    case "YTD":
                        view = BOYYTD;
                        break;
                    case "TOTAL":
                        view = BOYTOTALS;
                        break;
                    case "TOGO":
                        view = BOYTOGO;
                        break;
                    case "INT":
                        view = BOYINT;
                        break;
                    case "LE":
                        view = BOYLE;
                        break;
                    case "PBP":
                        view = BOYPBP;
                        break;
                    default:
                        view = BOYDATA;
                        break;
                }
            }
            return PartialView(view,GetSessionData(_channel +"YTDData"));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyData(string chan)
        {
            FormsController.ResetFormBOYSession();
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year; 
            SetSessionData(BOYYTDDATA, GetBoyYTDData("YTD").Where(m=>m.channel == channel).ToList());
            ViewBag.BoyData = GetSessionData(BOYYTDDATA);

            Session.Add("YTDData", ViewBag.BoyData);
            return PartialView(BOYDATA);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyYTD(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyYTD = GetBoyYTDData("YTD").Where(m => m.channel == channel).ToList();
            //if (Session["YTDData"] != null) BoyYTDData = (List<Models.BoyMassMarketModels>)Session["YTDData"];
            return PartialView(BOYYTD, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyTotals(string chan)
        {
            
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyYTD = GetBoyYTDData("TOTAL").Where(m => m.channel == channel).ToList();
            return PartialView(BOYTOTALS,GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyTOGO(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyYTD = GetBoyYTDData("TOGO").Where(m => m.channel == channel).ToList();
            return PartialView(BOYTOGO, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyINT(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCALC = GetBoyCalcData("INT").Where(m => m.channel == channel).ToList();
            return PartialView(BOYINT, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyPBP(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCALC = GetBoyCalcData("PBP").Where(m => m.channel == channel).ToList();
            return PartialView(BOYPBP, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyLE(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCALC = GetBoyCalcData("LE").Where(m => m.channel == channel).ToList();
            return PartialView(BOYLE, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyINTCustom(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCustom = GetBoyCalcCustomData("INT").Where(m => m.channel == channel).ToList();
            ViewBag.TableTitle = Helpers.PeriodUtil.Year.ToString() + " Int";
            return PartialView(BOYCUSTOM, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyPBPCustom(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCustom = GetBoyCalcCustomData("PBP").Where(m => m.channel == channel).ToList();
            ViewBag.TableTitle = (Helpers.PeriodUtil.Year + 1).ToString() + " PBP";
            return PartialView(BOYCUSTOM, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyLECustom(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCustom = GetBoyCalcCustomData("LE").Where(m => m.channel == channel).ToList();
            ViewBag.TableTitle = "BTG " + Helpers.PeriodUtil.Year.ToString("YY");
            ViewBag.TableTitle2 = Helpers.PeriodUtil.Year.ToString() + " LE";
            return PartialView(BOYCUSTOM, GetSessionData(BOYYTDDATA));
        }


        private dynamic GetTotalData(string _type, string _channel)
        {
            List<Models.BoyMassMarketModels> lst = new List<Models.BoyMassMarketModels>();
            string key = _channel +"YTDData";
            int _group = 0;
            if (!int.TryParse(_channel, out _group))//Comprobamos que el grupo no viaje en el canal
            {
                switch (_channel)
                {
                    case "MASS":
                        channel = 1;
                        break;
                    case "BEAUTY":
                        channel = 2;
                        break;
                    case "OTC":
                        channel = 3;
                        break;
                    default:
                        channel = 999999;
                        break;
                }

                switch (_type)
                {
                    case "GetBoyData":
                    case "GetBoyYTD":
                        if (GetSessionData(key) == null)
                        {
                            Session.Add(key, GetBOYByChannelData(GetDataType(_type)).Select(m => m).ToList());
                        }
                        lst = GetSessionData(key);
                        break;
                    case "GetBoyTOGO":
                    case "GetBoyTotals":
                        lst = GetBOYByChannelData(GetDataType(_type)).Select(m => m).ToList();
                        break;
                    case "GetBoyINT":
                    case "GetBoyLE":
                    case "GetBoyPBP":
                        lst = GetBOYByChannelData(GetDataType(_type)).Select(m => m).ToList();
                        break;
                    case "GetBoyINTCustom":
                    case "GetBoyLECustom":
                    case "GetBoyPBPCustom":
                        lst = GetBOYByChannelData(GetDataType(_type)).Select(m => m).ToList();
                        break;
                }


            }
            else
            {
                SetGroupType(_group);//Llamamos a la función que establece el grupo según configuración en WRK_VARIABLES
                if (group_type != null)
                {
                    //Está definido el grupo, enviamos la información configurada
                    string type = GetDataType(_type);
                    switch (type)
                    {
                        case "INT":
                        case "LE":
                        case "PBP":
                            lst =  GetBoyCalcData(type).Where(m=>m._id == group_type).Select(m=>m).ToList();
                            break;
                        default:
                            lst = GetBoyYTDData(type).Where(m => m._id == group_type).Select(m => m).ToList();
                            break;
                    }
                    Helpers.Session.SetSession(key, lst);
                }
            }
            return lst;
        }

        private List<Models.BoyMassMarketModels> GetBOYByChannelData(string _type)
        {
            List<Models.BoyMassMarketModels> lst = new List<Models.BoyMassMarketModels>();
            string type = "";
            switch (_type)
            {
                case "":
                    type = "YTD";
                    break;
                default:
                    type = _type.Replace("GetBoy", "").ToUpper();
                    break;
            }
            switch (type)
            {
                case "INT":
                        
                    List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_CALC> lint = (List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_CALC>)GetSessionDataTable("v_WRK_BOY_BY_CHANNEL_CALC");
                    var i = lint
                                .Where(m => m.TYPE == type && m.CHANNEL == channel
                                && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.CHANNEL)
                                .Select(p => new Models.BoyMassMarketModels
                    {
                        channel = p.CHANNEL,
                        market_col1 = (double?)p.MARKET_COL1,
                        market_pc = p.MARKET_CHGE,
                        sellin_col1 = (double?)p.SELLIN_COL1,
                        sellin_pc = p.SELLIN_CHGE,
                        sellout_col1 = (double?)p.SELLOUT_COL1,
                        sellout_pc = p.SELLOUT_CHGE,
                        type = p.TYPE,
                        share_col1 = p.SHARE_COL1,
                        share_pc = p.SHARE_CHGE,
                        conversion_rate = null,
                        brand = 9999,
                        market = 9999,
                    });
                    lst = i.ToList();
                    break;
                case "LE":
                    if (group_type != null)
                    {
                        //Está definido el grupo, enviamos la información configurada
                        return GetBoyCalcData(type).ToList();
                    }
                    List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_CALC> lle = (List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_CALC>)GetSessionDataTable("v_WRK_BOY_BY_CHANNEL_CALC");
                    var l = lle
                            .Where(m => m.TYPE == type && m.CHANNEL == channel
                                && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.CHANNEL)
                            .Select(p => new Models.BoyMassMarketModels
                    {
                        channel = p.CHANNEL,
                        market_col1 = (double?)p.MARKET_COL1,
                        market_col2 = (double?)p.MARKET_COL2,
                        market_pc = p.MARKET_CHGE,
                        market_pc_int = p.MARKET_VS_INT,
                        sellin_col1 = (double?)p.SELLIN_COL1,
                        sellin_col2 = (double?)p.SELLIN_COL2,
                        sellin_pc_int = p.SELLIN_VS_INT,
                        sellin_pc = p.SELLIN_CHGE,
                        sellout_col1 = (double?)p.SELLOUT_COL1,
                        sellout_col2 = (double)p.SELLOUT_COL2,
                        sellout_pc_int = p.SELLOUT_VS_INT,
                        sellout_pc = p.SELLOUT_CHGE,
                        type = p.TYPE,
                        share_col1 = p.SHARE_COL1,
                        share_col2 = null,
                        share_pc = p.SHARE_CHGE,
                        share_pc_int = p.SHARE_VS_INT,
                        conversion_rate = null,
                        brand = 9999,
                        market = 9999,
                    });
                    lst = l.ToList();
                    break;
                case "PBP":
                    if (group_type != null)
                    {
                        //Está definido el grupo, enviamos la información configurada
                        return GetBoyCalcData(type).ToList();
                    }
                    List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_CALC> lpbp = (List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_CALC>)GetSessionDataTable("v_WRK_BOY_BY_CHANNEL_CALC");
                    var b = lpbp
                        .Where(m => m.TYPE == type && m.CHANNEL == channel
                                && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.CHANNEL)
                        .Select(p => new Models.BoyMassMarketModels
                    {
                        channel = p.CHANNEL,
                        market_col1 = (double?)p.MARKET_COL1,
                        market_pc = p.MARKET_CHGE,
                        sellin_col1 = (double?)p.SELLIN_COL1,
                        sellin_pc = p.SELLIN_CHGE,
                        sellout_col1 = (double?)p.SELLOUT_COL1,
                        sellout_pc = p.SELLOUT_CHGE,
                        type = p.TYPE,
                        share_col1 = p.SHARE_COL1,
                        share_pc = p.SHARE_CHGE,
                        conversion_rate = null,
                        brand = 9999,
                        market = 9999,
                    });
                    lst = b.ToList();
                    break;
                default:
                    if (group_type != null)
                    {
                        //Está definido el grupo, enviamos la información configurada
                        return GetBoyYTDData(type).ToList();
                    }
                    List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_GENERAL> lcd = (List<StrawmanDBLibray.Entities.v_WRK_BOY_BY_CHANNEL_GENERAL>)GetSessionDataTable("v_WRK_BOY_BY_CHANNEL_GENERAL");
                    var q = lcd
                            .Where(m => m.TYPE == type && m.CHANNEL == channel
                                && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m=>m.CHANNEL)
                                .Select(p => new Models.BoyMassMarketModels
                    {
                        channel = p.CHANNEL,
                        boy_name = p.NAME,
                        brand_name = p.NAME,
                        market_col1 = (double?)p.MARKET_COL1,
                        market_col2 = (double?)p.MARKET_COL2,
                        market_pc = p.MARKET_CHGE,
                        sellin_col1 = (double?)p.SELLIN_COL1,
                        sellin_col2 = (double?)p.SELLIN_COL2,
                        sellin_pc = p.SELLIN_CHGE,
                        sellout_col1 = (double?)p.SELLOUT_COL1,
                        sellout_col2 = (double?)p.SELLOUT_COL2,
                        sellout_pc = p.SELLOUT_CHGE,
                        type = p.TYPE,
                        share_col1 = p.SHARE_COL1,
                        share_col2 = p.SHARE_COL2,
                        share_pc = p.SHARE_CHGE,
                        conversion_rate1 = null,
                        conversion_rate2 = null,
                        brand = 9999,
                        market = 9999,
                    });
                    lst = q.ToList();
                    break;
                
            }
            return lst;
        }

        private string GetDataType(string _type)
        {
            string ret = "";
            switch (_type)
            {
                case "GetBoyData":
                    ret = "YTD";
                    break;
                case "GetBoyYTD":
                    ret = "YTD";
                    break;
                case "GetBoyTOGO":
                    ret = "TOGO";
                    break;
                case "GetBoyTotals":
                    ret = "TOTAL";
                    break;
                case "GetBoyINT":
                case "GetBoyINTCustom":
                    ret = "INT";
                    break;
                case "GetBoyLE":
                case "GetBoyLECustom":
                    ret = "LE";
                    break;
                case "GetBoyPBP":
                case "GetBoyPBPCustom":
                    ret = "PBP";
                    break;
                default:
                    ret = _type.Replace("GetBoy", "").Replace("Custom", "").ToUpper();
                    break;
            }
            return ret;
        }

        private List<Models.BoyMassMarketModels> GetBoyCalcCustomData(string type)
        {
            List<Models.BoyMassMarketModels> query = null;

            switch (type)
            {
                case "INT":
                    query = ((List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA"))
                            .Where(p => p.TYPE == type && p.CHANNEL == channel
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m=>m.NTS_ORDER)
                            .Select(p=> new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME,
                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = (double?)p.MARKET_COL1,
                                market_pc = p.MARKET_PC,
                                sellin_col1 = (double?)p.SELLIN_COL1,
                                sellin_pc = p.SELLIN_PC,
                                sellout_col1 = (double?)p.SELLOUT_COL1,
                                sellout_pc = p.SELLOUT_PC,
                                type = p.TYPE,
                                market_name = p.BRAND_NAME,
                                //market_type = p.MARKET_TYPE,
                                //market_boy = p.MARKET_BOY,
                                market_id = (int?)p.MARKET_ID,
                                //market_col2 = (decimal?)p.MARKET_COL2,
                                //sellin_boy = (double?)p.SELLIN_BOY,
                                //sellin_col2 = (decimal?)p.SELLIN_COL2,
                                sellin_id = (int?)p.SELLIN_ID,
                                //sellin_type = p.SELLIN_TYPE,
                                //sellout_boy = p.SELLOUT_BOY,
                                //sellout_col2 = (decimal?)p.SELLOUT_COL2,
                                sellout_id = (int?)p.SELLOUT_ID
                                //sellout_type = p.SELLOUT_TYPE
                            }).ToList();
                    break;
                case "PBP":
                    query = ((List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA"))
                            .Where(p => p.TYPE == type && p.CHANNEL == channel
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.NTS_ORDER)
                            .Select(p=> new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME,
                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = (double?)p.MARKET_COL1,
                                market_pc = p.MARKET_PC,
                                sellin_col1 = (double?)p.SELLIN_COL1,
                                sellin_pc = p.SELLIN_PC,
                                sellout_col1 = (double?)p.SELLOUT_COL1,
                                sellout_pc = p.SELLOUT_PC,
                                type = p.TYPE,
                                market_name = p.BRAND_NAME,
                                //market_type = p.MARKET_TYPE,
                                //market_boy = p.MARKET_BOY,
                                market_id = (int?)p.MARKET_ID,
                                //market_col2 = (decimal?)p.MARKET_COL2,
                                //sellin_boy = p.SELLIN_BOY,
                                //sellin_col2 = (decimal?)p.SELLIN_COL2,
                                sellin_id = (int?)p.SELLIN_ID,
                                //sellin_type = p.SELLIN_TYPE,
                                //sellout_boy = p.SELLOUT_BOY,
                                //sellout_col2 = (decimal?)p.SELLOUT_COL2,
                                sellout_id = (int?)p.SELLOUT_ID
                                //sellout_type = p.SELLOUT_TYPE
                            }).ToList();
                    break;
                case "LE":
                    query = ((List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA"))
                            .Where(p => p.TYPE == type && p.CHANNEL == channel
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.NTS_ORDER)
                            .Select(p=> new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME,
                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = (double?)p.MARKET_COL2,
                                market_pc = p.MARKET_PC,
                                market_btg = (double?)p.MARKET_BTG,
                                sellin_col1 = (double?)p.SELLIN_COL2,
                                sellin_pc = p.SELLIN_PC,
                                sellin_btg = (double?)p.SELLIN_BTG,
                                sellout_col1 = (double?)p.SELLOUT_COL2,
                                sellout_pc = p.SELLOUT_PC,
                                sellout_btg = (double?)p.SELLOUT_BTG,
                                type = p.TYPE,
                                market_name = p.BRAND_NAME,
                                //market_type = p.MARKET_TYPE,
                                //market_boy = p.MARKET_BOY,
                                market_id = (int?)p.MARKET_ID,
                                market_col2 = (double?)p.MARKET_COL1,
                                //sellin_boy = p.SELLIN_BOY,
                                sellin_col2 = (double?)p.SELLIN_COL1,
                                sellin_id = (int?)p.SELLIN_ID,
                                //sellin_type = p.SELLIN_TYPE,
                                //sellout_boy = p.SELLOUT_BOY,
                                sellout_col2 = (double?)p.SELLOUT_COL1,
                                sellout_id = (int?)p.SELLOUT_ID,
                                market_pc_int =  p.MARKET_PC_COL2,
                                sellout_pc_int = p.SELLOUT_PC_COL2,
                                sellin_pc_int = p.SELLIN_PC_COL2
                                //sellout_type = p.SELLOUT_TYPE
                            }).ToList();
                    break;

                
            }
            return query.ToList();
        }


        private List<Models.BoyMassMarketModels> GetBoyCalcData(string type)
        {
            IEnumerable<Models.BoyMassMarketModels> query = null;
            
            switch (type)
            {
                case "INT":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> lint = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA");
                    query = lint
                            .Where(p => p.TYPE == type
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.NTS_ORDER)
                            .Select(p => new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name = p.BRAND_NAME,
                                conversion_rate = null,
                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = (double?)p.MARKET_COL1,
                                //market_col2 = p.MARKET_COL2,
                                //market_pc_int = p.MARKET_PC_INT,
                                market_pc = p.MARKET_PC,
                                sellin_col1 = (double?)p.SELLIN_COL1,
                                //sellin_col2 = p.SELLIN_COL2,
                                //sellin_pc_int = p.SELLIN__PC_INT,
                                sellin_pc = p.SELLIN_PC,
                                sellout_col1 = (double?)p.SELLOUT_COL1,
                                //sellout_col2 = p.SELLOUT_COL2,
                                //sellout_pc_int = p.SELLOUT_PC_INT,
                                sellout_pc = p.SELLOUT_PC,
                                share_col1 = p.SHARE_COL1,
                                share_pc = p.SHARE_PC,
                                //share_col2 = p.SHARE_COL2,
                                type = p.TYPE,
                                market_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME
                            }).AsEnumerable();
                    break;
                case "PBP":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> lpbp = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA");
                    query = lpbp
                            .Where(p => p.TYPE == type
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.NTS_ORDER)
                            .Select(p => new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name =p.BRAND_NAME,
                                conversion_rate = null,
                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = (double?)p.MARKET_COL1,
                                //market_col2 = p.MARKET_COL2,
                                //market_pc_int = p.MARKET_PC_INT,
                                market_pc = p.MARKET_PC,
                                sellin_col1 = (double?)p.SELLIN_COL1,
                                //sellin_col2 = p.SELLIN_COL2,
                                //sellin_pc_int = p.SELLIN__PC_INT,
                                sellin_pc = p.SELLIN_PC,
                                sellout_col1 = (double?)p.SELLOUT_COL1,
                                //sellout_col2 = p.SELLOUT_COL2,
                                //sellout_pc_int = p.SELLOUT_PC_INT,
                                sellout_pc = p.SELLOUT_PC,
                                share_col1 = p.SHARE_COL1,
                                share_pc = p.SHARE_PC,
                                // share_col2 = p.SHARE_COL2,
                                type = p.TYPE,
                                market_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME
                            }).AsEnumerable();
                    break;
                case "LE":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> lle = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA");
                    query = lle
                            .Where(p => p.TYPE == type
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.NTS_ORDER)
                            .Select(p => new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name = p.BRAND_NAME,
                                conversion_rate = null,
                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = (double?)p.MARKET_COL1,
                                market_col2 = (double?)p.MARKET_COL2,
                                market_pc = p.MARKET_PC,
                                market_pc_int = p.MARKET_PC_COL2,
                                sellin_col1 = (double?)p.SELLIN_COL1,
                                sellin_col2 = (double?)p.SELLIN_COL2,
                                sellin_pc = p.SELLIN_PC,
                                sellin_pc_int = p.SELLIN_PC_COL2,
                                sellout_col1 = (double?)p.SELLOUT_COL1,
                                sellout_col2 = (double?)p.SELLOUT_COL2,
                                sellout_pc = p.SELLOUT_PC,
                                sellout_pc_int = p.SELLOUT_PC_COL2,
                                share_col1 = p.SHARE_COL1,
                                share_col2 = p.SHARE_PC,
                                share_pc = p.SHARE_PC,
                                share_pc_int = p.SHARE_PC_COL2,
                                //share_col2 = p.SHARE_COL2,
                                type = p.TYPE,
                                market_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME
                            }).AsEnumerable();
                    break;
                default:
                    query = ((List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA"))
                            .Where(p => p.TYPE == type 
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m => m.NTS_ORDER)
                            .Select(p => new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name = p.BRAND_NAME,

                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = (double?)p.MARKET_COL1,
                                market_col2 = (double?)p.MARKET_COL2,

                                market_pc = (decimal?)p.MARKET_PC,
                                sellin_col1 = (double?)p.SELLIN_COL1,
                                sellin_col2 = (double?)p.SELLIN_COL2,

                                sellin_pc = (decimal?)p.SELLIN_PC,
                                sellout_col1 = (double?)p.SELLOUT_COL1,
                                sellout_col2 = (double?)p.SELLOUT_COL2,

                                sellout_pc = (decimal?)p.SELLOUT_PC,

                                type = p.TYPE,
                                market_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME
                            }).AsEnumerable();
                    break;
                
            }
            SetChannelTotalCalc(ref query, type);
            return query.ToList();
        }
        private List<Models.BoyMassMarketModels> GetBoyYTDData(string type)
        {

            var query = ((List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA"))
                        .Where (p=>p.TYPE == type
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .OrderBy(m=>m.NTS_ORDER).ThenBy(m=>m.BRAND)
                        .Select (p=>new Models.BoyMassMarketModels
                        {
                            channel = p.CHANNEL,
                            brand = p.BRAND,
                            brand_name = p.BRAND_NAME,
                            boy_name = p.NTS_NAME == null? p.BRAND_NAME: p.NTS_NAME,
                            conversion_rate1 = null,
                            conversion_rate2 = null,
                            vgroup = p.GROUP,
                            vorder = p.NTS_ORDER,
                            market = p.MARKET,
                            market_col1 = (double?)p.MARKET_COL1,
                            market_col2 = (double?)p.MARKET_COL2,
                            market_pc = p.MARKET_PC,
                            sellin_col1 = (double?)p.SELLIN_COL1,
                            sellin_col2 = (double?)p.SELLIN_COL2,
                            sellin_pc = p.SELLIN_PC,
                            sellout_col1 = (double?)p.SELLOUT_COL1,
                            sellout_col2 = (double?)p.SELLOUT_COL2,
                            sellout_pc = p.SELLOUT_PC,
                            share_col1 = p.SHARE_COL1,
                            share_col2 = p.SHARE_COL2,
                            share_pc = p.SHARE_PC,
                            type = p.TYPE
                        });
            SetChannelTotalCalc(ref query, type);
            return query.ToList();
            
        }
        
        public void SetChannelTotalCalc(ref IEnumerable<Models.BoyMassMarketModels> query, string type)
        {
            List<Models.BoyMassMarketModels> total = new List<Models.BoyMassMarketModels>();
            List<Models.BoyMassMarketModels> _le = new List<Models.BoyMassMarketModels>();
            List<Models.BoyMassMarketModels> _int = new List<Models.BoyMassMarketModels>();
            
            var chan = GetGroupedChannels(query);
            
            switch (type)
            {
                case "INT":
                    total = GetBoyYTDData("TOTAL");
                    chan = chan
                        .GroupBy(m => new {_type = m.type, _id = m._id })
                        .Select(p => new Models.BoyMassMarketModels
                        {
                            _id = p.Key._id,
                            channel = p.Sum(s => s.channel),
                            brand = p.Max(s => s.base_id) + p.Sum(s=>s.channel),
                            boy_name = p.LastOrDefault().boy_name,
                            vgroup = p.LastOrDefault().vgroup,
                            market = p.Max(s => s.base_id) + p.Sum(s => s.channel),
                            market_col1 = p.Sum(s => s.market_col1),
                            market_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)total.FirstOrDefault(m=>m._id == p.Key._id).market_col2, (decimal)p.Sum(s => s.market_col1)),
                            sellin_col1 = p.Sum(s => s.sellin_col1),
                            sellin_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)total.FirstOrDefault(m=>m._id == p.Key._id).sellin_col2,(decimal)p.Sum(s => s.sellin_col1)),
                            sellout_col1 = p.Sum(s => s.sellout_col1),
                            sellout_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)total.FirstOrDefault(m=>m._id == p.Key._id).sellout_col2,(decimal)p.Sum(s => s.sellout_col1)),
                            share_col1 = Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1)),
                            share_pc = 
                                    Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1))
                                - (decimal)total.FirstOrDefault(m => m._id == p.Key._id).share_col2,
                            type = p.Key._type

                        });

                    break;
                case "LE":
                    total = GetBoyYTDData("TOTAL");
                    _int = GetBoyCalcData("INT");
                    chan = chan
                        .GroupBy(m => new {_type = m.type, _id = m._id })
                        .Select(p => new Models.BoyMassMarketModels
                        {
                            _id = p.Key._id,
                            channel = p.Sum(s=>s.channel),
                            brand = p.Max(s => s.base_id) + p.Sum(s => s.channel),
                            boy_name = p.LastOrDefault().boy_name,
                            vgroup = p.LastOrDefault().vgroup,
                            market = p.Max(s => s.base_id) + p.Sum(s => s.channel),
                            market_col1 = p.Sum(s => s.market_col1),
                            market_col2 = p.Sum(s => s.market_col1) - _int.FirstOrDefault(m=>m._id == p.Key._id).market_col1,
                            market_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)total.FirstOrDefault(m => m._id == p.Key._id).market_col2, (decimal)p.Sum(s => s.market_col1)),
                            market_pc_int = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)_int.FirstOrDefault(m => m._id == p.Key._id).market_col1, (decimal)p.Sum(s => s.market_col1)),
                            sellin_col1 = p.Sum(s => s.sellin_col1),
                            sellin_col2 = p.Sum(s => s.sellin_col1) - _int.FirstOrDefault(m => m._id == p.Key._id).sellin_col1,
                            sellin_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)total.FirstOrDefault(m => m._id == p.Key._id).sellin_col2, (decimal)p.Sum(s => s.sellin_col1)),
                            sellin_pc_int = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)_int.FirstOrDefault(m => m._id == p.Key._id).sellin_col1, (decimal)p.Sum(s => s.sellin_col1)),
                            sellout_col1 = p.Sum(s => s.sellout_col1),
                            sellout_col2 = p.Sum(s => s.sellout_col1) - _int.FirstOrDefault(m => m._id == p.Key._id).sellout_col1,
                            sellout_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)total.FirstOrDefault(m => m._id == p.Key._id).sellout_col2, (decimal)p.Sum(s => s.sellout_col1)),
                            sellout_pc_int = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)_int.FirstOrDefault(m => m._id == p.Key._id).sellout_col1, (decimal)p.Sum(s => s.sellout_col1)),
                            share_col1 = Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1)),
                            share_pc = 
                                    Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1))
                                - (decimal)total.FirstOrDefault(m => m._id == p.Key._id).share_col2,
                            share_pc_int =
                                Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1))
                            - (decimal)_int.FirstOrDefault(m => m._id == p.Key._id).share_col1,
                            type = p.Key._type

                        });

                    break;
                case "PBP":
                    _le = GetBoyCalcData("LE");
                    chan = chan
                        .GroupBy(m => new { _type = m.type, _id = m._id })
                        .Select(p => new Models.BoyMassMarketModels
                        {
                            _id = p.Key._id,
                            channel = p.Sum(s => s.channel),
                            brand = p.Max(s => s.base_id) + p.Sum(s => s.channel),
                            boy_name = p.LastOrDefault().boy_name,
                            vgroup = p.LastOrDefault().vgroup,
                            market = p.Max(s => s.base_id) + p.Sum(s => s.channel),
                            market_col1 = p.Sum(s => s.market_col1),
                            market_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)_le.FirstOrDefault(m => m._id == p.Key._id).market_col1, (decimal)p.Sum(s => s.market_col1)),
                            sellin_col1 = p.Sum(s => s.sellin_col1),
                            sellin_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)_le.FirstOrDefault(m => m._id == p.Key._id).sellin_col1, (decimal)p.Sum(s => s.sellin_col1)),
                            sellout_col1 = p.Sum(s => s.sellout_col1),
                            sellout_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)_le.FirstOrDefault(m => m._id == p.Key._id).sellout_col1, (decimal)p.Sum(s => s.sellout_col1)),
                            share_col1 = Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1)),
                            share_pc = 
                                    Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1))
                                -   (decimal)_le.FirstOrDefault(m => m._id == p.Key._id).share_col1,
                            type = p.Key._type

                        });

                    break;
                default:
                    chan = chan
                        .GroupBy(m => new { _type = m.type, _id = m._id })
                        .Select(p => new Models.BoyMassMarketModels
                        {
                            _id = p.Key._id,
                            channel = p.Sum(s=>s.channel),
                            brand = p.Max(s => s.base_id) + p.Sum(s => s.channel),
                            boy_name = p.LastOrDefault().boy_name,
                            vgroup = p.LastOrDefault().vgroup,
                            market = p.Max(s => s.base_id) + p.Sum(s => s.channel),
                            market_col1 = p.Sum(s => s.market_col1),
                            market_col2 = p.Sum(s => s.market_col2),
                            market_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.market_col2)),
                            sellin_col1 = p.Sum(s => s.sellin_col1),
                            sellin_col2 = p.Sum(s => s.sellin_col2),
                            sellin_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)p.Sum(s => s.sellin_col1), (decimal)p.Sum(s => s.sellin_col2)),
                            sellout_col1 = p.Sum(s => s.sellout_col1),
                            sellout_col2 = p.Sum(s => s.sellout_col2),
                            sellout_pc = Helpers.StrawmanCalcs.CalcPCVSPY((decimal)p.Sum(s => s.sellout_col1), (decimal)p.Sum(s => s.sellout_col2)),
                            share_col1 = Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1)),
                            share_col2 = Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col2), (decimal)p.Sum(s => s.sellout_col2)),
                            share_pc = Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col1), (decimal)p.Sum(s => s.sellout_col1))
                            - Helpers.StrawmanCalcs.CalcShare((decimal)p.Sum(s => s.market_col2), (decimal)p.Sum(s => s.sellout_col2)),
                            type = p.Key._type

                        });
                    break;
            }
            query = query.Union(chan).ToList();            
        }

        private IEnumerable<Models.BoyMassMarketModels> GetGroupedChannels(IEnumerable<Models.BoyMassMarketModels> query)
        {
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> gchannels = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG, true);
            List<StrawmanDBLibray.Entities.GROUP_MASTER> gmaster = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER, true);
            List<StrawmanDBLibray.Entities.BOY_CONFIG> gconfig = (List<StrawmanDBLibray.Entities.BOY_CONFIG>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.BOY_CONFIG, true);
            List<StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG> mcfg = (List<StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.CALCS_MARKETS_CONFIG);
            List<StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG> bcfg = (List<StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.CALCS_BRANDS_CONFIG);
            var cfg = mcfg.Join(bcfg, m=>new{_brand = m.BRAND, _market = m.MARKET}, b=>new{_brand = b.BRAND, _market = b.MARKET}, (m,b)=>new{m=m, b= b}).AsEnumerable().Select(m=>new {
                _brand = m.m.BRAND,
                _market = m.m.MARKET,
                _brandcfg = m.b.CHANNELCFG,
                _marketcfg = m.m.CHANNELCFG
            }).ToList();
            var grp = gchannels.Where(m => m.TYPE_ID == (decimal?)StrawmanCalcs.GetGroupTypeByView(BY_CHANNEL_CONTROLLER)).AsEnumerable()
                .Join(gconfig, c => new { c.MARKET, c.BRAND }, n => new { n.MARKET, n.BRAND }, (c, n) => new
                {
                    id = c.GROUP_ID,
                    base_id = gmaster.FirstOrDefault(m => m.ID == c.GROUP_ID).BASE_ID,
                    market = c.MARKET,
                    brand = c.BRAND,
                    channel = n.CHANNEL,
                    market_config = cfg.Find(m=>m._brand == c.BRAND && m._market == c.MARKET) == null?0:cfg.Find(m=>m._brand == c.BRAND && m._market == c.MARKET)._marketcfg ?? n.MARKET_CONFIG,
                    sellout_config = cfg.Find(m=>m._brand == c.BRAND && m._market == c.MARKET) == null?0:cfg.Find(m => m._brand == c.BRAND && m._market == c.MARKET)._brandcfg ?? n.SELLOUT_CONFIG,
                    sellin_config = n.SELLIN_CONFIG,
                    name = gmaster.FirstOrDefault(m => m.ID == c.GROUP_ID).NAME
                }).AsEnumerable();
            var chan =  grp
                .Join(query.Where(m=>m.brand<9000&&m.market<9000).AsEnumerable(), m => new { _brand = m.brand, _market = m.market, _channel = m.channel}, l => new { _brand = l.brand, _market = l.market, _channel = l.channel },
                        (m, l) => new Models.BoyMassMarketModels
                        {
                            _id = (decimal)m.id,
                            channel = m.channel,
                            brand = m.brand,
                            boy_name = m.name,
                            market = m.market,
                            base_id = m.base_id,
                            market_col1 = l.market_col1 * (double)(m.market_config??1),
                            market_col2 = l.market_col2 * (double)(m.market_config??1),
                            sellin_col1 = l.sellin_col1 * (double)(m.sellin_config??1),
                            sellin_col2 = l.sellin_col2 * (double)(m.sellin_config??1),
                            sellout_col1 = l.sellout_col1 * (double)(m.sellout_config??1),
                            sellout_col2 = l.sellout_col2 * (double)(m.sellout_config??1),
                            type = l.type,
                            vgroup = l.vgroup

                        })
                .AsEnumerable();

            return chan
                        .GroupBy(m => new { _type = m.type, _id = m._id, _channel = m.channel })
                        .Select(p => new Models.BoyMassMarketModels
                        {
                            _id = p.Key._id,
                            base_id = p.Max(s=>s.base_id),
                            channel = p.Key._channel,
                            brand = p.Max(s => s.brand),
                            boy_name = p.LastOrDefault().boy_name,
                            vgroup = p.LastOrDefault().vgroup,
                            market = p.Max(s => s.market),
                            market_col1 = p.Sum(s => s.market_col1),
                            market_col2 = p.Sum(s => s.market_col2),
                            sellin_col1 = p.Sum(s => s.sellin_col1),
                            sellin_col2 = p.Sum(s => s.sellin_col2),
                            sellout_col1 = p.Sum(s => s.sellout_col1),
                            sellout_col2 = p.Sum(s => s.sellout_col2),
                            type = p.Key._type

                        });
            
        }

        #region Public Functions
        public List<Models.BoyMassMarketModels> GetMasterData(string _channel)
        {
            SetChannel(_channel);
            SetSessionData(BOYYTDDATA, GetBoyYTDData("YTD"));

            return GetSessionData(BOYYTDDATA).Where(m=>m.channel == channel).ToList();
        }
        #endregion
        //
        // GET: /BoyMassMarket/BOYJJByChannel

        public ActionResult BOYJJByChannel()
        {
            ViewBag.Title = "BOYJJByChannel";
            ViewBag.TabUrl = CONTROLLER + "/BOYJJByChannel";
            return View();
        }
        //
        // GET: /BoyMassMarket/

        public ActionResult Index()
        {
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            return View();
        }

        //
        // GET: /BoyMassMarket/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /BoyMassMarket/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BoyMassMarket/Create

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
        // GET: /BoyMassMarket/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /BoyMassMarket/Edit/5

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
        // GET: /BoyMassMarket/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /BoyMassMarket/Delete/5

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

        private const string _PATH = "~/Views/BoyMassMarket/";
        private const string BOYYTD = _PATH + "_BoyYTD.cshtml";
        private const string BOYDATA = _PATH + "_BoyData.cshtml";
        private const string BOYTOTALS = _PATH + "_BoyTotals.cshtml";
        private const string BOYTOGO = _PATH + "_BoyTOGO.cshtml";
        private const string BOYINT = _PATH + "_BoyINT.cshtml";
        private const string BOYPBP = _PATH + "_BoyPBP.cshtml";
        private const string BOYLE = _PATH + "_BoyLE.cshtml";
        private const string BOYINTCUSTOM = _PATH + "_BoyINTCustom.cshtml";
        private const string BOYPBPCUSTOM = _PATH + "_BoyPBPCustom.cshtml";
        private const string BOYLECUSTOM = _PATH + "_BoyLECustom.cshtml";
        private const string BOYCUSTOM = _PATH + "_BoyCustom.cshtml";
        private int channel;
        private int? group_type;
        private static string BOYYTDDATA = "YTDData";
        private const string CONTROLLER = "BoyMassMarket";
        private const string BY_CHANNEL_CONTROLLER = "BOYJJByChannel";

        //StrawmanConstants sc = new StrawmanConstants();

        public List<Models.BoyMassMarketModels> GetDataFromController(string key, int _channel)
        {
            this.channel = _channel;
            if (key == "TOGO")
                return GetSessionData(key);
            else
                return GetBoyCalcData(key);
        }

        private List<Models.BoyMassMarketModels> GetSessionData(string key)
        {
            List<Models.BoyMassMarketModels> list = null;
            if (Helpers.Session.GetSession(key) != null) list = (List<Models.BoyMassMarketModels>)Helpers.Session.GetSession(key);
            return list;
        }
        private void SetSessionData(string key, object obj)
        {
            if (key != null && obj != null)
                Helpers.Session.SetSession(key, obj);
        }

        private object GetSessionDataTable(string key)
        {
            object tmp = null;
            if (Helpers.Session.GetSession(key) == null)
            {
                if(key!= "WRK_BOY_DATA"){
                    tmp = StrawmanDBLibray.DBLibrary.GetBoyData(key);
                }else{
                    tmp = StrawmanDBLibray.DBLibrary.GetBoyData();
                }
                Helpers.Session.SetSession(key, tmp);
            }
            else
                tmp = Helpers.Session.GetSession(key);

            return tmp;
        }
        
        private void SetChannel(string chan)
        {
            switch (chan)
            {
                case StrawmanConstants.CHANNEL_BEAUTY:
                    this.channel = StrawmanConstants.getChannel(StrawmanConstants.CHANNEL_BEAUTY);
                    break;
                case StrawmanConstants.CHANNEL_OTC:
                    this.channel = StrawmanConstants.getChannel(StrawmanConstants.CHANNEL_OTC);
                    break;
                default:
                    this.channel = StrawmanConstants.getChannel(StrawmanConstants.CHANNEL_MASS);
                    break;
            }
            
        }

        private void SetGroupType(int? type)
        {
            decimal? by_channel_view = StrawmanCalcs.GetGroupTypeByView(BY_CHANNEL_CONTROLLER);
            if (type == null)
                group_type = by_channel_view == null ? 23 : (int)by_channel_view;
            else
                group_type = type;
        }

        //private decimal? GetGroupTypeByView(string view)
        //{
        //    List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> vars = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
        //    string type = vars.FirstOrDefault(m => m.VIEW == view).VALUE;
        //    decimal _type = 0;
        //    if (type == null || !decimal.TryParse(type, out _type))
        //        return null;
        //    return _type;
        //}
    }
}
