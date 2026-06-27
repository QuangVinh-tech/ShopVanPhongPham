namespace ShopVanPhongPham.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }
        public string? Status { get; set; }

        // Đổi List<OrderDetail>? → List<OrderDetail> với default = []
        // → hết warning CS8620 trong ThenInclude
        public List<OrderDetail> OrderDetails { get; set; } = new();
        public string PaymentMethod { get; set; } = "QR";        // "COD" hoặc "QR"
        public string PaymentStatus { get; set; } = "Chưa thanh toán"; // hoặc "Đã thanh toán"
    }
}
