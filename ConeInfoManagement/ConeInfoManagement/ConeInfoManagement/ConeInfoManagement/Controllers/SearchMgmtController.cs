using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConeInfoManagement.Models
{
    public class SearchMgmtController : Controller
    {
        private CONEINFOContext db = new CONEINFOContext();

        //
        // GET: /SearchMgmt/

        public ActionResult Index()
        {
            var searches = db.Searches.Include(s => s.Keyword);
            return View(searches.ToList());
        }

        //
        // GET: /SearchMgmt/Details/5

        public ActionResult Details(int id = 0)
        {
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            return View(search);
        }

        //
        // GET: /SearchMgmt/Create

        public ActionResult Create()
        {
            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name");
            return View();
        }

        //
        // POST: /SearchMgmt/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Search search)
        {
            if (ModelState.IsValid)
            {
                db.Searches.Add(search);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name", search.keywordType);
            return View(search);
        }

        //
        // GET: /SearchMgmt/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name", search.keywordType);
            return View(search);
        }

        //
        // POST: /SearchMgmt/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Search search)
        {
            if (ModelState.IsValid)
            {
                db.Entry(search).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.keywordType = new SelectList(db.Keywords, "type", "name", search.keywordType);
            return View(search);
        }

        //
        // GET: /SearchMgmt/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            return View(search);
        }

        //
        // POST: /SearchMgmt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Search search = db.Searches.Find(id);
            db.Searches.Remove(search);
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