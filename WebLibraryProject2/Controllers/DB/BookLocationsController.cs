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
        private LibraryDatabase db = new LibraryDatabase();

        // GET: BookLocations
        public ActionResult Index()
        {
            var bookLocations = db.BookLocations.Include(b => b.Publication);
            return View(bookLocations.ToList());
        }

        // GET: BookLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookLocation bookLocation = db.BookLocations.Find(id);
            if (bookLocation == null)
            {
                return HttpNotFound();
            }
            return View(bookLocation);
        }

        // GET: BookLocations/Create
        public ActionResult Create()
        {
            ViewBag.Publications = new SelectList(db.Publications, "Id", "Name");
            return View();
        }

        // POST: BookLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Room,Place,IsTaken,Publications")] BookLocation bookLocation)
        {
            if (ModelState.IsValid)
            {
                db.BookLocations.Add(bookLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", bookLocation.Publications);
            return View(bookLocation);
        }

        // GET: BookLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookLocation bookLocation = db.BookLocations.Find(id);
            if (bookLocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", bookLocation.Publications);
            return View(bookLocation);
        }

        // POST: BookLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Room,Place,IsTaken,Publications")] BookLocation bookLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Publications = new SelectList(db.Publications, "Id", "Name", bookLocation.Publications);
            return View(bookLocation);
        }

        // GET: BookLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookLocation bookLocation = db.BookLocations.Find(id);
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
            BookLocation bookLocation = db.BookLocations.Find(id);
            db.BookLocations.Remove(bookLocation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
