using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterBackend.DBContext.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterBackend.DBContext
{
    public class TeleQuestContext : DbContext
    {
        public TeleQuestContext(DbContextOptions<TeleQuestContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Quest> Quests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique(true);

            modelBuilder.Entity<Quest>()
                .HasOne(q => q.User)
                .WithMany(u => u.Quests)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
