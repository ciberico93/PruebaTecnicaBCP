function listaUsuario() {
    if ($.fn.DataTable.isDataTable('#tbdata')) {
        $('#tbdata').DataTable().destroy();
    }

    var table = $('#tbdata').DataTable({
        searching: true,
    });

    // Detecta el cambio en el campo de búsqueda global de DataTables
    $('.dataTables_filter input').on('input', function () {
        var valorBusqueda = $(this).val(); // Valor del campo de búsqueda global
        var filtrarPorDescripcion = $('#filtroDescripcionCheck').is(':checked');

        if (filtrarPorDescripcion) {
            // Limpia cualquier filtro previo en otras columnas y aplica solo a la columna Descripción
            table.columns().search(''); // Limpia búsquedas previas
            table.column(6).search(valorBusqueda).draw(); // Columna 2 es Descripción
        } else {
            // Si el checkbox no está marcado, realiza la búsqueda en todas las columnas
            table.search(valorBusqueda).draw();
        }
    });

    // Evento change para el checkbox para actualizar el filtro de inmediato
    $('#filtroDescripcionCheck').on('change', function () {
        $('.dataTables_filter input').trigger('input'); // Simula la entrada en el campo de búsqueda
    });
}

function EditarUsuario(id) {
    var url = '/Usuario/FrmEditarUsuario' + '/?id=' + id;
    window.location.href = url;
}

/*
 * delete row
 */
function EliminarUsuario(id) {
    Swal.fire({
        icon: 'warning',
        title: 'Sistema BCP',
        text: '¿Desea Eliminar el registro?',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: "Sí",
        cancelButtonText: "No",
        showCancelButton: true,
    }).then((result) => {
        if (result.isConfirmed) {
            // Realizar la eliminación solo si el usuario confirma
            $.post('/Usuario/EliminarUsuario', { id }, function (data) {
                console.log(data);
            });
            Swal.fire(
                {
                    icon: "success",
                    title: "Sistema BCP",
                    text: "El registro se eliminó correctamente"
                }).then(function (t) {
                    if (t.isConfirmed) {
                        window.location.href = '/Usuario/FrmUsuario';
                    }
                }
                );
        }
    });
}

$(document).ready(function () {
    listaUsuario();
});
