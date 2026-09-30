using Microsoft.AspNetCore.Mvc;
using RedSocial.Models;

namespace TP07.Controllers;

public class PublicacionController : Controller
{
    public IActionResult Index()
    {
        BD bd = new BD();
        List<Publicacion> publicaciones = bd.ObtenerPublicaciones(0);

        return View(publicaciones);
    }

    [HttpPost]
    public IActionResult CrearPublicacion(string titulo, string descripcion, string imagen)
    {
        int idUsuario = HttpContext.Session.GetInt32("IdUsuario").Value;

        BD bd = new BD();
        bd.CrearPublicacion(titulo, descripcion, imagen, idUsuario);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DarMeGusta(int idPublicacion)
    {
        int idUsuario = HttpContext.Session.GetInt32("IdUsuario").Value;

        BD bd = new BD();
        bool yaDioMeGusta = bd.ExisteMeGusta(idPublicacion, idUsuario);

        if (yaDioMeGusta)
        {
            bd.EliminarMeGusta(idPublicacion, idUsuario);
        }
        else
        {
            bd.AgregarMeGusta(idPublicacion, idUsuario);
        }

        int cantidadLikes = bd.ObtenerCantidadMeGusta(idPublicacion);

        ViewBag.CantidadLikes = cantidadLikes;

        return View();
    }

    [HttpPost]
    public IActionResult Comentar(int idPublicacion, string texto)
    {
        int idUsuario = HttpContext.Session.GetInt32("IdUsuario").Value;

        BD bd = new BD();
        bd.AgregarComentario(idPublicacion, idUsuario, texto);

        string nombreUsuario = bd.ObtenerNombreUsuario(idUsuario);

        ViewBag.NombreUsuario = nombreUsuario;
        ViewBag.Texto = texto;

        return View();
    }

    [HttpGet]
    public IActionResult ObtenerMas(int desde)
    {
        BD bd = new BD();
        List<Publicacion> publicaciones = bd.ObtenerPublicaciones(desde);

        return View(publicaciones);
    }
}