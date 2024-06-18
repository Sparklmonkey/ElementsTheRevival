using System.Collections.Generic;
using System.Threading.Tasks;

namespace Networking
{
    public interface IApiRequest
    {
        public Task<TResponse> PerformRequestWithBody<TRequest, TResponse>(string actionUrl, TRequest requestBody);
        public Task<TResponse> PerformEmptyRequest<TResponse>(string actionUrl);
        public Task<TResponse> PerformEmptyRequestWithQueries<TResponse>(string actionUrl, Dictionary<string, string> queryList);
        // public Task<TRequest, TResponse> PerformRequestWithBody(TRequest request, TResponse )
    }
}