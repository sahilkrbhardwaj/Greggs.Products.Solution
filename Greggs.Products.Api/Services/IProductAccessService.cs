using System.Collections.Generic;
using System.Threading.Tasks;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services
{
    public interface IProductAccessService
    {
        List<object> GetProducts(int? pageStart, int? pageSize, string toCurrency = "GBP");
        bool CheckToCurrency(string toCurrency);
    }
}