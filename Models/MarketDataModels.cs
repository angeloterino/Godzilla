using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace StrawmanApp.Models
{
    public class MarketDataModels: ViewModels
    {
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        public decimal? vgorder { get; set; }
        public decimal market { get; set; }
        public decimal channel { get; set; }
        public string market_name { get; set; }
        public string data { get; set; }
        public string source { get; set; }
        public string brand_name { get; set; }
        public bool is_wc { get; set; }
        public decimal? brand { get; set; }
    }
}