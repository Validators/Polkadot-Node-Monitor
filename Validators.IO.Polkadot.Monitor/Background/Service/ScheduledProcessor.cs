using Cronos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor.Background
{
	public abstract class ScheduledProcessor : ScopedProcessor
    {
        private CronExpression _schedule;
        private DateTime? _nextRun;

        /// <summary>
        /// Scheduler that supports seconds time intevals
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public ScheduledProcessor(IServiceScopeFactory serviceScopeFactory, string schedule) : base(serviceScopeFactory)
        {
            _schedule = CronExpression.Parse(schedule, CronFormat.IncludeSeconds);
            _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.UtcNow;
                if (now > _nextRun)
                {
                    try
                    {
                        await Process();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
