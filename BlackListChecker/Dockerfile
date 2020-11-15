FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

COPY *.* ./

ENV ASPNETCORE_URLS http://0.0.0.0:5001

CMD ["dotnet" "bin/Debug/netcoreapp3.1/BlackListChecker.dll"]

