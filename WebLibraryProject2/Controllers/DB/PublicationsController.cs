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
        public ActionResult Index(int? AuthorId)
        {
            {
                if (AuthorId == null)
                    return View(db.Publications.ToList());
                return View(db.Publications.Where(d => d.Authors.Any(f => f.Id == AuthorId)).ToList());
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
        public ActionResult Create([Bind(Include = "Id,Name,DatePublished,PublicationType,Publisher,InternetLocation")] Publication publication, string[] Authors, string[] Courses)
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
        public ActionResult Edit([Bind(Include = "Id,Name,DatePublished,PublicationType,Publisher,InternetLocation")] Publication publication, string[] Authors, string[] Courses)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            ViewBag.db = db;

            {
                var authors = new List<Author>();
                foreach (Author author in db.Authors)
                {
                    if (Authors.Any(e => e == author.ToString()))
                        authors.Add(author);
                }

                foreach (Author author in publication.Authors)
                {
                    if (!authors.Contains(author))
                        publication.Authors.Remove(author);
                }

                foreach (Author author in authors)
                {
                    if (!publication.Authors.Contains(author))
                        publication.Authors.Add(author);
                }
            }


            {
                var courses = new List<Courses>();
                foreach (Courses course in db.Courses)
                {
                    if (Courses.Any(e => e == course.ToString()))
                        courses.Add(course);
                }

                foreach (Courses course in publication.Courses)
                {
                    if (!courses.Contains(course))
                        publication.Courses.Remove(course);
                }

                foreach (Courses course in courses)
                {
                    if (!publication.Courses.Contains(course))
                        publication.Courses.Add(course);
                }

                foreach (Courses course in db.Courses)
                {
                    if (Courses.Any(e => e == course.ToString()))
                        publication.Courses.Add(course);
                }
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
