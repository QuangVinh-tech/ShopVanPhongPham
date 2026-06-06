using Microsoft.EntityFrameworkCore;
using ShopVanPhongPham.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ShopVanPhongPham.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }        
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
           new Product
           {
               Id = 1,
               Name = "Bút bi Thiên Long",
               Price = 5000,
               ImageUrl = "/assets/images/butthienlong.jpg",
               Category = "But"
           },
            new Product
            {
                Id = 2,
                Name = "Bút chì 2B",
                Price = 4000,
                ImageUrl = "/assets/images/butchib2.jpg",
                Category = "But"
            },
            new Product
            {
                Id = 3,
                Name = "Sổ tay A5",
                Price = 35000,
                ImageUrl = "/assets/images/sotay.jpg",
                Category = "So"
            },
            new Product
            {
                Id = 4,
                Name = "Tập 200 trang",
                Price = 15000,
                ImageUrl = "/assets/images/tap200trang.jpg",
                Category = "So"
            },
            new Product
            {
                Id = 5,
                Name = "Thước kẻ 30cm",
                Price = 8000,
                ImageUrl = "/assets/images/thuocke30cm.jpg",
                Category = "DungCu"
            },
            new Product
            {
                Id = 6,
                Name = "Kéo văn phòng",
                Price = 25000,
                ImageUrl = "/assets/images/keovanphong1.jpg",
                Category = "DungCu"
            },
            new Product
            {
                Id = 7,
                Name = "Bấm kim",
                Price = 30000,
                ImageUrl = "/assets/images/bamkim.jpg",
                Category = "DungCu"
            },
            new Product
            {
                Id = 8,
                Name = "Giấy A4 500 tờ",
                Price = 85000,
                ImageUrl = "/assets/images/giayA4.jpg",
                Category = "Giay"
            },
            new Product
            {
                Id = 9,
                Name = "Băng keo trong",
                Price = 10000,
                ImageUrl = "/assets/images/bangkeo.png",
                Category = "DungCu"
            },
            new Product
            {
                Id = 10,
                Name = "Hộp bút để bàn",
                Price = 45000,
                ImageUrl = "/assets/images/hopbut.jpg",
                Category = "DungCu"
            }
            );
        }
    }
}
