using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace smart_signature_timer.Models
{
    public partial class SSPContext : DbContext
    {
        public SSPContext()
        {
        }

        public SSPContext(DbContextOptions<SSPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ariticle> Ariticle { get; set; }
        public virtual DbSet<Constants> Constants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ariticle>(entity =>
            {
                entity.ToTable("ariticle");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ArticleUrl)
                    .HasColumnName("article_url")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.EosAccount)
                    .HasColumnName("eos_account")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.FissionFactor)
                    .HasColumnName("fission_factor")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SignId)
                    .HasColumnName("sign_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Constants>(entity =>
            {
                entity.ToTable("constants");

                entity.Property(e => e.Id).HasColumnType("varchar(64)");

                entity.Property(e => e.Value).HasColumnType("varchar(64)");
            });
        }
    }
}
