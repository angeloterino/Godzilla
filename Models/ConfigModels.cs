using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Models
{
    public class ConfigModels
    {
        public List<StrawmanDBLibray.Entities.v_STRWM_MARKET_DATA> strwm_market_data { get; set; }
    }
    public class MasterDataModels{
        private string _active_class = "btn-success";
        public decimal? channel { get; set; }
        public decimal? order { get; set; }
        public decimal? group { get; set; }
        public decimal? id { get; set; }
        public decimal? brand { get; set; }
        public decimal? market { get; set; }
        public string brand_name { get; set; }
        public string market_name { get; set; }
        public string channel_name { get; set; }
        public string group_name { get; set; }
        public string nts_name { get; set; }
        public string data { get; set; }
        public string source { get; set; }
        public string active_class { get { return _active_class; } set { _active_class = value; } }
        public List<SelectListItem> group_list { get; set; }
        public List<SelectListItem> channel_list { get; set; }
        public List<SelectListItem> config_list { get; set; }
        public List<SelectListItem> calc_list { get; set; }
        public List<SelectListItem> level_list { get; set; }
        public List<SelectListItem> order_list { get; set; }
        public decimal? config { get; set; }
        public string config_name { get; set; }
        public int? market_calc { get; set; }
        public string market_calc_name { get; set; }
        public int? brand_calc { get; set; }
        public string brand_calc_name { get; set; }
        public int? nts_calc { get; set; }
        public string nts_calc_name { get; set; }

        public string status { get; set; }

        public decimal? base_id { get; set; }

        public decimal? level { get; set; }

        public List<SelectListItem> source_list { get; set; }

        public string type { get; set; }
    }
    public class MasterDataRowEditModel
    {
        public string channel { get; set; }
        public string market { get; set; }
        public string brand { get; set; }
    }

    public class GenericConfigModel
    {
        private string _header_view = "_Header.cshtml",
                        _footer_view = "_Footer.cshtml",
                        _content_view = "_Content.cshtml";
        private List<TableConfig> _tables = new List<TableConfig>();
        public Navigator navigator { get; set; }
        public string path { get; set; }
        public string header_view { get { return this.path + _header_view; } set { this._header_view = value; } }
        public string content_view { get { return this.path + _content_view; } set { this._content_view = value; } }
        public string footer_view { get { return this.path + _footer_view; } set { this._content_view = value; } }
        public string title { get; set; }
        public List<TableConfig> tables { get { return  this._tables; } set { this._tables = value; } }
    }
    public class TableConfig
    {
        public object content { get; set; }
        public string id { get; set; }
        public string data_attributes { get; set; }
        public string view { get; set; }

        public int default_val { get; set; }
    }
    public class Navigator
    {
        private Data _data = new Data();
        public List<SelectListItem> content { get; set; }
        public Data data { get { return this._data; } set { this._data = value; } }
        public string view { get; set; }
    }
    public class Data
    {
        public string type { get; set; }
    }
}