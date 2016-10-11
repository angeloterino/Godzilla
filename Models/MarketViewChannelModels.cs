using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class MarketViewChannelModels:ViewModels
    {
        public decimal vid { get; set; }
        public string vhas_child { get; set; }
        public decimal? vchannel { get; set; }
        public decimal? vorder { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vparent { get; set; }
        public string name { get; set; }        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col6 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy4 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy5 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? _internal { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? _le { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? _pbp { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col1_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col2_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy_wc { get; set; }
        public bool is_wc { get; set; }

        public decimal? col3_wc { get; set; }
    }
    public partial class ViewModels
    {
        public string style { get; set; }
    }
}