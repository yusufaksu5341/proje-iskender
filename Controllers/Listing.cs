using Microsoft.AspNetCore.Mvc;

namespace ProjeIskender.Controllers;

[Route("/ilan")]
public class Listing : Controller
{
    [HttpGet("ekle")]
    public IActionResult Add() => View();

    [HttpGet("{id:int}")]
    public IActionResult Detail(int id) => View();
}
