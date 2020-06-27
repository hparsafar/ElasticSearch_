
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;
using Search.Models.Csv;
using Search.Models.Elasticsearch;

namespace Search.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ElasticClient client;

		public ISearchResponse<CapitalSearchDocument> Search { get; set; }
		public bool HasSearch => Search != null;

		[BindProperty(SupportsGet = true)]
		public string Term { get; set; }


		[BindProperty(SupportsGet = true)]
		public bool IsFuzzy { get; set; }

		public IndexModel(ElasticClient client)
		{
			this.client = client;
		}




		public void OnGet()
		{

			if (!string.IsNullOrWhiteSpace(Term))
			{

				if (!string.IsNullOrWhiteSpace(Term))
				{
					if (IsFuzzy == true)
					{
						Search =
							client.Search<CapitalSearchDocument>(s =>
								s.Query(q => q
										.Match(m => m
											.Field(f => f.Names)
											.Query(Term)
											.Fuzziness(Fuzziness.EditDistance(1))
										)
									)
									.Take(10)
							);
					}
					else
					{
						Search =
							client.Search<CapitalSearchDocument>(s =>
								s.Query(q => q
										.Match(m => m
											.Field(f => f.Names)
											.Query(Term)
											.Fuzziness(Fuzziness.EditDistance(0))
										)
									)
									.Take(10)
							);
					}
				}

			}
		}


		public void OnGetSearch()
		{

		}

		public void OnGetFuzzySearch()
		{
			if (!string.IsNullOrWhiteSpace(Term))
			{
				Search =
					client.Search<CapitalSearchDocument>(s =>
						s.Query(q => q
								.Match(m => m
									.Field(f => f.Names)
									.Query(Term)
									.Fuzziness(Fuzziness.EditDistance(1))
								)
							)
							.Take(10)
					);
			}
		}

		public void OnPostWork3()
		{
			//  Msg = "Work 3";
		}
		public void OnGetWork1(string type)
		{
			if (type == "FuzzySearch")
			{
				if (!string.IsNullOrWhiteSpace(Term))
				{
					Search =
						client.Search<CapitalSearchDocument>(s =>
							s.Query(q => q
									.Match(m => m
										.Field(f => f.Names)
										.Query(Term)
										.Fuzziness(Fuzziness.EditDistance(1))
									)
								)
								.Take(10)
						);
				}
			}
		}

		public string MapImageUrl(CapitalCityRecord result)
		{
			var location = string.Join(
				",", result.Latitude.ToString(), result.Longitude.ToString());

			return
				$"https://open.mapquestapi.com/staticmap/v5/map?key=nE1tqzT6DEcVhUw7e8T1ll6WRnW8afQM&center={location}&size=600,400@2x";
		}
	}
}