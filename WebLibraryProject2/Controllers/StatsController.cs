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
    public class StatsController : Controller
    {
        // GET: Stats
        public ActionResult Index()
        {
            using (var db = new LibraryDatabase())
                return View(db.Stats.Include(s => s.Publication).ToList());
        }

        // GET: Stats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stats stats;
            using (var db = new LibraryDatabase())
                stats = db.Stats.Find(id);

            if (stats == null)
            {
                return HttpNotFound();
            }

            return View(stats);
        }

        // GET: Stats/Create
        public ActionResult Create()
        {
            using (var db = new LibraryDatabase())
                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name");
            return View();
        }

        // POST: Stats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateTaken,Publications")] Stats stats)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                if (ModelState.IsValid)
                {
                    db.Stats.Add(stats);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", stats.Publications);
            }
            return View(stats);
        }

        // GET: Stats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stats stats;
            using (var db = new LibraryDatabase())
            {
                stats = db.Stats.Find(id);
                if (stats == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", stats.Publications);
            }
            return View(stats);
        }

        // POST: Stats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateTaken,Publications")] Stats stats)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(stats).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", stats.Publications);
            }
            return View(stats);
        }

        // GET: Stats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stats stats;
            using (var db = new LibraryDatabase())
                stats = db.Stats.Find(id);

            if (stats == null)
            {
                return HttpNotFound();
            }

            return View(stats);
        }

        // POST: Stats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            Stats stats;
            using (var db = new LibraryDatabase())
            {
                stats = db.Stats.Find(id);
                db.Stats.Remove(stats);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
