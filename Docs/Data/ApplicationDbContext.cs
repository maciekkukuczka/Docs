namespace Docs.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Doc> Docs { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<DocPath> DocPaths { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Doc>().ComplexProperty(x => x.Path);
        
        builder.Entity<Doc>().HasMany(x => x.Categories).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Images).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Links).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Notes).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.RelatedDocs).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Tags).WithMany(x => x.Docs);
        
    }
    
}
