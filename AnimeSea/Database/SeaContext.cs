using Microsoft.EntityFrameworkCore;

namespace AnimeSea.Database
{
    public class SeaContext : DbContext
    {
        public SeaContext(DbContextOptions options) : base(options)
        {
        }
    }
}
