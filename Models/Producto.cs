using System;
using System.Collections.Generic;

namespace PruebaTecnicaBcp.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? Sku { get; set; }

    public string? Nombre { get; set; }

    public short? IdTipo { get; set; }

    public string? Etiqueta { get; set; }

    public decimal? Precio { get; set; }

    public int? Stock { get; set; }

    public short? IdEstado { get; set; }

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
