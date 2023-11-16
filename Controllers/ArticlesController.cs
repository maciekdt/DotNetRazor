using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationList13X.Data;
using WebApplicationList13X.Models;
using WebApplicationList13X.ViewModels;

namespace WebApplicationList13X.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly MyDbContext _context;

        public ArticlesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Article.Include(p=>p.Category).ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,File,Price")] ArticleCreateViewModel articleViewModel)
        {
            string imageUri;
            if (articleViewModel.File == null)
            {
                imageUri = null;
            }
            else
            {
                imageUri = Guid.NewGuid().ToString() + "-" + articleViewModel.File.FileName;
                using (FileStream fs = new FileStream("wwwroot/upload/" + imageUri, FileMode.CreateNew))
                    await articleViewModel.File.CopyToAsync(fs);

            }
            Article article = new Article()
            {
                Name = articleViewModel.Name,
                ImageUri = imageUri,
                Price = articleViewModel.Price,
                Category = await _context.Category.FirstAsync(a => a.Id == articleViewModel.CategoryId)
            };
            
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        [Route("{id}/Edit")]
        public async Task<IActionResult> Edit([FromRoute]int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(p => p.Category)
                .FirstAsync(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_context.Category, "Id", "Name");
            return View(new ArticleEditViewModel() { Name = article.Name, Price = article.Price, CategoryId = article.Category.Id });
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("{id}/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ArticleEditViewModel articleViewModel)
        {
       

            if (ModelState.IsValid)
            {
                Article article = await _context.Article.FirstAsync(a => a.Id == id);
                article.Category = await _context.Category.FirstAsync(c => c.Id == articleViewModel.CategoryId);
                article.Name = articleViewModel.Name;
                article.Price = articleViewModel.Price;
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(articleViewModel);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article.FindAsync(id);
            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            if (article.ImageUri != null)
            {
                System.IO.File.Delete("wwwroot/upload/" + article.ImageUri);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
