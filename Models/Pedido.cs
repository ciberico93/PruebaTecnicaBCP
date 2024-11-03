using System;
using System.Collections.Generic;

namespace PruebaTecnicaBcp.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public string? NroPedido { get; set; }

    public DateTime? FechaPedido { get; set; }

    public DateTime? FechaDespacho { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public int? IdVendedor { get; set; }

    public int? IdDelivery { get; set; }

    public decimal? Total { get; set; }

    public short? IdEstadoPedido { get; set; }

    public short? IdEstado { get; set; }

    public virtual Usuario? IdDeliveryNavigation { get; set; }

    public virtual EstadoPedido? IdEstadoPedidoNavigation { get; set; }

    public virtual Usuario? IdVendedorNavigation { get; set; }

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
