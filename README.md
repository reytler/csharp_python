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

#ATENÇÃO Explicações da tarefa 2 item 4( **concorrência** ) vide: [Readme Python](python/readme.md)
---

### Parte 3 – Raciocínio Técnico

Responda às seguintes perguntas diretamente neste arquivo (README.md):
1. Que problemas você antecipa ao integrar os dois módulos?
    * Ao integrar os módulos eu me preocuparia com os contratos/serialização, versionamento do payload
    * Chamadas externas podem falhar, ter intermitência, causar duplicidade, latência.
    * Os possíveis podem criar duplicidade
    * O cache local precisa ser compartilhado fora do processo do python. Poderia usar um redis.
    * Me preocuparia com os logs, a observabilidade.
    * E com a segurança, poderiamos usar algum sistema de Tokens, como o OAuth.

2. Como garantiria **consistência de dados** entre os serviços, considerando que o envio de mensagens é assíncrono?
    ## OBS.: Partindo da idéia de que o C# só recebe a importação do pedido e emite o evento para o Python enriquecer os dados.
    * O Python consumiria uma fila e enriqueceria os dados e os alteraria diretamento no banco, afim de evitar complexidade caso devolvesse ao C# os dados enriquecidos.
    * Como a messageria é assíncrona, eu emitiria o evento na mesma transação do banco(C#), a partir dai o Python se encarregaria do resto, falback, retry caso a api externa caíse.
    * Utilizar estados no pedido, para a UI exibir, para ficar claro para o usuário.

3. Como garantiria **observabilidade** (ex: logs e rastreabilidade)?
    * Logs estruturados (JSON) com correlationId, pedidoId, clienteId, origem e latência.
    * Métricas: taxa de sucesso/erro, acerto de cache, latência externa, fila de eventos
    * Rastreamento de uma requisição do início ao fim.
4. Como prepararia esse sistema para escalar horizontalmente sem perder rastreabilidade e tolerância a falhas?
    * Para escalar eu utilizaria serviços stateless com cache compartilhado.
    * Centralizaria os logs com um SEQ Log, por exemplo.
    * Utilizaria retry/backoff e circuit breaker para não sobrecarregar mais ainda o serviço externo caso falhasse
        - Reenvios com tempo de espera, a cada vez que há muitas falhas o tempo de espera é aumentado até o serviço 
        externo voltar a responder corretamente.
---