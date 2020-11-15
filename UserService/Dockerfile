# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

COPY . ./

ENV ASPNETCORE_URLS http://0.0.0.0:5001

EXPOSE 5001

ENTRYPOINT ["dotnet", "out/UserService.dll"]

