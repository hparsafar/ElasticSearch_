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

namespace Search.Test
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public async Task CapitalCities_must_call_BulkAsync_method()
		{


			var mockFileSystem = new MockFileSystem();

			var mockInputFile = new MockFileData("city,city_ascii,lat,lng,country,iso2,iso3,admin_name,capital,population,id\nPristina,Pristina,42.6666,21.1724,Kosovo,XK,XKS,Prishtinë,primary,,1901760068");

			mockFileSystem.AddFile(@"capital_cities.csv", mockInputFile);

			var client = new ElasticClient(new Uri("http://localhost:9200"));
			var service = new CapitalCities(client,mockFileSystem);
			 List<CapitalSearchDocument> cp = new List<CapitalSearchDocument>();
			await service.RunAsync();
			
			await client.Received().BulkAsync(b => b.Index("capital")
				.CreateMany(Arg.Any<List<CapitalSearchDocument>>()));

		}


		[TestMethod]
		public async Task FuzzySearch_must_call_QueryMatch()
		{


			var mockFileSystem = new MockFileSystem();

			var mockInputFile = new MockFileData("city,city_ascii,lat,lng,country,iso2,iso3,admin_name,capital,population,id\nPristina,Pristina,42.6666,21.1724,Kosovo,XK,XKS,Prishtinë,primary,,1901760068");

			mockFileSystem.AddFile(@"capital_cities.csv", mockInputFile);

			var client = Substitute.For<IElasticClient>();
			//client. = new ConnectionSettings(new Uri("http://localhost:9200"));


			var service = new CapitalCities(client, mockFileSystem);
			List<CapitalSearchDocument> cp = new List<CapitalSearchDocument>();
			await service.RunAsync();

			await client.Received().BulkAsync(b => b.Index("capital")
				.CreateMany(Arg.Any<List<CapitalSearchDocument>>()));

		}



		//[TestMethod]
		//public async Task DeleteMessageAsync_MessageIsDeleted_WhenMessageIsFoundAsync()
		//{

		//	var client = new ElasticClient(new Uri("http://localhost:9200"));
		//	var model = new IndexModel(client);


		//	model.OnGet();

		//	//Assert

		//}
	}
}
