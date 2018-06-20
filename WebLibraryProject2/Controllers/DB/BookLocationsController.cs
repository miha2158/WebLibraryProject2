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
    public class BookLocationsController : Controller
    {
        public LibraryDatabase db = new LibraryDatabase();
        // GET: BookLocations
        public ActionResult Index(int? PublicationId)
        {
            {
                List<BookLocation> bookLocations;
                if (PublicationId == null)
                    bookLocations = db.BookLocations.Include(b => b.Publication).Include(b => b.Reader).ToList();
                else
                    bookLocations = db.BookLocations.Where(e => e.Publication.Id == PublicationId).Include(b => b.Publication).Include(b => b.Reader).ToList();
                return View(bookLocations);
            }
        }

        // GET: BookLocations/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookLocation bookLocation;
            {
                bookLocation = db.BookLocations.Find(id);
                if (bookLocation == null)
                {
                    return HttpNotFound();
                }
            }
            return View(bookLocation);
        }

        public ActionResult ReaderDetails(int id)
        {
            return RedirectToAction("Details", "Readers", new {id = id});
        }

        // GET: BookLocations/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                ViewBag.Publications = new SelectList(db.Publications);
                ViewBag.Readers = new SelectList(db.Readers);
            }
            return View();
        }

        // POST: BookLocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Room,Place,IsTaken,Publications,Readers")]
                                   BookLocation bookLocation)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                db.BookLocations.Add(bookLocation);
                db.Stats.Add(new Stats {DateTaken = DateTime.Now, Publication = bookLocation.Publication});
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Publications = new SelectList(db.Publications, bookLocation.Publication);
            ViewBag.Readers = new SelectList(db.Readers, bookLocation.Reader);

            return View(bookLocation);
        }

        // GET: BookLocations/Edit
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookLocation bookLocation;
            bookLocation = db.BookLocations.Find(id);
            if (bookLocation == null)
            {
                return HttpNotFound();
            }

            ViewBag.Publications = new SelectList(db.Publications, bookLocation.Publication);
            ViewBag.Readers = new SelectList(db.Readers, bookLocation.Reader);
            return View(bookLocation);
        }

        // POST: BookLocations/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Room,Place,IsTaken")] BookLocation bookLocation, string Readers)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            if (ModelState.IsValid)
            {
                var Reader = db.Readers.ToList().First(e => e.ToString() == Readers);
                db.Entry(bookLocation).State = EntityState.Modified;
                if (!db.BookLocations.Find(bookLocation.Id).IsTaken && bookLocation.IsTaken)
                    db.Stats.Add(new Stats {DateTaken = DateTime.Now, Publication = bookLocation.Publication});
                db.BookLocations.Find(bookLocation.Id).Reader = Reader;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Publications = new SelectList(db.Publications, bookLocation.Publication);
            ViewBag.Readers = new SelectList(db.Readers, bookLocation.Reader);

            db.Dispose();
            db = null;
            return View(bookLocation);
        }

        // GET: BookLocations/Delete
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookLocation bookLocation;
            {
                bookLocation = db.BookLocations.Find(id);
                if (bookLocation == null)
                {
                    return HttpNotFound();
                }
            }
            return View(bookLocation);
        }

        // POST: BookLocations/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                BookLocation bookLocation = db.BookLocations.Find(id);
                var publications = db.Publications.Where(e => e.BookLocations.Any(f => f.Id == bookLocation.Id));
                foreach (Publication publication in publications)
                    publication.BookLocations.Remove(bookLocation);

                var readers = db.Readers.Where(e => e.BookLocations.Any(f => f.Id == bookLocation.Id));
                foreach (Reader reader in readers)
                    reader.BookLocations.Remove(bookLocation);

                db.BookLocations.Remove(bookLocation);
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
