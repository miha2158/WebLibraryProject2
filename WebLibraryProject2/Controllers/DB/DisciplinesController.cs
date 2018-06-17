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
        public LibraryDatabase db;

        // GET: Disciplines
        public ActionResult Index()
        {
            {
                return View(db.Disciplines.ToList());
            }
        }

        // GET: Disciplines/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discipline discipline;
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
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            return View();
        }

        // POST: Disciplines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Discipline discipline)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

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

        // GET: Disciplines/Edit
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discipline discipline;
            {
                discipline = db.Disciplines.Find(id);
                if (discipline == null)
                {
                    return HttpNotFound();
                }
            }
            return View(discipline);
        }

        // POST: Disciplines/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Discipline discipline)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

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

        // GET: Disciplines/Delete
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discipline discipline;
            {
                discipline = db.Disciplines.Find(id);
                if (discipline == null)
                {
                    return HttpNotFound();
                }
            }
            return View(discipline);
        }

        // POST: Disciplines/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            Discipline discipline;
            {
                discipline = db.Disciplines.Find(id);
                db.Disciplines.Remove(discipline);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db?.Dispose();
            base.Dispose(disposing);
        }
    }
}
