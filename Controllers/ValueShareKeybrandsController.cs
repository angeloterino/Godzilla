using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    public class ValueShareKeybrandsController : Controller
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
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewKeybrandsController().GetMarketViewData(Classes.StrawmanViews.MONTH);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewKeybrandsController().GetBrandViewData(Classes.StrawmanViews.MONTH);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case YTD:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewKeybrandsController().GetMarketViewData(Classes.StrawmanViews.YTD);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewKeybrandsController().GetBrandViewData(Classes.StrawmanViews.YTD);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case MAT:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewKeybrandsController().GetMarketViewData(Classes.StrawmanViews.MAT);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewKeybrandsController().GetBrandViewData(Classes.StrawmanViews.MAT);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case BTG:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewKeybrandsController().GetMarketViewData(Classes.StrawmanViews.BTG);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewKeybrandsController().GetBrandViewData(Classes.StrawmanViews.BTG);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                    }).ToList();
                case TOTAL:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewKeybrandsController().GetMarketViewData(Classes.StrawmanViews.TOTAL);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewKeybrandsController().GetBrandViewData(Classes.StrawmanViews.TOTAL);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                        pcvspy3 = Helpers.StrawmanCalcs.CalcShare(m.col3, b.col3),
                    }).ToList();
                case BOY:
                    market = (List<Models.StrawmanViewSTDModel>)new MarketViewKeybrandsController().GetMarketViewData(Classes.StrawmanViews.BOY);
                    brand = (List<Models.StrawmanViewSTDModel>)new BrandViewKeybrandsController().GetBrandViewData(Classes.StrawmanViews.BOY);
                    return market.Join(brand, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.StrawmanViewSTDModel
                    {
                        vid = m.vid,
                        pcvspy1 = Helpers.StrawmanCalcs.CalcShare(m._internal, b._internal),
                        pcvspy2 = Helpers.StrawmanCalcs.CalcShare(m._le, b._le),
                        pcvspy3 = Helpers.StrawmanCalcs.CalcShare(m._pbp, b._pbp),
                    }).ToList();
                default:
                    return null;
            }
        }

        private List<Models.MarketViewChannelModels> GetMasterData()
        {
            List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER> db = (List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_KEYBRANDS_MASTER, true);
            return db.Select(p => new { _name = p.NAME, _vid = (decimal)p.ID })
                   .AsEnumerable()
                   .GroupBy(m => new { _name = m._name, _vid = m._vid }).AsEnumerable()
                   .ToList().Select(m => new Models.MarketViewChannelModels { name = m.Key._name, vid = m.Key._vid }).Union(new MarketViewKeybrandsController().GetGroupedByConfigView(KEYBRANDS_VIEW)).ToList();
        }
        //
        // GET: /ValueShare/

        public ActionResult Index()
        {
            return View();
        }

        #region Constants
        private const string _PATH = "~/Views/";
        private const string CONTROLLER = "ValueShareKeybrands";
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
        private const string KEYBRANDS_VIEW = "KEYBRANDS_MASTER_VIEW";
        #endregion

    }
}
