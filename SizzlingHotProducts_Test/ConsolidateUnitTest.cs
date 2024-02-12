using Xunit;
using SizzlingHotProducts.Services;
using SizzlingHotProducts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SizzlingHotProducts.Models;
using Assert = Xunit.Assert;

[TestClass]
public class ConsolidateUnitTest
{
    private readonly ISalesConsolidateService _salesConsolidateService;

    public ConsolidateUnitTest()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<OrderStatusService, OrderStatusService>()
            .AddScoped<ISalesConsolidateService, SalesConsolidateService>()
            .AddScoped<ISalesService, SalesService>()
            .BuildServiceProvider();

        _salesConsolidateService = serviceProvider.GetRequiredService<ISalesConsolidateService>();
    }


    [Fact]
    public void ConsolidateSales_Should_Consolidate_Sales()
    {
        // Arrange
        var sales = new List<Sale>
        {
            new Sale
            {
                OrderId = "O10",
                CustomerId = "C1",
                Entries = new List<SaleEntry>
                {
                    new SaleEntry { Id = "BBQ", Quantity = 2 },
                    new SaleEntry { Id = "Hammer", Quantity = 2 }
                },
                Date = "19/07/2021",
                Status = "completed"
            },
            new Sale
            {
                OrderId = "O11",
                CustomerId = "C1",
                Entries = new List<SaleEntry>
                {
                    new SaleEntry { Id = "BBQ", Quantity = 3 }
                },
                Date = "19/07/2021",
                Status = "completed"
            },
             new Sale
            {
                OrderId = "012",
                CustomerId = "C3",
                Entries = new List<SaleEntry>
                {
                    new SaleEntry { Id = "BBQ", Quantity = 2 },
                    new SaleEntry { Id = "Hammer", Quantity = 2 }
                },
                Date = "19/07/2021",
                Status = "completed"
            },
              new Sale
            {
                OrderId = "012",
                CustomerId = "C3",
                Date = "19/07/2021",
                Status = "cancelled"
            },
        };
   
        var consolidatedSales = _salesConsolidateService.ConsolidateSales(sales);

        // Assert
        Assert.NotNull(consolidatedSales);
        Assert.Equal(1, consolidatedSales.Count);

        var consolidatedSale = consolidatedSales[0];
        Assert.Equal("C1", consolidatedSale.CustomerId);
        Assert.Equal("19/07/2021", consolidatedSale.Date);
        Assert.Equal("O10,O11", consolidatedSale.OrderId);
        Assert.Equal("consolidated", consolidatedSale.Status);

        Assert.NotNull(consolidatedSale.Entries);
        Assert.Equal(2, consolidatedSale.Entries.Count);

        var bbqEntry = consolidatedSale.Entries.Find(entry => entry.Id == "BBQ");
        var hammerEntry = consolidatedSale.Entries.Find(entry => entry.Id == "Hammer");

        Assert.NotNull(bbqEntry);
        Assert.NotNull(hammerEntry);

        Assert.Equal(5, bbqEntry.Quantity);
        Assert.Equal(2, hammerEntry.Quantity);
    }
}
