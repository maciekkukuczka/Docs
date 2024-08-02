using Docs.Modules.Shortcuts;

namespace Docs.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
{
    string? currentUserId => httpContextAccessor.HttpContext?.User
        .FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public DbSet<Doc> Docs { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<DocPath> DocPaths { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Shortcut>Shortcuts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        

        // builder.Entity<Doc>().ComplexProperty(x => x.Path);
        builder.Entity<Doc>().HasMany(x => x.Subjects).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Categories).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Images).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Links).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Notes).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.RelatedDocs).WithMany(x => x.Docs);
        builder.Entity<Doc>().HasMany(x => x.Tags).WithMany(x => x.Docs);

        builder.Entity<Doc>().HasOne(x => x.Path).WithOne(x => x.Doc).HasForeignKey<DocPath>(x => x.DocId);
        
        builder.Entity<ApplicationUser>().HasMany(x => x.Subjects)
            .WithOne(x => x.User).HasForeignKey(x => x.UserId);


        // GLOBAL FILTERS
        if (!string.IsNullOrWhiteSpace(currentUserId))
        {
            /*builder.Entity<Doc>().HasQueryFilter(x => x.Subjects.Any(x =>
                x.UserId == currentUserId));*/
            // builder.Entity<Doc>().HasQueryFilter(x=>x.Subjects.Where(x=>x.UserId == currentUserId));
            
            builder.Entity<Subject>().HasQueryFilter(x=>x.UserId == currentUserId);
        }
        
        /*
        builder.Entity<Subject>(x=>
        {
            x.Property(w => w.Id).ValueGeneratedOnAdd();
            x.Property(w => w.Name).;
        });
    */
    }
}