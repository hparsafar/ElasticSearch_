
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Search.Models.Elasticsearch;

namespace Search
{
    public class Program
    {
        // make sure your program is running C# 7.1 or later
        public static async Task Main(string[] args)
        {
            // setup host
            var host = CreateWebHostBuilder(args).Build();

            // load records from Csv to Elasticsearch
            using (var scope = host.Services.CreateScope())
            {
                var loader = scope.ServiceProvider.GetRequiredService<DataImporter>();
                await loader.RunAsync();
            }

            // change our run to async
            await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
