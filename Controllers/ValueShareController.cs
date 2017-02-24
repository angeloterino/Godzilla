using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    public class ValueShareController : Controller
    {
        public ActionResult GetTitled()
        {
            ViewBag.ValueShareTitle = VALUE_SHARE_TITLE;
            ViewBag.LMValueShareTitle = LM;
            ViewBag.YTDValueShareTitle = YTD;
            ViewBag.MATValueShareTitle = MAT;
            ViewBag.BTGValueShareTitle = BTG;
            ViewBag.INTERNALValueShareTitle = INTERNAL;
            ViewBag.LEValueShareTitle = LE;
            ViewBag.PBPValueShareTitle = PBP;
            ViewBag.TotalValueShareTitle = TOTAL;
            ViewBag.BOYValueShareTitle = BOY;

            return PartialView(TITLED_PARTIAL);
        }
        public ActionResult GetLM()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(LM);
            ViewBag.TitleCol1 = (Helpers.PeriodUtil.Year - 1).ToString("YYYY") + " " + LM;
            ViewBag.TitleCol2 = Helpers.PeriodUtil.Year.ToString("YYYY") + " " + LM;
            ViewBag.MasterData = GetMasterData();
            return PartialView(LM_PARTIAL, model);
        }

        public ActionResult GetYTD()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(YTD);
            ViewBag.TitleCol1 = (Helpers.PeriodUtil.Year - 1).ToString("YYYY") + " " + YTD;
            ViewBag.TitleCol2 = Helpers.PeriodUtil.Year.ToString("YYYY") + " " + YTD;
            ViewBag.MasterData = GetMasterData();
            return PartialView(YTD_PARTIAL, model);
        }

        public ActionResult GetMAT()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(MAT);
            ViewBag.TitleCol1 = (Helpers.PeriodUtil.Year - 1).ToString("YYYY") + " " + MAT;
            ViewBag.TitleCol2 = Helpers.PeriodUtil.Year.ToString("YYYY") + " " + MAT;
            ViewBag.MasterData = GetMasterData();
            return PartialView(MAT_PARTIAL, model);
        }
        public ActionResult GetBTG()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(BTG);
            ViewBag.TitleCol1 = (Helpers.PeriodUtil.Year - 1).ToString("YYYY") + " " + BTG;
            ViewBag.TitleCol2 = Helpers.PeriodUtil.Year.ToString("YYYY") + " " + BTG;
            ViewBag.MasterData = GetMasterData();
            return PartialView(BTG_PARTIAL, model);
        }
        public ActionResult GetTotal()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(TOTAL);
            ViewBag.TitleCol1 = (Helpers.PeriodUtil.Year - 3).ToString("YYYY");
            ViewBag.TitleCol2 = (Helpers.PeriodUtil.Year - 2).ToString("YYYY");
            ViewBag.TitleCol3 = (Helpers.PeriodUtil.Year - 1).ToString("YYYY");
            ViewBag.MasterData = GetMasterData();
            return PartialView(TOTAL_PARTIAL, model);
        }

        public ActionResult GetBOY()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(BOY);
            ViewBag.TitleCol1 = Helpers.PeriodUtil.Year.ToString("YYYY") + " " + INTERNAL;
            ViewBag.TitleCol2 = Helpers.PeriodUtil.Year.ToString("YYYY") + " " + LE;
            ViewBag.TitleCol3 = (Helpers.PeriodUtil.Year + 1).ToString("YYYY") + " " + PBP;
            ViewBag.MasterData = GetMasterData();
            return PartialView(BOY_PARTIAL, model);
        }

        private List<Models.StrawmanViewSTDModel> GetData(string type)
        {
            List<Models.StrawmanViewSTDModel> market = new List<Models.StrawmanViewSTDModel>();
            List<Models.StrawmanViewSTDModel> brand = new List<Models.StrawmanViewSTDModel>();
            switch (type)
            {
                case LM:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MONTH);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MONTH);
                    return market.Join(brand, m => new { m.brand, m.market, m.channel }, b => new { b.brand, b.market, b.channel }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        brand = m.brand,
                        market = m.market,
                        channel = m.channel,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case YTD:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_YTD);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_YTD);
                    return market.Join(brand, m => new { m.brand, m.market, m.channel }, b => new { b.brand, b.market, b.channel }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        brand = m.brand,
                        market = m.market,
                        channel = m.channel,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case MAT:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MAT);
                    return market.Join(brand, m => new { m.brand, m.market, m.channel }, b => new { b.brand, b.market, b.channel }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        brand = m.brand,
                        market = m.market,
                        channel = m.channel,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case BTG:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BTG);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BTG);
                    return market.Join(brand, m => new { m.brand, m.market, m.channel }, b => new { b.brand, b.market, b.channel }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        brand = m.brand,
                        market = m.market,
                        channel = m.channel,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case TOTAL:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_TOTAL);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_TOTAL);
                    return market.Join(brand, m => new { m.brand, m.market, m.channel }, b => new { b.brand, b.market, b.channel }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        brand = m.brand,
                        market = m.market,
                        channel = m.channel,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                        pcvspy3 = Helpers.StrawmanCalcs.CalcShare(m.col3, b.col3),
                    }).ToList();
                case BOY:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewController().GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewController().GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY);
                    return market.Join(brand, m => new { m.brand, m.market, m.channel }, b => new { b.brand, b.market, b.channel }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        brand = m.brand,
                        market = m.market,
                        channel = m.channel,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m._internal, b._internal),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m._le, b._le),
                        pcvspy3 = Helpers.StrawmanCalcs.CalcShare(m._pbp, b._pbp),
                    }).ToList();
                default:
                    return null;
            }
        }

        private List<Models.MarketDataModels> GetMasterData()
        {
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> data = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_MARKET_DATA);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> wc_channels = var.Where(m => m.VIEW == Classes.Default.Variables.WC_CHANNELS).Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> colors = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_COLORS)
                    .Select(m => m).ToList();
            return data.Where(m => m.STATUS == "A").AsEnumerable()
                .GroupJoin(colors, l => new { ID = "BRAND:" + l.BRAND.ToString() + ";MARKET:" + l.MARKET.ToString() }, v => new { ID = v.NAME }, (l, v) => new { l = l, v = v })
                .SelectMany(f => f.v.DefaultIfEmpty(), (l, v) => new { l = l.l, v = v }).ToList()
                .Select(p => new Models.MarketDataModels
                {
                    market = (decimal)p.l.MARKET,
                    brand = (decimal)p.l.BRAND,
                    channel = (decimal)p.l.CHANNEL,
                    source = p.l.SOURCE,
                    vgroup = p.l.GROUP,
                    vorder = p.l.ORDER,
                    vgorder = p.l.GROUP_ORDER,
                    style = p.v == null ? "" : Helpers.StyleUtils.GetBGColor(p.v.VALUE, true),
                    is_wc = wc_channels.Exists(m => m.VALUE == p.l.CHANNEL.ToString())
                }).ToList();
        }
        //
        // GET: /ValueShare/

        public ActionResult Index()
        {
            return View();
        }

        #region Constants
        private const string _PATH = "~/Views/";
        private const string CONTROLLER = "ValueShare";
        private const string BOY_PARTIAL = _PATH + CONTROLLER + "/_BOY.cshtml";
        private const string LM_PARTIAL = _PATH + CONTROLLER + "/_LM.cshtml";
        private const string MAT_PARTIAL = _PATH + CONTROLLER + "/_MAT.cshtml";
        private const string TOTAL_PARTIAL = _PATH + CONTROLLER + "/_TOTAL.cshtml";
        private const string YTD_PARTIAL = _PATH + CONTROLLER + "/_YTD.cshtml";
        private const string BTG_PARTIAL = _PATH + CONTROLLER + "/_BTG.cshtml";
        private const string TITLED_PARTIAL = _PATH + CONTROLLER + "/_Titled.cshtml";

        private const string LM = "LM";
        private const string YTD = "YTD";
        private const string MAT = "MAT";
        private const string BTG = "BTG";
        private const string TOTAL = "TOTAL";
        private const string BOY = "BOY";

        private const string INTERNAL = "Internal";
        private const string LE = "LE";
        private const string PBP = "PBP";

        private const string VALUE_SHARE_TITLE = "J&J % Value Share";
        #endregion

    }
}
