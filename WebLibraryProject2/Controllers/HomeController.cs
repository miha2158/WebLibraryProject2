using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
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

        public ActionResult Report()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(int? reportType, DateTime? dateStart, DateTime? dateEnd, int[] authors)
        {
            string fileName = string.Empty;

            using (var db = new LibraryDatabase())
            {
                var Authors = db.Authors.ToList();
                if (authors != null)
                    Authors = Authors.Where(e => authors.Contains(e.Id)).ToList();

                var publications = db.Publications.Where(e => e.Authors.Any(f => Authors.Contains(f)));

                var stats = db.Stats.Where(e => publications.Contains(e.Publication)).ToList();
                if (dateStart.HasValue)
                    stats = stats.Where(e => e.DateTaken >= dateStart).ToList();
                if (dateEnd.HasValue)
                    stats = stats.Where(e => e.DateTaken <= dateEnd).ToList();


                var app = new Application
                {
                    DisplayAlerts = true,
                    Visible = false,
                };
                var wbook = app.Workbooks.Add(1);
                Worksheet wsheet = wbook.Worksheets[1];
                wsheet.Name = "Отчёт";

                switch (reportType)
                {
                    case 0:
                    {
                        var res = publications.Where(d => d.Authors.Any(f => f.toEnumWT == eWriterType.HseTeacher))
                                        .OrderBy(d => d.DDate)
                                        .ToArray();

                        wsheet.Cells[1, 1] = "Название";
                        wsheet.Cells[1, 2] = "Авторы";
                        wsheet.Cells[1, 3] = "Опубликовано";

                        for (int i = 0, offset = 2; i < res.Length; i++)
                        {
                            if (i == 0 || res[i].BookPublication != res[i - 1].BookPublication)
                                wsheet.Cells[i + offset++, 1] = $"{res[i].toEnumBP}s";

                            var authors1 = res[i].Authors
                                                .OrderBy(d => d.WriterType)
                                                .Select(d => d.ToString())
                                                .ToArray();

                            wsheet.Cells[i + offset, 1] = res[i].Name;
                            wsheet.Cells[i + offset, 2] = string.Join("\n", authors1);
                            wsheet.Cells[i + offset, 3] = $"{res[i].DDate}";

                        }
                        break;
                    }

                    case 1:
                    {
                        var res = publications.Where(e => e.Stats.Count > 0)
                                        .OrderBy(d => d.Stats.Count(e => stats.Contains(e)))
                                        .Reverse()
                                        .ToArray();

                        wsheet.Cells[1, 1] = "Название";
                        wsheet.Cells[1, 2] = "Авторы";
                        wsheet.Cells[1, 3] = "Взято раз";

                        for (int i = 0; i < res.Length; i++)
                        {
                            var authors1 = res[i].Authors
                                                .OrderBy(d => d.WriterType)
                                                .Select(d => d.ToString())
                                                .ToArray();

                            wsheet.Cells[i + 2, 1] = res[i].Name;
                            wsheet.Cells[i + 2, 2] = string.Join("\n", authors1);
                            wsheet.Cells[i + 2, 3] = res[i].Stats.Count(e => stats.Contains(e));
                        }

                        break;
                    }

                    case 2:
                    {
                        var res = publications.SelectMany(d => d.Locations)
                                        .Where(d => d.IsTaken)
                                        .OrderBy(d => d.Publication.Stats.Last().DateTaken.ToNiceDate())
                                        .ToArray();

                        wsheet.Cells[1, 1] = "Название";
                        wsheet.Cells[1, 2] = "Авторы";
                        wsheet.Cells[1, 3] = "Взявший";
                        wsheet.Cells[1, 4] = "Взято";

                        for (int i = 0; i < res.Length; i++)
                        {
                            var el = res[i];

                            wsheet.Cells[i + 2, 1] = el.Publication.Name;
                            wsheet.Cells[i + 2, 2] = string.Join("\n", el.Publication.Authors.Select(d => d.ToString()));
                            wsheet.Cells[i + 2, 3] = $"{el.Reader}, {el.Reader.Group}";
                            wsheet.Cells[i + 2, 4] = el.Publication.Stats.Last().DateTaken;
                        }

                        break;
                    }

                    case null:
                        return HttpNotFound();
                }

                wsheet.Columns.ColumnWidth = 255;
                wsheet.Rows.RowHeight = 255;
                wsheet.Columns.AutoFit();
                wsheet.Rows.AutoFit();

                fileName = $@"D:\Projects\WebLibraryProject2\WebLibraryProject2\bin\Report{reportType}.xlsx";
                wbook.SaveAs(fileName);
                wbook.Close(0);
                app.Quit();

            }

            var file = new FileInfo(fileName);
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.Flush();
                Response.TransmitFile(file.FullName);
                Response.End();
            }
            

            return RedirectToAction("Index");
        }
    }
}