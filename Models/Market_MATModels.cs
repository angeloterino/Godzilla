using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class Market_MATModels
    {
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        public decimal? pc_vs_py { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? mat_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? mat_col2 { get; set; }
    }
}