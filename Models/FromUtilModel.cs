using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Models
{
    public class FormUtilModel
    {
        public class ButtonsGroup
        {
            public List<FormUtilModel.Button> Buttons { get; set; }
            public string label_text { get; set; }
            public int? _float { get; set; }

            public static int left { get { return -1; } }
            public static int right { get { return 1; } }
        }

        public class Button
        {
            public string success { get; set; }
            public string text { get; set; }
            public string target { get; set; }
            public string variable { get; set; }
            public string controller { get; set; }
            public string type { get; set; }
            public string _class {get;set;}
            public string dataAttr { get; set; }
        }

        public static class InputTypes
        {
            public const string TEXT = "text";
            public const string CHECKBOX = "checkbox";
            public const string HIDDEN = "hidden";
            public const string SUBMIT = "submit";
            public const string BUTTON = "button";
            public const string SELECT = "select";
            public const string SELECTINPUT = "select_input";

        }
        public class InputModel : FormDefault
        {
            public string type { get; set; }
            public string defalut_value { get; set; }
            public bool is_selected { get; set; }

            public string view
            {
                get
                {
                    switch (this.type)
                    {
                        case InputTypes.CHECKBOX:
                        case InputTypes.HIDDEN:
                        case InputTypes.TEXT:
                            return ElementsView.INPUT_VIEW;
                        default:
                            return ElementsView.SUBMIT_VIEW;
                    }
                }
            }
        }
        public class SelectInputModel : FormDefault
        {
            public SelectModel select { get; set; }
            public InputModel input { get; set; }
            public InputModel hidden { get; set; }
            public string view { get { return ElementsView.SELECT_INPUT_VIEW; } }
        }

        public class SelectModel : FormDefault
        {
            public List<SelectListItem> options { get; set; }

            public string view { get { return ElementsView.SELECT_VIEW; } }
        }
        public class OptionModel : FormDefault
        {
            public string value { get; set; }
            public string text { get; set; }
        }
        public class TextArea : FormDefault
        {
            public string value { get; set; }
            public string market { get; set; }
            public string brand { get; set; }
            public string channel { get; set; }
            public string group { get; set; }
            [AllowHtml]
            public string comment { get; set; }

            public int rows { get; set; }
            public int columns { get; set; }

            public string letter_id { get; set; }
            public string comment_id { get; set; }

        }
    }
    public class FormModel : Deletable
    {
        public FormModel()
        {
            this.objects = new List<FormElement>();
        }
        public List<FormElement> objects { get; set; }

        public string view { get { return ElementsView.FORM_VIEW; } }
        public string table { get { return ElementsView.TABLE_FORM_VIEW; } }
        public string header { get { return ElementsView.HEADER_VIEW; } }
        public string modal_view { get { return ElementsView.MODAL_VIEW; } }
        public string action { get; set; }
        public string table_action { get; set; }
        public string controller { get; set; }

        public object table_controller { get; set; }



        public object refresh { get; set; }

        public string form_id { get; set; }
    }

    public class FormElement
    {
        public string partial_view { get; set; }
        public object model { get; set; }

        public int column { get; set; }

        public int row { get; set; }
    }

    public class FormDefault : FormComponents
    {
        public string id { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public string data_attributes { get; set; }
        public string data_target { get; set; }
        public string classes { get; set; }
        public bool is_required { get; set; }
        public string _for {get;set;}
        public string disabled { get; set; }
        public string source { get; set; }
    }

    public class FormComponents
    {
        public static string _id { get; set; }
        public static string _name { get; set; }
        public static string _description { get; set; }
        public static string _hidden_type { get; set; }
        public static string _table_id { get; set; }
        public static string _type_id { get; set; }
        public static string _hidden_table { get; set; }
        public static string _channel { get; set; }
        public static string _market { get; set; }
        public static string _brand { get; set; }
        public static string _group { get; set; }
        public static string _group_name { get; set; }
        public static string _nts { get; set; }
        public static string _rosetta { get; set; }
        public static string _market_name { get; set; }
        public static string _brand_name { get; set; }
        public static string _nts_config { get; set; }
        public static string _rosetta_config { get; set; }
        public static string _order { get; set; }

        public Dictionary<string, string> all = new Dictionary<string, string>()
        { 
            {Classes.Default.Attributes.INPUT_ID_ID, _id},
            {Classes.Default.Attributes.INPUT_NAME_ID, _name},
            {Classes.Default.Attributes.INPUT_DESCRIPTION_ID, _description},
            {Classes.Default.Attributes.INPUT_TABLE_ID, _table_id},
            {Classes.Default.Attributes.INPUT_TYPE_ID, _type_id},
            {Classes.Default.Attributes.HIDDEN_TYPE, _hidden_type},
            {Classes.Default.Attributes.HIDDEN_TABLE, _hidden_table},
            {Classes.Default.Attributes.INPUT_CHANNEL_ID, _channel},
            {Classes.Default.Attributes.CHANNEL_ID, _channel},
            {Classes.Default.Attributes.GROUP_ID, _group},
            {Classes.Default.Attributes.MARKET_ID, _market},
            {Classes.Default.Attributes.BRAND_ID, _brand},
            {Classes.Default.Attributes.BRAND_NAME_ID, _brand_name},
            {Classes.Default.Attributes.MARKET_NAME_ID, _market_name},
            {Classes.Default.Attributes.GROUP_NAME_ID, _group_name},
            {Classes.Default.Attributes.ORDER_ID, _order},
            {Classes.Default.Attributes.NTS_CONFIG_ID, _nts_config},
            {Classes.Default.Attributes.NTS_ID, _nts},
            {Classes.Default.Attributes.ROSSETA_ID, _rosetta},
            {Classes.Default.Attributes.ROSETTA_CONFIG_ID, _rosetta_config}
        };
    }
    public static class ElementsView
    {
        private static string _PATH = "~/Views/";
        private static string _FORMS_PATH = "Forms/";
        private static string _TABLES_PATH = "Tables/";

        private static string _COMPONENTS_PATH = _PATH + _FORMS_PATH + "Components/";
        private static string _COMPONENTS_TABLE_PATH = _PATH + _TABLES_PATH + "Components/";

        public static string FORM_VIEW = _COMPONENTS_PATH + "_FormSTD.cshtml";
        public static string INPUT_VIEW = _COMPONENTS_PATH + "_InputView.cshtml";
        public static string SELECT_VIEW = _COMPONENTS_PATH + "_SelectView.cshtml";
        public static string SUBMIT_VIEW = _COMPONENTS_PATH + "_Submit.cshtml";
        public static string MODAL_VIEW = _COMPONENTS_PATH + "_ModalContent.cshtml";
        public static string SELECT_INPUT_VIEW = _COMPONENTS_PATH + "_SelectInput.cshtml";
        public static string TABLE_FORM_VIEW = _PATH + _TABLES_PATH + "_FormTable.cshtml";
        public static string HEADER_VIEW = _COMPONENTS_TABLE_PATH + "_Header.cshtml";

    }

    public partial class Deletable
    {
        public string delete_controller { get; set; }
        public string delete_action { get; set; }
    }
}