using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WheelWhirlwind.Models;

public class VehicleController : Controller
{
    private readonly ILogger<VehicleController> _logger;
    private readonly ApplicationDbContext _db;
    
    public VehicleController(ILogger<VehicleController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    public ActionResult Search(int? page, string? type, string? brand, string? model, string? fuel, string? emissions, string? location){
        int currentPage = page ?? 1;
        int pageSize = 3; // Number of listings per page
        var q = _db.Filter(currentPage, pageSize, type, brand, model, fuel, emissions, location);
        
        ViewBag.Page = currentPage;
        ViewBag.Type = type; ViewBag.Brand = brand; ViewBag.Model = model; ViewBag.Fuel = fuel; ViewBag.Emissions = emissions; ViewBag.Location = location;
        ViewBag.TotalPages = (int)Math.Ceiling((double)q.TotalListings.Count / pageSize);

        return View(q.listings);
    }

    public IActionResult Announcement(int id){
        return View(id);
    }
    
    [HttpPost]
    public IActionResult Filter(){
        return RedirectToAction("Search", "Vehicle");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}