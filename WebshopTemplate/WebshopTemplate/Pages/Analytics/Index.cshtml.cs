namespace WebshopTemplate.Pages.Analytics
{
    [Authorize(Roles = "Manager,Admin")]
    public class IndexModel : PageModel
    {
        private readonly IAnalyticsService _analyticsService;

        public decimal TotalSales { get; set; }
        public List<ProductSalesDTO> TopSellingProducts { get; set; }

        public IndexModel(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public async Task OnGetAsync()
        {
            TotalSales = await _analyticsService.GetTotalSalesAsync(DateTime.Today);
            TopSellingProducts = await _analyticsService.GetTopSellingProductsAsync(DateTime.Today.AddDays(-30), DateTime.Today);
        }
    }
}
