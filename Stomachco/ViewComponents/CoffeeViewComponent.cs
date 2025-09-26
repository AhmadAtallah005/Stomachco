using Microsoft.AspNetCore.Mvc;
using Stomachco.Data;

namespace Stomachco.ViewComponents
{
    public class CoffeeViewComponent : ViewComponent
    {



        private StomDbContext _context;

        public CoffeeViewComponent(StomDbContext context)
        {

            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result = _context.coffees.Where(x => x.isDeleted == false && x.isPublished == true).OrderByDescending(x => x.CreationDate).Take(6);

            return View(result);
        }

    } 
}
