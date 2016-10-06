using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanDBLibray.Classes;
using StrawmanDBLibray.Entities;
using System.Data.Objects.DataClasses;

namespace StrawmanApp.Controllers
{
    public class CommentsController : Controller
    {
        //
        // GET: /Comments/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Comments(string market, string brand, string channel)
        {
            this.SetViewBag();
            if(!string.IsNullOrEmpty(market) && !string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(channel))
            {
                List<MANAGEMENT_LETTERS_REL> lst = (List<MANAGEMENT_LETTERS_REL>)StrawmanDBLibray.DBLibrary.GetComments(StrawmanDataTables.MANAGEMENT_LETTERS_REL);
                lst = lst.Where(m => m.BRAND == decimal.Parse(brand) && m.MARKET == decimal.Parse(market) && m.CHANNEL == decimal.Parse(channel)).Select(m => m).ToList();
                if (lst.Count > 0)
                {
                    List<LETTERS_COMMENT_DATA> cmt = (List<LETTERS_COMMENT_DATA>)StrawmanDBLibray.DBLibrary.GetCommentsByMasterId(lst.First().MANAGEMENT_LETTER_ID.ToString());
                    //List<LETTERS_COMMENT_DATA> cmt = lst.First().MANAGEMENT_LETTERS_MASTER.LETTERS_COMMENT_DATA.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).Select(m => m).ToList();
                    List<Entities.CommentsModel> model = cmt.Select(m => new Entities.CommentsModel
                    {
                        letter_id = (int?)m.ID,
                        month = (int)m.MONTH_PERIOD,
                        year = (int)m.YEAR_PERIOD,
                        text = m.COMMENT,
                        market = market,
                        brand = brand,
                        channel = channel,
                        date = m.DATE,
                        user = m.USER,
                    }).ToList();
                    return PartialView(COMMENTS_VIEW, model);
                }
                else
                {
                    List<Entities.CommentsModel> model = new List<Entities.CommentsModel>();
                    model.Add(new Entities.CommentsModel
                    {
                        market = market,
                        brand = brand,
                        channel = channel,
                    });
                    return PartialView(COMMENTS_VIEW, model);
                }
            }
            return null;
        }

        [Authorize]
        [HttpGet]
        public ActionResult NewComment(string market, string brand, string channel, string name)
        {
            MANAGEMENT_LETTERS_REL rel = (MANAGEMENT_LETTERS_REL)StrawmanDBLibray.DBLibrary.GetMasterRel(channel, market, brand);
            Models.FormUtilModel.TextArea model = new Models.FormUtilModel.TextArea
            {
                id="comment",
                name = name,
                data_attributes = "new"
            };
            if (rel != null)
            {
                model.comment_id = rel.ID.ToString();
            }
            return PartialView(NEW_COMMENT, model);
        }
        [Authorize]
        [HttpGet]
        public ActionResult EditComment(string id)
        {
            LETTERS_COMMENT_DATA letter = (LETTERS_COMMENT_DATA)StrawmanDBLibray.DBLibrary.GetCommentsById(id);
            MANAGEMENT_LETTERS_REL rel = (MANAGEMENT_LETTERS_REL)StrawmanDBLibray.DBLibrary.GetMasterRelByLetterId(letter.LETTER_ID.ToString());
            MANAGEMENT_LETTERS_MASTER master = (MANAGEMENT_LETTERS_MASTER) StrawmanDBLibray.DBLibrary.GetCommentsMasterById(letter.LETTER_ID.ToString());
            Models.FormUtilModel.TextArea model = new Models.FormUtilModel.TextArea
            {
                id = "comment",
                comment = letter.COMMENT,
                data_attributes = "edit"
            };
            if (letter != null)
            {
                model.letter_id = letter.ID.ToString();
                model.market = rel.MARKET.ToString();
                model.brand = rel.BRAND.ToString();
                model.channel = rel.CHANNEL.ToString();
                model.name = master.NAME;
            }
            return PartialView(NEW_COMMENT, model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult InsertComment(FormCollection data)
        {
            int result = 0;
            switch (data["data_attributes"])
            {
                case "new":
                    //insertar el nuevo comment
                    //EntityCollection<LETTERS_COMMENT_DATA> letters = new EntityCollection<LETTERS_COMMENT_DATA>();
                    //MANAGEMENT_LETTERS_MASTER master = null;
                    //MANAGEMENT_LETTERS_REL rel = null;
                    //if (data["comment_id"] != null)
                    //{
                    //    rel = (MANAGEMENT_LETTERS_REL)StrawmanDBLibray.DBLibrary.GetMasterRelById(data["comment_id"]);
                    //    master = (MANAGEMENT_LETTERS_MASTER)StrawmanDBLibray.DBLibrary.GetCommentsMasterById(rel.MANAGEMENT_LETTER_ID.ToString());
                    //    List<LETTERS_COMMENT_DATA> letters_tmp = (List<LETTERS_COMMENT_DATA>)StrawmanDBLibray.DBLibrary.GetCommentsByMasterId(master.ID.ToString());
                    //    letters_tmp.ForEach(letters.A);// (EntityCollection<LETTERS_COMMENT_DATA>)StrawmanDBLibray.DBLibrary.GetCommentsByMasterId(master.ID.ToString());
                    //}
                    LETTERS_COMMENT_DATA letter = new LETTERS_COMMENT_DATA
                    {
                      COMMENT = data["comment"],
                      USER = Helpers.UserUtils.UserName,
                      YEAR_PERIOD = Helpers.PeriodUtil.Year,
                      MONTH_PERIOD = Helpers.PeriodUtil.Month,
                      TYPE = COMMENT_TYPE,
                      DATE = DateTime.Now,
                    };

                    MANAGEMENT_LETTERS_MASTER master = new MANAGEMENT_LETTERS_MASTER
                    {
                            COUNTRY = Helpers.CountryUtil.Country,
                            NAME = data["name"],
                            CHANNEL = decimal.Parse(data["channel"].ToString()),
                            MAIN_GROUP = 0,
                            GROUP = 0,
                            CHANNEL_GROUP = decimal.Parse(data["channel"].ToString()),

                    };

                    MANAGEMENT_LETTERS_REL rel = new MANAGEMENT_LETTERS_REL
                    {
                        ID = data["comment_id"] != null? decimal.Parse(data["comment_id"].ToString()):0,
                        CHANNEL = decimal.Parse(data["channel"].ToString()),
                        BRAND = decimal.Parse(data["brand"].ToString()),
                        MARKET = decimal.Parse(data["market"].ToString()),
                    };

                    result = StrawmanDBLibray.DBLibrary.SaveComment(letter,master,rel);
                    break;
                case "edit":
                    //editar comment por id
                    if (data["letter_id"] != null)
                    {
                        LETTERS_COMMENT_DATA letter_ed = (LETTERS_COMMENT_DATA)StrawmanDBLibray.DBLibrary.GetCommentsById(data["letter_id"]);
                        letter_ed.COMMENT = data["comment"];
                        letter_ed.DATE = DateTime.Now;
                        result = StrawmanDBLibray.DBLibrary.SaveCommentData(StrawmanDataTables.LETTERS_COMMENT_DATA, letter_ed);
                    }
                    break;
            }
            return new JsonResult() { Data = new { Success = true, Result = result } };
        }
        [Authorize]
        [HttpGet]
        public ActionResult Credentials(string user, string letter_id)
        {
            if (user == Helpers.UserUtils.UserName)
            {
                return PartialView(EDIT_BUTTONS, new ViewDataDictionary { { "letter_id", letter_id }, { "btn_text", "Edit" } });
            }
            return null;
        }

        #region Private Funtions
        private void SetViewBag()
        {
            ViewBag.ModalTitle = Helpers.Texts.COMMENT_TITLE_EN;
            ViewBag.ModalId = MODEL_ID;
            ViewBag.CommentButtonText = Helpers.Texts.COMMENT_BUTTON_TEXT_EN;
            ViewBag.Permissions = Helpers.UserUtils.Permissions.GetPermissionsFor(Helpers.StrawmanViews.BOY.id);
        }
        #endregion
        #region Constants
        private const string PATH = "~/Views/";
        private const string CONTROLLER = "Comments";

        private const string COMMENTS_VIEW = PATH + CONTROLLER + "/_Comments.cshtml";
        private const string EDIT_BUTTONS = PATH + CONTROLLER + "/_Edit_Buttons.cshtml";
        private const string NEW_COMMENT = PATH + CONTROLLER + "/_EditComment.cshtml";
        private const string MODEL_ID = "CommentsModel";

        private const string COMMENT_TYPE = "BOY_COMMENTS";
        #endregion
    }
}
