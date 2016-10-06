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
                    case "TOTALS":
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
            SetSessionData(BOYYTDDATA, GetBoyYTDData("YTD"));
            ViewBag.BoyData = GetSessionData(BOYYTDDATA);

            Session.Add("YTDData", ViewBag.BoyData);
            return PartialView(BOYDATA);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyYTD(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyYTD = GetBoyYTDData("YTD");
            //if (Session["YTDData"] != null) BoyYTDData = (List<Models.BoyMassMarketModels>)Session["YTDData"];
            return PartialView(BOYYTD, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyTotals(string chan)
        {
            
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyYTD = GetBoyYTDData("TOTAL");
            return PartialView(BOYTOTALS,GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyTOGO(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year; 
            ViewBag.BoyYTD = GetBoyYTDData("TOGO");
            return PartialView(BOYTOGO, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyINT(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCALC = GetBoyCalcData("INT");
            return PartialView(BOYINT, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyPBP(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCALC = GetBoyCalcData("PBP");
            return PartialView(BOYPBP, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyLE(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCALC = GetBoyCalcData("LE");
            return PartialView(BOYLE, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyINTCustom(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCustom = GetBoyCalcCustomData("INT");
            ViewBag.TableTitle = Helpers.PeriodUtil.Year.ToString() + " Int";
            return PartialView(BOYCUSTOM, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyPBPCustom(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCustom = GetBoyCalcCustomData("PBP");
            ViewBag.TableTitle = (Helpers.PeriodUtil.Year + 1).ToString() + " PBP";
            return PartialView(BOYCUSTOM, GetSessionData(BOYYTDDATA));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyLECustom(string chan)
        {
            SetChannel(chan);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.BoyCustom = GetBoyCalcCustomData("LE");
            ViewBag.TableTitle = "BTG " + Helpers.PeriodUtil.Year.ToString("YY");
            ViewBag.TableTitle2 = Helpers.PeriodUtil.Year.ToString() + " LE";
            return PartialView(BOYCUSTOM, GetSessionData(BOYYTDDATA));
        }


        private dynamic GetTotalData(string _type, string _channel)
        {
            List<Models.BoyMassMarketModels> lst = new List<Models.BoyMassMarketModels>();
            string key = _channel +"YTDData";
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
                    if(GetSessionData(key)== null){
                        Session.Add(key,GetBOYByChannelData(GetDataType(_type)).Select(m=>m).ToList());
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
                    ret = "TOTALS";
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
            List<Models.BoyMassMarketModels> query = null;
            
            switch (type)
            {
                case "INT":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> lint = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA");
                    query = lint
                            .Where(p => p.CHANNEL == channel && p.TYPE == type
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
                            }).ToList();
                    break;
                case "PBP":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> lpbp = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA");
                    query = lpbp
                            .Where(p => p.CHANNEL == channel && p.TYPE == type
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
                            }).ToList();
                    break;
                case "LE":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> lle = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA");
                    query = lle
                            .Where(p => p.CHANNEL == channel && p.TYPE == type
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
                            }).ToList();
                    break;
                default:
                    query = ((List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA"))
                            .Where(p => p.TYPE == type && p.CHANNEL == channel
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
                            }).ToList();
                    break;
                
            }
            return query.ToList();
        }
        private List<Models.BoyMassMarketModels> GetBoyYTDData(string type)
        {

            var query = ((List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionDataTable("WRK_BOY_DATA"))
                        .Where (p=>p.TYPE == type && p.CHANNEL == channel
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
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> gchannels = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG, true);
            var chan = gchannels.Join(query, m => new { _brand = m.BRAND, _market = m.MARKET }, l => new { _brand = l.brand, _market = l.market }, (m, l) => new { m = m, l = l })
                .AsEnumerable()
                .GroupBy(m=>new{_channel = m.l.channel})
                .Select(p => new Models.BoyMassMarketModels
                {
                    channel = p.Key._channel,
                    brand = p.Max(s=>s.l.brand),
                    brand_name = p.LastOrDefault().l.brand_name,
                    boy_name = p.LastOrDefault().l.boy_name,
                    conversion_rate1 = null,
                    conversion_rate2 = null,
                    vgroup = p.LastOrDefault().l.vgroup,
                    market = p.LastOrDefault().l.market,
                    market_col1 = (double)p.Sum(s => (decimal)s.l.market_col1 * s.m.CONFIG),
                    market_col2 = (double)p.Sum(s => (decimal)s.l.market_col2 * s.m.CONFIG),
                    market_pc = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(s => (decimal)s.l.market_col1 * s.m.CONFIG), p.Sum(s => (decimal)s.l.market_col2 * s.m.CONFIG)),
                    sellin_col1 = (double)p.Sum(s => (decimal)s.l.sellin_col1 * s.m.CONFIG),
                    sellin_col2 = (double)p.Sum(s => (decimal)s.l.sellin_col2 * s.m.CONFIG),
                    sellin_pc = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(s => (decimal)s.l.sellin_col1 * s.m.CONFIG), p.Sum(s => (decimal)s.l.sellin_col2 * s.m.CONFIG)),
                    sellout_col1 = (double)p.Sum(s => (decimal)s.l.sellout_col1 * s.m.CONFIG),
                    sellout_col2 = (double)p.Sum(s => (decimal)s.l.sellout_col2 * s.m.CONFIG),
                    sellout_pc = Helpers.StrawmanCalcs.CalcPCVSPY(p.Sum(s => (decimal)s.l.sellout_col1 * s.m.CONFIG), p.Sum(s => (decimal)s.l.sellout_col2 * s.m.CONFIG)),
                    share_col1 = p.SHARE_COL1,
                    share_col2 = p.SHARE_COL2,
                    share_pc = p.SHARE_PC,
                    type = p.TYPE

                });

            return query.ToList();
            
        }

        #region Public Functions
        public List<Models.BoyMassMarketModels> GetMasterData(string channel)
        {
            SetChannel(channel);
            SetSessionData(BOYYTDDATA, GetBoyYTDData("YTD"));

            return GetSessionData(BOYYTDDATA);
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
        private static string BOYYTDDATA = "YTDData";
        private const string CONTROLLER = "BoyMassMarket";

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
    }
}
