using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class Market_PCVSPYModels
    {
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        public decimal? pcvspycol1 { get; set; }
        public decimal? pcvspycol2 { get; set; }
        public double? pcvspycol3 { get; set; }
        public double? pcvspycol4 { get; set; }
        public double? pcvspycol5 { get; set; }
    }
}