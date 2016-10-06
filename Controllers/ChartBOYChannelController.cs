using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrawmanApp.Controllers;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class ChartBOYChannelController : Controller
    {
        public ActionResult GetChart(string type)
        {            
            return GetChartByType(type);
        }

        public ActionResult GetChartByType(string type)
        {
            Models.ChartModel chart = new Models.ChartModel();

            ViewBag.ChartTitle = getChartTitle(type);                                
            chart = GetChartData(type);
            List<Models.ChartByChannelModels> lst = chart.chart_ytd;
            ViewBag.ChartModel = lst;
            
            return PartialView(GENERIC, chart);
        }
        public static Models.ChartModel GetChartDataExport(string type)
        {
            return GetChartData(type);
        }
        private static Models.ChartModel GetChartData(string type)
        {
            Models.ChartModel chart = new Models.ChartModel();
            Models.ChartByChannelModels c = new Models.ChartByChannelModels();
            using (Entities.godzillaChartsEntities db = new Entities.godzillaChartsEntities())
            {
                var query = db.v_CHART_BOY_JJ_BY_CHANNEL
                            .Where(p => p.CHART == type && p.TYPE == "LM" && (p.BRAND < 1000 || p.BRAND > 90000)
                             && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                            .Select(p =>
                             new Models.ChartByChannelModels
                            {
                                order = p.ORDER,
                                vgroup = p.GROUP,
                                brand = p.BRAND,
                                market = p.MARKET,
                                lm_market_share_l = p.LATEST,
                                lm_market_share_p = p.PREVIOUS,
                                lm_grouth_c = p.GROWTH_CATHEGORY,
                                lm_grouth_jj = p.GROWTH_J_J,
                                channel_name = p.NAME,
                                lm_grouth_c_pc = 0,
                                lm_grouth_jj_pc = 0,
                                lm_market_share_l_pc = 0,
                                lm_market_share_p_pc = 0,
                                mat_grouth_c = 0,
                                mat_grouth_c_pc = 0,
                                mat_grouth_jj = 0,
                                mat_grouth_jj_pc = 0,
                                mat_market_share_l = 0,
                                mat_market_share_l_pc = 0,
                                mat_market_share_p = 0,
                                mat_market_share_p_pc = 0,
                                mat_market_size = 0,
                                mat_market_size_pc = 0,
                                pbp_grouth_c = 0,
                                pbp_grouth_jj = 0,
                                pbp_market_size = 0,
                                pbp_share_l = 0,
                                pbp_share_p = 0,
                                ytd_grouth_c = 0,
                                ytd_grouth_c_pc = 0,
                                ytd_grouth_jj = 0,
                                ytd_grouth_jj_pc = 0,
                                ytd_market_share_l = 0,
                                ytd_market_share_l_pc = 0,
                                ytd_market_share_p = 0,
                                ytd_market_share_p_pc = 0
                            });

                chart.chart_lm = ChartByChannelController.GetChartsPercent(query.ToList());

                query = db.v_CHART_BOY_JJ_BY_CHANNEL
                        .Where(p=>p.CHART == type && p.TYPE == "MAT"
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                        .Select(p=>
                        new Models.ChartByChannelModels
                        {
                            order = p.ORDER,
                            vgroup = p.GROUP,
                            brand = p.BRAND,
                            market = p.MARKET,
                            mat_market_share_l = p.LATEST,
                            mat_market_share_p = p.PREVIOUS,
                            mat_grouth_c = p.GROWTH_CATHEGORY,
                            mat_grouth_jj = p.GROWTH_J_J,
                            mat_market_size = p.MARKET_SIZE,
                            lm_market_share_l = p.LATEST,
                            lm_market_share_p = p.PREVIOUS,
                            lm_grouth_c = p.GROWTH_CATHEGORY,
                            lm_grouth_jj = p.GROWTH_J_J,
                            channel_name = p.NAME,
                            lm_grouth_c_pc = 0,
                            lm_grouth_jj_pc = 0,
                            lm_market_share_l_pc = 0,
                            lm_market_share_p_pc = 0,
                            mat_grouth_c_pc = 0,
                            mat_grouth_jj_pc = 0,
                            mat_market_share_l_pc = 0,
                            mat_market_share_p_pc = 0,
                            mat_market_size_pc = 0,
                            pbp_grouth_c = 0,
                            pbp_grouth_jj = 0,
                            pbp_share_l = 0,
                            pbp_share_p = 0,
                            pbp_market_size = 0,
                            ytd_grouth_c = 0,
                            ytd_grouth_c_pc = 0,
                            ytd_grouth_jj = 0,
                            ytd_grouth_jj_pc = 0,
                            ytd_market_share_l = 0,
                            ytd_market_share_l_pc = 0,
                            ytd_market_share_p = 0,
                            ytd_market_share_p_pc = 0

                        });
                chart.chart_mat = new List<Models.ChartByChannelModels>();
                chart.chart_mat.AddRange(ChartByChannelController.GetChartsPercent(query.Where(m => m.market > 10000 && m.brand > 10000).ToList())); //By Channel + Total
                chart.chart_mat.AddRange(ChartByChannelController.GetChartsPercent(query.Where(m => (m.market < 10000 && m.brand < 10000 && m.market> 1000 && m.brand > 1000) || m.brand > 90000).ToList())); //By Category + Total
                chart.chart_mat.AddRange(ChartByChannelController.GetChartsPercent(query.Where(m => (m.market < 1000 && m.brand < 1000) || m.brand > 90000).ToList())); // By article + Total

                query = db.v_CHART_BOY_JJ_BY_CHANNEL
                            .Where(p => p.CHART == type && p.TYPE == "YTD" && (p.BRAND < 1000 || p.BRAND > 90000)
                            && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                            .Select(p =>
                        new Models.ChartByChannelModels
                        {
                            order = p.ORDER,
                            vgroup = p.GROUP,
                            brand = p.BRAND,
                            market = p.MARKET,
                            ytd_market_share_l = p.LATEST,
                            ytd_market_share_p = p.PREVIOUS,
                            ytd_grouth_c = p.GROWTH_CATHEGORY,
                            ytd_grouth_jj = p.GROWTH_J_J,
                            lm_market_share_l = p.LATEST,
                            lm_market_share_p = p.PREVIOUS,
                            lm_grouth_c = p.GROWTH_CATHEGORY,
                            lm_grouth_jj = p.GROWTH_J_J,
                            channel_name = p.NAME,
                            lm_grouth_c_pc = 0,
                            lm_grouth_jj_pc = 0,
                            lm_market_share_l_pc = 0,
                            lm_market_share_p_pc = 0,
                            mat_grouth_c = 0,
                            mat_grouth_c_pc = 0,
                            mat_grouth_jj = 0,
                            mat_grouth_jj_pc = 0,
                            mat_market_share_l = 0,
                            mat_market_share_l_pc = 0,
                            mat_market_share_p = 0,
                            mat_market_share_p_pc = 0,
                            mat_market_size = 0,
                            mat_market_size_pc = 0,
                            pbp_grouth_c = 0,
                            pbp_grouth_jj = 0,
                            pbp_share_l = 0,
                            pbp_share_p = 0,
                            pbp_market_size = 0,
                            ytd_grouth_c_pc = 0,
                            ytd_grouth_jj_pc = 0,
                            ytd_market_share_l_pc = 0,
                            ytd_market_share_p_pc = 0

                        });
                chart.chart_ytd = ChartByChannelController.GetChartsPercent(query.ToList());

                query = db.v_CHART_BOY_JJ_BY_CHANNEL
                        .Where(p => p.CHART == type && p.TYPE == "PBP" && (p.BRAND < 1000 || p.BRAND > 90000)
                        && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month))
                        .Select(p =>
                        new Models.ChartByChannelModels
                        {
                            order = p.ORDER,
                            vgroup = p.GROUP,
                            brand = p.BRAND,
                            market = p.MARKET,
                            pbp_grouth_c = p.GROWTH_CATHEGORY,
                            pbp_grouth_jj = p.GROWTH_J_J,
                            pbp_share_l = p.LATEST,
                            pbp_share_p = p.PREVIOUS,
                            pbp_market_size = (double?)p.MARKET_SIZE,
                            lm_market_share_l = 0,
                            lm_market_share_p = 0,
                            lm_grouth_c = 0,
                            lm_grouth_jj = 0,
                            channel_name = p.NAME,
                            lm_grouth_c_pc = 0,
                            lm_grouth_jj_pc = 0,
                            lm_market_share_l_pc = 0,
                            lm_market_share_p_pc = 0,
                            mat_grouth_c = 0,
                            mat_grouth_c_pc = 0,
                            mat_grouth_jj = 0,
                            mat_grouth_jj_pc = 0,
                            mat_market_share_l = 0,
                            mat_market_share_l_pc = 0,
                            mat_market_share_p = 0,
                            mat_market_share_p_pc = 0,
                            mat_market_size = 0,
                            mat_market_size_pc = 0,
                            ytd_grouth_c = 0,
                            ytd_grouth_c_pc = 0,
                            ytd_grouth_jj = 0,
                            ytd_grouth_jj_pc = 0,
                            ytd_market_share_l = 0,
                            ytd_market_share_l_pc = 0,
                            ytd_market_share_p = 0,
                            ytd_market_share_p_pc = 0

                        });

                chart.chart_pbp = ChartByChannelController.GetChartsPercent(query.ToList());
            }
            return chart;
        }

        private string getChartTitle(string type)
        {
            string ret = "";
            switch (type)
            {
                case "BOY J&J By Channel":
                    ret = "Spain - Total Business by Key OTC Brand - J&J Value Performance";
                    break;
                default:
                    ret = "Chart";
                    break;
            }
            return ret;
        }


        //
        // GET: /ChartBOYChannel/

        public ActionResult Index()
        {
            ViewBag.Title = "ChartBOYChannel";
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            return View();
        }

        //
        // GET: /ChartBOYChannel/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ChartBOYChannel/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ChartBOYChannel/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ChartBOYChannel/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ChartBOYChannel/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ChartBOYChannel/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ChartBOYChannel/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static string _CHART_PATH = "~/Views/ChartGeneric/";
        private string GENERIC = _CHART_PATH + "Index.cshtml";
        private static string MENU_URL = "StrawmanApp";
        private static string CONTROLLER_NAME = "ChartBOYChannel";
    }
}
