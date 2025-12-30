# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first for better caching
COPY DailyJobTracker.csproj ./
RUN dotnet restore

# Copy the rest of the source
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Listen on port 80 inside container
ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80

ENTRYPOINT ["dotnet", "DailyJobTracker.dll"]
