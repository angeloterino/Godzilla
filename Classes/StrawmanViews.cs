using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Classes
{
    public static partial class StrawmanViews
    {
        public const string MARKET = "MARKET";
        public const string BRAND = "BRAND";
        public const string NTS = "NTS";

        public const string MONTH = "MONTH";
        public const string MAT = "MAT";
        public const string YTD = "YTD";
        public const string BTG = "BTG";
        public const string BOY = "BOY";
        public const string TOTAL = "TOTAL";
        public const string PCVSPY = "PCVSPY";

        public class Partials
        {
            public class BOY
            {
                public const string TOGO = "_BoyTOGO";
                public const string INT = "_BoyINT";
                public const string LE = "_BoyLE";
                public const string PBP = "_BoyPBP";
            }
        }
    }

    public class ActionsNames
    {
        public static string GET_NIELSEN_PREV = "GetNielsenPrev";
        public static string GET_NTS_PREVIEW = "GetNTSPreview";

        //Nombres de las funciones del Backend
        public const string MASTER_DATA = "MasterData";
    }

    public class ControllersNames
    {
        public static string CONFIG = "Config";
    }
}