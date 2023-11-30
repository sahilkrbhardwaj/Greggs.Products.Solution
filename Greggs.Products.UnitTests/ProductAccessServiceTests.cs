using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests
{
    public class ProductServicesTest
    {
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IDataAccess<Product>> _dataAccessMock;
        private readonly Mock<ILogger<ProductAccessService>> _loggerMock;
        private readonly Mock<ICurrencyExchangeAccess> _currencyExchangeAccessMock;
        public ProductServicesTest()
        {
            _cacheMock = new Mock<IMemoryCache>();
            _dataAccessMock = new Mock<IDataAccess<Product>>();
            _loggerMock = new Mock<ILogger<ProductAccessService>>();
            _currencyExchangeAccessMock = new Mock<ICurrencyExchangeAccess>();
        }
        public static IMemoryCache GetCache(object expectedValue)
        {
            var mockCache = new Mock<IMemoryCache>();
            mockCache
                .Setup(x => x.TryGetValue(Constants.CURRENCY_EXCHANGE_CONFIG, out expectedValue))
                .Returns(true);
            return mockCache.Object;
        }

        [Fact]
        public void CheckToCurrency_WithValidCurrency_ReturnsTrue()
        { 
            //arrange
            var service = new ProductAccessService(_dataAccessMock.Object, _currencyExchangeAccessMock.Object, _cacheMock.Object, _loggerMock.Object);
            
            //act
            var result = service.CheckToCurrency("GBP");

            //assert
            Assert.True(result);
        }

        [Fact]
        public void CheckToCurrency_WithInvalidCurrency_ReturnsFalse()
        {
            //arrange
            var service = new ProductAccessService(_dataAccessMock.Object, _currencyExchangeAccessMock.Object, _cacheMock.Object, _loggerMock.Object);

            //act
            var result = service.CheckToCurrency("E");

            //assert
            Assert.False(result);
        }

        [Fact]
        public void CheckToCurrency_WithNullCurrency_ReturnsFalse()
        {
            //arrange
            var service = new ProductAccessService(_dataAccessMock.Object, _currencyExchangeAccessMock.Object, _cacheMock.Object, _loggerMock.Object);
            //act
            var result = service.CheckToCurrency(null);
            //assert
            Assert.False(result);
        }


        [Fact]
        public void CheckToCurrency_WithNullCache_ReturnsFalse()
        {
            //arrange

            var service = new ProductAccessService(_dataAccessMock.Object, _currencyExchangeAccessMock.Object, _cacheMock.Object, _loggerMock.Object);

            //act
            var result = service.CheckToCurrency("EUR");

            //assert
            Assert.False(result);
        }

        [Fact]
        public void GetProducts_ReturnsProductsInGBP()
        {
            //arrange
            var dataAccessMock = new ProductAccess();
            //var cache = MockMemoryCacheService.GetMemoryCache(null);
            
            var service = new ProductAccessService(dataAccessMock, _currencyExchangeAccessMock.Object, _cacheMock.Object, _loggerMock.Object);
            
            //act
            var result = service.GetProducts(0,5,"GBP");
            
            //assert
            Assert.NotNull(result);
            Assert.Contains("GBP",result[0].ToString());
            Assert.DoesNotContain("EUR", result[0].ToString());
        }
        [Fact]
        public void GetProducts_ReturnsProductsInEUR()
        {

            List<CurrencyExchange> exchangeList = new List<CurrencyExchange>
            {
                new CurrencyExchange { FromCurrency = "GBP", ToCurrency = "EUR", ExchangeRate = (decimal)1.1 },
                new CurrencyExchange { FromCurrency = "USD", ToCurrency = "EUR", ExchangeRate = (decimal)0.85 },
                
            };


            //arrange
            var dataAccessMock = new ProductAccess();
            var cache = GetCache(exchangeList);

            var service = new ProductAccessService(dataAccessMock, _currencyExchangeAccessMock.Object, cache, _loggerMock.Object);

            //act
            var result = service.GetProducts(0, 5, "EUR");

            //assert
            Assert.NotNull(result);
            Assert.Contains("EUR", result[0].ToString());
            Assert.DoesNotContain("GBP", result[0].ToString());
        }
    }
}

