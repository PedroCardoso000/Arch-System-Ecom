# 🛒 Arch System E-commerce

Mini sistema de e-commerce desenvolvido como teste técnico com foco em arquitetura moderna utilizando **DDD (Domain-Driven Design)** e **Clean Architecture**.

---

# 🚀 Como executar o projeto

## 📦 Pré-requisitos

* .NET 10 SDK
* Docker e Docker Compose

---

## ▶️ Subindo a aplicação

```bash
docker compose up --build
```

---

## 🌐 Acessos

* API: http://localhost:5000
* Swagger: http://localhost:5000/swagger

---

## 🧪 Executar testes

```bash
dotnet test
```

---

# 🧠 Descrição da arquitetura

O projeto foi estruturado seguindo os princípios de **Clean Architecture**, com separação clara de responsabilidades:

```
Core → Regras de negócio (domínio)
UseCases → Casos de uso (Application Layer)
Infrastructure → Persistência, Kafka, Outbox
WebApi → Interface HTTP (Minimal APIs)
Tests → Testes unitários do domínio
```

---

## 🔵 Core (Domínio)

Contém o coração da aplicação:

* Aggregates:

  * Pedido
  * Cliente
  * Produto
* Entities:

  * ItemPedido
* Value Objects:

  * Email
  * Money
  * Quantidade
* Domain Events:

  * PedidoConfirmadoDomainEvent
* Domain Services:

  * CalculadoraPedidoService

### ✔ Características

* Entidades encapsuladas
* Regras de negócio centralizadas
* Uso de Value Objects para garantir consistência
* Eventos de domínio para desacoplamento

---

## 🟢 UseCases (Application Layer)

Responsável por orquestrar o fluxo da aplicação.

* Commands:

  * CreatePedido
  * AddItemPedido
  * ConfirmarPedido
  * CreateCliente
  * CreateProduto

* Queries:

  * GetPedidoById
  * GetProduto
  * GetClienteById

* Handlers:

  * Executam os casos de uso

---

## 🟡 Infrastructure

Implementa detalhes técnicos:

* Entity Framework Core
* Repositórios
* Outbox Pattern
* Kafka Producer
* Background Worker

### 🔁 Outbox Pattern

Fluxo:

1. Evento de domínio é disparado
2. Evento é salvo na tabela Outbox
3. Worker lê eventos pendentes
4. Evento é publicado no Kafka
5. Evento é marcado como processado

---

## 🔴 WebApi

* Minimal APIs
* Swagger
* Injeção de dependência
* Endpoints REST

---

# 🧩 Decisões arquiteturais

## ✔ Uso de DDD

A modelagem foi orientada ao domínio, priorizando:

* Comportamento nas entidades
* Encapsulamento
* Invariantes protegidas

---

## ✔ Value Objects

Utilizados para representar conceitos do domínio:

* Email → validação de formato
* Money → encapsula valores monetários
* Quantidade → evita valores inválidos

Benefícios:

* Redução de erros
* Código mais expressivo
* Regras centralizadas

---

## ✔ Domain Events

O evento `PedidoConfirmadoDomainEvent` é disparado dentro do Aggregate.

Motivação:

* Desacoplamento entre domínio e integração
* Preparação para arquitetura orientada a eventos

---

## ✔ Outbox Pattern

Adotado para garantir consistência entre:

* Banco de dados
* Mensageria (Kafka)

Evita problemas de dual write.

---

## ✔ Kafka

Utilizado para publicação de eventos de integração:

* Topic: `pedido-confirmado`
* Payload contém dados do pedido confirmado

---

## ✔ Clean Architecture

Separação em camadas garante:

* Testabilidade
* Baixo acoplamento
* Facilidade de manutenção

---

## ✔ Minimal APIs

Escolhido por:

* Simplicidade
* Menor overhead
* Clareza no fluxo

---

## ✔ Testes Unitários

Focados no domínio:

* Regras de negócio
* Invariantes
* Eventos

Sem uso de mocks para manter simplicidade e confiabilidade.

---

# 📌 Funcionalidades implementadas

* Cadastro de clientes
* Cadastro de produtos
* Criação de pedidos
* Inclusão de itens no pedido
* Cálculo automático do total
* Confirmação de pedido
* Envio de evento de integração para Kafka quando o pedido for confirmado

---

# 🧪 Exemplos de cenários testados

* Pedido calcula total corretamente
* Pedido não pode ser confirmado sem itens
* Produto inativo não pode ser adicionado
* Email inválido lança exceção

---

# 📦 Tecnologias utilizadas

* .NET 10
* C#
* Entity Framework Core
* SQL Server 2022
* Docker Compose
* Kafka
* xUnit
* Swagger

---

# 👨‍💻 Autor

Pedro Cardoso

---
