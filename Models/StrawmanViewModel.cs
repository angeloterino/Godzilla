using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StrawmanApp.Models
{
    public class StrawmanViewModel
    {
        public Models.MarketViewChannelModels item;
        public Models.MarketViewChannelModels itemaux;
    }

    public class StrawmanDataModel
    {
        public List<Entities.StrwmanPreviewBrandData> brand;
        public List<Entities.StrwmanPreviewMarketData> market;
        public List<Entities.MasterTable.STRWM_NTS_DATA> nts;
        public List<Entities.MasterTable.STRWM_BMC_DATA> bmc;
    }

    public class StrwawmanTrViewModel
    {
        public string styleView;
        public System.Web.Mvc.ViewDataDictionary styleData;
        public string _class;
        public string _attr;
        public string[] _colPureData;
        public string[] _colData;
        public string[] _colType;
        public string[] _colStyle;
        public string[] _colAttr;
    }
    public class StrawmanSTDViewModel
    {
        public StrawmanSTDView item;
        public StrawmanSTDView itemaux;
    }
    public class StrawmanSTDView
    {
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        public decimal? channel { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
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
        public decimal? col1_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? col2_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? col3_wc { get; set; }
        public decimal? pccol1 { get; set; }
        public decimal? pccol2 { get; set; }
        public double? pccol3 { get; set; }
        public double? pccol4 { get; set; }
        public double? pccol5 { get; set; }

        public string source { get; set; }
    }

    public class StrawmanViewSTDModel
    {
        private bool _is_wc = false;
        public decimal? market { get; set; }
        public decimal? brand { get; set; }
        public decimal? channel { get; set; }
        public decimal? vgroup { get; set; }
        public decimal? vorder { get; set; }
        public decimal? vid { get; set; }
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy4_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy5_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col1_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? col2_wc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}%", NullDisplayText = "0")]
        public decimal? pcvspy_wc { get; set; }
        public decimal? _internalwc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? _lewc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}", NullDisplayText = "0")]
        public decimal? _pbpwc { get; set; }
        public decimal? vparent { get; set; }
        public bool is_wc { get { return _is_wc; } set { _is_wc = value; } }

        public decimal? config { get; set; }

        public decimal? col3_wc { get; set; }
    }
}