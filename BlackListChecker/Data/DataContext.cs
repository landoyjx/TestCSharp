using System;
using Microsoft.EntityFrameworkCore;


namespace BlackListChecker.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<BlackListItem> BlackLists { get; set; }
    }
}
