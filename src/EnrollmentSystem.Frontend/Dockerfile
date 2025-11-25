FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/EnrollmentSystem.Frontend/EnrollmentSystem.Frontend.csproj", "src/EnrollmentSystem.Frontend/"]
COPY ["src/EnrollmentSystem.Shared/EnrollmentSystem.Shared.csproj", "src/EnrollmentSystem.Shared/"]
RUN dotnet restore "src/EnrollmentSystem.Frontend/EnrollmentSystem.Frontend.csproj"
COPY . .
WORKDIR "/src/src/EnrollmentSystem.Frontend"
RUN dotnet build "EnrollmentSystem.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnrollmentSystem.Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnrollmentSystem.Frontend.dll"]