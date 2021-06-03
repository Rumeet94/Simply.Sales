#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Simply.Sales.TelegramBot/Simply.Sales.TelegramBot.csproj", "Simply.Sales.TelegramBot/"]
COPY ["Simply.Sales.BL/Simply.Sales.BLL.csproj", "Simply.Sales.BL/"]
COPY ["Simply.Sales.DLL/Simply.Sales.DLL.csproj", "Simply.Sales.DLL/"]
RUN dotnet restore "Simply.Sales.TelegramBot/Simply.Sales.TelegramBot.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Simply.Sales.TelegramBot/Simply.Sales.TelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Simply.Sales.TelegramBot/Simply.Sales.TelegramBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simply.Sales.TelegramBot.dll"]