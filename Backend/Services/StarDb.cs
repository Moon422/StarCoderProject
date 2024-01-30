using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class StarDb : DbContext
{
    public StarDb(DbContextOptions options) : base(options)
    {
    }
}
