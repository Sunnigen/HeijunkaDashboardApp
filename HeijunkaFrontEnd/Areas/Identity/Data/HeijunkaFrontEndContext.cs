using HeijunkaFrontEnd.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeijunkaFrontEnd.Models;

public class HeijunkaFrontEndContext : IdentityDbContext<HeijunkaUser>
{
    public HeijunkaFrontEndContext(DbContextOptions<HeijunkaFrontEndContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
        .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PostgreSQL uses the public schema by default - not dbo.
        modelBuilder.HasDefaultSchema("public");
        base.OnModelCreating(modelBuilder);

        //Rename Identity tables to lowercase
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetDefaultTableName();
            modelBuilder.Entity(entity.Name).ToTable(currentTableName.ToLower());
        }
    }
}