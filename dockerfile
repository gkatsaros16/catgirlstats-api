FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
# FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app
RUN apt-get update ;\
    apt-get install -y tzdata
EXPOSE 5000
# ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# ARG APPVERSION
# ARG NUGETSOURCES
WORKDIR /
COPY . .
RUN dotnet restore CatgirlStatsApi/CatgirlStatsApi.csproj
RUN dotnet restore CatgirlStatsLogic/CatgirlStatsLogic.csproj
RUN dotnet restore CatGirlStatsModels/CatGirlStatsModels.csproj

RUN dotnet build -c Release

FROM build AS publish
WORKDIR /CatgirlStatsApi/
RUN dotnet publish CatgirlStatsApi.csproj -c Release -o /app

FROM base AS final
ARG RUNTIMEENV
# ENV ASPNETCORE_ENVIRONMENT=${RUNTIMEENV}
# ENV TZ=America/New_York
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CatgirlStatsApi.dll"]

# 
# docker run --rm -it -p 8080:80 catgirl-service