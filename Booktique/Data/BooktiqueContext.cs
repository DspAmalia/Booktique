using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Booktique.Models.MainModels;

public class BooktiqueContext : DbContext
{
    public BooktiqueContext(DbContextOptions<BooktiqueContext> options)
        : base(options)
    {
    }

    public DbSet<Booktique.Models.MainModels.User> User { get; set; } = default!;
    public DbSet<Booktique.Models.MainModels.Book> Book { get; set; } = default!;
    public DbSet<Booktique.Models.MainModels.Review> Review { get; set; } = default!;
    public DbSet<Booktique.Models.MainModels.Favorite> Favorite { get; set; } = default!;
    public DbSet<Booktique.Models.MainModels.Folder> Folder { get; set; } = default!;
    public DbSet<Booktique.Models.MainModels.OrderItem> OrderItem { get; set; } = default!;
    public DbSet<Booktique.Models.MainModels.Order> Order { get; set; } = default!;
    public DbSet<Booktique.Models.MainModels.AntiqueListing> AntiqueListing { get; set; } = default!;

}
