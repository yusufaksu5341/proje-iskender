using ProjeIskender.Models;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services.Implementation;

public class TestProductService : IProductService
{
    public IEnumerable<ProductData> GetProducts(string name, uint page = 0, QueryOrder order = QueryOrder.Descending, QueryType qtype = QueryType.Date)
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

    public ProductData GetProduct(ulong id)
    {
        throw new NotImplementedException();
    }

    public void FollowProduct(ulong userId, ulong productId)
    {
        throw new NotImplementedException();
    }

    public bool IsFollowed(ulong userId, ulong productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProductPrice> GetProductPricesHistory(ulong productId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetProductImages(ulong productId)
    {
        throw new NotImplementedException();
    }

    public void HardDeleteProduct(ulong productId)
    {
        throw new NotImplementedException();
    }

    public void SetMainImage(ulong productId, ulong ownerId, string imagePath)
    {
        throw new NotImplementedException();
    }
}