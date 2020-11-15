using System;
using System.Threading.Tasks;

namespace GrainInterface
{
    public interface IRateLimit : Orleans.IGrainWithStringKey
    {
        Task<bool> CheckRateLimit();
    }
}
