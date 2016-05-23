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
    public class PostController : Controller
    {
        private ForumContext baza = new ForumContext();

        // GET: /Post/
        public ActionResult Lista()
        {
            return View(baza.Posty.ToList());
        }

        // GET: /Post/Utworz/{id}
        [Authorize]
        public ActionResult Utworz(int id)
        {
            PostModel model = new PostModel();
            model.ID = id;
            return View(model);
        }

        // POST: /Post/Utworz
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Utworz([Bind(Include = "ID, Tekst")] PostModel post)
        {
            if (ModelState.IsValid)
            {
                post.DataUtworzenia = DateTime.Now;
                post.DataModyfikacji = DateTime.Now;
                var store = new UserStore<ApplicationUser>(baza);
                var userManager = new UserManager<ApplicationUser>(store);
                post.Autor = userManager.FindByNameAsync(User.Identity.Name).Result;

                if (baza.Tematy.Find(post.ID).Posty == null)
                {
                    List<PostModel> posty = new List<PostModel>();
                    posty.Add(post);
                    baza.Tematy.Find(post.ID).Posty = posty;
                }
                else
                {
                    baza.Tematy.Find(post.ID).Posty.Add(post);
                }
                
                baza.Posty.Add(post);
                baza.SaveChanges();

                return RedirectToAction("Wyswietl", "Temat", new { id = post.Temat_ID });
            }

            return View(post);
        }

        // GET: /Post/Edytuj/{id}
        [Authorize]
        public ActionResult Edytuj(int id)
        {
            if (baza.Posty.Find(id) != null)
            {
                ViewBag.TematID = baza.Posty.Find(id).Temat_ID;
                return View(baza.Posty.Find(id));
            }
            return View();
        }

         // POST: /Post/Edytuj
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edytuj([Bind(Include = "ID, Tekst")] PostModel post)
        {
            if (ModelState.IsValid)
            {
                var store = new UserStore<ApplicationUser>(baza);
                var userManager = new UserManager<ApplicationUser>(store);
                var uzytkownik = userManager.FindByNameAsync(User.Identity.Name).Result;
                var admin = userManager.FindByNameAsync("Admin").Result;
                if (uzytkownik == baza.Posty.Find(post.ID).Autor || uzytkownik == admin)
                {
                    baza.Posty.Find(post.ID).DataModyfikacji = DateTime.Now;
                    baza.Posty.Find(post.ID).Tekst = post.Tekst;
                    baza.SaveChanges();

                    return RedirectToAction("Wyswietl", "Temat", new { id = baza.Posty.Find(post.ID).Temat_ID });
                }  
            }

            return View(post);
        }

        // GET: /Post/Usun/{id}
        [Authorize]
        public ActionResult Usun(int id)
        {
            if (baza.Posty.Find(id) != null)
            {
                var store = new UserStore<ApplicationUser>(baza);
                var userManager = new UserManager<ApplicationUser>(store);
                var uzytkownik = userManager.FindByNameAsync(User.Identity.Name).Result;
                var admin = userManager.FindByNameAsync("Admin").Result;
                if (uzytkownik == baza.Posty.Find(id).Autor || uzytkownik == admin)
                {
                    int TematID = baza.Posty.Find(id).Temat_ID;

                    PostModel post = baza.Posty.Find(id);
                    baza.Posty.Remove(post);
                    baza.SaveChanges();

                    return RedirectToAction("Wyswietl", "Temat", new { id = TematID });
                }
            }
            return RedirectToAction("Lista", "Forum");
        }
	}
}