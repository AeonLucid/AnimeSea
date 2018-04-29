using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeSea.Metadata;
using AnimeSea.Metadata.Providers;
using AnimeSea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeSea.Controllers.Web
{
    [Route("Search")]
    public class SearchController : Controller
    {
        private readonly MetadataManager _metadataManager;

        public SearchController(MetadataManager metadataManager)
        {
            _metadataManager = metadataManager; // Temporarily.
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

            var metadataProvider = _metadataManager.MetadataProviders[providerId].Instance;
            var searchResults = await metadataProvider.SearchAsync(query);

            return Ok(searchResults);
        }
    }
}
