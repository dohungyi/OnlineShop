## Project OnlineShop 
```
+ Business domain: e-Commerce application
+ Backend API:
    - REST API with .NET (7) design with Domain Driven Design mindset
    - Postgresql
    - MongoDb
    - Redis Caching
    - RabbitMQ
    - CQRS (Command Query Reposibility Separation)
    - ORM: EF Core, Dapper
    ...
+ Frontend:
    - Angular (16)
```

## Prepare environment
* Install dotnet core version in file `global.json`
* Jetbrains Rider Csharp 2023
* Docker Desktop

## How to run the project

Run command for build project
```Powershell
dotnet build
```

Go to folder contain file  `docker-compose`

1. Using docker-compose
```Powershell
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```

2. Using Jetbrains Rider Csharp 2023
- Open eCommerce.sln - `OnlineShop.sln`
- Run Compound to start multi projects



## Try your best
- https://github.com/microsoftarchive/redis/releases

- https://github.com/rabbitmq/rabbitmq-server/releases/tag/v3.12.6
- https://www.youtube.com/watch?v=V9DWKbalbWQ

- https://www.enterprisedb.com/downloads/postgres-postgresql-downloads



