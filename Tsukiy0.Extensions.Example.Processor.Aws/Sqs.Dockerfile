FROM public.ecr.aws/lambda/dotnet:5.0

WORKDIR /var/task
COPY "bin/Release/net5.0/linux-x64/publish"  .

CMD ["Tsukiy0.Extensions.Example.Processor.Aws.Sqs::Tsukiy0.Extensions.Example.Processor.Aws.Sqs.Function::Run"]
