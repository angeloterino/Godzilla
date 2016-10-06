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
    }
}