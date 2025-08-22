# Como eu implementaria a emissão de uma mensagem de evento assíncrona (sem usar bibliotecas prontas).


A ideia é criar um **EventBus interno** simples que permite que múltiplos consumidores sejam notificados de forma assíncrona sempre que um pedido é importado.

---

## 1. Conceito

Quando um pedido é importado:

1. O serviço salva o pedido no armazenamento (memória ou banco de dados).  
2. Em seguida, dispara um evento (`PedidoImportadoEvent`) que contém os dados do pedido.  
3. Consumidores registrados no sistema processam o evento de forma assíncrona.

---

## 2. Estrutura básica da lógica

### a) Evento de domínio

```csharp
public class PedidoImportadoEvent
{
    public long PedidoId { get; set; }
    public decimal Valor { get; set; }
}
```

### b) Criar um EventBus simples

```csharp
public class SimpleEventBus
{
    private readonly List<Func<PedidoImportadoEvent, Task>> _handlers = new();

    public void Subscribe(Func<PedidoImportadoEvent, Task> handler)
    {
        _handlers.Add(handler);
    }

    public async Task Publish(PedidoImportadoEvent evento)
    {
        foreach (var handler in _handlers)
        {
            _ = Task.Run(() => handler(evento));
        }
    }
}
```

### c) Integrar ao PedidoService

```csharp

public class PedidoService
{
    private static readonly List<Pedido> _pedidos = new();
    private readonly SimpleEventBus _eventBus;

    public PedidoService(SimpleEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task<Pedido> Salvar(Pedido pedido)
    {
        _pedidos.Add(pedido);

        // Publicar evento após salvar
        var evento = new PedidoImportadoEvent
        {
            PedidoId = pedido.Id,
            Valor = pedido.Valor
        };
        await _eventBus.Publish(evento);

        return pedido;
    }

    public Task<List<Pedido>> Listar() => Task.FromResult(_pedidos);
}

```

### d) Registrar consumidores
```csharp
var eventBus = new SimpleEventBus();

// Consumidor que envia e-mail
eventBus.Subscribe(async evento =>
{
    await Task.Delay(500); // Simula processamento assíncrono
    Console.WriteLine($"Email enviado para pedido {evento.PedidoId} no valor {evento.Valor}");
});

// Consumidor que atualiza outro sistema
eventBus.Subscribe(async evento =>
{
    await Task.Delay(500);
    Console.WriteLine($"Sistema externo notificado para pedido {evento.PedidoId}");
});

var service = new PedidoService(eventBus);
await service.Salvar(new Pedido { Id = 1, Valor = 100 });
```

## Resultado
* Assincrono
* Simples
* Escalável
* E não depende de bibliotecas externas