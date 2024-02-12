using SizzlingHotProducts.Models;

namespace SizzlingHotProducts.Interfaces
{
    public interface ISalesConsolidateService
    {
        List<Sale> ConsolidateSales(List<Sale> sales);
    }
}
