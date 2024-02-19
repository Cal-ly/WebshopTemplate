namespace WebshopTemplate.Services
{
    public class BasketService : IBasketService
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketService(IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;
        private string GetBasketId() => Session.GetString("BasketId") ?? Guid.NewGuid().ToString();

        public async Task<Basket> GetBasketAsync()
        {
            var basketId = GetBasketId();
            var basket = Session.GetBasket(basketId);
            if (basket == null)
            {
                basket = new Basket { Id = basketId };
                Session.SetBasket(basketId, basket);
            }
            return await Task.FromResult(basket);
        }

        public async Task AddBasketItemAsync(string productId, int quantity)
        {
            var basket = await GetBasketAsync();
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null) throw new InvalidOperationException("Product not found");

            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem != null)
            {
                basketItem.Quantity += quantity;
            }
            else
            {
                basket.Items.Add(new BasketItem { ProductId = productId, Quantity = quantity, ProductInBasket = product });
            }
            Session.SetBasket(basket.Id, basket);
        }

        public async Task RemoveBasketItemAsync(string productId)
        {
            var basket = await GetBasketAsync();
            var basketItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (basketItem != null)
            {
                if (basketItem.Quantity > 1)
                {
                    --basketItem.Quantity;
                }
                else
                {
                    basket.Items.Remove(basketItem);
                }
                Session.SetBasket(basket.Id, basket);
            }
        }

        public async Task TransferSessionBasketToUserAsync(string userId)
        {
            var basket = await GetBasketAsync();
            if (basket.CustomerId == null || basket.CustomerId == "Session Guest")
            {
                basket.CustomerId = userId;
                Session.SetBasket(basket.Id, basket);
            }
        }

        public async Task CreateBasketForSessionGuestAsync()
        {
            var basket = await GetBasketAsync();
            if (basket.CustomerId == null)
            {
                basket.CustomerId = "Session Guest";
                Session.SetBasket(basket.Id, basket);
            }
        }

        //public async Task CreateOrderFromBasket()
        //{
        //    var basket = await GetBasketAsync();
        //    var order = new Order
        //    {
        //        CustomerId = basket.CustomerId,
        //        OrderDate = DateTime.UtcNow,
        //        Status = OrderStatus.Submitted, // Set initial order status
        //        OrderDetails = new List<OrderDetail>(),
        //    };
        //
        //    foreach (var item in basket.Items)
        //    {
        //        order.OrderDetails.Add(new OrderDetail
        //        {
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity,
        //            Price = item.ProductInBasket.Price,
        //        });
        //    }
        //
        //    // Calculate the total price of the order if needed or perform additional processing
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();
        //}

        public async Task ClearBasketAsync()
        {
            var basketId = GetBasketId();
            Session.Remove(basketId);
            await Task.CompletedTask;
        }
    }
}