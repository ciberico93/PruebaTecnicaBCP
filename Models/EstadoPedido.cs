using System;
using System.Collections.Generic;

namespace PruebaTecnicaBcp.Models;

public partial class EstadoPedido
{
    public short IdEstadoPedido { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
