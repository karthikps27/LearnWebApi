using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BookDataService.Service;

namespace BookMaster.Data.Framework
{
    public class InputDataPoller : IHostedService
    {
        private readonly ILogger _logger;
        private readonly S3FileProcessorService _s3FileProcessorService;

        public InputDataPoller(ILogger<InputDataPoller> logger, S3FileProcessorService s3FileProcessorService)
        {
            _logger = logger;
            _s3FileProcessorService = s3FileProcessorService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var results = await _s3FileProcessorService.GetAllFiles();
                    if (!results.Any())
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10));
                        continue;
                    }

                    //TODO: minimize log
                    _logger.LogInformation($"S3 Bucket Results: {{results}}");

                    foreach (var s3Item in results)
                    {
                        await _s3FileProcessorService.ProcessFile(s3Item);
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"Unexpected error.");
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Nothing needed atm
            return Task.FromResult(0);
        }
    }
}
