using System; // Provides fundamental types and base classes for the Common Language Runtime (CLR)
using System.Collections.Generic; // Provides interfaces and classes for implementing collections
using System.Threading; // Provides classes and interfaces that enable multithreaded programming
using System.Threading.Tasks; // Provides types that simplify the work of writing concurrent and asynchronous code
using Greggs.Products.Api.DataAccess; // Namespace containing data access related classes/interfaces
using Greggs.Products.Api.Models; // Contains models representing entities in the product domain
using Microsoft.Extensions.Caching.Memory; // Provides in-memory caching functionality
using Microsoft.Extensions.Hosting; // Provides hosting infrastructure services
using Microsoft.Extensions.Logging; // Provides a generic interface and methods for logging

namespace Greggs.Products.Api.Services
{
    public class CacheService : BackgroundService // Inherits from BackgroundService for background task execution
    {
        private readonly IMemoryCache _cache; // Interface for in-memory caching
        private readonly ICurrencyExchangeAccess _currencyExchange; // Interface for accessing currency exchange data
        private readonly ILogger<CacheService> _logger; // Interface for logging in the CacheService
        private Timer _timer; // Timer object for periodic cache update

        // Constructor for CacheService class
        public CacheService(IMemoryCache cache, ICurrencyExchangeAccess currencyExchange, ILogger<CacheService> logger)
        {
            _cache = cache; // Assigns the provided cache instance to the private field _cache
            _currencyExchange = currencyExchange; // Assigns the provided currencyExchange instance to _currencyExchange
            _logger = logger; // Assigns the provided logger instance to the private field _logger
        }

        // Overrides the ExecuteAsync method of BackgroundService to define the background task execution logic
        protected override Task ExecuteAsync(CancellationToken cancelToken)
        {
            // Creates a timer to invoke the DoWork method periodically
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask; // Returns a completed task
        }

        // Method called by the timer at regular intervals
        private void DoWork(object state)
        {
            UpdateCurrencyConversionCache(); // Invokes the method to update the currency conversion cache
        }

        // Method to update the currency conversion cache
        private void UpdateCurrencyConversionCache()
        {
            // Logs information about the update of currency conversion cache with UTC timestamp
            _logger.LogInformation($"Updating currency conversion cache at UTC time {DateTime.UtcNow}");

            // Retrieves currency exchange configuration data
            var config = _currencyExchange.GetExchange();

            // Sets the retrieved currency exchange data into the cache
            _cache.Set<List<CurrencyExchange>>(Models.Constants.CURRENCY_EXCHANGE_CONFIG, config);
        }
    }
}
