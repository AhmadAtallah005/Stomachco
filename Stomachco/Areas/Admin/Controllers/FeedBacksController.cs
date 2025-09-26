using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stomachco.Data;
using Stomachco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stomachco.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class FeedBacksController : Controller
    {
        private readonly StomDbContext _context;

        public FeedBacksController(StomDbContext context)
        {
            _context = context;
        }

        // GET: Admin/FeedBacks
        public async Task<IActionResult> Index()
        {
            return View(await _context.feedBacks.ToListAsync());
        }

        // GET: Admin/FeedBacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.feedBacks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return View(feedBack);
        }

        // GET: Admin/FeedBacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/FeedBacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Email,Message")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedBack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedBack);
        }

        // GET: Admin/FeedBacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.feedBacks.FindAsync(id);
            if (feedBack == null)
            {
                return NotFound();
            }
            return View(feedBack);
        }

        // POST: Admin/FeedBacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Email,Message")] FeedBack feedBack)
        {
            if (id != feedBack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedBack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedBackExists(feedBack.Id))
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
            return View(feedBack);
        }

        // GET: Admin/FeedBacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.feedBacks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return View(feedBack);
        }

        // POST: Admin/FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedBack = await _context.feedBacks.FindAsync(id);
            if (feedBack != null)
            {
                _context.feedBacks.Remove(feedBack);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedBackExists(int id)
        {
            return _context.feedBacks.Any(e => e.Id == id);
        }
    }
}
