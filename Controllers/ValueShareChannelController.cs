using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    public class ValueShareChannelController : Controller
    {
        public ActionResult GetLM()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(LM);
            ViewBag.MasterData = GetMasterData();
            return PartialView(LM_PARTIAL, model);
        }

        public ActionResult GetYTD()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(YTD);
            ViewBag.MasterData = GetMasterData();
            return PartialView(YTD_PARTIAL, model);
        }

        public ActionResult GetMAT()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(MAT);
            ViewBag.MasterData = GetMasterData();
            return PartialView(MAT_PARTIAL, model);
        }
        public ActionResult GetBTG()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(BTG);
            ViewBag.MasterData = GetMasterData();
            return PartialView(BTG_PARTIAL, model);
        }
        public ActionResult GetTotal()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(TOTAL);
            ViewBag.MasterData = GetMasterData();
            return PartialView(TOTAL_PARTIAL, model);
        }

        public ActionResult GetBOY()
        {
            List<Models.StrawmanViewSTDModel> model = GetData(BOY);
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
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewChannelController().GetMarketViewData(Classes.StrawmanViews.MONTH);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewChannelController().GetBrandViewData(Classes.StrawmanViews.MONTH);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        channel = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case YTD:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewChannelController().GetMarketViewData(Classes.StrawmanViews.YTD);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewChannelController().GetBrandViewData(Classes.StrawmanViews.YTD);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        channel = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case MAT:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewChannelController().GetMarketViewData(Classes.StrawmanViews.MAT);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewChannelController().GetBrandViewData(Classes.StrawmanViews.MAT);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        channel = m.channel,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case BTG:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewChannelController().GetMarketViewData(Classes.StrawmanViews.BTG);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewChannelController().GetBrandViewData(Classes.StrawmanViews.BTG);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        channel = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case TOTAL:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewChannelController().GetMarketViewData(Classes.StrawmanViews.TOTAL);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewChannelController().GetBrandViewData(Classes.StrawmanViews.TOTAL);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        channel = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                        pcvspy3 = Helpers.StrawmanCalcs.CalcShare(m.col3, b.col3),
                    }).ToList();
                case BOY:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewChannelController().GetMarketViewData(Classes.StrawmanViews.BOY);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewChannelController().GetBrandViewData(Classes.StrawmanViews.BOY);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        channel = m.vid,
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
            List<StrawmanDBLibray.Entities.GROUP_MASTER> lst = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER, true);
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> var = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            var = var.Where(m => m.VIEW == Classes.Default.Variables.STRAWMAN_CHANNELS_COLORS)
                    .Select(m => m).ToList();
            List<Models.MarketDataModels> aux = lst
                .GroupJoin(var, l => new { ID = l.ID }, v => new { ID = decimal.Parse(v.NAME) }, (l, v) => new { l = l, v = v })
                .Where(m => m.l.TYPE == 20).Distinct()
                .SelectMany(f => f.v.DefaultIfEmpty(), (l, v) => new { l = l.l, v = v }).ToList()
                .Select(p => new Models.MarketDataModels
                {
                    vorder = p.l.ID,
                    channel = p.l.ID,
                    style = p.v == null ? "" : Helpers.StyleUtils.GetBGColor(p.v.VALUE, true),
                }).ToList();
            return aux;
        }
        //
        // GET: /ValueShare/

        public ActionResult Index()
        {
            return View();
        }

        #region Constants
        private const string _PATH = "~/Views/";
        private const string CONTROLLER = "ValueShareChannel";
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
