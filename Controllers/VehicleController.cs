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
    public ActionResult Search(int? page, string? type){
        int currentPage = page ?? 1;
        int pageSize = 3; // Number of listings per page
        List<VehicleListing> listings = new();
        if(HttpContext.Session.GetString("typeFilter") != null){
            listings = _db.VehicleListings.Where(vl => _db.Vehicles.Any(v => v.VehicleId == vl.VehicleId && v.Type == HttpContext.Session.GetString("typeFilter"))).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }else{
            listings = _db.VehicleListings.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
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
    [HttpPost]
    public ActionResult ClearFilter()
    {
        HttpContext.Session.Remove("typeFilter");
        return View("Search");
    }
    [HttpPost]
    public ActionResult GetDataFromSession(string typeFilter)
    {
        HttpContext.Session.SetString("typeFilter", typeFilter);
        return View("Search");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}