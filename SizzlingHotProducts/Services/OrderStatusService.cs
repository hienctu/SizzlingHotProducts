using SizzlingHotProducts.Models;
using SizzlingHotProducts.Constants;

namespace SizzlingHotProducts.Services
{
    public class OrderStatusService
    {
        public bool IsCancelledOrder(List<Sale> sales, string orderId)
        {
            return sales.Any(sale => sale.Status == OrderStatus.Cancelled && sale.OrderId == orderId);
        }
    }
}
