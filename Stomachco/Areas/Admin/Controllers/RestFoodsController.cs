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

    public class RestFoodsController : Controller
    {
        private readonly StomDbContext _context;
        private IWebHostEnvironment _environment;

        public RestFoodsController(StomDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/RestFoods
        public async Task<IActionResult> Index()
        {
            return View(await _context.restFoods.Where(x=>x.isPublished==true &&x.isDeleted==false).ToListAsync());
        }

        // GET: Admin/RestFoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restFood = await _context.restFoods
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restFood == null)
            {
                return NotFound();
            }

            return View(restFood);
        }

        // GET: Admin/RestFoods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/RestFoods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( RestFoodsViewModel model)
        {
            if (ModelState.IsValid)
            {

                var image=FileUpload(model);
                RestFood food = new RestFood
                {
                    CreationDate = model.CreationDate,
                    ID = model.ID,
                    isDeleted = model.isDeleted,
                    FoodName = model.FoodName,
                    isPublished = model.isPublished,
                    Price = model.Price,
                    PriceUnit = model.PriceUnit,
                    UserId = model.UserId,
                    FoodDescription = model.FoodDescription,
                    FoodImg = image

                };



                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/RestFoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.restFoods.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            var food = new RestFoodsViewModel
            {
                CreationDate = model.CreationDate,
                ID = model.ID,
                isDeleted = model.isDeleted,
                FoodName = model.FoodName,
                isPublished = model.isPublished,
                Price = model.Price,
                PriceUnit = model.PriceUnit,
                UserId = model.UserId,
                FoodDescription = model.FoodDescription,
                Img = model.FoodImg

            };

            return View(food);
        }

        // POST: Admin/RestFoods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,RestFoodsViewModel model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var image = FileUpload(model);
                    RestFood food = new RestFood
                    {
                        CreationDate = model.CreationDate,
                        ID = model.ID,
                        isDeleted = model.isDeleted,
                        FoodName = model.FoodName,
                        isPublished = model.isPublished,
                        Price = model.Price,
                        PriceUnit = model.PriceUnit,
                        UserId = model.UserId,
                        FoodDescription = model.FoodDescription,
                        FoodImg = image

                    };
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestFoodExists(model.ID))
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

        // GET: Admin/RestFoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restFood = await _context.restFoods
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restFood == null)
            {
                return NotFound();
            }

            return View(restFood);
        }

        // POST: Admin/RestFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restFood = await _context.restFoods.FindAsync(id);
            if (restFood != null)
            {
                _context.restFoods.Remove(restFood);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestFoodExists(int id)
        {
            return _context.restFoods.Any(e => e.ID == id);
        }
        public async Task<IActionResult> AllRestFood()
        {

            return View(await _context.restFoods.ToListAsync());
        }

        public async Task<IActionResult> SoftDelete(int id)
        {
            var data = _context.restFoods.Find(id);
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
                    if (!RestFoodExists(data.ID))
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
            return View(await _context.restFoods.Where(x => x.isDeleted == true).ToListAsync());

        }

        public IActionResult Remove(int id)
        {
            var res = _context.restFoods.Find(id);
            res!.isDeleted = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Abroved()
        {
            return View(await _context.restFoods.Where(x => x.isPublished == false).ToListAsync());
        }

        public IActionResult Published(int id)
        {


            var data = _context.restFoods.Find(id);
            data!.isPublished = true;
            _context.SaveChanges();
            return RedirectToAction("Index");



        }

        public IActionResult UnPublished(int id)
        {
            var data = _context.restFoods.Find(id);
            data!.isPublished = false;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public string FileUpload(RestFoodsViewModel model)
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
            string FillName = Path.GetFileNameWithoutExtension(model.FoodImg!.FileName);
            string newImgName = "Nextwo_" + FillName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.FoodImg.FileName);

            using (FileStream fileStream = new FileStream(Path.Combine(p, newImgName), FileMode.Create))
            {
                model.FoodImg.CopyTo(fileStream);
            }

            return "\\Images\\" + newImgName;
        }
    }
}
