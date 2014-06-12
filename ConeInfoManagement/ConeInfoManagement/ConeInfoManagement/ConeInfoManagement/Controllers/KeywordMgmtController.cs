using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConeInfoManagement.Models;

namespace ConeInfoManagement.Controllers
{
    public class KeywordMgmtController : Controller
    {
        private CONEINFOContext db = new CONEINFOContext();

        //
        // GET: /KeywordMgmt/

        public ActionResult Index()
        {
            return View(db.Keywords.ToList());
        }

        //
        // GET: /KeywordMgmt/Details/5

        public ActionResult Details(int id = 0)
        {
            Keyword keyword = db.Keywords.Find(id);
            if (keyword == null)
            {
                return HttpNotFound();
            }
            return View(keyword);
        }

        //
        // GET: /KeywordMgmt/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /KeywordMgmt/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                db.Keywords.Add(keyword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(keyword);
        }

        //
        // GET: /KeywordMgmt/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Keyword keyword = db.Keywords.Find(id);
            if (keyword == null)
            {
                return HttpNotFound();
            }
            return View(keyword);
        }

        //
        // POST: /KeywordMgmt/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                db.Entry(keyword).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(keyword);
        }

        //
        // GET: /KeywordMgmt/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Keyword keyword = db.Keywords.Find(id);
            if (keyword == null)
            {
                return HttpNotFound();
            }
            return View(keyword);
        }

        //
        // POST: /KeywordMgmt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Keyword keyword = db.Keywords.Find(id);
            db.Keywords.Remove(keyword);
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