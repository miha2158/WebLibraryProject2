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
        public ActionResult Index(int? PublicationId, string Search)
        {
            {
                var list = db.Authors.ToList();
                if (PublicationId != null)
                    list = list.Where(e => e.Publications.Any(f => f.Id == PublicationId)).ToList();
                if (Search != null)
                {
                    var query = Search.ToLower();
                    list = list.Where(g => g.First.ToLower().Contains(query) ||
                                           g.Last.ToLower().Contains(query) ||
                                           g.Patronimic.ToLower().Contains(query) ||
                                           g.toEnumWT.ToString().ToLower().Contains(query)).ToList();
                }
                return View(list.ToList());
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
                var publications = db.Publications.Where(e => e.Authors.Any(f => f.Id == author.Id));
                foreach (Publication publication in publications)
                {
                    publication.Authors.Remove(author);
                }
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
