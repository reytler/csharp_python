using csharp.dtos;

namespace csharp.Services
{
    public interface IPedidoService
    {
        Task<PedidoDto> Salvar(PedidoDto dto);
    }
}
