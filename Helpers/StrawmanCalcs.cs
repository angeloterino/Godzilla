using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Helpers
{
    public class StrawmanCalcs
    {
        public static decimal? CalcPCVSPY(decimal? col1, decimal? col2)
        {
            if (col1 <= 0) return 0;
            else if ((col2 / col1 - 1) * 100 > (decimal?)99.99) return (decimal?)99.99;
            return (col2 / col1 - 1) * 100;
        }

        public static decimal? CalcShare(decimal? col1, decimal? col2)
        {
            if (col1 <= 0) return 0;
            else if((col2/col1) *100 > (decimal?)99.99) return (decimal)99.99;
            return (col2 / col1) * 100;
        }

        public static decimal? GetGroupTypeByView(string view)
        {
            List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES> vars = (List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES, true);
            if (!vars.Exists(m => m.VIEW == view)) return null;
            string type = vars.FirstOrDefault(m => m.VIEW == view).VALUE;
            decimal _type = 0;
            if (type == null || !decimal.TryParse(type, out _type))
                return null;
            return _type;
        }
    }
}