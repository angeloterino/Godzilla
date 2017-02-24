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
        public ActionResult Comments(string market, string brand, string channel, string group, string id, string source)
        {
            this.SetViewBag();
            if (!string.IsNullOrEmpty(market) && !string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(channel))
            {
                List<MANAGEMENT_LETTERS_REL> lst = (List<MANAGEMENT_LETTERS_REL>)StrawmanDBLibray.DBLibrary.GetComments(StrawmanDataTables.MANAGEMENT_LETTERS_REL);
                lst = lst.Where(m => m.BRAND == decimal.Parse(brand) && m.MARKET == decimal.Parse(market) && m.CHANNEL == decimal.Parse(channel)).Select(m => m).ToList();
                if (lst.Count > 0)
                {
                    List<LETTERS_COMMENT_DATA> cmt = (List<LETTERS_COMMENT_DATA>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.LETTERS_COMMENT_DATA,false);
                    List<Entities.CommentsModel> model = cmt.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        .Join(lst, l=>new {_id = l.LETTER_ID},c=>new{_id =(decimal?) c.MANAGEMENT_LETTER_ID},(l,c)=>new{l=l}).AsEnumerable()
                        .Select(m => new Entities.CommentsModel
                    {
                        letter_id = (int?)m.l.ID,
                        month = (int)m.l.MONTH_PERIOD,
                        year = (int)m.l.YEAR_PERIOD,
                        text = m.l.COMMENT,
                        market = market,
                        brand = brand,
                        channel = channel,
                        group = group,
                        date = m.l.DATE,
                        user = m.l.USER,
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
                        group = group
                    });
                    return PartialView(COMMENTS_VIEW, model);
                }
            }
            else
            {
                //Comentarios Montly Comments y Managemet Letters
                switch (source)
                {
                    case Entities.CommentTypes.MONTHLY_COMMENTS:
                        List<LETTERS_COMMENT_DATA> cmt = (List<LETTERS_COMMENT_DATA>)StrawmanDBLibray.DBLibrary.GetCommentsByMasterId(id);
                        //List<LETTERS_COMMENT_DATA> cmt = lst.First().MANAGEMENT_LETTERS_MASTER.LETTERS_COMMENT_DATA.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).Select(m => m).ToList();
                        List<Entities.CommentsModel> model = cmt
                            .Where(m=>m.TYPE == source && m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                            .Select(m => new Entities.CommentsModel
                        {
                            letter_id = (int?)m.ID,
                            month = (int)m.MONTH_PERIOD,
                            year = (int)m.YEAR_PERIOD,
                            text = m.COMMENT,
                            market = market,
                            brand = brand,
                            channel = channel,
                            group = group,
                            date = m.DATE,
                            user = m.USER,
                        }).ToList();
                        return PartialView(COMMENTS_VIEW, model);

                    case Entities.CommentTypes.MANAGEMENT_LETTER:
                        List<LETTERS_COMMENT_DATA> cml = (List<LETTERS_COMMENT_DATA>)StrawmanDBLibray.DBLibrary.GetCommentsByMasterRelId(id);
                        //List<LETTERS_COMMENT_DATA> cmt = lst.First().MANAGEMENT_LETTERS_MASTER.LETTERS_COMMENT_DATA.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).Select(m => m).ToList();
                        if (cml == null)
                        {
                            List<Entities.CommentsModel> mmodel = new List<Entities.CommentsModel>();
                            mmodel.Add(new Entities.CommentsModel
                            {
                                market = market,
                                brand = brand,
                                channel = channel,
                                group = group,
                            });
                            return PartialView(COMMENTS_VIEW, mmodel);
                        }
                        List<Entities.CommentsModel> mlmodel = cml
                            .Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                            .Select(m => new Entities.CommentsModel
                        {
                            letter_id = (int?)m.ID,
                            month = (int)m.MONTH_PERIOD,
                            year = (int)m.YEAR_PERIOD,
                            text = m.COMMENT,
                            market = market,
                            brand = brand,
                            channel = channel,
                            group = group,
                            date = m.DATE,
                            user = m.USER,
                        }).ToList();
                        return PartialView(COMMENTS_VIEW, mlmodel);
                }
            }
            return null;
        }

        [Authorize]
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult NewComment(string market, string brand, string channel, string group, string name, string source, string id)
        {
            Models.FormUtilModel.TextArea model = new Models.FormUtilModel.TextArea
            {
                id="comment",
                name = name,
                data_attributes = "new",
                source = source
            };
            switch(source){
                case Entities.CommentTypes.MANAGEMENT_LETTER:
                case Entities.CommentTypes.MONTHLY_COMMENTS:
                    model.letter_id = id;
                    break;
                default:
                MANAGEMENT_LETTERS_REL rel = (MANAGEMENT_LETTERS_REL)StrawmanDBLibray.DBLibrary.GetMasterRel(channel, market, brand);
                
                if (rel != null)
                {
                    model.comment_id = rel.ID.ToString();
                }
                break;
            }
            return PartialView(NEW_COMMENT, model);
        }
        [Authorize]
        [HttpGet]
        public ActionResult EditComment(string id)
        {
            LETTERS_COMMENT_DATA letter = (LETTERS_COMMENT_DATA)StrawmanDBLibray.DBLibrary.GetCommentsById(id);
            Models.FormUtilModel.TextArea model = new Models.FormUtilModel.TextArea
            {
                id = "comment",
                comment = letter.COMMENT,
                data_attributes = "edit",
                letter_id = letter.LETTER_ID.ToString(),
                comment_id = id,
                source = letter.TYPE
            };
            if (letter != null)
            {
                switch (letter.TYPE)
                {
                    default:
                        MANAGEMENT_LETTERS_REL rel = (MANAGEMENT_LETTERS_REL)StrawmanDBLibray.DBLibrary.GetMasterRelByLetterId(letter.LETTER_ID.ToString());
                        MANAGEMENT_LETTERS_MASTER master = (MANAGEMENT_LETTERS_MASTER)StrawmanDBLibray.DBLibrary.GetCommentsMasterById(letter.LETTER_ID.ToString());

                        model.letter_id = letter.ID.ToString();
                        model.market = rel.MARKET.ToString();
                        model.brand = rel.BRAND.ToString();
                        model.channel = rel.CHANNEL.ToString();
                        model.name = master.NAME;
                        break;
                    case Entities.CommentTypes.MANAGEMENT_LETTER:
                    case Entities.CommentTypes.MONTHLY_COMMENTS:
                        model.name = letter.TYPE;
                        break;
                }
            }
            return PartialView(NEW_COMMENT, model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult InsertComment(Models.FormUtilModel.TextArea data)
        {
            if (Helpers.UserUtils.UserName == null) Response.Redirect("~/Account/LogIn?returnUrl=");
            int result = 0;
            string type = string.IsNullOrEmpty(data.comment) || data.comment.Trim().Length == 0?"delete":data.data_attributes;
            string source = data.source;

            switch (type)
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
                    switch (source)
                    {
                        default:
                            //Comentario
                            LETTERS_COMMENT_DATA letter = new LETTERS_COMMENT_DATA
                            {
                                COMMENT = data.comment,
                                USER = Helpers.UserUtils.UserName,
                                YEAR_PERIOD = Helpers.PeriodUtil.Year,
                                MONTH_PERIOD = Helpers.PeriodUtil.Month,
                                TYPE = COMMENT_TYPE,
                                DATE = DateTime.Now,
                            };

                            //Maestro Letters
                            MANAGEMENT_LETTERS_MASTER master = new MANAGEMENT_LETTERS_MASTER
                            {
                                COUNTRY = Helpers.CountryUtil.Country,
                                NAME = data.name,
                                CHANNEL = decimal.Parse(data.channel.ToString()),
                                CHANNEL_GROUP = decimal.Parse(data.channel.ToString()),

                            };

                            //Relacción Maestro Letters - Commentario
                            MANAGEMENT_LETTERS_REL rel = new MANAGEMENT_LETTERS_REL
                            {
                                ID = data.comment_id != null ? decimal.Parse(data.comment_id.ToString()) : 0,
                                CHANNEL = decimal.Parse(data.channel.ToString()),
                                BRAND = decimal.Parse(data.brand.ToString()),
                                MARKET = decimal.Parse(data.market.ToString()),
                            };
                            decimal tmp_group;
                            MANAGEMENT_LETTERS_MASTER_REL msrel = new MANAGEMENT_LETTERS_MASTER_REL
                            {
                                CHANNEL = decimal.Parse(data.channel.ToString()),
                                BRAND = decimal.Parse(data.brand.ToString()),
                                MARKET = decimal.Parse(data.market.ToString()),
                                GROUP = decimal.TryParse(data.group, out tmp_group)? tmp_group: default(decimal?)
                            };
                            result = StrawmanDBLibray.DBLibrary.SaveComment(letter, master, rel, msrel);
                        break;
                        case Entities.CommentTypes.MANAGEMENT_LETTER:
                            //Relación Maestro BOY-Maestro Letters
                            MANAGEMENT_LETTERS_MASTER_REL ms_rel = ((List<StrawmanDBLibray.Entities.MANAGEMENT_LETTERS_MASTER_REL>)Helpers.StrawmanDBLibrayData.Get(StrawmanDataTables.MANAGEMENT_LETTERS_MASTER_REL))
                                .Where(m => m.MASTER_ID == decimal.Parse(data.letter_id.ToString())).FirstOrDefault();
                            //Relación Maestro Letters - Comentario
                            MANAGEMENT_LETTERS_REL m_rel = new MANAGEMENT_LETTERS_REL
                            {
                                ID = data.letter_id != null ? decimal.Parse(data.letter_id.ToString()) : 0,
                                CHANNEL = (decimal)ms_rel.CHANNEL,
                                BRAND = (decimal)ms_rel.BRAND,
                                MARKET = (decimal)ms_rel.MARKET,
                            };
                            //Comentario
                            LETTERS_COMMENT_DATA m_letter = new LETTERS_COMMENT_DATA
                            {
                                LETTER_ID = decimal.Parse(data.letter_id),
                                COMMENT = data.comment,
                                USER = Helpers.UserUtils.UserName,
                                YEAR_PERIOD = Helpers.PeriodUtil.Year,
                                MONTH_PERIOD = Helpers.PeriodUtil.Month,
                                TYPE = source,
                                DATE = DateTime.Now,
                            };
                            result = StrawmanDBLibray.DBLibrary.SaveComment(m_letter, null, m_rel, ms_rel);

                            break;
                    }
                    break;
                case "delete":
                case "edit":
                    //editar comment por id
                    if (data.letter_id != null)
                    {
                        string _id = data.letter_id;
                        switch (source)
                        {
                            case Entities.CommentTypes.MONTHLY_COMMENTS:
                            case Entities.CommentTypes.MANAGEMENT_LETTER:
                                _id = data.comment_id;
                                break;
                        }
                        LETTERS_COMMENT_DATA letter_ed = (LETTERS_COMMENT_DATA)StrawmanDBLibray.DBLibrary.GetCommentsById(_id);
                        if (type == "edit")
                        {
                            letter_ed.COMMENT = data.comment;
                            letter_ed.DATE = DateTime.Now;
                            result = StrawmanDBLibray.DBLibrary.SaveCommentData(StrawmanDataTables.LETTERS_COMMENT_DATA, letter_ed);
                        }
                        else
                        {
                            result = StrawmanDBLibray.DBLibrary.RemoveComment(StrawmanDataTables.LETTERS_COMMENT_DATA, letter_ed);
                        }
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

        [ChildActionOnly]
        public ActionResult GetCommentsButton(ViewDataDictionary data)
        {
            data.Add(new KeyValuePair<string, object>("title", "Monthly Comments"));
            List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS> rel = (List<StrawmanDBLibray.Entities.v_WRK_MANAGEMENT_LETTERS>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.v_WRK_MANAGEMENT_LETTERS);
            decimal? group = data.Keys.Contains("group")?(decimal)data["group"]:default(decimal?);
            if (!rel.Exists(m => (group == null && m.MARKET == (decimal)data["market"] && m.BRAND == (decimal)data["brand"]) || (group != null && m.GROUP == group)))
                return null;
            return PartialView(COMMENTS_BUTTON,data);
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
        private const string COMMENTS_BUTTON = PATH + CONTROLLER + "/_CommentsButton.cshtml";
        private const string MODEL_ID = "CommentsModel";

        private const string COMMENT_TYPE = "BOY_COMMENTS";
        #endregion
    }
}
