

# Integração com Cielo Ecommerce Sandbox

## Descrição
Este é um projeto de integração com a **Cielo**, feito com **ASP.NET Core**, **Blazor**, e **Entity Framework** utilizando o **InMemory Database** para simulação de transações com cartão de crédito falso.

## Pré-requisitos

Antes de rodar o projeto localmente, certifique-se de ter o seguinte instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou acesso a um terminal

## Instalação e Configuração Local

### 1. Clonar o repositório

Abra um terminal e clone o repositório:

```bash
git clone https://github.com/Mateus-Galvao-de-Camargo/Integracao-CieloEcommerce-Sandbox.git
```

### 2. Navegar para o diretório do projeto

Entre na pasta do projeto clonado:

```bash
cd IntegracaoCieloEcommerceSandbox/IntegracaoCieloEcommerceSandbox
```

### 3. Configurar a API da Cielo

O projeto utiliza a API da Cielo para realizar simulações de transações de pagamento. A Cielo oferece gratuitamente as chaves para fazer uso desse ambiente de testes, só é necessário se [cadastrar](https://cadastrosandbox.cieloecommerce.cielo.com.br/) e guardar as chaves.

#### Opção 1: Usando arquivo de configuração local (Recomendado para desenvolvimento)

1. Copie o arquivo de exemplo:
```bash
cp appsettings.json.example appsettings.Local.json
```

2. Edite o arquivo `appsettings.Local.json` e configure suas chaves:
```json
{
  "Cielo": {
    "MerchantId": "SEU_MERCHANT_ID",
    "MerchantKey": "SUA_MERCHANT_KEY",
    "CieloApiCriaTransacaoUrl": "https://apisandbox.cieloecommerce.cielo.com.br/1/sales"
  }
}
```

**Nota:** O arquivo `appsettings.Local.json` não é versionado no Git, mantendo suas credenciais seguras.

#### Opção 2: Usando variáveis de ambiente

Você também pode configurar as credenciais através de variáveis de ambiente:

**Windows (PowerShell):**
```powershell
$env:Cielo__MerchantId="SEU_MERCHANT_ID"
$env:Cielo__MerchantKey="SUA_MERCHANT_KEY"
$env:Cielo__CieloApiCriaTransacaoUrl="https://apisandbox.cieloecommerce.cielo.com.br/1/sales"
```

**Linux/macOS:**
```bash
export Cielo__MerchantId="SEU_MERCHANT_ID"
export Cielo__MerchantKey="SUA_MERCHANT_KEY"
export Cielo__CieloApiCriaTransacaoUrl="https://apisandbox.cieloecommerce.cielo.com.br/1/sales"
```

**Nota:** No .NET, o separador hierárquico para variáveis de ambiente é `__` (dois underscores).

### 4. Rodar o projeto

Como o **EntityFramework InMemory** é usado, não é necessário configurar um banco de dados. O banco de dados em memória será usado para armazenar os dados temporariamente apenas durante a execução do projeto, pois o único objetivo deste projeto é testar a integração com a API da Cielo.

- Para rodar o projeto localmente, execute no terminal:

```bash
dotnet run
```

Ou, se estiver usando o **Visual Studio**:

- Abra o projeto.
- Pressione `F5` para iniciar o projeto em modo de depuração.

### 5. Acessar o projeto

Depois de iniciar o servidor, o projeto estará rodando em `http://localhost:5035`.

### 6. Testar as funcionalidades

Com este projeto você pode: 
- Cadastrar, Editar, Excluir e Visualizar Cartões (CRUD)
- Cadastrar, Capturar, Cancelar e Visualizar Transações de Cartão de Crédito

### 7. Usar os testes unitários
#### **OPCIONAL**

Abra o terminal e garanta que esteja no diretório do projeto

Caso não esteja apenas navegue até o projeto com: 
```bash
cd IntegracaoCieloEcommerceSandbox/IntegracaoCieloEcommerceSandbox
```
E use o comando:
```bash
dotnet test
```

Se estiver no Visual Studio você pode só usar do Test Explorer

## Tecnologias Utilizadas

- **ASP.NET Core 8.0**
- **Blazor** (Server-side)
- **Entity Framework Core** (InMemory Database)
- **API da Cielo** para transações de cartão de crédito

## Documentação da API E-Commerce Cielo
https://docs.cielo.com.br/ecommerce-cielo/docs/sobre-api-ecommerce

## Deploy no Microsoft Azure

### Pré-requisitos para Azure
- Conta no [Microsoft Azure](https://azure.microsoft.com/)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) instalado (opcional, para deploy via linha de comando)

### Opção 1: Deploy via Portal Azure

1. **Criar um App Service:**
   - Acesse o [Portal Azure](https://portal.azure.com/)
   - Clique em "Criar um recurso" > "Aplicativo Web"
   - Preencha os campos:
     - Nome do aplicativo (ex: `meu-app-cielo`)
     - Sistema Operacional: Windows ou Linux
     - Runtime stack: .NET 8
     - Região: escolha a mais próxima

2. **Configurar as variáveis de ambiente no Azure:**
   - No App Service criado, vá em "Configuração" > "Configurações do aplicativo"
   - Adicione as seguintes configurações:
     - Nome: `Cielo__MerchantId` | Valor: `seu_merchant_id`
     - Nome: `Cielo__MerchantKey` | Valor: `sua_merchant_key`
     - Nome: `Cielo__CieloApiCriaTransacaoUrl` | Valor: `https://apisandbox.cieloecommerce.cielo.com.br/1/sales`
   - Clique em "Salvar"

3. **Deploy do código:**
   - No Visual Studio: clique com botão direito no projeto > "Publicar" > selecione seu App Service
   - Ou use GitHub Actions / Azure DevOps para CI/CD automático

### Opção 2: Deploy via Azure CLI

```bash
# Login no Azure
az login

# Criar um Resource Group (se necessário)
az group create --name meu-resource-group --location brazilsouth

# Criar um App Service Plan
az appservice plan create --name meu-app-plan --resource-group meu-resource-group --sku B1 --is-linux

# Criar o Web App
az webapp create --resource-group meu-resource-group --plan meu-app-plan --name meu-app-cielo --runtime "DOTNET|8.0"

# Configurar as variáveis de ambiente
az webapp config appsettings set --resource-group meu-resource-group --name meu-app-cielo --settings \
  Cielo__MerchantId="seu_merchant_id" \
  Cielo__MerchantKey="sua_merchant_key" \
  Cielo__CieloApiCriaTransacaoUrl="https://apisandbox.cieloecommerce.cielo.com.br/1/sales"

# Deploy do código (a partir do diretório do projeto)
cd IntegracaoCieloEcommerceSandbox
az webapp up --resource-group meu-resource-group --name meu-app-cielo --runtime "DOTNET:8.0"
```

### Segurança das Credenciais

⚠️ **IMPORTANTE:** Nunca comite suas credenciais da Cielo no código! 

- As credenciais devem ser configuradas através de:
  - Variáveis de ambiente (local e produção)
  - Arquivo `appsettings.Local.json` (apenas local, não versionado)
  - Configurações do App Service no Azure (produção)
  
- O arquivo `appsettings.json` no repositório contém apenas valores vazios/placeholder
- Use o arquivo `appsettings.json.example` como referência para configuração
