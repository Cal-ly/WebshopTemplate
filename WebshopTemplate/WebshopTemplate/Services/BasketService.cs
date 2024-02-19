namespace WebshopTemplate.Services
{
    public class BasketService : IBasketService
    {
        private readonly IProductService IProductService;
        private readonly HttpContext httpContext;
        public string BasketId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; } = "Session Guest";
        public string SessionBasketId { get; set; } = "Session Guest";

        public BasketService(IProductService iproductservice, string basketId, string productId, int quantity, HttpContext httpContext)
        {
            IProductService = iproductservice;
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
            this.httpContext = httpContext;
        }

        public Task<Basket> GetBasketAsync(string basketId)
        {
            var basket = httpContext.Session.GetBasket(basketId);
            if (basket == null)
            {
                basket = new Basket { Id = basketId, CustomerId = UserId };
                httpContext.Session.SetBasket(basketId, basket);
            }
            return Task.FromResult(basket);
        }

        public async Task AddBasketItemAsync(string basketId, string productId, int quantity)
        {
            var basket = await GetBasketAsync(basketId);
            var product = await IProductService.GetProductByIdAsync(productId);
            var basketItem = new BasketItem { BasketId = basketId, ProductId = productId, Quantity = quantity, ProductInBasket = product };

            if (basket != null && product != null)
            {
                basket.Items.Add(basketItem);
                httpContext.Session.SetBasket(basketId, basket);
            }
            else
            {
                throw new Exception("Basket or product not found");
            }
            await httpContext.Session.CommitAsync();
        }

        public async Task RemoveBasketItemAsync(string basketId, string productId)
        {
            var basket = await GetBasketAsync(basketId);
            var basketItem = basket.Items.FirstOrDefault(item => item.ProductId == productId);

            if (basketItem != null)
            {
                basketItem.Quantity--;

                if (basketItem.Quantity <= 0)
                {
                    basket.Items.Remove(basketItem);
                }

                httpContext.Session.SetBasket(basketId, basket);
            }
        }

        public void TransferSessionBasketToUser(string userId, string sessionBasketId)
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
        public void TransferUserBasketToSession(string userId, string sessionBasketId)
        {
            // Get the user basket
            var userBasket = httpContext.Session.GetBasket(userId);

            if (userBasket != null)
            {
                // Set the basket for the session
                httpContext.Session.SetBasket(sessionBasketId, userBasket);

                // Remove the user basket
                httpContext.Session.Remove(userId);
            }
        }
    }
}
