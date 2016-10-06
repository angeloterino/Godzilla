using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Models
{
    public class MenuModels { }
    public class MenuItem
    {
        public string url { get; set; }
        public string name { get; set; }
        public string controller { get; set; }
        public string premission { get; set; }
        public int parent { get; set; }
        public int id { get; set; }

        public string divided_before { get; set; }

        public string disabled { get; set; }
    }
    public class TabItem 
    { 
        public string name { get; set; }
        public string url { get; set; }
        public string menu_url { get; set; }
        public string select_default { get; set; }
        public string controller { get; set; }

        public string title { get; set; }

        public string tab_url { get; set; }
    }
    public class DropDownListModels
    {
        public string id { get; set; }
        public string label { get; set; }
        public string title { get; set; }
        public string SelectedItemId { get; set; }
        public string data_attributes { get; set; }
        public string data_target { get; set; }
        public string data_controller { get; set; }
        public string data_type { get; set; }
        public List<SelectListItem> Items { get; set; }

    }

    public class LoaderViewModels
    {
        public DropDownListModels ddl { get; set; }
        public bool isUpdated { get; set; }
        public bool onError { get; set; }
        public string errorMsg { get; set; }
        public string fileName { get; set; }
        public int fileType { get; set; }
    }
}