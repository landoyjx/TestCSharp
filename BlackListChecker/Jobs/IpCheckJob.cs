using System;
using System.Threading.Tasks;
using BlackListChecker.Data;
using BlackListChecker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BlackListChecker.Jobs
{
    [DisallowConcurrentExecution]
    public class IpCheckJob : IJob
    {
        private readonly ILogger<IpCheckJob> _logger;
        private readonly IServiceProvider _provider;

        public IpCheckJob(ILogger<IpCheckJob> logger, IServiceProvider provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider;
        }

        public static DateTime ToDateTime(long seconds)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            epoch.AddSeconds(seconds);
            return epoch;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using(var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DataContext>();
                foreach(var item in dbContext.BlackLists)
                {
                    Console.WriteLine(item.Ip);

                    if (item.Added)
                    {
                        DateTime now = DateTime.Now;

                        Console.WriteLine("last time=" + item.LastTime.ToString());
                        Console.WriteLine("now=" + now.ToString());

                        DateTime lastTime = ToDateTime(item.LastTime);
                        TimeSpan span = now.Subtract(lastTime);
                        if (span.Minutes >= 5)
                        {
                            // remove banned ip
                            if (IPSetService.removeFromBlackList(item.Ip) == 0)
                            {
                                dbContext.BlackLists.Remove(item);
                            }
                            
                        }
                    }
                    else
                    {
                        // ban ip
                        if (IPSetService.AddToBlackList(item.Ip) == 0)
                        {
                            // update item added
                            item.Added = true;
                            dbContext.BlackLists.Update(item);
                        }

                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
