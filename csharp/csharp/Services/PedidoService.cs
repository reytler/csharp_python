using csharp.dtos;

namespace csharp.Services;

public class PedidoService : IPedidoService
{
    private readonly ILogger<PedidoService> _logger;

    private static readonly List<PedidoDto> _pedidos = new();

    public PedidoService(ILogger<PedidoService> logger)
    {
        _logger = logger;
    }


    /// <summary>
    /// Importa um pedido e retorna o pedido i´mportado.
    /// </summary>
    public async Task<PedidoDto> Salvar(PedidoDto pedido)
    {
        await Task.Delay(50); 
        _pedidos.Add(pedido);
        _logger.LogInformation("Pedido {PedidoId} salvo com sucesso.", pedido.Id);

        return pedido;
    }
}