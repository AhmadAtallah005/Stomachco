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
    [Authorize]

    public class CoffeesController : Controller
    {
        private readonly StomDbContext _context;
        private IWebHostEnvironment _environment;
        public CoffeesController(StomDbContext context, IWebHostEnvironment environment )
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/Coffees
        public async Task<IActionResult> Index()
        {
            return View(await _context.coffees.Where(x=>x.isPublished==true && x.isDeleted==false).ToListAsync());
        }

        // GET: Admin/Coffees/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.coffees
                .FirstOrDefaultAsync(m => m.CoffeeId == id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // GET: Admin/Coffees/Create
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoffeeViewModel coffee)
        {
            if (ModelState.IsValid)
            {

                var Image = FileUpload(coffee);
                Coffee coff = new Coffee
                {
                    CofeName = coffee.CofeName,
                    CofeDescription = coffee.CofeDescription,
                    CoffeeId = Guid.NewGuid(),
                    CreationDate = coffee.CreationDate,
                    isDeleted = coffee.isDeleted,
                    isPublished = coffee.isPublished,
                    UserId = coffee.UserId,
                    CoffeeImage = Image

                };

                _context.Add(coff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coffee);
        }

        // GET: Admin/Coffees/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.coffees.FindAsync(id);
            if (coffee == null)
            {
                return NotFound();
            }
            var model = new CoffeeViewModel { 
            CofeName=coffee.CofeName,
            isDeleted=coffee.isDeleted,
            isPublished=coffee.isPublished,
            UserId=coffee.UserId,
            CofeDescription = coffee.CofeDescription,
            CreationDate = coffee.CreationDate,
            Image=coffee.CoffeeImage,
            CoffeeId=coffee.CoffeeId,
            };

            return View(model);
        }

        // POST: Admin/Coffees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CoffeeViewModel model)
        {
            if (id != model.CoffeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imgName;
                    if (model.CoffeeImage == null)
                    {
                        var x = _context.restaurants.Find(model.CoffeeId);
                        imgName = x!.RestImage!;
                    }
                    else
                    {
                        var image = FileUpload(model);
                        Coffee coffee = new Coffee
                        {
                            CofeName = model.CofeName,
                            isDeleted = model.isDeleted,
                            isPublished = model.isPublished,
                            UserId = model.UserId,
                            CofeDescription = model.CofeDescription,
                            CreationDate = model.CreationDate,
                            CoffeeImage = image,
                            CoffeeId = model.CoffeeId,
                        };


                        _context.Update(coffee);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoffeeExists(model.CoffeeId))
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

        // GET: Admin/Coffees/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.coffees
                .FirstOrDefaultAsync(m => m.CoffeeId == id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // POST: Admin/Coffees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var coffee = await _context.coffees.FindAsync(id);
            if (coffee != null)
            {
                _context.coffees.Remove(coffee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoffeeExists(Guid id)
        {
            return _context.coffees.Any(e => e.CoffeeId == id);
        }

        public async Task<IActionResult> AllCoffees()
        {

            return View(await _context.coffees.ToListAsync());
        }

        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var data = _context.coffees.Find(id);
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
                    if (!CoffeeExists(data.CoffeeId))
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
            return View(await _context.coffees.Where(x => x.isDeleted == true).ToListAsync());

        }

        public IActionResult Remove(Guid id)
        {
            var res = _context.coffees.Find(id);
            res!.isDeleted = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Abroved()
        {
            return View(await _context.coffees.Where(x => x.isPublished == false).ToListAsync());
        }

        public IActionResult Published(Guid id)
        {


            var data = _context.coffees.Find(id);
            data!.isPublished = true;
            _context.SaveChanges();
            return RedirectToAction("Index");



        }

        public IActionResult UnPublished(Guid id)
        {
            var data = _context.coffees.Find(id);
            data!.isPublished = false;
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public string FileUpload(CoffeeViewModel model)
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
            string FillName = Path.GetFileNameWithoutExtension(model.CoffeeImage!.FileName);
            string newImgName = "Nextwo_" + FillName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.CoffeeImage.FileName);

            using (FileStream fileStream = new FileStream(Path.Combine(p, newImgName), FileMode.Create))
            {
                model.CoffeeImage.CopyTo(fileStream);
            }

            return "\\Images\\" + newImgName;
        }
    }
}
