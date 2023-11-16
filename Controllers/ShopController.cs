using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationList13X.Data;
using WebApplicationList13X.Models;

namespace WebApplicationList13X.Controllers
{
    public class ShopController : Controller
    {
        private readonly MyDbContext _context;

        public ShopController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Shop
        [Route("Index/")]
        public async Task<IActionResult> Index()
        {
            ViewData["Category"] = await _context.Category.ToListAsync();
            return View(new LinkedList<Article>());
        }

        // GET: Shop
        [Route("Index/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            ViewData["Category"] = await _context.Category.ToListAsync();
            return View(await _context.Article
                .Where(a => a.Category.Id == id)
                .Include(p => p.Category)
                .ToListAsync());
            
        }

        [Route("AddToCart/{id}")]
        public IActionResult AddToCart(int? id)
        {
            int amount = 1;
            if (Request.Cookies.ContainsKey(id.ToString()))
                amount = Int16.Parse(Request.Cookies[id.ToString()]);
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(id.ToString(), amount.ToString(), option);
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
