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
    public class TematController : Controller
    {
        private ForumContext baza = new ForumContext();

        // GET: /Temat/
        public ActionResult Lista()
        {
            return View(baza.Tematy.ToList());
        }

        // GET: /Temat/Wyswietl/{id}
        public ActionResult Wyswietl(int id)
        {

            if (baza.Tematy.Find(id) != null)
            {
                ViewBag.TematId = id;
                ViewBag.TematTytul = baza.Tematy.Find(id).Tytul;
                ViewBag.ForumID = baza.Tematy.Find(id).Forum_ID;

                return View(baza.Tematy.Find(id).Posty.ToList());
            }

            return View();
        }

        // GET: /Temat/Utworz/{id}
        [Authorize]
        public ActionResult Utworz(int id)
        {
            TworzenieTematuModel model = new TworzenieTematuModel();
            model.fid = id;
            return View(model);
        }

        // POST: /Temat/Utworz
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Utworz([Bind(Include = "fid, Tytul,Tekst")] TworzenieTematuModel tem)
        {
            if (ModelState.IsValid)
            {
                TematModel temat = new TematModel();
                temat.DataUtworzenia = DateTime.Now;
                temat.Tytul = tem.Tytul;

                if (baza.Forum.Find(tem.fid).Tematy == null)
                {
                    List<TematModel> tematy = new List<TematModel>();
                    tematy.Add(temat);
                    baza.Forum.Find(tem.fid).Tematy = tematy;
                }
                else
                {
                    baza.Forum.Find(tem.fid).Tematy.Add(temat);
                }

                baza.Tematy.Add(temat);
                baza.SaveChanges();

                PostModel post = new PostModel();
                post.Tekst = tem.Tekst;
                post.DataUtworzenia = DateTime.Now;
                post.DataModyfikacji = DateTime.Now;
                var store = new UserStore<ApplicationUser>(baza);
                var userManager = new UserManager<ApplicationUser>(store);
                post.Autor = userManager.FindByNameAsync(User.Identity.Name).Result;

                baza.Tematy.Find(temat.ID).Posty = new List<PostModel>();
                baza.Tematy.Find(temat.ID).Posty.Add(post);

                baza.Posty.Add(post);
                baza.SaveChanges();

                return RedirectToAction("Wyswietl", new { id = temat.ID });
            }

            return View();
        }

        // GET: /Temat/Edytuj/{id}
        [Authorize]
        public ActionResult Edytuj(int id)
        {
            if (baza.Tematy.Find(id) != null)
            {
                return View(baza.Tematy.Find(id));
            }
            return View();
        }

        // POST: /Temat/Edytuj
        [HttpPost]
        [Authorize(Users = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edytuj([Bind(Include = "ID, Tytul")] TematModel temat)
        {
            if (ModelState.IsValid)
            {
                baza.Tematy.Find(temat.ID).Tytul = temat.Tytul;
                baza.SaveChanges();

                return RedirectToAction("Wyswietl", new { id = temat.ID });
            }

            return View(temat);
        }

        // GET: /Temat/Usun/{id}
        [Authorize]
        [Authorize(Users = "Admin")]
        public ActionResult Usun(int id)
        {
            if (baza.Tematy.Find(id) != null)
            {
                int forumID = baza.Tematy.Find(id).Forum_ID;
                baza.Tematy.Remove(baza.Tematy.Find(id));
                baza.SaveChanges();

                return RedirectToAction("Wyswietl", "Forum", new { id = forumID });
            }
            return RedirectToAction("Lista", "Forum");
        }
	}
}