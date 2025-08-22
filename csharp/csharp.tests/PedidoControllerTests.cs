using csharp.dtos;
using csharp.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace csharp.tests;
public class PedidoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PedidoControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Deve importar o pedido corretamente")]
    public async Task Import_Pedido_Valido_Should_Return_Ok()
    {
        var pedido = new PedidoDto
        {
            Id = 1,
            Valor = 100.50m
        };

        var response = await _client.PostAsJsonAsync("/api/Pedido", pedido);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(content, $"Import order of id {pedido.Id}");
    }

    [Fact(DisplayName = "Deve recusar o pedido")]
    public async Task Import_Pedido_Valido_Should_Return_BadRequest()
    {
        var pedido = new PedidoDto
        {
            Id = null,
            Valor = 100.50m
        };

        var response = await _client.PostAsJsonAsync("/api/Pedido", pedido);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "Deve salvar o pedido no service corretamente.")]
    public async Task Salvar_Deve_Adicionar_Pedido()
    {
        var service = new PedidoService(NullLogger<PedidoService>.Instance);
        var pedido = new PedidoDto {Id= 1, Valor = 100 };

        var resultado = await service.Salvar(pedido);
        Assert.NotEqual(default, pedido.Id);
        Assert.Equal(resultado.Id, pedido.Id);
    }
}
