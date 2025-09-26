using Microsoft.AspNetCore.Mvc;

using Stomachco.Data;

namespace Stomachco.ViewComponents
{
    public class RestFoodViewComponent:ViewComponent
    {
      
            private StomDbContext _context;

            public RestFoodViewComponent(StomDbContext context)
            {

                _context = context;
            }

            public IViewComponentResult Invoke()
            {
                var result = _context.restFoods.Where(x => x.isDeleted == false && x.isPublished == true).OrderByDescending(x => x.CreationDate);
            return View(result);
            }

        
    }
}
