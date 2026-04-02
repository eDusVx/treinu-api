# Treinu API

Uma API RESTful completa para gerenciamento de treinadores e alunos. A plataforma permite que treinadores se registrem, gerenciem seus dados e se conectem com alunos, enquanto alunos podem se registrar e manter informações de contato e avaliações físicas.

## 📋 Sumário

- [Sobre](#sobre)
- [Tecnologias](#tecnologias)
- [Instalação e Setup](#instalação-e-setup)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
- [Rodando Localmente](#rodando-localmente)
- [Rodando com Docker](#rodando-com-docker)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Endpoints API](#endpoints-api)
- [Autenticação](#autenticação)
- [Contribuição](#contribuição)

## Sobre

A Treinu API é um backend robusto desenvolvido com .NET 10 que oferece:

- ✅ Autenticação e autorização com JWT
- ✅ Gerenciamento de usuários (Alunos e Treinadores)
- ✅ Sistema de convites entre treinadores e alunos
- ✅ Avaliações físicas para alunos
- ✅ Gerenciamento de contatos
- ✅ Recuperação de senha e autenticação por código
- ✅ Health checks para monitoramento
- ✅ Documentação automática com Swagger/OpenAPI

## Tecnologias

- **Runtime**: .NET 10.0
- **Linguagem**: C#
- **Banco de Dados**: PostgreSQL 15
- **ORM**: Entity Framework Core 10.0
- **Autenticação**: JWT Bearer
- **Validação**: FluentValidation
- **Padrão**: CQRS (Command Query Responsibility Segregation) com MediatR
- **API Docs**: Swagger/OpenAPI
- **Containerização**: Docker & Docker Compose

## Instalação e Setup

### Pré-requisitos

- .NET 10+ SDK
- PostgreSQL 15+ (ou Docker)
- Git

### Clonando o repositório

```bash
git clone https://github.com/seu-usuario/treinu-api.git
cd treinu-api
```

### Restaurando dependências

```bash
dotnet restore
```

## Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto ou utilize variáveis de ambiente do seu sistema:

```env
# Banco de Dados
ConnectionStrings__DefaultConnection=Host=localhost;Database=treinu_db;Username=treinu_user;Password=treinu_pass

# JWT Settings
JwtSettings__Secret=SuperSecretKeyForDevelopmentOnlyPleaseChangeLater!!123
JwtSettings__Issuer=TreinuApi
JwtSettings__Audience=TreinuClients
JwtSettings__ExpirationInMinutes=60

# SendGrid (Email)
SendGrid__ApiKey=sua_chave_sendgrid_aqui
SendGrid__FromEmail=seu-email@exemplo.com
SendGrid__FromName=Treinu App

# Docker
DB_USERNAME=treinu_user
DB_PASSWORD=treinu_pass
DB_DATABASE=treinu_db
```

## Rodando Localmente

### 1. Configurar Banco de Dados PostgreSQL

**Opção A: Com PostgreSQL instalado localmente**

```bash
# Criar banco de dados
createdb treinu_db

# Ou via psql
psql -U postgres -c "CREATE DATABASE treinu_db;"
```

**Opção B: Em um container Docker**

```bash
docker run --name treinu-postgres \
  -e POSTGRES_USER=treinu_user \
  -e POSTGRES_PASSWORD=treinu_pass \
  -e POSTGRES_DB=treinu_db \
  -p 5432:5432 \
  -d postgres:15-alpine
```

### 2. Aplicar Migrações

```bash
cd src/Treinu.Api
dotnet ef database update
```

### 3. Executar a Aplicação

```bash
dotnet run --project src/Treinu.Api/Treinu.Api.csproj
```

A API estará disponível em:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger**: `http://localhost:5000/swagger`

## Rodando com Docker

### Com Docker Compose

```bash
# Iniciar todos os serviços
docker-compose up -d

# Visualizar logs
docker-compose logs -f

# Parar serviços
docker-compose down
```

A aplicação estará disponível em `http://localhost:8080`

### Builder Manual com Docker

```bash
# Build da imagem
docker build -t treinu-api:latest .

# Executar container
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Host=db;Database=treinu_db;Username=treinu_user;Password=treinu_pass" \
  treinu-api:latest
```

## Estrutura do Projeto

```
treinu-api/
├── src/
│   ├── Treinu.Api/                 # Projeto da API (Controllers, Middlewares)
│   ├── Treinu.Application/          # Handlers e Services (Lógica de Negócio)
│   ├── Treinu.Contracts/            # DTOs e Requests/Responses
│   ├── Treinu.Domain/               # Entidades, Interfaces e Enums
│   └── Treinu.Infrastructure/       # Repositórios, EF Core Context, Segurança
├── tests/
│   └── Treinu.UnitTests/            # Testes Unitários
├── docker-compose.yml
├── Dockerfile
├── treinu.slnx                      # Solução
└── README.md
```

## Endpoints API

### 🔐 Autenticação (anônimo)

#### Login por Email/Senha
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "senha": "senha123"
}
```
**Response**: `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600
}
```

#### Solicitar Código de Login
```http
POST /api/auth/solicitar-codigo-login
Content-Type: application/json

{
  "email": "usuario@exemplo.com"
}
```
**Response**: `200 OK` - Código enviado por email

#### Login com Código
```http
POST /api/auth/login-codigo
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "codigo": "123456"
}
```
**Response**: `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600
}
```

#### Renovar Token
```http
POST /api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "token-anterior"
}
```
**Response**: `200 OK` - Novo token gerado

#### Solicitar Recuperação de Senha
```http
POST /api/auth/recuperar-senha
Content-Type: application/json

{
  "email": "usuario@exemplo.com"
}
```
**Response**: `200 OK` - Link de recuperação enviado por email

#### Redefinir Senha
```http
POST /api/auth/redefinir-senha
Content-Type: application/json

{
  "token": "token-recuperacao",
  "novaSenha": "novaSenha123",
  "confirmacaoSenha": "novaSenha123"
}
```
**Response**: `200 OK`

---

### 👨‍🎓 Alunos

#### Registrar Novo Aluno
```http
POST /api/aluno/register
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@exemplo.com",
  "senha": "senha123",
  "confirmacaoSenha": "senha123"
}
```
**Response**: `201 Created`

#### Adicionar Contato do Aluno
```http
POST /api/aluno/{alunoId}/contatos
Authorization: Bearer <token>
Content-Type: application/json

{
  "tipo": "Celular",
  "valor": "11999999999"
}
```
**Response**: `200 OK`
**Autorização**: Aluno ou Admin

#### Remover Contato do Aluno
```http
DELETE /api/aluno/{alunoId}/contatos/{contatoId}
Authorization: Bearer <token>
```
**Response**: `204 No Content`
**Autorização**: Aluno ou Admin

#### Adicionar Avaliação Física
```http
POST /api/aluno/{alunoId}/avaliacoes
Authorization: Bearer <token>
Content-Type: application/json

{
  "altura": 1.75,
  "peso": 80.5,
  "gorduraCorpora": 15.2,
  "dataavaliacao": "2026-04-02T10:30:00Z"
}
```
**Response**: `200 OK`
**Autorização**: Treinador ou Admin

#### Remover Avaliação Física
```http
DELETE /api/aluno/{alunoId}/avaliacoes/{avaliacaoId}
Authorization: Bearer <token>
```
**Response**: `204 No Content`
**Autorização**: Treinador ou Admin

---

### 🏋️ Treinadores

#### Registrar Novo Treinador
```http
POST /api/treinador
Content-Type: application/json

{
  "nome": "Carlos Trainer",
  "email": "carlos@exemplo.com",
  "senha": "senha123",
  "confirmacaoSenha": "senha123",
  "apresentacao": "Treinador especializado em musculação"
}
```
**Response**: `201 Created`

#### Adicionar Contato do Treinador
```http
POST /api/treinador/{treinadorId}/contatos
Authorization: Bearer <token>
Content-Type: application/json

{
  "tipo": "WhatsApp",
  "valor": "11988888888"
}
```
**Response**: `200 OK`
**Autorização**: Treinador ou Admin

#### Remover Contato do Treinador
```http
DELETE /api/treinador/{treinadorId}/contatos/{contatoId}
Authorization: Bearer <token>
```
**Response**: `204 No Content`
**Autorização**: Treinador ou Admin

#### Adicionar Especialização
```http
POST /api/treinador/{treinadorId}/especializacoes
Authorization: Bearer <token>
Content-Type: application/json

{
  "nome": "Musculação"
}
```
**Response**: `200 OK`
**Autorização**: Treinador ou Admin

#### Remover Especialização
```http
DELETE /api/treinador/{treinadorId}/especializacoes
Authorization: Bearer <token>
Content-Type: application/json

{
  "especializacaoId": "uuid"
}
```
**Response**: `204 No Content`
**Autorização**: Treinador ou Admin

---

### 📧 Convites

#### Convidar Aluno
```http
POST /api/convites/aluno
Authorization: Bearer <token>
Content-Type: application/json

{
  "alunoId": "uuid",
  "treinadorId": "uuid"
}
```
**Response**: `200 OK`
**Autorização**: Treinador ou Admin

---

### 👥 Usuários

#### Buscar Usuários (Paginado)
```http
GET /api/usuarios?tipoUsuario=Aluno&page=1&limit=10
Authorization: Bearer <token>
```
**Response**: `200 OK`
```json
{
  "items": [...],
  "totalPages": 5,
  "currentPage": 1,
  "pageSize": 10,
  "totalCount": 48
}
```
**Autorização**: Admin ou Treinador
**Query Parameters**:
- `tipoUsuario` (opcional): Aluno, Treinador
- `page` (padrão: 1): Número da página
- `limit` (padrão: 10): Itens por página

---

### 🔧 Admin

#### Aprovar Treinador
```http
PATCH /api/admin/treinador/{treinadorId}/approve
Authorization: Bearer <token>
```
**Response**: `204 No Content`
**Autorização**: Admin

---

## Autenticação

A API utiliza **JWT (JSON Web Tokens)** para autenticação e autorização.

### Como usar

1. **Obter Token**:
   - Fazer login em `/api/auth/login` com email e senha
   - Receber um token JWT

2. **Incluir Token**:
   ```http
   Authorization: Bearer seu_token_jwt_aqui
   ```

3. **Roles/Permissões**:
   - `Admin`: Acesso completo
   - `Treinador`: Gerencia alunos e suas avaliações
   - `Aluno`: Gerencia seus próprios dados

### Expiração

Os tokens expiram após 60 minutos. Use `/api/auth/refresh` para renovar.

## Health Check

```http
GET /health
```
Verifica a saúde da aplicação e conexão com o banco de dados.

## Documentação Interativa

Acesse o Swagger para testar os endpoints:
- **URL**: `http://localhost:5000/swagger`

Todos os endpoints estão documentados e podem ser testados diretamente da interface.

## Tratamento de Erros

A API retorna erros padronizados:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "email": ["Email inválido"],
    "senha": ["Senha deve ter pelo menos 6 caracteres"]
  }
}
```

## Build e Deploy

### Build para Produção

```bash
dotnet build treinu.slnx --configuration Release
```

### Publicar

```bash
dotnet publish src/Treinu.Api/Treinu.Api.csproj \
  -c Release \
  -o ./publish
```

## Executar Testes Unitários

```bash
dotnet test tests/Treinu.UnitTests/Treinu.UnitTests.csproj
```
---