using Microsoft.AspNetCore.Mvc;
using Stomachco.Data;

namespace Stomachco.ViewComponents
{
    public class SuperMarketViewComponent : ViewComponent
    {
        private StomDbContext _context;

        public SuperMarketViewComponent(StomDbContext context)
        {

            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result = _context.superMarkets.Where(x => x.isDeleted == false && x.isPublished == true).OrderByDescending(x => x.CreationDate).Take(6);

            return View(result);
        }




    }
}
