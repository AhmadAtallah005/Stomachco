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

    public class SMFoodsController : Controller
    {
        private readonly StomDbContext _context; private IWebHostEnvironment _environment;


        public SMFoodsController(StomDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/SMFoods
        public async Task<IActionResult> Index()
        {
            return View(await _context.sMFoods.Where(x=>x.isPublished==true&&x.isDeleted==false).ToListAsync());
        }

        // GET: Admin/SMFoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sMFood = await _context.sMFoods
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sMFood == null)
            {
                return NotFound();
            }

            return View(sMFood);
        }

        // GET: Admin/SMFoods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SMFoods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( SMFoodsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var image = FileUpload(model);
                SMFood food = new SMFood { 
                CreationDate=model.CreationDate,
                isDeleted=model.isDeleted,
                FoodImg=image,
                isPublished=model.isPublished,
                ID = model.ID,
                Price = model.Price,
                PriceUnit=model.PriceUnit,
                SMItem=model.SMItem,
                UserId = model.UserId
                
                };




                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/SMFoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.sMFoods.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            var food = new SMFoodsViewModel
            {
                CreationDate = model.CreationDate,
                isDeleted = model.isDeleted,
                Img = model.FoodImg,
                isPublished = model.isPublished,
                ID = model.ID,
                Price = model.Price,
                PriceUnit = model.PriceUnit,
                SMItem = model.SMItem,
                UserId = model.UserId

            };
            return View(food);
        }

        // POST: Admin/SMFoods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  SMFoodsViewModel model)
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
                    SMFood food = new SMFood
                    {
                        CreationDate = model.CreationDate,
                        isDeleted = model.isDeleted,
                        FoodImg = image,
                        isPublished = model.isPublished,
                        ID = model.ID,
                        Price = model.Price,
                        PriceUnit = model.PriceUnit,
                        SMItem = model.SMItem,
                        UserId = model.UserId

                    };
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SMFoodExists(model.ID))
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

        // GET: Admin/SMFoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sMFood = await _context.sMFoods
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sMFood == null)
            {
                return NotFound();
            }

            return View(sMFood);
        }

        // POST: Admin/SMFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sMFood = await _context.sMFoods.FindAsync(id);
            if (sMFood != null)
            {
                _context.sMFoods.Remove(sMFood);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SMFoodExists(int id)
        {
            return _context.sMFoods.Any(e => e.ID == id);
        }

        public async Task<IActionResult> AllSuperFood()
        {

            return View(await _context.sMFoods.ToListAsync());
        }

        public async Task<IActionResult> SoftDelete(int id)
        {
            var data = _context.sMFoods.Find(id);
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
                    if (!SMFoodExists(data.ID))
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
            return View(await _context.sMFoods.Where(x => x.isDeleted == true).ToListAsync());

        }

        public IActionResult Remove(int id)
        {
            var res = _context.sMFoods.Find(id);
            res!.isDeleted = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Abroved()
        {
            return View(await _context.sMFoods.Where(x => x.isPublished == false).ToListAsync());
        }

        public IActionResult Published(int id)
        {


            var data = _context.sMFoods.Find(id);
            data!.isPublished = true;
            _context.SaveChanges();
            return RedirectToAction("Index");



        }

        public IActionResult UnPublished(int id)
        {
            var data = _context.sMFoods.Find(id);
            data!.isPublished = false;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public string FileUpload(SMFoodsViewModel model)
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
