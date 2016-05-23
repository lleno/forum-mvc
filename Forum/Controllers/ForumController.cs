using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Forum.Models;

namespace Forum.Controllers
{
    public class ForumController : Controller
    {
        private ForumContext baza = new ForumContext();
        
        // GET: /Forum/
        public ActionResult Lista()
        {
            return View(baza.Forum.ToList());
        }

        // GET: /Forum/Wyswietl/{id}
        public ActionResult Wyswietl(int id)
        {
            
            if (baza.Forum.Find(id) != null)
            {
                ViewBag.ForumId = id;
                ViewBag.ForumNazwa = baza.Forum.Find(id).Nazwa;
                ViewBag.ForumOpis = baza.Forum.Find(id).Opis;

                return View(baza.Forum.Find(id).Tematy.ToList());
            }

            return View();
        }

        // GET: /Forum/Utworz
        [Authorize(Users = "Admin")]
        public ActionResult Utworz()
        {
            return View();
        }

        // POST: /Forum/Utworz
        [HttpPost]
        [Authorize(Users = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Utworz([Bind(Include = "Nazwa, Opis")] ForumModel forum)
        {
            if (ModelState.IsValid)
            {
                baza.Forum.Add(forum);
                baza.SaveChanges();
                return RedirectToAction("Wyswietl", new { id = forum.ID });
            }

            return View(forum);
        }

        // GET: /Forum/Edytuj/{id}
        [Authorize]
        public ActionResult Edytuj(int id)
        {
            if (baza.Forum.Find(id) != null)
            {
                return View(baza.Forum.Find(id));
            }
            return View();
        }

        // POST: /Forum/Edytuj
        [HttpPost]
        [Authorize(Users = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edytuj([Bind(Include = "ID, Nazwa, Opis")] ForumModel forum)
        {
            if (ModelState.IsValid)
            {
                baza.Forum.Find(forum.ID).Nazwa = forum.Nazwa;
                baza.Forum.Find(forum.ID).Opis = forum.Opis;
                baza.SaveChanges();

                return RedirectToAction("Wyswietl", new { id = forum.ID });
            }

            return View(forum);
        }

        // GET: /Forum/Usun/{id}
        [Authorize(Users = "Admin")]
        public ActionResult Usun(int id)
        {
            if (baza.Forum.Find(id) != null)
            {
                baza.Forum.Remove(baza.Forum.Find(id));
                baza.SaveChanges();
            }
            return RedirectToAction("Lista", "Forum");
        }
	}
}