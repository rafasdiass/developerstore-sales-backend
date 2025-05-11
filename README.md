
````markdown
# 🛒 DeveloperStore - Sales API

API RESTful desenvolvida para controle de vendas da DeveloperStore, seguindo os princípios de **DDD (Domain-Driven Design)**, **Clean Architecture** e **SOLID**. Esta API permite registrar, consultar e cancelar vendas, aplicar regras de desconto por quantidade e registrar eventos de domínio.

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

## 🛠 Tecnologias Utilizadas

- [.NET SDK 9+](https://dotnet.microsoft.com/)
- C# 12
- ASP.NET Core Web API
- Entity Framework Core
- xUnit + FluentAssertions + Moq
- Swagger (documentação de API)
- Git / GitHub

## 🧱 Arquitetura do Projeto

```text
/src
├── DeveloperStore.Sales.API            # Camada de apresentação (Controllers, config, Swagger)
├── DeveloperStore.Sales.Application    # Casos de uso (DTOs, comandos, handlers)
├── DeveloperStore.Sales.Domain         # Modelos de domínio (Entidades, VOs, serviços, interfaces)
├── DeveloperStore.Sales.Infrastructure # Repositórios, DbContext, integrações externas
└── DeveloperStore.Sales.Tests          # Testes unitários
````

## 📦 Modelagem do Domínio

### `Sale` (Venda)

Entidade agregadora com:

* `SaleNumber`: identificador único da venda
* `SaleDate`: data da venda
* `Customer`: cliente (ID + Nome)
* `Branch`: filial (ID + Nome)
* `Items`: lista de produtos vendidos
* `IsCancelled`: indicador de cancelamento
* `TotalAmount`: valor total da venda (apenas itens não cancelados)

### `SaleItem` (Item da Venda)

* `Product`: produto (ID + Nome)
* `Quantity`: quantidade adquirida
* `UnitPrice`: valor unitário
* `Discount`: desconto aplicado automaticamente
* `TotalItemAmount`: total do item após desconto
* `IsCancelled`: indicador de cancelamento do item

### Value Objects

* `ExternalCustomer`, `ExternalBranch`, `ExternalProduct`: objetos imutáveis representando dados externos (ID + Nome), conforme padrão de identidade externa com denormalização.

## 📜 Regras de Negócio

| Quantidade de Itens | Desconto Aplicado |
| ------------------- | ----------------- |
| Abaixo de 4         | 0% (sem desconto) |
| De 4 a 9            | 10%               |
| De 10 a 20          | 20%               |
| Acima de 20         | Não permitido     |

* Vendas podem ser canceladas integralmente ou por item
* Descontos são automáticos e baseados na **quantidade**
* Total da venda considera apenas **itens não cancelados**

## 🔔 Eventos de Domínio

Eventos registrados internamente (log):

* `SaleCreated`
* `SaleModified`
* `SaleCancelled`
* `ItemCancelled`

## 💻 Como Clonar o Projeto

```bash
git clone https://github.com/rafasdiass/developerstore-sales-backend.git
cd developerstore-sales-api
```

## ▶️ Como Executar

### Pré-requisitos

* .NET SDK 9 instalado
* Editor (VS Code ou Visual Studio)
* Git

### Passos

1. Restaurar pacotes

   ```bash
   dotnet restore
   ```

2. Executar a aplicação

   ```bash
   dotnet run --project src/DeveloperStore.Sales.API
   ```

3. Acessar o Swagger

   ```text
   http://localhost:5000/swagger
   ```

## 🧪 Testes

Execute os testes unitários com:

```bash
dotnet test
```

Abrange:

* regras de desconto
* cancelamento de venda e item
* totalização de venda

## 👤 Autor

Desenvolvido por **Rafael de Souza Dias**

* GitHub: [rafasdiass](https://github.com/rafasdiass)
* E-mail: [rafasdiasdev@gmail.com](mailto:rafasdiasdev@gmail.com)
* LinkedIn: [linkedin.com/in/rdrafaeldias](https://www.linkedin.com/in/rdrafaeldias/)
