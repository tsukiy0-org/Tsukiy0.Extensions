FROM tsukiy0/dotnet6-runtime

WORKDIR /var/task
COPY ./bin/Release/net6.0/linux-x64/publish .

CMD ["Tsukiy0.Extensions.Example.AspNetCore::Tsukiy0.Extensions.Example.AspNetCore.Function::FunctionHandlerAsync"]
