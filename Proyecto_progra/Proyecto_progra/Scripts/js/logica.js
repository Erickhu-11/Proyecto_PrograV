// login.js
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
