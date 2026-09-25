namespace RedSocial.Models;

    public class Publicacion
{
    public int Id { get; set; }
    public string Imagen { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public string IdUsuario { get; set; }
    public string Fecha { get; set; }
}