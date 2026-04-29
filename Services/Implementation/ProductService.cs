using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjeIskender.Context;
using ProjeIskender.Controllers.Api;
using ProjeIskender.Models;
using ProjeIskender.Models.Dto;
using ProjeIskender.Models.Exceptions;

namespace ProjeIskender.Services.Implementation;

public class ProductService : IProductService
{
    private IskenderContext _context;
    public ProductService(IskenderContext context)
    {
        _context = context;
    }
    
    public IEnumerable<ProductData> GetProducts(string name, uint page = 0, QueryOrder order = QueryOrder.Descending, QueryType qtype = QueryType.Date)
    {
        var productContext = _context.ProductData;
        
        var now = DateTime.Now;
        IQueryable<ProductData> filteredData;
        if (string.IsNullOrEmpty(name))
            filteredData = productContext.Where(x => x.Visible && x.ExpirationDate > now);
        else
            filteredData = productContext.Where(x => EF.Functions.ILike(x.Name, name + "%") && x.Visible && x.ExpirationDate > now);

        if (order == QueryOrder.Random)
        {
            return filteredData.Skip((int)(page * 20)).Take(20);
        }

        IQueryable<ProductData> orderedData;
        
        if (order == QueryOrder.Descending)
        {
            if (qtype == QueryType.CurrentPrice)
                orderedData = filteredData.OrderByDescending(x => x.CurrentPrice);
            else if  (qtype == QueryType.StartPrice)
                orderedData = filteredData.OrderByDescending(x => x.StartingPrice);
            else 
                orderedData = filteredData.OrderByDescending(x => x.CreationDate);
        }
        else
        {
            if (qtype == QueryType.CurrentPrice)
                orderedData = filteredData.OrderBy(x => x.CurrentPrice);
            else if  (qtype == QueryType.StartPrice)
                orderedData = filteredData.OrderBy(x => x.StartingPrice);
            else 
                orderedData = filteredData.OrderBy(x => x.CreationDate);
        }
        
        return orderedData.Skip((int)(page * 20)).Take(20);
    }

    public ProductData GetProduct(ulong id)
    {
        ProductData? product = _context.ProductData.Find(id);

        if (product == null)
        {
            throw new Exception("Product not found");
        }

        return product;
    }

    public bool IsFollowed(ulong userId, ulong productId)
    {
        var userFollow = _context.UserFollow;

        var follow = userFollow.Find(userId, productId);

        return follow != null;
    }

    public void FollowProduct(ulong userId, ulong productId)
    {
        var userFollow = _context.UserFollow;

        var follow = userFollow.Find(userId, productId);

        if (follow == null)
        {
            follow = new UserFollow()
            {
                UserId = userId,
                ProductId = productId
            };
            
            userFollow.Add(follow);
        }
        else
        {
            userFollow.Remove(follow);
        }
        
        _context.SaveChanges();
    }

    public IEnumerable<float> GetProductPriceAll(ulong productId)
    {
        var prices = _context.ProductPrice;

        return prices.Where(x => x.ProductId == productId).Select(x => x.Price);
    }

    public IEnumerable<ProductPrice> GetProductPricesHistory(ulong productId)
    {
        var prices = _context.ProductPrice;

        return prices.Where(x => x.ProductId == productId);
    }

    public float GetProductPrice(ulong productId)
    {
        var products = _context.ProductData;

        return products.First(x => x.ProductId == productId).CurrentPrice;
    }

    public IEnumerable<float> GetProductPrices(IEnumerable<ulong> products)
    {
        var productsData = _context.ProductData;

        foreach (var x in products)
        {
            var res = productsData.Find(x);
            if (res == null)
            {
                throw new Exception("Product not found");
            }

            yield return res.CurrentPrice;
        }
    }

    public IEnumerable<string> GetProductImages(ulong productId)
    {
        var productImages = _context.ProductImages;

        return productImages.Where(x => x.ProductId == productId).Select(x => x.ResourcePath);
    }

    public ulong CreateProduct(ProductData product)
    {
        _context.ProductData.Add(product);
        
        _context.SaveChanges();

        return product.ProductId;
    }

    public void DeleteProduct(ulong productId)
    {
        var productData = _context.ProductData;

        var product = productData.First(x => x.ProductId == productId);

        product.Visible = false;
        
        productData.Update(product);
        
        _context.SaveChanges();
    }
    
    public void HardDeleteProduct(ulong productId)
    {
        var productData = _context.ProductData;

        var product = productData.First(x => x.ProductId == productId);
        
        productData.Remove(product);
        
        _context.SaveChanges();
    }

    public void AddProductImage(ulong productId, string imagePath)
    {
        var productImages = _context.ProductImages;

        var prodImage = new ProductImages()
        {
            ProductId = productId,
            ResourcePath = imagePath,
        };
        
        productImages.Add(prodImage);
        
        _context.SaveChanges();
    }

    public void RemoveProductImage(ulong productId, string imagePath)
    {
        var productImages = _context.ProductImages;

        var prodImage = productImages.First(x => x.ProductId == productId && x.ResourcePath == imagePath);
        
        productImages.Remove(prodImage);
        
        _context.SaveChanges();
    }

    public bool MakeBid(ulong userId, ulong productId, float price)
    {
        var productData = _context.ProductData;
        var productPrices = _context.ProductPrice;

        var prod = productData.First(x => x.ProductId == productId);

        if (price < prod.CurrentPrice)
        {
            return false;
        }

        prod.CurrentPrice = price;
        productPrices.Add(new ProductPrice()
        {
            ProductId = productId,
            UserId = userId,
            Price = price,
            BidDate = DateTime.Now
        });
        
        _context.SaveChanges();
        return true;
    }

    public void SetMainImage(ulong productId, ulong ownerId, string imagePath)
    {
        var productData =  _context.ProductData;
        
        var product = productData.Find(productId);
        
        if (product == null)
            throw new Exception("Product not found");

        if (product.OwnerId != ownerId)
            throw new UserNotOwnerException();
        
        product.MainImage = imagePath;
        
        productData.Update(product);

        if (_context.SaveChanges() != 1)
        {
            throw new InternalErrorException();
        }
    }
}