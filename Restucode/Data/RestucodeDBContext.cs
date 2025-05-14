using Microsoft.EntityFrameworkCore;
using Restucode.Data.Entities;

namespace Restucode.Data
{
    public class RestucodeDBContext: DbContext
    {
        public RestucodeDBContext(DbContextOptions<RestucodeDBContext> options) : base(options)
        {
        }

        public DbSet<CategoryEntity> Categories { get; set; } = null!;
    }
}
