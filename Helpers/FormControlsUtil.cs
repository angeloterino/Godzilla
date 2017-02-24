using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Helpers
{
    public class FormControlsUtil
    {
        public static List<SelectListItem> SelectAddBlank(List<SelectListItem> list)
        {
            List<SelectListItem> _lst = new List<SelectListItem>();
            _lst.Add(new SelectListItem { Text = "", Value = null });
            _lst.AddRange(list);
            return _lst;
        }
    }
}