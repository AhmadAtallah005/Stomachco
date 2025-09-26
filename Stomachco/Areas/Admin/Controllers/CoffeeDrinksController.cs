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
    public class CoffeeDrinksController : Controller
    {
        private readonly StomDbContext _context;
        private IWebHostEnvironment _environment;

        public CoffeeDrinksController(StomDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/CoffeeDrinks
        public async Task<IActionResult> Index()
        {
            return View(await _context.coffeeDrinks.Where(x=>x.isPublished==true &&x.isDeleted==false).ToListAsync());
        }

        // GET: Admin/CoffeeDrinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffeeDrinks = await _context.coffeeDrinks
                .FirstOrDefaultAsync(m => m.ID == id);
            if (coffeeDrinks == null)
            {
                return NotFound();
            }

            return View(coffeeDrinks);
        }

        // GET: Admin/CoffeeDrinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CoffeeDrinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CoffeeDrinksViewModel model)
        {
            if (ModelState.IsValid)
            {
                var image = FileUpload(model);
                CoffeeDrinks drink= new CoffeeDrinks
                {
                    DrinkDescription=model.DrinkDescription,
                    DrinkName=model.DrinkName,
                    ID = model.ID,
                    isDeleted=model.isDeleted,
                    isPublished=model.isPublished,
                    Price=model.Price,
                    PriceUnit=model.PriceUnit,
                    UserId = model.UserId,
                    CreationDate=model.CreationDate,
                    DrinkImg=image,
                
                
                }; 

                _context.Add(drink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/CoffeeDrinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffeeDrinks = await _context.coffeeDrinks.FindAsync(id);
            if (coffeeDrinks == null)
            {
                return NotFound();
            }
            var model = new CoffeeDrinksViewModel {
                DrinkDescription = coffeeDrinks.DrinkDescription,
                DrinkName = coffeeDrinks.DrinkName,
                ID = coffeeDrinks.ID,
                isDeleted = coffeeDrinks.isDeleted,
                isPublished = coffeeDrinks.isPublished,
                Price = coffeeDrinks.Price,
                PriceUnit = coffeeDrinks.PriceUnit,
                UserId = coffeeDrinks.UserId,
                CreationDate = coffeeDrinks.CreationDate,
                Img = coffeeDrinks.DrinkImg,

            };
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CoffeeDrinksViewModel coffeeDrinks)
        {
            if (id != coffeeDrinks.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imgName;
                    if (coffeeDrinks.DrinkImg == null)
                    {
                        var x = _context.restaurants.Find(coffeeDrinks.ID);
                        imgName = x!.RestImage!;
                    }
                    else
                    {
                        var image = FileUpload(coffeeDrinks);
                        CoffeeDrinks drinks=new CoffeeDrinks
                    {
                        DrinkDescription = coffeeDrinks.DrinkDescription,
                        DrinkName = coffeeDrinks.DrinkName,
                        ID = coffeeDrinks.ID,
                        isDeleted = coffeeDrinks.isDeleted,
                        isPublished = coffeeDrinks.isPublished,
                        Price = coffeeDrinks.Price,
                        PriceUnit = coffeeDrinks.PriceUnit,
                        UserId = coffeeDrinks.UserId,
                        CreationDate = coffeeDrinks.CreationDate,
                        DrinkImg = image,

                    };

                        _context.Update(drinks);
                    await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoffeeDrinksExists(coffeeDrinks.ID))
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
            return View(coffeeDrinks);
        }

        // GET: Admin/CoffeeDrinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffeeDrinks = await _context.coffeeDrinks
                .FirstOrDefaultAsync(m => m.ID == id);
            if (coffeeDrinks == null)
            {
                return NotFound();
            }

            return View(coffeeDrinks);
        }

        // POST: Admin/CoffeeDrinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coffeeDrinks = await _context.coffeeDrinks.FindAsync(id);
            if (coffeeDrinks != null)
            {
                _context.coffeeDrinks.Remove(coffeeDrinks);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoffeeDrinksExists(int id)
        {
            return _context.coffeeDrinks.Any(e => e.ID == id);
        }

        public async Task<IActionResult> AllCoffeeDrinks()
        {

            return View(await _context.coffeeDrinks.ToListAsync());
        }

        public async Task<IActionResult> SoftDelete(int id)
        {
            var data = _context.coffeeDrinks.Find(id);
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
                    if (!CoffeeDrinksExists(data.ID))
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
            return View(await _context.coffeeDrinks.Where(x => x.isDeleted == true).ToListAsync());

        }

        public IActionResult Remove(int id)
        {
            var res = _context.coffeeDrinks.Find(id);
            res!.isDeleted = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Abroved()
        {
            return View(await _context.coffeeDrinks.Where(x => x.isPublished == false).ToListAsync());
        }

        public IActionResult Published(int id)
        {


            var data = _context.coffeeDrinks.Find(id);
            data!.isPublished = true;
            _context.SaveChanges();
            return RedirectToAction("Index");



        }

        public IActionResult UnPublished(int id)
        {
            var data = _context.coffeeDrinks.Find(id);
            data!.isPublished = false;
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public string FileUpload(CoffeeDrinksViewModel model)
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
            string FillName = Path.GetFileNameWithoutExtension(model.DrinkImg!.FileName);
            string newImgName = "Nextwo_" + FillName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.DrinkImg.FileName);

            using (FileStream fileStream = new FileStream(Path.Combine(p, newImgName), FileMode.Create))
            {
                model.DrinkImg.CopyTo(fileStream);
            }

            return "\\Images\\" + newImgName;
        }

    }
}
