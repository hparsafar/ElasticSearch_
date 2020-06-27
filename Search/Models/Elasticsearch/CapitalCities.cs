using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Nest;
using Search.Models.Csv;

namespace Search.Models.Elasticsearch
{
    public class CapitalCities
    {
        public const string IndexName = "capitals";
        private readonly IFileSystem fileSystem;
        private IElasticClient client;

        public CapitalCities(IElasticClient client, IFileSystem _fileSystem)
        {
            this.client = client;
            fileSystem = _fileSystem;
        }

        public async Task RunAsync()
        {
            // if the index exists, let's delete it
            // you probably don't want to do this kind of
            // index management in a production environment
            var index = await client.Indices.ExistsAsync(IndexName);

            if (index.Exists)
            {
                await client.Indices.DeleteAsync(IndexName);
            }

            // let's create the index
            var createResult =
                await client.Indices.CreateAsync(IndexName, c => c
                    .Settings(s => s
                        .Analysis(a => a
                            // our custom search analyzer
                            .AddSearchAnalyzer()
                        )
                    )
                .Map<CapitalSearchDocument>(m => m.AutoMap())
            );


            
            // let's load the data
            var file = fileSystem.File.Open("capital_cities.csv", FileMode.Open);
            using (var csv = new CsvReader(new StreamReader(file)))
            {
                // describe's the csv file
                csv.Configuration.RegisterClassMap<CapitalCitiesMapping>();

                var records = csv
                    .GetRecords<CapitalCityRecord>()
                    .Select(record => new CapitalSearchDocument(record))
                    .ToList();

                // we are pushing all the data in at once
                var bullkResult =
                    await client
                    .BulkAsync(b => b
                        .Index(IndexName)
                        .CreateMany(records)
                    );
            }
        }
    }
}