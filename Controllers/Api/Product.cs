using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Attributes;
using ProjeIskender.Models;
using ProjeIskender.Models.Account;
using ProjeIskender.Models.Dto;
using ProjeIskender.Models.Product;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers.Api;

[Route("api/product")]
[ApiController]
[Authentication]
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
                Date = DateTime.SpecifyKind(x.CreationDate,  DateTimeKind.Unspecified),
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
        try
        {
            var prod = productService.GetProduct(id);
            if (prod.Visible == false)
                return NotFound();

            return Ok(prod);
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
        if (createRequest.ExpirationDate >= DateTime.Now.AddMonths(1))
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
            ExpirationDate = createRequest.ExpirationDate,
            OwnerId = userId,
            Visible = createRequest.ImageCount == 0
        };

        var prodId = productService.CreateProduct(product);
        
        return Ok(prodId);
    }

    [HttpPost("image")]
    public IActionResult AddProductImage([FromQuery(Name = "product-id")] ulong productId)
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
}