import pytest
from app.enricher import enrich_order, _cache

def test_enrich_order_success(monkeypatch):
    # Força sucesso
    monkeypatch.setattr("random.random", lambda: 0.5)
    order = {"order_id": 1, "cliente_id": 123}
    enriched = enrich_order(order)
    assert enriched["cliente_nome"] == "Cliente 123"

def test_enrich_order_fallback(monkeypatch):
    # Força falha
    monkeypatch.setattr("random.random", lambda: 0.9)
    order_id = "2"
    order = {"order_id": order_id, "cliente_id": 456}

    # Primeiro, cache vazio → deve retornar "Desconhecido"
    enriched = enrich_order(order)
    assert enriched["cliente_nome"] == "Desconhecido"

    # Adiciona manualmente no cache
    _cache[order_id] = ({"cliente_nome": "Cache Cliente 456"}, time.time())
    enriched2 = enrich_order(order)
    assert enriched2["cliente_nome"] == "Cache Cliente 456"

def test_cache_expiration(monkeypatch):
    order_id = "3"
    order = {"order_id": order_id, "cliente_id": 789}

    # Coloca no cache com timestamp antigo
    _cache[order_id] = ({"cliente_nome": "Velho Cliente"}, time.time() - 600)
    monkeypatch.setattr("random.random", lambda: 0.9)  # Força falha externa
    enriched = enrich_order(order)
    assert enriched["cliente_nome"] == "Desconhecido"