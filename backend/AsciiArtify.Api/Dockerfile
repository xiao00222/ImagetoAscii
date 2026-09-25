# STAGE 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AsciiArtify.Api.csproj", "./"]
RUN dotnet restore "./AsciiArtify.Api.csproj"

COPY . .
RUN dotnet publish "AsciiArtify.Api.csproj" -c Release -o /app/publish

# STAGE 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5225

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AsciiArtify.Api.dll"]