FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Abarnathy.HistoryAPI/src/Abarnathy.HistoryAPI.csproj", "Abarnathy.HistoryAPI/"]
RUN dotnet restore "Abarnathy.HistoryAPI/Abarnathy.HistoryAPI.csproj"
COPY . .
WORKDIR "/src/Abarnathy.HistoryAPI"
RUN dotnet build "Abarnathy.HistoryAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abarnathy.HistoryAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Abarnathy.HistoryAPI/src/. .
#COPY Abarnathy.HistoryAPI/src/wait-for-it.sh ./wait-for-it.sh
#RUN chmod +x ./wait-for-it.sh
#ENTRYPOINT ["./wait-for-it.sh", "sql_server:1434", "-t", "40", "--", "dotnet", "Abarnathy.HistoryAPI.dll"]
ENTRYPOINT ["dotnet", "Abarnathy.HistoryAPI.dll"]