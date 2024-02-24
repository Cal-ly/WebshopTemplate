namespace WebshopTemplate.Interfaces;

public interface IBasketService
{
    Task<Basket> GetBasketAsync();
    Task AddBasketItemAsync(string productId, int quantity);
    Task RemoveBasketItemAsync(string productId);
    Task TransferSessionBasketToUserAsync(string userId);
    Task ClearBasketAsync();
}