using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor.Background
{
	public abstract class ScopedProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedProcessor(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await ProcessInScope(scope.ServiceProvider);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public abstract Task ProcessInScope(IServiceProvider serviceProvider);
    }
}
