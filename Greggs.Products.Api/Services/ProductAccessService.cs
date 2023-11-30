using System; // Provides fundamental types and base classes for the Common Language Runtime (CLR)
using System.Collections.Generic; // Provides interfaces and classes for implementing collections
using System.Linq; // Provides classes and interfaces that support queries that use Language-Integrated Query (LINQ)
using Greggs.Products.Api.DataAccess; // Namespace containing data access related classes/interfaces
using Greggs.Products.Api.Models; // Contains models representing entities in the product domain
using Microsoft.Extensions.Caching.Memory; // Provides in-memory caching functionality
using Microsoft.Extensions.Logging; // Provides a generic interface and methods for logging

namespace Greggs.Products.Api.Services
{
    /// <summary>
    /// Service handling access to products, providing methods to retrieve product information.
    /// </summary>
    public class ProductAccessService : IProductAccessService // Implements the IProductAccessService interface
    {
        private readonly IDataAccess<Product> _productAccess; // Private field to access product data
        private readonly ICurrencyExchangeAccess _currencyExchangeAccess; // Private field to access exchange data
        private readonly IMemoryCache _cache; // Interface for in-memory caching
        private readonly ILogger<ProductAccessService> _logger; // Interface for logging in the ProductAccessService

        /// <summary>
        /// Constructor for ProductAccessService.
        /// </summary>
        /// <param name="productAccess">Data access interface for products.</param>
        public ProductAccessService(IDataAccess<Product> productAccess, ICurrencyExchangeAccess currencyExchangeAccess, IMemoryCache cache, ILogger<ProductAccessService> logger)
        {
            _productAccess = productAccess ?? throw new ArgumentNullException(nameof(productAccess));
            _currencyExchangeAccess = currencyExchangeAccess; // Assigns the provided currencyExchangeAccess instance to _currencyExchangeAccess
            _cache = cache; // Assigns the provided cache instance to the private field _cache
            _logger = logger; // Assigns the provided logger instance to the private field _logger
        }

        /// <summary>
        /// Checks if the target currency is valid.
        /// </summary>
        /// <param name="toCurrency">Target currency to check.</param>
        /// <returns>True if the target currency is valid, otherwise false.</returns>
        public bool CheckToCurrency(string toCurrency)
        {
            if (string.IsNullOrEmpty(toCurrency))
                return false;
            if (toCurrency == "GBP")
                return true;

            var config = _cache.Get<List<CurrencyExchange>>(Models.Constants.CURRENCY_EXCHANGE_CONFIG);
            if (config == null)
                return false;

            var currencyExchange = config.FirstOrDefault(e => string.Equals(e.ToCurrency, toCurrency, StringComparison.Ordinal));
            return currencyExchange != null;
        }

        /// <summary>
        /// Retrieves products with prices converted to the specified currency asynchronously with optional pagination.
        /// </summary>
        /// <param name="pageStart">Start page number (optional).</param>
        /// <param name="pageSize">Page size (optional).</param>
        /// <param name="toCurrency">Target currency for price conversion (default: "GBP").</param>
        /// <returns>A list of objects containing product details with prices in the specified currency.</returns>
        public List<object> GetProducts(int? pageStart, int? pageSize, string toCurrency = "GBP")
        {
            toCurrency = toCurrency.ToUpper();

            if (!CheckToCurrency(toCurrency))
                return null;

            var products = _productAccess.List(pageStart, pageSize);
            if (products.Count() == 0)
            {
                return null;
            }

            var result = new List<object>();
            var config = _cache.Get<List<CurrencyExchange>>(Models.Constants.CURRENCY_EXCHANGE_CONFIG);

            if (toCurrency == "GBP")
            {
                var newItem = new
                {
                    Currency = "GBP",
                    ProductsCount = products.Count(),
                    Products = products
                };
                result.Add(newItem);
            }
            else
            {
                var currencyExchange = config.FirstOrDefault(e => string.Equals(e.ToCurrency, toCurrency, StringComparison.Ordinal));
                var newItem = new
                {
                    Currency = toCurrency,
                    ProductsCount = products.Count(),
                    Products = products.Select(e => new { Name = e.Name, PriceInEuros = e.PriceInPounds * currencyExchange.ExchangeRate })
                };
                result.Add(newItem);
            }
            return result;
        }
    }
}
