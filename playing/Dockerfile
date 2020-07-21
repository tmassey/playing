FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
RUN mkdir -p /root/src/app  
WORKDIR /root/src/app
EXPOSE 17209

COPY playing playing
COPY Phoenix.Api.Core Phoenix.Api.Core

FROM base AS publish
WORKDIR /root/src/app/playing
#RUN dotnet restore ./playing.csproj
RUN dotnet publish --runtime linux-arm --self-contained false --framework netcoreapp3.1 -c Release -o published 
#RUN dotnet publish --self-contained false --framework netcoreapp3.1 -c Release -o published 


FROM publish AS final
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster-arm32v7
WORKDIR /root/
COPY --from=publish /root/src/app/playing/published .
ENTRYPOINT ["dotnet", "./playing.dll"]