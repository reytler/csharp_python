using csharp.dtos;
using csharp.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace csharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{ 
    private readonly PedidoService _pedidoService;

    public PedidoController(PedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Import([FromBody] Pedido pedido)
    {
        try
        {
            var pedidoImportado = await _pedidoService.Salvar(pedido);
            return Ok("Pedido importado com sucesso");
        }
        catch
        {
            return BadRequest("Erro ao importar pedido");
        }      
    }
}