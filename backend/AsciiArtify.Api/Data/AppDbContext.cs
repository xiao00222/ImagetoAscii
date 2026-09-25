using System;
using Microsoft.EntityFrameworkCore;

namespace AsciiArtify.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<ConversionRecord> Conversions => Set<ConversionRecord>();
}