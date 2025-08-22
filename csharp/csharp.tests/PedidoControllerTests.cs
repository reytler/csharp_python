using csharp.dtos;
using csharp.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace csharp.tests;
public class PedidoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PedidoControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Import_Pedido_Valido_Should_Return_Ok()
    {
        var pedido = new Pedido
        {
            Id = 1,
            Valor = 100.50m
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Pedido", pedido);

        // Assert
        response.EnsureSuccessStatusCode(); // 200 OK
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Pedido importado com sucesso");
    }

    [Fact]
    public async Task Salvar_Deve_Adicionar_Pedido()
    {
        var service = new PedidoService();
        var pedido = new Pedido { Valor = 100 };

        var resultado = await service.Salvar(pedido);
        resultado.Should().NotBeNull();

        var lista = await service.Listar();
        lista.Should().Contain(p => p.Valor == 100);
    }
}
