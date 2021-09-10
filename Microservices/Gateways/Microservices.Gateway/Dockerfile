#projenin derlenebilmesi sdk imajına ihtiyaç vardır
#bu imaja tekrar erişebilmek için build ismini veriyoruz
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

#app klasörünün içerisine konumlanıyoruz
WORKDIR /app

#Container içerisinde ki port numarasını belirledim
#Dış dünyaya açılacak port numarası değildir
EXPOSE 80


#Gateway projesinin DLL ini Container içerisine kopyaladım
COPY Gateways/Microservices.Gateway/*.csproj Gateways/Microservices.Gateway/

#DLL dosyasını restore ediyorum
RUN dotnet restore Gateways/Microservices.Gateway/*.csproj

COPY . .

#DLL Release modunda out klasörüne publish ediyorum
RUN dotnet publish Gateways/Microservices.Gateway/*.csproj -c Release -o out

#SDK ile işim bitti.Artık projenin ayağa kalkması için runtime lazım
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime

#yeni imajımda konuçlanacağım yeri seçtim
WORKDIR /app

#SDK ile oluşmuş olan publish klasörünün içeriğini runtime imaj içerisinde ki app içerisine kopyala
COPY --from=build /app/out .

#Oluşan DLL i çalıştırıyoruz
ENTRYPOINT [ "dotnet","Microservices.Gateway.dll" ]

