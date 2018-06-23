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
    public class PublicationsController : Controller
    {
        public LibraryDatabase db = new LibraryDatabase();

        // GET: Publications
        public ActionResult Index(int? AuthorId, int? CourseId, int? ReaderId, string Search)
        {
            {
                
                var list = db.Publications.ToList();
                if (AuthorId != null)
                    list = list.Where(d => d.Authors.Any(f => f.Id == AuthorId)).ToList();
                if (CourseId != null)
                    list = list.Where(d => d.Courses.Any(f => f.Id == CourseId)).ToList();
                if (ReaderId != null)
                    list = list.Where(d => d.BookLocations.Any(e => e.Reader.Id == ReaderId)).ToList();
                if (Search != null)
                {
                    string query = Search.ToLower();
                    list = list.Where(d => d.Name.ToLower().Contains(query) ||
                                           d.DatePublished.ToLongDateString().ToLower().Contains(query) ||
                                           d.Courses.Any(f => f.Course.ToString().ToLower().Contains(query)) ||
                                           d.Disciplines.Any(f => f.Name.ToLower().Contains(query)) ||
                                           d.toEnumBP.ToString().ToLower().Contains(query) ||
                                           d.toEnumPT.ToString().ToLower().Contains(query)).ToList();
                }
                return View(list.ToList());
            }
        }

        public ActionResult Authors(int PublicationId)
        {
            return RedirectToAction("Index", "Authors", new {PublicationId = PublicationId});
        }

        public ActionResult BookLocations(int PublicationId)
        {
            return RedirectToAction("Index", "BookLocations", new {PublicationId = PublicationId});
        }

        public ActionResult Courses(int PublicationId)
        {
            return RedirectToAction("Index", "Courses", new {PublicationId = PublicationId});
        }

        // GET: Publications/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publication publication;
            publication = db.Publications.Find(id);

            if (publication == null)
            {
                return HttpNotFound();
            }

            ViewBag.db = db;
            return View(publication);
        }

        // GET: Publications/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            ViewBag.db = db;

            return View();
        }

        // POST: Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Name,DatePublished,toEnumPT,Publisher,InternetLocation")] Publication publication, string[] Authors, string[] Courses, string[] Disciplines)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            ViewBag.db = db;

            foreach (Author author in db.Authors)
            {
                if(Authors.Any(e => e == author.ToString()))
                    publication.Authors.Add(author);
            }

            foreach (Courses course in db.Courses)
            {
                if(Courses.Any(e => e == course.ToString()))
                    publication.Courses.Add(course);
            }

            foreach (var discipline in db.Disciplines)
            {
                if (Disciplines.Any(e => e == discipline.ToString()))
                    publication.Disciplines.Add(discipline);
            }

            if (ModelState.IsValid)
            {
                db.Publications.Add(publication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publication);
        }

        // GET: Publications/Edit
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publication publication;
            ViewBag.db = db;
            publication = db.Publications.Find(id);

            if (publication == null)
            {
                return HttpNotFound();  
            }
            return View(publication);
        }

        // POST: Publications/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DatePublished,toEnumPT,Publisher,InternetLocation")] Publication publication, string[] Authors, string[] Courses, string[] Disciplines)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            ViewBag.db = db;

            var publication2 = db.Publications.Find(publication.Id);
            publication2.Id = publication.Id;
            publication2.Publisher = publication2.Publisher;
            publication2.Name = publication.Name;
            publication2.toEnumPT = publication.toEnumPT;
            publication2.DatePublished = publication.DatePublished;
            publication2.InternetLocation = publication.InternetLocation;
            publication = publication2;

            if(Authors != null)
            {
                publication.Authors.Clear();
                foreach (var author in db.Authors)
                    if (Authors.Any(e => e == author.ToString()))
                        publication.Authors.Add(author);
            }

            if(Courses != null)
            {
                publication.Courses.Clear();
                foreach (Courses course in db.Courses)
                    if (Courses.Any(e => e == course.ToString()))
                        publication.Courses.Add(course);
            }

            if(Disciplines != null)
            {
                publication.Disciplines.Clear();
                foreach (var discipline in db.Disciplines)
                    if (Disciplines.Any(e => e == discipline.ToString()))
                        publication.Disciplines.Add(discipline);
            }

            if (ModelState.IsValid)
            {
                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publication);
        }

        // GET: Publications/Delete
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            ViewBag.db = db;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publication publication;
            publication = db.Publications.Find(id);

            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publications/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                Publication publication = db.Publications.Find(id);
                publication.Authors.Clear();
                publication.Courses.Clear();
                publication.Disciplines.Clear();
                foreach (BookLocation location in db.BookLocations)
                    if (Equals(location.Publication, publication))
                        db.BookLocations.Remove(location);
                db.Publications.Remove(publication);
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
