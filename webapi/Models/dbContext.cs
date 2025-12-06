using Microsoft.EntityFrameworkCore;

namespace onlineQuranTutor.Models
{
    public partial class dbContext : DbContext
    {
        public dbContext()
        {
        }

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Juz> Juzs { get; set; }

        public virtual DbSet<Quran> Qurans { get; set; }

        public virtual DbSet<Surah> Surahs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Password=123456;Persist Security Info=True;User ID=sa;Initial Catalog=onlineQuranTutor;Data Source=QAIS;TrustServerCertificate=True;Encrypt=False;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}