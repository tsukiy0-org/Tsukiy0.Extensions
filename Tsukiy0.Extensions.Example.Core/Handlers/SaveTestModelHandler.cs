using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Tsukiy0.Extensions.Example.Core.Models;
using Tsukiy0.Extensions.Example.Core.Services;

namespace Tsukiy0.Extensions.Example.Core.Handlers
{
    public record SaveTestModelRequest(TestModel TestModel) : IRequest<Unit> { }

    public class SaveTestModelHandler : IRequestHandler<SaveTestModelRequest, Unit>
    {
        private readonly ITestModelRepository _repo;

        public SaveTestModelHandler(ITestModelRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(SaveTestModelRequest request, CancellationToken cancellationToken)
        {
            await _repo.PutAll(new List<TestModel> { request.TestModel });
            return Unit.Value;
        }
    }
}
