using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class ChartJJVInternalController : Controller
    {
        public ActionResult GetChart(string type)
        {            
            return GetChartByType(type);
        }

        public ActionResult GetChartByType(string type)
        {
            Models.ChartModel chart = new Models.ChartModel();

            ViewBag.ChartTitle = getChartTitle(type);
            switch(type){
                case "ByChannel":
                    chart = GetChartByChannelData(type);
                    break;
                case "ByFranchise":
                    chart = GetChartByFranchiseData(type);
                    break;
            }
            List<Models.ChartByChannelModels> lst = chart.chart_ytd;
            ViewBag.ChartModel = lst;
            
            return PartialView(GENERIC, chart);
        }
        public static Models.ChartModel GetChartDataExport(string type)
        {
            Models.ChartModel model = new Models.ChartModel();
            switch (type)
            {
                case "ByChannel":
                    model = GetChartByChannelData(type);
                    break;
                case "ByFranchise":
                    model = GetChartByFranchiseData(type);
                    break;
            }
            return model;
        }
        
        private static Models.ChartModel GetChartByChannelData(string type)
        {
            Models.ChartModel chart = new Models.ChartModel();
            Models.ChartByChannelModels c = new Models.ChartByChannelModels();
            List<Entities.v_CHART_JJ_V_INTERNAL_BY_CHANNEL> channel_lst = new List<Entities.v_CHART_JJ_V_INTERNAL_BY_CHANNEL>();
            using (Entities.godzillaChartsEntities db = new Entities.godzillaChartsEntities())
            {
                channel_lst = db.v_CHART_JJ_V_INTERNAL_BY_CHANNEL                            
                            .OrderBy(m => m.ORDER).ToList();
            }
            var query = channel_lst
                        .Where(p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        .Select(p => new Models.ChartByChannelModels
                            {
                                order = p.ORDER,
                                brand = p.ID,
                                market = p.ID,
                                vgroup = p.ID,
                                lm_market_share_l = p.MTG_SHARE_LATEST,
                                lm_market_share_p = p.MTG_SHARE_PREVIOUS,
                                lm_grouth_c = p.LM_GROWTH_CATEGORY,
                                lm_grouth_jj = p.LM_GROWTH_JJ,
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

                query = channel_lst
                        .Where (p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        .Select(p=>
                        new Models.ChartByChannelModels
                        {
                            order = p.ORDER,
                            brand = p.ID,
                            market = p.ID,
                            vgroup = p.ID,
                            mat_market_share_l = p.MARKET_SHARE_LATEST,
                            mat_market_share_p = p.MARKET_SHARE_PREVIOUS,
                            mat_grouth_c = p.MAT_GROWTH_CATEGORY,
                            mat_grouth_jj = p.MAT_GROWTH_JJ,
                            mat_market_size = p.MARKET_SIZE,
                            lm_market_share_l = p.MTG_SHARE_LATEST,
                            lm_market_share_p = p.MTG_SHARE_PREVIOUS,
                            lm_grouth_c = p.LM_GROWTH_CATEGORY,
                            lm_grouth_jj = p.LM_GROWTH_JJ,
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
                chart.chart_mat = ChartByChannelController.GetChartsPercent(query.ToList());
 
                query = channel_lst
                        .Where (p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        .Select(p =>
                        new Models.ChartByChannelModels
                        {
                            order = p.ORDER,
                            brand = p.ID,
                            market = p.ID,
                            vgroup = p.ID,
                            ytd_market_share_l = p.YTD_SHARE_LATEST,
                            ytd_market_share_p = p.YTD_SHARE_PREVIOUS,
                            ytd_grouth_c = p.YTD_GROWTH_CATEGORY,
                            ytd_grouth_jj = p.YTD_GROWTH_JJ,
                            lm_market_share_l = p.MTG_SHARE_LATEST,
                            lm_market_share_p = p.MTG_SHARE_PREVIOUS,
                            lm_grouth_c = p.LM_GROWTH_CATEGORY,
                            lm_grouth_jj = p.LM_GROWTH_JJ,
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

                query = channel_lst
                        .Where (p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        .Select(p =>
                        new Models.ChartByChannelModels
                        {
                            order = p.ORDER,
                            brand = p.ID,
                            market = p.ID,
                            vgroup = p.ID,
                            pbp_grouth_c = p.PBP_GROWTH_CATEGORY,
                            pbp_grouth_jj = p.PBP_GROWTH_JJ,
                            pbp_share_l = p.PBP_SHARE_LATEST,
                            pbp_share_p = p.PBP_SHARE_PREVIOUS,
                            pbp_market_size = (double?)p.PBP_MARKET_SIZE,
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
            
            return chart;
        }

        private static Models.ChartModel GetChartByFranchiseData(string type)
        {
            Models.ChartModel chart = new Models.ChartModel();
            Models.ChartByChannelModels c = new Models.ChartByChannelModels();
            List<Entities.v_CHART_JJ_V_INTERNAL_BY_FRANCHISE> channel_lst = new List<Entities.v_CHART_JJ_V_INTERNAL_BY_FRANCHISE>();
            using (Entities.godzillaChartsEntities db = new Entities.godzillaChartsEntities())
            {
                channel_lst = db.v_CHART_JJ_V_INTERNAL_BY_FRANCHISE.OrderBy(m => m.ORDER).ToList();
            }
            var query = channel_lst
                        .Where (p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        .Select(p => new Models.ChartByChannelModels
            {
                order = p.ORDER,
                brand = p.ID,
                market = p.ID,
                vgroup = p.ID,
                lm_market_share_l = p.MTG_SHARE_LATEST,
                lm_market_share_p = p.MTG_SHARE_PREVIOUS,
                lm_grouth_c = p.LM_GROWTH_CATEGORY,
                lm_grouth_jj = p.LM_GROWTH_JJ,
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

            query = channel_lst
                    .Where (p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                    .Select(p =>
                    new Models.ChartByChannelModels
                    {
                        order = p.ORDER,
                        brand = p.ID,
                        market = p.ID,
                        vgroup = p.ID,
                        mat_market_share_l = p.MARKET_SHARE_LATEST,
                        mat_market_share_p = p.MARKET_SHARE_PREVIOUS,
                        mat_grouth_c = p.MAT_GROWTH_CATEGORY,
                        mat_grouth_jj = p.MAT_GROWTH_JJ,
                        mat_market_size = p.MARKET_SIZE,
                        lm_market_share_l = p.MTG_SHARE_LATEST,
                        lm_market_share_p = p.MTG_SHARE_PREVIOUS,
                        lm_grouth_c = p.LM_GROWTH_CATEGORY,
                        lm_grouth_jj = p.LM_GROWTH_JJ,
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
            chart.chart_mat = ChartByChannelController.GetChartsPercent(query.ToList());

            query = channel_lst
                    .Where (p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                    .Select(p =>
                    new Models.ChartByChannelModels
                    {
                        order = p.ORDER,
                        brand = p.ID,
                        market = p.ID,
                        vgroup = p.ID,
                        ytd_market_share_l = p.YTD_SHARE_LATEST,
                        ytd_market_share_p = p.YTD_SHARE_PREVIOUS,
                        ytd_grouth_c = p.YTD_GROWTH_CATEGORY,
                        ytd_grouth_jj = p.YTD_GROWTH_JJ,
                        lm_market_share_l = p.MTG_SHARE_LATEST,
                        lm_market_share_p = p.MTG_SHARE_PREVIOUS,
                        lm_grouth_c = p.LM_GROWTH_CATEGORY,
                        lm_grouth_jj = p.LM_GROWTH_JJ,
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

            query = channel_lst
                    .Where (p=>p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                    .Select(p =>
                    new Models.ChartByChannelModels
                    {
                        order = p.ORDER,
                        brand = p.ID,
                        market = p.ID,
                        vgroup = p.ID,
                        pbp_grouth_c = p.PBP_GROWTH_CATEGORY,
                        pbp_grouth_jj = p.PBP_GROWTH_JJ,
                        pbp_share_l = p.PBP_SHARE_LATEST,
                        pbp_share_p = p.PBP_SHARE_PREVIOUS,
                        pbp_market_size = (double?)p.PBP_MARKET_SIZE,
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


        #region Default Functions
        //
        // GET: /ChartJJVInternal/

        public ActionResult Index()
        {
            ViewBag.Title = CONTROLLER_NAME;
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            return View();
        }

        //
        // GET: /ChartJJVInternal/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ChartJJVInternal/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ChartJJVInternal/Create

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
        // GET: /ChartJJVInternal/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ChartJJVInternal/Edit/5

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
        // GET: /ChartJJVInternal/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ChartJJVInternal/Delete/5

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
        #endregion

        #region Constants

        private static string _CHART_PATH = "~/Views/ChartJJVInternal/";
        private string GENERIC = _CHART_PATH + "_GenericChart.cshtml";
        private static string MENU_URL = "StrawmanApp";
        private static string CONTROLLER_NAME = "ChartJJVInternal";

        #endregion

    }
}
