using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationList13X.Data;
using WebApplicationList13X.Models;
using WebApplicationList13X.ViewModels;

namespace WebApplicationList13X.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly MyDbContext _context;

        public ShopCartController(MyDbContext context)
        {
            _context = context;
        }

        // GET: ShopCart
        public IActionResult Index()
        {
            var articles = _context.Article
                .Include(a => a.Category)
                .AsEnumerable()
                .Where(a => Request.Cookies.ContainsKey(a.Id.ToString()))
                .ToList()
                .ConvertAll(a => new ArticleCardViewModel
                {
                    Article = a,
                    Amount = Int16.Parse(Request.Cookies[a.Id.ToString()])
                });

            int sum = 0;
            foreach(ArticleCardViewModel article in articles)
            {
                sum += article.Amount * article.Article.Price;
            }
            ViewBag.Sum = sum;
            return View(articles);
        }


        [Route("Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (Request.Cookies[id.ToString()] != null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(id.ToString(), "0", option);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("AddOne/{id}")]
        public IActionResult AddOne(int? id)
        {
            if (Request.Cookies[id.ToString()] != null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append(id.ToString(), (Int16.Parse(Request.Cookies[id.ToString()])+1).ToString(), option);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("RemoveOne/{id}")]
        public IActionResult RemoveOne(int? id)
        {
            if (Request.Cookies[id.ToString()] != null && Int16.Parse(Request.Cookies[id.ToString()]) > 1)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append(id.ToString(), (Int16.Parse(Request.Cookies[id.ToString()]) - 1).ToString(), option);
            }
            return RedirectToAction(nameof(Index));
        }


        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
