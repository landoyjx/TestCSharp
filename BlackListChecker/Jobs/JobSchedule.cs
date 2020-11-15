using System;
namespace BlackListChecker.Jobs
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression)
        {
            this.JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
            CronExpression = cronExpression ?? throw new ArgumentNullException(nameof(cronExpression));
        }

        public Type JobType { get; private set; }
        public string CronExpression { get; private set; }
        public JobStatus JobStatus { get; set; } = JobStatus.Init;
    }

    public enum JobStatus : byte
    {
        Init = 0,
        Running = 1,
        Scheduling = 2,
        Stopped = 3,

    }
}