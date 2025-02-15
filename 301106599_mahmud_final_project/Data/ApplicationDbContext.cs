using _301106599_mahmud_final_project.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _301106599_mahmud_final_project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
               .Property(j => j.CategoryId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Location>()
               .Property(j => j.LocationId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>()
               .Property(j => j.ProductId)
            .ValueGeneratedOnAdd();

            //modelBuilder.Entity<JobApplication>()
            //        .HasOne(a => a.Product)
            //        .WithMany(j => j.JobApplications)
            //        .HasForeignKey(a => a.JobId);
  
            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    LocationId = 1,
                    Name = "Toronto"
                },
                new Location
                {
                    LocationId = 2,
                    Name = "Mississauga"
                },
                new Location
                {
                    LocationId = 3,
                    Name = "Brampton"
                },
                new Location
                {
                    LocationId = 4,
                    Name = "Scarborough"
                }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Clothing"
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "Books"
                },
                new Category
                {
                    CategoryId = 4,
                    Name = "Furniture"
                },
                new Category
                {
                    CategoryId = 5,
                    Name = "Appliances"
                }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Dell XPS",
                    SerialNo = "123456",
                    Date = new DateOnly(2021, 1, 1),
                    Price = 1000,
                    Quantity = 10,
                    CategoryId = 1,
                    LocationId = 1,
                    Description = "Dell Laptop",
                    ImageUrl = "https://www.dell.com/en-ca/shop/laptops-ultrabooks/xps-15-laptop/spd/xps-15-9530-laptop/caexcpbts9530gvhd"
                },
                new Product
                {
                    ProductId = 2,
                    Name = "T-Shirt",
                    SerialNo = "123457",
                    Date = new DateOnly(2021, 1, 1),
                    Price = 20,
                    Quantity = 100,
                    CategoryId = 2,
                    LocationId = 2,
                    Description = "Nike T-Shirt",
                    ImageUrl = "https://www.jiffy.com/ca/mo-4800J1.html?ac=Sapphire&utm_adgroup=163642421436&utm_term=&campcat=&target=pla-2313723458585&physical=9001383&matchtype=&adtype=pla&jid=JS999985&pgid=2313723458585&net=g&m=&search=[true]&creative=702815230789&where=&country=CA&bookmark=8585128489953639874&gad_source=1"
                }
                );

                }
    }
}
