using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.ViewModel
{
    public class PedidoViewModel
    {
        public Pedido oPedido { get; set; }
        public List<PedidoDetalle> PedidoDetalle { get; set; }
        public Producto? Producto { get; set; }
    }
}
