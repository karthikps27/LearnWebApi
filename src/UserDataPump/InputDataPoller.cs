using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UserDataPump.Framework
{
    public class InputDataPoller : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using StreamReader streamReader = new StreamReader("C:/repo/LearnWebApi/src/UserDataPump/Resources/BookResponse.json");           
            while (!cancellationToken.IsCancellationRequested)
            {
                var jsonInputReader = new JsonInputReader(streamReader);
                Console.WriteLine(jsonInputReader.GetAllDataFromJsonFile());
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
