using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WUM.Lib.Models.DB_Context;

public partial class ManagementContext : DbContext
{
    public ManagementContext()
    {
    }

    public ManagementContext(DbContextOptions<ManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WelldoneLog> WelldoneLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WelldoneLog>(entity =>
        {
            entity.ToTable("welldone_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.LogDate).HasColumnName("log_date");
            entity.Property(e => e.LogLevel)
                .HasMaxLength(50)
                .HasColumnName("log_level");
            entity.Property(e => e.LogMsg)
                .HasMaxLength(1000)
                .HasColumnName("log_msg");
            entity.Property(e => e.LogSource)
                .HasMaxLength(100)
                .HasColumnName("log_source");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
