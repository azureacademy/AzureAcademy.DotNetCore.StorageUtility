FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore "AzureAcademy.DotNetCore.StorageUtility.csproj"

WORKDIR "/src"
RUN dotnet build "AzureAcademy.DotNetCore.StorageUtility.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AzureAcademy.DotNetCore.StorageUtility.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AzureAcademy.DotNetCore.StorageUtility.dll"]

