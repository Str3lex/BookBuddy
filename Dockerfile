FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["BookBuddy.csproj", "BookBuddy/"]
RUN dotnet restore "BookBuddy/BookBuddy.csproj"

COPY . .
WORKDIR "/src/BookBuddy"
RUN dotnet build "BookBuddy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookBuddy.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookBuddy.dll"]