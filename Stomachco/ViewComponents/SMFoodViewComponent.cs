using Microsoft.AspNetCore.Mvc;
using Stomachco.Data;

namespace Stomachco.ViewComponents
{
    public class SMFoodViewComponent:ViewComponent
    {

        private StomDbContext _context;

        public SMFoodViewComponent(StomDbContext context)
        {

            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result = _context.sMFoods.Where(x => x.isDeleted == false && x.isPublished == true).OrderByDescending(x => x.CreationDate);

            return View(result);
        }
    }
}
