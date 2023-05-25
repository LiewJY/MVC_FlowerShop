using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_FlowerShop.Areas.Identity.Data;

namespace MVC_FlowerShop.Data;

public class MVC_FlowerShopContext : IdentityDbContext<MVC_FlowerShopUser>
{
    public MVC_FlowerShopContext(DbContextOptions<MVC_FlowerShopContext> options)
        : base(options)
    {
    }

    public DbSet<MVC_FlowerShop.Models.Flower> FlowerTable { get; set; }






    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
