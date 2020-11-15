using System;
using System.Threading.Tasks;


namespace GrainInterface
{
    public interface IValue : Orleans.IGrainWithStringKey
    {
        Task<string> GetAsync();

        Task<bool> SetAsync(string value);
    }
}
