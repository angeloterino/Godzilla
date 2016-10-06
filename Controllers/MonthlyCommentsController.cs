using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class MonthlyCommentsController : Controller
    {
        public JsonResult UpdateComment(Entities.CommentsModel collection)
        {
            return new FormsController().UpdateComment(collection);
        }
        //
        // GET: /ManagementLetter/GetComments/Id
        public ActionResult DeleteComment(int id)
        {
            Entities.CommentsModel comment = new Entities.CommentsModel();
            comment.id = id;
            comment.letter_id = (int)GetLetterID(id);
            comment.del_letter = true;
            comment.type = Entities.CommentTypes.MONTHLY_COMMENTS;
            comment.success_text = DELETE_SUCCESS;
            comment.error_text = ERROR_TEXT;
            comment.container = CONTAINER_CONTROL;
            return new FormsController().UpdateComment(comment);
        }

        //
        // GET: /ManagementLetter/GetComments
        public ActionResult CommentForm(decimal _channel)
        {
            Entities.CommentsModel cmodel = new Entities.CommentsModel();
            cmodel.letter_id = int.Parse(_channel.ToString());
            cmodel.user = Helpers.UserUtils.UserName;
            cmodel.type = Entities.CommentTypes.MONTHLY_COMMENTS;
            cmodel.month = Helpers.PeriodUtil.Month;
            cmodel.year = Helpers.PeriodUtil.Year;
            ViewBag.TitleText = TITLE_TEXT;
            ViewBag.DeleteAction = CONTROLLER + "/DeleteComment";
            return PartialView(COMMENTFORMVIEW,cmodel);
        }
        //
        // GET: /ManagementLetter/GetComments
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetComments()
        {
            using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
            {

                var t = db.v_WRK_MONTHLY_COMMENTS_DATA
                            .Where(m=>m.CHANNEL < 9 
                                    && m.MONTH_PERIOD == Helpers.PeriodUtil.Month && m.YEAR_PERIOD == Helpers.PeriodUtil.Year)
                            .Select(m => m).ToList();
                ViewBag.CommentsData = t.ToList();
                
            }
            ViewBag.UpdateAction = CONTROLLER + "/GetCommentsByChannel";
            ViewBag.EditAction = CONTROLLER + "/GetCommentById";
            ViewBag.CurrentUser = Helpers.UserUtils.UserName;
            return PartialView(MONTHLYCOMMENTS_COMMENTS, GetCommentsText());
        }
        //
        // GET: /ManagementLetter/GetData

        public ActionResult GetData()
        {

            return PartialView(MONTHLYCOMMENTS_DATA, GetCommentsData());
        }

        //
        // GET: /ManagementLetter/GetCommentById
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetCommentById(string _id)
        {
            decimal id = decimal.Parse(_id);
            string text = GetCommentsText().Where(m => m.ID == id).FirstOrDefault().COMMENT;
            return Json(new { text = text }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /ManagementLetter/GetCommentsByChannel
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetCommentsByChannel(string _channel)
        {
            return PartialView(COMMENT_CONTAINER, GetCommentsText(int.Parse(_channel)));
        }

        private decimal GetLetterID(int id)
        {
            return (decimal)GetCommentsText().Where(m => m.ID == (decimal)id).FirstOrDefault().LETTER_ID;
        }

        private List<Entities.LETTERS_COMMENT_DATA> GetCommentsText()
        {
            return GetCommentsText(null);
        }

        private List<Entities.LETTERS_COMMENT_DATA> GetCommentsText(int? _channel)
        {
            List<Entities.LETTERS_COMMENT_DATA> lst = new List<Entities.LETTERS_COMMENT_DATA>();
            using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
            {
                var q = db.LETTERS_COMMENT_DATA
                            .Where  (m =>
                                     m.TYPE == Entities.CommentTypes.MONTHLY_COMMENTS && 
                                     m.MONTH_PERIOD == Helpers.PeriodUtil.Month &&
                                     m.YEAR_PERIOD == Helpers.PeriodUtil.Year &&
                                    (_channel == null ||
                                    (_channel != null && m.LETTER_ID == _channel)))
                            .Select(m => m).ToList();
                lst = q.Select(m => m).ToList();

            }
            return lst;
        }

        private List<Entities.MonthlyCommentsModel> GetCommentsData()
        {
            List<Entities.MonthlyCommentsModel> lst = new List<Entities.MonthlyCommentsModel>();
            using (Entities.godzillaCommentsEntities db = new Entities.godzillaCommentsEntities())
            {
                var q = db.v_WRK_MONTHLY_COMMENTS_DATA
                        .Select(m => m).ToList();
                lst = q
                        .Where(p => p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        .Select(m => new Entities.MonthlyCommentsModel
                {
                    name = m.NAME,
                    month_market_growth = m.MONTH_MARKET_GROWTH,
                    month_brand_growth = m.MONTH_BRAND_GROWTH,
                    month_market_share = m.MONTH_MARKET_SHARE,
                    month_pt = m.MONTH_PT,
                    ytd_market_growth = m.YTD_MARKET_GROWTH,
                    ytd_brand_growth = m.YTD_BRAND_GROWTH,
                    ytd_market_share = m.YTD_MARKET_SHARE,
                    ytd_pt = m.YTD_PT
                }).ToList();
            }
            return (lst);
        }

        public ActionResult Index()
        {
            ViewBag.Title = CONTROLLER;
            ViewBag.TabUrl = CONTROLLER + "/Index"; 
            ViewBag.NTSMonth = "";
            ViewBag.NTSYTD = "";
            ViewBag.PeriodMonth = new DateTime(DateTime.Now.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            int _year = Helpers.PeriodUtil.Year;
            ViewBag.MonthTitle = ViewBag.PeriodMonth + " " + (_year).ToString() + " Vs " + ViewBag.PeriodMonth + " " + (_year - 1).ToString() + "";
            ViewBag.YTDTitle = "YTD " + ViewBag.PeriodMonth + "  " + (_year).ToString() + " Vs YTD  " + ViewBag.PeriodMonth + " " + (_year - 1).ToString() + "";
            return View();
        }

        //
        // GET: /MonthlyComments/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MonthlyComments/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MonthlyComments/Create

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
        // GET: /MonthlyComments/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MonthlyComments/Edit/5

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
        // GET: /MonthlyComments/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MonthlyComments/Delete/5

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
        #region Constants
        private const string CONTROLLER = "MonthlyComments";
        private const string _PATH = "~/Views/MonthlyComments/";
        private const string MONTHLYCOMMENTS_DATA = _PATH + "_Data.cshtml";
        private const string MONTHLYCOMMENTS_COMMENTS = _PATH + "_Comments.cshtml";
        private const string COMMENTFORMVIEW = "~/Views/Forms/_CommentForm.cshtml";
        private const string COMMENT_CONTAINER = _PATH + "_CommentsContainer.cshtml";
        private const string TITLE_TEXT = "Añadir comentario: ";
        private const string UPDATE_SUCCESS = "Comentario actualizado con éxito.";
        private const string DELETE_SUCCESS = "Comentario eliminado.";
        private const string INSERT_SUCCESS = "Comentario añadido.";
        private const string ERROR_TEXT = "Se ha producido un error al editar/añadir el comentario.";
        private const string CONTAINER_CONTROL = "container_comments";

        #endregion
    }
}
