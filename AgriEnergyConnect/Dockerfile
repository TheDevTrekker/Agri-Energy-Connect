# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore dependencies
COPY . .
RUN dotnet restore

# Build and publish the app to the /app folder
RUN dotnet publish -c Release -o /app

# Use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app .

# Set the entry point
ENTRYPOINT ["dotnet", "AgriEnergyConnect.dll"]
