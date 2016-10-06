using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class Market_YTDModels
    {
        
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? ytd_2013 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? ytd_2014 { get; set; }
        public decimal? pc_vs_py { get; set; } 

    }
}