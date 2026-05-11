using ProjeIskender.Models;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services;

public interface IProductService
{
    public IEnumerable<ProductData> GetProducts(string name, uint page = 0, QueryOrder order = QueryOrder.Descending, QueryType qtype = QueryType.Date, ulong? tag = null);
    public ProductData GetProduct(ulong id);

    public ulong GetOwner(ulong productId);
    
    public void FollowProduct(ulong userId, ulong productId);
    public bool IsFollowed(ulong userId, ulong productId);

    public IEnumerable<string> GetProductTags(ulong productId);
    public void AddTag(ulong productId, ulong tagId);
    
    public IEnumerable<float> GetProductPriceAll(ulong productId);
    public IEnumerable<ProductPrice> GetProductPricesHistory(ulong productId);
    public int GetProductBidCount(ulong productId);
    public float GetProductPrice(ulong productId);
    
    public IEnumerable<float> GetProductPrices(IEnumerable<ulong> products);

    public IEnumerable<string> GetProductImages(ulong productId);

    public ulong CreateProduct(ProductData product);
    public void DeleteProduct(ulong productId);
    public void HardDeleteProduct(ulong productId);
    
    public void AddProductImage(ulong productId, string imageUuid);
    public void RemoveProductImage(ulong productId, string imageUuid);

    public bool MakeBid(ulong userId, ulong productId, float price);
    
    public void SetMainImage(ulong productId, ulong ownerId, string imagePath);

    public IEnumerable<Comment> GetComment(ulong productId);
    public void AddComment(ulong productId, ulong sender, string comment);
}