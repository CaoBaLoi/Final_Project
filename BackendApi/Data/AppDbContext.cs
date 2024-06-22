using Househole_shop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Househole_shop.Data{
    public class AppDbContext : IdentityDbContext<User>{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {

            }
    }
}