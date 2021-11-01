FROM mcr.microsoft.com/dotnet/runtime:5.0

WORKDIR /var/task
COPY ./bin/Release/net5.0/linux-x64/publish .

CMD ["dotnet", "Tsukiy0.Extensions.Example.Processor.Aws.Batch.dll"]
