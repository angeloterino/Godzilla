using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrawmanApp.Controllers
{
    [Authorize]
    public class Market_YTDController : Controller
    {
        //
        // GET: /Market_YTD/

        public ActionResult Index()
        {
            StrawmanApp.Models.DataClasses1DataContext db = new Models.DataClasses1DataContext();
            var query = from p in db.v_WRK_MARKET_YTD
                        where (p.YEAR_PERIOD == Helpers.PeriodUtil.Year && p.MONTH_PERIOD == Helpers.PeriodUtil.Month)
                        select new Models.Market_YTDModels { market = p.MARKET, brand = p.BRAND, ytd_2013 = p.YTD_COL1, ytd_2014 = p.YTD_COL2, pc_vs_py = p.PCVSPY };
            ViewBag.Market_YTD = query.ToList().AsEnumerable();
            return View(ViewBag.Market_YTD);
        }

        //
        // GET: /Market_YTD/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Market_YTD/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Market_YTD/Create

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
        // GET: /Market_YTD/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Market_YTD/Edit/5

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
        // GET: /Market_YTD/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Market_YTD/Delete/5

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
    }
}
