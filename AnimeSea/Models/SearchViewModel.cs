using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
