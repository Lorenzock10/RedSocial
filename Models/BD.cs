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

}