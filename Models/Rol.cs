using System;
using System.Collections.Generic;

namespace PruebaTecnicaBcp.Models;

public partial class Rol
{
    public short IdRol { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
