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
        // GET: Courses
        public ActionResult Index()
        {
            using (var db = new LibraryDatabase())
            {
                return View(db.Courses.ToList());
            }
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Courses course;
            using (var db = new LibraryDatabase())
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
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseNumber")] Courses course)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
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

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Courses course;
            using (var db = new LibraryDatabase())
            {
                course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseNumber")] Courses course)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
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

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Courses course;
            using (var db = new LibraryDatabase())
            {
                course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                Courses course = db.Courses.Find(id);
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
