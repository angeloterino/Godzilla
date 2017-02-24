using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class BoyMassMarketModels
    {
        public decimal? channel { get; set; }
        public decimal? brand { get; set; }
        public string brand_name { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        public decimal? market { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? market_col1 { get; set; }
        [Range(typeof(decimal), "0","99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? market_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? sellin_col1 { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? sellin_pc { get; set; }
        public string type { get; set; }
        public string market_name { get; set; }
        public string market_type { get; set; }
        public double? market_boy { get; set; }
        public int? market_id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? market_col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? sellin_col2 { get; set; }
        public double? sellin_boy { get; set; }
        public int? sellin_id { get; set; }
        public string sellin_type { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? sellout_col1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? sellout_col2 { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? sellout_pc { get; set; }
        public double? sellout_boy { get; set; }
        public int? sellout_id { get; set; }
        public string sellout_type { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? share_col1 { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? share_col2 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? share_pc { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? conversion_rate { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? conversion_rate1 { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? conversion_rate2 { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? market_pc_int { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? sellin_pc_int { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public decimal? sellout_pc_int { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", NullDisplayText = @"&nbsp;", HtmlEncode = false)]
        public decimal? share_pc_int { get; set; }
        public string boy_name { get; set; }

        public decimal _id { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? market_btg { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? sellin_btg { get; set; }
        [Range(typeof(decimal), "0", "99.99")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText=@"&nbsp;", HtmlEncode = false)]
        public double? sellout_btg { get; set; }

        public decimal? base_id { get; set; }
    }
}