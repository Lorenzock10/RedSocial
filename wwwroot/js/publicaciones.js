function darMeGusta(idPublicacion)
{
    fetch('/Publicacion/DarMeGusta?idPublicacion=' + idPublicacion, {method: 'POST', headers: 
        {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {

        if (data.error)
        {
            alert(data.error);
            return;
        }

        document.getElementById("likes-" + idPublicacion).innerHTML = data.cantidadLikes;

        // Actualizar texto del botón Me Gusta según el estado devuelto por el servidor
        try {
            const publicacionElem = document.querySelector(".publicacion[data-id='" + idPublicacion + "']");
            if (publicacionElem) {
                const boton = publicacionElem.querySelector('button');
                if (boton) {
                    boton.innerText = data.meGusta ? 'Ya no me gusta' : 'Me Gusta';
                }
            }
        } catch (e) {
            console.error(e);
        }
    })
    .catch((error) => {
        console.error('Error:', error);
    });
}


function comentar(idPublicacion)
{
    let texto = document.getElementById("texto-" + idPublicacion).value;

    fetch('/Publicacion/Comentar?idPublicacion=' + idPublicacion + '&texto=' + encodeURIComponent(texto), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {

        if (data.error)
        {
            alert(data.error);
            return;
        }

        let comentarios = document.getElementById("comentarios-" + idPublicacion);

        let nuevoComentario =
            "<div class='comentario'>" +
            "<strong>" + data.nombreUsuario + "</strong>" +
            "<p>" + data.texto + "</p>" +
            "<small>" + data.fechaComentario + "</small>" +
            "</div>";

        comentarios.innerHTML += nuevoComentario;

        document.getElementById("texto-" + idPublicacion).value = "";
    })
    .catch((error) => {
        console.error('Error:', error);
    });
}


function obtenerMas()
{
    let cantidadPublicaciones =
        document.querySelectorAll(".publicacion").length;

    fetch('/Publicacion/ObtenerMas?desde=' + cantidadPublicaciones, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {

        let publicaciones =
            document.getElementById("publicaciones");

        data.publicaciones.forEach(publicacion => {

            let comentarios = "";

            publicacion.comentarios.forEach(comentario => {

                comentarios +=
                    "<div class='comentario'>" +
                    "<strong>" + comentario.nombreUsuario + "</strong>" +
                    "<p>" + comentario.texto + "</p>" +
                    "<small>" + comentario.fechaComentario + "</small>" +
                    "</div>";

            });

            let imagen = "";

            if (publicacion.imagen != null && publicacion.imagen != "")
            {
                imagen =
                    "<img src='/imagenes/" +
                    publicacion.imagen +
                    "' width='300' />";
            }

            let nuevaPublicacion =
                "<div class='publicacion' data-id='" + publicacion.id + "'>" +

                "<h2>" + publicacion.titulo + "</h2>" +

                "<p>" + publicacion.descripcion + "</p>" +

                "<p>Publicado por: <strong>" +
                publicacion.nombreUsuario +
                "</strong></p>" +

                "<p>" + publicacion.fechaPublicacion + "</p>" +

                imagen +

                "<br>" +

                "<button onclick='darMeGusta(" +
                publicacion.id +
                ")'>Me Gusta</button>" +

                "<span id='likes-" +
                publicacion.id +
                "'>" +
                publicacion.cantidadMeGusta +
                "</span>" +

                "<h3>Comentarios</h3>" +

                "<div id='comentarios-" +
                publicacion.id +
                "'>" +
                comentarios +
                "</div>" +

                "<br>" +

                "<input type='text' id='texto-" +
                publicacion.id +
                "' placeholder='Escribí un comentario...'>" +

                "<button onclick='comentar(" +
                publicacion.id +
                ")'>Comentar</button>" +

                "<hr>" +

                "</div>";

            publicaciones.innerHTML += nuevaPublicacion;
        });

        if (!data.hayMas)
        {
            document.getElementById("btnVerMas").style.display = "none";
        }
    })
    .catch((error) => {
        console.error('Error:', error);
    });
}