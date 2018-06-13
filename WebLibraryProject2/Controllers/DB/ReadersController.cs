using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebLibraryProject2.Models;

namespace WebLibraryProject2.Controllers
{
    public class ReadersController : Controller
    {
        // GET: Readers
        public ActionResult Index()
        {
            using (var db = new LibraryDatabase())
                return View(db.Readers.ToList());
        }

        // GET: Readers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader reader;
            using (var db = new LibraryDatabase())
                reader = db.Readers.Find(id);

            if (reader == null)
            {
                return HttpNotFound();
            }
            return View(reader);
        }

        // GET: Readers/Create
        public ActionResult Create()
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            return View();
        }

        // POST: Readers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,First,Last,Patronimic,AccessLevel,Group")] Reader reader)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();


            using (var db = new LibraryDatabase())
                if (ModelState.IsValid)
                {
                    db.Readers.Add(reader);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            return View(reader);
        }

        // GET: Readers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader reader;
            using (var db = new LibraryDatabase())
                reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }

            return View(reader);
        }

        // POST: Readers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,First,Last,Patronimic,AccessLevel,Group")] Reader reader)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();


            using (var db = new LibraryDatabase())
                if (ModelState.IsValid)
            {
                db.Entry(reader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reader);
        }

        // GET: Readers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader reader;
            using (var db = new LibraryDatabase())
                reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }
            return View(reader);
        }

        // POST: Readers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();


            using (var db = new LibraryDatabase())
            {
                Reader reader = db.Readers.Find(id);
                db.Readers.Remove(reader);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
