using CsvHelper;
using Search.Models.Elasticsearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Models.Csv
{
	public static class CsvProcessor
	{

		public static List<CapitalSearchDocument> ProcessCsvFiles(IFileSystem fileSystem)
		{
			var file = fileSystem.File.Open("capital_cities.csv", FileMode.Open);
			StreamReader streamReader = new StreamReader(file);
			var csv = new CsvReader(streamReader);
			
				// describe's the csv file
				csv.Configuration.RegisterClassMap<CapitalCitiesMapping>();

				return csv
					.GetRecords<CapitalCityRecord>()
					.Select(record => new CapitalSearchDocument(record))
					.ToList();

				
		}

	}
}
