using ProjeIskender.Models;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services;

public interface IProductService
{
    public IEnumerable<ProductData> GetProducts(string name, int page = 0, QueryOrder order = QueryOrder.Descending, QueryType qtype = QueryType.Date);
    
    public IEnumerable<float> GetProductPriceAll(ulong productId);
    public float GetProductPrice(ulong productId);
    
    public IEnumerable<float> GetProductPrices(IEnumerable<ulong> products);

    public ulong CreateProduct(ProductData product);
    public void DeleteProduct(ulong productId);
    
    public void AddProductImage(ulong productId, string imagePath);
    public void RemoveProductImage(ulong productId, string imagePath);

    public bool MakeBid(ulong userId, ulong productId, float price);
}