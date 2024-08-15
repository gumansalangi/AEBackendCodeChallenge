# Use the official .NET Core SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the csproj file and restore any dependencies (via nuget)
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining project files and build the project
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET Core runtime image as a runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port your app runs on
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "AEBackendCodeChallenge.dll"]
