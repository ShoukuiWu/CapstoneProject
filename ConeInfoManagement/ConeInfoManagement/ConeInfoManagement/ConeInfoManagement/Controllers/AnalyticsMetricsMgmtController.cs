using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConeInfoManagement.Models
{
    public class AnalyticsMetricsMgmtController : Controller
    {
        private CONEINFOContext db = new CONEINFOContext();

        //
        // GET: /AnalyticsMetricsMgmt/

        public ActionResult Index()
        {
            return View(db.AnalyticsMetrics.OrderByDescending(a => a.date).ToList());
        }

        //
        // GET: /AnalyticsMetricsMgmt/Details/5

        public ActionResult Details(int id = 0)
        {
            AnalyticsMetric analyticsmetric = db.AnalyticsMetrics.Find(id);
            if (analyticsmetric == null)
            {
                return HttpNotFound();
            }
            string keyword = db.Keywords.Where(r => r.type == analyticsmetric.type).Select(r => r.name).Single().ToString();
            ViewBag.keyword = keyword;

            return View(analyticsmetric);
        }

        public ActionResult Result(int id = 0)
        {
            AnalyticsMetric analyticsmetric = db.AnalyticsMetrics.Find(id);
            if (analyticsmetric == null)
            {
                return HttpNotFound();
            }
            string keyword = db.Keywords.Where(r => r.type == analyticsmetric.type).Select(r => r.name).Single().ToString();
            ViewBag.keyword = keyword;

            return View(db.SearchLogs.Where(a => a.keywordType == analyticsmetric.type).OrderByDescending(a => a.searchDate).ToList());
        }


        //
        // GET: /AnalyticsMetricsMgmt/Create

        public ActionResult Create()
        {
            ViewBag.type = new SelectList(db.Keywords, "type", "name");
            return View();
        }

        //
        // POST: /AnalyticsMetricsMgmt/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnalyticsMetric analyticsmetric)
        {
            if (ModelState.IsValid)
            {
                db.AnalyticsMetrics.Add(analyticsmetric);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(analyticsmetric);
        }

        //
        // GET: /AnalyticsMetricsMgmt/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AnalyticsMetric analyticsmetric = db.AnalyticsMetrics.Find(id);
            if (analyticsmetric == null)
            {
                return HttpNotFound();
            }
            string keyword = db.Keywords.Where(r => r.type == analyticsmetric.type).Select(r => r.name).Single().ToString();
            ViewBag.keyword = keyword;

            ViewBag.type = new SelectList(db.Keywords, "type", "name", keyword);

            return View(analyticsmetric);
        }

        //
        // POST: /AnalyticsMetricsMgmt/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AnalyticsMetric analyticsmetric)
        {
            if (ModelState.IsValid)
            {
                db.Entry(analyticsmetric).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(analyticsmetric);
        }

        //
        // GET: /AnalyticsMetricsMgmt/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AnalyticsMetric analyticsmetric = db.AnalyticsMetrics.Find(id);
            if (analyticsmetric == null)
            {
                return HttpNotFound();
            }
            string keyword = db.Keywords.Where(r => r.type == analyticsmetric.type).Select(r => r.name).Single().ToString();
            ViewBag.keyword = keyword;

            return View(analyticsmetric);
        }

        //
        // POST: /AnalyticsMetricsMgmt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnalyticsMetric analyticsmetric = db.AnalyticsMetrics.Find(id);
            db.AnalyticsMetrics.Remove(analyticsmetric);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}