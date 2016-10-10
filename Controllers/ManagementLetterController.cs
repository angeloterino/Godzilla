using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class ManagementLetterController : Controller
    {
        //
        // GET: /ManagementLetter/GetComments
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetComments()
        {
            List<Entities.v_WRK_MANAGEMENT_LETTERS> lst = new List<Entities.v_WRK_MANAGEMENT_LETTERS>();
            using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
            {
                var q = db.LETTERS_COMMENT_DATA
                        .Select(m => m).AsEnumerable();

                var t = db.v_WRK_MANAGEMENT_LETTERS.Select(m => m).AsEnumerable();
                lst = t.Select(m =>m).ToList();

                ViewBag.LettersData = q
                        .Where(m => m.TYPE == Entities.CommentTypes.MANAGEMENT_LETTER 
                        && (m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).ToList();

            }
            return PartialView(MANAGEMENTLETTER_COMMENTS, lst);
        }
        //
        // GET: /ManagementLetter/GetData

        public ActionResult GetData()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_MANAGEMENT_LETTERS_DATA, true);
            List<Entities.ManagementLetterModel>  lst = data
                .Where(p => p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                .Select(m => new Entities.ManagementLetterModel
            {
                name = m.SHORT_NAME,
                ytd_col1 = m.YTD_COL1,
                ytd_col2 = m.YTD_COL2,
                ytd_col3 = m.YTD_COL3,
                latest_col1 = m.LATEST_COL1,
                latest_col2 = m.LATEST_COL2,
                latest_col3 = m.LATEST_COL3
            }).ToList();
            ViewBag.HeadTitle = "SOUTHERN EUROPE MARKET SHARE DATA";
            string period_text = new DateTime(DateTime.Now.Year, Helpers.PeriodUtil.Month, DateTime.Now.Day).ToString("MMMM", CultureInfo.InvariantCulture).ToString();
            period_text += " " + Helpers.PeriodUtil.Year.ToString();
            ViewBag.PeriodText = period_text;
            ViewBag.Country = Helpers.CountryUtil.Country;
            
            return PartialView(MANAGEMENTLETTER_DATA,lst);
        }

        //
        // GET: /ManagementLetter/

        public ActionResult Index()
        {
            ViewBag.Title = CONTROLLER_NAME;
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.PeriodMonth = new DateTime(DateTime.Now.Year, Helpers.PeriodUtil.Month, DateTime.Now.Day).ToString("MMMM", CultureInfo.InvariantCulture);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            return View();
        }

        //
        // GET: /ManagementLetter/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ManagementLetter/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ManagementLetter/Create

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
        // GET: /ManagementLetter/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ManagementLetter/Edit/5

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
        // GET: /ManagementLetter/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ManagementLetter/Delete/5

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

        ///Constantes del controlador
        #region Constants

        private const string _PATH = "~/Views/ManagementLetter/";
        private const string CONTROLLER_NAME = "ManagementLetter";
        private const string MANAGEMENTLETTER_DATA = _PATH + "_Data.cshtml";
        private const string MANAGEMENTLETTER_COMMENTS = _PATH + "_Comments.cshtml";

        #endregion
    }
}
