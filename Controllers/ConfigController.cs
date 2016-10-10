using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using StrawmanDBLibray.Classes;
using StrawmanApp.Classes.Extensions;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class ConfigController : Controller
    {
        //
        // GET: /Config/
        public ActionResult Index()
        {
            ViewBag.Title = "Config";
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.Message = MESSAGE;
            ViewBag.SubmitText = SUBMIT_TEXT;
            ViewBag.SuccessMessage = SUCCESS_MESSAGE;
            ViewBag.FailMessage = FAIL_MESSAGE;
            Entities.PeriodUtilModel model = GetPeriodData(new Entities.PeriodUtilModel());
            
            return View(model);
        }
        public ActionResult LoadConfig()
        {
            //TODO: 
            //Obtener los datos a mostrar en el formulario
            //Datos maestros:
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> master = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)StrawmanDBLibray.DBLibrary.GetMarketData(StrawmanDataTables.v_STRWM_MARKET_DATA,Helpers.PeriodUtil.Year,Helpers.PeriodUtil.Month);
            Models.ConfigModels cm = new Models.ConfigModels();
            cm.strwm_market_data = master.Where(m => m.MARKET < 9000 && m.BRAND < 9000).Select(m => m).ToList();
            //Columnas del excel:
            //Datos configurados:
            //  NTS:
            //  IMS/Nielsen: 
            return PartialView(LOAD_CONFIG);
        }
        public ActionResult CurrencyBanner()
        {
            string change_value = "";
            using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
            {
                var q = db.WRK_VIEWS_VARIABLES.Where(m => m.VIEW == DOLAR_CHANGE && (m.YEAR_PERIOD == null || m.YEAR_PERIOD <= Helpers.PeriodUtil.Year) &&(m.MONTH_PERIOD == null || m.MONTH_PERIOD <= Helpers.PeriodUtil.Month))
                                              .OrderByDescending(m=>m.YEAR_PERIOD).ThenByDescending(m=>m.MONTH_PERIOD).FirstOrDefault();
                double val = 1;
                if(double.TryParse(q.VALUE,out val)){
                    change_value = val.ToString();
                }else{
                    change_value = val.ToString();
                }
            }
            return PartialView(CURRENCY_BANNER, new ViewDataDictionary { { "dolar_change", change_value } });
        }
        public ActionResult CurrencyAdjust()
        {
            string default_value = "1";
            string min_value = "1";
            string max_value = "1000000";
            string increment = "1000";
            return PartialView(CURRENCY_ADJUST, new ViewDataDictionary { { "default-value", default_value },{"min-value",min_value},{"max-value",max_value},{"increment",increment} });
        }
        public ActionResult PeriodBanner()
        {
            string _month = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", CultureInfo.InvariantCulture));
            return PartialView(PERIOD_BANNER, new ViewDataDictionary { { "_monthPeriod", _month }, { "_yearPeriod", Helpers.PeriodUtil.Year.ToString() } });
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult PeriodicCombo()
        {
            return PartialView(SHARED_PERIOD_SELECTOR, GetPeriodData(new Entities.PeriodUtilModel()));
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetPeriodBanner()
        {
            string _month = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month,1).ToString("MMMM",CultureInfo.InvariantCulture));
            return PartialView(PERIOD_BANNER, new ViewDataDictionary {{"_monthPeriod" , _month}, {"_yearPeriod", Helpers.PeriodUtil.Year.ToString() }});
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpGet]
        public ActionResult GetMonthList(string _year)
        {
            IEnumerable<SelectListItem> obj = null;
            decimal? year = decimal.Parse(_year);
            using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            {
                var q = db.PERIOD_TABLE.Where(m => m.YEAR_PERIOD == year).Select(m => m).ToList();
                obj = q.Select(m => new SelectListItem
                {
                    Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(new DateTime(DateTime.Now.Year, (int)m.MONTH_PERIOD, 1).ToString("MMMM", CultureInfo.InvariantCulture)),
                    Value =m.MONTH_PERIOD.ToString()
                }).ToList();
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpGet]
        public JsonResult ChangePeriodAjax(string _year, string _month)
        {
            System.Collections.Specialized.NameValueCollection n = new System.Collections.Specialized.NameValueCollection();

            using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            {
                int _y = int.Parse(_year);
                var q = db.PERIOD_TABLE.Where(m => m.YEAR_PERIOD == _y).Max(m => m.MONTH_PERIOD);
                if (int.Parse(_month) > q.Value) _month = q.Value.ToString();
            }
            n.Add("_month",_month);
            n.Add("_year",_year);
            FormCollection collection = new FormCollection(n);
            return ChangePeriod(collection);
        }
        public JsonResult ChangePeriod(FormCollection collection)
        {
            Helpers.PeriodUtil.Month = int.Parse(collection[0]);
            Helpers.PeriodUtil.Year = int.Parse(collection[1]);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
        }

        private Entities.PeriodUtilModel GetPeriodData(Entities.PeriodUtilModel model)
        {
            using (Entities.GodzillaEntity.GodzillaEntities db = new Entities.GodzillaEntity.GodzillaEntities())
            {
                decimal _id = (decimal)db.PERIOD_TABLE.OrderBy(m => m.ID).ToList().Last().ID;
                var q = db.PERIOD_TABLE.OrderBy(m=>new{m.YEAR_PERIOD,m.MONTH_PERIOD}).Select(m => m);
                //Año y mes por defecto
                model.selected_year_period = Helpers.PeriodUtil.Year;
                model.selected_month_period = Helpers.PeriodUtil.Month;
                //Listado de meses disponibles
                model.month_list = q.ToList().Where(m => m.YEAR_PERIOD == model.selected_year_period).Select(m => new SelectListItem
                {
                    Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(new DateTime(DateTime.Now.Year, (int)m.MONTH_PERIOD, 1).ToString("MMMM", CultureInfo.InvariantCulture)),
                    Value = m.MONTH_PERIOD.ToString(),
                    Selected = (m.MONTH_PERIOD == model.selected_month_period)
                });
                model.year_list = q.ToList().Select(m => m.YEAR_PERIOD).Distinct().Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = m.ToString(),
                    Selected = (m == model.selected_year_period)
                }).Distinct();
                model.year_text = PERIOD_YEAR_TEXT;
                model.month_text = PERIOD_MONTH_TEXT;
            }
            return model;
        }
        #region LoaderPreview

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult LoaderPreview()
        {
            return PartialView(LOADER_PREVIEW);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetNTSPreview()
        {
            ViewDataDictionary data = new ViewDataDictionary();
            List<object> rows = new List<object>();
            //Buscar en la base de datos
            List<StrawmanDBLibray.Entities.STRWM_NTS_DATA_BCK> bdata = (List<StrawmanDBLibray.Entities.STRWM_NTS_DATA_BCK>)StrawmanDBLibray.DBLibrary.GetLoaderData(StrawmanDBLibray.Classes.StrawmanDataTables.STRWM_NTS_DATA_BCK);
            //Introducir los datos de la tabla como celdas por cada fila.
            foreach (StrawmanDBLibray.Entities.STRWM_NTS_DATA_BCK drow in bdata)
            {
                List<ViewDataDictionary> cells = new List<ViewDataDictionary>();
                ViewDataDictionary cell_content = new ViewDataDictionary();

                string cell_data_attributes = "";
                string cell_classes = "";
                cell_content.Add(Classes.HtmlElements.CLASSES, cell_classes);

                ViewDataDictionary cell = new ViewDataDictionary();
                cell.Add(Classes.HtmlElements.PARTIAL_VIEW, TD_VIEW);
                //DESCRIPTION
                //cell_data_attributes = "";
                //cell_content.Add(Classes.HtmlElements.DATA_ATTRIBUTES, cell_data_attributes);
                //cell_content.Add(Classes.HtmlElements.CONTENT, drow.DESCRIPTION.ToString());
                //cell.Add(Classes.HtmlElements.CONTENT, cell_content);
                //cells.Add(cell);
                //MONTH AMOUNT ACTUAL
                cell = new ViewDataDictionary();
                cell_content = new ViewDataDictionary();
                cell.Add(Classes.HtmlElements.PARTIAL_VIEW, TD_VIEW);
                cell_data_attributes = "data-id=\"" + drow.ID + "\" data-original-data=\"" + drow.MONTH_AMOUNT_ACTUAL + "\" ";
                cell_content.Add(Classes.HtmlElements.DATA_ATTRIBUTES, cell_data_attributes);
                cell_content.Add(Classes.HtmlElements.CONTENT, drow.MONTH_AMOUNT_ACTUAL);
                cell.Add(Classes.HtmlElements.CONTENT, cell_content);
                cells.Add(cell);
                //YTD AMOUNT ACTUAL
                cell = new ViewDataDictionary();
                cell_content = new ViewDataDictionary();
                cell.Add(Classes.HtmlElements.PARTIAL_VIEW, TD_VIEW);
                cell_data_attributes = "data-id=\"" + drow.ID + "\" data-original-data=\"" + drow.YTD_AMOUNT_ACTUAL + "\" ";
                cell_content.Add(Classes.HtmlElements.DATA_ATTRIBUTES, cell_data_attributes);
                cell_content.Add(Classes.HtmlElements.CONTENT, drow.YTD_AMOUNT_ACTUAL);
                cell.Add(Classes.HtmlElements.CONTENT, cell_content);
                cells.Add(cell);
                //YEAR PERIOD
                cell = new ViewDataDictionary();
                cell_content = new ViewDataDictionary();
                cell.Add(Classes.HtmlElements.PARTIAL_VIEW, TD_VIEW);
                cell_data_attributes = "";
                cell_content.Add(Classes.HtmlElements.DATA_ATTRIBUTES, cell_data_attributes);
                cell_content.Add(Classes.HtmlElements.CONTENT, drow.YEAR_PERIOD.ToString());
                cell.Add(Classes.HtmlElements.CONTENT, cell_content);
                cells.Add(cell);
                //MONTH PERIOD
                cell = new ViewDataDictionary();
                cell_content = new ViewDataDictionary();
                cell.Add(Classes.HtmlElements.PARTIAL_VIEW, TD_VIEW);
                cell_data_attributes = "";
                cell_content.Add(Classes.HtmlElements.DATA_ATTRIBUTES, cell_data_attributes);
                cell_content.Add(Classes.HtmlElements.CONTENT, drow.MONTH_PERIOD.ToString());
                cell.Add(Classes.HtmlElements.CONTENT, cell_content);
                cells.Add(cell);
                rows.Add(cells);
            }
            data.Add(Classes.HtmlElements.TABLE_ROWS, rows);
            string classes = "";
            data.Add(Classes.HtmlElements.CLASSES, classes);

            string data_atributtes = "";
            data.Add(Classes.HtmlElements.DATA_ATTRIBUTES, data_atributtes);

            return PartialView(TR_VIEW, data);
        }

        #endregion
        #region GroupsConfig
        public ActionResult GroupsConfig()
        {
            //ViewBag.MenuUrl = MENU_BOY_CONFIGURE;
            ViewBag.TabUrl = GROUPS_CONFIGURE;
            return View();
        }
        public ActionResult GetTabs(int? type)
        {
            //Obtener los tipos de datos
            //List<StrawmanDBLibray.Entities.GROUP_TYPES> types = (List<StrawmanDBLibray.Entities.GROUP_TYPES>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(StrawmanDataTables.GROUP_TYPES);
            //Cargar datos en el modelo tabs
            Models.DropDownListModels dd = new Models.DropDownListModels();
            dd.id = "Select Group Type";
            dd.title = "Group Type";
            dd.data_attributes = "data-type=\"group_types\" data-target=\".master.wrapper\" data-controller=\"GetMasterData\"";
            dd.data_type = "group_types";
            dd.data_target = "master";
            dd.data_controller = "GetMasterData";
            dd.Items = StrawmanDBLibray.Repository.GROUP_TYPES.getAll().Select(m => new SelectListItem
            {
                Text = m.NAME,
                Value = m.ID.ToString(),
            }).ToList();
            //List<Models.TabItem> data = types.Select(p => new Models.TabItem
            //                            {
            //                                select_default = (type == p.ID || type == null)?"true":"false",
            //                                tab_url = p.ID.ToString(),
            //                                url="GetMasterData",
            //                                title = p.NAME
            //                            }).ToList();
            //Devolver vista parial de BootstrapTabs
            return PartialView(DROP_DOWN_LIST_FOR,dd);
        }
        public ActionResult GetMasterData(int? type)
        {
            //Obtener los datos Master según el tipo
            List<StrawmanDBLibray.Entities.GROUP_MASTER> types = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(StrawmanDataTables.GROUP_MASTER);
            //Cargar datos en el modelo MasterData
            
            List<Models.Groups_Data> data = types.Where(m => type == null ? m.TYPE == 1 : m.TYPE == type).Select(p => new Models.Groups_Data
                                        {
                                            name = p.NAME,
                                            id = p.ID,
                                            level  = p.LEVEL,
                                            base_id = p.BASE_ID,
                                            config = p.GROUP_CONFIG,
                                            type_id = p.TYPE,
                                            group_id = p.ID
                                        }).ToList();
            //Devolver vista parcial de Master Data
            return PartialView(GROUP_MASTER, data);
        }
        public ActionResult GetConfigData(int? type, int? group)
        {
            //Obtener los datos de configuración según tipo y grupo
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> config = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)StrawmanDBLibray.DBLibrary.GetStrawmanConfig(StrawmanDataTables.GROUP_CONFIG);
            //Cargar datos en el modelo Grid
            List<Models.MasterDataModels> model = config.Where(m => (type == null ? m.TYPE_ID == 1 : m.TYPE_ID == type) && (group == null ? m.GROUP_ID == 1 : m.GROUP_ID == group)).Select(p => new Models.MasterDataModels
                                        {
                                            brand_name = p.BRAND_NAME,
                                            id = p.ID,
                                            config_name = Models.ConfigOperationsModel.GetOppName(p.CONFIG),
                                            source = p.SOURCE
                                        }).ToList();
            //Devolver vista parcial de Grid con opciones de edición
            return PartialView(GROUP_CONFIG, model);
        }
        public ActionResult GetEditForGroupMaster(int _id)
        {
            return GetGroupEditFor(StrawmanDataTables.GROUP_MASTER, _id);
        }
        public ActionResult GetEditForGroupConfig(int _id)
        {
            return GetGroupEditFor(StrawmanDataTables.GROUP_CONFIG, _id);
        }

        private ActionResult GetGroupEditFor(string table, int _id)
        {
            switch (table)
            {
                case StrawmanDataTables.GROUP_CONFIG:
                    StrawmanDBLibray.Entities.GROUP_CONFIG cfg = StrawmanDBLibray.Repository.GROUP_CONFIG.getById(_id);
                    List<Models.MasterDataModels> model_g_c = new List<Models.MasterDataModels>();
                    model_g_c.Add(new Models.MasterDataModels
                    {
                        brand_name = cfg.BRAND_NAME,
                        id = cfg.ID,
                        config_name = Models.ConfigOperationsModel.GetOppName(cfg.CONFIG),
                        config_list = Models.ConfigOperationsModel.GetOppList(cfg.CONFIG),
                        config = cfg.CONFIG,
                        source = cfg.SOURCE
                    });
                    return PartialView(GROUP_CONFIG_EDIT, model_g_c);
            }
            return null;
        }
        #endregion
        #region PreviewData

        public ActionResult PreviewData()
        {
            ViewBag.Title = PREVIEW_CONTROLLER;
            ViewBag.MenuUrl = CONTROLLER;
            return View(PREVIEW_DATA_VIEW);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult PreviewDataGrid()
        {
            Models.StrawmanDataModel model = new Models.StrawmanDataModel();
            model.brand = (List<Entities.StrwmanPreviewBrandData>)GetStrawmanData(BRAND);
            model.market = (List<Entities.StrwmanPreviewMarketData>)GetStrawmanData(MARKET);
            return PartialView(PREVIEW_DATA_GRID, model);
                //Json(new{data = model}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PreviewDataGridAction()
        {
            Models.StrawmanDataModel model = new Models.StrawmanDataModel();
            model.brand = (List<Entities.StrwmanPreviewBrandData>)GetStrawmanData(BRAND);
            model.market = (List<Entities.StrwmanPreviewMarketData>)GetStrawmanData(MARKET);
            return PartialView(PREVIEW_DATA_GRID, model);
            //Json(new{data = model}, JsonRequestBehavior.AllowGet);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult PreviewDataRowEdit(string market, string brand, string channel, string mode)
        {
            string PARTIAL_VIEW = mode == "edit" ? PREVIEW_DATA_GRID_ROW : PREVIEW_DATA_GRID;
            Models.StrawmanDataModel model = new Models.StrawmanDataModel();
            if (!string.IsNullOrEmpty(brand))
            {
                model.brand = ((List<Entities.StrwmanPreviewBrandData>)GetStrawmanData(BRAND))
                    .Where(m => m.MARKET == int.Parse(market) && m.BRAND == int.Parse(brand) && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).Select(m => m).ToList();
            }
            else
            {
                model.market = ((List<Entities.StrwmanPreviewMarketData>)GetStrawmanData(MARKET))
                    .Where(m => m.MARKET == int.Parse(market) && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).Select(m => m).ToList();
            }
            return PartialView(PARTIAL_VIEW, model);
        }
        public ActionResult PreviewDataRow(string market, string brand, string channel)
        {
            return PreviewDataRowEdit(market,brand,channel,null);
        }
        public ActionResult UpdateDataRow(FormCollection coll)
        {
            int id = int.Parse(coll["id"].ToString());
            return Json(new { status = "success" });
        }
        private object GetStrawmanData(string type)
        {
            object ret = new object();
            using (Entities.MasterTable.MasterTableEntities db = new Entities.MasterTable.MasterTableEntities())
            {
                switch (type)
                {
                    case MARKET:
                        var q = db.STRWM_MARKET_DATA
                                .Join(db.MARKET_MASTER, p => p.MARKET, t => t.ID, (p, t) => new { p.MARKET, p.TOTAL, p.MONTH, p.MAT, p.YTD, p.DESCRIPTION, t.CHANNEL, p.YEAR_PERIOD, p.MONTH_PERIOD })
                                .Where(m => m.MARKET != null
                                        && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                .Select(m => new Entities.StrwmanPreviewMarketData
                                {
                                    MARKET = m.MARKET,
                                    CHANNEL = m.CHANNEL,
                                    TOTAL = m.TOTAL,
                                    MONTH = m.MONTH,
                                    MAT = m.MAT,
                                    YTD = m.YTD,
                                    DESCRIPTION = m.DESCRIPTION,
                                    YEAR_PERIOD =m.YEAR_PERIOD,
                                    MONTH_PERIOD = m.MONTH_PERIOD

                                });
                        ret = new List<Entities.StrwmanPreviewMarketData>();
                        ret = q.ToList();
                        break;
                    case BRAND:
                        var s = db.STRWM_BRAND_DATA
                                    .Join(db.BRAND_MASTER, b => b.BRAND, d => d.ID, (b, d) => new { b.MARKET, b.BRAND, b.TOTAL, b.MONTH, b.MAT, b.YTD, b.DESCRIPTION, d.CHANNEL, b.YEAR_PERIOD, b.MONTH_PERIOD })
                                    .Where(m => m.MARKET != null && m.BRAND != null && m.DESCRIPTION != "N/A" && m.DESCRIPTION != null
                                            && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                                    .Select(m => new Entities.StrwmanPreviewBrandData
                                    {
                                        MARKET = m.MARKET,
                                        BRAND = m.BRAND,
                                        CHANNEL = m.CHANNEL,
                                        TOTAL = m.TOTAL,
                                        MONTH = m.MONTH,
                                        MAT = m.MAT,
                                        YTD = m.YTD,
                                        DESCRIPTION = m.DESCRIPTION,
                                        YEAR_PERIOD = m.YEAR_PERIOD,
                                        MONTH_PERIOD = m.MONTH_PERIOD
                                    });
                        ret = new List<Entities.StrwmanPreviewBrandData>();
                        ret = s.ToList();
                        break;
                }
            }
            
            return ret;
        }

        #endregion

        #region MasterData
        [HttpGet]
        [Authorize]
        public ActionResult GetGroupsSelectList(string _channel)
        {
            int channel = _channel != null ? int.Parse(_channel) : Helpers.Channels.MASS;
            List<SelectListItem> lst = GetGroupList(channel);
            Models.MasterDataModels model = new Models.MasterDataModels
            {
                group = 0,
                group_list = lst
            };
            return PartialView(DROP_DOWN_LIST, model);

        }
        public ActionResult ChangeChanel(string _channel)
        {
            int channel = _channel != null ? int.Parse(_channel) : Helpers.Channels.MASS;

            return MasterDataGrid(channel.ToString(),"1");
        }
        public ActionResult MasterData()
        {
            ViewBag.Channel = Helpers.StrawmanConstants.getChannel(Helpers.StrawmanConstants.CHANNEL_MASS);
            this.channel = ViewBag.Channel;
            return View();
        }
        public ActionResult MasterDataGrid(string _channel, string grid_page)
        {
            List<Models.MasterDataModels> model = new List<Models.MasterDataModels>();
            _channel = _channel == null?"1":_channel; 
            model = GetMasterData(int.Parse(_channel));
            return PartialView(MASTER_DATA_GRID, model);
        }
        public ActionResult MasterDataRow(string market, string brand, string channel)
        {
            List<Models.MasterDataModels> model = GetMasterData(int.Parse(channel))
                .Where(m => m.id == int.Parse(market) && m.brand == int.Parse(brand)).Select(m => m).ToList();
            return PartialView(MASTER_DATA_GRID, model);
        }
        public ActionResult MasterDataRowEdit(string market, string brand, string channel)
        {
            ViewBag.ModalId = "add_item";
            ViewBag.ModalTitle = Helpers.MessageByLanguage.EditItem;
            ViewBag.DefaultForm = "EditItemForm";
            ViewBag.DefaultFormParams = new {type = Classes.StrawmanViews.MARKET, channel = channel, market = market, brand = brand };
            ViewBag.DefaultController = CONTROLLER_NAME;
            ViewBag.DefaultFooter = "GetFooterForItemForm";
            return PartialView(new Models.FormModel().modal_view);

            //List<Models.MasterDataModels> model = GetMasterData(int.Parse(channel))
            //    .Where(m => m.id == int.Parse(market) && m.brand == int.Parse(brand)).Select(m=>m).ToList();
            //return PartialView(MASTER_DATA_EDIT_ROW, model);
        }
        public ActionResult EditItemForm(string type, string channel, string market, string brand)
        {
            channel = channel == null ? Helpers.Channels.MASS.ToString() : channel;
            type = type == null ? Classes.StrawmanViews.MARKET : type;
            Models.FormModel _model = FormUtilController.GetMasterDataFormFor(type, int.Parse(channel), int.Parse(market), int.Parse(brand));
            return PartialView(_model.view, _model);
        }
        public ActionResult NewItemForm(string type, string channel)
        {
            channel = channel == null ? Helpers.Channels.MASS.ToString() : channel;
            type = type == null ? Classes.StrawmanViews.MARKET : type;
            Models.FormModel _model = FormUtilController.GetMasterDataFormFor(type, int.Parse(channel));
            return PartialView(_model.view, _model);
        }
        public ActionResult GetSelectList(string _data, string _type)
        {
            int channel = _data != null ? int.Parse(_data) : Helpers.Channels.MASS;
            return new FormUtilController().GetSelectListFor(_type, channel);
            //List<SelectListItem> lst = GetGroupList(channel);
            //Models.MasterDataModels model = new Models.MasterDataModels
            //{
            //    group = 0,
            //    group_list = lst
            //};
            //return PartialView(DROP_DOWN_LIST, model);

        }
        [ChildActionOnly]
        public ActionResult GetNavButtonsForItemForm()
        {
            return new FormUtilController().GetButtonsGroupByName(FormUtilController.ButtonListsTypes.MASTER_DATA_NAV);
        }
        [ChildActionOnly]
        public ActionResult GetFooterForItemForm()
        {
            return new FormUtilController().GetButtonsGroupByName(FormUtilController.ButtonListsTypes.SAVE_AND_CLOSE);
        }
        public ActionResult AddItemForm()
        {
            ViewBag.ModalId = "add_item";
            ViewBag.ModalTitle = Helpers.MessageByLanguage.AddItem;
            ViewBag.DefaultForm = "NewItemForm";
            ViewBag.DefaultController = CONTROLLER_NAME;
            ViewBag.DefaultFooter = "GetFooterForItemForm";
            return PartialView(new Models.FormModel().modal_view);
        }
        private List<Models.MasterDataModels> GetMasterData(int _channel)
        {
            if (Helpers.Session.GetSession(GET_MASTER_DATA) != null)
                return ((List<Models.MasterDataModels>)Helpers.Session.GetSession(GET_MASTER_DATA)).Where(m=>m.channel == _channel).Select(m=>m).ToList();

            List<Models.MasterDataModels> model = new List<Models.MasterDataModels>();
            //List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> db = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_MARKET_DATA, true);
            List<StrawmanDBLibray.Entities.MARKET_MASTER> mmaster = (List<StrawmanDBLibray.Entities.MARKET_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_MASTER, false);
            List<StrawmanDBLibray.Entities.BRAND_MASTER> bmaster = (List<StrawmanDBLibray.Entities.BRAND_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.BRAND_MASTER, false);
            List<StrawmanDBLibray.Entities.ROSETTA_LOADER> rosetta = (List<StrawmanDBLibray.Entities.ROSETTA_LOADER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.ROSETTA_LOADER, false);
            var db = mmaster.Join(bmaster, m => new { _market = m.ID }, n => new { _market = (decimal)n.MARKET }, (m, n) => new { m = m, n = n })
                .AsEnumerable();
            List<StrawmanDBLibray.Entities.MARKET_GROUPS> groups = (List<StrawmanDBLibray.Entities.MARKET_GROUPS>) Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_GROUPS, true);
            //var q = db.Where(m => m.n.ID < 9000 && m.n.MARKET < 9000).Select(m=>m);
            //var g = q.ToList().Join(groups, p=>p.GROUP, s=>s.ID, (p,s)=> new{p.GROUP, s.ID, s.NAME}).Distinct().Select(m => new SelectListItem
            var g = groups.Select(m=>new SelectListItem
            {
                Text = m.NAME,
                Value = m.ID.ToString()
            }); 
            
            List<SelectListItem> group_list = g.ToList();
            List<SelectListItem> channels = GetChannelList(_channel);
            var md = db.ToList().Select(m => new Models.MasterDataModels
            {
                channel= m.m.CHANNEL,
                channel_name = channels.Where(n => decimal.Parse(n.Value) == m.m.CHANNEL).FirstOrDefault().Text,
                brand = m.n.ID,
                market = m.m.ID,
                id = m.m.ID,
                brand_name = m.n.NAME,
                market_name = m.m.NAME,
                source = rosetta.Exists(r => r.BRAND_ID == m.n.ID && r.MARKET_ID == m.m.ID)?rosetta.Find(r => r.BRAND_ID == m.n.ID && r.MARKET_ID == m.m.ID).SOURCE:Helpers.MessageByLanguage.NotFound,
                group = m.m.GROUP,
                group_name = groups.Exists(n => n.ID == m.m.GROUP)?groups.Where(n => n.ID == m.m.GROUP).FirstOrDefault().NAME: Helpers.MessageByLanguage.NotFound,
                order = m.n.GROUP,
                group_list = group_list,
                channel_list = channels,
                status = rosetta.Exists(r => r.BRAND_ID == m.n.ID && r.MARKET_ID == m.m.ID) ? rosetta.Find(r => r.BRAND_ID == m.n.ID && r.MARKET_ID == m.m.ID).STATUS : null,
                active_class = rosetta.Exists(r => r.BRAND_ID == m.n.ID && r.MARKET_ID == m.m.ID) ? 
                                    rosetta.Find(r => r.BRAND_ID == m.n.ID && r.MARKET_ID == m.m.ID).STATUS =="A"?
                                        Classes.HtmlElements.ButtonStyles.SUCCESS: 
                                        rosetta.Find(r => r.BRAND_ID == m.n.ID && r.MARKET_ID == m.m.ID).STATUS =="B"?
                                            Classes.HtmlElements.ButtonStyles.DANGER : Classes.HtmlElements.ButtonStyles.WARNING : Classes.HtmlElements.ButtonStyles.DISABLED
            });

            model = md.ToList();
            Helpers.Session.SetSession(GET_MASTER_DATA, model);
            return model.Where(m => m.channel == _channel).Select(m => m).ToList();
        }
        public string GetNameFor(string table, int? _id)
        {
            string ret = null;
            switch (table)
            {
                case StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_MASTER:
                    List<StrawmanDBLibray.Entities.MARKET_MASTER> mst = (List<StrawmanDBLibray.Entities.MARKET_MASTER>)Helpers.StrawmanDBLibrayData.Get(table, true);
                    ret = mst.Find(m => m.ID == _id).NAME;
                    break;
                case StrawmanDBLibray.Classes.StrawmanDataTables.BRAND_MASTER:
                    List<StrawmanDBLibray.Entities.BRAND_MASTER> bst = (List<StrawmanDBLibray.Entities.BRAND_MASTER>)Helpers.StrawmanDBLibrayData.Get(table, true);
                    ret = bst.Find(m => m.ID == _id).NAME;
                    break;
                case StrawmanDBLibray.Classes.StrawmanDataTables.CHANNEL_MASTER:
                    List<StrawmanDBLibray.Entities.CHANNEL_MASTER> cst = (List<StrawmanDBLibray.Entities.CHANNEL_MASTER>)Helpers.StrawmanDBLibrayData.Get(table, true);
                    ret = cst.Find(m => m.ID == _id).NAME;
                    break;
            }
            return ret;
        }
        /// <summary>
        /// Devuelve el grupo por JSON seleccionado para el Market
        /// </summary>
        /// <param name="_var1">MARKET_ID</param>
        /// <returns>Json GROUP_ID</returns>
        public JsonResult SetGroupByMarket(string _var1)
        {
            int _market = int.Parse(_var1);
            List<StrawmanDBLibray.Entities.MARKET_MASTER> mstr = (List<StrawmanDBLibray.Entities.MARKET_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_MASTER,false);
            decimal? grp = mstr.Find(m => m.ID == _market).GROUP;

            return Json(new {group = grp}, JsonRequestBehavior.AllowGet);
            
        }

        public List<SelectListItem> GetGroupList(int _channel)
        {
            List<StrawmanDBLibray.Entities.MARKET_GROUPS> groups = (List<StrawmanDBLibray.Entities.MARKET_GROUPS>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_GROUPS, true);
            List<StrawmanDBLibray.Entities.MARKET_MASTER> db = (List<StrawmanDBLibray.Entities.MARKET_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_MASTER, true);
            //List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> db = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_MARKET_DATA, true);
            //var q = db.Where(m => m.BRAND < 9000 && m.MARKET < 9000).Select(m => m);
            var g = db.ToList().Join(groups, p => p.GROUP, s => s.ID, (p, s) => new { p.GROUP, s.ID, s.NAME, p.CHANNEL }).Distinct().Where(m=>m.CHANNEL == _channel || m.CHANNEL == null).Select(m => new SelectListItem
            {
                Text = m.NAME.Replace("TOTAL", "").Trim(),
                Value = m.ID.ToString()
            });
            return g.ToList();
        }
        public List<SelectListItem> GetChannelList(int? _channel)
        {
            List<StrawmanDBLibray.Entities.CHANNEL_MASTER> channels = (List<StrawmanDBLibray.Entities.CHANNEL_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.CHANNEL_MASTER, true);
            return channels.Select(m => new SelectListItem
            {
                Text = m.NAME.Replace("TOTAL", "").Trim(),
                Value = m.ID.ToString(),
                Selected = m.ID == _channel
            }).ToList();
        }
        public List<SelectListItem> GetMarketList(int _channel, int? _selected)
        {
            List<StrawmanDBLibray.Entities.MARKET_MASTER> master = (List<StrawmanDBLibray.Entities.MARKET_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_MASTER,true);
            return master.Where(m => m.CHANNEL == _channel).Select(m => new SelectListItem
            {
                Text = m.NAME,
                Value = m.ID.ToString(),
                Selected = m.ID == _selected
            }).ToList();
        }
        #endregion

        #region UserManagement

        public ActionResult Users()
        {
            ViewBag.MenuUrl = USER_CONFIGURE;
            ViewBag.TabUrl = CONTROLLER_NAME + "/" + USER_CONFIGURE;
            Models.UsersManageModel model = new Models.UsersManageModel();
            List<StrawmanDBLibray.Entities.USERS_ROLES> list = (List<StrawmanDBLibray.Entities.USERS_ROLES>) Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.USERS_ROLES, true);
            model.UsersList = list.Select(m => new SelectListItem { Text = m.USER, Value = m.ID.ToString() }).ToList();
            model.user = list.FirstOrDefault().USER;
            return View(USERS_CONFIG,model);
        }

        public ActionResult UsersManagement(string _user, int _channel)
        {
            Models.UsersManageModel model = new Models.UsersManageModel();
            List<StrawmanDBLibray.Entities.MENU_MASTER> menu_master = (List<StrawmanDBLibray.Entities.MENU_MASTER>) Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.MENU_MASTER, true);
            List<StrawmanDBLibray.Entities.MENU_CONFIG> menu_config = (List<StrawmanDBLibray.Entities.MENU_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.MENU_CONFIG, false);
            model.MenuAccess = menu_master.Where(m => m.LEVEL > 0).Select(m => new Models.UserMenuAccess
            {
                ID = m.ID,
                NAME = m.NAME,
                isChecked = menu_config != null && menu_config.Exists(t=>t.USER == _user && t.MENU_ID== m.ID),
                Config = new StrawmanDBLibray.Entities.MENU_CONFIG { MENU_ID = m.ID, PERMISSION = menu_config == null || !menu_config.Exists(t => t.USER == _user && t.MENU_ID == m.ID)? "RO" : menu_config.Where(t => t.USER == _user && t.MENU_ID == m.ID).FirstOrDefault().PERMISSION, USER = _user }

            }).ToList();
            model.ViewTable = Helpers.StrawmanViews.BOY.name;
            model.ViewsList = GetViewsList();

            Models.UsersViewAccess vuser = new Models.UsersViewAccess();
            vuser.views = new List<SelectListItem>();
            vuser.views.Add(new SelectListItem { Text = Helpers.StrawmanViews.BOY.name, Value = Helpers.StrawmanViews.BOY.id, Selected = true });
            vuser.views.Add(new SelectListItem { Text = Helpers.StrawmanViews.MANAGEMENT_LETTERS.name, Value = Helpers.StrawmanViews.MANAGEMENT_LETTERS.id});
            vuser.views.Add(new SelectListItem { Text = Helpers.StrawmanViews.MONTHLY_COMMENTS.name, Value = Helpers.StrawmanViews.MONTHLY_COMMENTS.id });
            List<StrawmanDBLibray.Entities.WRK_BOY_DATA> data = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.WRK_BOY_DATA, true);
            data = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.TYPE == "YTD" && m.BRAND<9000 &&m.MARKET<9000).Select(m => m).ToList();
            List<StrawmanDBLibray.Entities.USERS_PERMISSIONS> permissions = (List<StrawmanDBLibray.Entities.USERS_PERMISSIONS>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.USERS_PERMISSIONS,false);
            List<StrawmanDBLibray.Entities.USERS_ROLES> roles = (List<StrawmanDBLibray.Entities.USERS_ROLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.USERS_ROLES, true);
            model.ViewAccess = data.Select(m => new Models.UsersViewAccess
            {
                ID = (long)m.ID,
                CHANNEL = m.CHANNEL,
                channel_name = Helpers.StrawmanConstants.getChannelById((int)m.CHANNEL),
                BRAND = m.BRAND,
                MARKET = m.MARKET,
                NAME = m.NTS_NAME + " / " + m.MARKET_NAME + " / " + m.BRAND_NAME,
                isChecked = permissions != null && permissions.Where(n=>n.USER ==roles.Where(r=>r.USER == _user).FirstOrDefault().ID).ToList()
                                                                .Exists(s=>s.BRAND == m.BRAND && s.MARKET == m.MARKET && s.CHANNEL == m.CHANNEL)
            }).ToList();
            return PartialView(PERMISSIONS, model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateMenuAccess(FormCollection form)
        {
            int ret = 0;
            string status = "fail";
            bool success = false;
            FormCollection this_form = form;
            List<StrawmanDBLibray.Entities.MENU_MASTER> menu_master = (List<StrawmanDBLibray.Entities.MENU_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.MENU_MASTER, true);
            List<StrawmanDBLibray.Entities.MENU_CONFIG> menu_config = (List<StrawmanDBLibray.Entities.MENU_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.MENU_CONFIG, true);
            //Hay que comprobar si es el primer registro del menú, si no hay que añadir la opción 0 al configurador.
            StrawmanDBLibray.Entities.MENU_CONFIG menu = menu_config.Where(m => m.MENU_ID == 0).FirstOrDefault();
            if (menu != null)
            {
                menu = new StrawmanDBLibray.Entities.MENU_CONFIG();
                menu.MENU_ID = 0;
                menu.PERMISSION = "RW";
                menu.USER = this_form["user"];

                int ret_menu = StrawmanDBLibray.DBLibrary.SaveData(StrawmanDataTables.MENU_CONFIG, menu);
            }
            foreach (StrawmanDBLibray.Entities.MENU_MASTER item in menu_master.Where(m => m.LEVEL > 0).Select(m => m).ToList())
            {
                if (this_form["o.isChecked_" + item.ID] != null)
                {
                    StrawmanDBLibray.Entities.MENU_CONFIG cfg = null;
                    if (bool.Parse(this_form["o.isChecked_" + item.ID]))
                    {
                        string permission = this_form["o.Config.PERMISSION_" + item.ID];
                        cfg = new StrawmanDBLibray.Entities.MENU_CONFIG { MENU_ID = item.ID, USER = this_form["user"], PERMISSION = permission };
                        ret = StrawmanDBLibray.DBLibrary.SaveData(StrawmanDataTables.MENU_CONFIG, cfg);
                    }
                    else
                    {
                        cfg = menu_config.Find(m => m.MENU_ID == item.ID);
                        if (cfg!=null) ret = StrawmanDBLibray.DBLibrary.DeleteData(StrawmanDataTables.MENU_CONFIG, cfg);
                    }
                }
            }
            if (ret >= 0)
            {
                success = true;
                status = "success";
            }
            return new JsonResult {
                Data = new { Status = status, Sucess = success },
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateViewAccess(FormCollection form)
        {
            int ret = 0;
            string status = "fail";
            bool success = false;
            FormCollection this_form = form;

            string table = this_form["ViewTable"];
            switch (table)
            {
                case "BOY":
                    List<StrawmanDBLibray.Entities.WRK_BOY_DATA> data = (List<StrawmanDBLibray.Entities.WRK_BOY_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.WRK_BOY_DATA, true);
                    data = data.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.TYPE == "YTD").Select(m => m).ToList();
                    foreach (StrawmanDBLibray.Entities.WRK_BOY_DATA item in data)
                    {
                        List<KeyValuePair<string,string>> key_values = new List<KeyValuePair<string,string>>();
                        string identity = this_form["o.isChecked_" + item.ID];
                        StrawmanDBLibray.Entities.USERS_PERMISSIONS cfg = new StrawmanDBLibray.Entities.USERS_PERMISSIONS();
                        List<StrawmanDBLibray.Entities.USERS_ROLES> users = (List<StrawmanDBLibray.Entities.USERS_ROLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.USERS_ROLES, true);
                        StrawmanDBLibray.Entities.USERS_ROLES user = users.Find(u => u.USER == this_form["user"]);
                        if (user == null) user = new StrawmanDBLibray.Entities.USERS_ROLES { USER = this_form["user"], ROLE = 3 };
                        if (identity != null)
                        {
                            string[] values = identity.Split('_');
                            foreach (string value in values)
                            {
                                key_values.Add(new KeyValuePair<string, string>(value.Split(':')[0], value.Split(':')[1]));
                            }
                            cfg = new StrawmanDBLibray.Entities.USERS_PERMISSIONS
                            {
                                MARKET = decimal.Parse(key_values.Find(m => m.Key == "MARKET").Value),
                                BRAND = decimal.Parse(key_values.Find(m => m.Key == "BRAND").Value),
                                CHANNEL = decimal.Parse(key_values.Find(m => m.Key == "CHANNEL").Value),
                                VIEW = Helpers.StrawmanViews.BOY.id,
                                USERS_ROLES = user
                            };
                        }
                        else
                        {
                            cfg = new StrawmanDBLibray.Entities.USERS_PERMISSIONS
                            {
                                MARKET = item.MARKET,
                                BRAND = item.BRAND,
                                CHANNEL = item.CHANNEL,
                                VIEW = Helpers.StrawmanViews.BOY.id,
                                USERS_ROLES = user
                            };
                        }
                        if (this_form["o.isChecked_" + item.ID] != null)
                        {
                            //Comprobar si existe y si no añadirlo
                            ret = StrawmanDBLibray.DBLibrary.SaveData(StrawmanDataTables.USERS_PERMISSIONS, cfg);
                        }
                        else
                        {
                            //Comprobar si existe y si no borrarlo
                            ret = StrawmanDBLibray.DBLibrary.DeleteData(StrawmanDataTables.USERS_PERMISSIONS, cfg);
                        }
                        
                    }
                    break;
                case "MANAGEMENT_LETTERS":
                    break;
                case "MONTHLY_COMMENTS":
                    break;
            }

            if (ret >= 0)
            {
                success = true;
                status = "success";
            }
            return new JsonResult
            {
                Data = new { Status = status, Sucess = success },
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        private List<SelectListItem> GetViewsList()
        {
            List<SelectListItem> views = new List<SelectListItem>();
            views.Add(new SelectListItem { Value = Helpers.StrawmanViews.BOY.id, Text = Helpers.StrawmanViews.BOY.name, Selected = true });
            views.Add(new SelectListItem { Value = Helpers.StrawmanViews.MANAGEMENT_LETTERS.id, Text = Helpers.StrawmanViews.MANAGEMENT_LETTERS.name });
            views.Add(new SelectListItem { Value = Helpers.StrawmanViews.MONTHLY_COMMENTS.id, Text = Helpers.StrawmanViews.MONTHLY_COMMENTS.name });
            return views;
        }

        private List<Models.BoyMassMarketModels> GetBoyMasterData(string channel)
        {
            Controllers.BoyMassMarketController cont = new Controllers.BoyMassMarketController();
            List<Models.BoyMassMarketModels> list = cont.GetMasterData(channel);
            cont.Dispose();
            return list;
        }

        #endregion

        //Región exclusiva para modificar la vista de _Scripts.cshtml
        #region Scripts
        [Authorize]
        public ActionResult CheckPermissions(string view)
        {
            string _partialView = Helpers.StrawmanViews.Scripts.voidScript;
            if (!Helpers.UserUtils.Permissions.GetPermissions())
            {
                if (!Helpers.UserUtils.Permissions.GetPermissionsFor(view)) view = null;
            }
            switch (view)
            {
                case Helpers.StrawmanViews.BOY.id:
                    _partialView = Helpers.StrawmanViews.BOY.Scripts.forms;
                    break;
                case Helpers.StrawmanViews.BOYBYCHANNEL.id:
                    _partialView = Helpers.StrawmanViews.BOYBYCHANNEL.Scripts.forms;
                    break;
                case Helpers.StrawmanViews.MONTHLY_COMMENTS.id:
                    _partialView = Helpers.StrawmanViews.MONTHLY_COMMENTS.Scripts.forms;
                    break;
                case Helpers.StrawmanViews.MANAGEMENT_LETTERS.id:
                    _partialView = Helpers.StrawmanViews.MANAGEMENT_LETTERS.Scripts.forms;
                    break;

            }
            return PartialView(_partialView);
        }
        [Authorize]
        public ActionResult GetPermissionsByView(string _view, string _row, string _channel)
        {
            switch (_view)
            {
                case Helpers.StrawmanViews.BOY.id:
                    List<Models.BoyMassMarketModels> data = GetBoyMasterData(_channel);
                    int _market = (int)data[int.Parse(_row)].market;
                    int _brand = (int)data[int.Parse(_row)].brand;
                    int _chann = Helpers.StrawmanConstants.getChannel(_channel);
                    if (Helpers.UserUtils.Permissions.GetPermissions()) return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                    if (Helpers.UserUtils.Permissions.GetPermissionsFor(_view))
                    {
                        return Json(new { status = Helpers.UserUtils.Permissions.GetPermissionsByData(_view, _market, _brand, _chann) }, JsonRequestBehavior.AllowGet);
                    }
                    break;
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region BOYForms
        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetBoyEdit(string _type, string _row, string _channel)
        {
            string _view = Helpers.StrawmanViews.BOY.id;
            List<Models.BoyMassMarketModels> data = GetBoyMasterData(_channel);
            int _market = (int)data[int.Parse(_row)].market;
            int _brand = (int)data[int.Parse(_row)].brand;
            int _chann = Helpers.StrawmanConstants.getChannel(_channel);
            if (Helpers.UserUtils.Permissions.GetPermissions()) return new FormsController().GetBoyForm(_market, _brand, _chann, Classes.BOYTypes.GetTypeByFormName(_type));
            if ( Helpers.UserUtils.Permissions.GetPermissionsByData(_view, _market, _brand, _chann))
            {
                return new FormsController().GetBoyForm(_market, _brand, _chann, Classes.BOYTypes.GetTypeByFormName(_type));
            }
            return null;
        }
        [ChildActionOnly]
        public ActionResult GetPartialBOY(string partial, Models.BoyMassMarketModels bmodel)
        {
            object model = null;
            List<Models.BoyMassMarketModels> lmodel = new List<Models.BoyMassMarketModels>();
            lmodel.Add(bmodel);
            model = lmodel;
            switch (partial) { 
                case Classes.StrawmanViews.Partials.BOY.TOGO:
                    ViewBag.BoyYTD = lmodel;
                    break;
                case Classes.StrawmanViews.Partials.BOY.INT:
                case Classes.StrawmanViews.Partials.BOY.LE:
                case Classes.StrawmanViews.Partials.BOY.PBP:
                    ViewBag.BoyCALC = lmodel;
                    break;
            }
            partial = partial + ".cshtml";
            ViewBag.YearPeriod = StrawmanApp.Helpers.PeriodUtil.Year;
            return PartialView(BOY_PATH + partial, model);
        }
        #endregion

        #region BOYConfig
        public ActionResult BOYConfig()
        {
            ViewBag.MenuUrl = BOY_CONFIG;
            ViewBag.TabUrl = CONTROLLER_NAME + "/" + BOY_CONFIG;
            List<StrawmanDBLibray.Entities.BOY_CONFIG> list = StrawmanDBLibray.Repository.BOY_CONFIG.getAll();
            List<SelectListItem> channels = GetChannelList(Helpers.Channels.MASS);
            Models.GenericConfigModel model = new Models.GenericConfigModel();
            model.path = BOY_CONFIG_PATH;
            foreach (int channel in channels.Select(m => int.Parse(m.Value)).Distinct())
            {
                model.tables.Add(new Models.TableConfig
                {
                    id = "boy_config",
                    content = list.Where(m => m.CHANNEL == channel).Select(m => new Models.MasterDataModels
                    {
                        channel = m.CHANNEL,
                        channel_name = channels.Find(s => decimal.Parse(s.Value) == m.CHANNEL).Text,
                        id = m.ID,
                        brand_name = StrawmanDBLibray.Repository.BRAND_MASTER.getById((int)m.BRAND).NAME ?? Helpers.MessageByLanguage.NotFound,
                        market_name = StrawmanDBLibray.Repository.MARKET_MASTER.getById((int)m.MARKET).NAME ?? Helpers.MessageByLanguage.NotFound,
                        nts_name = m.NTS_NAME,
                        order = m.NTS_ORDER,
                        market_calc_name = StrawmanDBLibray.Repository.BOY_CONFIG.GetCalcStatus(m.MARKET_CONFIG, StrawmanDBLibray.Repository.BOY_CONFIG.Columns.MARKET_CONFIG),
                        brand_calc_name = StrawmanDBLibray.Repository.BOY_CONFIG.GetCalcStatus(m.SELLOUT_CONFIG, StrawmanDBLibray.Repository.BOY_CONFIG.Columns.SELLOUT_CONFIG),
                        nts_calc_name = StrawmanDBLibray.Repository.BOY_CONFIG.GetCalcStatus(m.SELLIN_CONFIG, StrawmanDBLibray.Repository.BOY_CONFIG.Columns.SELLIN_CONFIG),
                        config_name = StrawmanDBLibray.Repository.BOY_CONFIG.GetCalcStatus(m.CONSOLIDATE, StrawmanDBLibray.Repository.BOY_CONFIG.Columns.CONSOLIDATE),
                    }).ToList(),
                    view = BOY_CONFIG_TABLE_VIEW
                });
            }
            model.navigator = new Models.Navigator{ content = channels, view = CHANNEL_PILLS_VIEW };
            return View(BOY_CONFIG_VIEW, model);
        }
        #endregion

        #region Save Forms Backend
        public ActionResult Save(FormCollection collection)
        {
            string status = "success";
            bool status_ok = true;
            int ret = 0;
            object obj = null;
            string message = SUCCESS_MESSAGE;
            string field = "";
            //Localizar el tipo de actualización.
            string type = collection[Classes.Default.Attributes.ALL.INPUT_TYPE_ID];
            //Localizar la tabla a actualzar
            string table = collection[Classes.Default.Attributes.ALL.INPUT_TABLE_ID];
            //Canal
            decimal? channel = collection[Classes.Default.Attributes.ALL.INPUT_CHANNEL_ID] == null? null: (decimal?)decimal.Parse(collection[Classes.Default.Attributes.ALL.INPUT_CHANNEL_ID]);
            //Comprobar rol admin
            if (Helpers.UserUtils.Permissions.GetPermissions())
            {
                //Enviar los datos a la tabla via StrawmanDBLibrary 
                try
                {
                    switch (type)
                    {
                        case Classes.ActionsNames.MASTER_DATA:
                            //Comprobar que los datos no vienen en blanco
                            if (collection[Classes.Default.Attributes.ALL.MARKET_NAME_ID].Length == 0 && int.Parse(collection[Classes.Default.Attributes.ALL.MARKET_ID]) == 0)
                            {
                                status_ok = false;
                                field = Classes.Default.Attributes.MARKET_NAME_ID;
                            }
                            if (collection[Classes.Default.Attributes.ALL.BRAND_NAME_ID].Length == 0)
                            {
                                status_ok = false;
                                field = Classes.Default.Attributes.BRAND_NAME_ID;
                            }
                            //Reescribimos el canal (por defecto en hidden, pero viene en select)
                            channel = (decimal?)decimal.Parse(collection[Classes.Default.Attributes.ALL.CHANNEL_ID]);
                            int market = int.Parse(collection[Classes.Default.Attributes.ALL.MARKET_ID]);
                            int? _brand = String.IsNullOrEmpty(collection[Classes.Default.Attributes.ALL.BRAND_ID]) ? null : (int?)int.Parse(collection[Classes.Default.Attributes.ALL.BRAND_ID]);
                            StrawmanDBLibray.Entities.BRAND_MASTER brand = new StrawmanDBLibray.Entities.BRAND_MASTER
                            {
                                NAME = collection[Classes.Default.Attributes.ALL.BRAND_NAME_ID],
                                GROUP = decimal.Parse(collection[Classes.Default.Attributes.ALL.GROUP_ID]),
                                CHANNEL = channel,
                                ID = _brand == null?0:(decimal)_brand
                            };
                            System.Data.Objects.DataClasses.EntityCollection<StrawmanDBLibray.Entities.BRAND_MASTER> lbrand = new System.Data.Objects.DataClasses.EntityCollection<StrawmanDBLibray.Entities.BRAND_MASTER>();
                            lbrand.Add(brand);
                            obj = new StrawmanDBLibray.Entities.MARKET_MASTER
                            {
                                GROUP = decimal.Parse(collection[Classes.Default.Attributes.ALL.GROUP_ID]),
                                NAME = collection[Classes.Default.Attributes.ALL.MARKET_NAME_ID],
                                CHANNEL = channel,
                                BRAND_MASTER = lbrand,
                                ID = market
                            };
                            break;
                    }
                    if (status_ok && obj != null)
                    {
                        ret = StrawmanDBLibray.DBLibrary.SaveData(table, obj);
                    }
                    else
                    {
                        status = "fail";
                        message = Helpers.MessageByLanguage.FieldRequired;
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            
            return Json(new { status = status, message = message, field = field}, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Generic Functions
         
        #endregion

        #region Default functions
        //
        // GET: /Config/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Config/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Config/Create

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
        // GET: /Config/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Config/Edit/5

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
        // GET: /Config/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Config/Delete/5

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
        #endregion

        #region Constants
        public string CONTROLLER_NAME = "Config";

        private int channel = 0;
        private const string PERIOD_YEAR_TEXT = "Año:";
        private const string PERIOD_MONTH_TEXT = "Mes:";
        private const string MENU_URL = "StrawmanApp";
        private const string MESSAGE = "Seleccione el periodo a establecer.";
        private const string SUBMIT_TEXT = "Guardar";
        private const string SUCCESS_MESSAGE = "Actualización realizada.";
        private const string FAIL_MESSAGE = "Se ha producido un error.";

        private const string _PATH = "~/Views/",
                             CONTROLLER ="Config",
                             COMPONENTS = "Components";
        
        private const string MASTER_DATA_VIEW = _PATH + CONTROLLER + "/MasterData.cshtml";
        private const string MASTER_DATA_GRID = _PATH + CONTROLLER + "/_MasterDataGrid.cshtml";
        private const string MASTER_DATA_EDIT_ROW = _PATH + CONTROLLER + "/_MasterDataEditRow.cshtml";
        private const string DROP_DOWN_LIST = _PATH + CONTROLLER + "/Components/_DropDownList.cshtml";
        private const string DROP_DOWN_LIST_FOR = _PATH + CONTROLLER + "/Components/_DropDownListFor.cshtml";
        private const string PERMISSIONS = _PATH + CONTROLLER + "/Users/_Permissions.cshtml";
        private const string USERS_CONFIG = _PATH + CONTROLLER + "/Users/Index.cshtml";

        private const string PREVIEW_CONTROLLER = "Preview";
        private const string PREVIEW_PATH = _PATH + CONTROLLER + "/" + PREVIEW_CONTROLLER;
        private const string PREVIEW_DATA_VIEW = PREVIEW_PATH + "/Index.cshtml";
        private const string PREVIEW_DATA_GRID = PREVIEW_PATH + "/_DataGrid.cshtml";
        private const string PREVIEW_DATA_GRID_ROW = PREVIEW_PATH + "/_DataEditRow.cshtml";

        private const string GROUPS_CONFIGURE = CONTROLLER + "/GroupsConfigure";
        private const string GROUPS_CONFIGURE_PATH = _PATH + "Forms/GroupsConfigure.cshtml";
        private const string BOOTSTRAP_TABS = _PATH + "Tabs/_BootstrapTabs.cshtml";
        private const string GROUP_MASTER = _PATH + CONTROLLER + "/Groups/_GroupMaster.cshtml";
        private const string GROUP_MASTER_EDIT = _PATH + CONTROLLER + "/Groups/_GroupMasterEdit.cshtml";
        private const string GROUP_CONFIG = _PATH + CONTROLLER + "/Groups/_GroupConfig.cshtml";
        private const string GROUP_CONFIG_EDIT = _PATH + CONTROLLER + "/Groups/_GroupConfigEdit.cshtml";

        private const string MARKET = "MARKET";
        private const string BRAND = "BRAND";

        private const string SHARED_PERIOD_SELECTOR = _PATH + "/Shared/_PeriodSelector.cshtml";
        private const string PERIOD_BANNER = _PATH + "/Shared/_PeriodBanner.cshtml";

        private const string DOLAR_CHANGE = "DOLAR_CHANGE";
        private const string CURRENCY_BANNER = _PATH + "/Shared/_CurrencyBanner.cshtml";
        private const string CURRENCY_ADJUST = _PATH + "/Shared/_CurrencyAdjust.cshtml";

        private const string LOAD_CONFIG = _PATH + CONTROLLER + "/_LoadConfig.cshtml";

        private const string FORMS_CONTROLLER = "Forms";
        private const string LOADER_PREVIEW = _PATH + FORMS_CONTROLLER + "/Tables/_LoaderPreview.cshtml";
        private const string TD_VIEW = _PATH + FORMS_CONTROLLER + "/Tables/Components/_TdView.cshtml";
        private const string TR_VIEW = _PATH + FORMS_CONTROLLER + "/Tables/Components/_TrView.cshtml";

        private const string BOY_CONTROLLER = "BoyMassMarket";
        private const string BOY_PATH = _PATH + BOY_CONTROLLER + "/";

        private const string BOY_CONFIG = "BOYConfig",
                             BOY_CONFIG_PATH = _PATH + CONTROLLER + "/" + BOY_CONFIG + "/",
                             BOY_CONFIG_TABLE_VIEW = BOY_CONFIG_PATH + "_DataGrid.cshtml",
                             BOY_CONFIG_VIEW = BOY_CONFIG_PATH + "Index.cshtml",
                             CHANNEL_PILLS_VIEW = _PATH + CONTROLLER + "/" + COMPONENTS + "/_ChannelPills.cshtml",
                             USER_CONFIGURE = "UserManagment",
                             GET_MASTER_DATA = "GetMasterData";

        #endregion
    }
}
