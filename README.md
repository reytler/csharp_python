## ✅ Tarefas

### Parte 1 – C# (correção e testes)

Você recebeu o seguinte código legado em um controller de API:

```csharp
[HttpPost]
public async Task<IActionResult> Import([FromBody] Pedido pedido)
{
    if (pedido.Id == null)
        return BadRequest();

    if (pedido.Valor < 0)
        return BadRequest();

    _pedidoService.Salvar(pedido);
    return Ok();
}
```

#### Tarefas:

1. Liste todos os problemas técnicos ou lógicos que você identifica no código.
    * A função Import do controller tem uma asinatura assícrona mas não está utilizando "await" no _pedidoService.Salvar(pedido)
    * O serviço não está sendo instânciado em nenhum lugar do código, portando não pode ser utilizado
    * Validações superciais e feitas manualmente
    * HTTP status genéricos e sem informação
    * Ausencia de tratamento de exceção

2. Reescreva o código corrigindo os problemas e aplicando boas práticas.
    * Código reescrito utilizando programação assícrona corretamente
    * O Serviço _pedidoService foi criado e inserido no Conteiner DI do Asp.Net, sendo assim instanciado no construtor do controller
    * Foi criada uma classe de Dto com validações por DataAnnotations
    * Agora os HTTP status retornam a devida informação
    * Foi criado o tratamento de exceção com bloco tryCatch

3. Implemente **1 teste de unidade** e **1 teste de integração mínima**.
    * Foram criados testes de integração e unitário no projeto de testes a parte(csharp.tests)

4. Suponha que após importar pedidos, o sistema precisa publicar uma **mensagem de evento assíncrona**.
   - Descreva como você implementaria isso (sem usar bibliotecas prontas).
   - Apresente a estrutura básica da lógica.

   Foi utilizado o Observer Pattern, simples, de forma assícrona e escalável. Vide: [Explicação da mensageria assícrona](csharp/Estrutura-mensagem-evento-assincrona.md)