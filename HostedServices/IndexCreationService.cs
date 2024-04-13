
using Redis.OM;
using RedisApp.Controllers;

namespace RedisApp
{
    public class IndexCreationService : IHostedService, IIndexCreationService
    {
        private readonly RedisConnectionProvider _provider;

        public IndexCreationService(RedisConnectionProvider provider)
        {
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _provider.Connection.CreateIndexAsync(typeof(Person));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public interface IIndexCreationService
    {
    }
}