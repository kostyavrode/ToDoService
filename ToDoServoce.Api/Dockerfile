FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ToDoService.Domain/ToDoService.Domain.csproj", "ToDoService.Domain/"]
COPY ["ToDoService.Application/ToDoService.Application.csproj", "ToDoService.Application/"]
COPY ["ToDoService.Infrastructure/ToDoService.Infrastructure.csproj", "ToDoService.Infrastructure/"]
COPY ["ToDoServoce.Api/ToDoServoce.Api.csproj", "ToDoServoce.Api/"]

RUN dotnet restore "ToDoServoce.Api/ToDoServoce.Api.csproj"

COPY . .
WORKDIR "/src/ToDoServoce.Api"
RUN dotnet build "ToDoServoce.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoServoce.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoServoce.Api.dll"]
