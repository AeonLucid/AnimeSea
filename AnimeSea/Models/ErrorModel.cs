using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeSea.Models
{
    public class ErrorModel
    {
        public ErrorModel(string error = "")
        {
            Error = error;
        }

        public string Error { get; set; }
    }
}
