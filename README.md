# Exemplo de Idempotência com Redis em API .NET

## ✅ Objetivo

Este projeto demonstra como implementar **idempotência** em requisições HTTP utilizando Redis como mecanismo de armazenamento temporário.  
Isso garante que uma mesma operação enviada múltiplas vezes — como criação de pedido ou início de pagamento — seja tratada **apenas uma vez**, evitando efeitos colaterais ou duplicidade.

---

## 🧠 O que é Idempotência?

Idempotência significa que uma operação pode ser executada várias vezes, mas o efeito final será o mesmo que se fosse executada apenas uma vez.

Exemplo:  
O usuário clica 2 ou 3 vezes no botão **“Pagar”** →  
✅ pagamento não deve ser duplicado.

---

## 🔧 Tecnologias Utilizadas

| Tecnologia | Função |
|----------|--------|
| .NET 7+ | Web API (Minimal API) |
| Redis | Controle de idempotência |
| Docker | Execução do Redis |

---

## 🚀 Como rodar o Redis com Docker

### Comando simples

```bash
docker run -d --name redis -p 6379:6379 redis
