using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class Market_TotalCustomModels
    {
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? v2015 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? v2014 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? v2013 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? v2012 { get; set; }
    }
}