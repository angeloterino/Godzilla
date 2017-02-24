using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Helpers
{
    public class StrawmanConstants
    {
        public static int getChannel(string CHANNEL) {
            int _response = 0;            
            switch (CHANNEL)
            {
                case "BEAUTY":
                    _response = Channels.BEAUTY;
                    break;
                case "OTC":
                    _response = Channels.OTC;
                    break;
                case "MASS":
                    _response = Channels.MASS;
                    break;
            }
            
            return _response;
            
        }
        public static string getChannelById(int CHANNEL)
        {
            string _response = "";
            switch (CHANNEL)
            {
                case 3:
                    _response = CHANNEL_BEAUTY;
                    break;
                case 2:
                    _response = CHANNEL_OTC;
                    break;
                case 1:
                    _response = CHANNEL_MASS;
                    break;
            }

            return _response;

        }
        public const string CHANNEL_BEAUTY = "BEAUTY";
        public const string CHANNEL_OTC = "OTC";
        public const string CHANNEL_MASS = "MASS";

        public static class Colors {
            public static string Red = "rgb(255,0,0)";
            public static string Blue = "rgb(0,0,255)";
            public static string Green = "rgb(0,255,0)";
            public static string White = "rgb(255,255,255)";
            public static string Black = "rgb(0,0,0)";
            public static string Yellow = "rgb(255,255,0)";
            public static string Magenta = "rgb(0,255,255)";
            public static string MediumBlue = "rgb(0,102,255)";
            public static string DarkBlue = "rgb(0,102,204)";
            public static string NightBlue = "rgb(23,55,93)";
            public static string LinkBlue = "rgb(50, 120, 255)";
            public static string LigthLinkBlue = "rgb(150, 170, 255)";
            public static string LigthGreen = "#f0faf0";
            public static string PalePink = "rgb(252,213,180)";
            public static string PaleGreen = "rgb(153,255,153)";
            public static string Orange = "rgb(255,153,0)";
            public static string PaleOrange = "rgb(255,192,0)";
            public static string Transparent = "transparent";
            
            public static string BackGroundColor(string str)
            {
                switch (str)
                {
                    case "Red":
                        return Red;
                    case "Blue":
                        return Blue;
                    case "Green":
                        return Green;
                    case "White":
                        return White;
                    case "Black":
                        return Black;
                    case "Yellow":
                        return Yellow;
                    case "Magenta":
                        return Magenta;
                    case "MediumBlue":
                        return MediumBlue;
                    case "DarkBlue":
                        return DarkBlue;
                    case "NightBlue":
                        return NightBlue;
                    case "LinkBlue":
                        return LinkBlue;
                    case "LigthLinkBlue":
                        return LigthLinkBlue;
                    case "LigthGreen":
                        return LigthGreen;
                    case "PalePink":
                        return PalePink;
                    case "PaleGreen":
                        return PaleGreen;
                    case "Orange":
                        return Orange;
                    case "PaleOrange":
                        return PaleOrange;
                }
                return Transparent;
            }

            public static string ColorByBackgroundColor(string back)
            {
                switch (back)
                {
                    case "Red":
                    case "Blue":
                    case "Black":
                    case "DarkBlue":
                    case "NightBlue":
                        return White;
                }
                return Black;
            }
        }
    }
    public class UserUtils
    {
        private string _user;
        private string _rol;
        private string _admin;

        private static UserUtils myObject = new UserUtils();

        public static string UserName
        {
            get { return myObject._user; }
            set  
            {
                myObject._user = value;
                List<StrawmanDBLibray.Entities.USERS_ROLES> roles = (List<StrawmanDBLibray.Entities.USERS_ROLES>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.USERS_ROLES, true);
                List<StrawmanDBLibray.Entities.USER_ROLE_TYPES> rolesTypes = (List<StrawmanDBLibray.Entities.USER_ROLE_TYPES>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.USER_ROLE_TYPES, true);
                myObject._rol = roles != null ? roles.Exists(m => m.USER == myObject._user) ? rolesTypes.Find(t=> t.ID ==roles.Find(m => m.USER == myObject._user).ROLE).NAME : Permissions.Admin : Permissions.Admin; 
            }
        }
        public class Permissions
        {
            public static string Rol
            {
                get { return myObject._rol; }
                set { myObject._rol = value; }
            }
            public const string Admin = "admin";
            public const string Super = "super";
            public const string Standard = "standard";

            public static bool GetPermissionsFor(string view)
            {
                return GetPermissions() ? true : CheckPermissions(true).Exists(m => m.VIEW == view && m.USER == RolesByUser(true));
            }
            public static bool GetPermissionsByData(string view, int market, int brand, int channel)
            {
                return GetPermissions() ? true : CheckPermissions(true).Exists(m => m.VIEW == view && m.USER == RolesByUser(true) && m.MARKET == market && m.BRAND == brand && m.CHANNEL == channel);
            }
            public static bool GetPermissions()
            {
                return Rol != Standard;
            }
            private static List<StrawmanDBLibray.Entities.USERS_PERMISSIONS> CheckPermissions(bool cache)
            {
                return (List<StrawmanDBLibray.Entities.USERS_PERMISSIONS>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.USERS_PERMISSIONS, cache);
            }
            private static List<StrawmanDBLibray.Entities.USERS_ROLES> Roles(bool cache)
            {
                return (List<StrawmanDBLibray.Entities.USERS_ROLES>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.USERS_ROLES, cache);
            }
            private static decimal RolesByUser(bool cache)
            {
                return Roles(cache).Find(n => n.USER == UserName).ID;
            }
        }
    }
    public class LanguageUtil{
        private string _lang;
        private static LanguageUtil myObject = new LanguageUtil();
        public static string Lang
        {
            get {return myObject._lang;}
            set{myObject._lang = value;}
        }
    }

    public class StrawmanViews
    {
        public class MANAGEMENT_LETTERS
        {
            public static string name = "Management Letters";
            public const string id = "MANAGEMENT_LETTERS";
            public class Scripts
            {
                public static string forms = "~/Views/Shared/_ScriptsCommentsEnableManagement.cshtml";
            }
        }
        public class MONTHLY_COMMENTS
        {
            public static string name = "Monthly Comments";
            public const string id = "MONTHLY_COMMENTS";
            public class Scripts
            {
                public static string forms = "~/Views/Shared/_ScriptsCommentsEnableManagement.cshtml";
            }
        }

        public class BOY
        {
            public static string name = "BOY";
            public const string id = "BOY";
            public class Scripts
            {
                public static string forms = "~/Views/Shared/_ScriptsEnableManagement.cshtml";
            }
        }
        public class KPI
        {
            public static string name = "KPI";
            public const string id = "KPI";
            public class Scripts
            {
                public static string forms = "~/Views/Shared/_ScriptsEnableManagementKPI.cshtml";
            }
        }
        public class BOYBYCHANNEL
        {
            public static string name = "BOYJJByChannel";
            public const string id = "BOYJJByChannel";
            public class Scripts
            {
                public static string forms = "~/Views/Shared/_ScriptsCommentsEnableManagement.cshtml";
            }
        }
        public class Scripts
        {
            public static string voidScript = "~/Views/Shared/_VoidScript.cshtml";
        }
    }

    public class StrawmanView
    {
        private string _name;
        private string _id;
        private static StrawmanView myObject = new StrawmanView();

        public static string Name
        {
            get { return myObject._name; }
            set { myObject._name = value; }
        }
        public static string Id
        {
            get { return myObject._id; }
            set { myObject._id = value; }
        }
    }
    public class PeriodUtil
    {
        private int _year;
        private int _month;
        private static PeriodUtil myObject = new PeriodUtil();
        
        public static int Year
        {
            get { return myObject._year; }
            set { myObject._year = value; }
        }

        public static int Month
        {
            get { return myObject._month; }
            set { myObject._month = value; }
        }
    }

    public class CountryUtil
    {
        private string _country;

        private static CountryUtil myObject = new CountryUtil();

        public static string Country
        {
            get { return myObject._country; }
            set { myObject._country = value; }
        }
    }
    /// <summary>
    /// Mensajes de validación
    /// </summary>
    public class Messages{

        private string _success = " saved successfully.";
        private string _fail = "Error inserting/updating ";
        private static Messages myObject = new Messages();
        public static string Success(string _append)
        {
            return _append + myObject._success;
        }
        public static string Failure(string _append)
        {
            return myObject._fail + _append + ".";
        }
    
    }

    public static partial class Channels
    {
        private const int _MASS = 1;
        private const int _OTC = 2;
        private const int _BEAUTY = 3;
        public static int MASS { get { return _MASS; } }
        public static int OTC { get { return _OTC; } }
        public static int BEAUTY { get { return _BEAUTY; } }
        public static int WC_CHANNEL { get { return _OTC; } }
    }

    public static class Modes
    {
        public static int Edit = 1;
        public static int Read = 0;
        public static int Delete = 2;
        public static int Update = 3;
    }
    //Iconos
    public static class Icons
    {
        public static string Delete
        {
            get { return _Delete.ToHtmlString(); }
        }
        private static HtmlString _Delete = new HtmlString("<i class=\"fa fa-times\"></i>");
    }
    //Funcion que devuelve los textos definidos en Texts según lenguaje (inglés por defecto)
    public static class MessageByLanguage
    {
        public static string Order { get { switch (LanguageUtil.Lang) { case Languages.ES: return Texts.ORDER_ES; default: return Texts.ORDER_EN; } } }
        public static string Items { get { switch (LanguageUtil.Lang) { case Languages.ES: return Texts.ITEMS_ES; default: return Texts.ITEMS_EN; } } }
        public static string Item { get { switch (LanguageUtil.Lang) { case Languages.ES: return Texts.ITEM_ES; default: return Texts.ITEM_EN; } } }
        public static string BrandKeybrandsConfig { get { switch (LanguageUtil.Lang) { default: return Texts.BRAND_KEYBRANDS_CONFIG; } } }
        public static string BrandFranchiseConfig { get { switch (LanguageUtil.Lang) { default: return Texts.BRAND_FRANCHISE_CONFIG; } } }
        public static string BrandChannelConfig { get { switch (LanguageUtil.Lang) { default: return Texts.BRAND_CHANNEL_CONFIG; } } }
        public static string BrandSuperGroupConfig { get { switch (LanguageUtil.Lang) { default: return Texts.BRAND_SUPER_GROUP_CONFIG; } } }
        public static string BrandGroupConfig { get { switch (LanguageUtil.Lang) { default: return Texts.BRAND_GROUP_CONFIG; } } }
        public static string MarketKeybrandsConfig { get { switch (LanguageUtil.Lang) { default: return Texts.MARKET_KEYBRANDS_CONFIG; } } }
        public static string MarketFranchiseConfig { get { switch (LanguageUtil.Lang) { default: return Texts.MARKET_FRANCHISE_CONFIG; } } }
        public static string MarketChannelConfig { get { switch (LanguageUtil.Lang) { default: return Texts.MARKET_CHANNEL_CONFIG; } } }
        public static string MarketSuperGroupConfig { get { switch (LanguageUtil.Lang) { default: return Texts.MARKET_SUPER_GROUP_CONFIG; } } }
        public static string MarketGroupConfig { get { switch (LanguageUtil.Lang) { default: return Texts.MARKET_GROUP_CONFIG; } } }
        public static string BrandConfig { get { switch (LanguageUtil.Lang) { default: return Texts.BRAND_CONFIG; } } }
        public static string MarketConfig { get { switch (LanguageUtil.Lang) { default: return Texts.MARKET_CONFIG; } } }
        public static string NTS { get { switch (LanguageUtil.Lang) { default: return Texts.NTS; } } }
        public static string IMS_Nielsen { get { switch (LanguageUtil.Lang) { default: return Texts.IMS_NIELSEN; } } }
        public static string Save { get { switch (LanguageUtil.Lang) { case Languages.ES:return Texts.SAVE_ES; default:return Texts.SAVE_EN; } } }
        public static string Substract { get { switch (LanguageUtil.Lang) { case Languages.ES:return Texts.SUBSTRACT_ES; default: return Texts.SUBSTRACT_EN; } } }
        public static string NoAdd { get { switch (LanguageUtil.Lang) { case Languages.ES: return Texts.NO_ADD_ES; default: return Texts.NO_ADD_EN; } } }
        public static string NotCofigError { get { switch (LanguageUtil.Lang) { case Languages.ES:return Texts.NOT_CONFIG_ERROR_ES; default:return Texts.NOT_CONFIG_ERROR_EN; } } }
        public static string NotFound { get { switch (LanguageUtil.Lang) { case Languages.ES:return Texts.NOT_FOUND_ES; default:return Texts.NOT_FOUND_EN; } } }
        public static string EditItem { get { switch (LanguageUtil.Lang) { case Languages.ES: return Texts.EDIT_ITEM_ES; default: return Texts.EDIT_ITEM_EN; } } }
        public static string Brand { get { switch (LanguageUtil.Lang) { case Languages.ES: return Texts.BRAND_ES; default: return Texts.BRAND_EN; } } }
        public static string Channel { get { switch (LanguageUtil.Lang) { case Languages.ES: return Texts.CHANNEL_ES; default: return Texts.CHANNEL_EN; } } }
        public static string FieldRequired
        {
            get 
            { 
                switch (LanguageUtil.Lang) 
                { 
                    case Languages.ES: return Texts.FIELD_REQUIRED_ES;
                    default: return Texts.FIELD_REQUIRED_EN; 
                } 
            }
        }

        public static string SaveItem
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.SAVE_ITEM_ES;
                    default:
                        return Texts.SAVE_ITEM_EN;
                }
            }

        }
        public static string Market
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.MARKET_ES;
                    default:
                        return Texts.MARKET_EN;
                }
            }
        }
        public static string New
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.NUEVO;                        
                    default:
                        return Texts.NEW;                        
                }
            }
        }

        public static string Delete
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.BORRAR;
                    default:
                        return Texts.DELETE;
                }
            }
        }

        public static string Edit
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.EDITAR;
                    default:
                        return Texts.EDIT;
                }
            }
        }

        public static string Add
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.ANADIR;
                    default:
                        return Texts.ADD;
                }
            }
        }
        public static string AddItem
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.ADD_ITEM_ES;
                    default:
                        return Texts.ADD_ITEM_EN;
                }
            }
        }
        public static string Close
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.CLOSE_ES;
                    default:
                        return Texts.CLOSE_EN;
                }
            }
        }
        public static string DeleteCommentConfirm
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.DELETE_COMMENT_CONFIRM_ES;
                    default:
                        return Texts.DELETE_COMMENT_CONFIRM_ENG;
                }
            }
        }
        //"¿Está seguro de eliminar el comentario?"

        public static string NullComment
        {
            get 
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.NULL_COMMENT_ES;
                    default:
                        return Texts.NULL_COMMENT_EN;
                }
            }
        }

        public static string Name
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.NAME_ES;
                    default:
                        return Texts.NAME_EN;
                }
            }
        }
        public static string Group
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.GROUP_ES;
                    default:
                        return Texts.GROUP_EN;
                }
            }
        }
        //"Comentario de error por defecto"

        public static string DefaultErrorComment
        {
            get
            {
                switch (LanguageUtil.Lang)
                {
                    case Languages.ES:
                        return Texts.COMMON_ERROR_MSG_ES;
                    default:
                        return Texts.COMMON_ERROR_MSG_EN;
                }
            }
        }
    }

#region Private_Constats
    //Funciones para la selección de textos por lenguaje
    static class Languages{
        public const string ES = "ES";
        public const string ENG = "ENG";
        public const string POR = "POR";
        public const string ITA = "ITA";
    }
    //Clase para almacenar todos los textos 
    static partial class Texts
    {
        //Ingles -Default-
        private const string _NEW = "New";
        private const string _DELETE = "Delete";
        private const string _EDIT = "Edit";
        private const string _ADD = "Add";
        private const string _DELETE_COMMENT_CONFIRM_ENG = "Do you want to delete this comment?";
        private const string _COMMENT_TITLE_EN = "Comment for";
        private const string _COMMENT_BUTTON_TEXT_EN = "New Comment";
        private const string _NULL_COMMENT_EN = "No comments found";
        private const string _COMMON_ERROR_MSG_EN =   @"An error has occurred while trying to process your request. "
                                                      +"Please try to close your session and login again. "
                                                      +"If the problem persists, try closing and opening a new instance of the web browser. "
                                                      +"If after all this error continues appearing, please contact with the application's manager. ";
        private const string _ITEMS_EN = "Items",
                             _ITEM_EN ="Item",
                             _ADD_ITEM_EN = "Add Item";
        private const string _CLOSE_EN = "Close";
        private const string _NAME_EN = "Name";
        private const string _GROUP_EN = "Group";
        private const string _MARKET_EN = "Market";
        private const string _SAVE_ITEM_EN = "Save Item";
        private const string _FIELD_REQUIRED_EN = "The field is mandatory";
        private const string _BRAND_EN = "Brand";
        private const string _CHANNEL_EN = "Channel";
        private const string _EDIT_ITEM_EN = "Edit Item";
        private const string _NOT_FOUND_EN = "Not Found";
        private const string _NOT_CONFIG_ERROR_EN = "No basic settings found. Please contact with the application's manager.";
        private const string _NO_ADD_EN = "Not Add";
        private const string _SUBSTRACT_EN = "Substract";
        private const string _SAVE_EN = "Save";
        private const string _MARKET_CONFIG_EN = "Market Config";
        private const string _MARKET_KEYBRANDS_CONFIG = "Market Keybrands Config",
                             _MARKET_FRANCHISE_CONFIG = "Market Franchise Config",
                             _MARKET_CHANNEL_CONFIG = "Market Channel Config",
                             _MARKET_SUPER_GROUP_CONFIG = "Market Super Group Config",
                             _MARKET_GROUP_CONFIG = "Market Group Config"
                             ;
        private const string _BRAND_CONFIG_EN = "Brand Config";
        private const string _BRAND_KEYBRANDS_CONFIG = "Brand Keybrands Config",
                             _BRAND_FRANCHISE_CONFIG = "Brand Franchise Config",
                             _BRAND_CHANNEL_CONFIG = "Brand Channel Config",
                             _BRAND_SUPER_GROUP_CONFIG = "Brand Super Group Config",
                             _BRAND_GROUP_CONFIG="Brand Group Config"
                             ;
        private const string _ORDER_EN = "Order";
        //Español
        private const string _NUEVO = "Nuevo";
        private const string _BORRAR = "Borrar";
        private const string _EDITAR = "Editar";
        private const string _ANADIR = "Añadir";
        private const string _DELETE_COMMENT_CONFIRM_ES = "¿Está seguro de eliminar el comentario?";
        private const string _COMMENT_TITLE_ES = "Comentario para";
        private const string _COMMENT_BUTTON_TEXT_ES = "Nuevo Comentario";
        private const string _NULL_COMMENT_ES = "No se han registrado comentarios";
        private const string _COMMON_ERROR_MSG_ES =  @"Se ha producido un error al intentar procesar su solicitud. "
                                                     +"Por favor, intente cerrar la sesión y vuelva a iniciar sesión. "
                                                     +"Si el problema persiste, intente cerrar y abrir una nueva instancia del navegador web. "
                                                     +"Si después de todo este error sigue apareciendo, póngase en contacto con el administrador de la aplicación.";
        private const string _ITEMS_ES = "Artículos",
                             _ITEM_ES ="Artículo",
                             _ADD_ITEM_ES = "Añadir Item";
        private const string _NAME_ES = "Nombre";
        private const string _GROUP_ES = "Grupo";
        private const string _CLOSE_ES = "Cerrar";
        private const string _MARKET_ES = "Market";
        private const string _SAVE_ITEM_ES = "Save Item";
        private const string _FIELD_REQUIRED_ES = "El campo es obligatorio";
        private const string _BRAND_ES = "Brand";
        private const string _CHANNEL_ES = "Channel";
        private const string _EDIT_ITEM_ES = "Edit Item";
        private const string _NOT_FOUND_ES = "Not Found";
        private const string _NOT_CONFIG_ERROR_ES = "No se ha encontrado la configuración básica del usuario. Por favor, póngase en contacto con el administrador de la aplicación";
        private const string _NO_ADD_ES = "No añadir";
        private const string _SUBSTRACT_ES = "Restar";
        private const string _SAVE_ES = "Guardar";
        private const string _ORDER_ES = "Orden";

        private const string _NTS = "NTS";
        private const string _IMS_NIELSEN ="IMS/Nielsen";

        //Funciones GET
        //Inglés
        public static string NEW { get { return _NEW; } }
        public static string DELETE { get { return _DELETE; } }
        public static string EDIT { get { return _EDIT; } }
        public static string ADD { get { return _ADD; } }
        public static string DELETE_COMMENT_CONFIRM_ENG { get { return _DELETE_COMMENT_CONFIRM_ENG; } }
        public static string COMMENT_TITLE_EN { get { return _COMMENT_TITLE_EN; } }
        public static string COMMENT_BUTTON_TEXT_EN { get { return _COMMENT_BUTTON_TEXT_EN; } }
        public static string NULL_COMMENT_EN { get { return _NULL_COMMENT_EN; } }
        public static string COMMON_ERROR_MSG_EN { get { return _COMMON_ERROR_MSG_EN; } }
        public static string ADD_ITEM_EN { get { return _ADD_ITEM_EN; } }
        public static string NAME_EN { get { return _NAME_EN; } }
        public static string GROUP_EN { get { return _GROUP_EN; } }
        public static string CLOSE_EN { get { return _CLOSE_EN; } }
        public static string MARKET_EN { get { return _MARKET_EN; } }
        public static string SAVE_ITEM_EN { get { return _SAVE_ITEM_EN; } }
        public static string FIELD_REQUIRED_EN { get { return _FIELD_REQUIRED_EN; } }
        public static string BRAND_EN { get { return _BRAND_EN; } }
        public static string CHANNEL_EN { get { return _CHANNEL_EN; } }
        public static string EDIT_ITEM_EN { get { return _EDIT_ITEM_EN; } }
        public static string NOT_FOUND_EN { get { return _NOT_FOUND_EN; } }
        public static string NOT_CONFIG_ERROR_EN { get { return _NOT_CONFIG_ERROR_EN; } }
        public static string NO_ADD_EN { get { return _NO_ADD_EN; } }
        public static string SUBSTRACT_EN { get { return _SUBSTRACT_EN; } }
        public static string SAVE_EN { get { return _SAVE_EN; } }
        public static string MARKET_CONFIG { get { return _MARKET_CONFIG_EN; } }
        public static string BRAND_CONFIG { get { return _BRAND_CONFIG_EN; } }
        public static string BRAND_KEYBRANDS_CONFIG { get { return _BRAND_KEYBRANDS_CONFIG; } }
        public static string BRAND_FRANCHISE_CONFIG { get { return _BRAND_FRANCHISE_CONFIG; } }
        public static string BRAND_CHANNEL_CONFIG { get { return _BRAND_CHANNEL_CONFIG; } }
        public static string BRAND_SUPER_GROUP_CONFIG { get { return _BRAND_SUPER_GROUP_CONFIG; } }
        public static string BRAND_GROUP_CONFIG { get { return _BRAND_GROUP_CONFIG; } }
        public static string MARKET_KEYBRANDS_CONFIG { get { return _MARKET_KEYBRANDS_CONFIG; } }
        public static string MARKET_FRANCHISE_CONFIG { get { return _MARKET_FRANCHISE_CONFIG; } }
        public static string MARKET_CHANNEL_CONFIG { get { return _MARKET_CHANNEL_CONFIG; } }
        public static string MARKET_SUPER_GROUP_CONFIG { get { return _MARKET_SUPER_GROUP_CONFIG; } }
        public static string MARKET_GROUP_CONFIG { get { return _MARKET_GROUP_CONFIG; } }
        public static string ITEMS_EN { get { return _ITEMS_EN; } }
        public static string ITEM_EN { get { return _ITEM_EN; } }
        public static string ORDER_EN { get { return _ORDER_EN; } }
        //Español
        public static string NUEVO { get { return _NUEVO; } }
        public static string BORRAR { get { return _BORRAR; } }
        public static string EDITAR { get { return _EDITAR; } }
        public static string ANADIR { get { return _ANADIR; } }
        public static string DELETE_COMMENT_CONFIRM_ES { get { return _DELETE_COMMENT_CONFIRM_ES; } }
        public static string COMMENT_TITLE_ES { get { return _COMMENT_TITLE_ES; } }
        public static string COMMENT_BUTTON_TEXT_ES { get { return _COMMENT_BUTTON_TEXT_ES; } }
        public static string NULL_COMMENT_ES { get { return _NULL_COMMENT_ES; } }
        public static string COMMON_ERROR_MSG_ES { get { return _COMMON_ERROR_MSG_ES; } }
        public static string ADD_ITEM_ES { get { return _ADD_ITEM_ES; } }
        public static string NAME_ES { get { return _NAME_ES; } }
        public static string GROUP_ES { get { return _GROUP_ES; } }
        public static string CLOSE_ES { get { return _CLOSE_ES; } }
        public static string MARKET_ES { get { return _MARKET_ES; } }
        public static string SAVE_ITEM_ES { get { return _SAVE_ITEM_ES; } }
        public static string FIELD_REQUIRED_ES { get { return _FIELD_REQUIRED_ES; } }
        public static string BRAND_ES { get { return _BRAND_ES; } }
        public static string CHANNEL_ES { get { return _CHANNEL_ES; } }
        public static string EDIT_ITEM_ES { get { return _EDIT_ITEM_ES; } }
        public static string NOT_FOUND_ES { get { return _NOT_FOUND_ES; } }
        public static string NOT_CONFIG_ERROR_ES { get { return _NOT_CONFIG_ERROR_ES; } }
        public static string NO_ADD_ES { get { return _NO_ADD_ES; } }
        public static string SUBSTRACT_ES { get { return _SUBSTRACT_ES; } }
        public static string SAVE_ES { get { return _SAVE_ES; } }

        public static string NTS { get { return _NTS; } }
        public static string IMS_NIELSEN { get { return _IMS_NIELSEN; } }
        public static string ITEMS_ES { get { return _ITEMS_ES; } }
        public static string ITEM_ES { get { return _ITEM_ES; } }
        public static string ORDER_ES { get { return _ORDER_ES; } }   
    }
#endregion
}