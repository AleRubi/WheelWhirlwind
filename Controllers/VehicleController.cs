using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WheelWhirlwind.Models;

public class VehicleController : Controller
{
    private readonly ILogger<VehicleController> _logger;

    
    public VehicleController(ILogger<VehicleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Search(){
        return View();
    }
    [HttpGet]
    public IActionResult Announcement(){
        return View();
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