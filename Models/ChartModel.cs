using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Models
{
    public class ChartModel
    {
        public List<Models.ChartByChannelModels> chart_lm { get; set; }
        public List<Models.ChartByChannelModels> chart_mat { get; set; }
        public List<Models.ChartByChannelModels> chart_pbp { get; set; }
        public List<Models.ChartByChannelModels> chart_ytd { get; set; }
        public List<Models.ChartByChannelModels> chart_btg { get; set; }
    }
}