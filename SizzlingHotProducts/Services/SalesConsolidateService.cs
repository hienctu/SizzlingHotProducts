using SizzlingHotProducts.Constants;
using SizzlingHotProducts.Interfaces;
using SizzlingHotProducts.Models;

namespace SizzlingHotProducts.Services
{
    public class SalesConsolidateService : ISalesConsolidateService
    {
        private readonly OrderStatusService _orderStatusService;

        public SalesConsolidateService(OrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }
        public List<Sale> ConsolidateSales(List<Sale> sales)
        {
            // Filter out cancelled orders and their corresponding completed orders
            var validSales = sales
                .Where(sale => sale.Status == OrderStatus.Completed && !_orderStatusService.IsCancelledOrder(sales, sale.OrderId))
                .ToList();

            // Group valid sales by customer, date, and product ID
            var groupedSales = validSales
                .GroupBy(sale => new { sale.CustomerId, sale.Date })
                .Select(group => new Sale
                {
                    OrderId = string.Join(",", group.Select(s => s.OrderId)), // Concatenate order IDs
                    CustomerId = group.Key.CustomerId,
                    Entries = group.SelectMany(s => s.Entries)
                                   .GroupBy(entry => entry.Id)
                                   .Select(entryGroup => new SaleEntry
                                   {
                                       Id = entryGroup.Key,
                                       Quantity = entryGroup.Sum(e => e.Quantity)
                                   })
                                   .ToList(),
                    Date = group.Key.Date,
                    Status = "consolidated"
                })
                .ToList();

            return groupedSales;
        }
    }
}
