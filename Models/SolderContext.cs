using Microsoft.EntityFrameworkCore;

namespace solder.Models
{
    public class SolderContext : DbContext
    {
        public DbSet<Solder> Solders {get;set;}
        public DbSet<FileModel> Files {get;set;}

        public SolderContext(DbContextOptions options) 
            :base(options)
        {
            Database.EnsureCreated();
        }            
    }
}