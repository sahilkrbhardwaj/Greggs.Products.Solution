using Greggs.Products.Api.Models;
using System.Collections.Generic;

namespace Greggs.Products.Api.DataAccess;

public interface ICurrencyExchangeAccess
{
    List<CurrencyExchange> GetExchange();
}