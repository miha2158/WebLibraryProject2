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
    public class BookLocationsController : Controller
    {
        // GET: BookLocations
        public ActionResult Index()
        {
            using (var db = new LibraryDatabase())
            {
                var bookLocations = db.BookLocations.Include(b => b.Publication).Include(b => b.Reader);
                return View(bookLocations.ToList());
            }
        }

        // GET: BookLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookLocation bookLocation;
            using (var db = new LibraryDatabase())
            {
                bookLocation = db.BookLocations.Find(id);
                if (bookLocation == null)
                {
                    return HttpNotFound();
                }
            }
            return View(bookLocation);
        }

        // GET: BookLocations/Create
        public ActionResult Create()
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name");
                ViewBag.Readers = new SelectList(db.Readers, "Id", "First");
            }
            return View();
        }

        // POST: BookLocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Room,Place,IsTaken,Publications,Readers")] BookLocation bookLocation)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {

                if (ModelState.IsValid)
                {
                    db.BookLocations.Add(bookLocation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", bookLocation.Publications);
                ViewBag.Readers = new SelectList(db.Readers, "Id", "First", bookLocation.Readers);
            }
            return View(bookLocation);
        }

        // GET: BookLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookLocation bookLocation;
            using (var db = new LibraryDatabase())
            {
                bookLocation = db.BookLocations.Find(id);
                if (bookLocation == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", bookLocation.Publications);
                ViewBag.Readers = new SelectList(db.Readers, "Id", "First", bookLocation.Readers);
            }
            return View(bookLocation);
        }

        // POST: BookLocations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Room,Place,IsTaken,Publications,Readers")] BookLocation bookLocation)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(bookLocation).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", bookLocation.Publications);
                ViewBag.Readers = new SelectList(db.Readers, "Id", "First", bookLocation.Readers);
            }
            return View(bookLocation);
        }

        // GET: BookLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookLocation bookLocation;
            using (var db = new LibraryDatabase())
            {
                bookLocation = db.BookLocations.Find(id);
            }

            if (bookLocation == null)
            {
                return HttpNotFound();
            }
            return View(bookLocation);
        }

        // POST: BookLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                BookLocation bookLocation = db.BookLocations.Find(id);
                db.BookLocations.Remove(bookLocation);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
