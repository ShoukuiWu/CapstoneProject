using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConeInfoManagement.Models;

namespace ConeInfoManagement.Models
{
    public class AccountMgmtController : Controller
    {
        private CONEINFOContext db = new CONEINFOContext();

        //
        // GET: /AccountMgmt/

        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Person);
            return View(accounts.ToList());
        }

        //
        // GET: /AccountMgmt/Details/5

        public ActionResult Details(int id = 0)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // GET: /AccountMgmt/Create

        public ActionResult Create()
        {
            ViewBag.personNumber = new SelectList(db.People, "number", "firstName");
            return View();
        }

        //
        // POST: /AccountMgmt/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.personNumber = new SelectList(db.People, "number", "firstName", account.personNumber);
            return View(account);
        }

        //
        // GET: /AccountMgmt/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.personNumber = new SelectList(db.People, "number", "firstName", account.personNumber);
            return View(account);
        }

        //
        // POST: /AccountMgmt/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.personNumber = new SelectList(db.People, "number", "firstName", account.personNumber);
            return View(account);
        }

        //
        // GET: /AccountMgmt/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        //
        // POST: /AccountMgmt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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