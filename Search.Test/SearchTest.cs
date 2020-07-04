using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using NSubstitute;
using Search.Models.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions;
using System.IO;
using Microsoft.Extensions.DependencyModel;
using Search.Pages;
using System.Security.Policy;
using Search.Models.Csv;

namespace Search.Test
{
	[TestClass]
	public class SearchTest
	{


		[TestMethod]
		public void CapitalCities_must_call_BulkAsync_method()
		{
			var mockFileSystem = new MockFileSystem();
			var mockInputFile = new MockFileData("city,city_ascii,lat,lng,country,iso2,iso3,admin_name,capital,population,id\nPristina,Pristina,42.6666,21.1724,Kosovo,XK,XKS,Prishtinë,primary,,1901760068");
			mockFileSystem.AddFile(@"capital_cities.csv", mockInputFile);
			CapitalCityRecord record = new CapitalCityRecord() {
				City = "Pristina",
				Country = "Kosovo",
				Id = "1901760068",
				CityAscii= "Pristina",
				Population=null,
				Latitude = new decimal(42.6666),
				Longitude = new decimal(21.1724)
			};
			CapitalSearchDocument capitalSearch = new CapitalSearchDocument(record);
			List<CapitalSearchDocument> cp = new List<CapitalSearchDocument>() { capitalSearch };



			var parameter = CsvProcessor.ProcessCsvFiles(mockFileSystem);

			Assert.AreEqual(capitalSearch, parameter[0]);


		}


	}
}
