using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Tsukiy0.Extensions.Data.Aws.Services;
using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.Models;
using Tsukiy0.Extensions.Data.Models;
using Tsukiy0.Extensions.Data.Services;

namespace Tsukiy0.Extensions.Data.Aws.Services
{
    public abstract class AbstractDynamoVersionDaoMapper<T> : AbstractVersionDaoMapper<T, Dictionary<string, AttributeValue>>, IDynamoDaoMapper<T>
    {
        protected AbstractDynamoVersionDaoMapper(IList<VersionMapper<T, Dictionary<string, AttributeValue>>> versionMappers) : base(versionMappers)
        {
        }

        protected override DaoVersion ToDaoVersion(Dictionary<string, AttributeValue> u)
        {
            var dao = u.FromAttributeMap<DynamoDao>();
            return new DaoVersion
            (
                Version: dao.__VERSION,
                Type: dao.__TYPE
            );
        }

        private record DynamoDao(string __PK, string __SK, string __TYPE, int __VERSION) : IDynamoDao { }
    }
}
