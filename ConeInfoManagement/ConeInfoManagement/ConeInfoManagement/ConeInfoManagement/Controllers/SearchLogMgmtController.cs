using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConeInfoManagement.Models
{
    public class SearchLogMgmtController : Controller
    {
        private CONEINFOContext db = new CONEINFOContext();

        //
        // GET: /SearchLogMgmt/

        public ActionResult Index()
        {
            var searchlogs = db.SearchLogs.Include(s => s.Keyword).Include(s => s.Search);
            return View(searchlogs.ToList());
        }

        //
        // GET: /SearchLogMgmt/Details/5

        public ActionResult Details(int id = 0)
        {
            SearchLog searchlog = db.SearchLogs.Find(id);
            if (searchlog == null)
            {
                return HttpNotFound();
            }
            return View(searchlog);
        }

        //
        // GET: /SearchLogMgmt/Create

        public ActionResult Create()
        {
            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name");
            ViewBag.searchId = new SelectList(db.Searches, "id", "question");
            return View();
        }

        //
        // POST: /SearchLogMgmt/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SearchLog searchlog)
        {
            if (ModelState.IsValid)
            {
                db.SearchLogs.Add(searchlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name", searchlog.keywordType);
            ViewBag.searchId = new SelectList(db.Searches, "id", "question", searchlog.searchId);
            return View(searchlog);
        }

        //
        // GET: /SearchLogMgmt/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SearchLog searchlog = db.SearchLogs.Find(id);
            if (searchlog == null)
            {
                return HttpNotFound();
            }
            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name", searchlog.keywordType);
            ViewBag.searchId = new SelectList(db.Searches, "id", "question", searchlog.searchId);
            return View(searchlog);
        }

        //
        // POST: /SearchLogMgmt/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SearchLog searchlog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searchlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name", searchlog.keywordType);
            ViewBag.searchId = new SelectList(db.Searches, "id", "question", searchlog.searchId);
            return View(searchlog);
        }

        //
        // GET: /SearchLogMgmt/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SearchLog searchlog = db.SearchLogs.Find(id);
            if (searchlog == null)
            {
                return HttpNotFound();
            }
            return View(searchlog);
        }

        //
        // POST: /SearchLogMgmt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearchLog searchlog = db.SearchLogs.Find(id);
            db.SearchLogs.Remove(searchlog);
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