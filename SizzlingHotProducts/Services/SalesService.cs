using SizzlingHotProducts.Interfaces;
using SizzlingHotProducts.Models;
using System.Globalization;

namespace SizzlingHotProducts.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISalesConsolidateService _salesConsolidateService;

        public SalesService(ISalesConsolidateService salesConsolidateService)
        {
            _salesConsolidateService = salesConsolidateService;
        }

        public List<Sale> ProcessSales(List<Sale> sales)
        {
            return _salesConsolidateService.ConsolidateSales(sales);
        }

        public TopProductInfo GetTopSellingProduct(List<Sale> consolidatedSales, List<Product> products)
        {
           
            var allEntries = consolidatedSales.SelectMany(sale => sale.Entries);

            // Group by product ID and sum the quantities
            var topProduct = allEntries
                .GroupBy(entry => entry.Id)
                .Select(group => new TopProductInfo { 
                    ProductId = group.Key,
                    ProductName = products.FirstOrDefault(p => p.Id == group.Key)?.Name,
                    TotalQuantity = group.Sum(entry => entry.Quantity) 
                })
                .OrderByDescending(x => x.TotalQuantity)
                .FirstOrDefault();

            return topProduct;
        }

        public List<TopProductInfo> GetTopSellingProductsLast3Days(List<Sale> consolidatedSales, DateTime fromDate, List<Product> products)
        {

            // Filter sales for the last 3 days
            var salesLast3Days = consolidatedSales
                .Where(sale => DateTime.ParseExact(sale.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= fromDate.AddDays(-3))
                .ToList();

            var allEntries = salesLast3Days
                             .SelectMany(sale => sale.Entries.Select(entry => new
                             {
                                 SaleDate = sale.Date,
                                 entry.Id,
                                 entry.Quantity
                             }))
                             .ToList();

            // Calculate total quantity sold for each product each day
            var dailyTotals = allEntries
                .GroupBy(entry => new { entry.Id, entry.SaleDate })
                .Select(group => new
                {
                    ProductId = group.Key.Id,
                    SaleDate = group.Key.SaleDate,
                    TotalQuantity = group.Sum(entry => entry.Quantity)
                })
                .ToList();

            // Find the most sold product each day, considering alphabetical order product name for ties
            var topProducts = dailyTotals
                    .GroupBy(total => total.SaleDate)
                    .SelectMany(group =>
                    {
                        var maxQuantity = group.Max(total => total.TotalQuantity);
                        return group
                            .Where(total => total.TotalQuantity == maxQuantity)
                            .OrderBy(total => products.FirstOrDefault(p => p.Id == total.ProductId)?.Name);
                    })
                    .Select(top => new TopProductInfo
                    {
                        ProductId = top.ProductId,
                        ProductName = products.FirstOrDefault(p => p.Id == top.ProductId)?.Name,
                        TotalQuantity = top.TotalQuantity,
                        Date = top.SaleDate
                    })
                    .ToList();

            return topProducts;
        }
    }
}
