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
        // GET: Authors
        public ActionResult Index()
        {
            using (var db = new LibraryDatabase())
            {
                return View(db.Authors.ToList());
            }
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new LibraryDatabase())
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
            if (!User.Identity.isAdmin())
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
                using (var db = new LibraryDatabase())
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new LibraryDatabase())
            {
                Author author = db.Authors.Find(id);
                if (author == null)
                {
                    return HttpNotFound();
                }

                return View(author);
            }
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,First,Last,Patronimic,WriterType")] Author author)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                using (var db = new LibraryDatabase())
                {
                    db.Entry(author).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new LibraryDatabase())
            {
                Author author = db.Authors.Find(id);
                if (author == null)
                {
                    return HttpNotFound();
                }

                return View(author);
            }
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();
            using (var db = new LibraryDatabase())
            {
                Author author = db.Authors.Find(id);
                db.Authors.Remove(author);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
