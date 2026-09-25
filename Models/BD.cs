using Dapper;
using Microsoft.Data.SqlClient;

namespace RedSocial.Models;

public class BD
{


private string _connectionString = @"Server=localhost; DataBase = DBRedSocial; Integrated Security = True; TrustServerCertificate = True;";


public Usuarios ObtenerUsuario(string nombreUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
        return connection.QueryFirstOrDefault<Usuarios>(query, new { NombreUsuario = nombreUsuario });
    }
}



public void RegistrarUsuario(Usuarios usuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"INSERT INTO Usuarios (NombreUsuario, Contraseña, Nombre, Apellido) VALUES (@NombreUsuario, @Contrasenia, @Nombre, @Apellido)";

        connection.Execute(query, new
        {
            NombreUsuario = usuario.NombreUsuario,
            Contrasenia = usuario.Contrasenia,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido
        });
    }
}



public bool ExisteUsuario(string nombreUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"SELECT TOP 1 1 FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
        int? existe = connection.ExecuteScalar<int?>(query, new { NombreUsuario = nombreUsuario });
        return existe.HasValue && existe.Value == 1;
    }
}


public void CrearPublicacion(string titulo, string descripcion, string imagen, int idUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"INSERT INTO Publicaciones (IdUsuario, Titulo, Descripcion, Imagen, FechaPublicacion) VALUES (@IdUsuario, @Titulo, @Descripcion, @Imagen, GETDATE())";

        connection.Execute(query, new
        {
            IdUsuario = idUsuario,
            Titulo = titulo,
            Descripcion = descripcion,
            Imagen = imagen
        });
    }
}


public List<Publicacion> ObtenerPublicaciones(int desde)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"SELECT * FROM Publicaciones ORDER BY FechaPublicacion DESC OFFSET @Desde ROWS FETCH NEXT 10 ROWS ONLY";

        return connection.Query<Publicacion>(query, new
        {
            Desde = desde
        }).ToList();
    }
}


public bool ExisteMeGusta(int idPublicacion, int idUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"SELECT TOP 1 1 FROM PublicacionesMeGusta WHERE IdPublicación = @IdPublicacion AND IdUsuario = @IdUsuario";

        int? existe = connection.ExecuteScalar<int?>(query, new
        {
            IdPublicacion = idPublicacion,
            IdUsuario = idUsuario
        });

        return existe.HasValue;
    }
}


public void AgregarMeGusta(int idPublicacion, int idUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"INSERT INTO PublicacionesMeGusta (IdPublicación, IdUsuario) VALUES (@IdPublicacion, @IdUsuario)";

        connection.Execute(query, new
        {
            IdPublicacion = idPublicacion,
            IdUsuario = idUsuario
        });
    }
}


public void EliminarMeGusta(int idPublicacion, int idUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"DELETE FROM PublicacionesMeGusta WHERE IdPublicación = @IdPublicacion AND IdUsuario = @IdUsuario";

        connection.Execute(query, new
        {
            IdPublicacion = idPublicacion,
            IdUsuario = idUsuario
        });
    }
}


public int ObtenerCantidadMeGusta(int idPublicacion)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"SELECT COUNT(*) FROM PublicacionesMeGusta WHERE IdPublicación = @IdPublicacion";

        return connection.ExecuteScalar<int>(query, new
        {
            IdPublicacion = idPublicacion
        });
    }
}


public void AgregarComentario(int idPublicacion, int idUsuario, string texto)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"INSERT INTO Comentarios (IdPublicacion, IdUsuarioComenta, Texto, FechaComentario) VALUES (@IdPublicacion, @IdUsuario, @Texto, GETDATE())";

        connection.Execute(query, new
        {
            IdPublicacion = idPublicacion,
            IdUsuario = idUsuario,
            Texto = texto
        });
    }
}


public string ObtenerNombreUsuario(int idUsuario)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"SELECT NombreUsuario FROM Usuarios WHERE Id = @IdUsuario";

        return connection.ExecuteScalar<string>(query, new
        {
            IdUsuario = idUsuario
        });
    }
}


}