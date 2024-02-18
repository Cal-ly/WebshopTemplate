namespace WebshopTemplate.Services
{
    public class BasketService : IBasketService
    {
        public string BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public string SessionBasketId { get; set; }

        public BasketService(string basketId, int productId, int quantity)
        {
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }

        public async Task AddItemAsync(string basketId, int productId, int quantity)
        {
            // Implement, so the enduser can add items to the basket by clicking the "Add to basket" button
        }

        public async Task RemoveItemAsync(string basketId, int productId)
        {
            // Implement, so the enduser can remove items from the basket by clicking the "Remove" button
        }

        public void TransferSessionBasketToUser(string userId, string sessionBasketId, HttpContext httpContext)
        {
            // Get the session basket
            var sessionBasket = httpContext.Session.GetBasket(sessionBasketId);

            if (sessionBasket != null)
            {
                // Set the basket for the user
                httpContext.Session.SetBasket(userId, sessionBasket);

                // Remove the session basket
                httpContext.Session.Remove(sessionBasketId);
            }
        }
    }
}
