using System.Collections.Generic;
using System.Threading;
using Amazon.DynamoDBv2.Model;

namespace Tsukiy0.Extensions.Data.Aws.Extensions
{
    public class DynamoEnumerable : IAsyncEnumerable<Dictionary<string, AttributeValue>>
    {
        public IAsyncEnumerator<Dictionary<string, AttributeValue>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }

}
