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
        // GET: Publications
        public ActionResult Index()
        {
            using (var db = new LibraryDatabase())
                return View(db.Publications.ToList());
        }

        // GET: Publications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publication publication;
            using (var db = new LibraryDatabase())
                publication = db.Publications.Find(id);

            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // GET: Publications/Create
        public ActionResult Create()
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            return View();
        }

        // POST: Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DatePublished,PublicationType,Publisher,InternetLocation,BookPublication")] Publication publication)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
                if (ModelState.IsValid)
            {
                db.Publications.Add(publication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publication);
        }

        // GET: Publications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publication publication;
            using (var db = new LibraryDatabase())
                publication = db.Publications.Find(id);

            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DatePublished,PublicationType,Publisher,InternetLocation,BookPublication")] Publication publication)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
                if (ModelState.IsValid)
                {
                    db.Entry(publication).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            return View(publication);
        }

        // GET: Publications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publication publication;
            using (var db = new LibraryDatabase())
                publication = db.Publications.Find(id);

            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.isAdmin())
                return HttpNotFound();

            using (var db = new LibraryDatabase())
            {
                Publication publication = db.Publications.Find(id);
                db.Publications.Remove(publication);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
