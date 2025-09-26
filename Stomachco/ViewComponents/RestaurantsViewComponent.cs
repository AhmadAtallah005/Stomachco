using Microsoft.AspNetCore.Mvc;
using Stomachco.Data;

namespace Stomachco.ViewComponents
{
    public class RestaurantsViewComponent : ViewComponent
    {
        private StomDbContext _context;

        public RestaurantsViewComponent(StomDbContext context)
        {
            
             _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result=_context.restaurants.Where(x=>x.isDeleted==false && x.isPublished==true).OrderByDescending(x=>x.CreationDate);

            return View(result);
        }




    }
}
