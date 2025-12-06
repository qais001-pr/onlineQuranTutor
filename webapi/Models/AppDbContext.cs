using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace onlineQuranTutor.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Guardian> Guardians { get; set; }

    public virtual DbSet<Juz> Juzs { get; set; }

    public virtual DbSet<Quran> Qurans { get; set; }

    public virtual DbSet<Surah> Surahs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Password=123456;Persist Security Info=True;User ID=sa;Initial Catalog=onlineQuranTutor;Data Source=QAIS;TrustServerCertificate=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Guardian>(entity =>
        {
            entity.HasKey(e => e.GuardianId).HasName("PK__Guardian__8A1718C1627F1182");

            entity.ToTable("Guardian");

            entity.Property(e => e.GuardianId).HasColumnName("guardianID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Guardians)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Guardian_User");
        });

        modelBuilder.Entity<Juz>(entity =>
        {
            entity.HasKey(e => e.JuzId).HasName("PK__Juz__6B37E3A99DC124D8");

            entity.ToTable("Juz");

            entity.Property(e => e.JuzId)
                .ValueGeneratedNever()
                .HasColumnName("Juz_ID");
            entity.Property(e => e.ArbabicStartWord).HasColumnName("Arbabic_Start_Word");
            entity.Property(e => e.StartingVerseId).HasColumnName("Starting_Verse_ID");
            entity.Property(e => e.SurahId).HasColumnName("Surah_ID");

            entity.HasOne(d => d.StartingVerse).WithMany(p => p.Juzs)
                .HasForeignKey(d => d.StartingVerseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juz_QuranVerse");

            entity.HasOne(d => d.Surah).WithMany(p => p.Juzs)
                .HasForeignKey(d => d.SurahId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Juz_Surah");
        });

        modelBuilder.Entity<Quran>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quran__3214EC27AE652C70");

            entity.ToTable("Quran");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SuraId).HasColumnName("SuraID");
            entity.Property(e => e.VerseId).HasColumnName("VerseID");

            entity.HasOne(d => d.Sura).WithMany(p => p.Qurans)
                .HasForeignKey(d => d.SuraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quran_Surah");
        });

        modelBuilder.Entity<Surah>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__surahs__3214EC0704E9877C");

            entity.ToTable("surahs");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SurahNames).HasColumnName("surah_names");
            entity.Property(e => e.SurahUrduNames).HasColumnName("surah_Urdu_Names");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CDF22371662");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164B00D1238").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PictureType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pictureType");
            entity.Property(e => e.ProfilePicture).HasColumnName("profilePicture");
            entity.Property(e => e.Timezone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("timezone");
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
