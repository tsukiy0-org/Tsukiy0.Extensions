using System.Threading.Tasks;
using Tsukiy0.Extensions.Processor.Models;
using Tsukiy0.Extensions.Processor.Services;

namespace Tsukiy0.Extensions.Processor.Aws.Runtimes
{
    public abstract class LambdaProcessorRuntime<TEvent, T>
    {
        private readonly IProcessor<T> processor;

        public LambdaProcessorRuntime(IProcessor<T> processor)
        {
            this.processor = processor;
        }

        public async Task Run(TEvent e)
        {
            var message = await ToMessage(e);
            await processor.Run(message);
        }

        abstract protected Task<Message<T>> ToMessage(TEvent e);
    }
}
