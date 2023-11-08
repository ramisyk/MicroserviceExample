using Microsoft.EntityFrameworkCore;

namespace Stock.API.Models;

public class StockAPIDbContext : DbContext
{
    public StockAPIDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Entities.Stock> Stocks { get; set; }
}