using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Classes
{
    public partial class BOYTypes
    {
        private const string _TOTAL = "TOTAL";
        private const string _INT = "INT";
        private const string _LE = "LE";
        private const string _PBP = "PBP";
        private const string _BTG = "BTG";
        private const string _YTD = "YTD";

        public const string TOTAL = _TOTAL; 
        public const string INT = _INT;
        public const string LE = _LE; 
        public const string PBP =_PBP; 
        public const string BTG = _BTG;
        public const string YTD = _YTD;

        public const string _WC = "_WC";
        private const string BoyViews_BoyTOGO = "BoyViews_BoyTOGO";
        private const string BoyViews_BoyINT = "BoyViews_BoyINT";
        private const string BoyViews_BoyLE = "BoyViews_BoyLE";
        private const string BoyViews_BoyPBP = "BoyViews_BoyPBP";

        public static string GetTypeByFormName(string name)
        {
            string ret = null;
            switch (name)
            {
                case BoyViews_BoyTOGO:
                    return BTG;
                case BoyViews_BoyINT:
                    return INT;
                case BoyViews_BoyLE:
                    return LE;
                case BoyViews_BoyPBP:
                    return PBP;
            }
            return ret;
        }
    }

    public class NTSTypes : BOYTypes
    { }
}