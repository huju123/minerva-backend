
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Minerva_Backend/Minerva_Backend.csproj", "Minerva_Backend/"]
RUN dotnet restore "Minerva_Backend/Minerva_Backend.csproj"

COPY . .
WORKDIR /src/Minerva_Backend

RUN dotnet publish "Minerva_Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Minerva_Backend.dll"]
