using Microsoft.AspNetCore.Mvc;
using SizzlingHotProducts.Interfaces;
using SizzlingHotProducts.Models;

namespace SizzlingHotProducts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly IJsonFileAccess _json;

        public SalesController(ISalesService salesService, IJsonFileAccess json)
        {
            _salesService = salesService;
            _json = json;
        }

        /// <summary>
        /// Consolidate sales
        /// </summary>
        /// <returns>List of consolidated sales</returns>
        [HttpGet("consolidate")]
        public IActionResult GetConsolidatedSales()
        {
            var data = _json.ReadJsonFile<Sale>("orders.json");
            var consolidatedSales = _salesService.ProcessSales(data);
            return Ok(consolidatedSales);
        }

        /// <summary>
        /// Gets top selling products in the last 3 days
        /// </summary>
        /// <returns>List of products sold in each date</returns>
        [HttpGet("top3days")]
        public IActionResult GetSales()
        {
            
            var data = _json.ReadJsonFile<Sale>("orders.json");
            var products = _json.ReadJsonFile<Product>("products.json");
            var consolidatedSales = _salesService.ProcessSales(data);

            var topProducts = _salesService.GetTopSellingProductsLast3Days(consolidatedSales, new DateTime(2021,07,20), products); //assuming today is 20/7/2021 - we can change this to DateTime.Now to get the current date

            return Ok(topProducts);
        }

        /// <summary>
        /// Gets top selling product
        /// </summary>
        /// <returns>A product - Id, name, quanlity selling</returns>
        [HttpGet("top")]
        public IActionResult GetTopProduct()
        {

            var data = _json.ReadJsonFile<Sale>("orders.json");
            var products = _json.ReadJsonFile<Product>("products.json");
            var consolidatedSales = _salesService.ProcessSales(data);

            var topProduct = _salesService.GetTopSellingProduct(consolidatedSales, products);


            return Ok(topProduct);
        }
    }
}
