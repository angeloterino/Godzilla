using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using StrawmanApp.Helpers;
using System.IO;
using Excel;
using ExcelApi;
using StrawmanApp.Helpers;
using System.Threading.Tasks;

namespace StrawmanApp.Controllers
{
    public class BOYCfg : Entities.GodzillaEntity.BOY_CONFIG { };
    [Authorize]
    public class FormsController : Controller
    {
        private StrawmanDBLibray.Entities.godzillaDBLibraryEntity db;
        private System.Data.Objects.ObjectSet<Entities.GodzillaEntity.v_WRK_BOY_CUSTOM_DATA> tbases;
        private System.Data.Objects.ObjectSet<StrawmanDBLibray.Entities.v_WRK_BOY_BASIC_DATA> tbase;
        //
        // GET: /Forms/

        public ActionResult Index()
        {
            Entities.GodzillaEntity.GodzillaEntities gent = new Entities.GodzillaEntity.GodzillaEntities();
            Entities.GodzillaEntity.BOY_CONFIG bc = new Entities.GodzillaEntity.BOY_CONFIG();
            //var v = (from data in gent.BOY_CONFIG
            //         select new { ID = data.ID,BRAND = data.BRAND, CHANNEL = data.CHANNEL, EntityKey = data.EntityKey, MARKET = data.MARKET, NTS_NAME = data.NTS_NAME, NTS_ORDER = data.NTS_ORDER });
            var v = from data in gent.BOY_CONFIG.AsQueryable()
                    where data.NTS_NAME != null group new Entities.BoyConfigModel { vorder = (int)data.NTS_ORDER, name = data.NTS_NAME}
                    by data.NTS_NAME into ALT
                    select  ALT;
            var ret = gent.BOY_CONFIG.Where(m => m.NTS_NAME != null)
                    .GroupBy(m => new { m.NTS_NAME, m.NTS_ORDER })
                    .Select(m => new Entities.BoyConfigModel { vorder = m.Key.NTS_ORDER, name = m.Key.NTS_NAME });
                                
            return View(ret.ToList());
        }
        public ActionResult EditForm(string name,int order)
        {
            Entities.GodzillaEntity.GodzillaEntities gent = new Entities.GodzillaEntity.GodzillaEntities();             
            ViewBag.Result = name;
            var ret = gent.BOY_CONFIG.Where(m => m.NTS_NAME == name &&  m.NTS_ORDER == order)                    
                    .Select(m => new Entities.BoyConfigModel { vorder = m.NTS_ORDER, name = m.NTS_NAME,brand = m.BRAND, market = m.MARKET});
            var lst = new List<Entities.BoyConfigModel>();
            foreach (Entities.BoyConfigModel b in ret.ToList())
            {
                var query = gent.BRAND_MASTER.Where(m => m.ID == b.brand && m.MARKET == b.market)
                                .Select(m => m).FirstOrDefault();
                var markets = gent.MARKET_MASTER.Where(m => m.ID == query.MARKET).Select(m => m).FirstOrDefault();
                lst.Add(new Entities.BoyConfigModel
                            { market = query.MARKET,
                              brand = query.ID,
                              market_name = markets.NAME,
                              brand_name = query.NAME,
                              name = b.name,
                              vorder = b.vorder
                            });
            }
            return PartialView("~/Views/Forms/_Result.cshtml",lst);
        }
        public ActionResult InsertData(string name, int order)
        {
            Entities.GodzillaEntity.GodzillaEntities gent = new Entities.GodzillaEntity.GodzillaEntities();
            var lst = gent.BOY_CONFIG.Where(m => m.NTS_NAME == name && m.NTS_ORDER == order).Select(m => m);
            System.Linq.IQueryable<Entities.GodzillaEntity.BRAND_MASTER> grp = gent.BRAND_MASTER.Select(m => m);
            List<Entities.GodzillaEntity.BRAND_MASTER> ret = grp.ToList();
            foreach (Entities.GodzillaEntity.BOY_CONFIG b in lst)
            {
                ret.RemoveAll(m => m.ID == b.BRAND && m.MARKET == b.MARKET);
            }
            Entities.ListModel lm = new Entities.ListModel();
            lm.datamaster = ret;
            lm.brand_master = ret.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.NAME.ToString()
            });
            return PartialView("~/Views/Forms/_List.cshtml", lm);
        }
        public ActionResult InsertField(Entities.ListModel var)
        {
            string name="";
            int order=0;
            string id = var.selectedId;
            Entities.GodzillaEntity.GodzillaEntities gent = new Entities.GodzillaEntity.GodzillaEntities();
            gent.BOY_CONFIG.AddObject(new Entities.GodzillaEntity.BOY_CONFIG { BRAND = 0, MARKET = 0, CHANNEL = 0, ID = 0, NTS_NAME = "", NTS_ORDER = 0 });
            gent.SaveChanges();
            return PartialView(EditForm(name, order));
        }

        #region BOYS Tabs
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOYMassMarket()
        {
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/GetBOYMassMarket";
            channel = StrawmanConstants.getChannel(StrawmanConstants.CHANNEL_MASS);
            BOYConfigure();
            return View(BOY_CONFIGURE);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOYOTC()
        {
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/GetBOYOTC";
            channel = StrawmanConstants.getChannel(StrawmanConstants.CHANNEL_OTC);
            BOYConfigure();
            return View(BOY_CONFIGURE);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBOYBeauty()
        {
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/GetBOYBeauty";
            channel = StrawmanConstants.getChannel(StrawmanConstants.CHANNEL_BEAUTY);
            BOYConfigure();
            return View(BOY_CONFIGURE);
        }
        #endregion

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult BOYConfigure()
        {
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/BOYConfigure";
            if (channel == 0) channel = StrawmanConstants.getChannel(StrawmanConstants.CHANNEL_MASS);
            RefreshDataCache();
            object formModel = GetSessionObject("BOYFormModel");
            //List<Models.BOYFormModel> formModel = GetBOYFormModel();
            //SetSessionObject("BOYFormModel", formModel);

            return View(formModel);
        }
        private List<Models.BoyMassMarketModels> GetBoyCalcCustomData(string type)
        {
            IEnumerable<Models.BoyMassMarketModels> query = null;            
            switch (type)
            {
                case "INT":
                    if (GetSessionData("v_WRK_BOY_CALC_DATA_INT") == null)
                    {
                        SetSessionData("v_WRK_BOY_CALC_DATA_INT");
                    }
                    query = from p in (IEnumerable<Models.BoyMassMarketModels>)GetSessionData("v_WRK_BOY_CALC_DATA_INT")
                            where p.type == type && p.channel == channel
                            select p;
                    break;
                case "PBP":
                    if (GetSessionData("v_WRK_BOY_CALC_DATA_PBP") == null)
                    {
                        SetSessionData("v_WRK_BOY_CALC_DATA_PBP");
                    }
                    query = from p in (IEnumerable<Models.BoyMassMarketModels>)GetSessionData("v_WRK_BOY_CALC_DATA_PBP")
                            where p.type == type && p.channel == channel
                            select p;                   
                    break;
                case "LE":
                    if (GetSessionData("v_WRK_BOY_CALC_DATA_LE") == null)
                    {
                        SetSessionData("v_WRK_BOY_CALC_DATA_LE");
                    }
                    query = from p in (IEnumerable<Models.BoyMassMarketModels>)GetSessionData("v_WRK_BOY_CALC_DATA_LE")
                            where p.type == type && p.channel == channel
                            select p;                     
                    break;
                default:
                    query = from p in tbases
                            where p.TYPE == type 
                            select new Models.BoyMassMarketModels
                            {
                                channel = p.CHANNEL,
                                brand = p.BRAND,
                                brand_name = p.BRAND_NAME,
                                boy_name = p.NTS_NAME,
                                vgroup = p.GROUP,
                                market = p.MARKET,
                                market_col1 = p.MARKET_COL1,
                                market_pc = p.MARKET_PC,
                                sellin_col1 = p.SELLIN_COL1,
                                sellin_pc = p.SELLIN_PC,
                                sellout_col1 = p.SELLOUT_COL1,
                                sellout_pc = p.SELLOUT_PC,
                                type = p.TYPE,
                                market_name = p.MARKET_NAME,
                                market_type = p.MARKET_TYPE,
                                market_boy = p.MARKET_BOY,
                                market_id = (int)p.MARKET_ID,
                                market_col2 = p.MARKET_COL2,
                                sellin_boy = p.SELLIN_BOY,
                                sellin_col2 = p.SELLIN_COL2,
                                sellin_id = (int)p.SELLIN_ID,
                                sellin_type = p.SELLIN_TYPE,
                                sellout_boy = p.SELLOUT_BOY,
                                sellout_col2 = p.SELLOUT_COL2,
                                sellout_id = (int)p.SELLOUT_ID,
                                sellout_type = p.SELLOUT_TYPE
                            };
                    break;
            }
            return query.ToList();
        }

        private List<Models.BoyMassMarketModels> GetBoyYTDData(string type)
        {
            if (GetSessionData("v_WRK_BOY_BASIC_DATA") == null)
            {
                SetSessionData("v_WRK_BOY_BASIC_DATA");
            }
            List<Models.BoyMassMarketModels> _tbase = GetSessionData("v_WRK_BOY_BASIC_DATA");
            var query = from p in _tbase
                        where p.type == type
                        select p;
            return query.ToList();
        }

        private List<Models.BOYFormModel> GetBOYFormModel()
        {
            List<Models.BOYFormModel> formModel = new List<Models.BOYFormModel>();
            List<Models.BoyMassMarketModels> lst = GetBoyYTDData("YTD").Where(m=> m.channel == channel).Select(m=>m).ToList();
            List<Models.BoyMassMarketModels> _int = GetBoyCalcCustomData("INT");
            List<Models.BoyMassMarketModels> _le = GetBoyCalcCustomData("LE");
            List<Models.BoyMassMarketModels> _btg = GetBoyYTDData("TOGO").Where(m => m.channel == channel).Select(m => m).ToList();
            List<Models.BoyMassMarketModels> _pbp = GetBoyCalcCustomData("PBP");
            Models.BOYFormModel bf = null;
            foreach (Models.BoyMassMarketModels item in lst)
            {
                
                bf = new Models.BOYFormModel(
                        _int.Where(m=>m.brand == item.brand && m.market == item.market && m.channel == item.channel).FirstOrDefault(),
                        _btg.Where(m => m.brand == item.brand && m.market == item.market && m.channel == item.channel).FirstOrDefault(),
                        _pbp.Where(m => m.brand == item.brand && m.market == item.market && m.channel == item.channel).FirstOrDefault(),
                        _le.Where(m => m.brand == item.brand && m.market == item.market && m.channel == item.channel).FirstOrDefault());
                bf.item = item;
                formModel.Add(bf);
            }
            return formModel;
        }

        public ActionResult GroupsConfigure()
        {
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/GroupsConfigure";
            return View();
        }
        
        public ActionResult GetGroups()
        {
            List<StrawmanDBLibray.Entities.GROUP_MASTER> ret = StrawmanDBLibray.Repository.GROUP_MASTER.getAll();
            Entities.GroupListModel lm = new Entities.GroupListModel();
            lm.datamaster = ret;
            lm.group_master = ret.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.NAME.ToString()
            });
            return PartialView("~/Views/Forms/_GroupList.cshtml", lm);
        }
        public ActionResult GetGroupsTypes()
        {
            List<StrawmanDBLibray.Entities.GROUP_TYPES> types = StrawmanDBLibray.Repository.GROUP_TYPES.getAll();
            Entities.GroupListModel lm = new Entities.GroupListModel();
            lm.datatypes = types;
            lm.group_types = types.Select(i => new SelectListItem
            {
                Value = i.ID.ToString(),
                Text = i.NAME
            });
            return PartialView("~/Views/Forms/_GroupList.cshtml", lm);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetGroupsAjax()
        {
            List<StrawmanDBLibray.Entities.GROUP_MASTER> ret = StrawmanDBLibray.Repository.GROUP_MASTER.getAll();
            Entities.GroupListModel lm = new Entities.GroupListModel();
            lm.datamaster = ret;
            lm.group_master = ret.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.NAME.ToString()
            });
            return PartialView("~/Views/Forms/_GroupList.cshtml", lm);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetGroupsConfigAjax(string _id)
        {
            List<StrawmanDBLibray.Entities.GROUP_MASTER> ret = StrawmanDBLibray.Repository.GROUP_MASTER.getAll();
            Entities.GroupListModel lm = new Entities.GroupListModel();
            lm.datamaster = ret;
            lm.group_master = ret.Select(item => new SelectListItem
            {
                Value = item.ID.ToString(),
                Text = item.NAME.ToString()
            });
            return PartialView("~/Views/Forms/_GroupItemsView.cshtml", lm);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult NewGroup()
        {
            Entities.NewGroupModel ngoup = new Entities.NewGroupModel();
            List<SelectListItem> t = new List<SelectListItem>();
            for (int i = 0; i<5; i++){ t.Add(new SelectListItem{ Text = i.ToString(), Value = i.ToString(), Selected = (i==0) });}
            ngoup.levellist = t;
            Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities();
            ngoup.typelist = db.GROUP_TYPES.Select(m =>m).AsEnumerable().Select(item => new SelectListItem
            {
                Text = item.NAME.ToString(), Value = item.ID.ToString()
            });
            return PartialView("~/Views/Forms/_NewGroup.cshtml", ngoup);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult SelectGroup(string _selectedId)
        { 
            Entities.EditGroupModel ngoup = new Entities.EditGroupModel();
            using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            {
                int _id = int.Parse(_selectedId);
                var q = db.GROUP_MASTER.Where(m=>m.ID == (decimal)_id).Select(m => m);
                Entities.GodzillaEntity.GROUP_MASTER s = q.FirstOrDefault();
                ngoup.editcoll = q.ToList();
                ngoup.name = s.NAME;
                ngoup.selectedId = s.ID.ToString();
                List<SelectListItem> t = new List<SelectListItem>();
                for (int i = 0; i < 5; i++) { t.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = (i == 0) }); }                
                ngoup.levellist = t;
                ngoup.level = (int)s.LEVEL;
                ngoup.typelist = db.GROUP_TYPES.Select(m => m).AsEnumerable().Select(item => new SelectListItem
                {
                    Text = item.NAME.ToString(),
                    Value = item.ID.ToString()
                }).ToList();
                ngoup.type = (int)s.TYPE;
            }
            ViewBag.ActionName = "UpdateGroup";
            return PartialView("~/Views/Forms/_EditGroup.cshtml", ngoup);
        }
        
        #region GetKPI
        public ActionResult KPIConfigure()
        {
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/KPIConfigure";
            return View();
        }

        public ActionResult GetKPI()
        {
            using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
            {
                var query = db.KPI_MASTER.Select(m => m);
                List<StrawmanDBLibray.Entities.KPI_MASTER> ret = query.ToList();
                return PartialView("~/Views/Forms/_KPIList.cshtml", ret);
            }
        }
        public ActionResult GetKPIOptions()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            model.Add(new SelectListItem { Text = KPI_COLUMS.NTS, Value = KPI_COLUMS.NTS, Selected = true });
            model.Add(new SelectListItem { Text = KPI_COLUMS.MARKET_SHARE, Value = KPI_COLUMS.MARKET_SHARE });
            model.Add(new SelectListItem { Text = KPI_COLUMS.BRAND_CONTRIBUTION, Value = KPI_COLUMS.BRAND_CONTRIBUTION });
            ViewBag.SelectListName = KPI_SELECT_COLUMNS_NAME;
            ViewBag.SelectListLabel = KPI_SELECT_COLUMNS_LABEL;
            return PartialView(SELECT_LIST_VIEW, model);
        }
        public ActionResult InsertKPI(Entities.ListModel var)
        {
            string name = "";
            int order = 0;
            string id = var.selectedId;
            using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity gent = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
            {
                gent.KPI_MASTER.AddObject(new StrawmanDBLibray.Entities.KPI_MASTER { NAME = "", GROUP = 0 });
                gent.SaveChanges();
            }
            return PartialView(EditForm(name, order));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult SelectKPI(string _selectedId, string _selectValue)
        {
            string _column = _selectValue;
            Entities.EditKPIModel ngoup = new Entities.EditKPIModel();
            
            using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
            {
                int _id = int.Parse(_selectedId);
                var q = db.KPI_CONFIG.Join(db.KPI_MASTER, c => c.KPI, n => n.ID, (c, n) => new { c = c, n = n }).Where(m => m.c.KPI == (decimal)_id).Where(m => m.c.KPI_COLUMN == _column).Select(m => 
                       new Entities.KPIConfig
                            {
                                ID = m.c.ID,
                                MARKET = m.c.MARKET,
                                BRAND = m.c.BRAND,
                                CHANNEL = m.c.CHANNEL,
                                CONFIG = m.c.CONFIG,
                                BRAND_NAME = m.c.BRAND_NAME,
                                KPI = m.c.KPI,
                                KPI_COLUMN = m.c.KPI_COLUMN,
                                ORDER = m.n.ORDER,
                                NAME = m.n.NAME,
                                PARENT = m.n.PARENT
                            });

                ngoup.itemslist = q.OrderBy(m => m.ORDER).ToList();
                
                ngoup.selectedId = _id.ToString();
                ngoup.selectedColumn = _selectValue.ToString();
            }

            ViewBag.ActionName = "UpdateGroup";
            return PartialView("~/Views/Forms/_KPIItemsList.cshtml", ngoup);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ShowKPIList(string _selectedId, string _kpiColumn)
        {
            Entities.EditKPIModel ngroup = new Entities.EditKPIModel();
            ngroup.selectedId = _selectedId;
            ngroup.selectedColumn = _kpiColumn;
            using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
            {                
                int _id = int.Parse(_selectedId);
                var t = db.KPI_MASTER.Where(m => m.ID == (decimal)_id).Select(m => m).FirstOrDefault();
                decimal? _channel = (t != null) ? t.CHANNEL : null;
                string _type = (_kpiColumn != null) ? _kpiColumn : null;
                switch (_kpiColumn)
                {
                    default:
                        ngroup.additemslist = db.WRK_BOY_DATA.Join(db.CHANNEL_MASTER, d => d.CHANNEL, c => c.ID, (d, c) => new { d = d, c = c }).Where(m => m.d.TYPE == "TOTAL" && m.d.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.d.MONTH_PERIOD == Helpers.PeriodUtil.Month
                                                && (m.d.CHANNEL == _channel || _channel == null))
                            .Select(m => new Entities.WrkBoyData
                            {
                                MARKET = m.d.MARKET,
                                BRAND = m.d.BRAND,
                                CHANNEL = m.d.CHANNEL,
                                CHANNEL_NAME = m.c.NAME,
                                BRAND_NAME = m.d.BRAND_NAME,
                                MARKET_NAME = m.d.MARKET_NAME,
                                NTS_NAME = m.d.NTS_NAME,
                                NTS_ORDER = m.d.NTS_ORDER,
                                ID = m.d.ID,
                                GROUP = m.d.GROUP,
                                TYPE = m.d.TYPE,
                                SUM_ROWS = m.d.SUM_ROWS
                            }).OrderBy(m=>new {m.NTS_ORDER, m.BRAND}).ToList();
                        break;
                    case KPI_COLUMS.MARKET_SHARE:
                        ngroup.additemslist = db.WRK_BRAND_BOY.Join(db.CHANNEL_MASTER, d => d.CHANNEL, c => c.ID, (d, c) => new { d = d, c = c })
                                                                .Join(db.MARKET_MASTER, d => d.d.MARKET, ms => ms.ID, (d, ms) => new { d.d, d.c, ms = ms })
                                                                .Join(db.BRAND_MASTER, d => d.d.BRAND, bs => bs.ID, (d, bs) => new { d.d, d.c, d.ms, bs = bs })
                                                                .Where(m => m.d.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.d.MONTH_PERIOD == Helpers.PeriodUtil.Month && (m.d.CHANNEL == _channel || _channel == null)).Select(m => new Entities.WrkBoyData
                        {
                            MARKET = (decimal)m.d.MARKET,
                            BRAND = (decimal)m.d.BRAND,
                            CHANNEL = (decimal)m.d.CHANNEL,
                            CHANNEL_NAME = m.c.NAME,
                            BRAND_NAME = m.bs.NAME,
                            MARKET_NAME = m.ms.NAME,
                            ID = m.d.ID,
                            GROUP = m.d.GROUP,
                        }).ToList();
                        break;
                }
                ngroup.itemslist = db.KPI_CONFIG.Join(db.KPI_MASTER, c => c.KPI, n => n.ID, (c, n) => new { c = c, n = n }).Where(m => m.c.KPI == (decimal)_id).Select(m => 
                    new Entities.KPIConfig
                            {
                                ID = m.c.ID,
                                MARKET = m.c.MARKET,
                                BRAND = m.c.BRAND,
                                CHANNEL = m.c.CHANNEL,
                                CONFIG = m.c.CONFIG,
                                BRAND_NAME = m.c.BRAND_NAME,
                                KPI = m.c.KPI,
                                KPI_COLUMN = m.c.KPI_COLUMN,
                                ORDER = m.n.ORDER,
                                NAME = m.n.NAME,
                                PARENT = m.n.PARENT
                            }).ToList();

            }
            ViewBag.ActionName = "UpdateItemsKPI";
            return PartialView("~/Views/Forms/_KPIItemsList.cshtml", ngroup);
        }

        [HttpPost]
        public JsonResult UpdateItemsKPI(FormCollection data)
        {
            int result = 0;
            List<int> selected = new List<int>();
            try
            {
                int selectedId = int.Parse(data["selectedId"]);
                string datos = data["selectedItems"];
                string selectedColumn = data["selectedColumn"];
                foreach (string value in datos.Split(','))
                {                    
                    if(!value.Equals("false")){
                        int sel = int.Parse(value);
                        selected.Add(sel);
                    }
                }
                // TODO: Add insert logic here
                if (selected != null)
                {
                    using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
                    {
                        var q = db.KPI_CONFIG.Where(m => m.KPI == selectedId && m.KPI_COLUMN == selectedColumn).Select(m => m);
                        decimal? parent = 0;
                        foreach (StrawmanDBLibray.Entities.KPI_CONFIG item in q.ToList())
                        {
                            parent = db.KPI_MASTER.Where(m => m.ID == item.KPI).Select(m=>m.PARENT).FirstOrDefault();
                            db.KPI_CONFIG.DeleteObject(item);
                        }
                        switch(selectedColumn){
                            default:
                                foreach (int i in selected)
                                {
                                    var t = db.WRK_BOY_DATA.Where(m => m.ID == i && m.BRAND < 9000 && m.MARKET < 9000).FirstOrDefault();
                                    if (t != null)
                                    {
                                        db.KPI_CONFIG.AddObject(new StrawmanDBLibray.Entities.KPI_CONFIG
                                        {
                                            MARKET = t.MARKET,
                                            BRAND = t.BRAND,
                                            BRAND_NAME = (t.BRAND_NAME == null) ? t.NTS_NAME : t.BRAND_NAME,
                                            CHANNEL = t.CHANNEL,
                                            KPI = selectedId,
                                            KPI_COLUMN = KPI_COLUMS.NTS,
                                            CONFIG = 1
                                        });
                                    }                                 
                                }
                                break;
                            case KPI_COLUMS.MARKET_SHARE:
                                foreach (int i in selected)
                                {
                                    var t = db.WRK_BRAND_BOY.Join(db.BRAND_MASTER, c=>c.BRAND, d=> d.ID, (c,d)=>new{c,d}).Where(m => m.c.ID == i && m.c.BRAND < 9000 && m.c.MARKET < 9000).FirstOrDefault();
                                    if (t != null)
                                    {
                                        db.KPI_CONFIG.AddObject(new StrawmanDBLibray.Entities.KPI_CONFIG
                                        {
                                            MARKET = t.c.MARKET,
                                            BRAND = t.c.BRAND,
                                            BRAND_NAME = t.d.NAME,
                                            CHANNEL = t.c.CHANNEL,
                                            KPI = selectedId,
                                            KPI_COLUMN = KPI_COLUMS.MARKET_SHARE,
                                            CONFIG = 1
                                        });
                                    }
                                }
                                break;
                        }
                        //Actualizar parents
                        //while(parent != null){
                        //    //actualizar el pariente con los datos de los hijos en la tabla CONFIG
                        //    //Borrar datos
                        //    //Insertar nuevos datos
                        //    var d = db.KPI_CONFIG.Where(m=>m.KPI == parent).Select(m=>m);
                        //    foreach(StrawmanDBLibray.Entities.KPI_CONFIG cfg in d.ToList()){
                        //        db.KPI_CONFIG.DeleteObject(cfg);
                        //    }
                        //    var g = db.KPI_CONFIG.Join(db.KPI_MASTER, c => c.KPI, n => n.ID, (c, n) => new { c = c, n = n }).Where(m => m.n.PARENT == parent).Select(t=>t);
                        //    foreach (StrawmanDBLibray.Entities.KPI_MASTER mst in g.ToList()){
                        //        var s = db.KPI_CONFIG.Where(m=>m.KPI == mst.c.ID).Select(m=>m).FirstOrDefault();
                        //    }
                        //    //Comprobamos que este item tiene definido un pariente. Si es así asignamos el nuevo pariente para realizar la misma operación
                        //    parent =
                        //}
                        result = db.SaveChanges();
                    }
                }
                return new JsonResult() { Data = new { Success = true, Result = result } };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
        }


        #endregion
        
        //
        // GET: /Forms/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Forms/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Forms/Create

        [HttpPost]
        public JsonResult InsertGroup(Entities.NewGroupModel collection)
        {
            int result = 0;
            try
            {
                // TODO: Add insert logic here
                using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                {
                    var q = db.GROUP_TYPES.Where(m => m.ID == collection.type).Select(m => m.BASE_ID).FirstOrDefault();

                    int value = (int)q;


                    db.GROUP_MASTER.AddObject(new Entities.GodzillaEntity.GROUP_MASTER
                    {
                        BASE_ID = value,
                        LEVEL = collection.level,
                        TYPE = collection.type,
                        NAME = collection.name
                    });

                    result = db.SaveChanges();
                }

                return new JsonResult() { Data = new { Success = true, Result = result } };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
        }
        public JsonResult UpdateGroup(Entities.NewGroupModel collection)
        {
            int result = 0;
            try
            {
                // TODO: Add insert logic here
                using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                {
                    var q = db.GROUP_TYPES.Where(m => m.ID == collection.type).Select(m => m.BASE_ID).FirstOrDefault();

                    int value = (int)q;

                    int _id = int.Parse(collection.selectedId);

                    Entities.GodzillaEntity.GROUP_MASTER u = db.GROUP_MASTER.Where(i => i.ID == (decimal)_id).Select(m => m).FirstOrDefault();

                    if (u!=null){
                                            
                        u.BASE_ID = (collection.type != u.TYPE)?value:u.BASE_ID;
                        u.LEVEL = (collection.level != u.LEVEL)?collection.level:u.LEVEL;
                        u.TYPE = (collection.type != u.TYPE)?collection.type:u.TYPE;
                        u.NAME = (collection.name != u.NAME) ? collection.name : u.NAME;
                    };

                    result = db.SaveChanges();
                }

                return new JsonResult() { Data = new { Success = true, Result = result } };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
        }
        public JsonResult DeleteGroup(Entities.NewGroupModel collection)
        {
            int result = 0;
            try
            {
                using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                {
                    int _id = int.Parse(collection.selectedId);

                    Entities.GodzillaEntity.GROUP_MASTER u = db.GROUP_MASTER.Where(i => i.ID == (decimal)_id).Select(m => m).FirstOrDefault();

                    if (u != null)
                    {
                        db.GROUP_MASTER.DeleteObject(u);
                    }
                    db.SaveChanges();
                }

                return new JsonResult() { Data = new { Success = true, Result = result } };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
        }

        public JsonResult UpdateBoy(FormCollection collection)
        {
            int result = 0;
            string var_type = "";
            string _message = "Actualización BOY";
            try
            {
                //Recuperamos la caché del modelo
                List<Entities.EditBOYModel> list = (List<Entities.EditBOYModel>)GetSessionObject("EditBOYModel");
                this.channel = 0;
                // TODO: Add Update logic here
                using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
                {
                    db.CommandTimeout = 50000;
                    _message = "Análisis del modelo";
                    foreach (Entities.EditBOYModel model in list)
                    {
                        this.channel = (int)model._channel;
                        //INT
                        Entities.EditBOY qint = model.INT;
                        ///PBP
                        Entities.EditBOY qpbp = model.PBP;
                        //LE
                        Entities.EditBOY qle = model.BTG;

                        //Actualizamos datos MARKET
                        List<StrawmanDBLibray.Entities.STRWM_BOY_DATA> sbd = db.STRWM_BOY_DATA.Where(m => m.ID == qint.market_id || m.ID == qpbp.market_id || m.ID == qle.market_id).Select(m => m).ToList();
                        sbd.Find(m => m.ID == qint.market_id).INT = qint.market_pc_f;
                        sbd.Find(m => m.ID == qint.market_id).USER = Helpers.UserUtils.UserName;
                        sbd.Find(m => m.ID == qint.market_id).LAST_DATE = DateTime.Now;
                        sbd.Find(m => m.ID == qle.market_id).BTG = qle.market_pc_f;
                        sbd.Find(m => m.ID == qle.market_id).USER = Helpers.UserUtils.UserName;
                        sbd.Find(m => m.ID == qle.market_id).LAST_DATE = DateTime.Now;
                        sbd.Find(m => m.ID == qpbp.market_id).PBP = qpbp.market_pc_f;
                        sbd.Find(m => m.ID == qpbp.market_id).USER = Helpers.UserUtils.UserName;
                        sbd.Find(m => m.ID == qpbp.market_id).LAST_DATE = DateTime.Now;
                       
                        //Actualizamos datos SELLOUT
                        List<StrawmanDBLibray.Entities.STRWM_BOY_DATA> sbso = db.STRWM_BOY_DATA.Where(m => m.ID == qint.sellout_id || m.ID == qpbp.sellout_id || m.ID == qle.sellout_id).Select(m => m).ToList();
                        sbso.Find(m => m.ID == qint.sellout_id).INT = qint.sellout_pc_f;
                        sbso.Find(m => m.ID == qint.sellout_id).USER = Helpers.UserUtils.UserName;
                        sbso.Find(m => m.ID == qint.sellout_id).LAST_DATE = DateTime.Now;
                        sbso.Find(m => m.ID == qle.sellout_id).BTG = qle.sellout_pc_f;
                        sbso.Find(m => m.ID == qle.sellout_id).USER = Helpers.UserUtils.UserName;
                        sbso.Find(m => m.ID == qle.sellout_id).LAST_DATE = DateTime.Now;
                        sbso.Find(m => m.ID == qpbp.sellout_id).PBP = qpbp.sellout_pc_f;
                        sbso.Find(m => m.ID == qpbp.sellout_id).USER = Helpers.UserUtils.UserName;
                        sbso.Find(m => m.ID == qpbp.sellout_id).LAST_DATE = DateTime.Now;

                        //Actualizamos datos SELLIN
                        List<StrawmanDBLibray.Entities.STRWM_BOY_DATA> sbsi = db.STRWM_BOY_DATA.Where(m => m.ID == qint.sellin_id || m.ID == qpbp.sellin_id || m.ID == qle.sellin_id).Select(m => m).ToList();
                        sbsi.Find(m => m.ID == qint.sellin_id).INT = qint.sellin_pc_f;
                        sbsi.Find(m => m.ID == qint.sellin_id).USER = Helpers.UserUtils.UserName;
                        sbsi.Find(m => m.ID == qint.sellin_id).LAST_DATE = DateTime.Now;
                        sbsi.Find(m => m.ID == qle.sellin_id).BTG = qle.sellin_pc_f;
                        sbsi.Find(m => m.ID == qle.sellin_id).USER = Helpers.UserUtils.UserName;
                        sbsi.Find(m => m.ID == qle.sellin_id).LAST_DATE = DateTime.Now;
                        sbsi.Find(m => m.ID == qpbp.sellin_id).PBP = qpbp.sellin_pc_f;
                        sbsi.Find(m => m.ID == qpbp.sellin_id).USER = Helpers.UserUtils.UserName;
                        sbsi.Find(m => m.ID == qpbp.sellin_id).LAST_DATE = DateTime.Now;

                        //Actualizamos datos WRK_BOY_DATA
                        
                        List<StrawmanDBLibray.Entities.WRK_BOY_DATA> data =   db.WRK_BOY_DATA
                                                                        .Where(m => m.CHANNEL == model._channel && m.MARKET == model._market && m.BRAND == model._brand && m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                                                                        .Select(m=>m).ToList();

                        //INT
                        var_type = "INT";

                        data.Find(m => m.TYPE == var_type).MARKET_COL1 = (decimal?)qint.market_col1;
                        data.Find(m => m.TYPE == var_type).MARKET_COL2 = (decimal?)qint.market_col2;
                        data.Find(m => m.TYPE == var_type).MARKET_PC = qint.market_pc;
                        data.Find(m => m.TYPE == var_type).SELLIN_COL1 = (decimal?)qint.sellin_col1;
                        data.Find(m => m.TYPE == var_type).SELLIN_COL2 = (decimal?)qint.sellin_col2;
                        data.Find(m => m.TYPE == var_type).SELLIN_PC = qint.sellin_pc;
                        data.Find(m => m.TYPE == var_type).SELLOUT_COL1 = (decimal?)qint.sellout_col1;
                        data.Find(m => m.TYPE == var_type).SELLOUT_COL2 = (decimal?)qint.sellout_col2;
                        data.Find(m => m.TYPE == var_type).SELLOUT_PC = qint.sellout_pc;
                        data.Find(m => m.TYPE == var_type).CONVERSION_RATE_COL1 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLIN_COL1 , data.Find(m => m.TYPE == var_type).SELLOUT_COL1,null);
                        data.Find(m => m.TYPE == var_type).CONVERSION_RATE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLIN_COL2 , data.Find(m => m.TYPE == var_type).SELLOUT_COL2,null);
                        data.Find(m => m.TYPE == var_type).SHARE_COL1 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLOUT_COL1 , data.Find(m => m.TYPE == var_type).MARKET_COL1,null);
                        data.Find(m => m.TYPE == var_type).SHARE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLOUT_COL2 , data.Find(m => m.TYPE == var_type).MARKET_COL2,null);
                        SetBOYPercent(ref data, var_type);
                        data.Find(m => m.TYPE == var_type).SHARE_PC = CheckShare(data.Find(m => m.TYPE == var_type).SHARE_COL1, data.Find(m => m.TYPE == "TOTAL").SHARE_COL2);

                        //LE
                        var_type = "LE";

                        data.Find(m => m.TYPE == var_type).MARKET_COL1 = (decimal?)qle.market_col1;
                        data.Find(m => m.TYPE == var_type).MARKET_COL2 = (decimal?)qle.market_col1 -(decimal?)qint.market_col1;
                        data.Find(m => m.TYPE == var_type).MARKET_PC = CheckDiv((decimal?)qle.market_col1, data.Find(m => m.TYPE == "TOTAL").MARKET_COL2,null) - 1;
                        data.Find(m => m.TYPE == var_type).MARKET_PC_COL2 = CheckDiv((decimal?)qle.market_col1, (decimal?)qint.market_col1,null) - 1;
                        data.Find(m => m.TYPE == var_type).SELLIN_COL1 = (decimal?)qle.sellin_col1;
                        data.Find(m => m.TYPE == var_type).SELLIN_COL2 = (decimal?)qle.sellin_col1 - (decimal?)qint.sellin_col1;
                        data.Find(m => m.TYPE == var_type).SELLIN_PC = CheckDiv((decimal?)qle.sellin_col1, data.Find(m => m.TYPE == "TOTAL").SELLIN_COL2,null) - 1;
                        data.Find(m => m.TYPE == var_type).SELLIN_PC_COL2 = CheckDiv((decimal?)qle.sellin_col1, (decimal?)qint.sellin_col1,null) - 1;
                        data.Find(m => m.TYPE == var_type).SELLOUT_COL1 = (decimal?)qle.sellout_col1;
                        data.Find(m => m.TYPE == var_type).SELLOUT_COL2 = (decimal?)qle.sellout_col1 - (decimal?)qint.sellout_col1;
                        data.Find(m => m.TYPE == var_type).SELLOUT_PC = CheckDiv((decimal?)qle.sellout_col1, data.Find(m => m.TYPE == "TOTAL").SELLOUT_COL2,null) - 1;
                        data.Find(m => m.TYPE == var_type).SELLOUT_PC_COL2 = CheckDiv((decimal?)qle.sellout_col1, (decimal?)qint.sellout_col1,null) - 1;
                        data.Find(m => m.TYPE == var_type).CONVERSION_RATE_COL1 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLIN_COL1, data.Find(m => m.TYPE == var_type).SELLOUT_COL1,null);
                        data.Find(m => m.TYPE == var_type).CONVERSION_RATE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLIN_COL2, data.Find(m => m.TYPE == var_type).SELLOUT_COL2,null);
                        data.Find(m => m.TYPE == var_type).SHARE_COL1 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLOUT_COL1, data.Find(m => m.TYPE == var_type).MARKET_COL1,null);
                        data.Find(m => m.TYPE == var_type).SHARE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLOUT_COL2, data.Find(m => m.TYPE == var_type).MARKET_COL2,null);
                        //BTG
                        data.Find(m => m.TYPE == var_type).MARKET_BTG = (decimal?)qle.market_col2;
                        data.Find(m => m.TYPE == var_type).SELLIN_BTG = (decimal?)qle.sellin_col2;
                        data.Find(m => m.TYPE == var_type).SELLOUT_BTG = (decimal?)qle.sellout_col2;
                        SetBOYPercent(ref data, var_type);
                        data.Find(m => m.TYPE == var_type).SHARE_PC = CheckShare(data.Find(m => m.TYPE == var_type).SHARE_COL1, data.Find(m => m.TYPE == "TOTAL").SHARE_COL2);
                        data.Find(m => m.TYPE == var_type).SHARE_PC_COL2 = CheckShare(data.Find(m => m.TYPE == var_type).SHARE_COL1,data.Find(m => m.TYPE == "INT").SHARE_COL1);

                        //PBP
                        var_type = "PBP";

                        data.Find(m => m.TYPE == var_type).MARKET_COL1 = (decimal?)qpbp.market_col1;
                        data.Find(m => m.TYPE == var_type).MARKET_COL2 = (decimal?)qpbp.market_col2;
                        data.Find(m => m.TYPE == var_type).MARKET_PC = qpbp.market_pc;
                        data.Find(m => m.TYPE == var_type).SELLIN_COL1 = (decimal?)qpbp.sellin_col1;
                        data.Find(m => m.TYPE == var_type).SELLIN_COL2 = (decimal?)qpbp.sellin_col2;
                        data.Find(m => m.TYPE == var_type).SELLIN_PC = qpbp.sellin_pc;
                        data.Find(m => m.TYPE == var_type).SELLOUT_COL1 = (decimal?)qpbp.sellout_col1;
                        data.Find(m => m.TYPE == var_type).SELLOUT_COL2 = (decimal?)qpbp.sellout_col2;
                        data.Find(m => m.TYPE == var_type).SELLOUT_PC = qpbp.sellout_pc;
                        data.Find(m => m.TYPE == var_type).CONVERSION_RATE_COL1 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLIN_COL1, data.Find(m => m.TYPE == var_type).SELLOUT_COL1,null);
                        data.Find(m => m.TYPE == var_type).CONVERSION_RATE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLIN_COL2, data.Find(m => m.TYPE == var_type).SELLOUT_COL2,null);
                        data.Find(m => m.TYPE == var_type).SHARE_COL1 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLOUT_COL1, data.Find(m => m.TYPE == var_type).MARKET_COL1,null);
                        data.Find(m => m.TYPE == var_type).SHARE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLOUT_COL2, data.Find(m => m.TYPE == var_type).MARKET_COL2,null);
                        SetBOYPercent(ref data, var_type);
                        data.Find(m => m.TYPE == var_type).SHARE_PC = CheckShare(data.Find(m => m.TYPE == var_type).SHARE_COL1, data.Find(m => m.TYPE == "LE").SHARE_COL1);
                        //TOGO
                        var_type = "TOGO";

                        data.Find(m => m.TYPE == var_type).MARKET_COL2 = (decimal?)qle.market_col2;
                        data.Find(m => m.TYPE == var_type).MARKET_PC =qle.market_pc;
                        data.Find(m => m.TYPE == var_type).SELLIN_COL2 = (decimal?)qle.sellin_col2;
                        data.Find(m => m.TYPE == var_type).SELLIN_PC = qle.sellin_pc;
                        data.Find(m => m.TYPE == var_type).SELLOUT_COL2 = (decimal?)qle.sellout_col2;
                        data.Find(m => m.TYPE == var_type).SELLOUT_PC = qle.sellout_pc;
                        data.Find(m => m.TYPE == var_type).CONVERSION_RATE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLIN_COL2, data.Find(m => m.TYPE == var_type).SELLOUT_COL2,null);
                        data.Find(m => m.TYPE == var_type).SHARE_COL2 = CheckDiv(data.Find(m => m.TYPE == var_type).SELLOUT_COL2, data.Find(m => m.TYPE == var_type).MARKET_COL2,null);
                        SetBOYPercent(ref data, var_type);
                        data.Find(m => m.TYPE == var_type).SHARE_PC = CheckShare(data.Find(m => m.TYPE == var_type).SHARE_COL2, data.Find(m => m.TYPE == var_type).SHARE_COL1);
                    }
                    //Actualizar datos de los sumatorios y de los canales
                    //localizamos los datos de los registros a actualizar. 
                    //220916: Añadida la opción NTS_ORDER > 0 para evitar que calcule sumatorios no definidos.
                    _message = "Cálculo sumatorios";
                    var j = db.WRK_BOY_DATA.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.MARKET < 9000 && m.BRAND < 9000 && m.NTS_ORDER > 0)
                            .AsEnumerable()
                            .Join(list, m => new { _channel = (decimal?)m.CHANNEL, _market = (decimal?)m.MARKET, _brand = (decimal?)m.BRAND }
                                        , y => new { y._channel, y._market, y._brand }
                                        , (m, y) => m)                            
                            .Select(m=>new {nts_order = m.NTS_ORDER,channel = m.CHANNEL, year_period = m.YEAR_PERIOD, month_period = m.MONTH_PERIOD}).Distinct();
                    foreach (var ji in j){
                        var prev = db.WRK_BOY_DATA.Where(m => m.YEAR_PERIOD == ji.year_period && m.MONTH_PERIOD == ji.month_period && m.NTS_ORDER == ji.nts_order && m.CHANNEL == ji.channel && m.MARKET < 9000 && m.BRAND < 9000)
                            .AsEnumerable()
                            .Join(db.BOY_CONFIG.Where(m => m.CONSOLIDATE == null || m.CONSOLIDATE == 1).AsEnumerable()
                                                , m => new { _channel = m.CHANNEL, _brand = m.BRAND, _market = m.MARKET }
                                                , n => new { _channel = (decimal)n.CHANNEL, _brand = (decimal)n.BRAND, _market = (decimal)n.MARKET }
                                                , (m, n) => new { m = m, n = n });
                        var conf = prev
                            .Select(m => new StrawmanDBLibray.Entities.WRK_BOY_DATA
                            {
                                NTS_ORDER = m.m.NTS_ORDER,
                                CHANNEL = m.m.CHANNEL,
                                TYPE = m.m.TYPE,
                                MARKET = m.m.MARKET,
                                BRAND = m.m.BRAND,
                                MARKET_COL1 = m.m.MARKET_COL1 * (decimal?)(m.n.MARKET_CONFIG?? 1),
                                MARKET_COL2 = m.m.MARKET_COL2 * (decimal?)(m.n.MARKET_CONFIG ?? 1),
                                SELLIN_COL1 = m.m.SELLIN_COL1 * (decimal?)(m.n.SELLIN_CONFIG ?? 1),
                                SELLIN_COL2 = m.m.SELLIN_COL2 * (decimal?)(m.n.SELLIN_CONFIG ?? 1),
                                SELLOUT_COL1 = m.m.SELLOUT_COL1 * (decimal?)(m.n.SELLOUT_CONFIG ?? 1),
                                SELLOUT_COL2 = m.m.SELLOUT_COL2 * (decimal?)(m.n.SELLOUT_CONFIG ?? 1),
                                MARKET_BTG = m.m.MARKET_BTG,
                                SELLIN_BTG = m.m.SELLIN_BTG,
                                SELLOUT_BTG = m.m.SELLOUT_BTG,
                            })
                            .AsEnumerable();
                        var sum = conf
                            .GroupBy(m => new { m.NTS_ORDER, m.CHANNEL, m.TYPE })
                            .Select(m => new StrawmanDBLibray.Entities.WRK_BOY_DATA
                            {
                                NTS_ORDER = m.Key.NTS_ORDER,
                                CHANNEL = m.Key.CHANNEL,
                                TYPE = m.Key.TYPE,
                                MARKET_COL1 = m.Sum(t => t.MARKET_COL1),
                                MARKET_COL2 = m.Sum(t => t.MARKET_COL2),
                                SELLIN_COL1 = m.Sum(t => t.SELLIN_COL1),
                                SELLIN_COL2 = m.Sum(t => t.SELLIN_COL2),
                                SELLOUT_COL1 = m.Sum(t => t.SELLOUT_COL1),
                                SELLOUT_COL2 = m.Sum(t => t.SELLOUT_COL2),
                                MARKET_BTG = m.Sum(t => t.MARKET_BTG),
                                SELLIN_BTG = m.Sum(t => t.SELLIN_BTG),
                                SELLOUT_BTG = m.Sum(t => t.SELLOUT_BTG),
                                MARKET = m.Max(t => t.MARKET) + 9000,
                                BRAND = m.Max(t => t.BRAND) + 9000
                            }).ToList();
                        if (sum.Count == 0) continue;//No es necesario actualizar el sumatorio de los que no consolidan ya que no tienen.
                        var _int = sum.Find(n=>n.TYPE == "INT");
                        var _le = sum.Find(n => n.TYPE == "LE");
                        var _pbp = sum.Find(n => n.TYPE == "PBP");
                        var _togo = sum.Find(n => n.TYPE == "TOGO");
                        var _ytd = sum.Find(n => n.TYPE == "YTD");
                        var _total = sum.Find(n => n.TYPE == "TOTAL");

                        var _aux = _int;
                        object group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _total = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _total.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _int = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _le, _total, ref group, "WRK_BOY_DATA");


                        _aux = _le;
                        group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _le = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _int, _total, ref group, "WRK_BOY_DATA");


                        _aux = _togo;
                        group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _togo = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _le, _ytd, ref group, "WRK_BOY_DATA");


                        _aux = _pbp;
                        group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _pbp = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _le, null, ref group, "WRK_BOY_DATA");

                    }
                    //Actualizar agrupaciones personalizadas
                    _message = "Cálculo sumatorios configurables";
                    var per = db.GROUP_CONFIG.Where(m => m.TYPE_ID == 21)
                                .AsEnumerable()
                                .Join(list, m => new { _brand = m.BRAND, _market = m.MARKET }, n => new { _brand = n._brand, _market = n._market }, (m, n) => new { m = m, n = n })
                                .Select(m => new {_group = m.m.GROUP_ID, _type = m.m.TYPE_ID}).Distinct();
                    foreach(var ji in per){
                        var _grouped = db.GROUP_CONFIG.Where(f => f.TYPE_ID == ji._type && f.GROUP_ID == ji._group).Select(m=>m).AsEnumerable();
                        var bcfg = db.BOY_CONFIG
                            .Join(_grouped,m=>new{_brand = m.BRAND, _market = m.MARKET},n=>new{_brand= n.BRAND,_market =n.MARKET}, (m,n)=>new{m=m,n=n}).AsEnumerable()
                                        .Select(m => new { _brand = m.m.BRAND, _market = m.m.MARKET, _channel = m.m.CHANNEL, _group = ji._group, _marketcfg = m.m.MARKET_CONFIG, _selloutcfg = m.m.SELLOUT_CONFIG, _sellincfg = m.m.SELLIN_CONFIG, _base = m.n.GROUP_MASTER.BASE_ID }).AsEnumerable();
                        var cgroup = db.WRK_BOY_DATA.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.MARKET < 9000 && m.BRAND < 9000 && m.NTS_ORDER > 0)
                                .AsEnumerable()
                                .Join(bcfg, m => new { _brand = m.BRAND, _market = m.MARKET }, n => new { _brand = (decimal)n._brand, _market = (decimal)n._market }, (m, n) => new { m = m, n = n })
                                .Select(m => new StrawmanDBLibray.Entities.WRK_BOY_DATA
                                    {
                                        GROUP = m.n._group,
                                        NTS_ORDER = m.m.NTS_ORDER,
                                        CHANNEL = m.m.CHANNEL,
                                        TYPE = m.m.TYPE,
                                        MARKET = m.m.MARKET + (decimal)(m.n._base == null?1:m.n._base),
                                        BRAND = m.m.BRAND + (decimal)(m.n._base == null?1:m.n._base),
                                        MARKET_COL1 = m.m.MARKET_COL1 * (decimal?)(m.n._marketcfg == null ? 1 : m.n._marketcfg),
                                        MARKET_COL2 = m.m.MARKET_COL2 * (decimal?)(m.n._marketcfg == null ? 1 : m.n._marketcfg),
                                        SELLIN_COL1 = m.m.SELLIN_COL1 * (decimal?)(m.n._sellincfg == null ? 1 : m.n._sellincfg),
                                        SELLIN_COL2 = m.m.SELLIN_COL2 * (decimal?)(m.n._sellincfg == null ? 1 : m.n._sellincfg),
                                        SELLOUT_COL1 = m.m.SELLOUT_COL1 * (decimal?)(m.n._selloutcfg == null ? 1 : m.n._selloutcfg),
                                        SELLOUT_COL2 = m.m.SELLOUT_COL2 * (decimal?)(m.n._selloutcfg == null ? 1 : m.n._selloutcfg),
                                        MARKET_BTG = m.m.MARKET_BTG,
                                        SELLIN_BTG = m.m.SELLIN_BTG,
                                        SELLOUT_BTG = m.m.SELLOUT_BTG
                                    })
                                    .AsEnumerable()
                                    .GroupBy(m => new { m.GROUP, m.TYPE })
                                    .Select(m => new StrawmanDBLibray.Entities.WRK_BOY_DATA
                                    {
                                        NTS_ORDER = m.Max(t => t.NTS_ORDER),
                                        CHANNEL = m.Max(t => t.CHANNEL),
                                        TYPE = m.Key.TYPE,
                                        MARKET_COL1 = m.Sum(t => t.MARKET_COL1),
                                        MARKET_COL2 = m.Sum(t => t.MARKET_COL2),
                                        SELLIN_COL1 = m.Sum(t => t.SELLIN_COL1),
                                        SELLIN_COL2 = m.Sum(t => t.SELLIN_COL2),
                                        SELLOUT_COL1 = m.Sum(t => t.SELLOUT_COL1),
                                        SELLOUT_COL2 = m.Sum(t => t.SELLOUT_COL2),
                                        MARKET_BTG = m.Sum(t => t.MARKET_BTG),
                                        SELLIN_BTG = m.Sum(t => t.SELLIN_BTG),
                                        SELLOUT_BTG = m.Sum(t => t.SELLOUT_BTG),
                                        MARKET = m.Max(t => t.MARKET),
                                        BRAND = m.Max(t => t.BRAND)
                                    }).ToList();
                        if (cgroup.Count == 0) continue;
                        var _int = cgroup.Find(n => n.TYPE == "INT");
                        var _le = cgroup.Find(n => n.TYPE == "LE");
                        var _pbp = cgroup.Find(n => n.TYPE == "PBP");
                        var _togo = cgroup.Find(n => n.TYPE == "TOGO");
                        var _total = cgroup.Find(n => n.TYPE == "TOTAL");
                        var _ytd = cgroup.Find(n => n.TYPE == "YTD");
                        _message = "Registro de valores en la base de datos";
                        var _aux = _int;
                        object group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE 
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _message = "TOTAL";
                        _total = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _total.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _message = "INT";
                        _int = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _le, _total, ref group, "WRK_BOY_DATA");


                        _aux = _le;
                        group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _message = "LE";
                        _le = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _int, _total, ref group, "WRK_BOY_DATA");


                        _aux = _togo;
                        group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _message = "TOGO";
                        _togo = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _le, _ytd, ref group, "WRK_BOY_DATA");


                        _aux = _pbp;
                        group = db.WRK_BOY_DATA.Where(m => m.MARKET == _aux.MARKET && m.CHANNEL == _aux.CHANNEL && m.BRAND == _aux.BRAND && m.TYPE == _aux.TYPE
                                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                        _message = "PBP";
                        _pbp = (StrawmanDBLibray.Entities.WRK_BOY_DATA)CalcBOYUpdates(_aux, _le, null, ref group, "WRK_BOY_DATA");
                    }
                                    
                    ////Actualizar datos de los canales
                    //foreach (var ji in j.Select(m => new { channel = m.channel, year_period = m.year_period, month_period = m.month_period }).Distinct())
                    //{
                    //    var sum = db.WRK_BOY_DATA.Where(m => m.YEAR_PERIOD == ji.year_period && m.MONTH_PERIOD == ji.month_period && m.CHANNEL == ji.channel && m.MARKET < 9000 && m.BRAND < 9000)
                    //        .GroupBy(m => new { m.CHANNEL, m.TYPE }).ToList()
                    //        .Select(m => new StrawmanDBLibray.Entities.WRK_BOY_DATA
                    //        {
                    //            CHANNEL = m.Key.CHANNEL,
                    //            TYPE = m.Key.TYPE
                    //            ,
                    //            MARKET_COL1 = m.Sum(t => t.MARKET_COL1)
                    //            ,
                    //            MARKET_COL2 = m.Sum(t => t.MARKET_COL2)
                    //            ,
                    //            SELLIN_COL1 = m.Sum(t => t.SELLIN_COL1)
                    //            ,
                    //            SELLIN_COL2 = m.Sum(t => t.SELLIN_COL2)
                    //            ,
                    //            SELLOUT_COL1 = m.Sum(t => t.SELLOUT_COL1)
                    //            ,
                    //            SELLOUT_COL2 = m.Sum(t => t.SELLOUT_COL2)
                    //            ,
                    //            MARKET_BTG = m.Sum(t => t.MARKET_BTG)
                    //            ,
                    //            SELLIN_BTG = m.Sum(t => t.SELLIN_BTG)
                    //            ,
                    //            SELLOUT_BTG = m.Sum(t => t.SELLOUT_BTG)
                    //            ,
                    //            MARKET = m.Max(t => t.MARKET) + 9000
                    //            ,
                    //            BRAND = m.Max(t => t.BRAND) + 9000
                    //        }).ToList();
                    //    var _int = sum.Find(n => n.TYPE == "INT");
                    //    var _le = sum.Find(n => n.TYPE == "LE");
                    //    var _pbp = sum.Find(n => n.TYPE == "PBP");
                    //    var _togo = sum.Find(n => n.TYPE == "TOGO");
                    //    var _total = sum.Find(n => n.TYPE == "TOTAL");
                    //    var _ytd = sum.Find(n => n.TYPE == "YTD");
                    //    var _aux = _int;
                        
                    //    object group = db.WRK_BOY_BY_CHANNEL_CALC.Where(m => m.CHANNEL == _aux.CHANNEL && m.TYPE == _aux.TYPE
                    //                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                    //    CalcBOYUpdates(_aux, _le, _total, ref group, "WRK_BOY_CHANNEL_CALC");

                    //    _aux = _le;
                    //    group = db.WRK_BOY_BY_CHANNEL_CALC.Where(m => m.CHANNEL == _aux.CHANNEL && m.TYPE == _aux.TYPE
                    //                                                                           && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                    //    CalcBOYUpdates(_aux, _int, _total, ref group, "WRK_BOY_CHANNEL_CALC");

                    //    _aux = _pbp;
                    //    group = db.WRK_BOY_BY_CHANNEL_CALC.Where(m => m.CHANNEL == _aux.CHANNEL && m.TYPE == _aux.TYPE
                    //                                                  && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                    //    CalcBOYUpdates(_aux, _le, null, ref group, "WRK_BOY_CHANNEL_CALC");

                    //    _aux = _togo;
                    //    group = db.WRK_BOY_BY_CHANNEL_GENERAL.Where(m => m.CHANNEL == _aux.CHANNEL && m.TYPE == _aux.TYPE
                    //                                                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).FirstOrDefault();
                    //    CalcBOYUpdates(_aux, _le, _ytd, ref group, "WRK_BOY_CHANNEL_GENERAL");
                        
                    //}
                    //int _id = int.Parse(collection.selectedId);

                    //Entities.GROUP_MASTER u = db.GROUP_MASTER.Where(i => i.ID == (decimal)_id).Select(m => m).FirstOrDefault();

                    //if (u != null)
                    //{

                    //    u.BASE_ID = (collection.type != u.TYPE) ? value : u.BASE_ID;
                    //    u.LEVEL = (collection.level != u.LEVEL) ? collection.level : u.LEVEL;
                    //    u.TYPE = (collection.type != u.TYPE) ? collection.type : u.TYPE;º
                    //    u.NAME = (collection.name != u.NAME) ? collection.name : u.NAME;
                    //};

                    //Salvamos datos y devolvemos el resultado.
                    result = db.SaveChanges();
                }
               
                RefreshDataCache();
                ResetFormBOYSession();
                return new JsonResult() { Data = new { Success = true, Result = result, @Controller = "BOYForm" } };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false, Message = _message },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
        }

        private object CalcBOYUpdates(object aux, object aux2, object aux3, ref object ret, string table_to_update)
        {
            switch (table_to_update)
            {
                //case "WRK_BOY_CHANNEL_CALC":
                //    StrawmanDBLibray.Entities.WRK_BOY_BY_CHANNEL_CALC saux = (StrawmanDBLibray.Entities.WRK_BOY_BY_CHANNEL_CALC)ret;
                //    switch (saux.TYPE)
                //    {
                //        case"INT":
                //            saux.MARKET_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1;
                //            saux.MARKET_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL2;
                //            saux.SELLIN_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1;
                //            saux.SELLIN_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL2;
                //            saux.SELLOUT_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1;
                //            saux.SELLOUT_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL2;
                //            saux.MARKET_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1) - 1;
                //            saux.SELLIN_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1) - 1;
                //            saux.SELLOUT_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1) - 1;
                //            saux.SHARE_COL1 = CheckDiv(saux.SELLOUT_COL1 , saux.MARKET_COL1);
                //            saux.SHARE_COL2 = CheckDiv(saux.SELLOUT_COL2 , saux.MARKET_COL2);
                //            saux.SHARE_CHGE = (saux.SHARE_COL1 - CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLOUT_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).MARKET_COL2)) * 100;
                //            saux.CONVERSION_RATE_COL1 = CheckDiv(saux.SELLIN_COL1 , saux.SELLOUT_COL1);
                //            saux.CONVERSION_RATE_COL2 = CheckDiv(saux.SELLIN_COL2 , saux.SELLOUT_COL2);
                //            break;
                //        case "LE":
                //            //aux = LE, aux2 = INT, aux3 = TOTAL
                //            saux.MARKET_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL2;
                //            saux.MARKET_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL2 - (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1;
                //            saux.SELLIN_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL2;
                //            saux.SELLIN_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL2 - (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1;
                //            saux.SELLOUT_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL2;
                //            saux.SELLOUT_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL2 - (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1;
                //            saux.MARKET_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).MARKET_COL2) - 1;
                //            saux.SELLIN_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLIN_COL2) - 1;
                //            saux.SELLOUT_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLOUT_COL2) - 1;
                //            saux.SHARE_COL1 = CheckDiv(saux.SELLOUT_COL1 , saux.MARKET_COL1);
                //            saux.SHARE_COL2 = CheckDiv(saux.SELLOUT_COL2 , saux.MARKET_COL2);
                //            saux.SHARE_CHGE = (saux.SHARE_COL1 - CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLOUT_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).MARKET_COL2)) * 100;
                //            //saux.SHARE_PC_COL2 = (saux.SHARE_COL1 - (_int.sellout_col1 / _int.market_col1)) * 100;
                //            saux.CONVERSION_RATE_COL1 = CheckDiv(saux.SELLIN_COL1 , saux.SELLOUT_COL1);
                //            saux.CONVERSION_RATE_COL2 = CheckDiv(saux.SELLIN_COL2 , saux.SELLOUT_COL2);
                //            break;
                //        case "PBP":
                //            //aux = PBP; aux2 = LE
                //            saux.MARKET_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1;
                //            saux.MARKET_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL2;
                //            saux.SELLIN_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1;
                //            saux.SELLIN_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL2;
                //            saux.SELLOUT_COL1 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1;
                //            saux.SELLOUT_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL2;
                //            saux.MARKET_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1) - 1;
                //            saux.SELLIN_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1) - 1;
                //            saux.SELLOUT_CHGE = CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1) - 1;
                //            saux.SHARE_COL1 = CheckDiv(saux.SELLOUT_COL1 ,saux.MARKET_COL1);
                //            saux.SHARE_COL2 = CheckDiv(saux.SELLOUT_COL2 ,saux.MARKET_COL2);
                //            saux.SHARE_CHGE = (saux.SHARE_COL1 - CheckDiv((decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1)) * 100;
                //            saux.CONVERSION_RATE_COL1 = CheckDiv(saux.SELLIN_COL1 , saux.SELLOUT_COL1);
                //            saux.CONVERSION_RATE_COL2 = CheckDiv(saux.SELLIN_COL2 , saux.SELLOUT_COL2);
                //            break;
                //    }
                //    ret = saux;
                //    break;
                case "WRK_BOY_CHANNEL_GENERAL":
                    //aux = TOGO, aux2 = LE, aux3 = YTD
                    StrawmanDBLibray.Entities.WRK_BOY_BY_CHANNEL_GENERAL gaux = (StrawmanDBLibray.Entities.WRK_BOY_BY_CHANNEL_GENERAL)ret;
                    gaux.MARKET_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1 - (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).MARKET_COL2;
                    gaux.SELLIN_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1 - (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLIN_COL2;
                    gaux.SELLOUT_COL2 = (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1 - (double?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLOUT_COL2;
                    gaux.MARKET_CHGE = CheckDiv((decimal?)gaux.MARKET_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1,null) - 1;
                    gaux.SELLIN_CHGE = CheckDiv((decimal?)gaux.SELLIN_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1,null) - 1;
                    gaux.SELLOUT_CHGE = CheckDiv((decimal?)gaux.SELLOUT_COL2 ,((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1,null) - 1;
                    gaux.SHARE_COL2 = CheckDiv(gaux.SELLOUT_COL2 , gaux.MARKET_COL2,null);
                    gaux.SHARE_CHGE = CheckDiv(gaux.SHARE_COL2 , CheckDiv (((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1,null), 100);
                    gaux.CONVERSION_RATE_COL2 = CheckDiv(gaux.SELLIN_COL2 , gaux.SELLOUT_COL2,null);
                    return gaux;
                case "WRK_BOY_DATA":
                    //aux = INT, aux2 = LE, aux3 = TOTAL
                    StrawmanDBLibray.Entities.WRK_BOY_DATA daux = (StrawmanDBLibray.Entities.WRK_BOY_DATA)ret;
                    switch (daux.TYPE)
                    {
                        case"INT":
                            daux.MARKET_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1;
                            daux.MARKET_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL2;
                            daux.SELLIN_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1;
                            daux.SELLIN_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL2;
                            daux.SELLOUT_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1;
                            daux.SELLOUT_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL2;
                            daux.MARKET_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1 ,((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).MARKET_COL2,null) - 1)*100;
                            daux.SELLIN_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1 ,((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLIN_COL2,null) - 1)*100;
                            daux.SELLOUT_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1 ,((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLOUT_COL2,null) - 1)*100;
                            daux.SHARE_COL1 = CheckDiv(daux.SELLOUT_COL1 ,daux.MARKET_COL1,100);
                            daux.SHARE_COL2 = CheckDiv(daux.SELLOUT_COL2 ,daux.MARKET_COL2,100);
                            daux.SHARE_PC = CheckShare(daux.SHARE_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SHARE_COL2);
                            daux.CONVERSION_RATE_COL1 = CheckDiv(daux.SELLIN_COL1 , daux.SELLOUT_COL1,100);
                            daux.CONVERSION_RATE_COL2 = CheckDiv(daux.SELLIN_COL2 , daux.SELLOUT_COL2,100);
                            return daux;
                        case "LE":
                            //aux = LE, aux2 = INT, aux3 = TOTAL
                            daux.MARKET_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1;
                            daux.MARKET_COL2 = daux.MARKET_COL1 - (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1;
                            daux.SELLIN_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1;
                            daux.SELLIN_COL2 = daux.SELLIN_COL1 - (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1;
                            daux.SELLOUT_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1;
                            daux.SELLOUT_COL2 = daux.SELLOUT_COL1 - (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1;
                            daux.MARKET_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).MARKET_COL2,null) - 1)*100;
                            daux.SELLIN_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLIN_COL2,null) - 1)*100;
                            daux.SELLOUT_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLOUT_COL2,null) - 1)*100;
                            daux.MARKET_PC_COL2 = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1,null) - 1)*100;
                            daux.SELLIN_PC_COL2 = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1,null) - 1) * 100;
                            daux.SELLOUT_PC_COL2 = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1,null) - 1) * 100;
                            daux.SHARE_COL1 = CheckDiv(daux.SELLOUT_COL1, daux.MARKET_COL1, 100);
                            //daux.SHARE_COL2 = CheckDiv(daux.SELLOUT_COL2, daux.MARKET_COL2) * 100;
                            daux.SHARE_PC = CheckShare(daux.SHARE_COL1,((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SHARE_COL2);
                            daux.SHARE_PC_COL2 = CheckShare(daux.SHARE_COL1, ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SHARE_COL1); 
                            daux.CONVERSION_RATE_COL1 = CheckDiv(daux.SELLIN_COL1, daux.SELLOUT_COL1, 100);
                            daux.CONVERSION_RATE_COL2 = CheckDiv(daux.SELLIN_COL2, daux.SELLOUT_COL2, 100);
                            return daux;
                        case "PBP":
                            //aux = PBP; aux2 = LE
                            daux.MARKET_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1;
                            daux.MARKET_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL2;
                            daux.SELLIN_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1;
                            daux.SELLIN_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL2;
                            daux.SELLOUT_COL1 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1;
                            daux.SELLOUT_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL2;
                            daux.MARKET_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1 , ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1,null) - 1)*100;
                            daux.SELLIN_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1 , ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1,null) - 1)*100;
                            daux.SELLOUT_PC = (CheckDiv(((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1 , ((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1,null) - 1)*100;
                            daux.SHARE_COL1 = CheckDiv(daux.SELLOUT_COL1 , daux.MARKET_COL1, 100);
                            daux.SHARE_COL2 = CheckDiv(daux.SELLOUT_COL2, daux.MARKET_COL2, 100);
                            daux.SHARE_PC = CheckShare(daux.SHARE_COL1,((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SHARE_COL1);
                            daux.CONVERSION_RATE_COL1 = CheckDiv(daux.SELLIN_COL1, daux.SELLOUT_COL1, 100);
                            daux.CONVERSION_RATE_COL2 = CheckDiv(daux.SELLIN_COL2, daux.SELLOUT_COL2, 100);
                            return daux;
                        case "TOGO":
                            daux.MARKET_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).MARKET_COL1 - (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).MARKET_COL2;
                            daux.SELLIN_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLIN_COL1 - (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLIN_COL2;
                            daux.SELLOUT_COL2 = (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux2).SELLOUT_COL1 - (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux3).SELLOUT_COL2;
                            daux.MARKET_PC = (CheckDiv(daux.MARKET_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).MARKET_COL1,null) - 1)*100;
                            daux.SELLIN_PC = (CheckDiv(daux.SELLIN_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLIN_COL1,null) - 1)*100;
                            daux.SELLOUT_PC = (CheckDiv(daux.SELLOUT_COL2 , (decimal?)((StrawmanDBLibray.Entities.WRK_BOY_DATA)aux).SELLOUT_COL1,null) - 1)*100;
                            daux.SHARE_COL2 = CheckDiv(daux.SELLOUT_COL2, daux.MARKET_COL2, 100);
                            daux.SHARE_PC = CheckShare(daux.SHARE_COL2, daux.SHARE_COL1);
                            daux.CONVERSION_RATE_COL2 = CheckDiv(daux.SELLIN_COL2, daux.SELLOUT_COL2, 100);
                            return daux;
                    }
                    break;
            }
            return null;

        }
        private decimal? CheckDiv(decimal? div1, decimal? div2,int?pc)
        {
            return CheckDiv((double?)div1, (double?)div2, pc);
        }
        private decimal? CheckDiv(double? div1, double? div2, int? pc)
        {
            if (div1 == null || div2 == null) return null;
            if (div2 == 0) return 0;
            return (decimal?)(div1 / div2) * (pc??1);
        }
        private decimal? CheckShare(decimal? col1, decimal? col2)
        {
            if (col1 == null || col2 == null) return null;
            return (Math.Round((decimal)col1, 2) - Math.Round((decimal)col2, 2));

        }
        private void SetBOYPercent(ref List<StrawmanDBLibray.Entities.WRK_BOY_DATA> data, string type)
        {
            
            if (type != "TOGO")
            {
                if (type == "LE")
                {
                    //PC_COL1
                    data.Find(m => m.TYPE == type).MARKET_PC = data.Find(m => m.TYPE == type).MARKET_PC * 100;
                    data.Find(m => m.TYPE == type).SELLOUT_PC = data.Find(m => m.TYPE == type).SELLOUT_PC * 100;
                    data.Find(m => m.TYPE == type).SELLIN_PC = data.Find(m => m.TYPE == type).SELLIN_PC * 100;
                    //PC_COL2
                    data.Find(m => m.TYPE == type).MARKET_PC_COL2 = data.Find(m => m.TYPE == type).MARKET_PC_COL2 * 100;
                    data.Find(m => m.TYPE == type).SELLOUT_PC_COL2 = data.Find(m => m.TYPE == type).SELLOUT_PC_COL2 * 100;
                    data.Find(m => m.TYPE == type).SELLIN_PC_COL2 = data.Find(m => m.TYPE == type).SELLIN_PC_COL2 * 100;
                }
                //CONVERSION_RATE
                data.Find(m => m.TYPE == type).CONVERSION_RATE_COL1 = data.Find(m => m.TYPE == type).CONVERSION_RATE_COL1 * 100;
                //SHARE
                data.Find(m => m.TYPE == type).SHARE_COL1 = data.Find(m => m.TYPE == type).SHARE_COL1 * 100;

            }
            else
            {
                //CONVERSION_RATE
                data.Find(m => m.TYPE == type).CONVERSION_RATE_COL2 = data.Find(m => m.TYPE == type).CONVERSION_RATE_COL2 * 100;
                //SHARE
                data.Find(m => m.TYPE == type).SHARE_COL2 = data.Find(m => m.TYPE == type).SHARE_COL2 * 100;
            }
        }

        private void RefreshDataCache()
        {
            //Borramos las tablas temporales para obligar a cargar los datos de la base de datos de nuevo
            SetSessionObject("v_WRK_BOY_CALC_DATA_INT", null);
            SetSessionObject("v_WRK_BOY_CALC_DATA_LE", null);
            SetSessionObject("v_WRK_BOY_CALC_DATA_PBP", null);
            SetSessionObject("v_WRK_BOY_CALC_DATA", null);
            //Cargamos los datos de la tabla maestra
            //Para WRK_BOY_DATA
            Helpers.Session.SetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BOY_DATA, Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BOY_DATA, false));
            //Para v_WRK_MARKET_BOY
            Helpers.Session.SetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY, Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_BOY, false));
            //Para v_WRK_BRAND_BOY_DATA
            Helpers.Session.SetSession(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY, Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_BOY, false));
            //Para v_WRK_MARKET_BOY_FRANCHISE
            //Para v_WRL_BRAND_BOY_FRANCHISE
            SetSessionData("v_WRK_BOY_CALC_DATA");
            SetSessionData("v_WRK_BOY_BASIC_DATA");
            SetSessionObject("BOYFormModel", GetBOYFormModel());

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult CalcBoyCfg(int _id, string type, int brand, int market, int channel, string control, string column, string value)
        {
            int result = 0;
            double? calc = 0, org_val = 0, org_val2 = 0, pbp_val = 0;

            bool is_pc = false;

            double? ma_total = 0;
            double? si_total = 0;
            double? so_total = 0;

            double? ma_ytd = 0;
            double? si_ytd = 0;
            double? so_ytd = 0;

            double? ma_togo = 0;
            double? si_togo = 0;
            double? so_togo = 0;

            double? ma_le = 0;
            double? si_le = 0;
            double? so_le = 0;

            double? ma_pbp = 0;
            double? si_pbp = 0;
            double? so_pbp = 0;

            string _column = "", _type = "";

            this.channel = channel;            
            Models.BOYFormModel bf = new Models.BOYFormModel();
            List<Entities.EditBOYModel> bm_lst = (List<Entities.EditBOYModel>)GetSessionObject("EditBOYModel");
            List<Models.BOYFormModel> bf_lst = (List<Models.BOYFormModel>)GetSessionObject("BOYFormModelList");
            Entities.EditBOYModel bm = null;
            if (bm_lst != null)
                bm= bm_lst.Where(m=>m._market == market && m._brand == brand && m._channel == channel).FirstOrDefault();

            if (control.EndsWith("_pc"))
            {
                is_pc = true;
                double val = 0;
                if (value.TrimEnd().EndsWith("%"))
                {
                    val = double.Parse(value.Replace("%", "").Replace(".",",").Trim());                    
                }
                else
                {
                    val = double.Parse(value.Trim());
                }
                calc = val/100;
            }
            //Recalculo el porcentaje teneindo en cuenta la modificación del total.
            List<StrawmanDBLibray.Entities.WRK_BOY_DATA> lst = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BOY_DATA, false);
            //TODO: Antes de recuperar los datos de las tablas temporales hay que hacer una consulta a la base de datos para saber si se ha modificado la tabla desde la última consulta.
            var d = lst.Where(i => i.BRAND == brand && i.MARKET == market && i.CHANNEL == channel
                                            && (i.YEAR_PERIOD == Helpers.PeriodUtil.Year && i.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                            .Select(m => new Models.BoyMassMarketModels
                                            {
                                                type = m.TYPE,
                                                brand = m.BRAND,
                                                market = m.MARKET,
                                                market_col1 = (double?)m.MARKET_COL1,
                                                market_col2 = (double?)m.MARKET_COL2,
                                                sellout_col1 = (double?)m.SELLOUT_COL1,
                                                sellout_col2 = (double?)m.SELLOUT_COL2,
                                                sellin_col1 = (double?)m.SELLIN_COL1,
                                                sellin_col2 = (double?)m.SELLIN_COL2,
                                                share_col1 = m.SHARE_COL1,
                                                share_col2 = m.SHARE_COL2,
                                                share_pc = m.SHARE_PC,
                                                share_pc_int = m.SHARE_PC_COL2
                                            }).ToList();                                         

            ma_total = (d.FirstOrDefault(i=>i.type == "TOTAL").market_col2);
            si_total = (d.FirstOrDefault(i => i.type == "TOTAL").sellin_col2);
            so_total = (d.FirstOrDefault(i => i.type == "TOTAL").sellout_col2);

            ma_ytd = (d.FirstOrDefault(i => i.type == "YTD").market_col2);
            si_ytd = (d.FirstOrDefault(i => i.type == "YTD").sellin_col2);
            so_ytd = (d.FirstOrDefault(i => i.type == "YTD").sellout_col2);

            ma_togo = (d.FirstOrDefault(i => i.type == "TOGO").market_col1);
            si_togo = (d.FirstOrDefault(i => i.type == "TOGO").sellin_col1);
            so_togo = (d.FirstOrDefault(i => i.type == "TOGO").sellout_col1);
        
            //Hay que contar con que se pueden haber modificado los valores BTG y LE, por lo que el cálculo de PBP dependerá de ello.
            //Comprobamos que no existe la caché para el modelo y si es así obtenemos los datos que le corresponden para el cálculo.
            if (bm == null)
            {
                var l = d.Find(i=>i.type == "LE");
                //Obtenemos los originale para el cálculo de PBP
                ma_le = l.market_col1;
                si_le = l.sellin_col1;
                so_le = l.sellout_col1;

                var p = d.Find(i => i.type == "PBP");
                ma_pbp = p.market_col1;
                si_pbp = p.sellin_col1;
                so_pbp = p.sellout_col1;
            }
            else
            {
                //La caché del modelo ha sido generada, por lo que tenemos que realizar el cálculo con los datos del formulario.
                ma_le = bm.BTG.market_col1;
                si_le = bm.BTG.sellin_col1;
                so_le = bm.BTG.sellout_col1;
            }
        
            double? ma_value = 0, ma_value2 = 0, si_value= 0, si_value2 = 0, so_value= 0, so_value2 = 0;
                    
            if (column.Contains("PBP")) _type = "PBP";
            if (column.Contains("INT")) _type = "INT";
            if (column.Contains("LE") || column.Contains("BTG")) _type = "BTG";

            if (column.Contains("MARKET")) _column = "MARKET";
            if (column.Contains("SELLIN")) _column = "SELLIN";
            if (column.Contains("SELLOUT")) _column = "SELLOUT";

            //calculo del procentaje
            //El cálculo se realizará para el total si se ha modificado el porcentaje, o para el porcentaje si se ha modificado el total.
            //hay que tener en cuenta el caso especial de LE, ya que se tiene que calcular siempre LE y PBP
            if (!is_pc)
            {
                switch (_type)
                {
                    case "PBP":
                        ma_value = (double?) CheckDiv(double.Parse(value), ma_le,null) - 1;
                        si_value = (double?) CheckDiv(double.Parse(value), si_le,null) - 1;
                        so_value = (double?) CheckDiv(double.Parse(value), so_le,null) - 1;
                        break;
                    case "INT":
                        ma_value = (double?) CheckDiv(double.Parse(value), ma_total,null) - 1;
                        si_value = (double?) CheckDiv(double.Parse(value), si_total,null) - 1;
                        so_value = (double?) CheckDiv(double.Parse(value), so_total,null) - 1;
                        break;
                    case "BTG":
                        double ma_btg_tmp = 0, si_btg_tmp = 0, so_btg_tmp = 0;
                        if (control.Contains("_col2")) //Corresponde a BTG
                        {
                            ma_btg_tmp = (double.Parse(value));
                            si_btg_tmp = (double.Parse(value));
                            so_btg_tmp = (double.Parse(value));
                        }
                        else //corresponde a LE
                        {
                            ma_btg_tmp = (double.Parse(value)) - (ma_ytd??0);
                            si_btg_tmp = (double.Parse(value)) - (si_ytd??0);
                            so_btg_tmp = (double.Parse(value)) - (so_ytd??0);
                        }
                        ma_value = (double?) CheckDiv(ma_btg_tmp, ma_togo,null) - 1;
                        si_value = (double?) CheckDiv(si_btg_tmp, si_togo,null) - 1;
                        so_value = (double?) CheckDiv(so_btg_tmp, so_togo,null) - 1;
                        break;
                }
                switch (_column)
                {
                    case "MARKET":
                        calc = ma_value;                                
                        break;
                    case "SELLOUT":
                        calc = so_value;                                
                        break;
                    case "SELLIN":
                        calc = si_value;                                
                        break;
                }
                //En el caso de que no sea un cálculo para btg, asignamos el valor de la columna original.
                org_val = double.Parse(value);
                //Si se trata de BTG:                        
                if (_type == "BTG")
                {
                    //Asignamos el valor de esa operación a la columna BTG (org_val) y el valor recibido al LE (org_val2)  
                    org_val2 = org_val;
                    //1- Identificamos cuál de las dos columnas se trata (LE o BTG)
                    if (control.Contains("_col2")) //Corresponde a BTG
                    {
                        //2- Si es la columna BTG, asignamos el valor original a la columna BTG (org_val) y la LE (org_val2) se calcula sumándole el ytd correspondiente.
                        org_val = org_val2;
                        if (_column == "MARKET") org_val2 = org_val + ma_ytd; //para market
                        else if (_column == "SELLOUT") org_val2 = org_val + so_ytd; //para sellout
                        else if (_column == "SELLIN") org_val2 = org_val + si_ytd; //para sellin
                    }
                    else
                    {
                        //3- Si la columna modificada es la LE, debemos restarle el valor ytd correspondiente para hallar el nuevo valor BTG.
                        if (_column == "MARKET") org_val = org_val2 - ma_ytd; //para market
                        else if (_column == "SELLOUT") org_val = org_val2 - so_ytd; //para sellout
                        else if (_column == "SELLIN") org_val = org_val2 - si_ytd; //para sellin
                    }
                    
                }
                        
                        

            }
            else
            {
                //Es un porcentaje modificado, por lo que hay que calcular totales:
                switch (_type)
                {
                    case "PBP":
                        ma_value = (calc + 1) * (ma_le??0);
                        si_value = (calc + 1) * (si_le??0);
                        so_value = (calc + 1) * (so_le??0);
                        break;
                    case "INT":
                        ma_value = (calc + 1) * (ma_total??0);
                        si_value = (calc + 1) * (si_total??0);
                        so_value = (calc + 1) * (so_total??0);
                        break;
                    case "BTG":

                        //Calculos para la columna BTG modificada.
                        ma_value = (calc + 1) * (ma_togo??0);
                        si_value = (calc + 1) * (si_togo??0);
                        so_value = (calc + 1) * (so_togo??0); 
                        //recalculamos el valor de LE.
                        ma_value2 = ma_value + (ma_ytd??0);
                        si_value2 = si_value + (si_ytd??0);
                        so_value2 = so_value + (so_ytd??0);
                        break;
                }
                //asignamos el valor calculado a la varible genérica org_val y org_val2 (solo para BTG).
                switch (_column)
                {
                    case "MARKET":
                        org_val = ma_value;
                        org_val2 = ma_value2;
                        break;
                    case "SELLOUT":
                        org_val = so_value;
                        org_val2 = so_value2;
                        break;
                    case "SELLIN":
                        org_val = si_value;
                        org_val2 = si_value2;
                        break;
                }

            }                

            if (_type == "BTG") 
            {
                // Es necesario volver a calcular el porcentaje de PBP en el caso de modificar LE/BTG
                // Calculamos los valores tanto de cantidad como de porcentaje para PBP en este caso.
                // Cantidades
                double pbp_value = 0;
                //En el caso de que se modifique LE (BTG) hay que obtener el entero para PBP de la caché para volver a calcular el porcentaje:            
                if (_column == "MARKET") pbp_value = bm == null? (ma_pbp??0): (double)bm.PBP.market_col1;
                if (_column == "SELLOUT")pbp_value = bm == null? (so_pbp??0): (double)bm.PBP.sellout_col1;                      
                if (_column == "SELLIN") pbp_value = bm == null? (si_pbp??0): (double)bm.PBP.sellin_col1;

                pbp_val = (double?)CheckDiv(pbp_value ,org_val2,null) -1; //(pbp_value_pc) / org_val) - 1; 
            }
            try
            {
                bf = bf_lst != null &&
                     bf_lst.Exists(m => m.item.market == market && m.item.brand == brand && m.item.channel == channel) ?
                        bf_lst.Find(m => m.item.market == market && m.item.brand == brand && m.item.channel == channel) :
                        GetBOYFormModel().Find(m => m.item.market == market && m.item.brand == brand && m.item.channel == channel);

                //Preparamos el ViewBag para llevar los datos del la consulta original a modo de control.                   

                //bf.INT = bf.INT == null ? GetBoyCalcCustomData("INT").Where(m => m.brand == brand && m.market == market && m.channel == channel).FirstOrDefault() : bf.INT;
                //bf.BTG = bf.BTG == null ? GetBoyYTDData("TOGO").Where(m => m.brand == brand && m.market == market && m.channel == channel).FirstOrDefault() : bf.BTG;
                //bf.LE = bf.LE == null ? GetBoyYTDData("LE").Where(m => m.brand == brand && m.market == market && m.channel == channel).FirstOrDefault() : bf.LE;
                //bf.PBP = bf.PBP == null ? GetBoyCalcCustomData("PBP").Where(m => m.brand == brand && m.market == market && m.channel == channel).FirstOrDefault() : bf.PBP;
                //bf.item = bf.item == null ? GetBoyYTDData("YTD").Where(m => m.brand == brand && m.market == market && m.channel == channel).FirstOrDefault() : bf.item;
                bm = bm == null?SetEditBoyModel(bf,brand, market, channel):bm;
                    
                switch (_type)
                {
                    case "PBP":
                        //lst = GetSessionData("v_WRK_BOY_CALC_DATA_PBP");
                        //lst = lst.Where(m => m.brand == brand && m.market == market && m.channel == channel).Select(m=>m).ToList();
                        bm.PBP.market_col1 = column.Contains("MARKET") ? org_val : bm.PBP.market_col1;
                        bm.PBP.sellin_col1 = column.Contains("SELLIN") ? org_val : bm.PBP.sellin_col1;
                        bm.PBP.sellout_col1 = column.Contains("SELLOUT") ? org_val : bm.PBP.sellout_col1;
                        bm.PBP.market_pc = column.Contains("MARKET") ? (decimal?)calc*100 : bm.PBP.market_pc;                            
                        bm.PBP.sellin_pc = column.Contains("SELLIN") ? (decimal?)calc*100 : bm.PBP.sellin_pc;                            
                        bm.PBP.sellout_pc = column.Contains("SELLOUT") ? (decimal?)calc*100 : bm.PBP.sellout_pc;
                        bm.PBP.market_pc_f = column.Contains("MARKET") ? calc : bm.PBP.market_pc_f;
                        bm.PBP.sellin_pc_f = column.Contains("SELLIN") ? calc : bm.PBP.sellin_pc_f;
                        bm.PBP.sellout_pc_f = column.Contains("SELLOUT") ? calc : bm.PBP.sellout_pc_f;
                        break;
                    case "INT":
                        //lst = GetSessionData("v_WRK_BOY_CALC_DATA_INT");
                        //lst = lst.Where(m => m.brand == brand && m.market == market && m.channel == channel).Select(m => m).ToList();
                        bm.INT.market_col1 = column.Contains("MARKET") ? org_val : bm.INT.market_col1;
                        bm.INT.sellin_col1 = column.Contains("SELLIN") ? org_val : bm.INT.sellin_col1;
                        bm.INT.sellout_col1 = column.Contains("SELLOUT") ? org_val : bm.INT.sellout_col1;
                        bm.INT.market_pc = column.Contains("MARKET") ? (decimal?)calc*100: bm.INT.market_pc;
                        bm.INT.sellin_pc = column.Contains("SELLIN") ? (decimal?)calc*100: bm.INT.sellin_pc;
                        bm.INT.sellout_pc = column.Contains("SELLOUT") ? (decimal?)calc*100: bm.INT.sellout_pc;
                        bm.INT.market_pc_f = column.Contains("MARKET") ? calc : bm.INT.market_pc_f;
                        bm.INT.sellin_pc_f = column.Contains("SELLIN") ? calc : bm.INT.sellin_pc_f;
                        bm.INT.sellout_pc_f = column.Contains("SELLOUT") ? calc : bm.INT.sellout_pc_f;
                        break;
                    case "BTG":
                        //lst = GetSessionData("v_WRK_BOY_CALC_DATA_LE");
                        //lst = lst.Where(m => m.brand == brand && m.market == market && m.channel == channel).Select(m => m).ToList();
                        bm.BTG.market_col1 = column.Contains("MARKET") ? org_val2 : bm.BTG.market_col1;
                        bm.BTG.market_col2 = column.Contains("MARKET") ? org_val : bm.BTG.market_col2;
                        bm.BTG.sellin_col1 = column.Contains("SELLIN") ? org_val2 : bm.BTG.sellin_col1;
                        bm.BTG.sellin_col2 = column.Contains("SELLIN") ? org_val : bm.BTG.sellin_col2;
                        bm.BTG.sellout_col1 = column.Contains("SELLOUT") ? org_val2 : bm.BTG.sellout_col1;
                        bm.BTG.sellout_col2 = column.Contains("SELLOUT") ? org_val : bm.BTG.sellout_col2;
                        bm.BTG.market_pc = column.Contains("MARKET") ? (decimal?)calc*100 : bm.BTG.market_pc;
                        bm.BTG.sellin_pc = column.Contains("SELLIN") ? (decimal?)calc*100 : bm.BTG.sellin_pc;
                        bm.BTG.sellout_pc = column.Contains("SELLOUT") ? (decimal?)calc*100 : bm.BTG.sellout_pc;
                        bm.BTG.market_pc_f = column.Contains("MARKET") ? calc : bm.BTG.market_pc_f;
                        bm.BTG.sellin_pc_f = column.Contains("SELLIN") ? calc : bm.BTG.sellin_pc_f;
                        bm.BTG.sellout_pc_f = column.Contains("SELLOUT") ? calc : bm.BTG.sellout_pc_f;
                        // Modificar LE o BTG no solo modifica las cantidades de LE y BTG, sino también las PBP. Hay que calcular la cantidad para PBP.
                        bm.PBP.market_pc_f = column.Contains("MARKET") ? pbp_val : (double?)bm.PBP.market_pc_f;
                        bm.PBP.sellin_pc_f = column.Contains("SELLIN") ? pbp_val : (double?)bm.PBP.sellin_pc_f;
                        bm.PBP.sellout_pc_f = column.Contains("SELLOUT") ? pbp_val : (double?)bm.PBP.sellout_pc_f;
                        bm.PBP.market_pc = column.Contains("MARKET") ? (decimal?)pbp_val * 100 : bm.PBP.market_pc;
                        bm.PBP.sellin_pc = column.Contains("SELLIN") ? (decimal?)pbp_val * 100 : bm.PBP.sellin_pc;
                        bm.PBP.sellout_pc = column.Contains("SELLOUT") ? (decimal?)pbp_val * 100 : bm.PBP.sellout_pc;
                        break;

                }

                switch (_type)
                { 
                    case"INT":
                        bf.INT.market_col1 =  column.Contains("MARKET") ? bm.INT.market_col1:bf.INT.market_col1;
                        bf.INT.sellin_col1 = column.Contains("SELLIN") ? bm.INT.sellin_col1 : bf.INT.sellin_col1;
                        bf.INT.sellout_col1 = column.Contains("SELLOUT") ? bm.INT.sellout_col1:bf.INT.sellout_col1;
                        bf.INT.market_pc = column.Contains("MARKET") ? bm.INT.market_pc : bf.INT.market_pc;
                        bf.INT.sellin_pc = column.Contains("SELLIN") ? bm.INT.sellin_pc:bf.INT.sellin_pc;
                        bf.INT.sellout_pc = column.Contains("SELLOUT") ? bm.INT.sellout_pc : bf.INT.sellout_pc;

                        bf.LE.market_pc_int = column.Contains("MARKET") ?(CheckDiv(bf.LE.market_col1, bf.INT.market_col1,null) - 1) * 100:bf.LE.market_pc_int;
                        bf.LE.sellin_pc_int = column.Contains("SELLIN") ?(CheckDiv(bf.LE.sellin_col1, bf.INT.sellin_col1,null) - 1) * 100:bf.LE.sellin_pc_int;
                        bf.LE.sellout_pc_int = column.Contains("SELLOUT") ?(CheckDiv(bf.LE.sellout_col1, bf.INT.sellout_col1,null) - 1) * 100:bf.LE.sellout_pc_int;

                        break;
                    case "PBP":
                        bf.PBP.market_col1 = column.Contains("MARKET") ?bm.PBP.market_col1:bf.PBP.market_col1;
                        bf.PBP.sellin_col1 = column.Contains("SELLIN") ? bm.PBP.sellin_col1 : bf.PBP.sellin_col1;
                        bf.PBP.sellout_col1 = column.Contains("SELLOUT") ?bm.PBP.sellout_col1:bf.PBP.sellout_col1;
                        bf.PBP.market_pc = column.Contains("MARKET") ?bm.PBP.market_pc:bf.PBP.market_pc;
                        bf.PBP.sellin_pc = column.Contains("SELLIN") ? bm.PBP.sellin_pc : bf.PBP.sellin_pc;
                        bf.PBP.sellout_pc = column.Contains("SELLOUT") ? bm.PBP.sellout_pc : bf.PBP.sellout_pc;

                        break;
                    case "BTG":
                        bf.LE.market_col1 = column.Contains("MARKET") ? bm.BTG.market_col1 : bf.LE.market_col1;
                        bf.BTG.market_col2 = column.Contains("MARKET") ?bm.BTG.market_col2:bf.BTG.market_col2;
                        bf.LE.sellin_col1 = column.Contains("SELLIN") ? bm.BTG.sellin_col1 : bf.LE.sellin_col1;
                        bf.BTG.sellin_col2 = column.Contains("SELLIN") ?bm.BTG.sellin_col2:bf.BTG.sellin_col2;
                        bf.LE.sellout_col1 = column.Contains("SELLOUT") ? bm.BTG.sellout_col1 : bf.LE.sellout_col1;
                        bf.BTG.sellout_col2 = column.Contains("SELLOUT") ?bm.BTG.sellout_col2:bf.BTG.sellout_col2;
                        bf.BTG.market_pc = column.Contains("MARKET") ?bm.BTG.market_pc:bf.BTG.market_pc;
                        bf.BTG.sellin_pc = column.Contains("SELLIN") ?bm.BTG.sellin_pc:bf.BTG.sellin_pc;
                        bf.BTG.sellout_pc = column.Contains("SELLOUT") ?bm.BTG.sellout_pc:bf.BTG.sellout_pc;
                        bf.LE.market_pc = column.Contains("MARKET") ? (CheckDiv(bf.LE.market_col1, d.Find(m => m.type == "TOTAL").market_col2,null) - 1) * 100 : bf.LE.market_pc;
                        bf.LE.sellout_pc = column.Contains("SELLOUT") ? (CheckDiv(bf.LE.sellout_col1, d.Find(m => m.type == "TOTAL").sellout_col2,null) - 1) * 100 : bf.LE.sellout_pc;
                        bf.LE.sellin_pc = column.Contains("SELLIN") ? (CheckDiv(bf.LE.sellin_col1, d.Find(m => m.type == "TOTAL").sellin_col2,null) - 1) * 100 : bf.LE.sellin_pc;

                        bf.LE.market_col2 = column.Contains("MARKET") ?bf.LE.market_col1 - bf.INT.market_col1:bf.LE.market_col2;
                        bf.LE.sellin_col2 = column.Contains("SELLIN") ?bf.LE.sellin_col1 - bf.INT.sellin_col1:bf.LE.sellin_col2;
                        bf.LE.sellout_col2 = column.Contains("SELLOUT") ?bf.LE.sellout_col1 - bf.INT.sellout_col1:bf.LE.sellout_col2;
                        bf.LE.market_pc_int = column.Contains("MARKET") ?(CheckDiv(bf.LE.market_col1, bf.INT.market_col1,null) - 1) * 100:bf.LE.market_pc_int;
                        bf.LE.sellin_pc_int = column.Contains("SELLIN") ?(CheckDiv(bf.LE.sellin_col1, bf.INT.sellin_col1,null) - 1) * 100:bf.LE.sellin_pc_int;
                        bf.LE.sellout_pc_int = column.Contains("SELLOUT") ?(CheckDiv(bf.LE.sellout_col1, bf.INT.sellout_col1,null) - 1) * 100:bf.LE.sellout_pc_int;

                        bf.PBP.market_col1 = column.Contains("MARKET") ?bm.PBP.market_col1:bf.PBP.market_col1;
                        bf.PBP.sellin_col1 = column.Contains("SELLIN") ?bm.PBP.sellin_col1:bf.PBP.sellin_col1;
                        bf.PBP.sellout_col1 = column.Contains("SELLOUT") ?bm.PBP.sellout_col1:bf.PBP.sellout_col1;
                        bf.PBP.market_pc = column.Contains("MARKET") ?bm.PBP.market_pc:bf.PBP.market_pc;
                        bf.PBP.sellin_pc = column.Contains("SELLIN") ?bm.PBP.sellin_pc:bf.PBP.sellin_pc;
                        bf.PBP.sellout_pc = column.Contains("SELLOUT") ?bm.PBP.sellout_pc:bf.PBP.sellout_pc;
                        break;
                }
                bf.INT.share_col1 = CheckDiv(bm.INT.sellout_col1, bm.INT.market_col1, 100);
                bf.LE.share_col1 = CheckDiv(bm.BTG.sellout_col1, bm.BTG.market_col1, 100);
                bf.BTG.share_col2 = CheckDiv(bm.BTG.sellout_col2, bm.BTG.market_col2, 100);
                bf.PBP.share_col1 = CheckDiv(bm.PBP.sellout_col1, bm.PBP.market_col1, 100);

                bf.BTG.share_pc = CheckShare(bf.BTG.share_col1, bf.BTG.share_col2);
                bf.LE.share_pc = CheckShare(bf.LE.share_col1, d.Find(m => m.type == "TOTAL").share_col2);
                bf.LE.share_pc_int = CheckShare(bf.LE.share_col1,bf.INT.share_col1);
                bf.PBP.share_pc = CheckShare(bf.PBP.share_col1,bf.BTG.share_col1);
                bf.INT.share_pc = CheckShare(bf.INT.share_col1,d.Find(m => m.type == "TOTAL").share_col2);

                bf.INT.conversion_rate = null;
                bf.INT.conversion_rate1 = null;

                bf.PBP.conversion_rate = null;
                bf.PBP.conversion_rate1 = null;

                bf.BTG.conversion_rate = null;
                bf.BTG.conversion_rate1 = null;
                bf.BTG.conversion_rate2 = null;
                bf.LE.conversion_rate = null;
                bf.LE.conversion_rate1 = null;
                bf.LE.conversion_rate2 = null;
                bf.model = bm;
                if (bm_lst == null) bm_lst = new List<Entities.EditBOYModel>();
                if (bm_lst != null && bm_lst.Where(m => m._brand == bm._brand && m._market == bm._market && m._channel == bm._channel).Select(m => m) != null)
                {
                    //El registro existe, por lo tanto lo actualizamos
                    bm_lst.Remove(bm);
                }
                bm_lst.Add(bm);

                //Modo edición
                bf.mode = Helpers.Modes.Edit;
                if (bf_lst == null) bf_lst = new List<Models.BOYFormModel>();
                if (bf_lst != null && bf_lst.Where(m => m.item.brand == bf.item.brand && m.item.market == bf.item.market && m.item.channel == bf.item.channel).Select(m => m) != null)
                {
                    //El registro existe, por lo tanto lo actualizamos
                    bf_lst.Remove(bf);
                }
                bf_lst.Add(bf);

                                       
                
                SetSessionObject("EditBOYModel", bm_lst);
                SetSessionObject("BOYFormModelList", bf_lst);
                //Enviamos el modelo a la vista parcial para el formulario de modificación con un botón de Guardar activo para guardar los cambios.
                return PartialView(RETURN_BOY_FORM, bf);
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        #region Session Object Util
       /// <summary>
       /// Restablece los datos del formulario al llamar a actualizar la pantalla BOY's
       /// </summary>
        public static void ResetFormBOYSession()
        {
            SetSessionObject("EditBOYModel", null);
            SetSessionObject("BOYFormModelList", null);
        }
        private static void SetSessionObject(string p, object bm)
        {
            Helpers.Session.SetSession(p, bm);
        }

        private object GetSessionObject(string p)
        {
            return Helpers.Session.GetSession(p);
        }

        #endregion

        private Entities.EditBOYModel SetEditBoyModel(Models.BOYFormModel bf, int brand, int market, int channel)
        {
            Entities.EditBOYModel ebm = new Entities.EditBOYModel();
            //List<Models.BoyMassMarketModels> lst = null;
            ebm._market = market;
            ebm._brand = brand;
            ebm._channel = channel;
            ebm.INT = new Entities.EditBOY
                       {
                           brand = bf.INT.brand,
                           market = bf.INT.market,
                           channel = bf.INT.channel,
                           market_col1 = bf.INT.market_col1,
                           sellin_col1 = bf.INT.sellin_col1,
                           sellout_col1 = bf.INT.sellout_col1,
                           market_col2= bf.INT.market_col2,
                           sellin_col2 = bf.INT.sellin_col2,
                           sellout_col2 = bf.INT.sellout_col2,
                           market_pc = bf.INT.market_pc,
                           sellin_pc = bf.INT.sellin_pc,
                           sellout_pc = bf.INT.sellout_pc,
                           market_id = bf.INT.market_id,
                           sellin_id = bf.INT.sellin_id,
                           sellout_id = bf.INT.sellout_id,
                           market_pc_f = bf.INT.market_boy,
                           sellin_pc_f = bf.INT.sellin_boy,
                           sellout_pc_f = bf.INT.sellout_boy
                       };


            //lst = GetSessionData("v_WRK_BOY_CALC_DATA_PBP");
            ebm.PBP = new Entities.EditBOY
                       {
                           brand = bf.PBP.brand,
                           market = bf.PBP.market,
                           channel = bf.PBP.channel,
                           market_col1 = bf.PBP.market_col1,
                           sellin_col1 = bf.PBP.sellin_col1,
                           sellout_col1 = bf.PBP.sellout_col1,
                           market_col2 = bf.PBP.market_col2,
                           sellin_col2 = bf.PBP.sellin_col2,
                           sellout_col2 = bf.PBP.sellout_col2,
                           market_pc = bf.PBP.market_pc,
                           sellin_pc = bf.PBP.sellin_pc,
                           sellout_pc = bf.PBP.sellout_pc,
                           market_id = bf.PBP.market_id,
                           sellin_id = bf.PBP.sellin_id,
                           sellout_id = bf.PBP.sellout_id,
                           market_pc_f = bf.PBP.market_boy,
                           sellin_pc_f = bf.PBP.sellin_boy,
                           sellout_pc_f = bf.PBP.sellout_boy
                       };


            //lst = GetSessionData("v_WRK_BOY_CALC_DATA_LE");
            ebm.BTG = new Entities.EditBOY
                       {
                           brand = bf.BTG.brand,
                           market = bf.BTG.market,
                           channel = bf.BTG.channel,
                           market_col1 = bf.LE.market_col1,
                           sellin_col1 = bf.LE.sellin_col1,
                           sellout_col1 = bf.LE.sellout_col1,
                           market_col2 = bf.BTG.market_col2,
                           sellin_col2 = bf.BTG.sellin_col2,
                           sellout_col2 = bf.BTG.sellout_col2,
                           market_pc = bf.BTG.market_pc,
                           sellin_pc = bf.BTG.sellin_pc,
                           sellout_pc = bf.BTG.sellout_pc,
                           market_id = bf.BTG.market_id,
                           sellin_id = bf.BTG.sellin_id,
                           sellout_id = bf.BTG.sellout_id,
                           market_pc_f = bf.BTG.market_boy,
                           sellin_pc_f = bf.BTG.sellin_boy,
                           sellout_pc_f = bf.BTG.sellout_boy                          
                       };

            return ebm;
        }

        public JsonResult groupNameExist(string name)
        {
            bool exists = true;
            using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            {
                var q = db.GROUP_MASTER.Where(m => m.NAME == name).Select(m => m);
                List<Entities.GodzillaEntity.GROUP_MASTER> lst = q.ToList();
                exists = !(lst.Count > 0);
            }


            return Json(exists);
        }
        //
        // GET: /Forms/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Forms/Edit/5

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
        // GET: /Forms/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Forms/Delete/5

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

        private dynamic GetSessionData(string key)
        {
            List<Models.BoyMassMarketModels> list = null;
            if (GetSessionObject(key) == null) return null;
            //if (GetSessionObject(key) != null) list = (List<Models.BoyMassMarketModels>)GetSessionObject(key);
            switch (key)
            {
                case "v_WRK_BOY_BASIC_DATA":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> table = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionObject(key);
                    var query = from p in table select
                    new Models.BoyMassMarketModels
                    {
                        channel = p.CHANNEL,
                        brand = p.BRAND,
                        brand_name = p.BRAND_NAME,
                        boy_name = p.NTS_NAME,
                        conversion_rate1 = p.CONVERSION_RATE_COL1,
                        conversion_rate2 = p.CONVERSION_RATE_COL2,
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
                        market_id = (int?)p.MARKET_ID,
                        sellout_id = (int?)p.SELLOUT_ID,
                        sellin_id = (int?)p.SELLIN_ID,
                        type = p.TYPE
                    };
                    list = query.ToList();
                    break;
                case "v_WRK_BOY_CALC_DATA_LE":
                    List<Models.BoyMassMarketModels> cletable = (List<Models.BoyMassMarketModels>)GetSessionObject(key);
                    var queryle = from p in cletable
                            select p;
                    list = queryle.ToList();
                    break;
                case "v_WRK_BOY_CALC_DATA_PBP":
                    var querypbp = from p in (List<Models.BoyMassMarketModels>)GetSessionObject(key)
                            select p;
                    list = querypbp.ToList();
                   break;
                case "v_WRK_BOY_CALC_DATA_INT":
                   var queryint = from p in (List<Models.BoyMassMarketModels>)GetSessionObject(key)
                           select p;
                   list = queryint.ToList();
                   break;
                case "v_WRK_BOY_CALC_DATA":
                   return (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionObject(key);                   

            }

            return list;
        }

        private void SetSessionData(string table)
        {
            using (db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
        {
            db.CommandTimeout = 50000;
            if (GetSessionData("v_WRK_BOY_CALC_DATA") == null)
            {
                var query = db.WRK_BOY_DATA
                            .Where(p => p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month 
                                     && (p.MARKET < 9000 && p.BRAND < 9000))
                            .Select(p => p);
                List<StrawmanDBLibray.Entities.WRK_BOY_DATA> list = query.ToList();
                SetSessionObject("v_WRK_BOY_CALC_DATA", list);
            }
            if (!String.IsNullOrEmpty(table))
            {
                List<StrawmanDBLibray.Entities.WRK_BOY_DATA> calcDataTable = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)GetSessionData("v_WRK_BOY_CALC_DATA");

                    db.CommandTimeout = 50000;
                    switch (table)
                    {
                        case "v_WRK_BOY_BASIC_DATA":
                            var query = from p in db.WRK_BOY_DATA
                                        where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                                        select p;
                            SetSessionObject(table, query.ToList());
                            break;

                        case "v_WRK_BOY_CALC_DATA_LE":
                            var query_le = from p in calcDataTable
                                           where p.TYPE == "LE"
                                           select new Models.BoyMassMarketModels
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
                                               //market_boy = (double?)p.MARKET_PC,
                                               market_id = (int?)p.MARKET_ID,
                                               market_col2 = (double?)p.MARKET_BTG,
                                               //sellin_boy = (double?)p.SELLIN_PC,
                                               sellin_col2 = (double?)p.SELLIN_BTG,
                                               sellin_id = (int?)p.SELLIN_ID,
                                               //sellin_type = p.SELLIN_TYPE,
                                               //sellout_boy = (double?)p.SELLOUT_PC,
                                               sellout_col2 = (double?)p.SELLOUT_BTG,
                                               sellout_id = (int?)p.SELLOUT_ID,
                                               share_col1 = p.SHARE_COL1,
                                               share_col2 = p.SHARE_COL2,
                                               share_pc = p.SHARE_PC,
                                               share_pc_int = p.SHARE_PC_COL2,
                                               sellin_pc_int = p.SELLIN_PC_COL2,
                                               sellout_pc_int = p.SELLOUT_PC_COL2,
                                               market_pc_int = p.MARKET_PC_COL2
                                               //sellout_type = p.SELLOUT_TYPE
                                           };
                            List<Models.BoyMassMarketModels> query_le_lst = query_le.ToList();
                            foreach (Models.BoyMassMarketModels dat in query_le_lst)
                            {
                                StrawmanDBLibray.Entities.STRWM_BOY_DATA t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.market_id).FirstOrDefault();
                                if (t != null)
                                {
                                    //dat.market_pc = (decimal?)t.BTG * 100;
                                    //dat.market_pc_int = (decimal?)t.BTG;
                                    dat.market_boy = t.BTG;
                                }
                                t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.sellin_id).FirstOrDefault();
                                if (t != null)
                                {
                                    //dat.sellin_pc = (decimal?)t.BTG * 100;
                                    //dat.sellin_pc_int = (decimal?)t.BTG;
                                    dat.sellin_boy = t.BTG;
                                }
                                t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.sellout_id).FirstOrDefault();
                                if (t != null)
                                {
                                    //dat.sellout_pc = (decimal?)t.BTG * 100;
                                    //dat.sellout_pc_int = (decimal?)t.BTG;
                                    dat.sellout_boy = t.BTG;
                                }
                            }
                            SetSessionObject(table, query_le_lst);
                            break;
                        case "v_WRK_BOY_CALC_DATA_INT":
                            var query_int = from p in calcDataTable
                                            where p.TYPE == "INT"
                                            select new Models.BoyMassMarketModels
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
                                                //market_boy = (double?)p.MARKET_PC,
                                                market_id = (int?)p.MARKET_ID,
                                                //market_col2 = (decimal?)p.MARKET_COL2,
                                                //sellin_boy = (double?)p.SELLIN_PC,
                                                //sellin_col2 = (decimal?)p.SELLIN_COL2,
                                                sellin_id = (int?)p.SELLIN_ID,
                                                //sellin_type = p.SELLIN_TYPE,
                                                //sellout_boy = (double?)p.SELLOUT_PC,
                                                //sellout_col2 = (decimal?)p.SELLOUT_COL2,
                                                sellout_id = (int?)p.SELLOUT_ID,
                                                share_col1 = p.SHARE_COL1,
                                                share_col2 = p.SHARE_COL2,
                                                share_pc = p.SHARE_PC
                                                //sellout_type = p.SELLOUT_TYPE

                                            };
                            List<Models.BoyMassMarketModels> query_int_lst = query_int.ToList();
                            foreach (Models.BoyMassMarketModels dat in query_int_lst)
                            {
                                StrawmanDBLibray.Entities.STRWM_BOY_DATA t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.market_id).FirstOrDefault();
                                if (t != null)
                                {
                                    dat.market_pc = (decimal?)t.INT * 100;
                                    dat.market_pc_int = (decimal?)t.INT;
                                    dat.market_boy = t.INT;
                                }
                                t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.sellin_id).FirstOrDefault();
                                if (t != null)
                                {
                                    dat.sellin_pc = (decimal?)t.INT * 100;
                                    dat.sellin_pc_int = (decimal?)t.INT;
                                    dat.sellin_boy = t.INT;
                                }
                                t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.sellout_id).FirstOrDefault();
                                if (t != null)
                                {
                                    dat.sellout_pc = (decimal?)t.INT * 100;
                                    dat.sellout_pc_int = (decimal?)t.INT;
                                    dat.sellout_boy = t.INT;
                                }
                            }
                            SetSessionObject(table, query_int_lst);
                            break;
                        case "v_WRK_BOY_CALC_DATA_PBP":
                            var query_pbp = from p in calcDataTable
                                            where p.TYPE == "PBP"
                                            select new Models.BoyMassMarketModels
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
                                                sellin_pc = p.SELLIN_PC ,
                                                sellout_col1 = (double?)p.SELLOUT_COL1,
                                                sellout_pc = p.SELLOUT_PC,
                                                type = p.TYPE,
                                                market_name = p.BRAND_NAME,
                                                //market_type = p.MARKET_TYPE,
                                                //market_boy = (double?)p.MARKET_PC,
                                                market_id = (int?)p.MARKET_ID,
                                                //market_col2 = (decimal?)p.MARKET_COL2,
                                                //sellin_boy = (double?)p.SELLIN_PC,
                                                //sellin_col2 = (decimal?)p.SELLIN_COL2,
                                                sellin_id = (int?)p.SELLIN_ID,
                                                //sellin_type = p.SELLIN_TYPE,
                                                //sellout_boy = (double?)p.SELLOUT_PC,
                                                //sellout_col2 = (decimal?)p.SELLOUT_COL2,
                                                sellout_id = (int?)p.SELLOUT_ID,
                                                share_col1 = p.SHARE_COL1,
                                                share_pc = p.SHARE_PC
                                                //sellout_type = p.SELLOUT_TYPE
                                            };
                            List<Models.BoyMassMarketModels> query_pbp_lst = query_pbp.ToList();
                            foreach (Models.BoyMassMarketModels dat in query_pbp_lst)
                            {
                                StrawmanDBLibray.Entities.STRWM_BOY_DATA t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.market_id).FirstOrDefault();
                                if (t != null)
                                {
                                    dat.market_pc = (decimal?)t.PBP * 100;
                                    dat.market_pc_int = (decimal?)t.PBP;
                                    dat.market_boy = t.PBP;
                                }
                                t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.sellin_id).FirstOrDefault();
                                if (t != null)
                                {
                                    dat.sellin_pc = (decimal?)t.PBP * 100;
                                    dat.sellin_pc_int = (decimal?)t.PBP;
                                    dat.sellin_boy = t.PBP;
                                }
                                t = db.STRWM_BOY_DATA.Where(m => m.ID == dat.sellout_id).FirstOrDefault();
                                if (t != null)
                                {
                                    dat.sellout_pc = (decimal?)t.PBP * 100;
                                    dat.sellout_pc_int = (decimal?)t.PBP;
                                    dat.sellout_boy = t.PBP;
                                }
                                
                            }
                            SetSessionObject(table, query_pbp_lst);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region Render Partials

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult BOYForm()
        {
            object BOYModel = GetSessionObject("BOYFormModel");
            ViewBag.Channel = channel;
            return PartialView(BOY_FORM,BOYModel);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [Authorize]
        public ActionResult GetBoyForm(int _market, int _brand, int _channel, string _boyTyp)
        {
            Models.BOYFormModel model = new Models.BOYFormModel();
            this.channel = _channel;
            //RefreshDataCache();
            List<Models.BOYFormModel> session = (List<Models.BOYFormModel>)GetSessionObject("BOYFormModelList");
            //List<Models.BOYFormModel> edit = session == null? GetBOYFormModel():session;
            model = session != null? session.Find(m => m.item.market == _market && m.item.brand == _brand && m.item.channel == _channel):
                 GetBOYFormModel().Find(m => m.item.market == _market && m.item.brand == _brand && m.item.channel == _channel);
            model = model == null ? GetBOYFormModel().Find(m => m.item.market == _market && m.item.brand == _brand && m.item.channel == _channel) : model;
            model.FormType = _boyTyp;
            return PartialView(BOY_FORM_EDIT, model);
        }
        public ActionResult ChangeChanel(string _channel)
        {            
            this.channel = _channel != null? int.Parse(_channel): Helpers.Channels.MASS;
            SetSessionObject("BOYFormModel", GetBOYFormModel());

            return BOYForm();
        }
        
        public PartialViewResult BOYFormComponent(int market, int brand, int channel)
        {
            Entities.EditBOYModel bm = (Entities.EditBOYModel)GetSessionData("EditBOYModel");


            Models.BOYFormModel bf = new Models.BOYFormModel();
            //bf.INT = GetBoyCalcCustomData("INT");
            //bf.LE = GetBoyCalcCustomData("LE");
            //bf.PBP = GetBoyCalcCustomData("PBP");
            //bf.lstaux = GetBoyYTDData("YTD");
            //bm._market = market;
            //bm._brand = brand;
            //bm._channel = channel;
            //bf.itemaux = bf.lstaux.Where(m => m.brand == brand && m.market == market && m.channel == channel).FirstOrDefault();
            //bf.editaux = bm;

            ////Reestablecemos el modelo de edición para evitar que se mezclen datos.
            //SetSessionObject("EditBOYModel", null);
            return PartialView(EDITED_BODY, bf);
        }

        #endregion

        #region upload file

        public ActionResult LoaderView()
        {
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/LoaderView";
            Models.LoaderViewModels lvm = new Models.LoaderViewModels();
            lvm.ddl = GetDropDownList(0);
            lvm.isUpdated = false;
            return View(lvm);
        }

        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            HttpPostedFileBase myFile = Request.Files[0];
            bool isUploaded = false;
            string message = "File upload failed";
            string fileName = "";
            int file_type = int.Parse(Request.Form["ddl.SelectedItemId"].ToString());

            Models.LoaderViewModels lvm = new Models.LoaderViewModels();
            lvm.ddl = GetDropDownList(file_type);            
            if (myFile != null && myFile.ContentLength != 0 && file_type > 0)
            {
                string pathForSaving = Server.MapPath("~/Uploads");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        fileName = System.IO.Path.GetFileName(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        isUploaded = true;
                        message = "File uploaded successfully!";
                        
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                        lvm.onError = true;
                        lvm.errorMsg = message;
                    }
                }
            }
            lvm.isUpdated = isUploaded;
            lvm.fileName = fileName;
            lvm.fileType = file_type;
            ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/LoaderView";
            return View(LOADER_VIEW, lvm);
        }

        [HttpGet]
        public ActionResult ProcessFile(string fileName, string fileType)
        {
            bool success = false;
            string message = "Error processing file...";
            string spath = Server.MapPath("~/Uploads");
            string path = Path.Combine(spath, fileName);
            string ext = Path.GetExtension(path);
            int last_transaction = 0;
            int[] transactions = new int[]{};
            List<Models.YearTransactionModel> ylist = new List<Models.YearTransactionModel>();

            FileStream fis = System.IO.File.OpenRead(path);
            IExcelDataReader reader;
            
            if (ext.ToUpper().Equals(".XLSX"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(fis);
            }
            else
            {
                reader = ExcelReaderFactory.CreateBinaryReader(fis);
            }
            if (reader.IsValid)
            {
                List<StrawmanDBLibray.Classes.ExcelLoader> lst = new List<StrawmanDBLibray.Classes.ExcelLoader>();
                reader.IsFirstRowAsColumnNames = true;
                System.Data.DataSet ds = reader.AsDataSet();
                System.Data.DataTable dt = ds.Tables["CARGA GODZILLA"]; //Prueba. Tiene que ser 0 o bien usar directamente reader.Read();
                if (dt == null) dt = ds.Tables[0];//Si no existe la pestaña, por defecto la primera.
                if(int.Parse(fileType) == 1){
                    //Datos NST
                    var query = from System.Data.DataRow row in dt.Rows
                                select new StrawmanDBLibray.Classes.ExcelLoader
                                {
                                    col1 = row[0].ToString(),
                                    col2 = row[1].ToString(),
                                    col3 = row[2].ToString(),
                                    col4 = row[3].ToString(),
                                    col5 = row[4].ToString(),
                                    col6 = row[5].ToString(),
                                    col7 = row[6].ToString(),
                                    col8 = row[7].ToString(),
                                    col9 = row[8].ToString(),
                                    col10 = row[9].ToString(),
                                    col11 = row[10].ToString(),
                                    col12 = row[11].ToString(),
                                    col13 = row[12].ToString()
                                };
                    lst = query.ToList();
                }
                else if (int.Parse(fileType) == 2) //Datos Nielsen
                {                    
                    foreach (System.Data.DataTable ddt in ds.Tables)
                    {
                        int columns_count = ddt.Columns.Count;
                        var query = from System.Data.DataRow row in ddt.Rows
                                    select new Entities.ExcelLoader
                                    {
                                        col1 = ((columns_count>1)? row[1].ToString() : null),
                                        col2 = ((columns_count>2)? row[2].ToString():null),
                                        col3 = ((columns_count>3)? row[3].ToString():null),
                                        col4 = ((columns_count>4)? row[4].ToString():null),
                                        col5 = ((columns_count>5)? row[5].ToString():null),
                                        col6 = ((columns_count>6)? row[6].ToString():null),
                                        col7 = ((columns_count>7)? row[7].ToString():null),
                                        col8 = ((columns_count>8)? row[8].ToString():null),
                                        col9 = ((columns_count>9)? row[9].ToString():null),
                                        col10 = ((columns_count>10)? row[10].ToString():null),
                                        col11 = ((columns_count>11)? row[11].ToString():null),
                                        col12 = ((columns_count>12)? row[12].ToString():null),
                                        col13 = ((columns_count>13)? row[13].ToString():null),
                                        col14 = ((columns_count>14)? row[14].ToString():null),
                                        col15 = ((columns_count>15)? row[15].ToString():null),
                                        col16 = ((columns_count>16)? row[16].ToString():null),
                                        col17 = ((columns_count>17)? row[17].ToString():null),
                                        col18 = ((columns_count>18)? row[18].ToString():null),
                                        col19 = ((columns_count>19)? row[19].ToString():null),
                                        col20 = ((columns_count>20)? row[20].ToString():null),
                                        col21 = ((columns_count>21)? row[21].ToString():null),
                                        col22 = ((columns_count>22)? row[22].ToString():null),
                                        col23 = ((columns_count>23)? row[23].ToString():null),
                                        col24 = ((columns_count>24)? row[24].ToString():null),
                                        col25 = ((columns_count>25)? row[25].ToString():null),
                                        col26 = ((columns_count>26)? row[26].ToString():null),
                                        col27 = ((columns_count>26)? row[27].ToString():null),
                                        col28 = ((columns_count>27)? row[28].ToString():null),
                                        col29 = ((columns_count>28)? row[29].ToString():null),
                                    };
                        lst.AddRange(query.ToList().GetRange(25, ddt.Rows.Count - 25));
                    }
                }
                else if (int.Parse(fileType) == 3)
                {
                    //IMS Los datos vienen en diferentes tablas. 
                    //Recorrer las tablas y añadir los datos 
                    foreach (System.Data.DataTable ddt in ds.Tables)
                    {
                        var query = from System.Data.DataRow row in ddt.Rows
                                    where !row.IsNull(1) && !row.IsNull(15)
                                    select new Entities.ExcelLoader
                                    {
                                        col1 = row[1].ToString(),
                                        col2 = row[2].ToString(),
                                        col3 = row[3].ToString(),
                                        col4 = row[4].ToString(),
                                        col5 = row[5].ToString(),
                                        col6 = row[6].ToString(),
                                        col7 = row[7].ToString(),
                                        col8 = row[8].ToString(),
                                        col9 = row[9].ToString(),
                                        col10 = row[10].ToString(),
                                        col11 = row[11].ToString(),
                                        col12 = row[12].ToString(),
                                        col13 = row[13].ToString(),
                                        col14 = row[14].ToString(),
                                        col15 = row[15].ToString(),
                                        col16 = row[16].ToString(),
                                        col17 = row[17].ToString(),
                                        col18 = row[18].ToString(),
                                        col19 = row[19].ToString(),
                                        col20 = row[20].ToString(),
                                        col21 = row[21].ToString(),
                                        col22 = row[22].ToString(),
                                        col23 = row[23].ToString(),
                                        col24 = row[24].ToString(),
                                        col25 = row[25].ToString(),
                                        col26 = row[26].ToString(),
                                        col27 = row[27].ToString(),
                                        col28 = row[28].ToString(),
                                        col29 = row[29].ToString(),
                                    };
                        lst.AddRange(query.ToList());
                    }
                }
                else if (int.Parse(fileType) == 4)
                {
                    foreach (System.Data.DataTable ddt in ds.Tables)
                    {
                        int columns_count = ddt.Columns.Count;
                        var query = from System.Data.DataRow row in ddt.Rows
                                    select new Entities.ExcelLoader
                                    {
                                        col1 = (columns_count > 0) ? row[0].ToString() : null,
                                        col2 = (columns_count > 1) ? row[1].ToString():null,
                                        col3 = (columns_count > 2) ? row[2].ToString():null,
                                        col4 = (columns_count > 3) ? row[3].ToString():null,
                                        col5 = (columns_count > 4) ? row[4].ToString():null,
                                        col6 = (columns_count > 5) ? row[5].ToString():null,
                                        col7 = (columns_count > 6) ? row[6].ToString():null,
                                        col8 = (columns_count > 7) ? row[7].ToString():null,
                                        col9 = (columns_count > 8) ? row[8].ToString():null,
                                        col10 = (columns_count > 9) ? row[9].ToString():null,
                                        col11 = (columns_count > 10) ? row[10].ToString():null,
                                        col12 = (columns_count > 11) ? row[11].ToString():null,
                                        col13 = (columns_count > 12) ? row[12].ToString():null,
                                        col14 = (columns_count > 13) ? row[13].ToString():null,
                                        col15 = (columns_count > 14) ? row[14].ToString():null,
                                        col16 = (columns_count > 15) ? row[15].ToString():null,
                                        col17 = (columns_count > 16) ? row[16].ToString():null,
                                        col18 = (columns_count > 17) ? row[17].ToString():null,
                                        col19 = (columns_count > 18) ? row[18].ToString():null,
                                        col20 = (columns_count > 19) ? row[19].ToString():null,
                                        col21 = (columns_count > 20) ? row[20].ToString():null,
                                        col22 = (columns_count > 21) ? row[21].ToString():null,
                                        col23 = (columns_count > 22) ? row[22].ToString():null,
                                        col24 = (columns_count > 23) ? row[23].ToString():null,
                                        col25 = (columns_count > 24) ? row[24].ToString():null,
                                        col26 = (columns_count > 25) ? row[25].ToString():null,
                                        col27 = (columns_count > 26) ? row[26].ToString():null,
                                        col28 = (columns_count > 27) ? row[27].ToString():null,
                                        col29 = (columns_count > 28) ? row[28].ToString():null,
                                        col30 = (columns_count > 29) ? row[29].ToString():null,
                                        col31 = (columns_count > 30) ? row[30].ToString():null,
                                        col32 = (columns_count > 31) ? row[31].ToString():null,
                                        col33 = (columns_count > 32) ? row[32].ToString():null,
                                        col34 = (columns_count > 33) ? row[33].ToString():null,
                                        col35 = (columns_count > 34) ? row[34].ToString():null,
                                        col36 = (columns_count > 35) ? row[35].ToString():null,
                                        col37 = (columns_count > 36) ? row[36].ToString():null,
                                        col38 = (columns_count > 37) ? row[37].ToString():null,
                                        col39 = (columns_count > 38) ? row[38].ToString():null,
                                        col40 = (columns_count > 39) ? row[39].ToString():null,
                                        col41 = (columns_count > 40) ? row[40].ToString():null,
                                        col42 = (columns_count > 41) ? row[41].ToString():null,
                                        col43 = (columns_count > 42) ? row[42].ToString():null,
                                        col44 = (columns_count > 43) ? row[43].ToString():null,
                                        col45 = (columns_count > 44) ? row[44].ToString():null,
                                        col46 = (columns_count > 45) ? row[45].ToString():null,
                                        col47 = (columns_count > 46) ? row[46].ToString():null,
                                        col48 = (columns_count > 47) ? row[47].ToString():null,
                                        col49 = (columns_count > 48) ? row[48].ToString():null,
                                        data = row.ItemArray
                                    };
                        lst.AddRange(query.ToList());
                    }
                }
                TempData["LoadNTS"] = lst; //insertar en la tabla temporal correspondiente
                TempData["fileType"] = fileType;
                success = true;
                }
            else
            {
                message += " Tipo de archivo no permitido";
            }
            return new JsonResult { 
                                Data = new { Success = success, Message = message },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
        }
        [Authorize]
        [HttpGet]
        public ActionResult ProcessTransaction(string fileName, string fileType)
        {
            List<StrawmanDBLibray.Classes.ExcelLoader> lst = (List<StrawmanDBLibray.Classes.ExcelLoader>)TempData["LoadNTS"];
            int last_transaction = 0;
            int[] transactions = new int[] { };
            List<Models.YearTransactionModel> ylist = new List<Models.YearTransactionModel>();
            bool success = false;
            string message = "Error processing file...";
            string procedure = "";
            switch (int.Parse(fileType))
            {
                case 1:
                    procedure = SP_TMP_NTS_DATA;
                    break;
                case 2:
                    procedure = SP_TMP_NIELSEN_DATA;
                    break;
                case 3:
                    procedure = SP_TMP_IMS_DATA;
                    break;
                case 4:
                    procedure = SP_CUSTOM_LOADER;
                    break;
            }
                //Last Transaction
                using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                {
                    var q = db.TRANSACTION_TABLE.Select(m => m.LAST_TRANSACTION_ID).ToList().Last();
                    last_transaction = (int)q;
                    last_transaction++;
                    db.ExecuteStoreCommand("exec sp_TRANSACTION_UPDATE @TRANSACTION = {0}", last_transaction);
                }
                //Datos NTS
                if(int.Parse(fileType) == 1){
                    int ret = StrawmanDBLibray.Upload.NTSData(TMP_NTS_DATA, last_transaction, lst);
                    //using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                    //{
                    //    db.ExecuteStoreCommand("exec sp_TRUNCATE_TMP_DATA @TABLE = {0}", TMP_NTS_DATA);

                    //    foreach(Entities.ExcelLoader loader in lst){
                    //        db.TMP_NTS_DATA.AddObject(new Entities.GodzillaEntity.TMP_NTS_DATA
                    //        {
                    //           Local_CustGrp_1_name = loader.col1,
                    //           Mat_Local_Class_1 = loader.col2,
                    //           Material = loader.col3,
                    //           Fiscal_year_period = loader.col4,
                    //           NTS = loader.col5,
                    //           CHANNEL = loader.col6,
                    //           EUROCODE = loader.col7,
                    //           CONCAT_1 = loader.col8,
                    //           MAPPING = loader.col9,
                    //           CROSS_FRANCHISE = loader.col10,
                    //           LINK_CHANNEL = loader.col11,
                    //           CHECK = loader.col12,
                    //           YTD = loader.col13,
                    //           TRANSACTION_ID = last_transaction
                    //        });
                    //    }
                    //    db.SaveChanges();
                    //}
                }
                else if ((int.Parse(fileType) == 2) || (int.Parse(fileType) == 3))
                {
                    //Datos Nielsen e IMS
                    using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                    {
                        int _firstYear, _firstMonth;
                        int _month = Helpers.PeriodUtil.Month;
                        int _year = Helpers.PeriodUtil.Year;
                        //Se asume que los datos a cargar son del periodo actual menos 24 meses
                        DateTime _period = (_month < 12) ? new DateTime(_year - 2, _month + 1, 1) : new DateTime(_year - 1, 1, 1);
                        _firstMonth = _period.Month;
                        _firstYear = _period.Year;

                        if (int.Parse(fileType) == 2)
                            db.ExecuteStoreCommand("exec sp_TRUNCATE_TMP_DATA @TABLE = {0}", TMP_NIELSEN_DATA);
                        else
                            db.ExecuteStoreCommand("exec sp_TRUNCATE_TMP_DATA @TABLE = {0}", TMP_IMS_DATA);

                        int col_start = 6;
                        int max_cols = 24;
                        foreach (Entities.ExcelLoader loader in lst)
                        {
                            _year = _firstYear;
                            _month = _firstMonth;
                            string[] cols = {
                                                loader.col6,
                                                loader.col7,
                                                loader.col8,
                                                loader.col9,
                                                loader.col10,
                                                loader.col11,
                                                loader.col12,
                                                loader.col13,
                                                loader.col14,
                                                loader.col15,
                                                loader.col16,
                                                loader.col17,
                                                loader.col18,
                                                loader.col19,
                                                loader.col20,
                                                loader.col21,
                                                loader.col22,
                                                loader.col23,
                                                loader.col24,
                                                loader.col25,
                                                loader.col26,
                                                loader.col27,
                                                loader.col28,
                                                loader.col29
                                            };
                            for (int i = 0; i < cols.Length; i++)
                            {
                                if (int.Parse(fileType) == 2)
                                {
                                    db.TMP_NIELSEN_DATA.AddObject(new Entities.GodzillaEntity.TMP_NIELSEN_DATA
                                    {
                                        BRAND_NAME = loader.col5,
                                        DESCRIPTION = loader.col2,
                                        NIELSEN_ID = loader.col1,
                                        EUROCODE = loader.col4,
                                        AMOUNTH = cols[i],
                                        TRANSACTION_ID = last_transaction,
                                        MONTH_PERIOD = _month.ToString(),
                                        YEAR_PERIOD = _year.ToString()
                                    });
                                }
                                else
                                {
                                    if (loader.col1 == SALES_EUROS_PUB)
                                    {
                                        db.TMP_IMS_DATA.AddObject(new Entities.GodzillaEntity.TMP_IMS_DATA
                                        {
                                            BRAND_NAME = loader.col5,
                                            DESCRIPTION = loader.col2,
                                            IMS_ID = loader.col1,
                                            EUROCODE = loader.col4,
                                            AMOUNTH = cols[i],
                                            TRANSACTION_ID = last_transaction,
                                            MONTH_PERIOD = _month.ToString(),
                                            YEAR_PERIOD = _year.ToString()
                                        });
                                    }else if(loader.col1 == STOCK_UNITS){
                                        db.TMP_IMS_STOCK_DATA.AddObject(new Entities.GodzillaEntity.TMP_IMS_STOCK_DATA
                                        {
                                            BRAND_NAME = loader.col5,
                                            DESCRIPTION = loader.col2,
                                            IMS_ID = loader.col1,
                                            EUROCODE = loader.col4,
                                            AMOUNTH = cols[i],
                                            TRANSACTION_ID = last_transaction,
                                            MONTH_PERIOD = _month.ToString(),
                                            YEAR_PERIOD = _year.ToString()
                                        });
                                    }
                                }
                                _month ++;
                                if (_month > 12)
                                {
                                    _month = 1;
                                    _year++;
                                }
                            }
                        }
                        db.SaveChanges();
                    }

                }
                else if (int.Parse(fileType) == 4)
                {
                    using (Entities.MasterTable.MasterTableEntities db = new Entities.MasterTable.MasterTableEntities())
                    {
                        List<Entities.StrwmanPreviewMarketData> mrk_lst = lst.Join(db.STRWM_BRAND_DATA,p=>p.col1, m=>m.DESCRIPTION, (p,m)=>new{p = p, m= m} ).Select( g =>new Entities.StrwmanPreviewMarketData{
                            MARKET = g.m.MARKET,
                            DATA = g.m.DATA,
                            DESCRIPTION = g.m.DESCRIPTION,
                        }).ToList();
                        DateTime date = DateTime.Parse(lst.Select(m=>m.col6).First().ToString());
                        int month_period = date.Month;
                        int year_period = date.Year;
                        int year_transaction = 0;
                        StrawmanDBLibray.Classes.ExcelLoader date_list = lst.Select(m => m).First();
                        List<Entities.CalcData> _data = new List<Entities.CalcData>();
                        int row_index = 1;
                        db.ExecuteStoreCommand("TRUNCATE TABLE [STRWM_MARKET_DATA_BCK]");
                        db.ExecuteStoreCommand("TRUNCATE TABLE [STRWM_BRAND_DATA_BCK]");
                        List<Entities.MasterTable.STRWM_MARKET_DATA_BCK> mdata = new List<Entities.MasterTable.STRWM_MARKET_DATA_BCK>();
                        List<Entities.MasterTable.STRWM_BRAND_DATA_BCK> bdata = new List<Entities.MasterTable.STRWM_BRAND_DATA_BCK>();
                        foreach (Entities.ExcelLoader loader in lst)
                        {
                            string _name = loader.col4;
                            string _source = loader.col2;
                            int _channel = 0;
                            switch (loader.col1)
                            {
                                case CALC.OTC:
                                    _channel = Helpers.Channels.OTC;
                                    break;
                                case CALC.BEAUTY:
                                    _channel = Helpers.Channels.BEAUTY;
                                    break;
                                case CALC.MASS:
                                    _channel = Helpers.Channels.MASS;
                                    break;

                            }
                            //identificar el código del producto a insertar
                            var ts = db.ROSETTA_LOADER.Where(m => m.EXCEL_ROW == row_index).Select(m => m);
                            //insertar valores
                            if (ts != null)
                            {
                                foreach (Entities.MasterTable.ROSETTA_LOADER t in ts)
                                {    
                                    //los datos comienzan en la columna 5. Finalizan en la 48.
                                    for (int i = 5; i< loader.data.Length; i++)
                                    {
                                        if (String.IsNullOrEmpty(date_list.data.ElementAt(i).ToString()))
                                        {
                                            continue;
                                        }
                                        Entities.CalcData cd = new Entities.CalcData();
                                        cd.data = loader.data.ElementAt(i).ToString();
                                        double _cd_data = 0;
                                        if (!double.TryParse(cd.data, out _cd_data)) cd.data = _cd_data.ToString();
                                        //identificar el periodo de la columna
                                        //  - La última columna correspondería con el periodo actual a cargar.
                                        //  - Tratamos la primera columna como Enero - 2012 
                                        DateTime current = DateTime.Parse(date_list.data.ElementAt(i).ToString());
                                        cd.year_period = current.Year;
                                        cd.month_period = current.Month;
                                        year_transaction = cd.year_period;
                                        if (ylist.Exists(e => e.year == year_transaction))
                                        {
                                            last_transaction = (int)ylist.Where(f => f.year == year_transaction).FirstOrDefault().transaction;
                                        }
                                        else 
                                        {
                                            last_transaction = GetTransactionId();
                                            ylist.Add(new Models.YearTransactionModel { year = year_transaction, transaction = last_transaction });
                                        }
                                        string __data = current.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);
                                
                                        if (t.BRAND_ID != null) cd.BRAND = (int)t.BRAND_ID;
                                        if (t.MARKET_ID != null) cd.MARKET = (int)t.MARKET_ID;
                                        if (t.GROUP_ID != null) cd.GROUP = (int)t.GROUP_ID;
                                        _data.Add(cd);
                                        if (loader.col3 == BRAND_DESCRIPTION || t.BRAND_ID != null)
                                        {
                                            bdata.Add(new Entities.MasterTable.STRWM_BRAND_DATA_BCK
                                            {
                                                YEAR_PERIOD = cd.year_period,
                                                MONTH_PERIOD = cd.month_period,
                                                MONTH = CalcInsertData(CALC.MONTH, _data, cd.year_period, cd.month_period),
                                                YTD = CalcInsertData(CALC.YTD, _data, cd.year_period, cd.month_period),
                                                MAT = CalcInsertData(CALC.MAT, _data, cd.year_period, cd.month_period),
                                                TOTAL = 0,
                                                BRAND = t.BRAND_ID,
                                                MARKET = t.MARKET_ID,
                                                GROUP = t.GROUP_ID,
                                                DATA = __data,
                                                SOURCE = _source,
                                                DESCRIPTION = t.DESCRIPTION,
                                                USER = Helpers.UserUtils.UserName,
                                                LAST_DATE = DateTime.Now,
                                                TRANSACTION_ID = last_transaction
                                            });
                                        }
                                        else if (loader.col3 == MARKET_DESCRIPTION || t.BRAND_ID == null)
                                        {
                                            mdata.Add(new Entities.MasterTable.STRWM_MARKET_DATA_BCK
                                            {
                                                YEAR_PERIOD = cd.year_period,
                                                MONTH_PERIOD = cd.month_period,
                                                MONTH = CalcInsertData(CALC.MONTH, _data, cd.year_period, cd.month_period),
                                                YTD = CalcInsertData(CALC.YTD, _data, cd.year_period, cd.month_period),
                                                MAT = CalcInsertData(CALC.MAT, _data, cd.year_period, cd.month_period),
                                                TOTAL = 0,
                                                MARKET = t.MARKET_ID,
                                                GROUP = t.GROUP_ID,
                                                DATA = __data,
                                                SOURCE = _source,
                                                DESCRIPTION = t.DESCRIPTION,
                                                USER = Helpers.UserUtils.UserName,
                                                LAST_DATE = DateTime.Now,
                                                TRANSACTION_ID = last_transaction
                                            });
                                        }
                                        //Si el Diciembre podemos actualizar todos los datos totales para ese año
                                        if (cd.month_period == 12)
                                        {
                                            //db.SaveChanges();
                                            decimal? _total = 0;
                                            _total = CalcInsertData(CALC.TOTAL, _data, cd.year_period, cd.month_period);
                                            //var u = db.STRWM_BRAND_DATA_BCK.Where(m => m.YEAR_PERIOD == cd.year_period && m.MONTH_PERIOD != cd.month_period
                                            //        && m.MARKET == cd.MARKET && m.BRAND == cd.BRAND && m.GROUP == cd.GROUP).Select(m => m);
                                            //foreach (Entities.MasterTable.STRWM_BRAND_DATA_BCK table in u.ToList())
                                            //{
                                            //    table.TOTAL = _total;
                                            //}
                                            if (loader.col3 == BRAND_DESCRIPTION)
                                            {
                                                foreach (Entities.CalcData dat in _data.Where(m => m.year_period == cd.year_period && m.BRAND == t.BRAND_ID && m.MARKET == t.MARKET_ID).ToList())
                                                {
                                                    var s = bdata.Where(m => m.MARKET == dat.MARKET && m.BRAND == dat.BRAND && m.YEAR_PERIOD == dat.year_period).Select(m => m);
                                                    foreach (Entities.MasterTable.STRWM_BRAND_DATA_BCK bck in s.ToList())
                                                    {
                                                        bck.TOTAL = _total;
                                                    }
                                                }
                                            }
                                            else if (loader.col3 == MARKET_DESCRIPTION || t.BRAND_ID == null)
                                            {
                                                foreach (Entities.CalcData dat in _data.Where(m => m.year_period == cd.year_period && m.MARKET == t.MARKET_ID && m.GROUP == t.GROUP_ID).ToList())
                                                {
                                                    var s = mdata.Where(m => m.MARKET == dat.MARKET && m.GROUP == dat.GROUP && m.YEAR_PERIOD == dat.year_period).Select(m => m);
                                                    foreach (Entities.MasterTable.STRWM_MARKET_DATA_BCK bck in s.ToList())
                                                    {
                                                        bck.TOTAL = _total;
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            row_index++;
                        }
                        foreach (Entities.MasterTable.STRWM_BRAND_DATA_BCK btmp in bdata)
                        {
                            db.STRWM_BRAND_DATA_BCK.AddObject(btmp);
                        }
                        foreach (Entities.MasterTable.STRWM_MARKET_DATA_BCK mtmp in mdata)
                        {
                            db.STRWM_MARKET_DATA_BCK.AddObject(mtmp);
                        }
                        db.SaveChanges();
                    }
                }
                success = SetTransaction(fileName, procedure, ylist, transactions, last_transaction);
                return new JsonResult { 
                                Data = new { Success = success, Message = message, Transaction = (transactions.Length == 0)?new int[]{last_transaction}:transactions },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
        }

        private bool SetTransaction(string fileName, string procedure,List<Models.YearTransactionModel> ylist, int[] transactions, int last_transaction)
        {
            bool ret = false;
            try{
                using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                {
                    int number_rep = 0, _id = 0;
                    var d = db.TRIGGER_TABLE.Select(m => m.ID);
                    if (d.Count() > 0) _id = (int)d.Max();
                    var q = db.TRIGGER_TABLE.Where(m => m.FILE_NAME == fileName).ToList();
                    if (q.Count > 0)
                    {
                        number_rep = (int)q.Max(m => m.NUMBER_REP);
                        _id = (int)db.TRIGGER_TABLE.Max(m => m.ID);
                    }

                    _id++;
                    number_rep++;
                    if (ylist.Count == 0) ylist.Add(new Models.YearTransactionModel { year = Helpers.PeriodUtil.Year, transaction = last_transaction });
                    foreach (Models.YearTransactionModel md in ylist)
                    {
                        db.TRIGGER_TABLE.AddObject(new Entities.GodzillaEntity.TRIGGER_TABLE
                        {
                            ID = _id,
                            FILE_NAME = fileName,
                            NUMBER_REP = number_rep,
                            STATUS = STATUS_Q,
                            TRANSACTION_ID = md.transaction,
                            PROCEDURE = procedure,
                            USER = Helpers.UserUtils.UserName,
                        });
                        transactions = transactions.Union(new int[] { md.transaction }).ToArray();
                        number_rep++;
                        _id++;
                    }
                    db.SaveChanges();
                    ret = true;
                }
            }catch(Exception ex){}
            return ret;
        }

        public async Task<ActionResult> ExecuteStoredProcedure(string transactionId)
        {
            bool success = false;
            string message = "Error executing procedure...";
            int tras = 0;
            
            if(int.TryParse(transactionId, out tras)){
                success = true;
                Task t = Task.Factory.StartNew(() =>
                {
                    using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                    {
                        db.CommandTimeout = 500000;
                        db.ExecuteStoreCommand("sp_TRIGGER_LAUNCHER");
                        db.SaveChanges();
                    }
                    
                });
                //t.Start();
            }
            return new JsonResult
            {
                Data = new { Success = success, Message = message },
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult RefreshTriggerStatus(string transactionId, string lastId)
        {
            bool success = true;
            string[] message = { "Error executing procedure..." };
            string status = "";
            int trans = 0, last = 0;
            if(int.TryParse(transactionId, out trans) && (int.TryParse(lastId, out last)))
            {
                using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
                {
                    try
                    {
                        var q = db.LOG_TABLE.Where(m => m.TRANSACTION_ID == trans && m.ID >= last).OrderBy(m => m.ID);
                        if (q.Count() < 1)
                        {
                            message = (last == 0) ? new string[] { WAIT_FOR_INFO_DATA } : new string[] { NO_DATA_FOUND_MESSAGE };
                        }
                        else
                        {
                            var t = (q.Count() == 1) ? q.ToList().First() : q.ToList().Where(m => m.ID != last).First();
                            success = t.TYPE != "Error" || true;
                            status = t.TYPE;
                            last = (int)t.ID;
                            message = new string[] { t.MESSAGE };
                        }
                    }
                    catch (Exception ex)
                    {
                        message = new string[] {WAIT_FOR_INFO_DATA};
                    }
                    
                }
            }

            return new JsonResult
            {
                Data = new {Success = success, Message = message, LastId = last,Status = status },
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult LoaderPreview()
        {
            List<Entities.ExcelLoader> lst = TempData["LoadNTS"] as List<Entities.ExcelLoader>;
            return View(lst);
        }

        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        private Models.DropDownListModels GetDropDownList(int _val)
        {
            Models.DropDownListModels ddl = new Models.DropDownListModels();
            ddl.Items = new List<SelectListItem>();
            ddl.Items.Add(new SelectListItem
            {
                Text = SELECT_TEXT,
                Value = "0",
                Selected = _val == 0
            });
            ddl.Items.Add(new SelectListItem
            {
                Text = NTS_TEXT,
                Value = "1",
                Selected = _val == 1
            });
            //ddl.Items.Add(new SelectListItem
            //{
            //    Text = NIELSEN_TEXT,
            //    Value = "2"
            //});
            //ddl.Items.Add(new SelectListItem
            //{
            //    Text = IMS_TEXT,
            //    Value = "3"
            //});
            ddl.Items.Add(new SelectListItem
            {
                Text = CUSTOM_TEXT,
                Value = "4",
                Selected = _val == 4
            });

            ddl.SelectedItemId = "0";
            return ddl;
        }

        private int GetTransactionId()
        {
            int last_transaction = -1;
            //Last Transaction
            using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            {
                var q = db.TRANSACTION_TABLE.Select(m => m.LAST_TRANSACTION_ID).ToList().Last();
                last_transaction = (int)q;
                last_transaction++;
                db.ExecuteStoreCommand("exec sp_TRANSACTION_UPDATE @TRANSACTION = {0}", last_transaction);
            }
            return last_transaction;
        }
        #endregion

        #region CommentsForm

        public ActionResult CommentsForm()
        {
            Entities.CommentsModel cm = new Entities.CommentsModel();
            cm.type = Entities.CommentTypes.MONTHLY_COMMENTS;
            ViewBag.TabUrl = CONTROLLER_NAME + "/CommentsForm";
            ViewBag.Title = COMMENTS_FORM;

            ViewBag.BtnSubmitText = BUTTON_SUBMIT_TEXT;
            ViewBag.BtnDeleteText = BUTTON_DELETE_TEXT;

            using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
            {
                cm.channel_label = COMMENTS_CHANNEL_LABEL;
                var q = db.CHANNEL_MASTER.Select(m => m).ToList();
                List<SelectListItem> channel_list = new List<SelectListItem>();
                channel_list.Add(new SelectListItem
                {
                    Text = COMMENTS_CHANNEL_LIST_SELECT,
                    Value = "0",
                    Selected = true
                });
                channel_list.AddRange(q.Select(m=> new SelectListItem{
                    Text = m.NAME,
                    Value = m.ID.ToString()
                }).ToList());
                cm.channel_list = channel_list;
            }
            cm.group_label = COMMENTS_GROUP_LIST_LABEL;
            List<SelectListItem> group_list = new List<SelectListItem>();
            group_list.Add(new SelectListItem
            {
                Text = COMMENTS_CHANNEL_LIST_SELECT,
                Value = "0",
                Selected = true
            });
            cm.group_list = group_list;
            
            cm.user = Helpers.UserUtils.UserName;
            cm.year = Helpers.PeriodUtil.Year;
            cm.month = Helpers.PeriodUtil.Month;

            return View(LETTERS_FORM_VIEW,cm);
        }

        public ActionResult LettersForm()
        {
            Entities.CommentsModel cm = new Entities.CommentsModel();
            cm.type = Entities.CommentTypes.MANAGEMENT_LETTER;
            ViewBag.TabUrl = CONTROLLER_NAME + "/LettersForm";
            ViewBag.Title = LETTERS_FORM;

            ViewBag.BtnSubmitText = BUTTON_SUBMIT_TEXT;
            ViewBag.BtnDeleteText = BUTTON_DELETE_TEXT;

            using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
            {
                cm.master_group_label = COMMENTS_GROUP_LABEL;
                var q = db.MANAGEMENT_LETTERS_MASTER.Where(m => m.MAIN_GROUP == 0 && m.CHANNEL == 0).Select(m => m).ToList();
                List<SelectListItem> master_group_list = new List<SelectListItem>();
                master_group_list.Add(new SelectListItem
                {
                    Text = COMMENTS_CHANNEL_LIST_SELECT,
                    Value = "0",
                    Selected = true
                });
                master_group_list.AddRange(q.Select(m => new SelectListItem
                {
                    Text = m.NAME,
                    Value = m.ID.ToString()

                }).ToList());
                master_group_list.Add(new SelectListItem
                {
                    Text = "(By Channel)",
                    Value = "999999"
                });
                cm.master_group_list = master_group_list;

                cm.channel_label = COMMENTS_CHANNEL_LABEL;
                q = db.MANAGEMENT_LETTERS_MASTER.Where(m => m.MAIN_GROUP == 0 && m.CHANNEL_GROUP == 0 &&m.CHANNEL != 0).Select(m => m).ToList();
                List<SelectListItem> channel_list = new List<SelectListItem>();
                channel_list.Add(new SelectListItem
                {
                    Text = COMMENTS_CHANNEL_LIST_SELECT,
                    Value = "0",
                    Selected = true
                });
                channel_list.AddRange(q.Select(m => new SelectListItem
                {
                    Text = m.NAME,
                    Value = m.CHANNEL.ToString()

                }).ToList());
                cm.channel_list = channel_list;

                cm.group_label = "Tipo";
                //var t = db.v_WRK_MANAGEMENT_LETTERS.Where(m =>m.MAIN_GROUP == 0 && m.CHANNEL_GROUP != 0 ).Select(m => m).ToList();
                List<SelectListItem> group_list = new List<SelectListItem>();
                group_list.Add(new SelectListItem
                {
                    Text = COMMENTS_CHANNEL_LIST_SELECT,
                    Value = "0",
                    Selected = true
                });
                //group_list.AddRange(t.Select(m => new SelectListItem
                //{
                //    Text = m.NAME,
                //    Value = m.ID.ToString()

                //}).ToList());
                cm.group_list = group_list;
            }
            cm.id = 0;
            cm.letter_id = 0;
            cm.user = Helpers.UserUtils.UserName;
            cm.year = Helpers.PeriodUtil.Year;
            cm.month = Helpers.PeriodUtil.Month;

            return View(cm);
        }
        //Llamada para actualizar el combo de grupos
        /*
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult UpdateComments(string _selectedId, string _main, string type)
        {
            return UpdateCommentsGroups(_selectedId, _main, type);
        }
         * */
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult UpdateCommentsGroups(string _selectedId, string _main, string type)
        {
            if (_main == "999999") _main = "0";
            object[] obj = new object[]{};
            decimal? main = (_main== null)? 0: decimal.Parse(_main);
            decimal? channel = decimal.Parse(_selectedId);
            if (type == Entities.CommentTypes.MANAGEMENT_LETTER)
            {
                using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
                {
                    var q = db.v_WRK_MANAGEMENT_LETTERS.Where(m => m.CHANNEL_GROUP == channel && m.MAIN_GROUP == main).Select(m => m).ToList();
                    object[] tmp = new object[q.Count + 1];
                    tmp[0] = new
                        {
                            Id = "0",
                            Value = COMMENTS_LIST_SELECT
                        };
                    if (q.Count > 0)
                    {
                        for (int i = 0; i < q.Count; i++)
                        {
                            tmp[i + 1] = new
                            {
                                Id = q[i].GROUP_ID.ToString(),
                                Value = q[i].NAME
                            };
                        }
                    }
                    obj = tmp;
                }
            }
            else if (type == Entities.CommentTypes.MONTHLY_COMMENTS)
            {
                using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
                {
                    var q = db.LETTERS_COMMENT_DATA.Where(m => m.TYPE == Entities.CommentTypes.MONTHLY_COMMENTS && m.LETTER_ID == channel && m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).Select(m => m).ToList();
                    object[] tmp = new object[q.Count + 2];
                    tmp[0] = new
                    {
                        Id = "0",
                        Value = COMMENTS_LIST_SELECT
                    };
                    if (q.Count > 0)
                    {
                        for (int i = 0; i < q.Count; i++)
                        {
                            tmp[i + 1] = new
                            {
                                Id = q[i].ID.ToString(),
                                Value = (i + 1).ToString() + "- " + ((q[i].COMMENT.Length > 10) ? q[i].COMMENT.Substring(0, 10) : q[i].COMMENT) + "..."
                            };
                        }
                    }
                    tmp[tmp.Length - 1] = new
                    {
                        Id = "-1",
                        Value = COMMENTS_NEW
                    };
                    obj = tmp;
                }
            }
            

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //Pinta los datos en la caja de texto
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetCommentText(string _selectedId, string _type)
        {
            string text = "";
            int id = -1;
            try
            {
                if (int.Parse(_selectedId) != 0)
                {
                    using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
                    {
                        if (_type == Entities.CommentTypes.MANAGEMENT_LETTER)
                        {
                            var q = db.LETTERS_COMMENT_DATA.ToList().Where(m => m.LETTER_ID == int.Parse(_selectedId) && m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).FirstOrDefault();
                            text = q.COMMENT;
                            id = (int)q.ID;
                        }
                        else if (_type == Entities.CommentTypes.MONTHLY_COMMENTS)
                        {
                            var q = db.LETTERS_COMMENT_DATA.ToList().Where(m => m.ID == int.Parse(_selectedId) && m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).FirstOrDefault();
                            text = q.COMMENT;
                            id = (int)q.ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { text = text, id = id }, JsonRequestBehavior.AllowGet);
        }

        //Actualiza o inserta los comentarios.
        public JsonResult UpdateComment(Entities.CommentsModel collection)
        {
            int result = 0;
            decimal letter_id = 0;
            int comment_id = 0;
            int _id = 0;
            try
            {
                _id = (int)((collection.id == null)? -1: collection.id);
                collection.del_letter = (collection.del_letter == null)? String.IsNullOrEmpty(collection.text):collection.del_letter; //si el texto viene nulo, lo borramos de la tabla
                if (collection.type == Entities.CommentTypes.MANAGEMENT_LETTER)
                {
                    if ((int.Parse(collection.channel_selected) == 0) || (int.Parse(collection.master_group_selected) == 0) || (int.Parse(collection.group_selected) == 0))
                    {
                        throw new Exception(EXCEPTION_MESSAGE_NO_SELECT);
                    }
                    letter_id = decimal.Parse(collection.group_selected);
                }
                else if (collection.type == Entities.CommentTypes.MONTHLY_COMMENTS)
                {                                       
                    letter_id = decimal.Parse(collection.channel_selected);
                    comment_id = (int)collection.id;
                }
                using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
                {
                    //Si el id viene a nulo es que se tiene que insertar el comentario
                    var s = db.LETTERS_COMMENT_DATA.ToList().Where(m => m.ID == _id && m.TYPE == collection.type).FirstOrDefault();
                    if (s == null)
                    {
                        db.LETTERS_COMMENT_DATA.AddObject(new Entities.LETTERS_COMMENT_DATA
                        {
                            DATE = DateTime.Now,
                            COMMENT = collection.text,
                            USER = collection.user,
                            LETTER_ID = letter_id,
                            MONTH_PERIOD = (decimal)(Helpers.PeriodUtil.Month),
                            YEAR_PERIOD = (decimal)(Helpers.PeriodUtil.Year),
                            TYPE = collection.type.ToString()
                        });
                    }
                    else
                    {
                        // No viene el Id nulo, por lo que asumimos que se está actualizando.
                        Entities.LETTERS_COMMENT_DATA letter = db.LETTERS_COMMENT_DATA.ToList().Where(m => m.ID == _id && m.TYPE == collection.type).FirstOrDefault();
                        // Comprobamos si se quiere actualizar o borrar
                        if ((bool)collection.del_letter)
                        {
                            // Si no es nulo es que se tiene que borrar.
                            db.LETTERS_COMMENT_DATA.DeleteObject(letter);
                        }else{
                            // Se tiene que actualizar, solamente el comentario.
                            letter.COMMENT = collection.text;
                        }
                    }
                    result = db.SaveChanges();
                }
                return new JsonResult() { Data = new { Success = true, Result = result, channel = collection.letter_id, message = collection.success_text, container = collection.container }, ContentType = "text/html",JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false, Status = "Error", message = collection.error_text },
                    ContentType= "text/html",
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        #endregion

        #region Private Functions
        //Funciones privadas para varios usos.
        private decimal? CalcInsertData(string type, List<Entities.CalcData>_data, int this_year_period, int this_month_period)
        {
            decimal ret = 0;
            int counter = 0;
            Entities.CalcData temp = _data.Last();
            switch (type)
            {
                case CALC.MONTH:
                    //Month corresponde al último dato proporcionado
                    string result = _data.Select(m => m.data).Last().ToString();
                    ret = decimal.Parse(result);
                    break;
                case CALC.YTD:
                    //YTD corresponde a la suma de este año desde enero hasta el mes en curso
                    //Comprobar primero que hay datos anteriores a este mes en la tabla temporal
                    counter = _data
                                .Where(m => m.year_period == this_year_period && decimal.Parse(m.data) > 0 && (m.MARKET == temp.MARKET && m.BRAND == temp.BRAND && m.GROUP == temp.GROUP))
                                .Select(m => m.data).Count();
                    //Si no hay datos, comprobar si existen datos en las tablas de la base de datos
                    //El contador tiene que equivaler al mes actual de cálculo
                    if (counter < this_month_period)
                    {
                        //Identificar los meses a buscar datos
                        List<int> _months = _data.Where(m => m.year_period == this_year_period).Select(m => m.month_period).ToList();
                        //Buscamos los datos para el año en curso y los meses desde el 1 hasta el this_year sin incluir este
                        decimal total = 0;
                        for (int i = 1; i < this_month_period; i++)
                        {
                            //Si el mes a buscar no está incluido en la lista de meses trato de recuperar los datos
                            if (!_months.Exists(m => m == i))
                            {
                                using (Entities.MasterTable.MasterTableEntities db = new Entities.MasterTable.MasterTableEntities())
                                {
                                    if (temp.BRAND == null || temp.BRAND == 0)
                                    {
                                        var b = db.STRWM_MARKET_DATA.Where(m => m.MARKET == temp.MARKET && m.GROUP == temp.GROUP && m.MONTH_PERIOD == i && m.YEAR_PERIOD == this_year_period).Select(m => m).ToList();
                                        if (b.Count != 0) total += (decimal)b.First().MONTH;
                                    }
                                    else
                                    {
                                        var r = db.STRWM_BRAND_DATA.Where(m => m.MARKET == temp.MARKET && m.BRAND == temp.BRAND && m.MONTH_PERIOD == i && m.YEAR_PERIOD == this_year_period).Select(m => m).ToList();
                                        if (r.Count != 0) total += (decimal)r.First().MONTH;
                                    }
                                }
                            }
                        }
                        ret = total;
                        //Si aun así no hay datos, calcular la media de los datos que tenemos y multiplicar por el número del mes.
                    }
                    //Comprobamos que los meses sumados corresponde con la suma esperada
                    //Si es menor, tenemos que realizar la media y multiplicar el resultado por el número de meses, para obtener un resultado coherente.
                    //En el caso de YTD no debería ser necesario, ya que siempre tiene que haber datos para los meses de este mismo año.
                    //Obtener los datos del this_year
                    //sumar los datos desde el mes 1 al mes this_month
                    var aux_data = _data.Where(m => m.month_period <= this_month_period && m.year_period == this_year_period).ToList();
                    var by_id = aux_data.Where(m => m.BRAND == temp.BRAND && m.GROUP == temp.GROUP && m.MARKET == temp.MARKET).ToList();
                    ret += by_id.Select(m => m).Sum(m => decimal.Parse(m.data));
                    //ret += _data.Where(m => m.month_period <= this_month_period && m.year_period == this_year_period && (m.BRAND == temp.BRAND && m.GROUP == temp.GROUP && m.MARKET == temp.MARKET)).Select(m => m).Sum(m => decimal.Parse(m.data));
                    break;
                case CALC.MAT:
                    //MAT corresponde a la suma desde el més siguiente a this_month +1 del año anterior al this_month del año this_year
                    //Comprobar primero si hay datos anteriores a este mes en la tabla temporal
                    counter = _data
                                .Where(m => m.year_period == this_year_period && decimal.Parse(m.data) > 0 && (m.MARKET == temp.MARKET && m.BRAND == temp.BRAND && m.GROUP == temp.GROUP))
                                .Select(m => m.data).Count();
                    //Si no hay datos (menor de 12), comprobar si existen datos en las tablas de la base de datos
                    if (counter < 12)
                    {
                        //Identificar los meses a buscar datos
                        List<int> _months = _data.Where(m => (m.year_period == this_year_period && m.month_period <= this_month_period) || (m.year_period == this_year_period - 1 && m.month_period > this_month_period)).Select(m => m.month_period).ToList();
                        //Buscamos los datos para el año en curso y los meses desde el 1 hasta el this_year sin incluir este
                        decimal total = 0;
                        int month_counter = _months.Count;
                        //Tenemos que busca para el año en curso y menor que el mes actual y para el año anterior mayor que el mes actual.
                        for (int i = 1; i < this_month_period; i++)
                        {
                            //Si el mes a buscar no está incluido en la lista de meses trato de recuperar los datos
                            
                            if (!_months.Exists(m => m == i))
                            {
                                using (Entities.MasterTable.MasterTableEntities db = new Entities.MasterTable.MasterTableEntities())
                                {
                                    if (temp.BRAND == null || temp.BRAND == 0)
                                    {
                                        var b = db.STRWM_MARKET_DATA.Where(m => m.MARKET == temp.MARKET && m.GROUP == temp.GROUP && m.MONTH_PERIOD == i && m.YEAR_PERIOD == this_year_period).Select(m => m).ToList();
                                        if (b.Count != 0) total += (decimal)b.First().MONTH;
                                    }
                                    else
                                    {
                                        var r = db.STRWM_BRAND_DATA.Where(m => m.MARKET == temp.MARKET && m.BRAND == temp.BRAND && m.MONTH_PERIOD == i && m.YEAR_PERIOD == this_year_period).Select(m => m).ToList();
                                        if (r.Count != 0) total += (decimal)r.First().MONTH;
                                    }
                                }
                                month_counter++;
                            }
                        }
                        for (int i = this_month_period; i <= 12; i++)
                        {
                            //Si el mes a buscar no está incluido en la lista de meses trato de recuperar los datos

                            if (!_months.Exists(m => m == i))
                            {
                                using (Entities.MasterTable.MasterTableEntities db = new Entities.MasterTable.MasterTableEntities())
                                {
                                    if (temp.BRAND == null || temp.BRAND == 0)
                                    {
                                        var b = db.STRWM_MARKET_DATA.Where(m => m.MARKET == temp.MARKET && m.GROUP == temp.GROUP && m.MONTH_PERIOD == i && m.YEAR_PERIOD == this_year_period - 1).Select(m => m).ToList();
                                        if (b.Count != 0) total += (decimal)b.First().MONTH;
                                    }
                                    else
                                    {
                                        var r = db.STRWM_BRAND_DATA.Where(m => m.MARKET == temp.MARKET && m.BRAND == temp.BRAND && m.MONTH_PERIOD == i && m.YEAR_PERIOD == this_year_period - 1).Select(m => m).ToList();
                                        if (r.Count != 0) total += (decimal)r.First().MONTH;
                                    }
                                }
                                month_counter++;
                            }
                            
                        }
                        //Añadimos los datos que existen en la tabla temporal
                        decimal last = _data.Where(m => m.month_period > this_month_period && m.year_period == this_year_period - 1 && (m.BRAND == temp.BRAND && m.GROUP == temp.GROUP && m.MARKET == temp.MARKET)).Select(m => m).Sum(m => decimal.Parse(m.data));
                        decimal current = _data.Where(m => m.month_period <= this_month_period && m.year_period == this_year_period && (m.BRAND == temp.BRAND && m.GROUP == temp.GROUP && m.MARKET == temp.MARKET)).Select(m => m).Sum(m => decimal.Parse(m.data));
                        total += (last + current);
                        //Añadimos al calculo del numero de meses el numero de meses localizado en las tablas temporales
                        //month_counter += counter;
                        //Si aun así no hay datos, calcular la media de los datos que tenemos y multiplicar por 12.
                        if (month_counter < 12)
                        {
                            decimal partial = (total / month_counter) * 12;
                            total = partial;
                        }
                        ret = total;
                    }
                    else
                    {
                        decimal last = _data.Where(m => m.month_period > this_month_period && m.year_period == this_year_period - 1 && (m.BRAND == temp.BRAND && m.GROUP == temp.GROUP && m.MARKET == temp.MARKET)).Select(m => m).Sum(m => decimal.Parse(m.data));
                        decimal current = _data.Where(m => m.month_period <= this_month_period && m.year_period == this_year_period && (m.BRAND == temp.BRAND && m.GROUP == temp.GROUP && m.MARKET == temp.MARKET)).Select(m => m).Sum(m => decimal.Parse(m.data));
                        ret = last + current;
                    }
                    //Comprobamos que los meses sumados corresponde con la suma esperada
                    //Si es menor, tenemos que realizar la media y multiplicar el resultado por el número de meses, para obtener un resultado coherente.
                    //Obtener los datos de this_year -1  y this_year
                    //Sumar los datos de this_year -1 y this_month +1 hasta 12
                    //Sumar al anterior los datos de this_year desde mes 1 hasta this_month
                    break;
                case CALC.TOTAL:
                    ret = _data.Where(m => m.year_period == this_year_period && (m.BRAND == temp.BRAND && m.GROUP == temp.GROUP && m.MARKET == temp.MARKET)).Select(m => m).Sum(m => decimal.Parse(m.data));
                    
                    //TOTAL es la suma de los datos de this_year hasta el mes 12
                    //En principio solo es aplicable a los datos this_year cuyo this_year -1 tenga hasta el mes 12
                    break;
            }
            return Math.Round(ret,2);
        }
        #endregion

        #region constants

        private int channel = 0;
        private static string BOY_TYD_DATA = "YTDData";
        private static string _PATH = "~/Views/Forms/";
        private string BOY = _PATH + "_BOYTable.cshtml";
        private string BOY_EDIT = _PATH + "_BOYEdit.cshtml";
        private string RETURN_BOY_FORM = _PATH + "_ReturnBOYForm.cshtml";
		private string EDITED_BODY = _PATH + "_BOYEdited.cshtml";
        private string BOY_FORM = _PATH + "_BOYForm.cshtml";
        private string BOY_FORM_EDIT = "~/Views/Config/BOY/_EditForm.cshtml";
        private string BOY_CONFIGURE = _PATH + "BOYConfigure.cshtml";
        private string LOADER_VIEW = _PATH + "LoaderView.cshtml";
        private string MENU_BOY_CONFIGURE = "BOYConfigure";
        private string LETTERS_FORM_VIEW = _PATH + "LettersForm.cshtml";
        private const string SELECT_LIST_VIEW = "~/Views/FormUtil/_SelectOptions.cshtml";
        private const string CONTROLLER_NAME = "Forms";
        private const string EXCEPTION_MESSAGE_NO_SELECT = "No channel/group selected.";
        private const string NO_DATA_FOUND_MESSAGE = "No data found.";
        private const string WAIT_FOR_INFO_DATA = "Waiting for data to retrieved...";

        private const string LETTERS_FORM = "LettersForm";

        private const string COMMENTS_FORM = "CommentsForm";
        private const string COMMENTS_CHANNEL_LABEL = "Channel";
        private const string COMMENTS_LIST_SELECT = "(Select)";
        private const string COMMENTS_GROUP_LIST_LABEL = "Comments";
        private const string COMMENTS_GROUP_LABEL = "Grouping";
        private const string COMMENTS_NEW = "--New comment--";
        //StrawmanConstants sc = new StrawmanConstants();
        private const string COMMENTS_CHANNEL_LIST_SELECT = "(Select)";

        private const string SELECT_TEXT = "Select data source";
        private const string NTS_TEXT = "NTS Data";
        private const string NTS_ABR = "NTS";
        private const string MARKET_ABR = "MARKET SHARE";
        private const string BRAND_ABR = "BRAND CONTRIBUTION";
        private const string NIELSEN_TEXT = "Nielsen";
        private const string IMS_TEXT = "IMS";
        private const string CUSTOM_TEXT = "Nielsen/IMS Data";
        
        private const string SALES_EUROS_PUB = "Sales Euros PUB";
        private const string STOCK_UNITS ="Stock Units";

        private const string STATUS_Q = "Q";
        private const string STATUS_R = "R";

        private const string SP_TMP_NTS_DATA = "sp_TMP_NTS_DATA";
        private const string SP_TMP_NIELSEN_DATA = "sp_TMP_NIELSEN_DATA";
        private const string SP_TMP_IMS_DATA = "sp_TMP_IMS_DATA";
        private const string SP_CUSTOM_LOADER = "sp_CUSTOM_LOADER";

        private const string TMP_NTS_DATA = "TMP_NTS_DATA";
        private const string TMP_NIELSEN_DATA = "TMP_NIELSEN_DATA";
        private const string TMP_IMS_DATA = "TMP_IMS_DATA";

        private const string BUTTON_SUBMIT_TEXT = "Save";
        private const string BUTTON_DELETE_TEXT = "Clear comment";

        private const string MARKET_DESCRIPTION = "Market / Segment";
        private const string BRAND_DESCRIPTION = "J&J Brand";

        private const string KPI_SELECT_COLUMNS_NAME = "KPIColumns_Select";
        private const string KPI_SELECT_COLUMNS_LABEL = "Columns";
        private class CALC
        {
            public const string MONTH = "MONTH";
            public const string YTD = "YTD";
            public const string MAT = "MAT";
            public const string TOTAL = "TOTAL";

            public const string OTC = "OTC";
            public const string BEAUTY = "Beauty";
            public const string MASS = "Mass Market";
        }
        private class KPI_COLUMS
        {
            public const string NTS = NTS_ABR;
            public const string MARKET_SHARE = MARKET_ABR;
            public const string BRAND_CONTRIBUTION = BRAND_ABR;
        }
        #endregion

    }

}
