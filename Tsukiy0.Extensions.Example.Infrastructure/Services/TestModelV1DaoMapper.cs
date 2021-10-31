using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Tsukiy0.Extensions.Data.Aws.Extensions;
using Tsukiy0.Extensions.Data.Aws.Models;
using Tsukiy0.Extensions.Data.Aws.Services;
using Tsukiy0.Extensions.Example.Core.Models;

namespace Tsukiy0.Extensions.Example.Infrastructure.Services
{
    public class TestModelV1DaoMapper : IDynamoDaoMapper<TestModel>
    {
        private record TestModelV1Dao(string __PK, string __SK, string __TYPE, int __VERSION, Guid Id, Guid Namespace) : IDynamoDao { }
        public async Task<TestModel> From(Dictionary<string, AttributeValue> source)
        {
            var dao = source.FromAttributeMap<TestModelV1Dao>();
            return new TestModel(dao.Id, dao.Namespace);
        }

        public async Task<Dictionary<string, AttributeValue>> To(TestModel destination)
        {
            return new TestModelV1Dao(
                __PK: destination.Namespace.ToString(),
                __SK: destination.Id.ToString(),
                __TYPE: "TEST",
                __VERSION: 1,
                Id: destination.Id,
                Namespace: destination.Namespace
            ).ToAttributeMap();
        }
    }
}
