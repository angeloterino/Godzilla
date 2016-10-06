using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Models
{
    public class BOYVariablesModels
    {
        private bool _share;
        private bool _sellout;
        private bool _sellin;
        private bool _rate;
        private int _index;
        private int _market;
        private int _brand;

        public bool share { get { return _share; } set { _share = value; } }
        public bool sellout { get { return _sellout; } set { _sellout = value; } }
        public bool sellin { get { return _sellin; } set { _sellin = value; } }
        public bool rate { get { return _rate; } set { _rate = value; } }
        public int index { get { return _index; } set { _index = value; } }
        public int market { get { return _market; } set { _market = value; } }
        public int brand { get { return _brand; } set { _brand = value; } }
    }

}