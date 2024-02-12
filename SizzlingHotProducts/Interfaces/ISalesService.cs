using SizzlingHotProducts.Models;

namespace SizzlingHotProducts.Interfaces
{
    public interface ISalesService
    {
        List<Sale> ProcessSales(List<Sale> sales);
        TopProductInfo GetTopSellingProduct(List<Sale> consolidatedSales, List<Product> products);
        List<TopProductInfo> GetTopSellingProductsLast3Days(List<Sale> consolidatedSales, DateTime fromDate, List<Product> products);
    }
}
