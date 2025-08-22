# Como eu implementaria a emissão de uma mensagem de evento assíncrona (sem usar bibliotecas prontas).


A ideia é usar **Observer Pattern** simples que permite que múltiplos consumidores sejam notificados de forma assíncrona sempre que um pedido é importado.

---

## 1. Conceito

Quando um pedido é importado:

1. O serviço salva o pedido no armazenamento (memória ou banco de dados).  
2. Há um Subject que mantém uma lista de observadores (Observers).
3. Quando o estado do Subject muda(O Pedido Service salva o pedido), ele notifica todos os observadores.

---

## 2. Estrutura básica da lógica

### Sujeito: PedidoService
### Observadores: consumidores que processam os eventos de pedido (e-mail, atualização de outro sistema, logs etc.).

```csharp
public interface IPedidoObserver
{
    Task PedidoImportado(Pedido pedido);
}
```

### b) Serviço de pedidos como sujeito

```csharp
public class PedidoService
{
    private static readonly List<Pedido> _pedidos = new();
    private readonly List<IPedidoObserver> _observers = new();

    public void RegisterObserver(IPedidoObserver observer)
    {
        _observers.Add(observer);
    }

    public async Task<Pedido> Salvar(Pedido pedido)
    {
        _pedidos.Add(pedido);

        // Notifica todos os observadores
        foreach (var observer in _observers)
        {
            // Task.Run cria uma task em background, separada do fluxo principal do método Salvar.
            _ = Task.Run(() => observer.PedidoImportado(pedido));
        }

        return Task.FromResult(pedido);
    }

    public Task<List<Pedido>> Listar() => Task.FromResult(_pedidos);
}
```

### c) Exemplo de observadores

```csharp

public class EmailObserver : IPedidoObserver
{
    public async Task PedidoImportado(Pedido pedido)
    {
        await Task.Delay(200); // Simula envio de e-mail
        Console.WriteLine($"Email enviado para pedido {pedido.Id}");
    }
}

public class LogObserver : IPedidoObserver
{
    public async Task PedidoImportado(Pedido pedido)
    {
        await Task.Delay(100); // Simula log
        Console.WriteLine($"Pedido {pedido.Id} registrado no log");
    }
}


```

### d) Registro e uso
```csharp

var service = new PedidoService();
service.RegisterObserver(new EmailObserver());
service.RegisterObserver(new LogObserver());

await service.Salvar(new Pedido { Id = 1, Valor = 100 });

```

### OBS.: 
    * Task.Run cria uma task em background, separada do fluxo principal do método Salvar.
    * O método Salvar não espera os observers terminarem.
    * Cada observer executa seu código de forma paralela e assíncrona.

## Resultado
* Assincrono
* Simples
* Escalável
* E não depende de bibliotecas externas