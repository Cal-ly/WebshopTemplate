﻿using Microsoft.EntityFrameworkCore;
using WebshopTemplate.DTO;

namespace WebshopTemplate.Interfaces
{
    public interface IAnalyticsService
    {
        public Task<decimal> GetTotalSalesAsync(DateTime date);
        public Task<decimal> GetTotalSalesFromToAsync(DateTime dateStart, DateTime dateEnd);
        public Task<List<ProductSalesDTO>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate);
    }
}
