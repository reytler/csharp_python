using csharp.dtos;

namespace csharp.Services;

public class PedidoService
{
    private static readonly List<Pedido> _pedidos = new();

    /// <summary>
    /// Salva um pedido e retorna o pedido criado.
    /// </summary>
    public Task<Pedido> Salvar(Pedido pedido)
    {
        _pedidos.Add(pedido);
        return Task.FromResult(pedido);
    }
    
    /// <summary>
    /// Retorna todos os pedidos salvos.
    /// </summary>
    public Task<List<Pedido>> Listar() => Task.FromResult(_pedidos);
}