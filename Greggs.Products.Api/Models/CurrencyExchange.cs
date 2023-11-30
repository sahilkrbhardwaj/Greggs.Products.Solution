namespace Greggs.Products.Api.Models;

public class CurrencyExchange
{
    public string FromCurrency { get; set; }
    public string ToCurrency { get; set; }
    public decimal ExchangeRate { get; set; }
}