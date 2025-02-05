using TagsApi.Models;

namespace TagsApi.Services
{
    public class InitializationHostedService : IHostedService
    {
        private readonly IInitializeService _initializeService;

        public InitializationHostedService(IInitializeService initializeService)
        {
            _initializeService = initializeService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var result = await _initializeService.InitializeAsync(new InitializeRequest() { DeleteExisting = true, MinValues = 1000 });
            if (result.IsSuccess)
            {
                Console.WriteLine("Inicjalizacja zakończona pomyślnie");
            }
            else
            {
                Console.WriteLine($"Błąd inicjalizacji: {result.Error}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}