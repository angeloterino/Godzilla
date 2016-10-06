using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class Market_BOYModels
    {
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText="0")]
        public double? _internal { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? _le { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public double? _pbp { get; set; }         
    }
}