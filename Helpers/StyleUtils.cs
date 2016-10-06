using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Helpers
{
    public class StyleUtils
    {
        public static string GetBGColor(string color, bool setColor)
        {
            if (String.IsNullOrEmpty(color)) return null;
            string bg = "background-color:" + Helpers.StrawmanConstants.Colors.BackGroundColor(color) + ";";
            if (setColor)
            {
                bg += "color:" + StrawmanConstants.Colors.ColorByBackgroundColor(color)+";";
            }
            return bg;
        }
    }
}