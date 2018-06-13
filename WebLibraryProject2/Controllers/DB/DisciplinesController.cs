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
    public class DisciplinesController : Controller
    {
        // GET: Disciplines
        public ActionResult Index()
        {
            using (var db = new LibraryDatabase())
            {
                return View(db.Disciplines.ToList());
            }
        }

        // GET: Disciplines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discipline discipline;
            using (var db = new LibraryDatabase())
            {
                discipline = db.Disciplines.Find(id);
                if (discipline == null)
                {
                    return HttpNotFound();
                }
            }
            return View(discipline);
        }

        // GET: Disciplines/Create
        public ActionResult Create()
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            return View();
        }

        // POST: Disciplines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Discipline discipline)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                if (ModelState.IsValid)
                {
                    db.Disciplines.Add(discipline);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(discipline);
        }

        // GET: Disciplines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discipline discipline;
            using (var db = new LibraryDatabase())
            {
                discipline = db.Disciplines.Find(id);
                if (discipline == null)
                {
                    return HttpNotFound();
                }
            }
            return View(discipline);
        }

        // POST: Disciplines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Discipline discipline)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(discipline).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(discipline);
        }

        // GET: Disciplines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discipline discipline;
            using (var db = new LibraryDatabase())
            {
                discipline = db.Disciplines.Find(id);
                if (discipline == null)
                {
                    return HttpNotFound();
                }
            }
            return View(discipline);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            Discipline discipline;
            using (var db = new LibraryDatabase())
            {
                discipline = db.Disciplines.Find(id);
                db.Disciplines.Remove(discipline);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
