
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

        public IndexModel(ElasticClient client)
        {
            this.client = client;
        }

        public void OnGet()
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

        public string MapImageUrl(CapitalCityRecord result)
        {
            var location = string.Join(
                ",", result.Latitude.ToString(), result.Longitude.ToString());

            return
                $"https://open.mapquestapi.com/staticmap/v5/map?key=nE1tqzT6DEcVhUw7e8T1ll6WRnW8afQM&center={location}&size=600,400@2x";
        }
    }
}