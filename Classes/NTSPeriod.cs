using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Classes
{
    public static partial class NTSPeriod
    {
        private const int _CURRENT = 0;
        private const int _LAST = 1;
        private const int _TWO_AGO = 2;

        public static int CURRENT { get { return _CURRENT; } }
        public static int LAST { get { return _LAST; } }
        public static int TWO_AGO { get { return _TWO_AGO; } }
    }
}