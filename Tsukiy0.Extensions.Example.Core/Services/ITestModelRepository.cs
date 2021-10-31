using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tsukiy0.Extensions.Example.Core.Models;

namespace Tsukiy0.Extensions.Example.Core.Services
{
    public interface ITestModelRepository
    {
        Task DeleteAll(IEnumerable<TestModel> models);
        Task PutAll(IEnumerable<TestModel> models);
        Task<IEnumerable<TestModel>> QueryByNamespace(Guid ns);
        Task<IEnumerable<TestModel>> ScanByNamespace(Guid ns);
    }
}
