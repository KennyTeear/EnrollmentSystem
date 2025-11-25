FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/EnrollmentSystem.AuthService/EnrollmentSystem.AuthService.csproj", "src/EnrollmentSystem.AuthService/"]
COPY ["src/EnrollmentSystem.Shared/EnrollmentSystem.Shared.csproj", "src/EnrollmentSystem.Shared/"]
RUN dotnet restore "src/EnrollmentSystem.AuthService/EnrollmentSystem.AuthService.csproj"
COPY . .
WORKDIR "/src/src/EnrollmentSystem.AuthService"
RUN dotnet build "EnrollmentSystem.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnrollmentSystem.AuthService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnrollmentSystem.AuthService.dll"]