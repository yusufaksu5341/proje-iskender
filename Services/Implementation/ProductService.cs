using ProjeIskender.Models;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services.Implementation;

public class ProductService : IProductService
{
    public IEnumerable<ProductData> GetProducts(string name, int page = 0, QueryOrder order = QueryOrder.Descending, QueryType qtype = QueryType.Date)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<float> GetProductPriceAll(ulong productId)
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