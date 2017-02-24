using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class TabsController : Controller
    {
        //
        // GET: /Tabs/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tabs(string title, string menu_url, string tab_url)
        {
            List<Models.TabItem> tabs = null;
            int menu_id = 0;
            string menu_id_string = (Helpers.Session.GetSession("menu_id") != null)?Helpers.Session.GetSession("menu_id").ToString():"";

            if (!int.TryParse(menu_id_string, out menu_id))
            {
                menu_id = 0;
            }

            using (Models.MenuDataClassesDataContext db = new Models.MenuDataClassesDataContext())
            {
                var query = from p in db.v_TAB_MASTERs
                            where (menu_id == 0 && p.MENU_URL == (menu_url==null? title:menu_url)) ||(menu_id != 0 && p.MENU_ID == menu_id)
                            select new Models.TabItem { name = p.NAME, url = p.URL, menu_url = p.MENU_URL, select_default = p.SELECT_DEFAULT , controller = p.CONTROLLER, title = title, tab_url = tab_url };
                tabs = query.ToList();
            }
            return PartialView("~/Views/Tabs/Tabs.cshtml", tabs);
        }

    }
}
