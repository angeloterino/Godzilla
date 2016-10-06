using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class KPIController : Controller
    {
        //
        // GET: /KPI/
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetKPINTS()
        {
            List<Entities.KpiModel> lst = GetKPIData("NTS");
            ViewBag.MasterData = GetKPIData(MASTER_DATA);
            return PartialView(KPICOLUMN,lst);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetKPIMarketShare()
        {
            List<Entities.KpiModel> lst = GetKPIData("MARKET SHARE");
            ViewBag.MasterData = GetKPIData(MASTER_DATA);
            return PartialView(KPICOLUMN, lst);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetKPIBrandContribution()
        {
            List<Entities.KpiModel> lst = GetKPIData("BRAND CONTRIBUTION");
            ViewBag.MasterData = GetKPIData(MASTER_DATA);
            return PartialView(KPICOLUMN, lst);
        }

        private List<Entities.KpiModel> GetKPIData(string _column)
        {
            List<Entities.KpiModel> lst = new List<Entities.KpiModel>();
            List<Entities.KpiModel> table = GetKPIModel(v_WRK_KPI);
            if (_column != MASTER_DATA)
            {
                lst = table
                        .Where(m => m.KPI_COLUMN == _column)
                        .Select(m => new Entities.KpiModel
                            {
                                NAME = m.NAME,
                                COL1 = m.COL1,
                                COL2 = (decimal?)m.COL2,
                                COL3 = (decimal?)m.COL3,
                                COLPC1 = m.COL1,
                                COLPC2 = (decimal?)m.COL2,
                                COLPC3 = (decimal?)m.COL3,
                                GROUP = m.GROUP,
                                KPI = m.KPI,
                                KPI_COLUMN = m.KPI_COLUMN,
                                isHead = m.GROUP == 0 || m.NAME.Equals("MASS") || m.NAME.Equals("OTC") || m.NAME.Equals("BEAUTY"),
                                isPC = _column.Equals("MARKET SHARE")
                            }).ToList();
            }
            else
            {
                lst = GetKPIModel(MASTER_DATA);
                
            }
            return lst;
        }

        public ActionResult GetTableHead(string table_name)
        {
            Entities.KpiModel mod = new Entities.KpiModel();
            mod.headRowStyle = "background-color: " + Helpers.StrawmanConstants.Colors.Red + ";height: 69px ! important;font-size:0.8em; font-weight:normal; color: " + Helpers.StrawmanConstants.Colors.White+"; " ;
            mod.titleRowStyle = "background-color: "+ Helpers.StrawmanConstants.Colors.Red +";font-size:0.9em;height: 39px;color: " + Helpers.StrawmanConstants.Colors.White + "; ";
            int current_year = Helpers.PeriodUtil.Year;

            mod.head = new Entities.KpiModel.headComponent[4];

            mod.head[0] = new Entities.KpiModel.headComponent { text = "", colStyle = "width:195px" }; ;

            mod.head[1] = new Entities.KpiModel.headComponent { text = (current_year - 1).ToString(), colStyle = "width:75px" };

            mod.head[2] = new Entities.KpiModel.headComponent { text = (current_year).ToString() + " By Target", colStyle = "width:75px" };

            mod.head[3] = new Entities.KpiModel.headComponent { text = (current_year).ToString() + " LE", colStyle = "width:75px" };

            switch (table_name)
            {
                case "NTS":
                    mod.title = "NTS BY BRAND";                                     
                    mod.head[0].text = mod.title;
                    break;

                case "BRAND_CONTRIBUTION":
                    mod.title = "BRAND CONTRIBUTION BY BRAND";                                      
                    mod.head[0].text = mod.title;
                    mod.head[3].text = (current_year).ToString() + " FINAL";                    
                    break;

                case "MARKET_SHARE":
                    mod.title = "MARKET SHARE BY BRAND";                    
                    mod.head[0].text = mod.title;
                    mod.head[3].text = (current_year).ToString() + " FINAL";                    
                    break;
            }
            return PartialView(KPIHEAD, mod);
        }

        public ActionResult Index()
        {
            ResetSessionObjects();
            ViewBag.MenuUrl = MENU_URL;
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            return View();
        }

        //
        // GET: /KPI/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /KPI/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /KPI/Create

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
        // GET: /KPI/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /KPI/Edit/5

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
        // GET: /KPI/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /KPI/Delete/5

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
        #region Functions

        private void ResetSessionObjects(){
            SetSessionObject(v_WRK_KPI,null);
            SetSessionObject(MASTER_DATA,null);
        }

        private object GetSessionObject(string obj_name)
        {
            return Session[obj_name];
        }
        private void SetSessionObject(string obj_name, object obj)
        {
            if (Session[obj_name] == null)
            {
                Session.Add(obj_name, obj);
            }
            else
            {
                Session[obj_name] = obj;
            }
        }

        private List<Entities.KpiModel> GetKPIModel(string table_name)
        {
            List<Entities.KpiModel> lst = null;
            if (GetSessionObject(table_name) != null)
            {
                return (List<Entities.KpiModel>)GetSessionObject(table_name);
            }
            else
            {
                switch (table_name)
                {
                    case v_WRK_KPI:
                        using (Models.DataClasses1DataContext db = new Models.DataClasses1DataContext())
                        {
                            db.CommandTimeout = 50000;
                            var q = db.v_WRK_KPI
                                    .Where(m => (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                                            || (m.YEAR_PERIOD == null && m.MONTH_PERIOD == null))
                                    .Select(m => new Entities.KpiModel
                                    {
                                        NAME = m.NAME,
                                        COL1 = m.COL1,
                                        COL2 = (decimal?)m.COL2,
                                        COL3 = (decimal?)m.COL3,
                                        COLPC1 = m.COL1,
                                        COLPC2 = (decimal?)m.COL2,
                                        COLPC3 = (decimal?)m.COL3,
                                        GROUP = m.GROUP,
                                        KPI = m.KPI,
                                        KPI_COLUMN = m.KPI_COLUMN,
                                        ID = m.ID
                                    });
                            lst = q.ToList();
                        }
                        break;
                    case MASTER_DATA:
                        using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
                        {
                            var t = db.KPI_MASTER.OrderBy(m => m.ORDER).Select(m => new Entities.KpiModel
                            {
                                NAME = m.NAME,
                                GROUP = m.GROUP,
                                KPI = m.ID,
                                isHead = m.GROUP == 0 || m.NAME.Equals("MASS") || m.NAME.Equals("OTC") || m.NAME.Equals("BEAUTY")
                            });
                            lst = t.ToList();
                        }
                        break;
                }
                SetSessionObject(table_name, lst);
            }
            return lst;
        }
        #endregion

        private string KPICOLUMN = "~/Views/KPI/_Column.cshtml";
        private string KPIHEAD = "~/Views/KPI/_Head.cshtml";
        private static string MENU_URL = "StrawmanApp";
        private const string CONTROLLER_NAME = "KPI";
        private const string v_WRK_KPI = "v_WRK_KPI";
        private const string MASTER_DATA = "MASTER_DATA";
    }
}
