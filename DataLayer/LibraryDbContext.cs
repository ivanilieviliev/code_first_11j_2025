namespace DataLayer;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext() : base()
    {

    }

    public LibraryDbContext(DbContextOptions options)
    : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("Server=127.0.0.1;Database=LibraryDb;Uid=root;Pwd=root;");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Customer> Customers { get; set; }


}
