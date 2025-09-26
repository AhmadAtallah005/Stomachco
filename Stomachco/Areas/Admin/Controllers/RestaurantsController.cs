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

    public class RestaurantsController : Controller
    {
        private readonly StomDbContext _context;
        private IWebHostEnvironment _environment;
        public RestaurantsController(StomDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/Restaurants
        public async Task<IActionResult> Index()
        {
            return View(await _context.restaurants.Where(x=>x.isPublished==true && x.isDeleted ==false).ToListAsync());
        }

        // GET: Admin/Restaurants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.restaurants
                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Admin/Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestaurantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Image = FileUpload(model);
                Restaurant rest = new Restaurant
                {
                    UserId = model.UserId,
                    RestImage = Image,
                    CreationDate = model.CreationDate,
                    isDeleted = model.isDeleted,
                    isPublished = model.isPublished,
                    RestaurantId = Guid.NewGuid(),
                    RestDesciption = model.RestDesciption,
                    RestName = model.RestName

                };
                _context.restaurants.Add(rest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Restaurants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            
            var model= new EditRestViewModel
            {
                UserId = restaurant.UserId,
                Image = restaurant.RestImage,
                CreationDate = restaurant.CreationDate,
                isDeleted = restaurant.isDeleted,
                isPublished = restaurant.isPublished,
                RestaurantId = restaurant.RestaurantId,
                RestDesciption = restaurant.RestDesciption,
                RestName = restaurant.RestName

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,  EditRestViewModel model)
        {

            if (id != model.RestaurantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imgName;
                    if (model.RestImage == null)
                    {
                        var x = _context.restaurants.Find(model.RestaurantId);
                        imgName = x!.RestImage!;
                    }
                    else
                    {
                        var Image = FileUpload(model);
                        Restaurant rest = new Restaurant
                        {
                            UserId = model.UserId,
                            RestImage = Image,
                            CreationDate = model.CreationDate,
                            isDeleted = model.isDeleted,
                            isPublished = model.isPublished,
                            RestaurantId = model.RestaurantId,
                            RestDesciption = model.RestDesciption,
                            RestName = model.RestName

                        };
                        _context.Update(rest);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(model.RestaurantId))
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

        // GET: Admin/Restaurants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.restaurants
                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Admin/Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var restaurant = await _context.restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.restaurants.Remove(restaurant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(Guid id)
        {
            return _context.restaurants.Any(e => e.RestaurantId == id);
        }

        public string FileUpload(RestaurantViewModel model)
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
            string FillName = Path.GetFileNameWithoutExtension(model.RestImage!.FileName);
            string newImgName = "Nextwo_" + FillName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.RestImage.FileName);

            using (FileStream fileStream = new FileStream(Path.Combine(p, newImgName), FileMode.Create))
            {
                model.RestImage.CopyTo(fileStream);
            }

            return "\\Images\\" + newImgName;
        }
        public string FileUpload(EditRestViewModel model)
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
            string FillName = Path.GetFileNameWithoutExtension(model.RestImage!.FileName);
            string newImgName = "Nextwo_" + FillName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.RestImage.FileName);

            using (FileStream fileStream = new FileStream(Path.Combine(p, newImgName), FileMode.Create))
            {
                model.RestImage.CopyTo(fileStream);
            }

            return "\\Images\\" + newImgName;
        }


        public async Task<IActionResult> AllRestaurnts()
        {

            return View(await _context.restaurants.ToListAsync());
        }

        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var data = _context.restaurants.Find(id);
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
                    if (!RestaurantExists(data.RestaurantId))
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
            return View(await _context.restaurants.Where(x => x.isDeleted == true).ToListAsync());

        }

        public IActionResult Remove(Guid id)
        {
            var res = _context.restaurants.Find(id);
            res!.isDeleted = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Abroved()
        { 
        return View(await _context.restaurants.Where(x=>x.isPublished==false).ToListAsync());
        }

        public IActionResult Published(Guid id) 
        {
            

            var data = _context.restaurants.Find(id);
            data!.isPublished = true;
            _context.SaveChanges();
            return RedirectToAction("Index");



        }

        public IActionResult UnPublished(Guid id)
        {
            var data = _context.restaurants.Find(id);
            data!.isPublished = false;
            _context.SaveChanges();
            return RedirectToAction("index");
        }

    }    

}
