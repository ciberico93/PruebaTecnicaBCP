using System;
using System.Collections.Generic;

namespace PruebaTecnicaBcp.Models;

public partial class PedidoDetalle
{
    public int IdPedidoDetalle { get; set; }

    public int? IdPedido { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? TotalPrecio { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
