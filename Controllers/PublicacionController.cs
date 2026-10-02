using Microsoft.AspNetCore.Mvc;
using RedSocial.Models;

namespace TP07.Controllers;

public class PublicacionController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("IdUsuario") == null)
        {
            return RedirectToAction("Registro", "Usuarios");
        }

        BD bd = new BD();

        List<Publicacion> publicaciones = bd.ObtenerPublicaciones(0);

        foreach (Publicacion publicacion in publicaciones)
        {
            publicacion.Comentarios = bd.ObtenerComentarios(publicacion.Id);
        }

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

        if (!bd.ExistePublicacion(idPublicacion))
        {
            return Json(new
            {
                error = "La publicación no existe."
            });
        }

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

        return Json(new
        {
            cantidadLikes = cantidadLikes,
            meGusta = !yaDioMeGusta
        });
    }

    [HttpPost]
    public IActionResult Comentar(int idPublicacion, string texto)
    {
        int idUsuario = HttpContext.Session.GetInt32("IdUsuario").Value;

        if (string.IsNullOrWhiteSpace(texto))
        {
            return Json(new
            {
                error = "El comentario no puede estar vacío."
            });
        }

        BD bd = new BD();

        if (!bd.ExistePublicacion(idPublicacion))
        {
            return Json(new
            {
                error = "La publicación no existe."
            });
        }

        bd.AgregarComentario(idPublicacion, idUsuario, texto);

        Comentarios comentario = bd.ObtenerUltimoComentario(idPublicacion, idUsuario);

        return Json(new
        {
            id = comentario.Id,
            nombreUsuario = comentario.NombreUsuario,
            texto = comentario.Texto,
            fechaComentario = comentario.FechaComentario
        });
    }

    [HttpGet]
    public IActionResult ObtenerMas(int desde)
    {
        BD bd = new BD();

        List<Publicacion> publicaciones = bd.ObtenerPublicaciones(desde);

        foreach (Publicacion publicacion in publicaciones)
        {
            publicacion.Comentarios = bd.ObtenerComentarios(publicacion.Id);
        }

        bool hayMas = bd.HayMasPublicaciones(desde + publicaciones.Count);

        return Json(new
        {
            publicaciones = publicaciones,
            hayMas = hayMas
        });
    }
}