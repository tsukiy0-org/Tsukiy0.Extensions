FROM mcr.microsoft.com/dotnet/runtime:6.0

WORKDIR /var/task
COPY ./bin/Release/net6.0/linux-x64/publish .

CMD ["dotnet", "Tsukiy0.Extensions.Example.Processor.Aws.Batch.dll"]
