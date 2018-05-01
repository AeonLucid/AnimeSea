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
