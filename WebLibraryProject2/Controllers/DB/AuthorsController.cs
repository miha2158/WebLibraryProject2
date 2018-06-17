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
    public class AuthorsController : Controller
    {
        public LibraryDatabase db =  new LibraryDatabase();

        // GET: Authors
        public ActionResult Index(int? PublicationId)
        {
            {
                if (PublicationId == null)
                    return View(db.Authors.ToList());
                return View( db.Authors.Where(e => e.Publications.Any(f => f.Id == PublicationId)).ToList());
            }
        }

        public ActionResult Publications(int AuthorId)
        {
            return RedirectToAction("Index", "Publications", new { PublicationId = AuthorId });
        }

        // GET: Authors/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            {
                Author author = db.Authors.Find(id);
                if (author == null)
                {
                    return HttpNotFound();
                }

                return View(author);
            }
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,First,Last,Patronimic,WriterType")] Author author)
        {
            if (ModelState.IsValid)
        {
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Edit
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            {
                Author author = db.Authors.Find(id);
                if (author == null)
                {
                    return HttpNotFound();
                }

                return View(author);
            }
        }

        // POST: Authors/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,First,Last,Patronimic,WriterType")] Author author)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Delete
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            {
                Author author = db.Authors.Find(id);
                if (author == null)
                {
                    return HttpNotFound();
                }

                return View(author);
            }
        }

        // POST: Authors/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            {
                Author author = db.Authors.Find(id);
                db.Authors.Remove(author);
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
