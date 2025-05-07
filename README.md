
# 🛒 DeveloperStore - Sales API

API RESTful desenvolvida para controle de vendas da DeveloperStore, seguindo os princípios de **DDD (Domain-Driven Design)**, **Clean Architecture** e **SOLID**. Esta API permite registrar, consultar e cancelar vendas, aplicar regras de desconto por quantidade e registrar eventos de domínio.

---

## 📚 Sumário

- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Arquitetura do Projeto](#arquitetura-do-projeto)
- [Modelagem do Domínio](#modelagem-do-domínio)
- [Regras de Negócio](#regras-de-negócio)
- [Eventos de Domínio](#eventos-de-domínio)
- [Como Clonar o Projeto](#como-clonar-o-projeto)
- [Como Executar](#como-executar)
- [Testes](#testes)
- [Autor](#autor)

---

## 🛠 Tecnologias Utilizadas

- [.NET SDK 9+](https://dotnet.microsoft.com/)
- C# 12
- ASP.NET Core Web API
- Entity Framework Core
- xUnit + FluentAssertions + Moq (testes)
- Swagger (documentação de API)
- Git/GitHub

---

## 🧱 Arquitetura do Projeto

A API foi desenvolvida com base na **Clean Architecture**, separando claramente as responsabilidades entre as camadas:

```
/src
├── DeveloperStore.Sales.API              # Camada de apresentação (controllers, config, Swagger)
├── DeveloperStore.Sales.Application     # Casos de uso (DTOs, comandos, serviços de aplicação)
├── DeveloperStore.Sales.Domain          # Modelos de domínio (entidades, VOs, eventos, interfaces)
├── DeveloperStore.Sales.Infrastructure  # Repositórios, contexto de dados, integrações externas
├── DeveloperStore.Sales.Tests           # Testes unitários
```

---

## 📦 Modelagem do Domínio

### `Sale` (Venda)
Entidade agregadora com os seguintes atributos:
- `SaleNumber`: Identificador único da venda
- `SaleDate`: Data da venda
- `Customer`: Cliente (referência externa com ID + Nome)
- `Branch`: Filial (referência externa com ID + Nome)
- `Items`: Lista de produtos vendidos
- `IsCancelled`: Indicador de cancelamento
- `TotalAmount`: Valor total da venda (apenas itens ativos)

### `SaleItem` (Item da Venda)
- `Product`: Produto (referência externa com ID + Nome)
- `Quantity`: Quantidade adquirida
- `UnitPrice`: Valor unitário
- `Discount`: Desconto aplicado automaticamente
- `TotalItemAmount`: Total do item após desconto
- `IsCancelled`: Cancelamento individual do item

### Value Objects
- `ExternalCustomer`, `ExternalBranch`, `ExternalProduct`: objetos imutáveis representando dados externos (ID + Nome), conforme o padrão de **identidade externa com denormalização**.

---

## 📜 Regras de Negócio

| Quantidade de Itens | Desconto Aplicado |
|---------------------|-------------------|
| Abaixo de 4         | 0% (sem desconto) |
| 4 a 9               | 10%               |
| 10 a 20             | 20%               |
| Acima de 20         | Não permitido     |

- Vendas podem ser canceladas integralmente ou por item
- Descontos são automáticos e baseados apenas na **quantidade**
- O valor total da venda considera apenas **itens não cancelados**

---

## 🔔 Eventos de Domínio

Embora sem integração com um broker de mensagens, o projeto registra os seguintes eventos em log:

- `SaleCreated`
- `SaleModified`
- `SaleCancelled`
- `ItemCancelled`

Esses eventos são emitidos internamente no domínio para futura extensibilidade.

---

## 💻 Como Clonar o Projeto

```bash
git clone https://github.com/rafasdiass/developerstore-sales-backend.git
cd developerstore-sales-api
```


---

## ▶️ Como Executar

### Pré-requisitos
- .NET SDK 9 instalado
- Editor como VS Code ou Visual Studio
- Terminal com Git instalado

### Passos

1. Restaurar os pacotes:
```bash
dotnet restore
```

2. Rodar a aplicação:
```bash
dotnet run --project src/DeveloperStore.Sales.API
```

3. Acessar o Swagger para testes:
```
http://localhost:5000/swagger
```

> A porta pode variar conforme sua configuração local

---

## 🧪 Testes

Para executar os testes unitários, utilize:

```bash
dotnet test
```

A suíte cobre:
- Regras de desconto
- Cancelamento de venda e de item
- Cálculo do valor total da venda

---

## 👤 Autor

Desenvolvido por **Rafael de Souza Dias**

- GitHub: [rafael](https://github.com/rafasdiass)
- E-mail: rafasdiasdev@gmail.com
- LinkedIn: [linkedin.com/in/rdrafaeldias](https://www.linkedin.com/in/rdrafaeldias/)

---
