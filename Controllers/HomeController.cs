using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Models;

namespace RedSocial.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult RedSocial()
    {
        string usuario = HttpContext.Session.GetString("Usuario");

        if (string.IsNullOrEmpty(usuario))
        {
            return RedirectToAction("Login", "Usuarios");
        }

        BD bd = new BD();
        var publicaciones = bd.ObtenerPublicaciones(0);

        ViewBag.Usuario = usuario;
        ViewBag.Publicaciones = publicaciones;
        return View();
    }
}
