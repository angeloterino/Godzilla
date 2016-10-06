using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrawmanApp.Models
{
    public class BOYFormModel
    {
        private BoyMassMarketModels _int;
        private BoyMassMarketModels _btg;
        private BoyMassMarketModels _pbp;
        private BoyMassMarketModels _le;
        private int _mode;

        public BOYFormModel(){}

        public BOYFormModel(BoyMassMarketModels _int, BoyMassMarketModels _btg, BoyMassMarketModels _pbp, BoyMassMarketModels _le)
        {
            // TODO: Complete member initialization
            this._int = _int;
            this._btg = _btg;
            this._pbp = _pbp;
            this._le = _le;
            this._mode = Helpers.Modes.Read;
        }
        public BoyMassMarketModels INT { get { return _int; } set { _int = value; } }
        public BoyMassMarketModels LE { get { return _le; } set { _le = value; } }
        public BoyMassMarketModels BTG { get { return _btg; } set { _btg = value; } }
        public BoyMassMarketModels PBP { get { return _pbp; } set { _pbp = value; } }

        //public List<BoyMassMarketModels> list { get; set; }
        public BoyMassMarketModels item { get; set; }
        public Entities.EditBOYModel model { get; set; }
        public int mode { 
            get{return _mode;}
            set{this._mode = value;}
        }
        public string FormType { get; set; }
    }
}
