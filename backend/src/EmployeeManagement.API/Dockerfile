#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EmployeeManagement.Core/EmployeeManagement.Core.csproj", "EmployeeManagement.Core/"]
COPY ["EmployeeManagement.DataAccess/EmployeeManagement.DataAccess.csproj", "EmployeeManagement.DataAccess/"]
COPY ["EmployeeManagement.Service/EmployeeManagement.Service.csproj", "EmployeeManagement.Service/"]
COPY ["EmployeeManagement.API/EmployeeManagement.API.csproj", "EmployeeManagement.API/"]
RUN dotnet restore "EmployeeManagement.API/EmployeeManagement.API.csproj"

COPY . .
WORKDIR "/src/EmployeeManagement.API" 
RUN dotnet build "EmployeeManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeManagement.API.csproj" -c Release -o /app/publish

FROM base AS final

# Change timezone to local time
ENV TZ=Asia/Baku
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagement.API.dll"]