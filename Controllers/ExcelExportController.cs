using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelW = Microsoft.Office.Interop.Excel;
using System.IO;
using Microsoft.Office.Core;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class ExcelExportController : Controller
    {
        [HttpPost]
        public virtual ActionResult ProcessFile(string name)
        {            
            string fileName = name;
            string dfileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString()+ DateTime.Now.Millisecond.ToString() + fileName;
            bool success = false;
            string message = "Error processing file...";
            string spath = Server.MapPath(TEMPLATE_PATH);
            string download_path = Server.MapPath(DOWNLOAD_PATH);
            string path = Path.Combine(spath, fileName);
            string dpath = Path.Combine(download_path,dfileName);
            string ext = Path.GetExtension(path);
            FileStream fis = System.IO.File.OpenWrite(dpath);
            ExcelW.Workbook myBook = null;
            ExcelW.Application myApp = null;
            ExcelW.Worksheet mySheet = null;

            myApp = new ExcelW.Application();
            myApp.Visible = false;

            myBook = myApp.Workbooks.Open(path);

            mySheet = SetWorkSheet(myBook.Worksheets[1], ChartType.CHART_BOY_CHANNEL);            

            myBook.SaveCopyAs(dpath);

            return RedirectToAction("LoaderPreview");
        }

        private ExcelW.Worksheet SetWorkSheet(dynamic dynamic, string p)
        {
            ExcelW.Worksheet mySheet = dynamic;

            List<Entities.v_CHART_BOY_JJ_BY_CHANNEL> chart = new List<Entities.v_CHART_BOY_JJ_BY_CHANNEL>();

            //Obtenemos los datos del chart a exportar
            Models.ChartModel model = Controllers.ChartBOYChannelController.GetChartDataExport("");
            //Desglosamos los datos y los asignamos a las celdas correspondientes
            List<Models.ChartByChannelModels> list_m = model.chart_mat.Where(m => m.market > 10000 && m.brand > 10000).ToList();
            //
            //2015 MAT Market Size ($ MM)
            ExcelW.Range msize_range = mySheet.get_Range("C4","C12");
            foreach (Models.ChartByChannelModels item in list_m)
            {
                int _row = msize_range.Find(item.channel_name).Row;
                mySheet.Cells[_row][3] = item.mat_market_size; //Total
            }
            //mySheet.Cells[4][3] = "";
            //mySheet.Cells[9][3] = "";
            //mySheet.Cells[11][3] = "";
            //2015 MAT Market Size ($MM)
            List<Models.ChartByChannelModels> list_mt = model.chart_mat.Where(m => (m.market < 10000 && m.brand < 10000 && m.market > 1000 && m.brand > 1000) || m.brand > 90000).ToList();
            msize_range = mySheet.get_Range("F4", "F12");
            foreach (Models.ChartByChannelModels item in list_mt)
            {
                int _row = msize_range.Find(item.channel_name).Row;
                mySheet.Cells[_row][6] = item.mat_market_size;
            }
            //mySheet.Cells[4][6] = "";
            //mySheet.Cells[5][6] = "";
            //mySheet.Cells[6][6] = "";
            //mySheet.Cells[7][6] = "";
            //mySheet.Cells[8][6] = "";
            //mySheet.Cells[9][6] = "";
            //mySheet.Cells[11][6] = "";
            //2015 MAT Market Share (%)
            List<Models.ChartByChannelModels> list_ms = model.chart_mat.Where(m => (m.market < 1000 && m.brand < 1000) || m.brand > 90000).ToList();
            msize_range = mySheet.get_Range("I4", "I12");
            foreach (Models.ChartByChannelModels item in list_mt)
            {
                int _row = msize_range.Find(item.channel_name).Row;
                mySheet.Cells[_row][9] = item.mat_market_share_p;
                mySheet.Cells[_row][10] = item.mat_market_share_l;

                mySheet.Cells[_row][13] = item.mat_grouth_c;
                mySheet.Cells[_row][14] = item.mat_grouth_jj;
            }
            //mySheet.Cells[3][9] = "";
            //mySheet.Cells[4][9] = "";
            //mySheet.Cells[5][9] = "";
            //mySheet.Cells[6][9] = "";
            //mySheet.Cells[7][9] = "";
            //mySheet.Cells[8][9] = "";
            //mySheet.Cells[9][9] = "";
            //mySheet.Cells[10][9] = "";
            //mySheet.Cells[11][9] = "";

            //mySheet.Cells[3][10] = "";
            //mySheet.Cells[4][10] = "";
            //mySheet.Cells[5][10] = "";
            //mySheet.Cells[6][10] = "";
            //mySheet.Cells[7][10] = "";
            //mySheet.Cells[8][10] = "";
            //mySheet.Cells[9][10] = "";
            //mySheet.Cells[10][10] = "";
            //mySheet.Cells[11][10] = "";
            //2015 MAT Growth (%)
            //mySheet.Cells[3][13] = "";
            //mySheet.Cells[4][13] = "";
            //mySheet.Cells[5][13] = "";
            //mySheet.Cells[6][13] = "";
            //mySheet.Cells[7][13] = "";
            //mySheet.Cells[8][13] = "";
            //mySheet.Cells[9][13] = "";
            //mySheet.Cells[10][13] = "";
            //mySheet.Cells[11][13] = "";

            //mySheet.Cells[3][14] = "";
            //mySheet.Cells[4][14] = "";
            //mySheet.Cells[5][14] = "";
            //mySheet.Cells[6][14] = "";
            //mySheet.Cells[7][14] = "";
            //mySheet.Cells[8][14] = "";
            //mySheet.Cells[9][14] = "";
            //mySheet.Cells[10][14] = "";
            //mySheet.Cells[11][14] = "";
            //2015 YTD Market Share (%)
            List<Models.ChartByChannelModels> list_y = model.chart_ytd.ToList();
            msize_range = mySheet.get_Range("I16", "I24");
            foreach (Models.ChartByChannelModels item in list_y)
            {
                int _row = msize_range.Find(item.channel_name).Row;
                mySheet.Cells[_row][9] = item.ytd_market_share_p;
                mySheet.Cells[_row][10] = item.ytd_market_share_l;

                mySheet.Cells[_row][13] = item.ytd_grouth_c;
                mySheet.Cells[_row][14] = item.ytd_grouth_jj;
            }
            //mySheet.Cells[15][9] = "";
            //mySheet.Cells[16][9] = "";
            //mySheet.Cells[17][9] = "";
            //mySheet.Cells[18][9] = "";
            //mySheet.Cells[19][9] = "";
            //mySheet.Cells[20][9] = "";
            //mySheet.Cells[21][9] = "";
            //mySheet.Cells[22][9] = "";
            //mySheet.Cells[23][9] = "";

            //mySheet.Cells[15][10] = "";
            //mySheet.Cells[16][10] = "";
            //mySheet.Cells[17][10] = "";
            //mySheet.Cells[18][10] = "";
            //mySheet.Cells[19][10] = "";
            //mySheet.Cells[20][10] = "";
            //mySheet.Cells[21][10] = "";
            //mySheet.Cells[22][10] = "";
            //mySheet.Cells[23][10] = "";
            //2015 YTD Growth (%)
            //mySheet.Cells[15][13] = "";
            //mySheet.Cells[16][13] = "";
            //mySheet.Cells[17][13] = "";
            //mySheet.Cells[18][13] = "";
            //mySheet.Cells[19][13] = "";
            //mySheet.Cells[20][13] = "";
            //mySheet.Cells[21][13] = "";
            //mySheet.Cells[22][13] = "";
            //mySheet.Cells[23][13] = "";

            //mySheet.Cells[15][14] = "";
            //mySheet.Cells[16][14] = "";
            //mySheet.Cells[17][14] = "";
            //mySheet.Cells[18][14] = "";
            //mySheet.Cells[19][14] = "";
            //mySheet.Cells[20][14] = "";
            //mySheet.Cells[21][14] = "";
            //mySheet.Cells[22][14] = "";
            //mySheet.Cells[23][14] = "";

            //2015 MTG Market Share (%)
            List<Models.ChartByChannelModels> list_l = model.chart_lm.ToList();
            msize_range = mySheet.get_Range("I40", "I48");
            foreach (Models.ChartByChannelModels item in list_l)
            {
                int _row = msize_range.Find(item.channel_name).Row;
                mySheet.Cells[_row][9] = item.lm_market_share_p;
                mySheet.Cells[_row][10] = item.lm_market_share_l;

                mySheet.Cells[_row][13] = item.lm_grouth_c;
                mySheet.Cells[_row][14] = item.lm_grouth_jj;
            }
            //mySheet.Cells[27][9] = "";
            //mySheet.Cells[28][9] = "";
            //mySheet.Cells[29][9] = "";
            //mySheet.Cells[30][9] = "";
            //mySheet.Cells[31][9] = "";
            //mySheet.Cells[32][9] = "";
            //mySheet.Cells[33][9] = "";
            //mySheet.Cells[34][9] = "";
            //mySheet.Cells[35][9] = "";

            //mySheet.Cells[27][10] = "";
            //mySheet.Cells[28][10] = "";
            //mySheet.Cells[29][10] = "";
            //mySheet.Cells[30][10] = "";
            //mySheet.Cells[31][10] = "";
            //mySheet.Cells[32][10] = "";
            //mySheet.Cells[33][10] = "";
            //mySheet.Cells[34][10] = "";
            //mySheet.Cells[35][10] = "";
            ////2015 BTG Growth (%)
            //mySheet.Cells[27][13] = "";
            //mySheet.Cells[28][13] = "";
            //mySheet.Cells[29][13] = "";
            //mySheet.Cells[30][13] = "";
            //mySheet.Cells[31][13] = "";
            //mySheet.Cells[32][13] = "";
            //mySheet.Cells[33][13] = "";
            //mySheet.Cells[34][13] = "";
            //mySheet.Cells[35][13] = "";

            //mySheet.Cells[27][14] = "";
            //mySheet.Cells[28][14] = "";
            //mySheet.Cells[29][14] = "";
            //mySheet.Cells[30][14] = "";
            //mySheet.Cells[31][14] = "";
            //mySheet.Cells[32][14] = "";
            //mySheet.Cells[33][14] = "";
            //mySheet.Cells[34][14] = "";
            //mySheet.Cells[35][14] = "";
            //2015 MTG Market Share (%)
            //mySheet.Cells[39][9] = "";
            //mySheet.Cells[40][9] = "";
            //mySheet.Cells[41][9] = "";
            //mySheet.Cells[42][9] = "";
            //mySheet.Cells[43][9] = "";
            //mySheet.Cells[44][9] = "";
            //mySheet.Cells[45][9] = "";
            //mySheet.Cells[46][9] = "";
            //mySheet.Cells[47][9] = "";

            //mySheet.Cells[39][10] = "";
            //mySheet.Cells[40][10] = "";
            //mySheet.Cells[41][10] = "";
            //mySheet.Cells[42][10] = "";
            //mySheet.Cells[43][10] = "";
            //mySheet.Cells[44][10] = "";
            //mySheet.Cells[45][10] = "";
            //mySheet.Cells[46][10] = "";
            //mySheet.Cells[47][10] = "";
            ////2015 MTG Growth (%)
            //mySheet.Cells[39][13] = "";
            //mySheet.Cells[40][13] = "";
            //mySheet.Cells[41][13] = "";
            //mySheet.Cells[42][13] = "";
            //mySheet.Cells[43][13] = "";
            //mySheet.Cells[44][13] = "";
            //mySheet.Cells[45][13] = "";
            //mySheet.Cells[46][13] = "";
            //mySheet.Cells[47][13] = "";

            //mySheet.Cells[39][14] = "";
            //mySheet.Cells[40][14] = "";
            //mySheet.Cells[41][14] = "";
            //mySheet.Cells[42][14] = "";
            //mySheet.Cells[43][14] = "";
            //mySheet.Cells[44][14] = "";
            //mySheet.Cells[45][14] = "";
            //mySheet.Cells[46][14] = "";
            //mySheet.Cells[47][14] = "";
            //2016 BP Market Size
            List<Models.ChartByChannelModels> list_p = model.chart_pbp.ToList();
            msize_range = mySheet.get_Range("S4", "S12");
            foreach (Models.ChartByChannelModels item in list_p)
            {
                int _row = msize_range.Find(item.channel_name).Row;

                mySheet.Cells[_row][19] = item.pbp_market_size;

                mySheet.Cells[_row][22] = item.pbp_share_p;
                mySheet.Cells[_row][23] = item.pbp_share_l;

                mySheet.Cells[_row][13] = item.pbp_grouth_c;
                mySheet.Cells[_row][14] = item.pbp_grouth_jj;
            }
            //mySheet.Cells[3][19] = "";
            //mySheet.Cells[4][19] = "";
            //mySheet.Cells[5][19] = "";
            //mySheet.Cells[6][19] = "";
            //mySheet.Cells[7][19] = "";
            //mySheet.Cells[8][19] = "";
            //mySheet.Cells[9][19] = "";
            //mySheet.Cells[11][19] = "";
            ////2016 BP Market Share (%)
            //mySheet.Cells[3][22] = "";
            //mySheet.Cells[4][22] = "";
            //mySheet.Cells[5][22] = "";
            //mySheet.Cells[6][22] = "";
            //mySheet.Cells[7][22] = "";
            //mySheet.Cells[8][22] = "";
            //mySheet.Cells[9][22] = "";
            //mySheet.Cells[10][22] = "";
            //mySheet.Cells[11][22] = "";

            //mySheet.Cells[3][23] = "";
            //mySheet.Cells[4][23] = "";
            //mySheet.Cells[5][23] = "";
            //mySheet.Cells[6][23] = "";
            //mySheet.Cells[7][23] = "";
            //mySheet.Cells[8][23] = "";
            //mySheet.Cells[9][23] = "";
            //mySheet.Cells[10][23] = "";
            //mySheet.Cells[11][23] = "";
            ////52 W Growth (%)
            //mySheet.Cells[3][26] = "";
            //mySheet.Cells[4][26] = "";
            //mySheet.Cells[5][26] = "";
            //mySheet.Cells[6][26] = "";
            //mySheet.Cells[7][26] = "";
            //mySheet.Cells[8][26] = "";
            //mySheet.Cells[9][26] = "";
            //mySheet.Cells[10][26] = "";
            //mySheet.Cells[11][26] = "";

            //mySheet.Cells[3][27] = "";
            //mySheet.Cells[4][27] = "";
            //mySheet.Cells[5][27] = "";
            //mySheet.Cells[6][27] = "";
            //mySheet.Cells[7][27] = "";
            //mySheet.Cells[8][27] = "";
            //mySheet.Cells[9][27] = "";
            //mySheet.Cells[10][27] = "";
            //mySheet.Cells[11][27] = "";
            //
            return mySheet;
        }

        private const string TEMPLATE_PATH = "~/Templates/Excel";
        private const string DOWNLOAD_PATH = "~/Downloads";

        private class ChartType
        {
            public const string CHART_BOY_CHANNEL = "CHART_BOY_CHANNEL";
        }
    }
}
