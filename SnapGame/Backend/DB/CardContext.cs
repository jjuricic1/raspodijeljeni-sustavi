using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DB
{
    public class CardContext :DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options)
        {
        }

        public DbSet<Card> Card { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Rank).IsRequired();
                entity.Property(e => e.Color).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
