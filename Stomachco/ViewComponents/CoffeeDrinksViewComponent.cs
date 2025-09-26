using Microsoft.AspNetCore.Mvc;
using Stomachco.Data;

namespace Stomachco.ViewComponents
{
    public class CoffeeDrinksViewComponent:ViewComponent
    {


        private StomDbContext _context;

        public CoffeeDrinksViewComponent(StomDbContext context)
        {

            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result = _context.coffeeDrinks.Where(x => x.isDeleted == false && x.isPublished == true).OrderByDescending(x => x.UserId);

            return View(result);
        }

    }
}
