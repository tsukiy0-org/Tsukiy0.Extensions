using System.Threading.Tasks;

namespace Tsukiy0.Extensions.Data.Services
{
    public interface IDaoMapper<T, U>
    {
        Task<U> To(T destination);
        Task<T> From(U source);
    }

}