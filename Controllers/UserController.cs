using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WheelWhirlwind.Models;
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly ApplicationDbContext db = new();
    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
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
        return View(db.Users.Where(i => i.Username == HttpContext.Session.GetString("Username")));
    }

    [HttpPost]
    public IActionResult Summary(User p)
    {
        foreach (var item in db.Users)
        {
            if (item.Username == p.Username)
            {
                TempData["AlertMessage"] = "Username already exists. Please choose another one.";
                return View("Signup");
            }
        }
        string hash = ComputeSHA256Hash(p.PasswordHash!);
        p.PasswordHash = hash;
        foreach (var item in db.Users)
        {
            if(item.Email == p.Email)
            {
                TempData["AlertMessage"] = "The email is already in use. Please log in.";
                return View("Login");
            }
        }
        db.Users.Add(p);
        db.SaveChanges();
        HttpContext.Session.SetString("Username", p.Username!);
        HttpContext.Session.SetString("EmailUser", p.Email!);
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
        foreach (var item in db.Users)
        {
            if (item.Email == p.Email && item.PasswordHash == hash)
            {
                HttpContext.Session.SetString("Username", p.Email!);
                HttpContext.Session.SetString("EmailUser", p.Email!);
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}