FROM mcr.microsoft.com/dotnet/sdk:5.0-focal

RUN apt-get update && apt-get install -y jq
