#projenin derlenebilmesi sdk imajına ihtiyaç vardır
#bu imaja tekrar erişebilmek için build ismini veriyoruz
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

#app klasörünün içerisine konumlanıyoruz
WORKDIR /app

#Container içerisinde ki port numarasını belirledim
#Dış dünyaya açılacak port numarası değildir
EXPOSE 80

#Dosya yolubu DockerCompose file a göre belirledim.Çalışacağı nokta orası çünkü
#Shared bu projede dll olarak ekli olduğu için container içerisine de 
#kopyalamam gerekiyordu
COPY Shared/Microservices.Shared/*.csproj Shared/Microservices.Shared/

#Yine Docker-Compose dosyamızın yoluna göre diğer katmanlara ulaşarak onları da image 'e dahil ediyoruz
COPY Services/Order/Microservices.Services.Order.Application/*.csproj Services/Order/Microservices.Services.Order.Application/
COPY Services/Order/Microservices.Services.Order.Domain/*.csproj Services/Order/Microservices.Services.Order.Domain/
COPY Services/Order/Microservices.Services.Order.Domain.Core/*.csproj Services/Order/Microservices.Services.Order.Domain.Core/
COPY Services/Order/Microservices.Services.Order.Infrastructure/*.csproj Services/Order/Microservices.Services.Order.Infrastructure/


#Order projesinin DLL ini Container içerisine kopyaladım
COPY Services/Order/Microservices.Services.Order.API/*.csproj Services/Order/Microservices.Services.Order.API/

#DLL dosyasını restore ediyorum
RUN dotnet restore Services/Order/Microservices.Services.Order.API/*.csproj

COPY . .

#DLL Release modunda out klasörüne publish ediyorum
RUN dotnet publish Services/Order/Microservices.Services.Order.API/*.csproj -c Release -o out

#SDK ile işim bitti.Artık projenin ayağa kalkması için runtime lazım
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime

#yeni imajımda konuçlanacağım yeri seçtim
WORKDIR /app

#SDK ile oluşmuş olan publish klasörünün içeriğini runtime imaj içerisinde ki app içerisine kopyala
COPY --from=build /app/out .

#Oluşan DLL i çalıştırıyoruz
ENTRYPOINT [ "dotnet","Microservices.Services.Order.API.dll" ]

