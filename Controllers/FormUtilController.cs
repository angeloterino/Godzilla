using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanApp.Models;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class FormUtilController : Controller
    {
        //
        // GET: /FormUtil/GetButtonsGroup
        [ChildActionOnly] 
        public PartialViewResult GetButtonsGroup(int channel)
        {
            FormUtilModel.ButtonsGroup bg = new FormUtilModel.ButtonsGroup();
            channel = channel == 0 ? Helpers.Channels.MASS : channel;
            bg.Buttons = GetButtonsList(ButtonListsTypes.CHANNEL_MASTER, channel);
            bg.label_text = ButtonListsTypes.CHANNEL_LABEL;
            List<FormUtilModel.ButtonsGroup> btg = new List<FormUtilModel.ButtonsGroup>();
            btg.Add(bg);
            FormUtilModel.ButtonsGroup nb = GetButtonsGroup(ButtonListsTypes.ADD_ONLY, 0, FormUtilModel.ButtonsGroup.right);
            btg.Add(nb);
            return PartialView(BUTTONS_GROUP, btg);
        }

        //
        // GET: /FormUtil/GetButtonsGroupChartByChannel
        [ChildActionOnly]
        public PartialViewResult GetButtonsGroupChartByChannel(int channel)
        {
            FormUtilModel.ButtonsGroup bg = new FormUtilModel.ButtonsGroup();
            bg.Buttons = GetButtonsList(ButtonListsTypes.CHART_BY_CHANNEL, channel);
            bg.label_text = ButtonListsTypes.CHART_BY_CHANNEL_LABEL;
            List<FormUtilModel.ButtonsGroup> btg = new List<FormUtilModel.ButtonsGroup>();
            btg.Add(bg);
            return PartialView(BUTTONS_GROUP, btg);
        }
        [ChildActionOnly]
        public PartialViewResult GetButtonsGroupByName(string name)
        {
            //Botones para MARKET, BRAND, GROUP
            FormUtilModel.ButtonsGroup bg = GetButtonsGroup(name, 0, GetFloatGroup(name)); new FormUtilModel.ButtonsGroup();
            List<FormUtilModel.ButtonsGroup> btg = new List<FormUtilModel.ButtonsGroup>();
            btg.Add(bg);
            return PartialView(BUTTONS_GROUP, btg);
        }

        #region Forms
        [ChildActionOnly]
        public static FormModel GetMasterDataFormFor(string type, int? _channel)
        {
            return  GetMasterDataFormFor(type, _channel, null,null);
        }
        [ChildActionOnly]
        public static FormModel GetMasterDataFormFor(string type,int? _channel, int? _market, int? _brand)
        {
            FormModel form = new FormModel();
            form.controller = new ConfigController().CONTROLLER_NAME;
            form.action = "Save";
            form.form_id = type;
            string _brand_name = _brand != null ? new ConfigController().GetNameFor(StrawmanDBLibray.Classes.StrawmanDataTables.BRAND_MASTER, _brand) : null;
            string _market_name = _market != null ? new ConfigController().GetNameFor(StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_MASTER, _market) : null;
            //Atributos para la lista de canales para actualziar la de market y group al cambiar de canal
            List<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("data-type","update"),
                new KeyValuePair<string,string>("for","select[for=market_name],select[for=group]"),
                new KeyValuePair<string,string>("data-target","GetSelectList"),
            };
            //Atributos para la lista de grupos para actualziar al cambiar de canal
            List<KeyValuePair<string, string>> gattributes = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("for","group"),
            };
            //Si es edición, enviamos el nombre del mercado al SELECTINPUT vía atributos
            List<KeyValuePair<string, string>> sattributes = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>("value",_market_name),
                new KeyValuePair<string,string>("data-type",Classes.Default.Attributes.MARKET_NAME_ID)
            };
            form.objects.Add(GetFormElement(FormUtilModel.InputTypes.SELECTINPUT, 1, 1, Helpers.MessageByLanguage.Market, true, Classes.Default.Attributes.MARKET_NAME_ID, new ConfigController().GetMarketList((int)_channel, _market), sattributes));
            form.objects.Add(GetFormElement(FormUtilModel.InputTypes.TEXT, 2, 1, Helpers.MessageByLanguage.Brand, true, Classes.Default.Attributes.BRAND_NAME_ID, _brand_name, null));
            form.objects.Add(GetFormElement(FormUtilModel.InputTypes.SELECT, 1, 2, Helpers.MessageByLanguage.Group, false, Classes.Default.Attributes.GROUP_ID, new ConfigController().GetGroupList((int)_channel), gattributes));
            form.objects.Add(GetFormElement(FormUtilModel.InputTypes.SELECT, 2, 2, Helpers.MessageByLanguage.Channel, false, Classes.Default.Attributes.CHANNEL_ID, new ConfigController().GetChannelList((int)_channel), attributes));
            form.objects.Add(GetFormElement(FormUtilModel.InputTypes.HIDDEN, 0, 0, null, false, Classes.Default.Attributes.INPUT_TYPE_ID, Classes.ActionsNames.MASTER_DATA, null));
            form.objects.Add(GetFormElement(FormUtilModel.InputTypes.HIDDEN, 0, 0, null, false, Classes.Default.Attributes.INPUT_TABLE_ID, StrawmanDBLibray.Classes.StrawmanDataTables.MARKET_MASTER, null));
            if(_brand != null)
                form.objects.Add(GetFormElement(FormUtilModel.InputTypes.HIDDEN, 0, 0, null, false, Classes.Default.Attributes.BRAND_ID, _brand.ToString(), null));
            return form;
        }
        [ChildActionOnly]
        public PartialViewResult GetSelectListFor(string _type, int? _channel)
        {
            FormUtilModel.SelectModel select = new FormUtilModel.SelectModel();
            switch (_type)
            {
                case Classes.Default.Attributes.GROUP_ID:
                    select = GetSelectComponent(null, _type, new ConfigController().GetGroupList((int)_channel));
                    break;
                case Classes.Default.Attributes.MARKET_NAME_ID:
                    select = GetSelectComponent(null, _type, new ConfigController().GetMarketList((int)_channel,null));
                    break;
            }
            return PartialView(select.view, select);
        }
        #endregion
        //
        // GET: /FormUtil/

        public ActionResult Index()
        {
            return View();
        }
        #region Functions
        /// <summary>
        /// Devuelve el elemento con los parámetros pasados(INPUT y SELECT)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Models.FormElement GetFormElement(string type, int _column, int _row, string _label, bool _isRequired, string _id, object _value, List<KeyValuePair<string,string>> _attr)
        {
            FormElement element = new Models.FormElement(){
                column = _column,
                row = _row,
                //partial_view = mimput.view
            };
            switch (type)
            {
                case FormUtilModel.InputTypes.TEXT:
                    FormUtilModel.InputModel input = GetInputComponent(_label, _isRequired, _id, _value);
                    if(_attr!=null) SetInputDataAttributes(ref input, _attr);
                    input.type = type;
                    element.model = input;
                    element.partial_view = input.view;
                    break;
                case FormUtilModel.InputTypes.SELECT:
                    List<SelectListItem> _options = _value.GetType() == typeof(string) ? null : (List<SelectListItem>)_value;
                    FormUtilModel.SelectModel select = GetSelectComponent(_label,_id, _options);
                    if (_attr != null) SetSelectDataAttributes(ref select, _attr);
                    element.model = select;
                    element.partial_view = select.view;
                    break;
                case FormUtilModel.InputTypes.HIDDEN:
                    FormUtilModel.InputModel hidden = GetInputComponent(_label, _isRequired, _id,  _value);
                    if (_attr != null) SetInputDataAttributes(ref hidden, _attr);
                    hidden.type = type;
                    element.model = hidden;
                    element.partial_view = hidden.view;
                    break;
                case FormUtilModel.InputTypes.CHECKBOX:
                    FormUtilModel.InputModel chekbox = GetInputComponent(_label, _isRequired, _id, _value);
                    if (_attr != null) SetInputDataAttributes(ref chekbox, _attr);
                    chekbox.type = type;
                    element.model = chekbox;
                    element.partial_view = chekbox.view;
                    break;
                case FormUtilModel.InputTypes.SELECTINPUT:
                    FormUtilModel.SelectInputModel selectinput = GetSelectInputModel(_label, _isRequired, _id, _value, _attr);
                    element.model = selectinput;
                    element.partial_view = selectinput.view;
                    break;
            }
            return element;
        }

        private static FormUtilModel.SelectInputModel GetSelectInputModel(string _label, bool is_required, string _id, object _value, List<KeyValuePair<string,string>> _attr)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem {Text ="", Value = "0"});
            list.AddRange((List<SelectListItem>)_value);
            if (!((List<SelectListItem>)_value).Exists(m => m.Selected == true)) ((List<SelectListItem>)_value).First().Selected = true;
            FormUtilModel.InputModel input = GetInputComponent(null, is_required, _id, ((List<SelectListItem>)_value).Find(m => m.Selected == true).Text);
            input.type = FormUtilModel.InputTypes.TEXT;
            input.data_attributes = _id;
            if (_attr != null) SetInputDataAttributes(ref input, _attr);
            FormUtilModel.InputModel hidden = GetInputComponent(null, is_required, _id.Substring(0, _id.IndexOf('_', 0, _id.Length)), ((List<SelectListItem>)_value).Find(m => m.Selected == true).Value);
            hidden.type = FormUtilModel.InputTypes.HIDDEN;
            hidden._for = _id;

            if (list.Exists(m => m.Selected == true)) list.Find(m => m.Selected == true).Selected = false;//Deseleccionamos el seleccionado por defecto.
            list.Find(m => m.Value == "0").Selected = true;//Seleccionamos el 0 (en blanco)
            FormUtilModel.SelectModel select = GetSelectComponent(null, null, list);
            select._for = _id;
            return new FormUtilModel.SelectInputModel()
            {
                label = _label,
                input = input,
                select = select,
                hidden = hidden
            };
        }

        private static FormUtilModel.InputModel GetInputComponent(string _label, bool is_required, string _id, object _value)
        {
           return new FormUtilModel.InputModel(){
                label = _label,
                is_required = is_required,
                id = _id,
                defalut_value = _value != null && _value.GetType() == typeof(string)?_value.ToString():null
           };
        }

        private static FormUtilModel.SelectModel GetSelectComponent(string _label, string _id, List<SelectListItem> _options)
        {
            return new FormUtilModel.SelectModel()
            {
                label = _label,
                options = _options,
                id = _id
            };
        }

        private FormUtilModel.ButtonsGroup GetButtonsGroup(string name, int _selected, int? _float)
        {
            return new FormUtilModel.ButtonsGroup()
            {
                Buttons = GetButtonsList(name, 0),
                _float = _float
            };
        }

        private static void SetInputDataAttributes(ref FormUtilModel.InputModel _comp, List<KeyValuePair<string, string>> _attr)
        {

            (_comp).data_attributes = _attr != null && _attr.Exists(m => m.Key == "data-type") ? _attr.Find(m => m.Key == "data-type").Value : null;
            (_comp)._for = _attr != null && _attr.Exists(m => m.Key == "for") ? _attr.Find(m => m.Key == "for").Value : null;
            (_comp).data_target = _attr != null && _attr.Exists(m => m.Key == "data-target") ? _attr.Find(m => m.Key == "data-target").Value : null;
            (_comp).defalut_value = _attr != null && _attr.Exists(m => m.Key == "value") ? _attr.Find(m => m.Key == "value").Value : null;
        }
        private static void SetSelectDataAttributes(ref FormUtilModel.SelectModel _comp, List<KeyValuePair<string, string>> _attr)
        {

            (_comp).data_attributes = _attr != null && _attr.Exists(m => m.Key == "data-type") ? _attr.Find(m => m.Key == "data-type").Value : null;
            (_comp)._for = _attr != null && _attr.Exists(m => m.Key == "for") ? _attr.Find(m => m.Key == "for").Value : null;
            (_comp).data_target = _attr != null && _attr.Exists(m => m.Key == "data-target") ? _attr.Find(m => m.Key == "data-target").Value : null;
        }
        private int? GetFloatGroup(string name)
        {
            switch (name)
            {
                case ButtonListsTypes.CHANNEL_MASTER:
                case ButtonListsTypes.CHART_BY_CHANNEL:
                case ButtonListsTypes.MASTER_DATA_NAV:
                    return FormUtilModel.ButtonsGroup.left;
                case ButtonListsTypes.ADD_ONLY:
                case ButtonListsTypes.SAVE_ONLY:
                case ButtonListsTypes.SAVE_AND_CLOSE:
                    return FormUtilModel.ButtonsGroup.right;
            }
            return null;
        }
        private List<Models.FormUtilModel.Button> GetButtonsList(string table, int selected)
        {
            List<Models.FormUtilModel.Button> lst = null;
            int cont = 0;
            switch (table)
            {
                case ButtonListsTypes.CHANNEL_MASTER:
                using (Models.ChannelsDataClassesDataContext db = new Models.ChannelsDataClassesDataContext())
                {
                    var q = from p in db.CHANNEL_MASTER
                            select new Models.FormUtilModel.Button
                            {
                                 target = "ChangeChanel",
                                 text = p.NAME,
                                 variable = p.ID.ToString(),
                                 success = selected == p.ID? "btn-success":null,
                                 type = "btn-change-chanel"
                            };
                    lst = q.ToList();
                }
                break;
                case ButtonListsTypes.CHART_BY_CHANNEL:
                string[] items = { "CHANNEL", "FRANCHISE", "ORAL", "FEMENINE", "COMPROMISED SKIN", "BABY CARE", "ADULTS SKINCARE", "KEY SKINCARE" };
                lst = new List<Models.FormUtilModel.Button>();
                foreach (string chart in items)
                {
                    lst.Add(new Models.FormUtilModel.Button
                    {
                        target="ChangeChart",
                        text = chart,
                        variable = cont.ToString(),
                        success = selected == cont? "btn-success":null,
                        type = "btn-change-chanel"
                    });
                    cont++;
                }
                break;
                case ButtonListsTypes.ADD_ONLY:
                lst = new List<Models.FormUtilModel.Button>();
                lst.Add(new Models.FormUtilModel.Button
                {
                    target = "AddItemForm",
                    controller ="Config",
                    text = Helpers.MessageByLanguage.AddItem,
                    type ="new_item",
                    variable = "modal_new_item"
                });
                break;
                case ButtonListsTypes.SAVE_ONLY:
                lst = new List<Models.FormUtilModel.Button>();
                lst.Add(new Models.FormUtilModel.Button
                {
                    target = "Save",
                    controller = "Config",
                    text = Helpers.MessageByLanguage.AddItem,
                    type = "submit",
                    variable = "item"
                });
                break;
                case ButtonListsTypes.SAVE_AND_CLOSE:
                lst = new List<Models.FormUtilModel.Button>();
                lst.Add(new Models.FormUtilModel.Button
                {
                    target = "Save",
                    controller = "Config",
                    text = Helpers.MessageByLanguage.SaveItem,
                    type = "submit",
                    variable = "item",
                    _class = "btn-success"
                });
                lst.Add(new Models.FormUtilModel.Button
                {
                    dataAttr = "data-dismiss=\"modal\"",
                    text = Helpers.MessageByLanguage.Close,
                    type = "close_modal",
                    variable = "modal",
                    _class = "btn-danger"
                });
                break;
                case ButtonListsTypes.MASTER_DATA_NAV:
                string[] btns = {Classes.StrawmanViews.MARKET, Classes.StrawmanViews.BRAND};
                lst = new List<Models.FormUtilModel.Button>();
                foreach (string btn in btns)
                {
                    lst.Add(new Models.FormUtilModel.Button
                    {
                        target = "NewItemForm",
                        text = btn,
                        variable = btn,
                        success = selected == cont? "btn-success":null,
                        type = "btn-change-section"
                    });
                    cont++;
                }
                break;
            }
            return lst; 
        }
        #endregion
        #region Constants
        private const string _PATH = "~/Views/Shared/";
        private const string BUTTONS_GROUP = _PATH + "_ButtonGroup.cshtml";
        #endregion

        public static class ButtonListsTypes
        {
            public const string CHANNEL_MASTER = "CHANNEL_MASTER";
            public const string CHART_BY_CHANNEL = "CHART_BY_CHANNEL";
            public const string ADD_ONLY = "ADD_ONLY";
            public const string CHANNEL_LABEL = "Channel:";
            public const string CHART_BY_CHANNEL_LABEL = "Chart:";
            public const string SAVE_ONLY = "SAVE_ONLY";
            public const string MASTER_DATA_NAV = "MASTER_DATA_NAV";
            public const string SAVE_AND_CLOSE = "SAVE_AND_CLOSE";
        }
        
    }
}
