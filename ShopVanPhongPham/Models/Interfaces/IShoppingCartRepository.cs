namespace ShopVanPhongPham.Models.Interfaces
{
    public interface IShoppingCartRepository
    {
        void AddToCart(int productId);
        void RemoveFromCart(int id);
        List<ShoppingCartItem> GetCartItems();
        int GetCartCount();
        void ClearCart();
    }

}
