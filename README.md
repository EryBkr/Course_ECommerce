## PORTS
* http://localhost:5000 Gateway
* http://localhost:5011 Catalog API
* http://localhost:5001 Auth Server
* http://localhost:5012 Photo Stock Server
* http://localhost:5013 Basket Server
* http://localhost:5014 Discount Server
* http://localhost:5015 Order API
* http://localhost:5016 Payment API
* http://localhost:5010 Client (Net Core MVC)

## PORTAINER IMAGE
* `docker volume create portainer_data`
* `docker run -d -p 8000:8000 -p 9000:9000 --name=portainer --restart=always -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer-ce`
* http://localhost:9000/#!/home (Portainer Url)

## DATABASES
* MongoDb App Settings (Catalog API & MongoDb Driver): `mongodb://localhost:27017` (Container ayağa kalkarken bu portu belirledim) Kaydetme işlemi yaptığımızda Database otomatik oluşacaktır.Remote db bilgilerine `MongoDbCompoass` üzerinden ulaşıyorum
* SQL Server Linux (Authentication Service & EFCore_Identity Library): `"docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password123*" -p 1433:1433 --name identitydb -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu"` `Connection String:"Server=localhost,1433;Database=identitydb;User=sa;Password=Password123*"` `DbBeaver` üzerinden ulaşıyorum.Uzak bağlantı kabul ediyor
* Redis (Basket API & Redis StackExchange): Docker portainer üzerinden ayağa kaldırdık localhost:6379 (Dataları Redis CLI ile görebileceğimiz gibi `Another Redis Desktop Manager` ile de görebiliriz)
* PostgreSql (Discount Server & DapperContrib & NpSql Connection) : Portainer aracılığıyla localhost:5432 adresinden container olarak ayağa kaldırdık.admin:Password123* kullanıcısına sahip olarak oluşturduk
* SQL Server Linux (Order Service & Ef_Core Library): `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password123*" -p 1444:1433 --name orderdb -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu"` Connection String:`"Server=localhost,1444;Database=orderdb;User=sa;Password=Password123*"` `DbBeaver` üzerinden ulaşıyorum.Uzak bağlantı kabul ediyor (Yukarıda ki MSSQL den farklı portta ayağa kaldırdım)

## MQ EVENT
RabbitMQ:docker run -d -p 5672:5672 -p 15672:15672 --name rabbitmqcontainer rabbitmq:3-management (portainer ı ve servisi ayağa kaldırıyorum)


