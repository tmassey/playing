FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /app
EXPOSE 17209

FROM base AS build
WORKDIR /src
COPY . .
WORKDIR /src/playing

FROM build AS publish
WORKDIR /src/playing/playing
RUN dotnet publish playing.csproj --runtime linux-arm --self-contained false --framework netcoreapp3.1 -c Release -o /app/publish

FROM publish AS final
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster-arm32v7
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "./playing.dll"]