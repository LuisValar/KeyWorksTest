# Projeto - Teste KeyWorks 

## Ferramentas Utilizadas

- VS code
- SQL Server Express
- Entity FrameWork


## Linguagem e arquitetura utilizadas

- ASP.Net core FrameWork 
- Arquitetura MVC

## Inicio do projeto

## Comandos para criação inicial do projeto
 
    - dotnet new web api
    - dotnet add package EntityFramework
    - dotnet add package Microsoft.EntityFrameworkCore.Desing
    - dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    
## Configuração de acesso ao banco de dados

    Foi utilizado uma conexão padrão de localhost para base de desenvolvimento,
        como pode ser visto no arquivo: appsettings.Development.json
        utilizando o seguinte trexo de código:
        "ConnectionStrings": {
    "ConexaoPadrao": "Server=localhost\\SQLEXPRESS01;Initial Catalog=StreamberryKW;TrustServerCertificate=True;Integrated Security=True" }
  
  no arquivo de Program.cs foi inicializado a conexão como pode ser vista nesse 
  trexo de código:
  builder.Services.AddDbContext<ContextMovies>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

## Inicialização

Ao rodar o comando dotnet watch run no terminal do vs code o programa ira "buildar" e irá abrir o swagger onde está as rotas criadas conforme solitado no desafio.



Duvidas estou a diposição.
