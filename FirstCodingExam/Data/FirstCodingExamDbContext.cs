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

    public virtual DbSet<HistoryCalculatedRecord> HistoryCalculatedRecords { get; set; }

    public virtual DbSet<HistoryRecord> HistoryRecords { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalculatedRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calculat__3214EC07EDD31AB7");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.Record).WithMany(p => p.CalculatedRecords)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calculate__Recor__5F7E2DAC");
        });

        modelBuilder.Entity<HistoryCalculatedRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HistoryC__3214EC0763A903C3");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.HistoryRecord).WithMany(p => p.HistoryCalculatedRecords)
                .HasForeignKey(d => d.HistoryRecordId)
                .HasConstraintName("FK__HistoryCa__Histo__662B2B3B");
        });

        modelBuilder.Entity<HistoryRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HistoryR__3214EC0781ED694A");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.Record).WithMany(p => p.HistoryRecords)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HistoryRe__Recor__625A9A57");

            entity.HasOne(d => d.User).WithMany(p => p.HistoryRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HistoryRe__UserI__634EBE90");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Records__3214EC077CEE0D8D");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Records)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Records__UserId__5BAD9CC8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07823E1ACF");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Firstname).HasMaxLength(255);
            entity.Property(e => e.Lastname).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
