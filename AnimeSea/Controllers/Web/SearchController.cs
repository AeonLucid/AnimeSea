using System.Linq;
using System.Threading.Tasks;
using AnimeSea.Data.Entities;
using AnimeSea.Metadata;
using AnimeSea.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeSea.Controllers.Web
{
    [Route("Search")]
    public class SearchController : Controller
    {
        private readonly MetadataManager _metadataManager;
        private readonly LiteDatabase _database;

        public SearchController(MetadataManager metadataManager, LiteDatabase database)
        {
            _metadataManager = metadataManager;
            _database = database;
        }

        protected SelectList GetProviders()
        {
            var providers = _metadataManager.MetadataProviders
                .Select(x => new SelectListItem(x.Value.Attribute.Name, x.Key.ToString()));

            return new SelectList(providers, nameof(SelectListItem.Value), nameof(SelectListItem.Text));
        }

        [Route("")]
        public IActionResult Index()
        {
            return View(new SearchViewModel
            {
                SearchProviders = GetProviders()
            });
        }

        [Route("Submit")]
        public async Task<IActionResult> Submit([FromQuery(Name = "q")] string query, [FromQuery(Name = "pid")] int providerId = 0)
        {
            if (!_metadataManager.MetadataProviders.ContainsKey(providerId))
            {
                return BadRequest(new ErrorModel("Invalid provider ID"));
            }

            // Search the provider
            var metadataProvider = _metadataManager.MetadataProviders[providerId].Instance;
            var searchResults = await metadataProvider.SearchAsync(query);

            // Search internally
            var searchResultIds = searchResults.Results.Select(x => x.Id);
            var libraryResults = _database.GetCollection<Serie>()
                .Find(x => x.ProviderId == providerId && searchResultIds.Contains(x.ProviderSerieId));


            return Ok(new
            {
                searchResults.Next,
                LibraryResults = libraryResults,
                searchResults.Results
            });
        }
    }
}
