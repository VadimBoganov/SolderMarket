using Microsoft.EntityFrameworkCore;

namespace solder.Models
{
    public class SolderContext : DbContext
    {
        public DbSet<SolderEntity> Entities {get; set;}
        public DbSet<SolderProduct> SolderProducts {get;set;}
        public DbSet<Solder> Solders {get;set;}
        public DbSet<SolderType> SolderTypes {get;set;}
        public DbSet<FileModel> Files {get;set;}

        public SolderContext(DbContextOptions options) 
            :base(options)
        {
            //Database.EnsureCreated();
        }            
    }
}