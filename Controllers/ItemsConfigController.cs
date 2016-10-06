using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    public class ItemsConfigController : Controller
    {
        //
        // GET: /ItemsConfig/

        public ActionResult Index()
        {
            return View();
        }
        //Editar artículo
        public ActionResult ShowItemCaracts(string _market, string _brand, string _channel)
        {
            Models.ItemsConfigModel model = GetMasterData(_market, _brand, _channel);
            return View(model);
        }
        //Obtiene los datos por artículo
        public ActionResult GetItemConfig(string _type, string _market, string _brand, string _channel)
        {
            Models.ItemsConfigModel model = GetMasterData(_market,_brand,_channel);
            List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA> data = (List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA>)GetData(Constants.V_TMP_MASTER_DATA);
            List<StrawmanDBLibray.Entities.ROSETTA_LOADER> nielsen_data = (List<StrawmanDBLibray.Entities.ROSETTA_LOADER>) GetData(Constants.ROSETTA_LOADER);
            List<StrawmanDBLibray.Entities.ROSETTA_STONE> nts_data = (List<StrawmanDBLibray.Entities.ROSETTA_STONE>) GetData(Constants.ROSETTA_STONE);
            model.nielsen_data = nielsen_data.Where(m=>m.CHANNEL_ID == int.Parse(_channel))
                                            .Select(m => new Models.Nielsen_Data
                                            {
                                                market = m.MARKET_ID,
                                                brand  = m.BRAND_ID,
                                                channel = m.CHANNEL_ID,
                                                market_description = m.DESCRIPTION,
                                                brand_description = m.DESCRIPTION,
                                                market_row = (int?)m.EXCEL_ROW,
                                                brand_row = m.BRAND_ID != null?(int?)m.EXCEL_ROW: null,
                                            }).ToList();
            model.nts_data = nts_data.Where(m=>m.CHANNEL == decimal.Parse (_channel))
                                            .Select(m=>new Models.NTS_Data{
                                                market = m.MARKET,
                                                brand = m.BRAND,
                                                channel = m.CHANNEL,
                                                eurocode= m.EUROCODE,
                                                id = m.ID
                                            }).ToList();
            model.groups_data = new Models.Groups_Data();
            model.type = _type;
            return PartialView(ITEM_EDIT, model);
        }
        //Cambio de canal
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ChangeChanel(string _channel)
        {
            int channel = _channel != null ? int.Parse(_channel) : Helpers.Channels.MASS;

            return GetItemsConfig(channel);
        }
        //Devuelve la lista de artículos en la aplicación por canal
        public ActionResult GetItemsConfig(int _channel)
        {
            List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA> data = (List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA>)GetData(Constants.V_TMP_MASTER_DATA);

            return PartialView(ITEMS_LIST, data.Where(m=>m.CHANNEL == _channel).Select(m=>m).ToList());
        }

        public ActionResult GetItemsConfigData(string _market, string _brand, string _channel)
        {
            List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA> mst_data = (List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA>)GetData(Constants.V_TMP_MASTER_DATA);
            List<StrawmanDBLibray.Entities.v_TMP_STRWM_DATA> strwm_data = (List<StrawmanDBLibray.Entities.v_TMP_STRWM_DATA>)GetData(Constants.V_TMP_STRWM_DATA);
            List<StrawmanDBLibray.Entities.v_TMP_NTS_DATA> nts_data = (List<StrawmanDBLibray.Entities.v_TMP_NTS_DATA>)GetData(Constants.V_TMP_NTS_DATA);
            var ret_mst = mst_data.Where(m => m.MARKET == (int.Parse(_market)) && m.BRAND == (int.Parse(_brand)) && m.CHANNEL == (int.Parse(_channel))).Select(m => m).FirstOrDefault();
            var ret_strw = strwm_data.Where(m => m.MARKET == (int.Parse(_market)) && m.BRAND == (int.Parse(_brand))).Select(m => new
            {
                m.MARKET,
                m.BRAND,
                CHANNEL = mst_data.Find(n=>n.MARKET == m.MARKET && n.BRAND == m.BRAND).CHANNEL,
                m.MARKET_DESCRIPTION,
                m.BRAND_DESCRIPTION,
                m.MARKET_MONTH,
                m.MARKET_YTD,
                m.MARKET_MAT,
                m.MARKET_TOTAL,
                m.BRAND_MONTH,
                m.BRAND_YTD,
                m.BRAND_MAT,
                m.BRAND_TOTAL
            }).FirstOrDefault();
            var ret_nts = nts_data.Where(m => m.MARKET == (int.Parse(_market)) && m.BRAND == (int.Parse(_brand))).Select(m => new
            {
                m.MARKET,
                m.BRAND,
                m.CHANNEL,
                m.NAME,
                m.AMOUNTH
            }).FirstOrDefault();

            StrawmanApp.Models.ItemsConfigModel ret = new Models.ItemsConfigModel
            {
                market = _market,
                brand = _brand,
                channel = _channel,
                market_description = (ret_mst == null)?null:ret_mst.MARKET_NAME,
                brand_description = (ret_mst == null) ? null : ret_mst.BRAND_NAME,
                market_month = (ret_strw == null)?null:ret_strw.MARKET_MONTH,
                market_ytd = (ret_strw == null)?null:ret_strw.MARKET_YTD,
                market_mat = (ret_strw == null)?null:ret_strw.MARKET_MAT,
                market_total = (ret_strw == null)?null:ret_strw.MARKET_TOTAL,
                brand_month = (ret_strw == null)?null:ret_strw.BRAND_MONTH,
                brand_ytd = (ret_strw == null)?null:ret_strw.BRAND_YTD,
                brand_mat = (ret_strw == null)?null:ret_strw.BRAND_MAT,
                brand_total = (ret_strw == null)?null:ret_strw.BRAND_TOTAL,
                nts_name = (ret_nts==null)?null:ret_nts.NAME,
                nts_amount = (ret_nts==null)?null:ret_nts.AMOUNTH
            };
            return PartialView(ITEM_DATA,ret);
        }
        #region Functions
        public object GetData(string _view)
        {
            if (Helpers.Session.GetSession(_view) == null){
                using (StrawmanDBLibray.Entities.godzillaDBLibraryEntity db = new StrawmanDBLibray.Entities.godzillaDBLibraryEntity())
                {
                    switch(_view)
                    {
                        case Constants.V_TMP_STRWM_DATA:
                            var s = db.v_TMP_STRWM_DATA.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).Select(m => m);
                            Helpers.Session.SetSession(_view, s.ToList());
                            break;
                        case Constants.V_TMP_NTS_DATA:
                            var n = db.v_TMP_NTS_DATA.Where(m => m.YEAR_PERIOD == Helpers.PeriodUtil.Year && m.MONTH_PERIOD == Helpers.PeriodUtil.Month).Select(m => m);
                            Helpers.Session.SetSession(_view, n.ToList());
                            break;
                        case Constants.V_TMP_MASTER_DATA:
                            var q = db.v_TMP_MASTER_DATA.Select(m => m);
                            Helpers.Session.SetSession(_view, q.ToList());
                            break;
                        case Constants.ROSETTA_LOADER:
                            var rl = db.ROSETTA_LOADER;
                            Helpers.Session.SetSession(_view, rl.ToList());
                            break;
                        case Constants.ROSETTA_STONE:
                            var rs = db.ROSETTA_STONE;
                            Helpers.Session.SetSession(_view, rs.ToList());
                            break;
                    }
                }
            }
            
            return Helpers.Session.GetSession(_view);
        }
        private Models.ItemsConfigModel GetMasterData(string _market, string _brand, string _channel)
        {
            List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA> data = (List<StrawmanDBLibray.Entities.v_TMP_MASTER_DATA>)GetData(Constants.V_TMP_MASTER_DATA);
            return data.Where(m => m.MARKET == int.Parse(_market) && m.BRAND == int.Parse(_brand) && m.CHANNEL == int.Parse(_channel))
                                .Select(m => new Models.ItemsConfigModel { market = _market, brand = _brand, channel = _channel, market_description = m.MARKET_NAME, brand_description = m.BRAND_NAME })
                                .FirstOrDefault();
        }
        #endregion

        #region Constants
        private const string PATH = "~/Views/ItemsConfig/";
        private const string ITEMS_LIST = PATH + "_ItemsList.cshtml";
        private const string ITEM_DATA = PATH + "_ItemDATA.cshtml";
        private const string ITEM_EDIT = PATH + "_ItemEdit.cshtml";

        private partial class Constants
        {
            public const string V_TMP_MASTER_DATA = "v_TMP_MASTER_DATA";
            public const string V_TMP_STRWM_DATA = "v_TMP_STRWM_DATA";
            public const string V_TMP_NTS_DATA = "v_TMP_NTS_DATA";
            public const string ROSETTA_LOADER = "ROSETTA_LOADER";
            public const string ROSETTA_STONE = "ROSETTA_STONE";
        }
        #endregion

    }
}
