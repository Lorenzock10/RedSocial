using Microsoft.AspNetCore.Mvc;
using RedSocial.Models;

namespace RedSocial.Controllers;

public class PublicacionController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("RedSocial", "Home");
    }

    [HttpPost]
    public IActionResult CrearPublicacion(string titulo, string descripcion, IFormFile imagen)
    {
        int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

        if (!idUsuario.HasValue)
        {
            return RedirectToAction("Login", "Usuarios");
        }

        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(descripcion))
        {
            TempData["ErrorPublicacion"] = "El título y la descripción son obligatorios.";
            return RedirectToAction("RedSocial", "Home");
        }

        string nombreArchivo = "sin-imagen.jpg";

        if (imagen != null && imagen.Length > 0)
        {
            string carpetaUploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(carpetaUploads);

            string extension = Path.GetExtension(imagen.FileName);
            nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string rutaCompleta = Path.Combine(carpetaUploads, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                imagen.CopyTo(stream);
            }
        }

        BD bd = new BD();
        bd.CrearPublicacion(titulo.Trim(), descripcion.Trim(), nombreArchivo, idUsuario.Value);

        TempData["MensajePublicacion"] = "Publicación creada correctamente.";
        return RedirectToAction("RedSocial", "Home");
    }
}