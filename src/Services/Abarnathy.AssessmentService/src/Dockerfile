FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Abarnathy.AssessmentService/src/Abarnathy.AssessmentService.csproj", "Services/Abarnathy.AssessmentService/"]
RUN dotnet restore "Services/Abarnathy.AssessmentService/Abarnathy.AssessmentService.csproj"
COPY . .
WORKDIR "/src/Services/Abarnathy.AssessmentService"
RUN dotnet build "Abarnathy.AssessmentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abarnathy.AssessmentService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Services/Abarnathy.AssessmentService/src/. .
ENTRYPOINT ["dotnet", "Abarnathy.AssessmentService.dll"]