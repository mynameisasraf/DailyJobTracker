# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./DailyJobTracker.sln ./
COPY ./DailyJobTracker ./DailyJobTracker

RUN dotnet restore DailyJobTracker/DailyJobTracker.csproj
RUN dotnet publish DailyJobTracker/DailyJobTracker.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "DailyJobTracker.dll"]
