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
    public class CoursesController : Controller
    {
        public LibraryDatabase db = new LibraryDatabase();

        // GET: Courses
        public ActionResult Index(int? PublicationId)
        {
            {
                if (PublicationId == null)
                    return View(db.Courses.ToList());
                return View(db.Courses.Where(e => e.Publications.Any(d => d.Id == PublicationId)).ToList());
            }
        }

        // GET: Courses/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Courses course;
            {
                course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseNumber")] Courses course)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(course);
        }

        // GET: Courses/Edit
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Courses course;
            {
                course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
            }
            return View(course);
        }

        // POST: Courses/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseNumber")] Courses course)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(course);
        }

        // GET: Courses/Delete
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Courses course;
            {
                course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
            }
            return View(course);
        }

        // POST: Courses/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                Courses course = db.Courses.Find(id);
                db.Courses.Remove(course);
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
