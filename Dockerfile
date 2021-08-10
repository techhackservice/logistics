FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 4001
COPY ./src ./
RUN dotnet restore && dotnet build


RUN dotnet publish Web.Api -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Web.Api.dll"]
#docker build -t logisticmateservice:dev  --network=host .
#docker run -d -p 4001:4001 --name logisticmateservice logisticmateservice:dev
#docker rm -f logisticmateservice