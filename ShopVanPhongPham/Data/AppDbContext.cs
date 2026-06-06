using Microsoft.EntityFrameworkCore;
using ShopVanPhongPham.Models;

namespace ShopVanPhongPham.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }        
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
           new Product
           {
               Id = 1,
               Name = "Bút bi Thiên Long",
               Price = 5000,
               ImageUrl = "https://placehold.co/300x300?text=But+Bi",
               Category = "But"
           },
new Product
{
    Id = 2,
    Name = "Bút chì 2B",
    Price = 4000,
    ImageUrl = "https://placehold.co/300x300?text=But+Chi",
    Category = "But"
},
new Product
{
    Id = 3,
    Name = "Sổ tay A5",
    Price = 35000,
    ImageUrl = "https://placehold.co/300x300?text=So+Tay",
    Category = "So"
},
new Product
{
    Id = 4,
    Name = "Tập 200 trang",
    Price = 15000,
    ImageUrl = "https://placehold.co/300x300?text=Tap+200",
    Category = "So"
},
new Product
{
    Id = 5,
    Name = "Thước kẻ 30cm",
    Price = 8000,
    ImageUrl = "https://placehold.co/300x300?text=Thuoc+Ke",
    Category = "DungCu"
},
new Product
{
    Id = 6,
    Name = "Kéo văn phòng",
    Price = 25000,
    ImageUrl = "https://placehold.co/300x300?text=Keo",
    Category = "DungCu"
},
new Product
{
    Id = 7,
    Name = "Bấm kim",
    Price = 30000,
    ImageUrl = "https://placehold.co/300x300?text=Bam+Kim",
    Category = "DungCu"
},
new Product
{
    Id = 8,
    Name = "Giấy A4 500 tờ",
    Price = 85000,
    ImageUrl = "https://placehold.co/300x300?text=Giay+A4",
    Category = "Giay"
},
new Product
{
    Id = 9,
    Name = "Băng keo trong",
    Price = 10000,
    ImageUrl = "https://placehold.co/300x300?text=Bang+Keo",
    Category = "DungCu"
},
new Product
{
    Id = 10,
    Name = "Hộp bút để bàn",
    Price = 45000,
    ImageUrl = "https://placehold.co/300x300?text=Hop+But",
    Category = "DungCu"
}
            );
        }
    }
}
