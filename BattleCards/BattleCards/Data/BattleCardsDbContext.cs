using BattleCards.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleCards.Data
{
    public class BattleCardsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=BattleCards;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCard>(uc =>
            {
                uc.HasKey(x => new { x.UserId, x.CardId });

                uc.HasOne(x => x.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                uc.HasOne(x => x.Card)
                .WithMany(c => c.Users)
                .HasForeignKey(x => x.CardId)
                .OnDelete(DeleteBehavior.Restrict);
            });
                

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<UserCard> UsersCards { get; set; }
    }
}
