using System;
using Orleans;
using Orleans.Runtime;

using GrainInterface;
using System.Threading.Tasks;

namespace GrainImpl
{
    public class ValueGrain : Orleans.Grain, IValue
    {
        private string _value = "";

        public ValueGrain()
        {
        }

        Task<string> IValue.GetAsync()
        {
            return Task.FromResult(_value);
        }

        Task<bool> IValue.SetAsync(string value)
        {
            this._value = value;
            return Task.FromResult(true);
        }
    }
}
