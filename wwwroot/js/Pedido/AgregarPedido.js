let _idDelivery = 0;
function MostrarProductos() {
    $.ajax({
        url: '/Pedido/MostrarProductos', // Ruta del controlador y acción
        type: 'GET',
        dataType: 'json',
        success: function (productos) {
            // Limpiar el select por si ya tiene opciones
            console.log(productos);
            $('#cboBuscarProducto').empty();
            // Agregar una opción predeterminada
            $('#cboBuscarProducto').append('<option value="">Seleccione un producto</option>');

            // Iterar sobre los productos y agregarlos al select
            $.each(productos, function (index, producto) {
                $('#cboBuscarProducto').append(
                    $('<option></option>')
                        .val(producto.idProducto) // Cambia 'id' si tu modelo tiene otro nombre de propiedad
                        .data('precio', producto.precio)
                        .text(producto.nombre) // Cambia 'nombre' si tu modelo tiene otro nombre de propiedad
                );
            });

            // Inicializar Select2 en el select después de llenar los datos
            $('#cboBuscarProducto').select2({
                placeholder: 'Seleccione un producto',
                allowClear: true
            });
        },
        error: function () {
            alert('Hubo un error al cargar los productos.');
        }
    });
}

function MostrarDelivery() {
    $.ajax({
        url: '/Pedido/MostrarDelivery', // Ruta del controlador y acción
        type: 'GET',
        dataType: 'json',
        success: function (deliverys) {
            /*debugger;*/
            // Limpiar el select por si ya tiene opciones
            console.log(deliverys);
            $('#cboBuscarDelivery').empty();
            // Agregar una opción predeterminada
            $('#cboBuscarDelivery').append('<option value="">Seleccione un delivery</option>');

            // Iterar sobre los productos y agregarlos al select
            $.each(deliverys, function (index, delivery) {
                $('#cboBuscarDelivery').append(
                    $('<option></option>')
                        .val(delivery.idUsuario) // Cambia 'id' si tu modelo tiene otro nombre de propiedad
                        .text(delivery.nombre) // Cambia 'nombre' si tu modelo tiene otro nombre de propiedad
                );
            });

            // Inicializar Select2 en el select después de llenar los datos
            $('#cboBuscarDelivery').select2({
                placeholder: 'Seleccione un delivery',
                allowClear: true
            });
        },
        error: function () {
            alert('Hubo un error al cargar los deliverys.');
        }
    });
}

// Función para agregar producto a la tabla
function AgregarProductos() {
    var productoId = $('#cboBuscarProducto').val();
    var productoNombre = $('#cboBuscarProducto option:selected').text();
    var precio = $('#cboBuscarProducto option:selected').data('precio');
    var cantidad = parseInt($('#cantidad').val(), 10);

    if (!productoId || !cantidad || cantidad <= 0) {
        Swal.fire({
            icon: 'warning',
            title: 'Error',
            text: 'Seleccione un producto y una cantidad válida.',
            confirmButtonText: 'Aceptar'
        });
        return;
    }

    var total = precio * cantidad;

    // Crear fila y agregarla a la tabla
    var fila = `
        <tr>
            <td><button type="button" class="btn btn-danger btn-sm btnEliminar">X</button></td>
            <td class="idProducto">${productoId}</td> <!-- Clase para ID producto -->
            <td>${productoNombre}</td>
            <td class="cantidad">${cantidad}</td> <!-- Clase para cantidad -->
            <td class="precio">${precio.toFixed(2)}</td> <!-- Clase para precio -->
            <td class="subtotal">${total.toFixed(2)}</td> <!-- Clase para subtotal -->
        </tr>
    `;
    $('#tbProducto tbody').append(fila);

    // Llama a la función para actualizar el total acumulado
    actualizarTotalAcumulado();
}

// Función para actualizar el total acumulado
function actualizarTotalAcumulado() {
    var totalAcumulado = 0;

    // Itera sobre cada fila de la tabla y suma los subtotales
    $('#tbProducto tbody tr').each(function () {
        var subtotal = parseFloat($(this).find('.subtotal').text());
        totalAcumulado += subtotal;
    });

    // Asigna el total acumulado al campo de entrada
    $('#txtTotal').val(totalAcumulado.toFixed(2));
}

// Event listener para eliminar filas y actualizar el total
$('#tbProducto').on('click', '.btnEliminar', function () {
    $(this).closest('tr').remove();
    actualizarTotalAcumulado();
});

// Función para eliminar fila de la tabla
function eliminarFila() {
    $(this).closest('tr').remove();
}

function TerminarPedido() {
    // Obtener datos del pedido
    let idVendedor = parseInt($('#IdVendedor').val()) || 0;
    let idDelivery = _idDelivery || 0; // Asegúrate de que _idDelivery tenga el valor correcto
    let total = parseFloat($('#txtTotal').val()) || 0;

    // Inicializar el array de detalles de pedido
    let PedidoDetalle = []; // Asegúrate de que PedidoDetalle esté definido como un array

    // Construir el objeto del pedido
    let oPedidoVM = {
        oPedido: {
            NroPedido: null,
            FechaPedido: null,
            FechaDespacho: null,
            FechaEntrega: null,
            IdVendedor: idVendedor,
            IdDelivery: idDelivery,
            Total: parseFloat(total.toFixed(2)) || 0,
            IdEstadoPedido: 1, // Estado inicial
            IdEstado: 1         // Estado activo
        },
        PedidoDetalle: PedidoDetalle // Asignar el array inicializado aquí
    };

    // Recorrer las filas de la tabla de productos y agregar cada detalle
    $('#tbProducto tbody tr').each(function () {
        let idProducto = parseInt($(this).find('.idProducto').text()) || 0; // Obtener el ID del producto
        let cantidad = parseInt($(this).find('.cantidad').text()) || 0; // Obtener la cantidad
        let totalPrecio = parseFloat($(this).find('.subtotal').text()) || 0; // Obtener el subtotal

        // Añadir el detalle al array PedidoDetalle
        PedidoDetalle.push({
            IdProducto: idProducto,
            Cantidad: cantidad,
            TotalPrecio: parseFloat(totalPrecio.toFixed(2))
        });
    });

    console.log(oPedidoVM);

    // Enviar el objeto oPedidoVM al servidor mediante AJAX (descomentar para usar)
    
    $.ajax({
        url: '/Pedido/FrmNuevoPedido', // Cambia esta URL según tu ruta en el servidor
        type: 'POST',
        async: false,
        contentType: 'application/json',
        data: JSON.stringify(oPedidoVM),
        success: function (response) {
            // Manejo exitoso de la respuesta
            Swal.fire({
                icon: 'success',
                title: 'Pedido Terminado',
                text: 'El pedido se ha enviado correctamente.',
                confirmButtonText: 'Aceptar'
            }).then(() => {
                window.location.href = '/Pedido/FrmNuevoPedido'; // Redirigir después de la confirmación
            });
            // Aquí puedes agregar lógica para limpiar el formulario o redirigir
        },
        error: function (error) {
            alert('Hubo un error al terminar el pedido.');
            console.error(error);
        }
    });
    
}

// Captura el evento de cambio en el select
$('#cboBuscarDelivery').on('change', function () {
    // Obtén el ID del delivery seleccionado
     _idDelivery = parseInt($(this).val());
});


$(document).ready(function () {
    MostrarProductos();
    MostrarDelivery();
});