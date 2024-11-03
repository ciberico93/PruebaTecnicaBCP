
function ActualizarUsuario() {

    // Realizar la solicitud POST
    $.post('/Usuario/ActualizarUsuario', $('#FrmEditarUsuario').serialize(), function (response) {
        console.log(response);

        if (response.success) {
            // Mostrar alerta de éxito
            Swal.fire({
                icon: 'success',
                title: 'Sistema Ventas',
                text: 'El usuario se actualizó exitosamente',
                confirmButtonText: 'Aceptar'
            }).then(function () {
                // Redirigir o realizar acciones adicionales si es necesario
                window.location.href = '/Usuario/FrmUsuario';
            });
        } else {
            // Mostrar alerta de error si la operación falló
            Swal.fire({
                icon: 'error',
                title: 'Sistema Ventas',
                text: response.message || 'Hubo un error al actualizar el usuario',
                confirmButtonText: 'Aceptar'
            });
        }

        // Ocultar el spinner de carga
        $('.indicator-progress').addClass('d-none');
        $('.indicator-label').removeClass('d-none');
    }).fail(function () {
        // Mostrar alerta de error genérico si la solicitud falla
        Swal.fire({
            icon: 'error',
            title: 'Sistema Ventas',
            text: 'Hubo un error al enviar la solicitud',
            confirmButtonText: 'Aceptar'
        });

        // Ocultar el spinner de carga
        $('.indicator-progress').addClass('d-none');
        $('.indicator-label').removeClass('d-none');
    });
}

$(document).ready(function () {
    $('#habilitarContrasena').on('change', function () {
        $('#contrasena').prop('disabled', !this.checked); // Habilita o deshabilita el campo de contraseña
    });
});