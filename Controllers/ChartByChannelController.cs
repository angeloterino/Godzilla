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
            if (type == "CHANNEL") return GetChartByChannel(type);
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
            dynamic query = null;
            switch(type){
                case "FRANCHISE":
                case "ORAL":
                    lst = GetChartsPc((List<Models.ChartByChannelModels>)GetChartByChannelData(type));
                    chart.chart_mat = lst.Select(m => new Models.ChartByChannelModels
                    {
                        channel_name = m.channel_name,
                        mat_market_share_l = m.mat_market_share_l,
                        mat_market_share_l_pc = m.mat_market_share_l_pc,
                        mat_market_share_p = m.mat_market_share_p,
                        mat_market_share_p_pc = m.mat_market_share_p_pc,
                        mat_grouth_c = m.mat_grouth_c,
                        mat_market_size = m.mat_market_size,
                        mat_grouth_c_pc = m.mat_grouth_c_pc,
                        mat_grouth_jj = m.mat_grouth_jj,
                        mat_grouth_jj_pc = m.mat_grouth_jj_pc,
                        mat_market_size_pc = m.mat_market_size_pc,
                        vid = m.vid,
                        brand = m.vid,
                        market = m.vid,
                        vgroup =m.vid
                    }).ToList();
                    chart.chart_lm = lst.Select(m => new Models.ChartByChannelModels
                    {
                        channel_name = m.channel_name,
                        lm_grouth_c = m.lm_grouth_c,
                        lm_grouth_c_pc = m.lm_grouth_c_pc,
                        lm_grouth_jj = m.lm_grouth_jj,
                        lm_grouth_jj_pc = m.lm_grouth_jj_pc,
                        lm_market_share_l = m.lm_market_share_l,
                        lm_market_share_l_pc = m.lm_market_share_l_pc,
                        lm_market_share_p = m.lm_market_share_p,
                        lm_market_share_p_pc = m.mat_market_size_pc,
                        vid = m.vid,
                        brand = m.vid,
                        market = m.vid,
                        vgroup =m.vid
                    }).ToList();
                    chart.chart_ytd = lst.Select(m => new Models.ChartByChannelModels
                    {
                        channel_name = m.channel_name,
                        ytd_grouth_c = m.ytd_grouth_c,
                        ytd_grouth_c_pc = m.ytd_grouth_c_pc,
                        ytd_grouth_jj = m.ytd_grouth_jj,
                        ytd_grouth_jj_pc = m.ytd_grouth_jj_pc,
                        ytd_market_share_l = m.ytd_market_share_l,
                        ytd_market_share_l_pc = m.ytd_market_share_l_pc,
                        ytd_market_share_p = m.ytd_market_share_p,
                        ytd_market_share_p_pc = m.ytd_market_share_p_pc,
                        vid = m.vid,
                        brand = m.vid,
                        market = m.vid,
                        vgroup =m.vid
                    }).ToList();
                    chart.chart_pbp = lst.Select(m => new Models.ChartByChannelModels
                    {
                        channel_name = m.channel_name,
                        pbp_grouth_c = m.pbp_grouth_c,
                        pbp_grouth_c_pc = m.pbp_grouth_c_pc,
                        pbp_grouth_jj = m.pbp_grouth_jj,
                        pbp_grouth_jj_pc = m.pbp_grouth_jj_pc,
                        pbp_market_size = m.pbp_market_size,
                        pbp_market_size_pc = m.pbp_market_size_pc,
                        pbp_share_l = m.pbp_share_l,
                        pbp_share_l_pc = m.pbp_share_l_pc,
                        pbp_share_p = m.pbp_share_p,
                        pbp_share_p_pc = m.pbp_share_p_pc,
                        vid = m.vid,
                        brand = m.vid,
                        market = m.vid,
                        vgroup = m.vid
                    }).ToList();
                    break;
                default:
                lst = (List<Models.ChartByChannelModels>)GetChartTable("v_CHARTS_LM");
                query = from p in lst
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
                break;
            }
            switch (type)
            {
                case "FRANCHISE":
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
            if(chart.chart_pbp == null) chart.chart_pbp = GetChartsPc(query.ToList());
            return chart;
        }

        public ActionResult GetChartByChannel(string type)
        {
            ViewBag.ChartTitle = "Spain - Total Business by Channel - J&J Value Performance";
            db = new Models.ChartsClassesDataContext();
            db.CommandTimeout = 50000;
            List<Models.ChartByChannelModels> lst = GetChartsPc(GetChartByChannelData(type));
            db.Dispose();            
            return PartialView(BYCHANNEL, lst);
        }

        private List<Models.ChartByChannelModels> GetChartByChannelData(string type)
        {
            var typ = ((List<StrawmanDBLibray.Entities.WRK_VIEWS_VARIABLES>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_VIEWS_VARIABLES))
                        .Where(m => m.VIEW == "CHART BY CONFIG" && m.NAME == type).Select(m => m.VALUE).FirstOrDefault();
            if (typ == null) return null;

            var grp = (List<StrawmanDBLibray.Entities.GROUP_MASTER>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_MASTER);
            var grpd = grp.Join(((List<StrawmanDBLibray.Entities.GROUP_CONFIG>)Helpers.StrawmanDBLibrayData.Get(StrawmanDBLibray.Classes.StrawmanDataTables.GROUP_CONFIG))
                            .Where(m => m.TYPE_ID == int.Parse(typ)).AsEnumerable(),
                            m => new { _id = m.ID }, c => new { _id = (decimal)c.GROUP_ID },
                            (m, c) => new
                            {
                                order = c.ID,
                                id = c.BRAND,
                                name = m.NAME,
                                source = c.SOURCE,
                                brand = c.BRAND,
                                market = c.MARKET
                            });
            if (grpd == null) return null;

            MarketViewFranchiseController mfranchise = new MarketViewFranchiseController();
            BrandViewFranchiseController bfranchise = new BrandViewFranchiseController();

            MarketViewChannelController mchannel = new MarketViewChannelController();
            BrandViewChannelController bchannel = new BrandViewChannelController();

            MarketViewController mstrawm = new MarketViewController();
            BrandViewController bstrawm = new BrandViewController();

            string vtype = Classes.StrawmanViews.MAT;

            var f_data_market_MAT = mfranchise.GetMarketViewData(vtype);
            var f_data_brand_MAT = bfranchise.GetBrandViewData(vtype);

            var c_data_market_MAT = mchannel.GetMarketViewData(vtype);
            var c_data_brand_MAT = bchannel.GetBrandViewData(vtype);

            var s_data_market_MAT = (List<Models.StrawmanViewSTDModel>)mstrawm.GetMarketViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_MARKET_MAT);
            var s_data_brand_MAT = (List<Models.StrawmanViewSTDModel>)bstrawm.GetBrandViewData(StrawmanDBLibray.Classes.StrawmanDataTables.WRK_BRAND_MAT);

            var fdata_MAT = GetCalcChartData(f_data_market_MAT, f_data_brand_MAT);

            var cdata_MAT = GetCalcChartData(c_data_market_MAT, c_data_brand_MAT);

            var sdata_MAT = GetCalcChartData(s_data_market_MAT, s_data_brand_MAT,"STRAWMAN");

            vtype = Classes.StrawmanViews.YTD;
            var f_data_market_YTD = mfranchise.GetMarketViewData(vtype);
            var f_data_brand_YTD = bfranchise.GetBrandViewData(vtype);

            var c_data_market_YTD = mchannel.GetMarketViewData(vtype);
            var c_data_brand_YTD = bchannel.GetBrandViewData(vtype);

            var fdata_YTD = GetCalcChartData(f_data_market_YTD, f_data_brand_YTD);
            var cdata_YTD = GetCalcChartData(c_data_market_YTD, c_data_brand_YTD);

            vtype = Classes.StrawmanViews.MONTH;
            var f_data_market_LM = mfranchise.GetMarketViewData(vtype);
            var f_data_brand_LM = bfranchise.GetBrandViewData(vtype);

            var c_data_market_LM = mchannel.GetMarketViewData(vtype);
            var c_data_brand_LM = bchannel.GetBrandViewData(vtype);

            var fdata_LM = GetCalcChartData(f_data_market_LM, f_data_brand_LM);
            var cdata_LM = GetCalcChartData(c_data_market_LM, c_data_brand_LM);

            vtype = Classes.StrawmanViews.BOY;
            var f_data_market_BOY = mfranchise.GetMarketViewData(vtype);
            var f_data_brand_BOY = bfranchise.GetBrandViewData(vtype);

            var c_data_market_BOY = mchannel.GetMarketViewData(vtype);
            var c_data_brand_BOY = bchannel.GetBrandViewData(vtype);


            vtype = Classes.StrawmanViews.TOTAL;
            var f_data_market_PBP = mfranchise.GetMarketViewData(vtype).Join(f_data_market_BOY, t=>new{_id=t.vid}, b=>new{_id= b.vid},(t,b)=> new Models.MarketViewChannelModels
            {
                vid = t.vid,
                col1 = t.col3,
                col2 = b._le
            }).ToList();
            var f_data_brand_PBP = bfranchise.GetBrandViewData(vtype).Join(f_data_brand_BOY, t => new { _id = t.vid }, b => new { _id = b.vid }, (t, b) => new Models.MarketViewChannelModels
            {
                vid = t.vid,
                col1 = t.col3,
                col2 = b._le
            }).ToList();

            var c_data_market_PBP = mchannel.GetMarketViewData(vtype).Join(c_data_market_BOY, t => new { _id = t.vid }, b => new { _id = b.vid }, (t, b) => new Models.StrawmanViewSTDModel
            {
                vid = t.vid,
                col1 = t.col3,
                col2 = b._le
            }).ToList();
            var c_data_brand_PBP = bchannel.GetBrandViewData(vtype).Join(c_data_brand_BOY, t => new { _id = t.vid }, b => new { _id = b.vid }, (t, b) => new Models.StrawmanViewSTDModel
            {
                vid = t.vid,
                col1 = t.col3,
                col2 = b._le
            }).ToList();

            var fdata_PBP = GetCalcChartData(f_data_market_PBP, f_data_brand_PBP);
            var cdata_PBP = GetCalcChartData(c_data_market_PBP, c_data_brand_PBP);

            return grpd.Select(p => new Models.ChartByChannelModels
            {
                channel_name = p.name,
                vid = (decimal)p.id,
                mat_market_size = p.source == "FRANCHISE"? fdata_MAT.Where(m=>m.vid == p.id).FirstOrDefault().market_size: p.source == "STRAWMAN"? sdata_MAT.Where(m=>m.brand == p.brand && m.market == p.market).FirstOrDefault().market_size: cdata_MAT.Where(m=>m.vid == p.id).FirstOrDefault().market_size,
                mat_market_share_l = p.source == "FRANCHISE" ? fdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().market_share_l : cdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().market_share_l,
                mat_market_share_p = p.source == "FRANCHISE" ? fdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().market_share_p : cdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().market_share_p,
                ytd_market_share_l = p.source == "FRANCHISE" ? fdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().market_share_l : cdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().market_share_l,
                ytd_market_share_p = p.source == "FRANCHISE" ? fdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().market_share_p : cdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().market_share_p,
                lm_market_share_l = p.source == "FRANCHISE" ? fdata_LM.Where(m => m.vid == p.id).FirstOrDefault().market_share_l : cdata_LM.Where(m => m.vid == p.id).FirstOrDefault().market_share_l,
                lm_market_share_p = p.source == "FRANCHISE" ? fdata_LM.Where(m => m.vid == p.id).FirstOrDefault().market_share_p : cdata_LM.Where(m => m.vid == p.id).FirstOrDefault().market_share_p,
                mat_grouth_c = p.source == "FRANCHISE" ? fdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().grouth_c : cdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().grouth_c,
                mat_grouth_jj = p.source == "FRANCHISE" ? fdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj : cdata_MAT.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj,
                ytd_grouth_c = p.source == "FRANCHISE" ? fdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().grouth_c : cdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().grouth_c,
                ytd_grouth_jj = p.source == "FRANCHISE" ? fdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj : cdata_YTD.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj,
                lm_grouth_c = p.source == "FRANCHISE" ? fdata_LM.Where(m => m.vid == p.id).FirstOrDefault().grouth_c : cdata_LM.Where(m => m.vid == p.id).FirstOrDefault().grouth_c,
                lm_grouth_jj = p.source == "FRANCHISE" ? fdata_LM.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj : cdata_LM.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj,
                pbp_market_size = p.source == "FRANCHISE" ? (double)fdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().market_size : (double)cdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().market_size,
                pbp_grouth_c = p.source == "FRANCHISE" ? fdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().grouth_c : cdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().grouth_c,
                pbp_grouth_jj = p.source == "FRANCHISE" ? fdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj : cdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().grouth_jj,
                pbp_share_p = p.source == "FRANCHISE" ? fdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().market_share_p: cdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().market_share_p,
                pbp_share_l = p.source == "FRANCHISE" ? fdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().market_share_l : cdata_PBP.Where(m => m.vid == p.id).FirstOrDefault().market_share_l,
            }).ToList();

        }

        private List<Models.ChartByChannelModels> GetCalcChartData(List<Models.MarketViewChannelModels> lst1, List<Models.MarketViewChannelModels> lst2)
        {
            return lst1.Join(lst2, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.ChartByChannelModels
            {
                vid = m.vid,
                market_size = m.col2 / 1000000,
                market_share_l = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                market_share_p = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                grouth_c = Helpers.StrawmanCalcs.CalcPCVSPY(m.col1, m.col2),
                grouth_jj = Helpers.StrawmanCalcs.CalcPCVSPY(b.col1, b.col2)
            }).ToList();
        }
        private List<Models.ChartByChannelModels> GetCalcChartData(List<Models.StrawmanViewSTDModel> lst1, List<Models.StrawmanViewSTDModel> lst2)
        {
            return GetCalcChartData(lst1, lst2, null);
        }
        private List<Models.ChartByChannelModels> GetCalcChartData(List<Models.StrawmanViewSTDModel> lst1, List<Models.StrawmanViewSTDModel> lst2, string type)
        {
            switch(type){
                case "STRAWMAN":
                    return lst1.Join(lst2, m => new { m.brand, m.market, m.channel }, b => new { b.brand,b.market,b.channel}, (m, b) => new Models.ChartByChannelModels
                    {
                        brand = m.brand,
                        market = m.market, 
                        market_size = m.col2 / 1000000,
                        market_share_l = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                        market_share_p = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        grouth_c = Helpers.StrawmanCalcs.CalcPCVSPY(m.col1, m.col2),
                        grouth_jj = Helpers.StrawmanCalcs.CalcPCVSPY(b.col1, b.col2)
                    }).ToList();
                default:
                    return lst1.Join(lst2, m => new { m.vid }, b => new { b.vid }, (m, b) => new Models.ChartByChannelModels
                    {
                        vid = (decimal)m.vid,
                        market_size = m.col2 / 1000000,
                        market_share_l = Helpers.StrawmanCalcs.CalcShare(m.col2, b.col2),
                        market_share_p = Helpers.StrawmanCalcs.CalcShare(m.col1, b.col1),
                        grouth_c = Helpers.StrawmanCalcs.CalcPCVSPY(m.col1, m.col2),
                        grouth_jj = Helpers.StrawmanCalcs.CalcPCVSPY(b.col1, b.col2)
                    }).ToList();
            }
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
            decimal mat_share = 0;
            decimal ytd_share = 0;
            decimal lm_share = 0;
            decimal mat_grouth_c = 0;
            decimal mat_grouth_jj = 0;
            decimal ytd_grouth_c = 0;
            decimal ytd_grouth_jj = 0;
            decimal lm_grouth_c = 0;
            decimal lm_grouth_jj = 0;
            decimal grouth = 0;
            decimal mat_grouth = 0;
            decimal ytd_grouth = 0;
            decimal lm_grouth = 0;
            decimal pbp_grouth_c = 0;
            decimal pbp_grouth_jj = 0;
            decimal pbp_share_l = 0;
            decimal pbp_share_p = 0;
            decimal pbp_grouth = 0;
            decimal pbp_share = 0;
    
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
                mat_share += share;

                ytd_market_share_l = ParseAbsolute(item.ytd_market_share_l);
                max_share = (max_share < ytd_market_share_l) ? ytd_market_share_l : max_share;
                share = ParseAbsolute(item.ytd_market_share_l);
                ytd_market_share_p = ParseAbsolute(item.ytd_market_share_p);
                max_share = (max_share < ytd_market_share_p) ? ytd_market_share_p : max_share;
                share += ParseAbsolute(item.ytd_market_share_p);
                ytd_share += share;
                
                lm_market_share_l = ParseAbsolute(item.lm_market_share_l);
                max_share = (max_share < lm_market_share_l) ? lm_market_share_l : max_share;
                share = ParseAbsolute(item.lm_market_share_l);
                lm_market_share_p = ParseAbsolute(item.lm_market_share_p);
                max_share = (max_share < lm_market_share_p) ? lm_market_share_p : max_share;
                share += ParseAbsolute(item.lm_market_share_p);
                lm_share += share;

                pbp_share_l = ParseAbsolute(item.pbp_share_l);
                max_share = (max_share < pbp_share_l) ? pbp_share_l : max_share;
                share = ParseAbsolute(item.pbp_share_l);
                pbp_share_p = ParseAbsolute(item.pbp_share_p);
                max_share = (max_share < pbp_share_p) ? pbp_share_p : max_share;
                share += ParseAbsolute(item.pbp_share_p);
                pbp_share += share;

                mat_grouth_c = ParseAbsolute(item.mat_grouth_c);
                max_grouth = (max_grouth < mat_grouth_c) ? mat_grouth_c : max_grouth;
                grouth = ParseAbsolute(item.mat_grouth_c);
                mat_grouth_jj = ParseAbsolute(item.mat_grouth_jj);
                max_grouth = (max_grouth < mat_grouth_jj) ? mat_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.mat_grouth_jj);
                mat_grouth += grouth;

                ytd_grouth_c = ParseAbsolute(item.ytd_grouth_c);
                max_grouth = (max_grouth < ytd_grouth_c) ? ytd_grouth_c : max_grouth;
                grouth = ParseAbsolute(item.ytd_grouth_c);
                ytd_grouth_jj = ParseAbsolute(item.ytd_grouth_jj);
                max_grouth = (max_grouth < ytd_grouth_jj) ? ytd_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.ytd_grouth_jj);
                ytd_grouth += grouth;

                lm_grouth_c = ParseAbsolute(item.lm_grouth_c);
                max_grouth = (max_grouth < lm_grouth_c) ? lm_grouth_c : max_grouth;
                grouth = ParseAbsolute(item.lm_grouth_c);
                lm_grouth_jj = ParseAbsolute(item.lm_grouth_jj);
                max_grouth = (max_grouth < lm_grouth_jj) ? lm_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.lm_grouth_jj);
                lm_grouth += grouth;

                pbp_grouth_c = ParseAbsolute(item.pbp_grouth_c);
                max_grouth = (max_grouth < pbp_grouth_c) ? pbp_grouth_c : max_grouth;
                grouth = ParseAbsolute(item.pbp_grouth_c);
                pbp_grouth_jj = ParseAbsolute(item.pbp_grouth_jj);
                max_grouth = (max_grouth < pbp_grouth_jj) ? pbp_grouth_jj : max_grouth;
                grouth += ParseAbsolute(item.pbp_grouth_jj);
                pbp_grouth += grouth;

                counter++;
                
            }
            
            //ajustamos el máximo del ancho de la barra a la media del resultado.
            //recalculamos para el caso especial de PBP
            if (pbp_grouth_c > 0 || pbp_grouth_jj > 0 || pbp_share_l > 0 || pbp_share_p > 0)
            {
                mat_share = mat_share / ((list.Count) / 2);
                ytd_share = ytd_share / ((list.Count) / 2);
                lm_share = lm_share / ((list.Count) / 2);

                share = share / ((list.Count) / 2);

                mat_grouth = mat_grouth / ((list.Count) / 2);
                ytd_grouth = ytd_grouth / ((list.Count) / 2);
                lm_grouth = lm_grouth / ((list.Count) / 2);

                grouth = grouth / ((list.Count) / 2);
            }
            else
            {
                pbp_share = pbp_share / ((list.Count) / 2);
                share = share / ((list.Count * 3) / 2);
                
                pbp_grouth = pbp_grouth / ((list.Count) / 2);
                grouth = grouth / ((list.Count * 3) / 2);
            }
            //ajustamos el máximo al tope del 90% en el caso de que exista un valor mayor que la media (daría porcentajes superiores al 90%, máximo permitido para visualizar correctamente)
            share = (share < max_share) ? max_share : share;
            grouth = (grouth < max_grouth) ? max_grouth : grouth;
            foreach (Models.ChartByChannelModels item in list)
            {                
                item.mat_market_size_pc = CalcPc((decimal)total_market, (decimal)item.mat_market_size);
                item.pbp_market_size_pc = CalcPc((decimal)total_market, (decimal)item.pbp_market_size);
                item.mat_market_share_l_pc = (item.mat_market_share_l == null)? 0 :CalcPc(mat_share, (decimal)(item.mat_market_share_l));
                item.mat_market_share_p_pc = (item.mat_market_share_p == null) ? 0 : CalcPc(mat_share, (decimal)(item.mat_market_share_p));
                item.ytd_market_share_l_pc = (item.ytd_market_share_l == null) ? 0 : CalcPc(ytd_share, (decimal)(item.ytd_market_share_l));
                item.ytd_market_share_p_pc = (item.ytd_market_share_p == null) ? 0 : CalcPc(ytd_share, (decimal)(item.ytd_market_share_p));
                item.lm_market_share_l_pc = (item.lm_market_share_l == null) ? 0 : CalcPc(lm_share, (decimal)(item.lm_market_share_l));
                item.lm_market_share_p_pc = (item.lm_market_share_p == null) ? 0 : CalcPc(lm_share, (decimal)(item.lm_market_share_p));
                item.pbp_share_l_pc = (item.pbp_share_l == null) ? 0 : CalcPc(pbp_share, (decimal)(item.pbp_share_l));
                item.pbp_share_p_pc = (item.pbp_share_p == null) ? 0 : CalcPc(pbp_share, (decimal)(item.pbp_share_p));
                item.mat_grouth_c_pc = (item.mat_grouth_c == null) ? 0 : CalcPc(mat_grouth, (decimal)(item.mat_grouth_c));
                item.mat_grouth_jj_pc = (item.mat_grouth_jj == null) ? 0 : CalcPc(mat_grouth, (decimal)(item.mat_grouth_jj));
                item.ytd_grouth_c_pc = (item.ytd_grouth_c == null) ? 0 : CalcPc(ytd_grouth, (decimal)(item.ytd_grouth_c));
                item.ytd_grouth_jj_pc = (item.ytd_grouth_jj == null) ? 0 : CalcPc(ytd_grouth, (decimal)(item.ytd_grouth_jj));
                item.lm_grouth_c_pc = (item.lm_grouth_c == null) ? 0 : CalcPc(lm_grouth, (decimal)(item.lm_grouth_c));
                item.lm_grouth_jj_pc = (item.lm_grouth_jj == null) ? 0 : CalcPc(lm_grouth, (decimal)(item.lm_grouth_jj));
                item.pbp_grouth_jj_pc = (item.pbp_grouth_jj == null) ? 0 : CalcPc(pbp_grouth, (decimal)(item.pbp_grouth_jj));
                item.pbp_grouth_c_pc = (item.pbp_grouth_c == null) ? 0 : CalcPc(pbp_grouth, (decimal)(item.pbp_grouth_c));
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
