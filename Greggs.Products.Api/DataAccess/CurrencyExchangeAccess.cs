using System.Collections.Generic; // Provides interfaces and classes for implementing collections
using Greggs.Products.Api.Models; // Contains models representing entities in the product domain
using Microsoft.Extensions.Configuration; // Provides configuration-related functionality
using Microsoft.Extensions.Logging; // Provides a generic interface and methods for logging

namespace Greggs.Products.Api.DataAccess
{
    /// <summary>
    /// DISCLAIMER: This is only here to help enable the purpose of this exercise, this doesn't reflect the way we work!
    /// </summary>
    public class CurrencyExchangeAccess : ICurrencyExchangeAccess // Implements the ICurrencyExchangeAccess interface
    {
        private readonly IConfiguration _configuration; // Interface for application configuration settings

        /// <summary>
        /// Constructor for CurrencyExchangeAccess.
        /// </summary>
        /// <param name="configuration">Configuration settings.</param>
        public CurrencyExchangeAccess(IConfiguration configuration)
        {
            _configuration = configuration; // Assigns the provided configuration instance to the private field _configuration
        }

        /// <summary>
        /// Retrieves the list of currency exchanges from the configuration.
        /// </summary>
        /// <returns>A list of CurrencyExchange objects.</returns>
        public List<CurrencyExchange> GetExchange()
        {
            List<CurrencyExchange> currencyExchange = new List<CurrencyExchange>();

            // Binds the configuration section "CURRENCY_EXCHANGE_CONFIG" to a list of CurrencyExchange objects
            _configuration.GetSection(Constants.CURRENCY_EXCHANGE_CONFIG).Bind(currencyExchange);

            return currencyExchange; // Returns the list of currency exchanges
        }
    }
}
