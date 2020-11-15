using System;
using System.Threading.Tasks;
using GrainInterface;
using Orleans;
using Orleans.Runtime;

namespace GrainImpl
{
    public class RateLimitGrain : Orleans.Grain, IRateLimit
    {
        private int numDropsInBucket = 0;
        private readonly int BUCKET_SIZE_IN_DROPS = 30;
        // 30 drops per minutes
        private readonly double DROPS_LEAK_PER_MS = 0.0005;
        private DateTime timeOfLastDropLeak = DateTime.Now;

        public RateLimitGrain()
        {
        }

        public Task<bool> CheckRateLimit()
        {
            DateTime now = DateTime.Now;

            double seconds = now.Subtract(timeOfLastDropLeak).TotalMilliseconds;
            long numberToLeak = (long)(seconds * DROPS_LEAK_PER_MS);
            if (numberToLeak > 0)
            {
                if (numDropsInBucket < numberToLeak)
                    numDropsInBucket = 0;
                else
                    numDropsInBucket -= (int)numberToLeak;
            }
            timeOfLastDropLeak = now;
            if (numDropsInBucket > BUCKET_SIZE_IN_DROPS)
            {
                numDropsInBucket++;
                return Task.FromResult(false);
            }

            // rate limit is ok now
            return Task.FromResult(true);
        }
    }
}
