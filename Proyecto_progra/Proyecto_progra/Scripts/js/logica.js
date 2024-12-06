function Agregar_btn() {
    window.location.href = '/Main/CrearConcierto';
}

function Borrar_btn() {
    window.location.href = '/Main/BorrarConcierto';
}

function Modificar_btn() {
    window.location.href = '/Main/Modificar_Concierto';
}

function Salir_btn() {
    window.location.href = '/Main/MainPage';
}

$(document).ready(function () {
    $('#login_btn').on('click', function (e) {
        e.preventDefault(); 

        const email = $('#email').val().trim();
        const password = $('#password').val().trim();

        if (!email) {
            alert("Por favor, ingrese su correo electrónico.");
            return;
        }

        if (!validateEmail(email)) {
            alert("Por favor, ingrese un correo electrónico válido.");
            return;
        }

        if (!password) {
            alert("Por favor, ingrese su contraseña.");
            return;
        }

        $.ajax({
            url: '/Main/Login',
            type: 'POST',
            data: {
                CORREO: email,
                CLAVE: password
            },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    window.location.href = "/Main/MainPageAdmin";
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error al conectar con el servidor.');
            }
        });
    });
    function validateEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }
});

$(document).ready(function () {
    $('#btnCrearConcierto').click(function (e) {
        e.preventDefault();

        const nombreBanda = $('#nombreBanda').val();
        const generoBanda = $('#generoBanda').val();
        const fechaConcierto = $('#fechaConcierto').val();
        const horaConcierto = $('#horaConcierto').val();
        const pais = $('#pais').val();
        const direccionConcierto = $('#direccionConcierto').val();

        if (!nombreBanda || !generoBanda || !fechaConcierto || !horaConcierto || !pais || !direccionConcierto) {
            alert('Por favor, complete todos los campos.');
            return;
        }

        $.ajax({
            url: '/Main/AgregarConcierto',
            type: 'POST',
            data: {
                nombre_Banda: nombreBanda,
                genero_Musical: generoBanda,
                fecha_Concierto: fechaConcierto,
                hora_Concierto: horaConcierto,
                pais: pais,
                direccion_Concierto: direccionConcierto
            },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    $('#nombreBanda').val('');
                    $('#generoBanda').val('');
                    $('#fechaConcierto').val('');
                    $('#horaConcierto').val('');
                    $('#pais').val('');
                    $('#direccionConcierto').val('');
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error al conectar con el servidor. Por favor, intente de nuevo.');
            }
        });
    });

    //$('#btnRegresar').click(function () {
    //    window.location.href = '/Main/Mainpage';
    //});
});

$(document).ready(function () {
    $.ajax({
        url: '/Main/ObtenerConciertos',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                const conciertos = response.data;
                const tbody = $('#tablaConciertos tbody');
                tbody.empty();

                conciertos.forEach(concierto => {
                    const row = `
                        <tr>
                            <td>${concierto.Nombre_Banda}</td>
                            <td>${concierto.Genero_Musical}</td>
                            <td>${new Date(concierto.Fecha_Concierto).toLocaleDateString()}</td>
                            <td>${concierto.Hora_Concierto}</td>
                            <td>${concierto.Pais}</td>
                            <td>${concierto.Direccion_Concierto}</td>
                        </tr>`;
                    tbody.append(row);
                });
            } else {
                alert(response.message || "No se pudieron cargar los conciertos.");
            }
        },
        error: function () {
            alert("Error al conectar con el servidor.");
        }
    });
});

$(document).ready(function () {
    $('#searchBox').on('keyup', function () {
        const value = $(this).val().toLowerCase();
        $('#tablaConciertos tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});

$(document).ready(function () {
    $("#btnEliminar").click(function () {
        const codigoConcierto = $("#codigoConcierto").val();

        if (!codigoConcierto) {
            $("#mensaje").text("Por favor, ingresa un código válido.").css("color", "red");
            return;
        }

        $.ajax({
            url: '/Main/EliminarConcierto',
            type: 'POST',
            data: JSON.stringify({ id: parseInt(codigoConcierto) }), 
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function () {
                $("#mensaje").text("Ocurrió un error al intentar eliminar el concierto.").css("color", "red");
            }
        });
    });

    $("#btnRegresar").click(function () {
        window.location.href = "/Main/MainpageAdmin";
    });
});

$(document).ready(function () {
    $('#btnModificarConcierto').click(function () {
        const idConcierto = $('#idConcierto').val();
        const nombreBanda = $('#nombreBanda').val();
        const generoBanda = $('#generoBanda').val();
        const fechaConcierto = $('#fechaConcierto').val();
        const horaConcierto = $('#horaConcierto').val();
        const pais = $('#pais').val();
        const direccionConcierto = $('#direccionConcierto').val();

        if (!idConcierto || !nombreBanda || !generoBanda || !fechaConcierto || !horaConcierto || !pais || !direccionConcierto) {
            alert("Por favor, completa todos los campos.");
            return;
        }

        $.ajax({
            url: '/Main/ModificarConcierto',
            type: 'POST',
            data: {
                idConcierto: idConcierto,
                nombreBanda: nombreBanda,
                generoBanda: generoBanda,
                fechaConcierto: fechaConcierto,
                horaConcierto: horaConcierto,
                pais: pais,
                direccionConcierto: direccionConcierto
            },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Ocurrió un error al realizar la solicitud: " + error);
            }
        });
    });
});