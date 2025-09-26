using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stomachco.Models;
using Stomachco.Models.ViewModel;

namespace Stomachco.Data
{
    public class StomDbContext:IdentityDbContext
    {

        public StomDbContext(DbContextOptions<StomDbContext>options):base(options)
        {
            
        }
        public DbSet<Coffee> coffees { get; set; }
        public DbSet<CoffeeDrinks> coffeeDrinks { get; set; }
        public DbSet<Restaurant> restaurants { get; set; }
        public DbSet<RestFood> restFoods { get; set; }
        public DbSet<SuperMarket> superMarkets { get; set; }
        public DbSet<SMFood> sMFoods { get; set; }
        public DbSet<FeedBack> feedBacks  { get; set; }

      










    }
}
