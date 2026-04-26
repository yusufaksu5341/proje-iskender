using ProjeIskender.Models;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services.Implementation;

public class TestProductService : IProductService
{
    private List<ProductData> testData = new List<ProductData>();

    private static IEnumerable<ProductData> OrderUtil<T>(IEnumerable<ProductData> data,
        QueryOrder order, Func<ProductData, T> selector)
    {
        if (order == QueryOrder.Ascending)
        {
            return data.OrderBy(selector);
        }
        else if (order == QueryOrder.Descending)
        {
            return data.OrderByDescending(selector);
        }
        return data;
    }
    
    public IEnumerable<ProductData> GetProducts(string name, uint page = 0, QueryOrder order = QueryOrder.Descending, QueryType qtype = QueryType.Date)
    {
        try
        {

            var now = DateTime.Now;
            var named = testData.Where(x => x.Name.StartsWith(name) || x.ExpirationDate < now);

            IEnumerable<ProductData> ordered;

            if (qtype == QueryType.Date)
            {
                ordered = OrderUtil(named, order, x => x.CreationDate);
            }
            else if (qtype == QueryType.CurrentPrice)
            {
                ordered = OrderUtil(named, order, x => x.CurrentPrice);
            }
            else
            {
                ordered = OrderUtil(named, order, x => x.StartingPrice);
            }

            return ordered.Skip((int)(page * 20)).Take(20);
        }
        catch (NullReferenceException e)
        {
            return Array.Empty<ProductData>();
        }
    }

    public ProductData GetProduct(ulong id)
    {
        var data = testData.First(x => x.ProductId == id);
        if (data == null)
        {
            throw new Exception("Id does not exist");
        }

        return data;
    }

    public IEnumerable<float> GetProductPriceAll(ulong productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProductPrice> GetProductPricesHistory(ulong productId)
    {
        throw new NotImplementedException();
    }

    public float GetProductPrice(ulong productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<float> GetProductPrices(IEnumerable<ulong> products)
    {
        throw new NotImplementedException();
    }

    public ulong CreateProduct(ProductData product)
    {
        throw new NotImplementedException();
    }

    public void DeleteProduct(ulong productId)
    {
        throw new NotImplementedException();
    }

    public void AddProductImage(ulong productId, string imagePath)
    {
        throw new NotImplementedException();
    }

    public void RemoveProductImage(ulong productId, string imagePath)
    {
        throw new NotImplementedException();
    }

    public bool MakeBid(ulong userId, ulong productId, float price)
    {
        throw new NotImplementedException();
    }
}