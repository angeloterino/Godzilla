using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Models
{
    public class ConfigOperationsModel
    {
        public string ADD { get { return Helpers.MessageByLanguage.Add; } }
        public string NO_ADD { get { return Helpers.MessageByLanguage.NoAdd; } }
        public string SUBSTRACT { get { return Helpers.MessageByLanguage.Substract; } }

        public static string GetOppName(decimal? _val) 
        {
            int value = _val == null?1:(int)_val;
            switch (value)
            {
                case 0:
                    return new ConfigOperationsModel().NO_ADD;
                case -1:
                    return new ConfigOperationsModel().SUBSTRACT;
                default:
                    return new ConfigOperationsModel().ADD;
            }
        }
        public static List<SelectListItem> GetOppList(decimal? _val) 
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (_val == null)
            {
                list.Add(new SelectListItem { Value = null, Text = "", Selected = true });
            }
            list.Add(new SelectListItem
            {
                Value = "1",
                Text = new ConfigOperationsModel().ADD,
                Selected = _val == 1
            });
            list.Add(new SelectListItem
            {
                Value = "0",
                Text = new ConfigOperationsModel().NO_ADD,
                Selected = _val == 0
            });

            list.Add(new SelectListItem
            {
                Value = "-1",
                Text = new ConfigOperationsModel().SUBSTRACT,
                Selected = _val == -1
            });
            return list;
        
        }

        public static List<SelectListItem> GetOppLevelList(decimal? _val, List<StrawmanDBLibray.Entities.GROUP_MASTER> mstr)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem
            {
                Value = "0",
                Text ="0",
                Selected = _val == 0
            });
            list.AddRange(mstr.Select(m=>new SelectListItem{
                Value = m.ID.ToString(),
                Text = m.NAME.ToString(),
                Selected = _val == m.ID
            }).AsEnumerable());
            return list;
        }
        public static List<SelectListItem> GetDefaultSourceList(string _val)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem
            {
                Text = "Default",
                Value ="",
                Selected = string.IsNullOrEmpty(_val)
            });
            list.Add(new SelectListItem
            {
                Text ="Channel",
                Value ="CHANNEL",
                Selected = _val == "CHANNEL"
            });
            list.Add(new SelectListItem
            {
                Text ="Franchise",
                Value ="FRANCHISE",
                Selected = _val == "FRANCHISE"
            });
            list.Add(new SelectListItem
            {
                Text = "Keybrands",
                Value ="KEYBRANDS",
                Selected = _val == "KEYBRANDS"
            });
            return list;
        }
        public static List<SelectListItem> GetSourceList(string _val, int? group)
        {
            List<SelectListItem> list = group!=null?new List<SelectListItem>():null;
            switch (group)
            {
                case 13:
                case 14:
                case 17:
                    list.Add(new SelectListItem
                    {
                        Text ="Default",
                        Value = null,
                        Selected = _val==null
                    });
                    list.Add(new SelectListItem
                    {
                        Text ="Brand",
                        Value = "BRAND",
                        Selected = _val == "BRAND"
                    });
                    list.Add(new SelectListItem
                    {
                        Text = "Market",
                        Value = "MARKET",
                        Selected = _val == "MARKET"
                    });
                    break;
            };
            return list;

        }
    }
}