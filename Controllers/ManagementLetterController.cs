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
        [ChildActionOnly]
        public ActionResult GetCommentWrapper(StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS item, List<StrawmanDBLibray.Entities.LETTERS_COMMENT_DATA> letters, bool get_child)
        {
            List<StrawmanDBLibray.Entities.MANAGEMENT_LETTERS_REL> rel = (List<StrawmanDBLibray.Entities.MANAGEMENT_LETTERS_REL>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.MANAGEMENT_LETTERS_REL,false);
            var d = rel.Where(m=>m.MARKET == item.MARKET && m.BRAND == item.BRAND && m.CHANNEL == item.CHANNEL).AsEnumerable();
            var s = letters.Join(d , l=>new {_id = (decimal)l.LETTER_ID}, r=>new{_id=r.MANAGEMENT_LETTER_ID},(l,r)=> new {l=l}).Select(l=>l.l).ToList();
            return PartialView(COMMENTS_WRAPPER, new ViewDataDictionary{{"item",item},{"letters",s}});
        }

        //
        // GET: /ManagementLetter/GetComments
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetComments()
        {
            List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS> lst = (List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_MANAGEMENT_LETTERS, Helpers.Session.CacheStatus);
            var ltr = (List<StrawmanDBLibray.Entities.LETTERS_COMMENT_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.LETTERS_COMMENT_DATA,false);

            ViewBag.LettersData = ltr
                    .Where(m =>(m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)).ToList();

            return PartialView(MANAGEMENTLETTER_COMMENTS, lst);
        }
        //
        // GET: /ManagementLetter/GetData

        public ActionResult GetData()
        {
            List<Entities.ShareBoardModel> share_month = (List<Entities.ShareBoardModel>)new ShareBoardController().GetShareBoardChannelData("MONTH");
            List<Entities.ShareBoardModel> share_ytd = (List<Entities.ShareBoardModel>)new ShareBoardController().GetShareBoardChannelData("YTD");

            decimal? group_type = Helpers.StrawmanCalcs.GetGroupTypeByView("MANAGEMENT_LETTERS_DATA");

            List<StrawmanDBLibray.Entities.GROUP_CONFIG> cfg = (List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG);
            List<StrawmanDBLibray.Entities.GROUP_MASTER> mstr = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER);
            //List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS_DATA> data = (List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_MANAGEMENT_LETTERS_DATA, true);
            
            List<Entities.MonthlyCommentsModel> lst = share_month.Join(share_ytd, m => new { _id = m.id }, y => new { _id = y.id }, (m, y) => new { m = m, y = y }).AsEnumerable()
                .Join(cfg.Where(m=>m.TYPE_ID == group_type).Select(m=>m).ToList(), d=> new{_id = d.m.id}, m=>new {_id = (decimal)m.BRAND}, (d,m)=>  new{d=d, m=m})
                .OrderBy(m=>m.m.ID)
                .AsEnumerable()
                .Select(m => new Entities.MonthlyCommentsModel
            {
                name = mstr.Where(s=>s.ID == m.m.GROUP_ID).FirstOrDefault().NAME,
                ytd_market_growth = m.d.y.col1,
                ytd_brand_growth = m.d.y.col2,
                ytd_market_share = m.d.y.col3,
                ytd_pt = m.d.y.col4,
                month_market_growth = m.d.m.col1,
                month_brand_growth = m.d.m.col2,
                month_market_share = m.d.m.col3,
                month_pt = m.d.m.col4
            }).ToList();
            ViewBag.HeadTitle = "SOUTHERN EUROPE MARKET SHARE DATA";
            string period_text = new DateTime(DateTime.Now.Year, Helpers.PeriodUtil.Month, DateTime.Now.Day).ToString("MMMM", CultureInfo.InvariantCulture).ToString();
            period_text += " " + Helpers.PeriodUtil.Year.ToString();
            ViewBag.PeriodText = period_text;
            ViewBag.Country = Helpers.CountryUtil.Country;
            ViewBag.MonthTitle = period_text + " " + (Helpers.PeriodUtil.Year - 1).ToString() + " Vs " + period_text + " " + (Helpers.PeriodUtil.Year).ToString() + " ";
            ViewBag.YTDTitle = "YTD " + period_text + " " + (Helpers.PeriodUtil.Year - 1).ToString() + " Vs YTD " + period_text + " " + (Helpers.PeriodUtil.Year).ToString() + " ";
            return PartialView(MONTHLY_COMMENTS_DATA, lst);
        }

        private void SetViewBagVariables()
        {
            List<StrawmanDBLibray.Entities.STRWM_NTS_DATA> sdata = (List<StrawmanDBLibray.Entities.STRWM_NTS_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.STRWM_NTS_DATA);
            var data = sdata.GroupBy(m => new { _yp = m.YEAR_PERIOD, _mp = m.MONTH_PERIOD }).AsEnumerable()
                .Select(m => new
                {
                    _yp = m.Key._yp,
                    _mp = m.Key._mp,
                    month_actual = m.Sum(p=>p.MONTH_AMOUNT_ACTUAL??0),
                    month_py = m.Sum(p=>p.MONTH_AMOUNT_PY??0),
                    ytd_actual = m.Sum(p=>p.YTD_AMOUNT_ACTUAL??0),
                    ytd_py = m.Sum(p=>p.YTD_AMOUNT_PY??0)
                }).AsEnumerable()
                .Select(m=>new{
                    _yp = m._yp,
                    _mp = m._mp,
                    month = Helpers.StrawmanCalcs.CalcPCVSPY(m.month_actual, m.month_py),
                    ytd = Helpers.StrawmanCalcs.CalcPCVSPY(m.ytd_actual, m.ytd_py)
                }).ToList();
            string period_text = new DateTime(DateTime.Now.Year, Helpers.PeriodUtil.Month, DateTime.Now.Day).ToString("MMMM", CultureInfo.InvariantCulture).ToString();
            ViewBag.MonthTitle = period_text + " " + (Helpers.PeriodUtil.Year - 1).ToString() + " Vs " + period_text + " " + (Helpers.PeriodUtil.Year).ToString() + " ";
            ViewBag.NTSMonth = data.Where(m => m._mp == Helpers.PeriodUtil.Month && m._yp == Helpers.PeriodUtil.Year).FirstOrDefault().month;
            ViewBag.YTDTitle = "YTD " + period_text + " " + (Helpers.PeriodUtil.Year - 1).ToString() + " Vs YTD " + period_text + " " + (Helpers.PeriodUtil.Year).ToString() + " ";
            ViewBag.NTSYTD = data.Where(m => m._mp == Helpers.PeriodUtil.Month && m._yp == Helpers.PeriodUtil.Year).FirstOrDefault().ytd;

        }

        //
        // GET: /ManagementLetter/

        public ActionResult Index()
        {
            SetViewBagVariables();
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
        private const string COMMENTS_WRAPPER = _PATH + "_CommentWrapper.cshtml";

        private const string MONTHLY_COMMENTS_DATA = "~/Views/MonthlyComments/_Data.cshtml";

        #endregion
    }
}
