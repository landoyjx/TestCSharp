using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;

using GrainInterface;
using System.Net;

namespace UserService.Services
{
    public class OrleanService
    {
        private static IClusterClient _client;
        private static OrleanService instance = null;

        private OrleanService()
        {

        }

        public static async Task<OrleanService> GetInstance()
        {
            if (instance == null)
            {
                instance = new OrleanService();

                _client = new ClientBuilder()
                   .UseLocalhostClustering()
                   .Configure<ClusterOptions>(options =>
                   {
                       options.ClusterId = "dev";
                       options.ServiceId = "TestCSharpApp";
                   })
                   .ConfigureLogging(logging => logging.AddConsole())
                   .Build();
                await _client.Connect(CreateRetryFilter());
            }
            return instance;
        }

        private static Func<Exception, Task<bool>> CreateRetryFilter(int maxAttempts = 5)
        {
            var attempt = 0;
            return RetryFilter;

            async Task<bool> RetryFilter(Exception exception)
            {
                attempt++;
                Console.WriteLine($"Cluster client attempt {attempt} of {maxAttempts} failed to connect to cluster.  Exception: {exception}");
                if (attempt > maxAttempts)
                {
                    return false;
                }

                await Task.Delay(TimeSpan.FromSeconds(4));
                return true;
            }
        }

        public IValue GetValueGrain(string key)
        {
            IValue grain = _client.GetGrain<IValue>(key);
            return grain;
        }

        public IRateLimit GetRateLimit(string ip)
        {
            IRateLimit grain = _client.GetGrain<IRateLimit>(ip);
            return grain;
        }
    }
}
