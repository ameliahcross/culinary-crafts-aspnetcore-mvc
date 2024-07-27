using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using culinary_crafts_dotnet_mvc.Models;

namespace culinary_crafts_dotnet_mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public DapperDb db = new DapperDb();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var recipesWithIngredients = await db.GetAllRecipesWithIngredients();
        return View(recipesWithIngredients);
    }
    
    public async Task<IActionResult> IncreaseRating(int Id)
    {
        if (Id > 0)
        {
            await db.IncreaseRating(Id);
            var recipesWithIngredients = await db.GetAllRecipesWithIngredients();
            return View("Index", recipesWithIngredients);
        }
        return View("Index");
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