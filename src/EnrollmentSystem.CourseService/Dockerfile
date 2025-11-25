FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/EnrollmentSystem.CourseService/EnrollmentSystem.CourseService.csproj", "src/EnrollmentSystem.CourseService/"]
COPY ["src/EnrollmentSystem.Shared/EnrollmentSystem.Shared.csproj", "src/EnrollmentSystem.Shared/"]
RUN dotnet restore "src/EnrollmentSystem.CourseService/EnrollmentSystem.CourseService.csproj"
COPY . .
WORKDIR "/src/src/EnrollmentSystem.CourseService"
RUN dotnet build "EnrollmentSystem.CourseService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnrollmentSystem.CourseService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnrollmentSystem.CourseService.dll"]