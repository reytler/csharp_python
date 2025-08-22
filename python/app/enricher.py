import random
import time


_cache = {}
_CACHE_TTL = 5 * 60  # 5 minutos

def _get_from_cache(order_id: str):
    entry = _cache.get(order_id)
    if entry:
        value, timestamp = entry
        if time.time() - timestamp < _CACHE_TTL:
            return value
        else:
            del _cache[order_id]
    return None


def _set_cache(order_id: str, data: dict):
    _cache[order_id] = (data, time.time())


def _simulate_external_enrichment(order: dict) -> dict:
    """
    Simula uma chamada externa. 
    90% de chance de sucesso, 10% de falha.
    """
    if random.random() < 0.9:
        # Sucesso
        return {"cliente_nome": f"Cliente {order['cliente_id']}"}
    else:
        # Falha
        raise Exception("Falha na API externa")
    
    
def enrich_order(order: dict) -> dict:
    # Validação básica
    if not isinstance(order, dict) or "order_id" not in order or "cliente_id" not in order:
        raise ValueError("Pedido inválido")

    order_id = str(order["order_id"])

    # Tenta enriquecer via API externa
    try:
        enriched_data = _simulate_external_enrichment(order)
        _set_cache(order_id, enriched_data)
    except Exception:
        # Fallback para cache local
        enriched_data = _get_from_cache(order_id)
        if not enriched_data:
            enriched_data = {"cliente_nome": "Desconhecido"}

    # Retorna o pedido com o campo cliente_nome
    return {**order, **enriched_data}