using System.Collections.Generic;
using System.Threading.Tasks;

namespace iADAATPA.MTProvider.API
{
    public interface IClient
    {
        Task<List<string>> Translate(List<string> sources, string source, string target);
        Task<bool> ValidateToken(string authToken);
    }
}