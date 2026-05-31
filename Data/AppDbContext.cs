using Microsoft.EntityFrameworkCore;
using ShopVanPhongPham.Models;

namespace ShopVanPhongPham.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Bút bi Thiên Long", Description = "Bút bi xanh cao cấp", Price = 5000, ImageUrl = "/assets/images/p1.jpg", Category = "But" },
                new Product { Id = 2, Name = "Bút chì 2B", Description = "Bút chì vẽ kỹ thuật", Price = 4000, ImageUrl = "/assets/images/p2.jpg", Category = "But" },
                new Product { Id = 3, Name = "Sổ tay A5", Description = "Sổ tay bìa cứng 200 trang", Price = 35000, ImageUrl = "/assets/images/p3.jpg", Category = "So" },
                new Product { Id = 4, Name = "Tập 200 trang", Description = "Tập học sinh kẻ ngang", Price = 15000, ImageUrl = "/assets/images/p4.jpg", Category = "So" },
                new Product { Id = 5, Name = "Thước kẻ 30cm", Description = "Thước nhựa trong suốt", Price = 8000, ImageUrl = "/assets/images/p5.jpg", Category = "DungCu" },
                new Product { Id = 6, Name = "Kéo văn phòng", Description = "Kéo cắt giấy inox", Price = 25000, ImageUrl = "/assets/images/p6.jpg", Category = "DungCu" },
                new Product { Id = 7, Name = "Bấm kim", Description = "Bấm kim 24/6 tiêu chuẩn", Price = 30000, ImageUrl = "/assets/images/p7.jpg", Category = "DungCu" },
                new Product { Id = 8, Name = "Giấy A4 500 tờ", Description = "Giấy in laser trắng sáng", Price = 85000, ImageUrl = "/assets/images/p8.jpg", Category = "Giay" },
                new Product { Id = 9, Name = "Băng keo trong", Description = "Băng keo văn phòng 2cm", Price = 10000, ImageUrl = "/assets/images/p9.jpg", Category = "DungCu" },
                new Product { Id = 10, Name = "Hộp bút để bàn", Description = "Hộp đựng bút nhựa ABS", Price = 45000, ImageUrl = "/assets/images/p10.jpg", Category = "DungCu" }
            );
        }
    }
}
