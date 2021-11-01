using System.Threading.Tasks;
using Tsukiy0.Extensions.Example.Core.Handlers;
using Tsukiy0.Extensions.Example.Infrastructure.Services;
using Tsukiy0.Extensions.Processor.Aws.Runtimes;

namespace Tsukiy0.Extensions.Example.Processor.Aws.Batch
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var runtime = new BatchProcessorRuntime<SaveTestModelRequest>(new SaveTestModelProcessor());
            await runtime.Run();
        }
    }
}
