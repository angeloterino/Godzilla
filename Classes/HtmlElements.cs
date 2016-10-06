using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Classes
{
    public class HtmlElements
    {
        public static string TABLE_ROWS = "table_rows";
        public static string DATA_ATTRIBUTES = "row_data_attributes";
        public static string CLASSES = "row_classes";
        public static string CELLS = "cells";
        public static string CONTENT = "cell_content";
        public static string PARTIAL_VIEW { get; set; }

        public class ButtonStyles
        {
            public static string DEFAULT = "btn-defaul";
            public static string DANGER = "btn-danger";
            public static string DISABLED = "disabled";
            public static string SUCCESS = "btn-success";
            public static string WARNING = "btn-warning";
        }
    }
}