using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Attributes;
using ProjeIskender.Models;
using ProjeIskender.Models.Account;
using ProjeIskender.Models.Dto;
using ProjeIskender.Models.Exceptions;
using ProjeIskender.Models.Product;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers.Api;

[Route("api/product")]
[ApiController]
// [Authentication]
public class Product : ControllerBase
{
    private readonly IProductService productService;
    private readonly IResourceService resourceService;
    
    public Product(IProductService productService, IResourceService resourceService)
    {
        this.productService = productService;
        this.resourceService = resourceService;
    }
    
    [HttpGet("search")]
    public IActionResult Search(
        [FromQuery, MaxLength(128)] string name = "", 
        [FromQuery] string order = "rand", 
        [FromQuery(Name = "order-by")] string orderBy = "date", 
        [FromQuery] uint page = 0)
    {
        QueryOrder queryOrder;
        QueryType queryType;
        switch (order)
        {
            case "rand":
                queryOrder = QueryOrder.Random;
                break;
            case "asc":
                queryOrder = QueryOrder.Ascending;
                break;
            case "desc":
                queryOrder = QueryOrder.Descending;
                break;
            default:
                return BadRequest("Geçersiz 'order' parametresi");
        }
        switch (orderBy)
        {
            case "date":
                queryType = QueryType.Date;
                break;
            case "curr-price":
                queryType = QueryType.CurrentPrice;
                break;
            case "start-price":
                queryType = QueryType.StartPrice;
                break;
            default:
                return BadRequest("Geçersiz 'order-by' parametresi");
        }

        try
        {
            var prods = productService
                .GetProducts(name, page, queryOrder, queryType)
                .Select(x => new SearchRespondBody()
            {
                Date = x.CreationDate,
                Name = x.Name,
                Price = x.CurrentPrice,
                Id = x.ProductId
            })
                .ToArray();
            
            return Ok(new SearchProductRespond()
            {
                Count = (byte)prods.Length,
                Products = prods
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(ulong id)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;
        try
        {
            var prod = productService.GetProduct(id);
            if (prod.Visible == false)
                return NotFound();

            if (prod.OwnerId != userId && prod.ExpirationDate < DateTime.Now)
                return NotFound();

            return Ok(new GetProductResult()
            {
                Name = prod.Name,
                CreationDate = prod.CreationDate,
                ExpirationDate = prod.ExpirationDate,
                CurrentPrice = prod.CurrentPrice,
                StartingPrice = prod.StartingPrice,
                MainImage = prod.MainImage,
                Details = prod.Details
            });
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

    [HttpPost("create")]
    [ContentAccept("application/json")]
    public IActionResult CreateProduct(
        [FromBody] CreateProductRequest createRequest
    )
    {
        if (!createRequest.SinglePrice && createRequest.ExpirationDate >= DateTime.Now.AddMonths(1))
        {
            return BadRequest("Teklif 1 aydan uzun olamaz");
        }

        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;
        ProductData product = new ProductData()
        {
            Name = createRequest.Name,
            CurrentPrice = createRequest.Price,
            StartingPrice = createRequest.Price,
            CreationDate = DateTime.Now,
            ExpirationDate = createRequest.SinglePrice ? (DateTime)createRequest.ExpirationDate! : DateTime.MaxValue,
            OwnerId = userId,
            Visible = true,
            SinglePrice = createRequest.SinglePrice,
            Details = (JsonElement?)createRequest.Details,
        };

        var prodId = productService.CreateProduct(product);
        
        return Ok(prodId.ToString());
    }

    [HttpGet("{productId}/image")]
    public IActionResult GetProductImages(ulong productId)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;
        try
        {
            var prod = productService.GetProduct(productId);
            if (prod.Visible == false || prod.ExpirationDate < DateTime.Now)
                return NotFound();

            var images = productService.GetProductImages(productId).ToArray();

            return Ok(new ProductImagesResult()
            {
                Count = images.Length,
                Images = images
            });
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }
    
    [HttpGet("{productId}/follow")]
    public IActionResult GetFollow(ulong productId)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;
        try
        {
            var prod = productService.GetProduct(productId);
            if (prod.Visible == false || prod.ExpirationDate < DateTime.Now)
                return NotFound();
            
            return Ok(productService.IsFollowed(userId, productId));
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }
    
    [HttpPost("{productId}/follow")]
    public IActionResult Follow(ulong productId)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;
        try
        {
            var prod = productService.GetProduct(productId);
            if (prod.Visible == false || prod.ExpirationDate < DateTime.Now)
                return NotFound();
            
            productService.FollowProduct(userId, productId);

            return Ok();
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }
    
    [HttpPost("{productId}/bid")]
    public IActionResult BidOnProduct(ulong productId, [FromQuery] float price)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;
        try
        {
            var prod = productService.GetProduct(productId);
            if (prod.Visible == false || prod.ExpirationDate < DateTime.Now)
                return NotFound();

            if (prod.SinglePrice)
                return BadRequest("Cannot bid buyable items");

            productService.MakeBid(userId, productId, price);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }
    
    [HttpPost("{productId}/buy")]
    public IActionResult BuyProduct(ulong productId)
    {
        try
        {
            var prod = productService.GetProduct(productId);
            if (prod.Visible == false || prod.ExpirationDate < DateTime.Now)
                return NotFound();

            if (!prod.SinglePrice)
                return BadRequest("Cannot buy bidable items");

            productService.DeleteProduct(productId);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

    [HttpPost("{productId}/image")]
    public IActionResult AddProductImage(ulong productId)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;

        ProductData prod;
        try
        {
            prod = productService.GetProduct(productId);
        }
        catch (Exception e)
        {
            return BadRequest("Product id does not exist");
        }

        if (prod.OwnerId != userId)
        {
            return Forbid();
        }
        
        var fstream = Request.Body;

        try
        {
            var resource = resourceService.CreateResource(Request.ContentType, fstream);
        
            productService.AddProductImage(productId, resource);

            return Ok(resource);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPut("{productId}/image/resource/{imagePath}")]
    public IActionResult SetMainImage(ulong productId, string imagePath)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;

        try
        {
            productService.SetMainImage(productId, userId, $"resource/{imagePath}");

            return Ok();
        }
        catch (UserNotOwnerException e)
        {
            return Forbid();
        }
        catch (InternalErrorException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch
        {
            
        }

        return BadRequest("Product does not exists");
    }

    [HttpGet("{productId}/comments")]
    public IActionResult GetComments(ulong productId)
    {
        return Ok(productService.GetComment(productId));
    }

    [HttpPost("{productId}/comment")]
    public IActionResult AddComment(ulong productId, [FromBody] SendCommentRequest comment)
    {
        ulong userId = (ulong)((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID;

        try
        {
            productService.AddComment(productId, userId, comment.Content);
            return Ok();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}