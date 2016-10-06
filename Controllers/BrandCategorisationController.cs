using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class BrandCategorisationController : Controller
    {
        //
        // GET: /BrandCategorisation/GetHeader
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetHeader()
        {
            ViewBag.BrandCatCountry = BRANDCATEGORISATION_COUNTRY;
            ViewBag.BrandCatText1 = BRANDCATEGORISATION_TEXT1;
            ViewBag.BrandCatText2 = BRANDCATEGORISATION_TEXT2;
            List<Entities.BrandCategorisationModel>  model = GetBrandCategorisationData(DataType.HEADER);
            Helpers.Session.SetSession(BRANDCATEGORISATION_HEADER, model);
            return PartialView(BRANDCATEGORISATION_HEADER, model);
        }

        //
        // GET: /BrandCategorisation/GetData
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetData()
        {
            List<Entities.BrandCategorisationModel>  model = GetBrandCategorisationData(DataType.DATA);
            ViewBag.Header = Helpers.Session.GetSession(BRANDCATEGORISATION_HEADER);
            return PartialView(BRANDCATEGORISATION_DATA, model);
        }

        //
        // GET: /BrandCategorisation/

        public ActionResult Index()
        {
            ViewBag.Title = CONTROLLER_NAME;
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.BrandCatTitle = BRANDCATEGORISATION_TITLE;
            ViewBag.BrandCatSubTitle = String.Format(BRANDCATEGORISATION_SUBTITLE,
                                                    new DateTime(DateTime.Now.Year, Helpers.PeriodUtil.Month, DateTime.Now.Day).ToString("MMMM", CultureInfo.InvariantCulture) + " " + Helpers.PeriodUtil.Year
                                                    );
            return View();
        }

        //
        // GET: /BrandCategorisation/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /BrandCategorisation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BrandCategorisation/Create

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
        // GET: /BrandCategorisation/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /BrandCategorisation/Edit/5

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
        // GET: /BrandCategorisation/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /BrandCategorisation/Delete/5

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

        ///Funciones privadas
        #region Private Functions
        ///<summary>Devuelve la lista de datos dependiendo del tipo de consulta.</summary> 
        ///<param name="type">Tipo de consulta definido en DataType</param>
        ///
        private List<Entities.BrandCategorisationModel> GetBrandCategorisationData(string type)
        {
            List<Entities.BrandCategorisationModel> lst = new List<Entities.BrandCategorisationModel>();
            using (Entities.godzillaBrandCategorisationEntities db = new Entities.godzillaBrandCategorisationEntities())
            {
                switch (type)
                {
                    case DataType.HEADER:
                        var h = db.v_WRK_BRAND_CATEGORISATION_MASTER.ToList().Select(m => new Entities.BrandCategorisationModel
                        {
                            group_level = m.GROUP_LEVEL,
                            order = m.ORDER,
                            group_name = m.GROUP_NAME,
                            name = m.NAME,
                            type = m.TYPE,
                            id = m.ID
                        });
                        lst = h.ToList();
                        break;
                    case DataType.DATA:
                        var d = db.v_WRK_BRAND_CATEGORISATION_DATA.ToList().Select(m => new Entities.BrandCategorisationModel
                        {
                            group_level = m.GROUP_LEVEL,
                            order = m.ORDER,
                            group_name = m.GROUP_NAME,
                            name = m.NAME,
                            type = m.TYPE,
                            id = m.ID,
                            act = m.ACT,
                            py = m.PY,
                            ytd_share_vspy = m.YTD_SHARE_VS_PY,
                            ytd_category_vspy = m.YTD_CATEGORY_VS_PY
                        });
                        lst = d.ToList();
                        break;
                    default:
                        break;
                }
            }
            return lst;
        }

        #endregion

        ///Constantes del controlador
        #region Constants
        private const string _PATH = "~/Views/BrandCategorisation/";
        private const string CONTROLLER_NAME = "BrandCategorisation";
        private const string BRANDCATEGORISATION_DATA = _PATH + "_Data.cshtml";
        private const string BRANDCATEGORISATION_HEADER = _PATH + "_Header.cshtml";
        private const string BRANDCATEGORISATION_TITLE = "Company Dropdown - YTD NTS";
        private const string BRANDCATEGORISATION_SUBTITLE = "Figures in thousands of USD at {0} rates";
        private const string BRANDCATEGORISATION_COUNTRY = "Spain";
        private const string BRANDCATEGORISATION_TEXT1 = "Monthly Business Operations Review";
        private const string BRANDCATEGORISATION_TEXT2 = "$ MM (@FBP Rates)";

        private class DataType
        {
            public const string HEADER = "HEADER";
            public const string DATA = "DATA";
        }
        
        #endregion
    }
}
