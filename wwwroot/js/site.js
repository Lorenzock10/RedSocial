function validarRegistro() {

    let usuario = document.getElementById("NombreUsuario").value;
    let contrasenia = document.getElementById("Contrasenia").value;
    let nombre = document.getElementById("Nombre").value;
    let apellido = document.getElementById("Apellido").value;

    if (usuario == "" || contrasenia == "" || nombre == "" || apellido == "") {
        alert("Todos los campos son obligatorios.");
        return false;
    }

    if (usuario.length < 4) {
        alert("El nombre de usuario debe tener al menos 4 caracteres.");
        return false;
    }

    if (contrasenia.length < 6) {
        alert("La contraseña debe tener al menos 6 caracteres.");
        return false;
    }

    for(let i = 0; i < nombre.length; i++){
    if(nombre[i] >= "0" && nombre[i] <= "9"){
        alert("El nombre no puede tener números.");
        return false;
    }
    }

    for(let i = 0; i < apellido.length; i++){
    if(apellido[i] >= "0" && apellido[i] <= "9"){
        alert("El apellido no puede tener números.");
        return false;
    }
    }

    return true;
}
