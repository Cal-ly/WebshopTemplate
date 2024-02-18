namespace WebshopTemplate.Interfaces
{
    public interface IBasketService
    {
        Task AddItemAsync(string basketId, int productId, int quantity);
        Task RemoveItemAsync(string basketId, int productId);
        void TransferSessionBasketToUser(string userId, string sessionBasketId, HttpContext httpContext);
    }

}
