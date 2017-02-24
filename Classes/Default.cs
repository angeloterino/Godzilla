using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StrawmanApp.Classes.Extensions;
namespace StrawmanApp.Classes
{
    public class Default
    {
        public static class ViewsPaths
        {
            private static string _VIEW_PATH = "~/Views";
            private static string _SHARED_PATH = "/Shared";

            public static string SCRIPTS = "~/Scripts";
            public static string LAYOUT = _VIEW_PATH + _SHARED_PATH + "/_Layout.cshtml";
            public static string SCRIPT_TAG = _VIEW_PATH + _SHARED_PATH + "/_ScriptTag.cshtml";

            public static string COMMON_JS = "/common.js";
        }

        public static class TitlePages
        {
            public static string FORM_TITLE = "Form";
        }

        public static class ActionNames
        {
            public static string DEFAULT_FORM = "DefaultForm";
            public static string UPLOAD_FORM_DATA = "UploadFormData";
            public static string GET_DATA_TABLE = "GetDataTable";
            public static string GET_SELECT_TABLES = "GetSelectTables";
            public static string DELETE_RECORD = "DeleteRecord";
            public static string REFRESH = "GetFormByTable";
        }

        public static class ControllerNames
        {
            public static string FORMS_CONTROLLER = "Forms";
            public static string UPLOAD_CONTROLLER = "Upload";
            public static string TABLES_CONTROLLER = "Tables";
        }

        public static class Labels
        {
            public static string NAME = "Name";
            public static string DESCRIPTION = "Description";
            public static string TABLE = "Table";
            public static string TYPE = "Type";
            public static string CATEGORY_TYPE = "Category Type";
        }
        public static class Attributes
        {
            //Los nombres de los atributos son el texto en mayúsculas más _ID
            //No se puede concatenar más de dos palabras
            //Para cada attributo tipo DUMMY_NAME_ID debe existir un attributo DUMMY_ID sin el name
            public const string INPUT_NAME_ID = "name";
            public const string INPUT_DESCRIPTION_ID = "description";
            public const string INPUT_ID_ID = "id";
            public const string INPUT_TYPE_ID = "type_id";
            public const string INPUT_TABLE_ID = "table_id";
            public const string INPUT_CATEGORY_TYPE_ID = "category_type";
            public const string INPUT_CHANNEL_ID = "channel_id";
            public const string MARKET_NAME_ID = "market_name";
            public const string BRAND_NAME_ID = "brand_name";
            public const string GROUP_NAME_ID = "group_name";
            public const string CHANNEL_ID = "channel";
            public const string NTS_CONFIG_ID = "nts_config";
            public const string NTS_ID = "nts";
            public const string ROSETTA_CONFIG_ID = "rosetta_config";
            public const string ROSSETA_ID = "rosetta";
            public const string MARKET_ID = "market";
            public const string BRAND_ID = "brand";
            public const string GROUP_ID ="group";
            public const string MARKET_CONFIG_ID = "market_config";
            public const string MARKET_GROUP_CONFIG_ID = "market_group_config";
            public const string MARKET_SUPERGROUP_CONFIG_ID = "market_supergroup_config";
            public const string MARKET_CHANNEL_CONFIG_ID = "market_channel_config";
            public const string MARKET_KEYBRANDS_CONFIG_ID = "market_keybrands_config";
            public const string MARKET_FRANCHISE_CONFIG_ID = "market_franchise_config";
            public const string BRAND_CONFIG_ID = "brand_config";
            public const string BRAND_GROUP_CONFIG_ID = "brand_group_config";
            public const string BRAND_SUPERGROUP_CONFIG_ID = "brand_supergroup_config";
            public const string BRAND_CHANNEL_CONFIG_ID = "brand_channel_config";
            public const string BRAND_KEYBRANDS_CONFIG_ID = "brand_keybrands_config";
            public const string BRAND_FRANCHISE_CONFIG_ID = "brand_franchise_config";
            public const string ORDER_ID = "order";

            public const string HIDDEN_TYPE = "form_type";
            public const string HIDDEN_NEW = "new";
            public const string HIDDEN_EDIT = "edit";
            public const string HIDDEN_TABLE = "table";

            public const string SUMBIT_VALUE = "Save";

            public const string SELECT_TABLE_ID = "tables_id";

            public static class Data
            {
                public static string SELECT_TABLE_DATA_ATTRIBUTES = "select_table";
            }

            public static class ALL
            {
                public static string INPUT_NAME_ID = Attributes.INPUT_NAME_ID.formated();
                public static string INPUT_DESCRIPTION_ID = Attributes.INPUT_DESCRIPTION_ID.formated();
                public static string INPUT_ID_ID = Attributes.INPUT_ID_ID.formated();
                public static string INPUT_TYPE_ID = Attributes.INPUT_TYPE_ID.formated();
                public static string INPUT_TABLE_ID = Attributes.INPUT_TABLE_ID.formated();

                public static string INPUT_CHANNEL_ID = Attributes.INPUT_CHANNEL_ID.formated();
                
                public static string HIDDEN_TYPE = Attributes.HIDDEN_TYPE.formated();
                public static string HIDDEN_NEW = Attributes.HIDDEN_NEW.formated();
                public static string HIDDEN_EDIT = Attributes.HIDDEN_EDIT.formated();
                public static string HIDDEN_TABLE = Attributes.HIDDEN_TABLE.formated();

                public static string GROUP_ID = Attributes.GROUP_ID.formated();
                public static string MARKET_ID = Attributes.MARKET_ID.formated();

                public static string MARKET_NAME_ID = Attributes.MARKET_NAME_ID.formated();
                public static string BRAND_NAME_ID = Attributes.BRAND_NAME_ID.formated();
                public static string GROUP_NAME_ID = Attributes.GROUP_NAME_ID.formated();
                public static string CHANNEL_ID = Attributes.CHANNEL_ID.formated();
                public static string BRAND_ID = Attributes.BRAND_ID.formated();
                public static string NTS_CONFIG_ID = Attributes.NTS_CONFIG_ID.formated();
                public static string ROSETTA_CONFIG_ID = Attributes.ROSETTA_CONFIG_ID.formated();

                public static string MARKET_CONFIG_ID = Attributes.MARKET_CONFIG_ID.formated();
                public static string MARKET_GROUP_CONFIG_ID = Attributes.MARKET_GROUP_CONFIG_ID.formated();
                public static string MARKET_SUPERGROUP_CONFIG_ID = Attributes.MARKET_SUPERGROUP_CONFIG_ID.formated();
                public static string MARKET_CHANNEL_CONFIG_ID = Attributes.MARKET_CHANNEL_CONFIG_ID.formated();
                public static string MARKET_FRANCHISE_CONFIG_ID = Attributes.MARKET_FRANCHISE_CONFIG_ID.formated();
                public static string MARKET_KEYBRANDS_CONFIG_ID = Attributes.MARKET_KEYBRANDS_CONFIG_ID.formated();
                public static string BRAND_CONFIG_ID = Attributes.BRAND_CONFIG_ID.formated();
                public static string BRAND_GROUP_CONFIG_ID = Attributes.BRAND_GROUP_CONFIG_ID.formated();
                public static string BRAND_SUPERGROUP_CONFIG_ID = Attributes.BRAND_SUPERGROUP_CONFIG_ID.formated();
                public static string BRAND_CHANNEL_CONFIG_ID = Attributes.BRAND_CHANNEL_CONFIG_ID.formated();
                public static string BRAND_FRANCHISE_CONFIG_ID = Attributes.BRAND_FRANCHISE_CONFIG_ID.formated();
                public static string BRAND_KEYBRANDS_CONFIG_ID = Attributes.BRAND_KEYBRANDS_CONFIG_ID.formated();
                public static string ORDER_ID = Attributes.ORDER_ID.formated();
            }

        }

        public static class FormElements
        {
            public const string SELECT = "SELECT";
            public const string INPUT = "INPUT";
            public const string HIDDEN = "HIDDEN";
            public const string CHECKBOX = "CHECKBOX";
            public const string SUBMIT = "SUBMIT";
            public const string BUTTON = "BUTTON";
        }

        public static class Variables
        {
            public const string STRAWMAN_COLORS = "STRAWMAN_COLORS";
            public const string STRAWMAN_CHANNELS_COLORS = "STRAWMAN_CHANNELS_COLORS";
            public const string WC_CHANNELS = "WC_CHANNELS";
        }

        public static class PartialViews
        {
            public const string TR_STYLE = "_TrStyle";
        }

    }
}
namespace StrawmanApp.Classes.Extensions
{
    public static class StringExtension
    {

        public static string formated(this string parameter)
        {
            string STRING_FORMAT_ALL = "all[{0}]";
            return String.Format(STRING_FORMAT_ALL, parameter);
        }
    }
}