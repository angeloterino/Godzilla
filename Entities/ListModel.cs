using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StrawmanApp.Entities
{
    public class ListModel
    {
        public IEnumerable<int> _id { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> brand_master { get; set; }
        public IEnumerable<GodzillaEntity.BRAND_MASTER> datamaster { get; set; }
        public string selectedId { get; set; }
    }
    
    public class GroupListModel
    {
        public IEnumerable<int> _id { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> group_master { get; set; }
        public IEnumerable<StrawmanDBLibray.Entities.GROUP_MASTER> datamaster { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> group_types { get; set; }
        public IEnumerable<StrawmanDBLibray.Entities.GROUP_TYPES> datatypes { get; set; }
        public string selectedId { get; set; }
    }

    public class NewGroupModel
    {
        public IEnumerable<int> _id { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> typelist { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> levellist { get; set; }
        [Required(ErrorMessage = "Nombre es un campo obligatorio.")]
        [Remote("groupNameExist", "Forms", HttpMethod = "POST", ErrorMessage = "El nombre del grupo ya existe en la tabla.")]
        public string name { get; set; }
        public int type { get; set; }
        public int level { get; set; }
        public string selectedId { get; set; }

        public List<GodzillaEntity.GROUP_MASTER> editcoll { get; set; }
    }
    public class EditGroupModel
    {
        public IEnumerable<int> _id { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> typelist { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> levellist { get; set; }
        [Required(ErrorMessage = "* Name is mandatory.")]        
        public string name { get; set; }
        public int type { get; set; }
        public int level { get; set; }
        public string selectedId { get; set; }

        public List<GodzillaEntity.GROUP_MASTER> editcoll { get; set; }
    }
    public class EditKPIModel
    {
        public IEnumerable<int> _id { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> typelist { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> levellist { get; set; }
        [Required(ErrorMessage = "Nombre es un campo obligatorio.")]
        public string name { get; set; }
        public int type { get; set; }
        public int level { get; set; }
        public string selectedId { get; set; }
        public string selectedColumn { get; set; }

        public List<KPIConfig > editcoll { get; set; }
        public List<KPIConfig> itemslist { get; set; }
        public List<WrkBoyData> additemslist { get; set; }
        public List<KPISelected> selecteditems { get; set; }
    }
    public class KPISelected
    {
        public int _id { get; set; }
        public bool _selected { get; set; }
    }
    public class KPIConfig:StrawmanDBLibray.Entities.KPI_CONFIG
    {
        public decimal? ORDER {get;set;}
        public string NAME  {get;set;}
        public decimal? PARENT { get; set; }
    }
    public class WrkBoyData : StrawmanDBLibray.Entities.WRK_BOY_DATA
    {
        public string CHANNEL_NAME { get; set; }
    }
    public class EditBOY
    {
        public decimal? channel { get; set; }
        public decimal? brand { get; set; }
        public string brand_name { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? market { get; set; }

        [Required(ErrorMessage = "Dato obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double? market_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double? market_col2 { get; set; }
        [Required(ErrorMessage = "Dato obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public decimal? market_pc { get; set; }

        [Required(ErrorMessage = "Dato obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double? sellin_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double? sellin_col2 { get; set; }
        [Required(ErrorMessage = "Dato obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public decimal? sellin_pc { get; set; }

        [Required(ErrorMessage = "Dato obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double? sellout_col1 { get; set; }
        [Required(ErrorMessage = "Dato obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double? sellout_col2 { get; set; }
        [Required(ErrorMessage = "Dato obligatorio.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public decimal? sellout_pc { get; set; }
 
        public int? market_id { get; set; }
        public int? sellin_id { get; set; }
        public int? sellout_id { get; set; }

        public double? market_pc_f { get; set; }
        public double? sellin_pc_f { get; set; }
        public double? sellout_pc_f { get; set; }
    }
    public class EditBOYModel
    {
        public EditBOY INT { get; set; }
        public EditBOY PBP { get; set; }
        public EditBOY BTG { get; set; }
        public decimal? _channel { get; set; }
        public decimal? _brand { get; set; }        
        public decimal? _market { get; set; }
        public string FormType { get; set; }
    }

    public class ShareBoardModel: StrawmanBasicModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0" )]
        public decimal? month_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? month_col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? month_col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}", NullDisplayText = "0")]
        public decimal? month_col4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? month_col5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? month_col6 { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? ytd_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? ytd_col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? ytd_col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}", NullDisplayText = "0")]
        public decimal? ytd_col4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? ytd_col5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? ytd_col6 { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? mat_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? mat_col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? mat_col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}", NullDisplayText = "0")]
        public decimal? mat_col4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? mat_col5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? mat_col6 { get; set; }


    }

    public class StrawmanBasicModel
    {
        public decimal id { get; set; }
        public decimal? brand { get; set; }
        public decimal? market { get; set; }
        public decimal? channel { get; set; }
        public decimal? order { get; set; }
        public decimal? group { get; set; }
        public decimal? parent { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}", NullDisplayText = "0")]
        public decimal? col4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? col5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? col6 { get; set; }

        public decimal? market_col1 { get; set; }
        public decimal? market_col2 { get; set; }
        public decimal? brand_col1 { get; set; }
        public decimal? brand_col2 { get; set; }

    }
    public class ExcelLoader: StrawmanDBLibray.Classes.ExcelLoader
    {
    }

    public class CalcData
    {
        public string data { get; set; }
        public int year_period { get; set; }
        public int month_period { get; set; }
        public int BRAND { get; set; }
        public int MARKET { get; set; }

        public int GROUP { get; set; }
    }
    public class KpiModel
    {
        private bool _editable = false;
        public string NAME { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? COL1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? COL2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? COL3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? COLPC1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? COLPC2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? COLPC3 { get; set; }

        public bool isHead { get; set; }
        public bool isPC { get; set; }

        public bool data_editable { get { return this._editable; } set { this._editable = value; } }

        public string title { get; set; }

        public string titleRowStyle { get; set; }

        public string headRowStyle { get; set; }

        public headComponent[] head { get; set; }

        public class headComponent
        {
            public string text { get; set; }
            public string colStyle { get; set; }
        }

        public decimal? GROUP { get; set; }

        public decimal? KPI { get; set; }

        public string KPI_COLUMN { get; set; }

        public decimal? ID { get; set; }
    }
    public class CommentsModel
    {
        public string master_group_label { get; set; }
        public string master_group_selected { get; set; }
        public IEnumerable<SelectListItem> master_group_list { get; set; }

        public string group_label { get; set; }
        public string group_selected { get; set; }
        public IEnumerable<SelectListItem> group_list { get; set; }

        public string channel_label { get; set; }
        public string channel_selected { get; set; }
        public IEnumerable<SelectListItem> channel_list { get; set; }

        public string user { get; set; }
        public bool permissions { get; set; }
        [DisplayFormat(NullDisplayText="No comments found")]
        public string text { get; set; }

        public int? letter_id { get; set; }

        public int year { get; set; }
        public int month { get; set; }
        /// <summary>
        /// Tipo de comentarion (Management Letter o Monthly Comment)
        /// </summary>
        public string type { get; set; }

        public bool? del_letter { get; set; }

        public int? id { get; set; }
        /// <summary>
        /// Comentario a mostrar si no se produce error al insertar/editar/borar
        /// </summary>
        public string success_text { get; set; }
        /// <summary>
        /// Comentario a mostrar si se produce error al insertar/editar/borar
        /// </summary>
        public string error_text { get; set; }
        /// <summary>
        /// Control que contiene el formulario de edición.
        /// </summary>
        public string container { get; set; }

        public string market { get; set; }
        public string brand { get; set; }
        public string channel { get; set; }
        public string group { get; set; }
        public DateTime? date { get; set; }
    }

    public static class CommentTypes {
        public const string MANAGEMENT_LETTER ="MANAGEMENT_LETTER";
        public const string MONTHLY_COMMENTS = "MONTHLY_COMMENTS";
        public const string BOY_COMMENTS = "BOY_COMMENTS";
    }

    public class CommentType {
        public override string ToString()
        {
            return base.ToString();
        } 
    }
    public class ManagementLetterModel
    {
        public string name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? ytd_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? ytd_col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? ytd_col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", NullDisplayText = "0")]
        public decimal? ytd_col4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? latest_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? latest_col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? latest_col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", NullDisplayText = "0")]
        public decimal? latest_col4 { get; set; }
    }

    public class MonthlyCommentsModel
    {
        public string name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public int? ytd_col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? month_market_growth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? month_brand_growth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? month_market_share { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? month_pt { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? ytd_market_growth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? ytd_brand_growth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? ytd_market_share { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? ytd_pt { get; set; }
    }
    public class BrandCategorisationModel
    {
        public string brand_name { get; set; }

        public decimal? market { get; set; }

        public decimal? brand { get; set; }

        public decimal id { get; set; }

        public decimal type { get; set; }

        public string name { get; set; }

        public string group_name { get; set; }

        public decimal? order { get; set; }

        public decimal group_level { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? act { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? py { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? ytd_share_vspy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? ytd_category_vspy { get; set; }
    }
    public class PeriodUtilModel
    {
        public IEnumerable<SelectListItem> month_list { get; set; }

        public decimal? selected_year_period { get; set; }

        public decimal? selected_month_period { get; set; }

        public IEnumerable<SelectListItem> year_list { get; set; }

        public string month_text { get; set; }

        public string year_text { get; set; }
    }

    public class StrwmanPreviewBrandData : Entities.MasterTable.STRWM_BRAND_DATA
    {
        public decimal? CHANNEL { get; set; }
    }
    public class StrwmanPreviewMarketData : Entities.MasterTable.STRWM_MARKET_DATA
    {
        public decimal? CHANNEL { get; set; }
    }
}