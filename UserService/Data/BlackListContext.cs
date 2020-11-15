using System;
using Microsoft.EntityFrameworkCore;


namespace UserService.Data
{
    public class BlackListContext : DbContext
    {
        public BlackListContext(DbContextOptions<BlackListContext> options) : base(options)
        {

        }

        public DbSet<BlackListItem> BlackLists { get; set; }

    }
}
