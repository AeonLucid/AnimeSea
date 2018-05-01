using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnimeSea.Models
{
    public class SearchViewModel
    {
        [Display(Name = "Provider")]
        public int SelectedSearchProviderId { get; set; }

        public SelectList SearchProviders { get; set; }
    }
}
