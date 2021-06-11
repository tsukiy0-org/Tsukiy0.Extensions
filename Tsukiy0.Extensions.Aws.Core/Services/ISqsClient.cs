using System.Collections.Generic;
using System.Threading.Tasks;
using Tsukiy0.AwsTools.Core.Models;

namespace Tsukiy0.Extensions.Aws.Core.Services
{
    public interface ISqsClient<T>
    {
        Task Send(IEnumerable<SendMessageEnvelope<T>> messages);
        Task<ReceiveMessageEnvelope<T>?> Receive();
        Task Delete(string receiptHandle);
    }
}
