using System; // Provides fundamental types and base classes for the Common Language Runtime (CLR)
using Greggs.Products.Api.Services; // Namespace containing service interfaces and implementations
using Microsoft.AspNetCore.Mvc; // Contains classes for building web APIs using ASP.NET Core
using Microsoft.Extensions.Logging; // Provides a generic interface and methods for logging
using Microsoft.AspNetCore.Http; // Provides types to work with HTTP requests and responses

namespace Greggs.Products.Api.Controllers
{
    [ApiController] // Indicates that the controller responds to HTTP requests
    [Route("[controller]")] // Specifies the route template ("/Product") for the controller
    public class ProductController : ControllerBase // Inherits from the ControllerBase class for ASP.NET Core MVC controllers
    {
        private readonly IProductAccessService _productAccessService; // Interface for accessing product-related data
        private readonly ILogger<ProductController> _logger; // Interface for logging in the ProductController

        // Constructor for the ProductController class
        public ProductController(ILogger<ProductController> logger, IProductAccessService productAccessService)
        {
            _logger = logger; // Assigns the provided logger instance to the private field _logger
            _productAccessService = productAccessService; // Assigns the provided productAccessService instance to _productAccessService
        }

        /// <summary>
        /// Retrieves a list of products with prices converted to euros asynchronously.
        /// </summary>
        /// <param name="pageStart">Start page number (default: 0).</param>
        /// <param name="pageSize">Page size (default: 5).</param>
        /// <param name="toCurrency">Target currency for price conversion (default: "GBP").</param>
        /// <returns>A list of objects containing product details with prices in euros.</returns>
        [HttpGet] // Responds to HTTP GET requests
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))] // Defines the possible response types for HTTP status code 204
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))] // Defines the possible response types for HTTP status code 400
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))] // Defines the possible response types for HTTP status code 404
        public ActionResult<object> GetProductsAsync(int pageStart = 0, int pageSize = 5, string toCurrency = "GBP")
        {
            try
            {
                // Log information about the incoming GET request with provided parameters
                _logger.Log(LogLevel.Information, $"Called GET /Product/Get for params pageStart :{pageStart}, pageSize : {pageSize}, toCurrency :{toCurrency} ");

                // Validate pageStart and pageSize parameters
                if (pageStart > pageSize || pageStart < 0 || pageSize < 0)
                    return BadRequest(Models.Constants.INVALID_PAGE_REQUEST); // Returns a 400 BadRequest response with an error message

                // Check if the specified target currency is valid
                if (!_productAccessService.CheckToCurrency(toCurrency))
                    return NotFound(Models.Constants.INVALID_CURRENCY); // Returns a 404 NotFound response with an error message

                // Get products with prices converted to the specified currency
                var products = _productAccessService.GetProducts(pageStart, pageSize, toCurrency);

                // If no products are found, return a 204 NoContent response
                if (products == null)
                {
                    return NoContent();
                }

                // Return the list of products with prices in the specified currency
                return products;
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the request processing
                _logger.LogError(ex, $"Error occurred for GET /Product/Get for params pageStart :{pageStart}, pageSize : {pageSize}, toCurrency :{toCurrency} ");
                throw; // Rethrows the exception to be handled by the caller
            }
        }
    }
}
