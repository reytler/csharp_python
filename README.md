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
    * Eu entendo que o endpoint seja uma importação vinda de outro sistema, portanto aceito enviar o Id pela request,
        porém o ideal é o servidor gerar o Id antes de salvar no banco.
    * Agora os HTTP status retornam a devida informação
    * Foi criado o tratamento de exceção com bloco tryCatch

3. Implemente **1 teste de unidade** e **1 teste de integração mínima**.
    * Foram criados testes de integração e unitário no projeto de testes a parte(csharp.tests)

4. Suponha que após importar pedidos, o sistema precisa publicar uma **mensagem de evento assíncrona**.
   - Descreva como você implementaria isso (sem usar bibliotecas prontas).
   - Apresente a estrutura básica da lógica.

   Foi utilizado o Observer Pattern, simples, de forma assícrona e escalável. Vide: [Explicação da mensageria assícrona](csharp/Estrutura-mensagem-evento-assincrona.md)

---

### Parte 2 – Python (enriquecimento com fallback)

Você deve criar uma função `enrich_order(order: dict) -> dict` que:

- Valida a entrada
- Simula chamada externa para enriquecer os dados
- Em caso de falha, busca os dados de um **cache local com expiração de 5 minutos**
- Sempre retorna o pedido com o campo `"cliente_nome"`

#### Tarefas:

1. Implemente a função `enrich_order`.
2. Crie um sistema de **cache local com expiração** (dicionário simples).
3. Escreva testes cobrindo:
   - Sucesso no enriquecimento
   - Fallback em caso de erro
   - Expiração de cache
4. Explique como lidaria com **concorrência** se vários processos chamassem a função ao mesmo tempo.

#ATENÇÃO Explicações da tarefa 2 vide: [Readme Python](python/readme.md)
---

### Parte 3 – Raciocínio Técnico

Responda às seguintes perguntas diretamente neste arquivo (README.md):

1. Que problemas você antecipa ao integrar os dois módulos?
2. Como garantiria **consistência de dados** entre os serviços, considerando que o envio de mensagens é assíncrono?
3. Como garantiria **observabilidade** (ex: logs e rastreabilidade)?
4. Como prepararia esse sistema para escalar horizontalmente sem perder rastreabilidade e tolerância a falhas?

---