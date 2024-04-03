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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            return View();
        }
        return View("Riepilogo", User);
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Riepilogo()
    {
        return View();
    }

    // [HttpPost]
    // public IActionResult Summary(User p)
    // {
    //     string hash = ComputeSHA256Hash(p.PasswordHash!);
    //     p.PasswordHash = hash;
    //     foreach (var item in db.Prenotaziones)
    //     {
    //         if (item.Username == p.Username && item.Password == hash)
    //         {
    //             TempData["AlertMessage"] = "User already exists. Please log in.";
    //             return View("Login");
    //         }
    //     }
    //     db.Prenotaziones.Add(p);
    //     db.SaveChanges();
    //     HttpContext.Session.SetString("Username", p.Username!);
    //     HttpContext.Session.SetString("NomeUtente", p.Nome!);
    //     HttpContext.Session.SetString("CognomeUtente", p.Cognome!);
    //     HttpContext.Session.SetString("EmailUtente", p.Email!);
    //     HttpContext.Session.SetString("NascitaUtente", p.dataNascita.ToString());
    //     HttpContext.Session.SetString("SessoUtente", p.sesso!);
    //     HttpContext.Session.SetString("RuoloUtente", p.ruolo!);
    //     return View(p);
    // }
    // public IActionResult Logout()
    // {
    //     HttpContext.Session.SetString("Username", "");
    //     TempData["Message"] = "You have been successfully logged out.";
    //     return View("Login");
    // }

    // public IActionResult Verifica(Prenotazione p)
    // {
    //     string hash = ComputeSHA256Hash(p.Password!);
    //     bool userFound = false;
    //     foreach (var item in db.Prenotaziones)
    //     {
    //         if (item.Username == p.Username && item.Password == hash)
    //         {
    //             HttpContext.Session.SetString("Username", p.Username!);
    //             HttpContext.Session.SetString("NomeUtente", item.Nome!);
    //             HttpContext.Session.SetString("CognomeUtente", item.Cognome!);
    //             HttpContext.Session.SetString("EmailUtente", item.Email!);
    //             HttpContext.Session.SetString("NascitaUtente", item.dataNascita.ToString());
    //             HttpContext.Session.SetString("SessoUtente", item.sesso!);
    //             HttpContext.Session.SetString("RuoloUtente", item.ruolo!);
    //             userFound = true;
    //             break;
    //         }
    //     }
    //     if (!userFound)
    //     {
    //         TempData["AlertMessage"] = "Invalid username or password. Please try again.";
    //         return RedirectToAction("Login");
    //     }
    //     return RedirectToAction("Index");
    // }

    // private string ComputeSHA256Hash(string input)
    // {
    //     byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
    //     StringBuilder builder = new();
    //     foreach (byte b in bytes)
    //     {
    //         builder.Append(b.ToString("x2"));
    //     }
    //     return builder.ToString();
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}