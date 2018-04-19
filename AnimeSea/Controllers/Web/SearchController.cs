using System.Threading.Tasks;
using AnimeSea.Metadata;
using AnimeSea.Metadata.Providers;
using Microsoft.AspNetCore.Mvc;

namespace AnimeSea.Controllers.Web
{
    [Route("Search")]
    public class SearchController : Controller
    {
        private readonly MetadataProviderBase _metadataProvider;

        public SearchController(MetadataManager metadataManager)
        {
            _metadataProvider = metadataManager.MetadataProviders[0].Instance; // Temporarily.
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Submit")]
        public async Task<IActionResult> Submit([FromQuery(Name = "q")] string query)
        {
            var searchResults = await _metadataProvider.SearchAsync(query);

            return Ok(searchResults);
        }
    }
}
