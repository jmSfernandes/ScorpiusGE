#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# this file should be moved to the solution dir... Only here to be added to the template;

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Scorpius/Scorpius.csproj", "Scorpius/"]
RUN dotnet restore "Scorpius/Scorpius.csproj"
COPY . .
WORKDIR "/src/Scorpius"
RUN dotnet build "Scorpius.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Scorpius.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scorpius.dll"]