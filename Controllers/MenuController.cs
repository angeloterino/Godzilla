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
    }
}
