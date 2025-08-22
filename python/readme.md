# Como fazer enriquecimento de Pedidos com Cache e Fallback

* Este projeto contém uma função Python para enriquecer pedidos (order) com informações adicionais de clientes, simulando uma chamada externa, mas com fallback usando um cache local com expiração.

## Estrutura do Código

1. Cache Local

```python
    _cache = {}
    _CACHE_TTL = 5 * 60  # 5 minutos
```

* "_get_from_cache" retorna os dados do cache se ainda estiverem válidos; caso contrário, remove a entrada.
* "_set_cache" grava os dados no cache junto com o timestamp de inserção.

```python
def _get_from_cache(order_id: str):
    entry = _cache.get(order_id)
    if entry:
        value, timestamp = entry
        if time.time() - timestamp < _CACHE_TTL:
            return value
        else:
            del _cache[order_id]
    return None
```

```python
def _set_cache(order_id: str, data: dict):
    _cache[order_id] = (data, time.time())
```

### Simula api externa
```python
def _simulate_external_enrichment(order: dict) -> dict:
    """
    Simula uma chamada externa. 
    90% de chance de sucesso, 10% de falha.
    """
    if random.random() < 0.9:
        return {"cliente_nome": f"Cliente {order['cliente_id']}"}
    else:
        raise Exception("Falha na API externa")

```

* Simula a resposta de api que retorna o nome do cliente com base no ```cliente_id```, pode falhar em 10% das vezes que for chamada, para testar o Fallback

## Função principal de enriquecimento ```enrich_order```

* Valida a entrada garantindo que o pedido seja um dicionário e contenha ```order_id``` e ```cliente_id```
* Chama a função ```_simulate_external_enrichment``` e se ela falhar tenta obter os dados em cache local e se não houver cache, retorna ```cliente_nome``` com valor "Desconhecido".
* Seguindo o fluxo padrão retorna o pedido enriquecido com o ```cliente_nome```

# Considerações sobre concorrência
* O código atual não é thread-safe. Se vários processos ou threads chamarem enrich_order simultaneamente, pode haver condições de corrida no acesso ao dicionário _cache.

* Para resolver isso:
 * Usar threading.Lock() para sincronizar acesso ao _cache.
 * Em sistemas distribuídos, usar um cache compartilhado (Redis, Memcached) com TTL e atomicidade.

### Exemplo com ```threading.Lock()```

```python
import threading

_cache_lock = threading.Lock()

def _set_cache(order_id, data):
    with _cache_lock:
        _cache[order_id] = (data, time.time())

def _get_from_cache(order_id):
    with _cache_lock:
        entry = _cache.get(order_id)
        if entry:
            value, timestamp = entry
            if time.time() - timestamp < _CACHE_TTL:
                return value
            else:
                del _cache[order_id]
    return None

```