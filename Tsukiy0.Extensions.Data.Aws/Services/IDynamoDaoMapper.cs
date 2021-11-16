using System.Collections.Generic;

using Amazon.DynamoDBv2.Model;

using Tsukiy0.Extensions.Data.Services;

namespace Tsukiy0.Extensions.Data.Aws.Services
{
    public interface IDynamoDaoMapper<T> : IDaoMapper<T, Dictionary<string, AttributeValue>>
    {

    }

}