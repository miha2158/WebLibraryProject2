using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibraryProject2.Models;

namespace WebLibraryProject2.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Search(string SearchBox)
        {
            if (SearchBox == null)
                return View();
            return View(new SearchModel{Search = SearchBox});
        }

        public ActionResult Publications(string Search)
        {
            return RedirectToAction("Index", "Publications", new {Search = Search});
        }

        public ActionResult Authors(string Search)
        {
            return RedirectToAction("Index", "Authors", new {Search = Search});
        }

        public ActionResult Readers(string Search)
        {
            return RedirectToAction("Index", "Readers", new {Search = Search});
        }
    }
}