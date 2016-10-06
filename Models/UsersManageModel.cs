using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Models
{
    public class UsersManageModel
    {
        [Display(Name = "Select User")]
        public string user { get; set; }

        public List<SelectListItem> UsersList{get;set;}
        public List<SelectListItem> ViewsList { get; set; }
        public List<UserMenuAccess> MenuAccess { get; set; }
        public List<UsersViewAccess> ViewAccess { get; set; }

        [Display(Name = "Select Editable View")]
        public string ViewTable { get; set; }
    }
    public class UserMenuAccess:StrawmanDBLibray.Entities.MENU_MASTER
    {
        public List<SelectListItem> Permissions
        {
            get
            {
                List<SelectListItem> _per = new List<SelectListItem>();
                _per.Add(new SelectListItem
                {
                    Text = "RO",
                    Value = "RO",
                    Selected = true
                });
                _per.Add(new SelectListItem { Text = "RW", Value = "RW" });
                return _per;
            }
        }
        public StrawmanDBLibray.Entities.MENU_CONFIG Config{get;set;}
        public bool isChecked { get; set; }
    }
    public class UsersViewAccess: StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA
    {
        public string view { get; set; }
        public string channel_name { get; set; }
        public List<SelectListItem> views { get; set; }
        public bool isChecked { get; set; }
    }
}