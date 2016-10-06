using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class MarketTotalModels
    {
        public decimal? vgroup { get; set; }
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? ytd { get; set; }
        public decimal? year { get; set; }
        public string group_name { get; set; }
        public string market_name { get; set; }
        public string brand_name { get; set; }


    }
}