using Xunit;
using SizzlingHotProducts.Services;
using SizzlingHotProducts.Interfaces;
using SizzlingHotProducts.Models;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
public class SalesServiceTest
{
    private readonly ISalesService _salesService;

    public SalesServiceTest()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<OrderStatusService, OrderStatusService>()
            .AddScoped<ISalesConsolidateService, SalesConsolidateService>()
            .AddScoped<ISalesService, SalesService>()
            .BuildServiceProvider();

        _salesService = serviceProvider.GetRequiredService<ISalesService>();
    }
    [Fact]
    public void GetTopSellingProductsLast3Days_ShouldReturnTopProducts_Sell_In_The_Last_3_Days()
    {
   
        // Act
        var result = _salesService.GetTopSellingProductsLast3Days(GetSampleConsolidatedSales(), new DateTime(2021, 07, 21), GetSampleProducts());

        // Assert
        // Add your assertions here based on the expected result
        Xunit.Assert.NotNull(result);
        var result21 = result.Where(x => x.Date == "21/07/2021").ToList();
        Xunit.Assert.True(result21[0].ProductId == "P2");
        Xunit.Assert.True(result21[0].ProductName == "Product 2");

        var result20 = result.Where(x => x.Date == "20/07/2021").ToList();
        Xunit.Assert.True(result20[0].ProductId == "P1");
        Xunit.Assert.True(result20[0].ProductName == "Product 1");

        var result19 = result.Where(x => x.Date == "20/07/2021").ToList();
        Xunit.Assert.True(result19[0].ProductId == "P1");
        Xunit.Assert.True(result19[0].ProductName == "Product 1");
    }

    [Fact]
    public void GetTopSellingProduct_ShouldReturnATopProduct()
    {

        // Act
        var result = _salesService.GetTopSellingProduct(GetSampleConsolidatedSales(), GetSampleProducts());

        Xunit.Assert.NotNull(result);
       
        Xunit.Assert.True(result.ProductId == "P2");
        Xunit.Assert.True(result.ProductName == "Product 2");
        Xunit.Assert.True(result.TotalQuantity == 10);
    }
    private List<Sale> GetSampleConsolidatedSales()
    {
        return new List<Sale>
        {
            new Sale
            {
                OrderId = "O10",
                CustomerId = "C1",
                Entries = new List<SaleEntry>
                {
                    new SaleEntry { Id = "P1", Quantity = 2 },
                    new SaleEntry { Id = "P2", Quantity = 4 }
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
                    new SaleEntry { Id = "P1", Quantity = 3 }
                },
                Date = "20/07/2021",
                Status = "completed"
            },
            new Sale
            {
                OrderId = "O12",
                CustomerId = "D",
                Entries = new List<SaleEntry>
                {
                    new SaleEntry { Id = "P2", Quantity = 3 }
                },
                Date = "20/07/2021",
                Status = "completed"
            },
              new Sale
            {
                OrderId = "O13",
                CustomerId = "C1",
                Entries = new List<SaleEntry>
                {
                    new SaleEntry { Id = "P2", Quantity = 1 }
                },
                Date = "21/07/2021",
                Status = "completed"
            },
              new Sale
            {
                OrderId = "O14",
                CustomerId = "C4",
                Entries = new List<SaleEntry>
                {
                    new SaleEntry { Id = "P2", Quantity = 2 }
                },
                Date = "21/07/2021",
                Status = "completed"
            }
        };
    }

    private List<Product> GetSampleProducts()
    {
        return new List<Product>
        {
            new Product { Id = "P1", Name = "Product 1"},
            new Product { Id = "P2", Name = "Product 2"}
        };
        
    }
}
