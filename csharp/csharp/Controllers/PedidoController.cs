using csharp.dtos;
using csharp.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace csharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{ 
    private readonly IPedidoService _pedidoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(IPedidoService pedidoService, ILogger<PedidoController> logger)
    {
        _pedidoService = pedidoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Import([FromBody] PedidoDto pedido)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        try
        {
            var created = await _pedidoService.Salvar(pedido);
            return Ok($"Import order of id {created.Id}");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Erro ao importar pedido: {pedido}", pedido);
            return Problem(statusCode:500,title:"Erro interno",detail: $"Erro ao importar pedido: {pedido}");
        }      
    }
}