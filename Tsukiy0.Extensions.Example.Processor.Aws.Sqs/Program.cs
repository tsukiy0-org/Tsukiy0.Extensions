using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Infrastructure.Services;
using Tsukiy0.Extensions.Messaging.Models;

namespace Tsukiy0.Extensions.Example.Processor.Aws.Sqs
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new SaveTestModelProcessor().Run(
                new Message<SaveTestModelRequest>(
                    new MessageHeader(1, Guid.NewGuid(), DateTimeOffset.UtcNow, new Dictionary<string, string>()),
                    new SaveTestModelRequest
                    {
                        TestModel = new Core.Models.TestModel
                        {
                            Id = Guid.NewGuid(),
                            Namespace = Guid.NewGuid()
                        }
                    }
                )
            );
        }
    }
}
