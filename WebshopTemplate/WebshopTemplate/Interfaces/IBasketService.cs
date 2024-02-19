namespace WebshopTemplate.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync(string basketId);
        Task AddBasketItemAsync(string basketId, string productId, int quantity);
        Task RemoveBasketItemAsync(string basketId, string productId);
        void TransferSessionBasketToUser(string userId, string sessionBasketId);
        void TransferUserBasketToSession(string userId, string sessionBasketId);
    }
}
