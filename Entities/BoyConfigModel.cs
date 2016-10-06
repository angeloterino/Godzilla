using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StrawmanApp.Entities
{
    public class BoyConfigModel
    {
        public int ID { get; set; }

        public System.Data.EntityKey entityKey { get; set; }

        public decimal? channel { get; set; }

        public decimal? brand { get; set; }

        public decimal? vorder { get; set; }

        public string name { get; set; }

        public string brand_name { get; set; }

        public string market_name { get; set; }

        public decimal? market { get; set; }
    }
}