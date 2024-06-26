using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WheelWhirlwind.Models;
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly ApplicationDbContext _db ;
    public UserController(ILogger<UserController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult SignUp()
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            return View();
        }
        return View("Summary", User);
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Summary()
    {
        return View(_db.Users.Where(i => i.Username == HttpContext.Session.GetString("Username")));
    }

    [HttpPost]
    public IActionResult Summary(User p)
    {
        foreach (var item in _db.Users)
        {
            if (item.Username == p.Username)
            {
                TempData["AlertMessage"] = "Username already exists. Please choose another one.";
                return View("Signup");
            }
        }
        string hash = ComputeSHA256Hash(p.PasswordHash!);
        p.PasswordHash = hash;
        foreach (var item in _db.Users)
        {
            if(item.Email == p.Email)
            {
                TempData["AlertMessage"] = "The email is already in use. Please log in.";
                return View("Login");
            }
        }
        _db.Users.Add(p);
        _db.SaveChanges();
        HttpContext.Session.SetString("Username", p.Username!);
        HttpContext.Session.SetString("EmailUser", p.Email!);
        foreach (var item in _db.Users)
        {
            if(item.Username == p.Username)
            {
                HttpContext.Session.SetInt32("UserId", item.UserId);
                break;
            }
        }
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Logout()
    {
        HttpContext.Session.SetString("Username", "");
        TempData["Message"] = "You have been successfully logged out.";
        return View("Login");
    }

    public IActionResult Verify(User p)
    {
        string hash = ComputeSHA256Hash(p.PasswordHash!);
        bool userFound = false;
        foreach (var item in _db.Users)
        {
            if (item.Email == p.Email && item.PasswordHash == hash)
            {
                HttpContext.Session.SetString("Username", p.Email!);
                HttpContext.Session.SetString("EmailUser", p.Email!);
                HttpContext.Session.SetInt32("UserId", item.UserId);
                userFound = true;
                break;
            }
        }
        if(!userFound)
        {
            TempData["AlertMessage"] = "Invalid username or password. Please try again.";
            return RedirectToAction("Login");
        }
        return RedirectToAction("Index", "Home");
    }

    private static string ComputeSHA256Hash(string input)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        StringBuilder builder = new();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }

    public IActionResult Profile(int id)
    {
        return View(id);
    }

    [HttpPost]
    public IActionResult Sell(int id)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            return View("Login");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Publish(Vehicle v, List<IFormFile> Images)
    {
        _db.Vehicles.Add(v);
        _db.SaveChanges();

        int? vehicleId = _db.Vehicles
            .OrderByDescending(v => v.VehicleId)
            .Select(v => v.VehicleId)
            .FirstOrDefault();
        
        VehicleListing vl = new(){
            VehicleId = vehicleId,
            UserId = HttpContext.Session.GetInt32("UserId")
        };
        _db.VehicleListings.Add(vl);
        _db.SaveChanges();

        foreach (var item in Images)
        {
            if (item != null && item.Length > 0)
            {
                var imageDir = "Images";
                var directoryPath = Path.Combine("wwwroot", imageDir);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                var filePath = Path.Combine(directoryPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await item.CopyToAsync(stream);

                VehicleImage vi = new()
                {
                    Path = fileName,
                    VehicleId = vehicleId
                };
                _db.VehicleImages.Add(vi);
                _db.SaveChanges();

            }
        }
        return View("Profile", HttpContext.Session.GetInt32("UserId"));
    }

    public IActionResult Favourite()
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            return View("Login");
        }
        return View();
    }

    public IActionResult AddFavourite(int id)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            return View("Login");
        }
        int? userId = HttpContext.Session.GetInt32("UserId");
        
        if(userId.HasValue){
            var isOwner = _db.VehicleListings.Any(i => i.UserId == userId && i.VehicleId == id);
            
            if (!isOwner){
                var existingFavorite = _db.VehicleUserFavourites.FirstOrDefault(f => f.UserId == userId && f.VehicleId == id);
                
                if(existingFavorite != null){
                    _db.VehicleUserFavourites.Remove(existingFavorite);
                }
                else{
                    VehicleUserFavourite vuf = new()
                    {
                        UserId = userId.Value,
                        VehicleId = id
                    };
                    _db.VehicleUserFavourites.Add(vuf);
                }
                _db.SaveChanges();
            }
        }
        return View("Favourite");
    }

    public IActionResult Settings(int id)
    {
        if(id != HttpContext.Session.GetInt32("UserId"))
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public IActionResult ChangeData(User updatedUser)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.UserId == updatedUser.UserId);
        
        if (existingUser != null){
            existingUser.Name = updatedUser.Name;
            existingUser.Surname = updatedUser.Surname;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            
            _db.SaveChanges();

            TempData["Message"] = "User data updated successfully.";
        }else{
            TempData["AlertMessage"] = "User not found.";
        }
        return View("Profile", HttpContext.Session.GetInt32("UserId"));
    }

    public IActionResult Delete(int id){
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            return View("Login");
        }

        var favourite = _db.VehicleUserFavourites.Where(i => i.VehicleId == id);
        foreach (var item in favourite)
        {
            _db.VehicleUserFavourites.Remove(item);
        }
        
        var listing = _db.VehicleListings.Where(i => i.VehicleId == id);
        foreach (var item in listing)
        {
            _db.VehicleListings.Remove(item);
        }

        var img = _db.VehicleImages.Where(i => i.VehicleId == id);
        foreach (var item in img)
        {
            _db.VehicleImages.Remove(item);
        }

        var vehicle = _db.Vehicles.Where(i => i.VehicleId == id);
        foreach (var item in vehicle)
        {
            _db.Vehicles.Remove(item);
        }
        _db.SaveChanges();
        return View("Profile", HttpContext.Session.GetInt32("UserId"));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}