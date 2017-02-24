using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu() {
            Models.MenuDataClassesDataContext db = new Models.MenuDataClassesDataContext();
            var query = from p in db.v_MENU_MASTER where p.USER == Helpers.UserUtils.UserName || p.USER == null
                        select new Models.MenuItem { name = p.NAME, url = p.URL, parent = (int)p.PARENT, id=(int)p.ID, premission = p.PERMISSION, controller = p.CONTROLLER, disabled = p.DISABLED, divided_before = p.DIVIDER_BEFORE };
            return PartialView("~/Views/Menu/Menu.cshtml", query.ToList());
        }

        public ActionResult Banner(string title, string menu_url, string tab_url)
        {
            string partial_view = "~/Views/Shared/UtilBanner/_UtilBanner.cshtml";
            string _y = "Y";
            int menu_id = 0;
            string menu_id_string = (Helpers.Session.GetSession("menu_id") != null)?Helpers.Session.GetSession("menu_id").ToString():"";
            bool currency_adjust = false, currency = false, pc_adjust = false, wc = false, period = false;

            if (!int.TryParse(menu_id_string, out menu_id))
            {
                menu_id = 0;
            }
            using(Models.MenuDataClassesDataContext db = new Models.MenuDataClassesDataContext())
            {
                Models.v_TAB_MASTER view = db.v_TAB_MASTERs.Where(m=>(menu_id == 0 && m.MENU_URL == (menu_url==null? title:menu_url)) || (menu_id != 0 && m.MENU_ID == menu_id)).FirstOrDefault();
                if (view != null)
                {
                    currency_adjust = view.CURRENCY_ADJUST == _y;
                    currency = view.CURRENCY == _y;
                    pc_adjust = view.PC_ADJUST == _y;
                    wc = view.WC == _y;
                    period = view.PERIOD == _y;
                }

            }
            ViewBag.WC = wc;
            ViewBag.CurrencyAdjust = currency_adjust;
            ViewBag.Currency = currency;
            ViewBag.Period = period;
            ViewBag.PCAdjust = pc_adjust;
            return PartialView(partial_view);
        }
        public void SetBannerCookie(string key, string value)
        {
            Response.Cookies.Set(new HttpCookie(key, value));
        }
    }
}
