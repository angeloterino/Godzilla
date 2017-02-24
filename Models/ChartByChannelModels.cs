using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrawmanApp.Models
{
    public class ChartByChannelModels
    {
        public string channel_name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public decimal? mat_market_size { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode=false)]
        public decimal? mat_market_share_l { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? mat_market_share_p { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? ytd_market_share_l { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? ytd_market_share_p { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? lm_market_share_l { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? lm_market_share_p { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? mat_grouth_c { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? mat_grouth_jj { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? ytd_grouth_c { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? ytd_grouth_jj { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? lm_grouth_c { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? lm_grouth_jj { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal mat_market_size_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal mat_market_share_l_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal mat_market_share_p_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal ytd_market_share_l_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal ytd_market_share_p_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal lm_market_share_l_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal lm_market_share_p_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal mat_grouth_c_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal mat_grouth_jj_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal ytd_grouth_c_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal ytd_grouth_jj_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal lm_grouth_c_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal lm_grouth_jj_pc { get; set; }

        private static string _fspc = "{0:F1}%";

        public decimal? vgroup { get; set; }

        public decimal? brand { get; set; }

        public decimal? market { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double? pbp_market_size { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal pbp_market_size_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? pbp_share_p { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal pbp_share_p_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? pbp_share_l { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal pbp_share_l_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? pbp_grouth_c { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal pbp_grouth_c_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? pbp_grouth_jj { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal pbp_grouth_jj_pc { get; set; }

        public decimal? order { get; set; }

        public string type { get; set; }

        public decimal vid { get; set; }

        public decimal? market_size { get; set; }

        public decimal? market_share_l { get; set; }

        public decimal? market_share_p { get; set; }

        public decimal? grouth_c { get; set; }

        public decimal? grouth_jj { get; set; }
    }
    public class ChartGenericModel
    {
        public string channel_name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public decimal? _size { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal _size_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? _share_l { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? _share_p { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal _share_l_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal _share_p_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? _grouth_c { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal? _grouth_jj { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal _grouth_c_pc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F1}%", HtmlEncode = false)]
        public decimal _grouth_jj_pc { get; set; }
        public decimal? vgroup { get; set; }

        public decimal? brand { get; set; }

        public decimal? market { get; set; }
    }
}