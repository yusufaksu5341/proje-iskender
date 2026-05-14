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
    
    public IEnumerable<ProductData> GetProducts(string name, uint page = 0, QueryOrder order = QueryOrder.Descending, QueryType qtype = QueryType.Date, ulong? tag = null)
    {
        var productContext = _context.ProductData;
        var tagContext = _context.ProductTags;
        
        var now = DateTime.Now;
        IQueryable<ulong>? taggedProducts = tag != null ? tagContext.Where(x => x.TagId == tag).Select(x => x.ProductId) : null;
        IQueryable<ProductData> filteredData = taggedProducts == null ? productContext.AsQueryable() : productContext.Where(x => taggedProducts.Contains(x.ProductId));
        if (string.IsNullOrEmpty(name))
            filteredData = filteredData.Where(x => x.Visible && x.ExpirationDate > now);
        else
            filteredData = filteredData.Where(x => EF.Functions.ILike(x.Name, name + "%") && x.Visible && x.ExpirationDate > now);

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

    public ulong GetOwner(ulong productId)
    {
        return _context.ProductData.First(x => x.ProductId == productId).OwnerId;
    }

    public bool IsFollowed(ulong userId, ulong productId)
    {
        var userFollow = _context.UserFollow;

        var follow = userFollow.Find(userId, productId);

        return follow != null;
    }

    public IEnumerable<string> GetProductTags(ulong productId)
    {
        var productTags = _context.ProductTags;
        var tagNames = _context.Tags;

        var tags = productTags.Where(x => x.ProductId == productId).Select(x => x.TagId);

        return tagNames.Where(x => tags.Contains(x.TagId)).Select(x => x.TagName);
    }


    public void AddTag(ulong productId, ulong tagId)
    {
        var productTags = _context.ProductTags;

        productTags.Add(new ProductTags()
        {
            ProductId = productId,
            TagId = tagId
        });

        if (_context.SaveChanges() != 1)
        {
            throw new Exception("Product already has this tag");
        }
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

    public int GetProductBidCount(ulong productId)
    {
        var prices = _context.ProductPrice;

        return prices.Count(x => x.ProductId == productId);
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
        long pid = (long)productId;
        float currentPrice = _context.Database
            .SqlQuery<float>($"SELECT current_price AS \"Value\" FROM product_tb WHERE product_id = {pid}")
            .FirstOrDefault();

        if (price <= currentPrice)
            return false;

        _context.Database.ExecuteSqlInterpolated(
            $"UPDATE product_tb SET current_price = {price} WHERE product_id = {pid}");
        _context.Database.ExecuteSqlInterpolated(
            $"INSERT INTO product_price_tb (bid_date, user_id, product_id, price) VALUES ({DateTime.Now}, {(long)userId}, {pid}, {price})");
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

    public IEnumerable<Comment> GetComment(ulong productId)
    {
        var commentData = _context.Comments;
        var res = commentData.Where(x => x.ProductId == productId);

        return res.ToArray();
    }

    public void AddComment(ulong productId, ulong sender, string comment)
    {
        _context.Database.ExecuteSqlInterpolated(
            $"INSERT INTO comment_tb (owner_id, product_id, creation_date, comment_context) VALUES ({(long)sender}, {(long)productId}, {DateTime.Now}, {comment})");
    }

    public IEnumerable<CommentDisplay> GetComments(ulong productId)
    {
        var comments = _context.Comments
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.Date)
            .ToList();

        var userIds = comments.Select(c => c.UserId).Distinct().ToList();
        var users = _context.UserData
            .Where(u => userIds.Contains(u.UserId))
            .ToDictionary(u => u.UserId, u => u.UserName);

        return comments.Select(c => new CommentDisplay
        {
            UserName = users.TryGetValue(c.UserId, out var n) ? n : "?",
            Content  = c.Content,
            Date     = c.Date,
        });
    }

    public IEnumerable<ProductData> GetAllVisibleProducts()
    {
        var now = DateTime.Now;
        return _context.ProductData
            .Where(x => x.Visible && x.ExpirationDate > now)
            .OrderByDescending(x => x.CreationDate)
            .Take(200)
            .ToList();
    }

    public ulong? GetLatestBidder(ulong productId)
    {
        var latest = _context.ProductPrice
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.BidDate)
            .FirstOrDefault();
        return latest?.UserId;
    }

    public IEnumerable<ProductPrice> GetUserBids(ulong userId)
    {
        return _context.ProductPrice
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.BidDate)
            .ToList();
    }

    public void UpdateProductInfo(ulong productId, string name, System.Text.Json.JsonElement? details)
    {
        var product = _context.ProductData.Find(productId);
        if (product == null) throw new Exception("Product not found");
        product.Name = name;
        product.Details = details;
        _context.ProductData.Update(product);
        _context.SaveChanges();
    }

    public IEnumerable<ProductData> GetFollowedProducts(ulong userId)
    {
        var followedIds = _context.UserFollow
            .Where(x => x.UserId == userId)
            .Select(x => x.ProductId)
            .ToList();
        return _context.ProductData
            .Where(x => followedIds.Contains(x.ProductId) && x.Visible)
            .OrderByDescending(x => x.CreationDate)
            .ToList();
    }

    public bool MakePurchase(ulong userId, ulong productId)
    {
        var product = _context.ProductData.Find(productId);
        if (product == null || !product.Visible || !product.SinglePrice)
            return false;

        _context.Database.ExecuteSqlInterpolated(
            $"INSERT INTO product_price_tb (bid_date, user_id, product_id, price) VALUES ({DateTime.Now}, {(long)userId}, {(long)productId}, {product.CurrentPrice})");

        product.Visible = false;
        _context.ProductData.Update(product);
        _context.SaveChanges();
        return true;
    }

    public IEnumerable<BidHistoryItem> GetBidHistory(ulong productId, int limit = 20)
    {
        var bids = _context.ProductPrice
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.BidDate)
            .Take(limit)
            .ToList();

        var userIds = bids.Select(b => b.UserId).Distinct().ToList();
        var users = _context.UserData
            .Where(u => userIds.Contains(u.UserId))
            .ToDictionary(u => u.UserId, u => u.UserName);

        return bids.Select(b => new BidHistoryItem
        {
            UserId   = b.UserId,
            UserName = users.TryGetValue(b.UserId, out var n) ? n : "?",
            Price    = b.Price,
            BidDate  = b.BidDate,
        });
    }

    public IEnumerable<ProductData> GetPurchases(ulong userId)
    {
        var now = DateTime.Now;

        var boughtProductIds = _context.ProductPrice
            .Where(x => x.UserId == userId)
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var fixedPurchases = _context.ProductData
            .Where(x => boughtProductIds.Contains(x.ProductId) && x.SinglePrice && !x.Visible)
            .ToList();

        var expiredAuctions = _context.ProductData
            .Where(x => boughtProductIds.Contains(x.ProductId) && !x.SinglePrice && x.ExpirationDate < now)
            .ToList();

        var auctionWins = new List<ProductData>();
        foreach (var auction in expiredAuctions)
        {
            var topBidder = _context.ProductPrice
                .Where(x => x.ProductId == auction.ProductId)
                .OrderByDescending(x => x.Price)
                .Select(x => x.UserId)
                .FirstOrDefault();
            if (topBidder == userId)
                auctionWins.Add(auction);
        }

        return fixedPurchases.Concat(auctionWins)
            .OrderByDescending(x => x.ExpirationDate)
            .ToList();
    }
}