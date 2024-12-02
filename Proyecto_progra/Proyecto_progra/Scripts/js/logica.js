$(document).ready(function () {
    $('#login_btn').on('click', function (e) {
        e.preventDefault(); // Evitar el comportamiento por defecto del formulario

        // Obtener los valores de los campos
        const email = $('#email').val().trim();
        const password = $('#password').val().trim();

        // Validar el correo y la contraseña
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

        // Enviar los datos al servidor
        $.ajax({
            url: '/Main/Login', // URL del método en tu controlador
            type: 'POST',
            data: {
                CORREO: email,
                CLAVE: password
            },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    // Redirigir al usuario si el login es exitoso
                    window.location.href = "/Main/CrearConcierto";
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error al conectar con el servidor.');
            }
        });
    });

    // Validar formato del correo electrónico
    function validateEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }
});

$(document).ready(function () {
    // Evento para el botón "Crear Concierto"
    $('#btnCrearConcierto').click(function (e) {
        e.preventDefault(); // Evita el comportamiento predeterminado del botón

        // Capturar los valores de los campos
        const nombreBanda = $('#nombreBanda').val();
        const generoBanda = $('#generoBanda').val();
        const fechaConcierto = $('#fechaConcierto').val();
        const horaConcierto = $('#horaConcierto').val();
        const pais = $('#pais').val();
        const direccionConcierto = $('#direccionConcierto').val();

        // Validar que todos los campos estén llenos
        if (!nombreBanda || !generoBanda || !fechaConcierto || !horaConcierto || !pais || !direccionConcierto) {
            alert('Por favor, complete todos los campos.');
            return;
        }

        // Enviar los datos al servidor mediante AJAX
        $.ajax({
            url: '/Main/AgregarConcierto', // Ruta del controlador
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
                    alert(response.message); // Mostrar mensaje de éxito

                    // Limpiar los campos del formulario
                    $('#nombreBanda').val('');
                    $('#generoBanda').val('');
                    $('#fechaConcierto').val('');
                    $('#horaConcierto').val('');
                    $('#pais').val('');
                    $('#direccionConcierto').val('');
                } else {
                    alert(response.message); // Mostrar mensaje de error del servidor
                }
            },
            error: function () {
                alert('Error al conectar con el servidor. Por favor, intente de nuevo.');
            }
        });
    });

    // Evento para el botón "Regresar"
    $('#btnRegresar').click(function () {
        window.location.href = '/Main/Mainpage'; // Redirigir a la página principal
    });
});

$(document).ready(function () {
    // Obtener la lista de conciertos
    $.ajax({
        url: '/Main/ObtenerConciertos', // Ruta al método en el controlador
        type: 'GET',
        success: function (response) {
            if (response.success) {
                const conciertos = response.data;
                const tbody = $('#tablaConciertos tbody');
                tbody.empty(); // Limpiar la tabla antes de insertar datos

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
    // Evento para filtrar la tabla en tiempo real
    $('#searchBox').on('keyup', function () {
        const value = $(this).val().toLowerCase();
        $('#tablaConciertos tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});

$(document).ready(function () {
    // Evento para el botón "Eliminar"
    $('#btnEliminar').click(function (e) {
        e.preventDefault(); // Evita el comportamiento predeterminado

        // Obtener el código del concierto
        const codigoConcierto = $('#codigoConcierto').val().trim();

        if (!codigoConcierto) {
            alert("Por favor, ingrese el código del concierto.");
            return;
        }

        // Enviar la solicitud de eliminación
        $.ajax({
            url: '/Main/EliminarConcierto', // URL del método en tu controlador
            type: 'POST',
            data: {
                codigo: codigoConcierto
            },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    $('#codigoConcierto').val(''); // Limpiar el campo después de eliminar
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error al conectar con el servidor. Por favor, intente de nuevo.');
            }
        });
    });

    // Evento para el botón "Regresar"
    $('#btnRegresar').click(function () {
        window.location.href = '/Main/Mainpage'; // Redirigir a la página principal
    });
});


