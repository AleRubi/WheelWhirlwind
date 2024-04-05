using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WheelWhirlwind.Models;

public class VehicleController : Controller
{
    private readonly ILogger<VehicleController> _logger;
    private readonly ApplicationDbContext _db ;
    
    public VehicleController(ILogger<VehicleController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    public ActionResult Search(int? page){
        int currentPage = page ?? 1;
        int pageSize = 3; // Number of listings per page

        var listings = _db.VehicleListings.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

        ViewBag.Page = currentPage;
        ViewBag.TotalPages = (int)Math.Ceiling((double)_db.VehicleListings.Count() / pageSize);

        return View(listings);
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