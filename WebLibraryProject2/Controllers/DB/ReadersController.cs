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
    public class ReadersController : Controller
    {
        public LibraryDatabase db = new LibraryDatabase();

        // GET: Readers
        public ActionResult Index(string Search)
        {
            var list = db.Readers.AsQueryable();
            if (Search != null)
            {
                string query = Search.ToLower();
                list = list.Where(d => d.First.ToLower().Contains(query) ||
                                       d.Last.ToLower().Contains(query) ||
                                       d.Patronimic.ToLower().Contains(query) ||
                                       d.Group.ToString().ToLower().Contains(query) ||
                                       d.toEnumAL.ToString().Contains(query));
            }
            return View(list.ToList());
        }

        public ActionResult Publications(int ReaderId)
        {
            return RedirectToAction("Index", "Publications", new {ReaderId = ReaderId});
        }

        // GET: Readers/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader reader;
            reader = db.Readers.Find(id);

            if (reader == null)
            {
                return HttpNotFound();
            }
            return View(reader);
        }

        // GET: Readers/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            return View();
        }

        // POST: Readers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,First,Last,Patronimic,AccessLevel,Group")] Reader reader)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();
            
            if (ModelState.IsValid)
            {
                if (reader.Group == null)
                    reader.Group = string.Empty;

                db.Readers.Add(reader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reader);
        }

        // GET: Readers/Edit
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader reader;
            reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }

            return View(reader);
        }

        // POST: Readers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,First,Last,Patronimic,AccessLevel,Group")] Reader reader)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();


            if (ModelState.IsValid)
            {
                if (reader.Group == null)
                    reader.Group = string.Empty;
                db.Entry(reader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reader);
        }

        // GET: Readers/Delete
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reader reader;
                reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }
            return View(reader);
        }

        // POST: Readers/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
                return HttpNotFound();

            {
                Reader reader = db.Readers.Find(id);
                db.Readers.Remove(reader);
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
