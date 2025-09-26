using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stomachco.Data;
using Stomachco.Models;
using Stomachco.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stomachco.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]

    public class SuperMarketsController : Controller
    {
        private readonly StomDbContext _context;
        private IWebHostEnvironment _environment;

        public SuperMarketsController(StomDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/SuperMarkets
        public async Task<IActionResult> Index()
        {
            return View(await _context.superMarkets.Where(x=>x.isPublished==true &&x.isDeleted==false).ToListAsync());
        }

        // GET: Admin/SuperMarkets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superMarket = await _context.superMarkets
                .FirstOrDefaultAsync(m => m.SuperMarketId == id);
            if (superMarket == null)
            {
                return NotFound();
            }

            return View(superMarket);
        }

        // GET: Admin/SuperMarkets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SuperMarkets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( SuperMarketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Image = FileUpload(model);
                SuperMarket superMarket = new SuperMarket { 
                CreationDate=model.CreationDate,    
                isDeleted=model.isDeleted,
                SuperMarketId= Guid.NewGuid(),
                isPublished=model.isPublished,
                SMImage=Image,
                SMName=model.SMName,
                UserId = model.UserId
                
                };

                _context.Add(superMarket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/SuperMarkets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superMarket = await _context.superMarkets.FindAsync(id);
            if (superMarket == null)
            {
                return NotFound();
            }
            var model = new SuperMarketViewModel {
            CreationDate=superMarket.CreationDate,
            isPublished=superMarket.isPublished,
            isDeleted=superMarket.isDeleted,
            SuperMarketId=superMarket.SuperMarketId,
            UserId=superMarket.UserId,
            SMName = superMarket.SMName,
           Image=superMarket.SMImage,
            
            };
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,  SuperMarketViewModel model)
        {
            if (id != model.SuperMarketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imgName;
                    if (model.SMImage == null)
                    {
                        var x = _context.restaurants.Find(model.SuperMarketId);
                        imgName = x!.RestImage!;
                    }
                    else
                    {
                        var image = FileUpload(model);
                        SuperMarket Sm = new SuperMarket {
                            CreationDate = model.CreationDate,
                            isPublished = model.isPublished,
                            isDeleted = model.isDeleted,
                            SuperMarketId = model.SuperMarketId,
                            UserId = model.UserId,
                            SMName = model.SMName,
                            SMImage = image
                        };

                    
                        _context.Update(Sm);
                    await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuperMarketExists(model.SuperMarketId))
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
            return View(model);
        }

        // GET: Admin/SuperMarkets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superMarket = await _context.superMarkets
                .FirstOrDefaultAsync(m => m.SuperMarketId == id);
            if (superMarket == null)
            {
                return NotFound();
            }

            return View(superMarket);
        }

        // POST: Admin/SuperMarkets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var superMarket = await _context.superMarkets.FindAsync(id);
            if (superMarket != null)
            {
                _context.superMarkets.Remove(superMarket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuperMarketExists(Guid id)
        {
            return _context.superMarkets.Any(e => e.SuperMarketId == id);
        }

        public async Task<IActionResult> AllSuperMarkets()
        {

            return View(await _context.superMarkets.ToListAsync());
        }

        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var data = _context.superMarkets.Find(id);
            if (data == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    data.isDeleted = true;
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuperMarketExists(data.SuperMarketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Deleted()
        {
            return View(await _context.superMarkets.Where(x => x.isDeleted == true).ToListAsync());

        }

        public IActionResult Remove(Guid id)
        {
            var res = _context.superMarkets.Find(id);
            res!.isDeleted = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Abroved()
        {
            return View(await _context.superMarkets.Where(x => x.isPublished == false).ToListAsync());
        }

        public IActionResult Published(Guid id)
        {


            var data = _context.superMarkets.Find(id);
            data!.isPublished = true;
            _context.SaveChanges();
            return RedirectToAction("Index");



        }

        public IActionResult UnPublished(Guid id)
        {
            var data = _context.superMarkets.Find(id);
            data!.isPublished = false;
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public string FileUpload(SuperMarketViewModel model)
        {
            string wwwPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(wwwPath)) { }
            string contentPath = _environment.ContentRootPath;
            if (string.IsNullOrEmpty(contentPath)) { }
            string p = Path.Combine(wwwPath, "Images");
            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }
            string FillName = Path.GetFileNameWithoutExtension(model.SMImage!.FileName);
            string newImgName = "Nextwo_" + FillName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.SMImage.FileName);

            using (FileStream fileStream = new FileStream(Path.Combine(p, newImgName), FileMode.Create))
            {
                model.SMImage.CopyTo(fileStream);
            }

            return "\\Images\\" + newImgName;
        }

    }
}
