using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MNN.WebAPI.Helpers
{
    public class JobRunner : IHostedService, IDisposable
    {
        private readonly IServiceScope _scope;

        //private readonly IShopifyService _shopifyService;

        private Timer _timer;

        private int _executedCount = 0;

        public JobRunner(IServiceScopeFactory scopeFactory)
        {
            this._scope = scopeFactory.CreateScope();

          //  this._shopifyService = this._scope.ServiceProvider.GetService<IShopifyService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
           // this._timer = new Timer(async (state) => await this.RunJobAsync(state), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(300));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this._timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this._timer?.Dispose();
            this._scope?.Dispose();
        }

        //private async Task RunJobAsync(object state)
        //{
        //     await this._shopifyService.ExecuteAsync();
        //}
    }
}
