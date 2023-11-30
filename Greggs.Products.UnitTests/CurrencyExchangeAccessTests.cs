using System.Collections.Generic;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests
{
    public class CurrencyExchangeAccessTests
    {
        [Fact]
        public void GetExchange_Returns_ExpectedResult()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            var currencyExchangeData = new List<CurrencyExchange>
            {
                new CurrencyExchange { FromCurrency = "GBP", ToCurrency = "EUR", ExchangeRate = (decimal)1.1 },
                new CurrencyExchange { FromCurrency = "USD", ToCurrency = "EUR", ExchangeRate = (decimal)0.85 },
            };
            configuration.Setup(c => c.GetSection(Constants.CURRENCY_EXCHANGE_CONFIG))
                         .Returns(new Mock<IConfigurationSection>().Object);
            var currencyExchangeAccess = new CurrencyExchangeAccess(configuration.Object);

            // Act
            var result = currencyExchangeAccess.GetExchange();

            // Assert
            // Add assertions to verify the correctness of the result
            Assert.NotNull(result);
            // Add more assertions based on your expected behavior
        }

        // Add more test methods as needed for different scenarios
    }
}
