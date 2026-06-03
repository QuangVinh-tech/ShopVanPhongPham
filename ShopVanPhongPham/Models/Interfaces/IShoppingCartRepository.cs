namespace ShopVanPhongPham.Models.Interfaces
{
    public interface IShoppingCartRepository
    {
        void AddToCart(int productId);
        void AddToCart(Product product, int quantity);   // ← THÊM MỚI
        void RemoveFromCart(int id);
        List<ShoppingCartItem> GetCartItems();
        decimal GetCartTotal();                          // ← THÊM MỚI
        int GetCartCount();
        void IncreaseQuantity(int productId);            // ← THÊM MỚI
        void DecreaseQuantity(int productId);            // ← THÊM MỚI
        void ClearCart();
    }

}
