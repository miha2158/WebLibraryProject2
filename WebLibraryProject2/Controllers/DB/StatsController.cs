using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebLibraryProject2.Models;

namespace WebLibraryProject2.Controllers.DB
{
    public class StatsController : Controller
    {
        public LibraryDatabase db = new LibraryDatabase();

        // GET: Stats
        public ActionResult Index()
        {
            {
                var stats = db.Stats.Include(s => s.Publication);
                return View(stats.ToList());
            }
        }

        // GET: Stats/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stats stats;
            {
                stats = db.Stats.Find(id);
                if (stats == null)
                {
                    return HttpNotFound();
                }
            }
            return View(stats);
        }

        // GET: Stats/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name");
            }
            return View();
        }

        // POST: Stats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTaken,Publications")] Stats stats)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                if (ModelState.IsValid)
                {
                    db.Stats.Add(stats);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", stats.Publication);
            }
            return View(stats);
        }

        // GET: Stats/Edit
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stats stats;
            {
                stats = db.Stats.Find(id);
                if (stats == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", stats.Publication);
            }
            return View(stats);
        }

        // POST: Stats/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTaken,Publications")] Stats stats)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                if (ModelState.IsValid)
                {
                    db.Entry(stats).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", stats.Publication);
            }
            return View(stats);
        }

        // GET: Stats/Delete
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stats stats;
            {
                stats = db.Stats.Find(id);
                if (stats == null)
                {
                    return HttpNotFound();
                }
            }
            return View(stats);
        }

        // POST: Stats/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                Stats stats = db.Stats.Find(id);
                var publication = db.Publications.First(e => e.Stats.Any(d => d.Id == stats.Id));
                publication.Stats.Remove(stats);
                db.Stats.Remove(stats);
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
