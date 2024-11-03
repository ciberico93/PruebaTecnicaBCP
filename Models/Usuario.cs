using System;
using System.Collections.Generic;

namespace PruebaTecnicaBcp.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string CodigoTrabajador { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Contrasena { get; set; }

    public string? Email { get; set; }

    public string? TelefonoCelular { get; set; }

    public string? Puesto { get; set; }

    public short? IdRol { get; set; }

    public short? IdEstado { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Pedido> PedidoIdDeliveryNavigations { get; set; } = new List<Pedido>();

    public virtual ICollection<Pedido> PedidoIdVendedorNavigations { get; set; } = new List<Pedido>();
}
