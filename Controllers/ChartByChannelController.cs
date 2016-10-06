using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class ChartByChannelController : Controller
    {
        private Models.ChartsClassesDataContext db;

        public ActionResult GetChart(string type)
        {
            ViewBag.Channel = 0;
            if (type == "CHANNEL") return GetChartByChannel();
            return GetChartByType(type);
        }

        public ActionResult GetChartByType(string type)
        {
                        
            ViewBag.ChartTitle = getChartTitle(type);
            db = new Models.ChartsClassesDataContext();
            db.CommandTimeout = 50000;
            Models.ChartModel chart = GetChartByFranchiseData(type);
            List<Models.ChartByChannelModels> lst = chart.chart_ytd;
            db.Dispose();
            ViewBag.ChartModel = lst;
            return PartialView(GENERIC, chart);
        }

        private string getChartTitle(string type)
        {
            string ret = "";
            switch (type)
            {
                case "FRANCHISE":
                    ret = "Spain - Total Business by Franchise - J&J Value Performance";
                    break;
                case "ORAL":
                    ret = "Spain - Oral Care - J&J Value Performance";
                    break;
                case "FEMENINE":
                    ret = "Spain - Feminine  Care - J&J Value Performance";
                    break;
                case "COMPROMISED SKIN":
                    ret = "Spain - Compromised Skin - J&J Value Performance";
                    break;
                case "BABY CARE":
                    ret = "Spain - Baby Care - J&J Value Performance";
                    break;
                case "ADULTS SKINCARE":
                    ret = "Spain - Adult Skin Care - J&J Value Performance ";
                    break;
                case "KEY SKINCARE":
                    ret = "Spain - Key Skin Care Brands  - J&J Value Performance";
                    break;
            }
            return ret;
        }

        private Models.ChartModel GetChartByFranchiseData(string type)
        {
            Models.ChartModel chart = new Models.ChartModel();
            Models.ChartByChannelModels c = new Models.ChartByChannelModels();
            List<Models.ChartByChannelModels> lst;

            lst = (List<Models.ChartByChannelModels>)GetChartTable("v_CHARTS_LM");
            var query = from p in lst
                        where (p.type == type)
                        select p
                        ;
            chart.chart_lm = GetChartsPc(query.ToList());
            lst = (List<Models.ChartByChannelModels>)GetChartTable("v_CHARTS_MAT_MARKET");
            query = from p in lst
                    where (p.type == type) 
                        select p
                        ;
            chart.chart_mat = GetChartsPc(query.ToList());
            lst = (List<Models.ChartByChannelModels>)GetChartTable("v_CHARTS_YTD");
            query = from p in lst
                    where (p.type == type)
                        select p
                        ;
            chart.chart_ytd = GetChartsPc(query.ToList());
            
            switch (type)
            {
                case "FRANCHISE":
                    lst = (List<Models.ChartByChannelModels>)GetChartTable("v_CHART_PBP_FRANCHISE");
                    query = from p in lst
                            where (p.type == type)
                            select p
                            ;
                    break;
                case "ADULTS SKINCARE":
                    lst = (List<Models.ChartByChannelModels>)GetChartTable("v_CHART_PBP_ADULTS_SKINCARE");
                    query = from p in lst
                            select p
                            ;
                    break;
                default:
                    lst = (List<Models.ChartByChannelModels>)GetChartTable("v_CHARTS_PBP");
                    query = from p in lst
                            where (p.type == type)
                            select p;
                    break;

            }
            chart.chart_pbp = GetChartsPc(query.ToList());
            return chart;
        }

        public ActionResult GetChartByChannel()
        {
            ViewBag.ChartTitle = "Spain - Total Business by Channel - J&J Value Performance";
            db = new Models.ChartsClassesDataContext();
            db.CommandTimeout = 50000;
            List<Models.ChartByChannelModels> lst = GetChartsPc(GetChartByChannelData());
            db.Dispose();            
            return PartialView(BYCHANNEL, lst);
        }

        private List<Models.ChartByChannelModels> GetChartByChannelData()
        {
            var query = from p in db.v_WRK_CHART_BY_CHANNEL
                        where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.ChartByChannelModels
                        {
                            channel_name = p.NAME,
                            mat_market_size = p.MAT_MARKET_SIZE,
                            mat_market_share_l = p.MAT_MARKET_SHARE_L,
                            mat_market_share_p = p.MAT_MARKET_SHARE_P,
                            ytd_market_share_l = p.YTD_MARKET_SHARE_L,
                            ytd_market_share_p = p.YTD_MARKET_SHARE_P,
                            lm_market_share_l = p.LM_MARKET_SHARE_L,
                            lm_market_share_p = p.LM_MARKET_SHARE_P,
                            mat_grouth_c = p.MAT_GROUTH_C,
                            mat_grouth_jj = p.MAT_GROUTH_JJ,
                            ytd_grouth_c = p.YTD_GROUTH_C,
                            ytd_grouth_jj = p.YTD_GROUTH_JJ,
                            lm_grouth_c = p.LM_GROUTH_C,
                            lm_grouth_jj = p.LM_GROUTH_JJ,
                            pbp_market_size = (double?)p.MAT_MARKET_SIZE,
                            pbp_grouth_c = p.LM_GROUTH_C,
                            pbp_grouth_jj = p.LM_GROUTH_JJ,
                            pbp_share_p = p.LM_MARKET_SHARE_P,
                            pbp_share_l = p.LM_MARKET_SHARE_L
                        };
            return query.ToList();
        }

        public static List<Models.ChartByChannelModels> GetChartsPercent(List<Models.ChartByChannelModels> list)
        {
            return GetChartsPc(list);
        }

        private static List<Models.ChartByChannelModels> GetChartsPc(List<Models.ChartByChannelModels> list)
        {
            decimal total_market = 0;
            decimal mat_market_share_l = 0;
            decimal mat_market_share_p = 0;
            decimal ytd_market_share_l = 0;
            decimal ytd_market_share_p = 0;
            decimal lm_market_share_p = 0;
            decimal lm_market_share_l = 0;
            decimal share = 0;
            decimal mat_grouth_c = 0;
            decimal mat_grouth_jj = 0;
            decimal ytd_grouth_c = 0;
            decimal ytd_grouth_jj = 0;
            decimal lm_grouth_c = 0;
            decimal lm_grouth_jj = 0;
            decimal grouth = 0;
            decimal pbp_grouth_c = 0;
            decimal pbp_grouth_jj = 0;
            decimal pbp_share_l = 0;
            decimal pbp_share_p = 0;
            decimal max_grouth = 0;
            decimal max_share = 0;
            int counter = 0;
            //El valor máximo de cada barra viene dado por la suma de las barras de las columnas de los grupos size, share y grouth.
            //La columna size tiene tantas barras como catergorías tenga la consulta.
            //Las columnas share y grouth tienen dos barras y tres columnas cada una por categoría.
            //En el caso de los gráficos PBP (LM) las columnas share y grouth tienen dos barras y una columa cada una por categoría.
            //La variable share almacena la suma de las cifras de las dos barras en la columna share.
            //La variable grouth almacena la suma de las cifras de las dos barras en la coluna grouth.
            foreach (Models.ChartByChannelModels item in list)
            {
                total_market += (item.mat_market_size > 0) ? (decimal)item.mat_market_size : (decimal)item.pbp_market_size;
                mat_market_share_l = ParseAbsolute(item.mat_market_share_l);
                max_share = (max_share< mat_market_share_l)?mat_market_share_l:max_share;
                share += ParseAbsolute(item.mat_market_share_l);
                mat_market_share_p = ParseAbsolute(item.mat_market_share_p);
                max_share = (max_share < mat_market_share_p) ? mat_market_share_p : max_share;
                share += ParseAbsolute(item.mat_market_share_p);
                ytd_market_share_l = ParseAbsolute(item.ytd_market_share_l);
                max_share = (max_share < ytd_market_share_l) ? ytd_market_share_l : max_share;
                share += ParseAbsolute(item.ytd_market_share_l);
                ytd_market_share_p = ParseAbsolute(item.ytd_market_share_p);
                max_share = (max_share < ytd_market_share_p) ? ytd_market_share_p : max_share;
                share += ParseAbsolute(item.ytd_market_share_p);
                lm_market_share_l = ParseAbsolute(item.lm_market_share_l);
                max_share = (max_share < lm_market_share_l) ? lm_market_share_l : max_share;
                share += ParseAbsolute(item.lm_market_share_l);
                lm_market_share_p = ParseAbsolute(item.lm_market_share_p);
                max_share = (max_share < lm_market_share_p) ? lm_market_share_p : max_share;
                share += ParseAbsolute(item.lm_market_share_p);
                pbp_share_l = ParseAbsolute(item.pbp_share_l);
                max_share = (max_share < pbp_share_l) ? pbp_share_l : max_share;
                share += ParseAbsolute(item.pbp_share_l);
                pbp_share_p = ParseAbsolute(item.pbp_share_p);
                max_share = (max_share < pbp_share_p) ? pbp_share_p : max_share;
                share += ParseAbsolute(item.pbp_share_p);
                

                mat_grouth_c = ParseAbsolute(item.mat_grouth_c);
                max_grouth = (max_grouth < mat_grouth_c) ? mat_grouth_c : max_grouth;
                grouth += ParseAbsolute(item.mat_grouth_c);
                mat_grouth_jj = ParseAbsolute(item.mat_grouth_jj);
                max_grouth = (max_grouth < mat_grouth_jj) ? mat_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.mat_grouth_jj);
                ytd_grouth_c = ParseAbsolute(item.ytd_grouth_c);
                max_grouth = (max_grouth < ytd_grouth_c) ? ytd_grouth_c : max_grouth;
                grouth += ParseAbsolute(item.ytd_grouth_c);
                ytd_grouth_jj = ParseAbsolute(item.ytd_grouth_jj);
                max_grouth = (max_grouth < ytd_grouth_jj) ? ytd_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.ytd_grouth_jj);
                lm_grouth_c = ParseAbsolute(item.lm_grouth_c);
                max_grouth = (max_grouth < lm_grouth_c) ? lm_grouth_c : max_grouth;
                grouth += ParseAbsolute(item.lm_grouth_c);
                lm_grouth_jj = ParseAbsolute(item.lm_grouth_jj);
                max_grouth = (max_grouth < lm_grouth_jj) ? lm_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.lm_grouth_jj);
                pbp_grouth_c = ParseAbsolute(item.pbp_grouth_c);
                max_grouth = (max_grouth < pbp_grouth_c) ? pbp_grouth_c : max_grouth;
                grouth += ParseAbsolute(item.pbp_grouth_c);
                pbp_grouth_jj = ParseAbsolute(item.pbp_grouth_jj);
                max_grouth = (max_grouth < pbp_grouth_jj) ? pbp_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.pbp_grouth_jj);
                
                counter++;
                
            }
            
            //ajustamos el máximo del ancho de la barra a la media del resultado.
            //recalculamos para el caso especial de PBP
            if (pbp_grouth_c > 0 || pbp_grouth_jj > 0 || pbp_share_l > 0 || pbp_share_p > 0)
            {
                share = share / ((list.Count) / 2);
                grouth = grouth / ((list.Count) / 2);
            }
            else
            {
                share = share / ((list.Count * 3) / 2);
                grouth = grouth / ((list.Count * 3) / 2);
            }
            //ajustamos el máximo al tope del 90% en el caso de que exista un valor mayor que la media (daría porcentajes superiores al 90%, máximo permitido para visualizar correctamente)
            share = (share < max_share) ? max_share : share;
            grouth = (grouth < max_grouth) ? max_grouth : grouth;
            foreach (Models.ChartByChannelModels item in list)
            {                
                item.mat_market_size_pc = CalcPc((decimal)total_market, (decimal)item.mat_market_size);
                item.pbp_market_size_pc = CalcPc((decimal)total_market, (decimal)item.pbp_market_size);
                item.mat_market_share_l_pc = (item.mat_market_share_l == null)? 0 :CalcPc(share, (decimal)(item.mat_market_share_l));
                item.mat_market_share_p_pc = (item.mat_market_share_p == null) ? 0 : CalcPc(share, (decimal)(item.mat_market_share_p));
                item.ytd_market_share_l_pc = (item.ytd_market_share_l == null) ? 0 : CalcPc(share, (decimal)(item.ytd_market_share_l));
                item.ytd_market_share_p_pc = (item.ytd_market_share_p == null) ? 0 : CalcPc(share, (decimal)(item.ytd_market_share_p));
                item.lm_market_share_l_pc = (item.lm_market_share_l == null) ? 0 : CalcPc(share, (decimal)(item.lm_market_share_l));
                item.lm_market_share_p_pc = (item.lm_market_share_p == null) ? 0 : CalcPc(share, (decimal)(item.lm_market_share_p));
                item.pbp_share_l_pc = (item.pbp_share_l == null) ? 0 : CalcPc(share, (decimal)(item.pbp_share_l));
                item.pbp_share_p_pc = (item.pbp_share_p == null) ? 0 : CalcPc(share, (decimal)(item.pbp_share_p));
                item.mat_grouth_c_pc = (item.mat_grouth_c == null) ? 0 : CalcPc(grouth, (decimal)(item.mat_grouth_c));
                item.mat_grouth_jj_pc = (item.mat_grouth_jj == null) ? 0 : CalcPc(grouth, (decimal)(item.mat_grouth_jj));
                item.ytd_grouth_c_pc = (item.ytd_grouth_c == null) ? 0 : CalcPc(grouth, (decimal)(item.ytd_grouth_c));
                item.ytd_grouth_jj_pc = (item.ytd_grouth_jj == null) ? 0 : CalcPc(grouth, (decimal)(item.ytd_grouth_jj));
                item.lm_grouth_c_pc = (item.lm_grouth_c == null) ? 0 : CalcPc(grouth, (decimal)(item.lm_grouth_c));
                item.lm_grouth_jj_pc = (item.lm_grouth_jj == null) ? 0 : CalcPc(grouth, (decimal)(item.lm_grouth_jj));
                item.pbp_grouth_jj_pc = (item.pbp_grouth_jj == null) ? 0 : CalcPc(grouth, (decimal)(item.pbp_grouth_jj));
                item.pbp_grouth_c_pc = (item.pbp_grouth_c == null) ? 0 : CalcPc(grouth, (decimal)(item.pbp_grouth_c));
            }
            return list;
        }

        private decimal CheckNull(decimal? nullable)
        {
            if (nullable == null) nullable = 0;
            return (decimal)nullable;
        }
        private static decimal ParseAbsolute(decimal? value)
        {
            decimal ret = 0;
            if (value != null) ret = (decimal)((value < 0) ? value * -1 : value);
            return ret;
            
        }
        private static decimal CalcPc(decimal total, decimal parcial)
        {
            decimal ret = 0;

            if (parcial != 0 && total != 0) ret = (ParseAbsolute(parcial) * 90) / total;
                
            return ret;
        }
        //
        // GET: /ChartByChannel/

        public ActionResult Index()
        {
            ViewBag.MenuUrl = MENU_URL;
            ViewBag.TabUrl = CONTROLLER_NAME + "/Index";
            ViewBag.Channel = 0;
            ViewBag.YearPeriod = Helpers.PeriodUtil.Year;
            ViewBag.MonthPeriod = new DateTime(Helpers.PeriodUtil.Year, Helpers.PeriodUtil.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture); 
            return View();
        }

        //
        // GET: /ChartByChannel/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ChartByChannel/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ChartByChannel/Create

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
        // GET: /ChartByChannel/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ChartByChannel/Edit/5

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
        // GET: /ChartByChannel/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ChartByChannel/Delete/5

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


        #region Functions
        private void SetSessionObject(string obj_name, object obj)
        {
            Session[obj_name] = obj;
        }
        private object GetSessionObject(string obj_name)
        {
            return Session[obj_name];
        }

        public List<Models.ChartByChannelModels> GetChartTable(string table)
        {
            List<Models.ChartByChannelModels> chart = null;
            if (GetSessionObject(table) == null)
            {
                SetChartTable(table);
            }
            chart = (List<Models.ChartByChannelModels>)GetSessionObject(table);
            return chart;
        }

        private void SetChartTable(string table)
        {
            List<Models.ChartByChannelModels> lst = null;
            using (Models.ChartsClassesDataContext ddb = new Models.ChartsClassesDataContext())
            {
                switch (table)
                {
                    case "v_CHARTS_LM":
                        var query = from p in ddb.v_CHARTS_LM
                            where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                            select new Models.ChartByChannelModels
                            {
                                vgroup = p.GROUP,
                                brand = p.BRAND,
                                market = p.MARKET,
                                lm_market_share_l = p.LATEST,
                                lm_market_share_p = p.PREVIOUS,
                                lm_grouth_c = p.GROUTH_CATEGORY,
                                lm_grouth_jj = p.GROUTH_J_J,
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
                                ytd_market_share_p_pc = 0,
                                type = p.CHART
                            };
                            lst = (query.ToList());
                        break;
                    case "v_CHARTS_MAT_MARKET":
                        query = from p in db.v_CHARTS_MAT_MARKET
                            where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                            select new Models.ChartByChannelModels
                            {
                                vgroup = p.GROUP,
                                brand = p.BRAND,
                                market = p.MARKET,
                                mat_market_share_l = p.LATEST,
                                mat_market_share_p = p.PREVIOUS,
                                mat_grouth_c = p.GROUTH_CATEGORY,
                                mat_grouth_jj = p.GROUTH_J_J,
                                mat_market_size = p.MARKET_SIZE,
                                lm_market_share_l = p.LATEST,
                                lm_market_share_p = p.PREVIOUS,
                                lm_grouth_c = p.GROUTH_CATEGORY,
                                lm_grouth_jj = p.GROUTH_J_J,
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
                                ytd_market_share_p_pc = 0,
                                type = p.CHART
                            };
                            lst = (query.ToList());
                        break;
                    case "v_CHARTS_YTD":
                        query = from p in db.v_CHARTS_YTD
                            where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                            select new Models.ChartByChannelModels
                            {
                                vgroup = p.GROUP,
                                brand = p.BRAND,
                                market = p.MARKET,
                                ytd_market_share_l = p.LATEST,
                                ytd_market_share_p = p.PREVIOUS,
                                ytd_grouth_c = p.GROUTH_CATEGORY,
                                ytd_grouth_jj = p.GROUTH_J_J,
                                lm_market_share_l = p.LATEST,
                                lm_market_share_p = p.PREVIOUS,
                                lm_grouth_c = p.GROUTH_CATEGORY,
                                lm_grouth_jj = p.GROUTH_J_J,
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
                                ytd_market_share_p_pc = 0,
                                type = p.CHART
                            };
                        lst = (query.ToList());
                        break;
                    case "v_CHARTS_PBP":
                        query = from p in db.v_CHARTS_PBP
                            where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                            select new Models.ChartByChannelModels
                            {
                                vgroup = p.GROUP,
                                brand = p.BRAND,
                                market = p.MARKET,
                                pbp_grouth_c = p.GROUTH_CATEGORY,
                                pbp_grouth_jj = p.GROUTH_J_J,
                                pbp_share_l = p.LATEST,
                                pbp_share_p = p.PREVIOUS,
                                pbp_market_size = p.MARKET_SIZE,
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
                                ytd_market_share_p_pc = 0,
                                type = p.CHART
                            };
                        lst = query.ToList();
                    break;
                    case "v_CHART_PBP_FRANCHISE":
                        query = from p in db.v_CHART_PBP_FRANCHISE
                                where (p.CHART == "FRANCHISE")
                                && (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                                select new Models.ChartByChannelModels
                                {
                                    vgroup = p.GROUP,
                                    brand = p.BRAND,
                                    market = p.MARKET,
                                    pbp_grouth_c = p.GROUTH_CATEGORY,
                                    pbp_grouth_jj = p.GROUTH_J_J,
                                    pbp_share_l = (decimal?)p.LATEST,
                                    pbp_share_p = (decimal?)p.PREVIOUS,
                                    pbp_market_size = p.MARKET_SIZE,
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
                                    ytd_market_share_p_pc = 0,
                                    type = p.CHART
                                };
                        lst = query.ToList();
                    break;
                    case "v_CHART_PBP_ADULTS_SKINCARE":
                        query = from p in db.v_CHART_PBP_ADULTS_SKINCARE
                                where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                                select new Models.ChartByChannelModels
                                {
                                    vgroup = p.GROUP,
                                    brand = p.BRAND,
                                    market = p.MARKET,
                                    pbp_grouth_c = (decimal?)p.GROUTH_CATEGORY,
                                    pbp_grouth_jj = (decimal?)p.GROUTH_J_J,
                                    pbp_share_l = (decimal?)p.LATEST,
                                    pbp_share_p = (decimal?)p.PREVIOUS,
                                    pbp_market_size = p.MARKET_SIZE,
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
                                    ytd_market_share_p_pc = 0,
                                    type = p.CHART
                                };
                        lst = query.ToList();
                        break;
                    default:
                        break;
                }
            }
            SetSessionObject(table, lst);
        }

        #endregion
        private static string _PATH = "~/Views/ChartByChannel/";
        private string BYCHANNEL = _PATH + "_ByChannel.cshtml";
        private string GENERIC = _PATH + "_GenericChart.cshtml";
        private static string MENU_URL = "StrawmanApp";
        private static string CONTROLLER_NAME = "ChartByChannel";
    }
}
