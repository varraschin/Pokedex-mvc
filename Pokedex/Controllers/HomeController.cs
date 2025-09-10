using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Data;
using Pokedex.Models;
using Microsoft.EntityFrameworkCore;
using Pokedex.ViewModels;

namespace Pokedex.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _db;

    public HomeController(ILogger<HomeController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        var pokemons = _db.Pokemons
            .Include(p => p.Regiao)
            .Include(p => p.Genero)
            .Include(p => p.Tipos)
            .ThenInclude(t => t.Tipo)
            .ToList();
        return View(pokemons);
    }

    public IActionResult Details(int id)
    {
        var pokemon = _db.Pokemons
            .Where(p => p.Numero == id)
            .Include(p => p.Regiao)
            .Include(p => p.Genero)
            .Include(p => p.Tipos)
            .ThenInclude(t => t.Tipo)
            .SingleOrDefault();

        DetailVM detail = new()
        {
            Atual = pokemon,
            Anterior = _db.Pokemons
                .OrderByDescending(p => p.Numero)
                .FirstOrDefault(p => p.Numero < id),
            Proximo = _db.Pokemons
                .OrderBy(p => p)
                .FirstOrDefault(p => p.Numero > id),
        };

        return View(pokemon);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
