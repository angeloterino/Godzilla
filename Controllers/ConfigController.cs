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
        #region LoadConfig
        public ActionResult LoadConfig()
        {
            //TODO: 
            //Obtener los datos a mostrar en el formulario
            //Datos maestros:
            List<StrawmanDBLibray.Classes.ExcelLoader> lst = (List<StrawmanDBLibray.Classes.ExcelLoader>)TempData.Peek("LoadNTS");
            List<StrawmanDBLibray.Entities.BRAND_MASTER> bmster = (List<StrawmanDBLibray.Entities.BRAND_MASTER>)StrawmanDBLibray.Repository.BRAND_MASTER.getAll();
            List<StrawmanDBLibray.Entities.MARKET_MASTER> mmster = (List<StrawmanDBLibray.Entities.MARKET_MASTER>)StrawmanDBLibray.Repository.MARKET_MASTER.getAll();
            List<StrawmanDBLibray.Entities.CHANNEL_MASTER> cmster = (List<StrawmanDBLibray.Entities.CHANNEL_MASTER>)StrawmanDBLibray.Repository.CHANNEL_MASTER.getAll();
            var preview_data = mmster.Join(bmster, m => new { _market = m.ID, _group = m.GROUP }, b => new { _market = (decimal)b.MARKET, _group = b.GROUP }, (m, b) => new { market = m, brand = b }).AsEnumerable();
            decimal tmp = -1;
            dynamic model = null;
            string partial = LOAD_CONFIG;
            int fileType = int.Parse(TempData.Peek("fileType").ToString());
            Models.ConfigModels cm = new Models.ConfigModels();
            switch (fileType)
            {
                case 1:
                    List<StrawmanDBLibray.Entities.NTS_MASTER> nmster = (List<StrawmanDBLibray.Entities.NTS_MASTER>)StrawmanDBLibray.Repository.NTS_MASTER.getAll();
                    var data = preview_data.Select(m => new Models.ItemsConfigModel
                    {
                        channel = m.brand.CHANNEL.ToString(),
                        market = m.market.ID.ToString(),
                        brand = m.brand.ID.ToString(),
                        brand_description = m.brand.NAME,
                        market_description = m.market.NAME,
                        channel_description = cmster.Where(n=>n.ID == m.brand.CHANNEL).Select(s=>s.NAME).FirstOrDefault(),
                        list_data = Helpers.FormControlsUtil.SelectAddBlank(lst.Select(n => new { MARKET_NAME = n.col13}).Distinct().AsEnumerable()
                        .Select(n => new SelectListItem
                        {
                            Value = n.MARKET_NAME,
                            Text = n.MARKET_NAME,
                            Selected = nmster.Exists(s=> s.MARKET_NAME == n.MARKET_NAME && m.market.ID.ToString() == s.MARKET && m.brand.ID.ToString() == s.BRAND)
                        }).ToList()),
                        nts_data = nmster.Select(n => new Models.NTS_Data
                        {
                            brand = decimal.TryParse(n.BRAND, out tmp) ? tmp : default(decimal?),
                            channel = decimal.TryParse(n.CHANNEL, out tmp) ? tmp : default(decimal?),
                            market = decimal.TryParse(n.MARKET, out tmp) ? tmp : default(decimal?),
                            id = n.ID,
                            market_name = n.MARKET_NAME
                        }).ToList(),
                        nts_name = nmster.Where(n => n.MARKET == m.market.ID.ToString() && n.BRAND == m.brand.ID.ToString()).Select(s => s.MARKET_NAME).FirstOrDefault(),
                    });
                    model = data.ToList();
                    partial = LOAD_CONFIG_NTS;
                break;
                default:
                    List<StrawmanDBLibray.Entities.ROSETTA_LOADER> rmster = (List<StrawmanDBLibray.Entities.ROSETTA_LOADER>)StrawmanDBLibray.Repository.ROSETTA_LOADER.getAll();
                    var m_union = mmster.Select(m => new Models.MarketDataModels { market = m.ID, brand = (decimal)0, channel = (decimal)m.CHANNEL, market_name = m.NAME }).ToList();
                    var b_union = bmster.Select(m => new Models.MarketDataModels { market = (decimal)m.MARKET, brand = m.ID, channel = (decimal)m.CHANNEL, market_name = m.NAME }).ToList();
                    var umster = m_union.Union(b_union).AsEnumerable().OrderBy(m => m.channel).ThenBy(m => m.market).ThenBy(m => m.brand).ToList();
                    //var umster = emster.OrderBy(m => new { m.channel, m.market, m.brand }).ToList();
                    var ddata = umster.Select(m => new Models.ItemsConfigModel 
                    { 
                        channel = m.channel.ToString(),
                        market = m.market.ToString(),
                        brand = m.brand.ToString(),
                        channel_description = cmster.Find(n=> n.ID == m.channel).NAME,
                        brand_description = bmster.Exists(n=>n.ID == m.brand)?bmster.Find(n=>n.ID == m.brand).NAME:null,
                        market_description = mmster.Find(n=>n.ID == m.market).NAME,
                        type = !bmster.Exists(n=>n.ID == m.brand)?"MARKET":"BRAND",
                        list_data = Helpers.FormControlsUtil.SelectAddBlank(lst.Where(s=> lst.IndexOf(s) > 0).Select(n => new SelectListItem
                        {
                            Value = (lst.IndexOf(n)).ToString(),
                            Text = (lst.IndexOf(n)).ToString() + " - " + n.col1 + " - " + n.col2 + " - " + n.col3 + " - " + n.col4,
                            Selected = rmster.Exists(s=> s.EXCEL_ROW == lst.IndexOf(n) + 1 && m.market == s.MARKET_ID && ((m.brand == 0 && s.BRAND_ID == null) || m.brand == s.BRAND_ID))
                        }).ToList()),
                        nielsen_data = rmster.Select(n=> new Models.Nielsen_Data{
                             brand = n.BRAND_ID,
                             market = n.MARKET_ID,
                             brand_description = n.BRAND,
                             market_description = n.DESCRIPTION,
                             id = n.ID,
                             excel_row = n.EXCEL_ROW
                        }).ToList(),
                        excel_row = rmster.Where(n => n.BRAND_ID == m.brand && n.MARKET_ID == m.market).Select(s => s.EXCEL_ROW).FirstOrDefault(),
                    });
                    model = ddata.ToList();
                    partial = LOAD_CONFIG_NIELSEN;
                break;
            }
            //Columnas del excel:
            //Datos configurados:
            //  NTS:
            //  IMS/Nielsen: 
            return PartialView(partial, model);
        }
        [Authorize]
        public ActionResult SaveConfigItem(string market, string brand, string channel, string source, string value)
        {
            List<Models.MasterConfig> mst = new List<Models.MasterConfig>(); 
            if (Helpers.Session.GetSession("MODELS_MASTER_CONFIG") != null)
            {
                mst = (List<Models.MasterConfig>)Helpers.Session.GetSession(MODELS_MASTER_CONFIG);
            }
            if (mst.Exists(m => m.market == market && m.brand == brand && m.channel == channel && m.source == source))
                mst.Find(m => m.market == market && m.brand == brand && m.channel == channel && m.source == source).value = value;
            else
                mst.Add(new Models.MasterConfig { market = market, brand = brand, channel = channel, source = source, value = value });
            Helpers.Session.SetSession(MODELS_MASTER_CONFIG, mst);
            return Json(new { Success = true,status = "success" }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult SaveConfigData()
        {
            int ret = -1;
            List<Models.MasterConfig> items = (List<Models.MasterConfig>)Helpers.Session.GetSession(MODELS_MASTER_CONFIG);
            List<StrawmanDBLibray.Classes.ExcelLoader> lst = (List<StrawmanDBLibray.Classes.ExcelLoader>)TempData.Peek("LoadNTS");
            if(Helpers.UserUtils.Permissions.GetPermissions()){
                foreach (Models.MasterConfig item in items)
                {
                    switch (item.source)
                    {
                        case "NTS":
                            List<StrawmanDBLibray.Classes.ExcelLoader> litems = lst.Where(m => m.col13 == item.value).Select(m => m).ToList();
                            StrawmanDBLibray.Entities.NTS_MASTER nmst = new StrawmanDBLibray.Entities.NTS_MASTER
                            {
                                MARKET_NAME = item.value,
                                BRAND = item.brand,
                                MARKET = item.market,
                                CHANNEL = item.channel,
                                Mat_Local_Class_2 = "",
                                Mat_Local_Class_3 = "",
                                Mat_Local_Class_4 = "",
                                STRAWMAN_CHECK = item.value,
                            };
                            foreach (StrawmanDBLibray.Classes.ExcelLoader litem in litems)
                            {
                                nmst.Mat_Local_Class_2 = litem.col3;
                                nmst.Mat_Local_Class_3 = litem.col4;
                                nmst.Mat_Local_Class_4 = litem.col5;
                                if (item.value != null)
                                {
                                    ret = StrawmanDBLibray.Repository.NTS_MASTER.SaveItem(nmst);
                                }
                                else
                                {
                                    ret = StrawmanDBLibray.Repository.NTS_MASTER.DeleteItem(nmst);
                                }
                            }
                            break;
                        default:
                            int _index = 0;
                            if (int.TryParse(item.value, out _index))
                            {
                                StrawmanDBLibray.Classes.ExcelLoader ritem = lst[_index];
                                StrawmanDBLibray.Entities.ROSETTA_LOADER ros = new StrawmanDBLibray.Entities.ROSETTA_LOADER
                                {
                                    MARKET_ID = decimal.Parse(item.market),
                                    BRAND_ID = decimal.Parse(item.brand),
                                    CHANNEL_ID = decimal.Parse(item.channel),
                                    EXCEL_ROW = _index
                                };
                                if (_index > 0)
                                {
                                    ret = StrawmanDBLibray.Repository.ROSETTA_LOADER.SaveItem(ros);
                                }
                                else
                                {
                                    ret = StrawmanDBLibray.Repository.ROSETTA_LOADER.DeleteItem(ros);
                                }
                            }
                            break;
                    }
                }
            }
            return Json(new { Success = ret>0,status = ret > 0 ? "success" : "fail" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
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
                                            source = p.SOURCE,
                                        }).ToList();
            //Devolver vista parcial de Grid con opciones de edición
            ViewBag.Group = group ?? 1;
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
        public ActionResult AddItemConfig(string _group, string _source)
        {
            List<SelectListItem> sources = Models.ConfigOperationsModel.GetDefaultSourceList(_source);
            //    new List<SelectListItem>();
            //sources.Add(new SelectListItem
            //{
            //    Text = "Default",
            //    Value ="",
            //    Selected = string.IsNullOrEmpty(_source)
            //});
            //sources.Add(new SelectListItem
            //{
            //    Text ="Channel",
            //    Value ="CHANNEL",
            //    Selected = !string.IsNullOrEmpty(_source) && _source == "CHANNEL"
            //});
            //sources.Add(new SelectListItem
            //{
            //    Text ="Franchise",
            //    Value ="FRANCHISE",
            //    Selected = !string.IsNullOrEmpty(_source) && _source == "FRANCHISE"
            //});
            //sources.Add(new SelectListItem
            //{
            //    Text = "Keybrands",
            //    Value ="KEYBRANDS",
            //    Selected = !string.IsNullOrEmpty(_source) && _source == "KEYBRANDS"
            //});
            string[] _channels_names = new string[]{ "mass", "beauty", "otc" };
            if (!string.IsNullOrEmpty(_source)) { _channels_names = new string[] {"default"}; }
            ViewDataDictionary vd = new ViewDataDictionary
            {
                {"group", _group},
                {"source",_source},
                {"channels", new string[]{Helpers.Channels.MASS.ToString(),Helpers.Channels.BEAUTY.ToString(),Helpers.Channels.OTC.ToString()}},
                {"channels_names", _channels_names},
                {"partial","GetGroupItemsByChannel"},
                {"action", "SaveItemsConfig"},
                {"controller","Config"},
                {"source_select", sources}
            };
            ViewBag.ModalTitle = Helpers.MessageByLanguage.AddItem;
            ViewBag.ButtonText = Helpers.MessageByLanguage.Save;
            return PartialView(GROUP_MODAL_ITEMS, vd);
        }
        public ActionResult GetGroupItemsByChannel(string _group, string _channel, string _source)
        {
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> cfg = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.GROUP_CONFIG);
            //List<StrawmanDBLibray.Entities.BRAND_MASTER> bmster = (List<StrawmanDBLibray.Entities.BRAND_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.BRAND_MASTER);
            //List<StrawmanDBLibray.Entities.MARKET_MASTER> mmster = (List<StrawmanDBLibray.Entities.MARKET_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.MARKET_MASTER);
            List<StrawmanDBLibray.Entities.MARKET_GROUPS> groups = (List<StrawmanDBLibray.Entities.MARKET_GROUPS>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.MARKET_GROUPS);
            _channel = _channel.Contains(" ")?_channel.Substring(0, _channel.IndexOf(" ")): _channel;
            int channel = (int.TryParse(_channel, out channel) ? channel : Helpers.StrawmanConstants.getChannel(_channel.ToUpper()));
            var cache = Helpers.Session.GetSession(GROUP_CFG_DATA);
            var data = cfg.Where(m => m.GROUP_ID == int.Parse(_group)).Select(m => m).ToList();
            if (cache != null)
            {
                var tmp = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)cache;
                if (tmp.Exists(m => m.GROUP_ID == int.Parse(_group)))
                    data = tmp;
            }
            else
            {
                Helpers.Session.SetSession(GROUP_CFG_DATA, data);
            }
            object ms = null;
            switch (_source)
            {
                case "CHANNEL":
                    List<Models.MarketViewChannelModels> cdata = (List<Models.MarketViewChannelModels>)new MarketViewChannelController().GetDataViewChannel();
                    ms = cdata
                        //bmster.Join(mmster, b => new { _market = (decimal)b.MARKET }, m => new { _market = m.ID }, (b, m) => new { b = b, m = m }).AsEnumerable()
                    .Select(d => new Models.ItemsConfigModel
                    {
                        selected = data.Exists(e => e.MARKET == d.vid && e.BRAND == d.vid),
                        market = d.vid.ToString(),
                        brand = d.vid.ToString(),
                        channel = d.vchannel.ToString(),
                        market_description = d.name,
                        brand_description = d.name,
                        group_id = _group,
                    }).ToList();
                    break;
                case "FRANCHISE":
                    List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_DATA> fdata = (List<StrawmanDBLibray.Entities.v_WRK_FRANCHISE_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.v_WRK_FRANCHISE_DATA);
                    ms = fdata
                        //bmster.Join(mmster, b => new { _market = (decimal)b.MARKET }, m => new { _market = m.ID }, (b, m) => new { b = b, m = m }).AsEnumerable()
                    .Select(d => new Models.ItemsConfigModel
                    {
                        selected = data.Exists(e => e.MARKET == d.ID && e.BRAND == d.ID),
                        brand = d.ID.ToString(),
                        market = d.ID.ToString(),
                        market_description = d.NAME,
                        group_id = _group,
                    }).ToList();
                    break;
                case "KEYBRANDS":
                    List<Models.MarketViewChannelModels> kdata = new MarketViewKeybrandsController().GetKeybrandsMasterData();
                    //List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER> kdata = (List<StrawmanDBLibray.Entities.v_KEYBRANDS_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.v_KEYBRANDS_MASTER);
                    ms = kdata
                        //bmster.Join(mmster, b => new { _market = (decimal)b.MARKET }, m => new { _market = m.ID }, (b, m) => new { b = b, m = m }).AsEnumerable()
                    .Select(d => new Models.ItemsConfigModel
                    {
                        selected = data.Exists(e => e.MARKET == d.vid && e.BRAND == d.vid),
                        brand = d.vid.ToString(),
                        market = d.vid.ToString(),
                        market_description = d.name,
                        group_id = _group,
                    }).ToList();
                    break;
                default:
                    List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> sdata = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.v_STRWM_MARKET_DATA);
                    ms = sdata.Where(m => m.CHANNEL == channel)
                        //bmster.Join(mmster, b => new { _market = (decimal)b.MARKET }, m => new { _market = m.ID }, (b, m) => new { b = b, m = m }).AsEnumerable()
                    .Select(d => new Models.ItemsConfigModel
                    {
                        selected = data.Exists(e => e.MARKET == d.MARKET && e.BRAND == d.BRAND),
                        brand = d.BRAND.ToString(),
                        market = d.MARKET.ToString(),
                        channel = d.CHANNEL.ToString(),
                        market_description = d.NAME,
                        brand_description = d.BRAND_NAME ?? d.NAME,
                        group_id = _group,
                        group_description = groups.Where(g => g.ID == d.GROUP).FirstOrDefault().NAME,
                    }).ToList();
                    break;
            }
            ViewBag.Controller = "GetGroupItemsByChannel";
            ViewBag.FormAction =  "SaveItemsConfig";
            ViewBag.FormController = "Config";
            return PartialView(GROUP_ITEMS, (List<Models.ItemsConfigModel>)ms);
        }
        public ActionResult SetGroupCfg(string _channel, string _market, string _brand, string _group, string _checked)
        {
            var data = Helpers.Session.GetSession(GROUP_CFG_DATA);
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> sdata = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.v_STRWM_MARKET_DATA);
            if (data != null)
            {
                var tmp = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)data;
                if (tmp.Exists(m => m.GROUP_ID == int.Parse(_group) && m.MARKET == int.Parse(_market) && m.BRAND == int.Parse(_brand)))
                {
                    if (!bool.Parse(_checked))
                        tmp.Remove(tmp.Where(m => m.GROUP_ID == int.Parse(_group) && m.MARKET == int.Parse(_market) && m.BRAND == int.Parse(_brand)).FirstOrDefault());
                }
                else
                {
                    if (bool.Parse(_checked))
                        tmp.Add(new StrawmanDBLibray.Entities.GROUP_CONFIG
                        {
                            BRAND = int.Parse(_brand),
                            MARKET = int.Parse (_market),
                            CONFIG = 1,
                            GROUP_ID = int.Parse(_group),
                            TYPE_ID = tmp.FirstOrDefault().TYPE_ID,
                            SOURCE = tmp.FirstOrDefault().SOURCE,
                            BRAND_NAME = sdata.Where(m=>m.CHANNEL == int.Parse(_channel) && m.MARKET == int.Parse(_market) && m.BRAND == int.Parse(_brand)).FirstOrDefault().BRAND_NAME
                        });
                }
                Helpers.Session.SetSession(GROUP_CFG_DATA, tmp);
            }
            return Json(new { success = "success"}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveGroupConfig(FormCollection collection)
        {
            string status = "ok";
            string _source = !String.IsNullOrEmpty(collection["o.source"]) ? collection["o.source"].ToString() : null;
            string _config = !String.IsNullOrEmpty(collection["o.config"]) ? collection["o.config"].ToString() : null;
            string _name = !String.IsNullOrEmpty(collection["o.brand_name"]) ? collection["o.brand_name"].ToString() : null;
            string _id = !String.IsNullOrEmpty(collection["o.id"]) ? collection["o.id"].ToString() : null;
            string _base_id = !String.IsNullOrEmpty(collection["o.base_id"]) ? collection["o.base_id"].ToString() : null;
            string _level = !String.IsNullOrEmpty(collection["o.level"]) ? collection["o.level"].ToString() : null;
            switch (collection["o.type"])
            {
                case StrawmanDataTables.GROUP_CONFIG:
                    if(!String.IsNullOrEmpty(_id))
                    {
                        StrawmanDBLibray.Entities.GROUP_CONFIG cfg = StrawmanDBLibray.Repository.GROUP_CONFIG.getById(int.Parse(_id));
                        string _duplicate = !String.IsNullOrEmpty(collection["o.duplicate"]) ? collection["o.duplicate"].ToString() : null;
                        if (_duplicate == null)
                        {
                            cfg.CONFIG = int.Parse(_config ?? "1");
                            cfg.SOURCE = _source == "None" || _source == "Default" ? null : _source.ToUpper();
                            if (!String.IsNullOrEmpty(_name)) cfg.BRAND_NAME = _name;
                            StrawmanDBLibray.Repository.GROUP_CONFIG.SaveItem(cfg);
                        }
                        else
                        {
                            StrawmanDBLibray.Entities.GROUP_CONFIG tmp = new StrawmanDBLibray.Entities.GROUP_CONFIG()
                            {
                                MARKET = cfg.MARKET,
                                BRAND = cfg.BRAND,
                                CONFIG = cfg.CONFIG,
                                SOURCE = cfg.SOURCE,
                                BRAND_NAME = cfg.BRAND_NAME,
                                GROUP_ID = cfg.GROUP_ID,
                                TYPE_ID = cfg.TYPE_ID
                            };
                            StrawmanDBLibray.Repository.GROUP_CONFIG.SaveItem(tmp);
                        }
                    }
                    break;
                case StrawmanDataTables.GROUP_MASTER:
                    if (!String.IsNullOrEmpty(_id))
                    {
                        StrawmanDBLibray.Entities.GROUP_MASTER mst = StrawmanDBLibray.Repository.GROUP_MASTER.getById(int.Parse(_id));
                        mst.LEVEL = _level!=null?int.Parse(_level): default(int?);
                        mst.BASE_ID = int.Parse(_base_id??"0");
                        mst.NAME = _name;
                        StrawmanDBLibray.Repository.GROUP_MASTER.SaveItem(mst);
                    }
                    break;
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult DeleteGroup(string _id, string _source)
        {
            string status = "ok";
            int id = 0;
            if (int.TryParse(_id, out id))
            {
                switch (_source)
                {
                    case StrawmanDataTables.GROUP_CONFIG:
                        var cache =  (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.Session.GetSession(GROUP_CFG_DATA);
                        if (cache != null && cache.Exists(m => m.ID == id))
                        {
                            cache.Remove(cache.Where(m => m.ID == id).FirstOrDefault());
                            Helpers.Session.SetSession(GROUP_CFG_DATA, cache);
                        }
                        StrawmanDBLibray.Repository.GROUP_CONFIG.DeleteItemById(id);
                        break;
                    case StrawmanDataTables.GROUP_MASTER:
                        StrawmanDBLibray.Repository.GROUP_MASTER.DeleteItemById(id);
                        break;
                }
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
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
                        type = table,
                        brand_name = cfg.BRAND_NAME,
                        id = cfg.ID,
                        config_name = Models.ConfigOperationsModel.GetOppName(cfg.CONFIG),
                        config_list = Models.ConfigOperationsModel.GetOppList(cfg.CONFIG),
                        config = cfg.CONFIG,
                        source = cfg.SOURCE,
                        source_list = Models.ConfigOperationsModel.GetSourceList(cfg.SOURCE, (int?)cfg.TYPE_ID)

                    });
                    return PartialView(GROUP_CONFIG_EDIT, model_g_c);
                case StrawmanDataTables.GROUP_MASTER:
                    StrawmanDBLibray.Entities.GROUP_MASTER cfgm = StrawmanDBLibray.Repository.GROUP_MASTER.getById(_id);
                    List<StrawmanDBLibray.Entities.GROUP_MASTER> mstrs = StrawmanDBLibray.Repository.GROUP_MASTER.getAll().Where(m => m.TYPE == cfgm.TYPE).Select(m => m).ToList();
                    List<Models.MasterDataModels> model_g_m = new List<Models.MasterDataModels>();
                    model_g_m.Add(new Models.MasterDataModels
                    {
                        type = table,
                        brand_name = cfgm.NAME,
                        id = cfgm.ID,
                        base_id = cfgm.BASE_ID,
                        level_list = Models.ConfigOperationsModel.GetOppLevelList(cfgm.LEVEL,mstrs.Where(m=>m.ID != _id).Select(m=>m).ToList()),
                        level = cfgm.LEVEL,
                    });
                    return PartialView(GROUP_MASTER_EDIT, model_g_m);
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
                case StrawmanDBLibray.Classes.StrawmanDataTables.NTS_MASTER:
                    StrawmanDBLibray.Entities.NTS_MASTER nst = StrawmanDBLibray.Repository.NTS_MASTER.getById(_id);
                    ret = nst.MARKET_NAME;
                    break;
                case StrawmanDBLibray.Classes.StrawmanDataTables.ROSETTA_LOADER:
                    StrawmanDBLibray.Entities.ROSETTA_LOADER rst = StrawmanDBLibray.Repository.ROSETTA_LOADER.getById(_id);
                    ret = rst.DESCRIPTION;
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
        public List<SelectListItem> GetStrawmanConfig(int? _channel, int? _market, int? _brand, string type, string column)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            switch (type)
            {
                case Classes.StrawmanViews.MARKET:
                    List<StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG> mcfg = (List<StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG>)StrawmanDBLibray.Repository.CALCS_MARKETS_CONFIG.getAll();
                    StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG mitem = mcfg.Find(m => m.MARKET == _market && m.BRAND == _brand);
                    switch (column)
                    {
                        //GROUP
                        case Classes.Default.Attributes.MARKET_GROUP_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(mitem.GROUPCFG);
                            break;
                        //SUPREGROUP
                        case Classes.Default.Attributes.MARKET_SUPERGROUP_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(mitem.SUPERCFG);
                            break;
                        //CHANNEL
                        case Classes.Default.Attributes.MARKET_CHANNEL_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(mitem.CHANNELCFG);
                            break;
                        //FRANCHISE
                        case Classes.Default.Attributes.MARKET_FRANCHISE_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(mitem.FRANCHISECFG);
                            break;
                        //KEYBRANDS
                        case Classes.Default.Attributes.MARKET_KEYBRANDS_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(mitem.KEYBRANDSCFG);
                            break;
                    }
                    break;
                case Classes.StrawmanViews.BRAND:
                    List<StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG> bcfg = (List<StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG>)StrawmanDBLibray.Repository.CALCS_BRANDS_CONFIG.getAll();
                    StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG bitem = bcfg.Find(m => m.MARKET == _market && m.BRAND == _brand);
                    switch (column)
                    {
                        //GROUP
                        case Classes.Default.Attributes.BRAND_GROUP_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(bitem.GROUPCFG);
                            break;
                        //SUPREGROUP
                        case Classes.Default.Attributes.BRAND_SUPERGROUP_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(bitem.SUPERCFG);
                            break;
                        //CHANNEL
                        case Classes.Default.Attributes.BRAND_CHANNEL_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(bitem.CHANNELCFG);
                            break;
                        //FRANCHISE
                        case Classes.Default.Attributes.BRAND_FRANCHISE_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(bitem.FRANCHISECFG);
                            break;
                        //KEYBRANDS
                        case Classes.Default.Attributes.BRAND_KEYBRANDS_CONFIG_ID:
                            items = Models.ConfigOperationsModel.GetOppList(bitem.KEYBRANDSCFG);
                            break;
                    }
                    break;
            }
            return items;
        }
        public List<SelectListItem> GetLoaderConfig(string type, int _channel, int _market, int _brand)
        {
            switch (type)
            {
                case "NTS":
                    StrawmanDBLibray.Entities.NTS_MASTER nmst = StrawmanDBLibray.Repository.NTS_MASTER.get(_channel, _market, _brand);
                    List<StrawmanDBLibray.Entities.NTS_MASTER> nlst = (List<StrawmanDBLibray.Entities.NTS_MASTER>)StrawmanDBLibray.Repository.NTS_MASTER.getAll();
                    return nlst.Select(m => new SelectListItem
                    {
                        Value = m.ID.ToString(),
                        Text = m.MARKET_NAME,
                        Selected = m.ID == (nmst == null?-1:nmst.ID)
                    }).ToList();
                case "IMS_NIELSEN":
                    StrawmanDBLibray.Entities.ROSETTA_LOADER rmst = StrawmanDBLibray.Repository.ROSETTA_LOADER.get(_channel, _market, _brand);
                    List<StrawmanDBLibray.Entities.ROSETTA_LOADER> rlst = (List<StrawmanDBLibray.Entities.ROSETTA_LOADER>)StrawmanDBLibray.Repository.ROSETTA_LOADER.getAll();
                    return rlst.Select(m => new SelectListItem
                    {
                        Value = m.ID.ToString(),
                        Text = m.DESCRIPTION,
                        Selected = m.ID == (rmst == null ? -1 : rmst.ID)
                    }).ToList();
                default:
                    return null;
            }
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
                case Helpers.StrawmanViews.KPI.id:
                    _partialView = Helpers.StrawmanViews.KPI.Scripts.forms;
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
            Models.GenericConfigModel model = new Models.GenericConfigModel();
            model.path = BOY_CONFIG_PATH;
            return View(BOY_CONFIG_VIEW, model);
        }
        public ActionResult GetBOYGroups()
        {
            List<StrawmanApp.Models.MasterDataModels> model = new List<StrawmanApp.Models.MasterDataModels>();
            List<StrawmanDBLibray.Entities.BOY_CONFIG> list = StrawmanDBLibray.Repository.BOY_CONFIG.getAll();
            model = 
                list.Where(m=>m.NTS_ORDER !=0).Select(m => new { name = m.NTS_NAME, order = m.NTS_ORDER }).Distinct().AsEnumerable()
                .Select(m => new Models.MasterDataModels
                {
                    nts_name = m.name,
                    order = m.order,
                }).Distinct().OrderBy(m => m.order).ToList();
                return PartialView(BOY_GROUPS, model);
        }
        public ActionResult GetBOYConfigItems(string order)
        {
            Models.GenericConfigModel model = new Models.GenericConfigModel();
            List<StrawmanDBLibray.Entities.BOY_CONFIG> list = StrawmanDBLibray.Repository.BOY_CONFIG.getAll();
            List<SelectListItem> channels = GetChannelList(Helpers.Channels.MASS);
            model.path = BOY_CONFIG_PATH;
            Models.TableConfig table = new Models.TableConfig
            {
                id = "boy_config",
                content = list.Where(m => m.NTS_ORDER == int.Parse(order)).Select(m => new Models.MasterDataModels
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
                view = BOY_CONFIG_TABLE_VIEW,
                default_val = int.Parse(order)
            };
            model.tables.Add(table);
            return PartialView(model.content_view, model);
        }
        public ActionResult EditBOYGroup(string _id)
        {
            List<StrawmanApp.Models.MasterDataModels> model = new List<Models.MasterDataModels>();
            List<StrawmanDBLibray.Entities.BOY_CONFIG> list = StrawmanDBLibray.Repository.BOY_CONFIG.getAll();
            model = list.Where(m => m.NTS_ORDER != 0).Select(m => new { name = m.NTS_NAME, order = m.NTS_ORDER }).Distinct().AsEnumerable()
                .Where(m => m.order == int.Parse(_id)).Select(m => new Models.MasterDataModels
                {
                    nts_name = m.name,
                    order = m.order,
                    id=m.order,
                    type = "group"
                }).ToList();
            return PartialView(BOY_GROUPS_EDIT, model);
        }
        #endregion

        #region KPIConfig
        public ActionResult KPIEditor(string source)
        {
            List<Entities.KpiModel> lst = new KPIController().GetKPIDataP("MASTER_DATA");
            List<Entities.KpiModel> model = new KPIController().GetKPIDataP("v_BRAND_CONTRIBUTION");
            ViewBag.MasterData = lst;
            return PartialView(KPI_EDITOR, model);
        }

        public ActionResult SaveKPIForm(List<Entities.KpiModel> data)
        {
            string status = "success";
            string message = SUCCESS_MESSAGE;
            System.Collections.Specialized.NameValueCollection form_data = Request.Form;
            int counter = 0;
            List<StrawmanDBLibray.Entities.BRAND_CONTRIBUTION> _data = new List<StrawmanDBLibray.Entities.BRAND_CONTRIBUTION>();
            foreach (string index in form_data.GetValues("item.KPI"))
            {
                string[] val_COL1 = form_data.GetValues("item.COL1");
                string[] val_COL2 = form_data.GetValues("item.COL2");
                string[] val_COL3 = form_data.GetValues("item.COL3");
                StrawmanDBLibray.Entities.BRAND_CONTRIBUTION mdl = new StrawmanDBLibray.Entities.BRAND_CONTRIBUTION()
                {
                    KPI_ID = decimal.Parse(index),
                    COL1 = decimal.Parse(val_COL1[counter]),
                    COL2 = decimal.Parse(val_COL2[counter]),
                    COL3 = decimal.Parse(val_COL3[counter]),
                    YEAR_PERIOD = Helpers.PeriodUtil.Year,
                    MONTH_PERIOD = Helpers.PeriodUtil.Month
                };
                _data.Add(mdl);
                counter++;
            }
            int ret = StrawmanDBLibray.Repository.BRAND_CONTRIBUTION.saveList(_data);
            if (ret < 0) status = "error";
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Manage View Groups Data
        [Authorize]
        public ActionResult DataGroup()
        {
            return GetDataGroupFor("0");
        }

        [Authorize]
        private ActionResult GetDataGroupFor(string _option) 
        {
            string _view = VIEW_DATA_GROUP;

            ViewBag.Options = "GetOptionsGroupFor";
            ViewBag.OptionsValue = _option;

            ViewBag.MasterList = "GetGroupMasterListFor";
            ViewBag.MasterListOption = _option;

            ViewBag.Title = "DataConfig";
            
            return View(_view);
        }

        [ChildActionOnly]
        public ActionResult GetOptionsGroupFor(string _option)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            model.Add(new SelectListItem{Text = "Group",Value = "GROUP",Selected = true});
            model.Add(new SelectListItem{Text ="Super-Group",Value ="SUPER"});
            model.Add(new SelectListItem { Text = "Channel", Value = "CHANNEL" });
            ViewBag.SelectName = "select_group";
            return PartialView(SELECT_LIST_VIEW, model);
        }
        [Authorize]
        public ActionResult GetGroupMasterListFor(string _option)
        {
            List<Models.ItemsConfigModel> model = new List<Models.ItemsConfigModel>();
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> data = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_MARKET_DATA, false);
            List<StrawmanDBLibray.Entities.CHANNEL_MASTER> cmster = StrawmanDBLibray.Repository.CHANNEL_MASTER.getAll();
            switch (_option)
            {
                case "SUPER":
                    List<StrawmanDBLibray.Entities.BRAND_MASTER> bmster = StrawmanDBLibray.Repository.BRAND_MASTER.getAll();
                    model = data.Where(m => (m.SOURCE == "SUPER") && ((m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month) || (m.YEAR_PERIOD == null && m.MONTH_PERIOD == null))).Select(m =>
                        new Models.ItemsConfigModel
                        {
                         market_description = m.NAME,
                         brand_description = m.BRAND_NAME?? m.NAME,
                         market = m.MARKET.ToString(),
                         brand = m.BRAND.ToString(),
                         channel = m.CHANNEL.ToString(),
                         channel_description = cmster.Find(s=>s.ID == m.CHANNEL).NAME,
                         id = (int) bmster.Where(s => s.GROUP == m.GROUP).First().SUPER_GROUP,
                         group_id = m.GROUP.ToString(),
                        }).ToList();
                    break;
                case "CHANNEL":
                    model = data.Where(m => (m.SOURCE == "CHANNEL") && ((m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month) || (m.YEAR_PERIOD == null && m.MONTH_PERIOD == null))).Select(m =>
                        new Models.ItemsConfigModel
                        {
                            market_description = m.NAME,
                            brand_description = m.BRAND_NAME ?? m.NAME,
                            market = m.MARKET.ToString(),
                            brand = m.BRAND.ToString(),
                            channel = m.CHANNEL.ToString(),
                            channel_description = cmster.Find(s => s.ID == m.CHANNEL).NAME,
                            id = (int)m.CHANNEL,
                            group_id = m.GROUP.ToString(),
                        }).ToList();
                    break;
                default:
                    model = data.Where(m => (m.SOURCE == "TOTAL" || m.SOURCE == "UNIQUE") && ((m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month) || (m.YEAR_PERIOD == null && m.MONTH_PERIOD == null))).Select(m =>
                        new Models.ItemsConfigModel
                        {
                         market_description = m.NAME,
                         brand_description = m.BRAND_NAME?? m.NAME,
                         market = m.MARKET.ToString(),
                         brand = m.BRAND.ToString(),
                         channel = m.CHANNEL.ToString(),
                         channel_description = cmster.Find(s=>s.ID == m.CHANNEL).NAME,
                         id = (int)m.GROUP,
                         group_id = m.GROUP.ToString(),
                        }).ToList();
                    break;
            }
            ViewBag.ActionName = "UpdateGroupItems";
            ViewBag.FormType = FormTypes.MASTER_DATA;
            ViewBag.SelectedId = _option;
            return PartialView(MASTER_DATA_LIST_VIEW, model);
        }
        [Authorize]
        public ActionResult GetGroupItemsListFor(string _option, string _group)
        {
            List<Models.ItemsConfigModel> model = new List<Models.ItemsConfigModel>();
            List<StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG> mfind = null;
            List<StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG> bfind = null;
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> data = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_MARKET_DATA,false);
            List<StrawmanDBLibray.Entities.CHANNEL_MASTER> cmster = StrawmanDBLibray.Repository.CHANNEL_MASTER.getAll();
            List<StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG> mcfg = StrawmanDBLibray.Repository.CALCS_MARKETS_CONFIG.getAll();
            List<StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG> bcfg = StrawmanDBLibray.Repository.CALCS_BRANDS_CONFIG.getAll();
            data = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000 && m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).Select(m => m).ToList();
            switch (_option)
            {
                case "SUPER":
                    List<StrawmanDBLibray.Entities.BRAND_MASTER> bmster = StrawmanDBLibray.Repository.BRAND_MASTER.getAll();
                    //bmster = bmster.Where(m => m.SUPER_GROUP == int.Parse(_group)).Select(m => m).ToList();
                    data = data.Join(bmster.Where(m => m.SUPER_GROUP == int.Parse(_group)).Select(m => m).AsEnumerable(), d => new { id = (decimal)d.BRAND }, b => new { id = b.ID }, (d, b) => new { d = d }).AsEnumerable()
                           .Select(m => m.d).ToList();
                    mfind = mcfg.Select(m => new StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG { BRAND = m.BRAND, MARKET = m.MARKET, GROUPCFG = m.SUPERCFG }).ToList();
                    bfind = bcfg.Select(m => new StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG { BRAND = m.BRAND, MARKET = m.MARKET, GROUPCFG = m.SUPERCFG }).ToList();
                    break;
                case "CHANNEL":
                    data = data.Where(m => m.CHANNEL == int.Parse(_group)).Select(m => m).ToList();
                    mfind = mcfg.Select(m => new StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG { BRAND = m.BRAND, MARKET = m.MARKET, GROUPCFG = m.CHANNELCFG }).ToList();
                    bfind = bcfg.Select(m => new StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG { BRAND = m.BRAND, MARKET = m.MARKET, GROUPCFG = m.CHANNELCFG }).ToList();
                    break;
                default:
                    data = data.Where(m => m.GROUP == int.Parse(_group)).Select(m => m).ToList();
                    mfind = mcfg.Select(m => new StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG { BRAND = m.BRAND, MARKET = m.MARKET, GROUPCFG = m.GROUPCFG }).ToList();
                    bfind = bcfg.Select(m => new StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG { BRAND = m.BRAND, MARKET = m.MARKET, GROUPCFG = m.GROUPCFG }).ToList();
                    break;
            }
            model = data.Select(m =>
                new Models.ItemsConfigModel
                {
                    market_description = m.NAME,
                    brand_description = m.BRAND_NAME ?? m.NAME,
                    market = m.MARKET.ToString(),
                    brand = m.BRAND.ToString(),
                    channel = m.CHANNEL.ToString(),
                    channel_description = cmster.Find(s => s.ID == m.CHANNEL).NAME,
                    id = (int)m.BRAND,
                    market_config = mfind.Exists(s=>s.BRAND == m.BRAND)? mfind.Find(s=>s.BRAND == m.BRAND).GROUPCFG:0,
                    brand_config = bfind.Exists(s=>s.BRAND == m.BRAND)? bfind.Find(s => s.BRAND == m.BRAND).GROUPCFG:0,
                    group_id = m.GROUP.ToString(),
                }).ToList();
            ViewBag.Option = _option;
            ViewBag.ActionName = "UpdateGroupItemsConfig";
            ViewBag.FormType = FormTypes.UPDATE_CONFIG;
            ViewBag.SelectedId = _option;
            return PartialView(MASTER_DATA_LIST_VIEW, model);
        }
        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult EditItemsDataGroup(string _option, string _channel, string _group)
        {
            List<Models.ItemsConfigModel> model = new List<Models.ItemsConfigModel>();
            int channel = int.Parse(_channel);
            List<StrawmanDBLibray.Entities.BRAND_MASTER> bmster = StrawmanDBLibray.Repository.BRAND_MASTER.getAll();
            List<StrawmanDBLibray.Entities.MARKET_MASTER> mmster = StrawmanDBLibray.Repository.MARKET_MASTER.getAll();
            List<StrawmanDBLibray.Entities.CHANNEL_MASTER> cmster = StrawmanDBLibray.Repository.CHANNEL_MASTER.getAll();
            switch (_option)
            {
                case "CHANNEL":
                    model = bmster.Where(m => m.CHANNEL == channel).Select(m => new Models.ItemsConfigModel
                    {
                        market_description = mmster.Find(s => s.ID == m.MARKET).NAME,
                        market = m.MARKET.ToString(),
                        brand_description = m.NAME,
                        brand = m.ID.ToString(),
                        channel_description = cmster.Find(s => s.ID == m.CHANNEL).NAME,
                        channel = _channel,
                        id = (int)m.ID,
                        group_id = m.GROUP.ToString(),
                        selected = m.CHANNEL.ToString() == _group
                    }).ToList();
                    break;
                case "SUPER":
                    model = bmster.Where(m => m.CHANNEL == channel).Select(m => new Models.ItemsConfigModel
                    {
                        market_description = mmster.Find(s => s.ID == m.MARKET).NAME,
                        market = m.MARKET.ToString(),
                        brand_description = m.NAME,
                        brand = m.ID.ToString(),
                        channel_description = cmster.Find(s => s.ID == m.CHANNEL).NAME,
                        channel = _channel,
                        id = (int)m.ID,
                        group_id = m.GROUP.ToString(),
                        selected = m.SUPER_GROUP.ToString() == _group
                    }).ToList();
                    break;
                default:
                    model = bmster.Where(m => m.CHANNEL == channel).Select(m => new Models.ItemsConfigModel
                    {
                        market_description = mmster.Find(s => s.ID == m.MARKET).NAME,
                        market = m.MARKET.ToString(),
                        brand_description = m.NAME,
                        brand = m.ID.ToString(),
                        channel_description = cmster.Find(s => s.ID == m.CHANNEL).NAME,
                        channel = _channel,
                        id = (int)m.ID,
                        group_id = m.GROUP.ToString(),
                        selected = m.GROUP.ToString() == _group
                    }).ToList();
                    break;
            }
            ViewBag.ActionName = "UpdateGroupItems";
            ViewBag.SelectedId = _channel;
            ViewBag.CurrentGroup = int.Parse(_group);
            ViewBag.FormType = FormTypes.ADD_ITEMS;
            return PartialView(MASTER_DATA_LIST_VIEW, model);
        }
        [Authorize]
        public ActionResult NewDataGroup(string _option)
        {
            ViewBag.ModalId = "new_group";
            ViewBag.ModalTitle = Helpers.MessageByLanguage.New;
            ViewBag.DefaultForm = "NewDataGroupForm";
            ViewBag.DefaultFormParams = new { _option = _option };
            ViewBag.DefaultController = CONTROLLER_NAME;
            ViewBag.DefaultFooter = "GetFooterForDataGroupForm";
            return PartialView(new Models.FormModel().modal_view);
        }
        [Authorize]
        public ActionResult EditDataGroup(string _option, string _id)
        {
            ViewBag.ModalId = "edit_group";
            ViewBag.ModalTitle = Helpers.MessageByLanguage.Edit;
            ViewBag.DefaultForm = "EditDataGroupForm";
            ViewBag.DefaultFormParams = new { _option = _option, _id = _id };
            ViewBag.DefaultController = CONTROLLER_NAME;
            ViewBag.DefaultFooter = "GetFooterForDataGroupForm";
            return PartialView(new Models.FormModel().modal_view);
        }
        [ChildActionOnly]
        public ActionResult GetFooterForDataGroupForm()
        {
            return new FormUtilController().GetButtonsGroupByName(FormUtilController.ButtonListsTypes.SAVE_AND_CLOSE);
        }
        [ChildActionOnly]
        public ActionResult EditDataGroupForm(string _option, string _id)
        {
            List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> data = (List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_STRWM_MARKET_DATA, false);
            int items = data.Where(m => m.BRAND < 9000 && m.MARKET < 9000 && m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.GROUP == int.Parse(_id)).Count();
            StrawmanDBLibray.Entities.MARKET_GROUPS grp = StrawmanDBLibray.Repository.MARKET_GROUPS.getById(int.Parse(_id));
            List<StrawmanDBLibray.Entities.BRAND_MASTER> brm = StrawmanDBLibray.Repository.BRAND_MASTER.getAll();
            StrawmanDBLibray.Entities.BRAND_MASTER aux = brm.Where(m => m.GROUP == int.Parse(_id)).FirstOrDefault();
            int channel = (int)(grp.CHANNEL ?? aux.CHANNEL);
            Models.FormModel model = FormUtilController.GetNewGroupFormFor(_option, channel.ToString(), _id, grp.NAME, grp.ORDER.ToString(), (items>0).ToString());
            return PartialView(model.view, model);
        }
        [ChildActionOnly]
        public ActionResult NewDataGroupForm(string _option)
        {
            Models.FormModel model = FormUtilController.GetNewGroupFormFor(_option, null, null, null, null, null);
            return PartialView(model.view, model);
        }
        [HttpPost]
        [Authorize]
        public ActionResult UpdateGroupItems(List<Models.ItemsConfigModel> model)
        {
            int ret = 0;
            bool success = false; string message = "Data Saved";
            List<Models.ItemsConfigModel> _model = model;
            string[] item_brand = Request.Form["item.brand"].Split(',');
            string[] item_market = Request.Form["item.market"].Split(',');
            string[] item_channel = Request.Form["item.channel"].Split(',');
            string[] item_group = Request.Form["item.group_id"].Split(',');
            string current_group = Request.Form["current_group"];
            int index = 0;
            if (Helpers.UserUtils.Permissions.GetPermissions())
            {
                foreach (string item in item_brand)
                {
                    bool save = false;
                    bool unselected = Request.Form["item_selected_" + item].Split(',')[0] == "true";
                    bool selected = Request.Form["item_selected_" + item].Split(',')[0] == item && Request.Form["item_selected_" + item].Split(',')[1] == "true";
                    StrawmanDBLibray.Entities.MARKET_MASTER mmster = StrawmanDBLibray.Repository.MARKET_MASTER.getById(int.Parse(item_market[index]));
                    StrawmanDBLibray.Entities.BRAND_MASTER bmster = StrawmanDBLibray.Repository.BRAND_MASTER.getById(int.Parse(item_brand[index]));
                    if (selected)
                    {
                        mmster.GROUP = int.Parse(current_group);
                        bmster.GROUP = int.Parse(current_group);
                        save = true;
                    }
                    else if (unselected)
                    {
                        if (mmster.GROUP == int.Parse(current_group)) { mmster.GROUP = 0; save = true; }
                        if (bmster.GROUP == int.Parse(current_group)) { bmster.GROUP = 0; save = true; }
                    }
                    if (save)
                    {
                        ret += StrawmanDBLibray.Repository.MARKET_MASTER.Save(mmster);
                        ret += StrawmanDBLibray.Repository.BRAND_MASTER.Save(bmster);
                    }
                    index++;
                }
            }
            else
                message = "No permissions for user " + Helpers.UserUtils.UserName + " for save data";
            if (ret > 0)
                success = true;
            else
                message = "Error saving data";
            return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        public ActionResult UpdateGroupItemsConfig(FormCollection collection)
        {
            int ret = 0;
            bool success = false; string message = "Data Saved";
            string[] item_brand = Request.Form["item.brand"].Split(',');
            string[] item_market = Request.Form["item.market"].Split(',');
            string[] item_channel = Request.Form["item.channel"].Split(',');
            string[] item_group = Request.Form["item.group_id"].Split(',');
            string current_group = Request.Form["current_group"];
            string option = Request.Form["source"];
            int index = 0;
            if (Helpers.UserUtils.Permissions.GetPermissions())
            {
                FormCollection coll = collection;
                foreach (string item in item_brand)
                {
                    bool unselected_ = Request.Form["market_config_" + item].Split(',')[0] == "true";
                    bool selected_ = Request.Form["market_config_" + item].Split(',')[0] == item && Request.Form["market_config_" + item].Split(',')[1] == "true";
                    List<StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG> mlist = StrawmanDBLibray.Repository.CALCS_MARKETS_CONFIG.getAll();
                    List<StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG> blist = StrawmanDBLibray.Repository.CALCS_BRANDS_CONFIG.getAll();
                    StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG mmster = mlist.Find(m=>m.BRAND == int.Parse(item));
                    if (mmster == null) mmster = new StrawmanDBLibray.Entities.CALCS_MARKETS_CONFIG
                    {
                        BRAND = int.Parse(item),
                        MARKET = int.Parse(item_market[index]),
                    };
                    switch (option)
                    {
                        case "CHANNEL":
                            mmster.CHANNELCFG = selected_ ? 1 : mmster.CHANNELCFG ?? 0;
                            if (unselected_) mmster.CHANNELCFG = 0;
                            break;
                        case "SUPER":
                            mmster.SUPERCFG = selected_ ? 1 : mmster.SUPERCFG ?? 0;
                            if (unselected_) mmster.SUPERCFG = 0;
                            break;
                        default:
                            mmster.GROUPCFG = selected_ ? 1 : mmster.GROUPCFG ?? 0;
                            if (unselected_) mmster.GROUPCFG = 0;
                            break;
                    }
                    if (unselected_ || selected_)
                    {
                        ret += StrawmanDBLibray.Repository.CALCS_MARKETS_CONFIG.Save(mmster);
                    }

                    unselected_ = Request.Form["brand_config_" + item].Split(',')[0] == "true";
                    selected_ = Request.Form["brand_config_" + item].Split(',')[0] == item && Request.Form["brand_config_" + item].Split(',')[1] == "true";

                    StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG bmster = blist.Find(m => m.BRAND == int.Parse(item));
                    if(bmster == null) bmster = new StrawmanDBLibray.Entities.CALCS_BRANDS_CONFIG
                    {
                        BRAND = int.Parse(item),
                        MARKET = int.Parse(item_market[index]),
                    };
                    switch (option)
                    {
                        case "CHANNEL":
                            bmster.CHANNELCFG = selected_ ? 1 : bmster.CHANNELCFG ?? 0;
                            if (unselected_) bmster.CHANNELCFG = 0;
                            break;
                        case "SUPER":
                            bmster.SUPERCFG = selected_ ? 1 : bmster.SUPERCFG ?? 0;
                            if (unselected_) bmster.SUPERCFG = 0;
                            break;
                        default:
                            bmster.GROUPCFG = selected_ ? 1 : bmster.GROUPCFG ?? 0;
                            if (unselected_) bmster.GROUPCFG = 0;
                            break;
                    }
                    if (unselected_ || selected_)
                    {
                        ret += StrawmanDBLibray.Repository.CALCS_BRANDS_CONFIG.Save(bmster);
                    }
                    index++;
                }
            }
            else
                message = "No permissions for user " + Helpers.UserUtils.UserName + " for save data";
            if (ret > 0)
                success = true;
            else
                message = "Error saving data";
            return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult SaveNewGroup(FormCollection collection)
        {
            bool success = false; int ret = -1;
            string message = "Not data saved";
            if (Helpers.UserUtils.Permissions.GetPermissions())
            {
                FormCollection coll = collection;
                string _id = coll[Classes.Default.Attributes.ORDER_ID.formated()];
                string _channel = coll[Classes.Default.Attributes.CHANNEL_ID.formated()];
                string _order = coll[Classes.Default.Attributes.ORDER_ID.formated()];
                string _name = coll[Classes.Default.Attributes.GROUP_NAME_ID.formated()];
                string table = coll[Classes.Default.Attributes.INPUT_TABLE_ID.formated()];
                switch (table)
                {
                    case StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_GROUPS:
                        StrawmanDBLibray.Entities.MARKET_GROUPS group = new StrawmanDBLibray.Entities.MARKET_GROUPS
                        {
                            ID = int.Parse(_id ?? "0"),
                            NAME = _name,
                            ORDER = int.Parse(_order ?? "0")
                        };
                        //Si no existe el canal asumimos que estamos actualizando solo el orden y en nombre
                        if (_channel != null) group.CHANNEL = int.Parse(_channel);

                        ret = StrawmanDBLibray.Repository.MARKET_GROUPS.Save(group);
                        break;
                }
                if (ret > 0)
                {
                    success = true;
                    message = "Data Saved";
                }
            }
            return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Forms Backend
        public ActionResult SaveItemsConfig(List<StrawmanApp.Models.ItemsConfigModel> model)
        {
            List<StrawmanDBLibray.Entities.GROUP_CONFIG> items = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.Session.GetSession(GROUP_CFG_DATA);
            int result = StrawmanDBLibray.Repository.GROUP_CONFIG.SaveGroupConfig(items);
            return Json(new { success = "success" }, JsonRequestBehavior.AllowGet);
        }
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
        public class FormTypes
        {
            public static string ADD_ITEMS = "ADD_ITEMS";
            public static string UPDATE_CONFIG = "UPDATE_CONFIG";
            public static string MASTER_DATA = "MASTER_DATA";
        }
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

        private const string GROUPS_CONFIGURE = CONTROLLER + "/GroupsConfig";
        private const string GROUPS_CONFIGURE_PATH = _PATH + "Forms/GroupsConfigure.cshtml";
        private const string BOOTSTRAP_TABS = _PATH + "Tabs/_BootstrapTabs.cshtml";
        private const string GROUP_MASTER = _PATH + CONTROLLER + "/Groups/_GroupMaster.cshtml";
        private const string GROUP_MASTER_EDIT = _PATH + CONTROLLER + "/Groups/_GroupMasterEdit.cshtml";
        private const string GROUP_CONFIG = _PATH + CONTROLLER + "/Groups/_GroupConfig.cshtml";
        private const string GROUP_CONFIG_EDIT = _PATH + CONTROLLER + "/Groups/_GroupConfigEdit.cshtml";
        private const string GROUP_MODAL_ITEMS = _PATH + CONTROLLER + "/Groups/_ModalItems.cshtml";
        private const string GROUP_ITEMS = _PATH + CONTROLLER + "/Groups/_Items.cshtml";
        private const string GROUP_CFG_DATA = "GROUP_CFG_DATA";

        private const string MARKET = "MARKET";
        private const string BRAND = "BRAND";

        private const string MODELS_MASTER_CONFIG = "MODELS_MASTER_CONFIG";

        private const string SHARED_PERIOD_SELECTOR = _PATH + "/Shared/_PeriodSelector.cshtml";
        private const string PERIOD_BANNER = _PATH + "/Shared/_PeriodBanner.cshtml";

        private const string DOLAR_CHANGE = "DOLAR_CHANGE";
        private const string CURRENCY_BANNER = _PATH + "/Shared/_CurrencyBanner.cshtml";
        private const string CURRENCY_ADJUST = _PATH + "/Shared/_CurrencyAdjust.cshtml";

        private const string LOAD_CONFIG = _PATH + CONTROLLER + "/_LoadConfig.cshtml";
        private const string LOAD_CONFIG_NTS = _PATH + CONTROLLER + "/LoadConfig/_GridNTS.cshtml";
        private const string LOAD_CONFIG_NIELSEN = _PATH + CONTROLLER + "/LoadConfig/_GridNielsen.cshtml";

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
                             BOY_GROUPS = BOY_CONFIG_PATH + "_GroupsList.cshtml",
                             BOY_GROUPS_EDIT = BOY_CONFIG_PATH + "_GroupsListEdit.cshtml",
                             CHANNEL_PILLS_VIEW = _PATH + CONTROLLER + "/" + COMPONENTS + "/_ChannelPills.cshtml",
                             USER_CONFIGURE = "UserManagment",
                             GET_MASTER_DATA = "GetMasterData";
        private const string KPI_EDITOR = _PATH + CONTROLLER + "/KPI/index.cshtml";

        private const string VIEW_DATA_GROUP = _PATH + CONTROLLER + "/Views/Index.cshtml",
                             MASTER_DATA_LIST_VIEW = _PATH + CONTROLLER + "/Views/_DataList.cshtml",
                             SELECT_LIST_VIEW = _PATH + CONTROLLER + "/Views/_SelectList.cshtml"
                             ;

        #endregion
    }
}
