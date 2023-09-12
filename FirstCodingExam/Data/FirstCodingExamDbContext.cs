using System;
using System.Collections.Generic;
using FirstCodingExam.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstCodingExam.Data;

public partial class FirstCodingExamDbContext : DbContext
{
    public FirstCodingExamDbContext()
    {
    }

    public FirstCodingExamDbContext(DbContextOptions<FirstCodingExamDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CalculatedRecord> CalculatedRecords { get; set; }

    public virtual DbSet<HistoryRecord> HistoryRecords { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalculatedRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calculat__3214EC07621018E9");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.Record).WithMany(p => p.CalculatedRecords)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calculate__Recor__607251E5");
        });

        modelBuilder.Entity<HistoryRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HistoryR__3214EC07B0B97A7A");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.Record).WithMany(p => p.HistoryRecords)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HistoryRe__Recor__634EBE90");

            entity.HasOne(d => d.User).WithMany(p => p.HistoryRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HistoryRe__UserI__6442E2C9");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Records__3214EC0706AD3366");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.User).WithMany(p => p.Records)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Records__UserId__5CA1C101");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07078D14AC");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Firstname).HasMaxLength(255);
            entity.Property(e => e.Lastname).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
