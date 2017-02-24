using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrawmanApp.Models
{
    public class ItemsConfigModel
    {
        public string market { get; set; }

        public string brand { get; set; }

        public string channel { get; set; }

        public string market_description { get; set; }

        public string brand_description { get; set; }

        public string channel_description { get; set; }

        public string group_description { get; set; }

        public string source { get; set; }

        public decimal? market_month { get; set; }

        public decimal? market_ytd { get; set; }

        public decimal? market_mat { get; set; }

        public decimal? market_total { get; set; }

        public decimal? brand_month { get; set; }

        public decimal? brand_ytd { get; set; }

        public decimal? brand_mat { get; set; }

        public string nts_name { get; set; }

        public decimal? nts_amount { get; set; }

        public decimal? brand_total { get; set; }
        public List<Nielsen_Data> nielsen_data { get; set; }
        public List<NTS_Data> nts_data { get; set; }
        public Groups_Data groups_data { get; set; }


        public string type { get; set; }

        public bool selected { get; set; }

        public string group_id { get; set; }

        public decimal? excel_row { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> list_data { get; set; }

        public long id { get; set; }

        public decimal? market_config { get; set; }

        public decimal? brand_config { get; set; }
    }
    public partial class Nielsen_Data
    {
        public string market_description { get; set; }
        public int? market_row { get; set; }
        public string brand_description { get; set; }
        public int? brand_row { get; set; }

        public decimal? market { get; set; }

        public decimal? brand { get; set; }

        public decimal? channel { get; set; }

        public decimal id { get; set; }

        public decimal? excel_row { get; set; }
    }
    public partial class NTS_Data
    {

        public decimal? market { get; set; }

        public decimal? brand { get; set; }

        public decimal? channel { get; set; }

        public decimal? eurocode { get; set; }


        public decimal id { get; set; }

        public string market_name { get; set; }
    }
    public partial class Groups_Data
    {
        public string name { get; set; }

        public decimal id { get; set; }

        public decimal? level { get; set; }

        public decimal? base_id { get; set; }

        public System.Data.Objects.DataClasses.EntityCollection<StrawmanDBLibray.Entities.GROUP_CONFIG> group_config { get; set; }

        public System.Data.Objects.DataClasses.EntityCollection<StrawmanDBLibray.Entities.GROUP_CONFIG> config { get; set; }

        public decimal group_id { get; set; }

        public decimal? type_id { get; set; }
    }
    public partial class MasterConfig
    {
        public string market { get; set; }
        public string brand { get; set; }
        public string channel { get; set; }
        public string source { get; set; }
        public string value { get; set; }
    }
    public partial class DataConfig
    {
        public MasterConfig[] items { get; set; }
    }
}
