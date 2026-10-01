using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Models;

namespace RedSocial.Controllers;

public class UsuariosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(Usuarios usuario)
    {
        if (usuario == null)
        {
            ViewBag.Error = "Debe completar los datos del login.";
            return View();
        }

        if (string.IsNullOrWhiteSpace(usuario.NombreUsuario) || string.IsNullOrWhiteSpace(usuario.Contrasenia))
        {
            ViewBag.Error = "Debe completar usuario y contraseña.";
            return View(usuario);
        }

        BD bd = new BD();
        Usuarios usuarioBD = bd.ObtenerUsuario(usuario.NombreUsuario);

        if (usuarioBD != null && usuarioBD.Contrasenia == usuario.Contrasenia)
        {
            HttpContext.Session.SetString("Usuario", usuarioBD.NombreUsuario);
            HttpContext.Session.SetInt32("IdUsuario", usuarioBD.Id);

            return RedirectToAction("RedSocial", "Home");
        }

ViewBag.Error = "Usuario o contraseña incorrectos";
return View(usuario);
    }

    [HttpPost]
    public IActionResult Registro(Usuarios usuario)
    {
        if (usuario == null)
        {
            ViewBag.Error = "Todos los campos son obligatorios.";
            return View();
        }

        if (string.IsNullOrWhiteSpace(usuario.NombreUsuario) || string.IsNullOrWhiteSpace(usuario.Contrasenia) || string.IsNullOrWhiteSpace(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Apellido))
        {
            ViewBag.Error = "Todos los campos son obligatorios.";
            return View(usuario);
        }

        if (usuario.NombreUsuario.Length < 4)
        {
            ViewBag.Error = "El nombre de usuario debe tener al menos 4 caracteres.";
            return View(usuario);
        }

        if (usuario.Contrasenia.Length < 6)
        {
            ViewBag.Error = "La contraseña debe tener al menos 6 caracteres.";
            return View(usuario);
        }

        BD bd = new BD();

        if (bd.ExisteUsuario(usuario.NombreUsuario))
        {
            ViewBag.Error = "El nombre de usuario ya existe.";
            return View(usuario);
        }

        bd.RegistrarUsuario(usuario);

        return RedirectToAction("Login");
    }

    public IActionResult Bienvenida()
    {
        string nombreUsuario = HttpContext.Session.GetString("Usuario");

        if (nombreUsuario == null)
        {
            return RedirectToAction("Login");
        }

        BD bd = new BD();
        Usuarios usuario = bd.ObtenerUsuario(nombreUsuario);

        return View(usuario);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

