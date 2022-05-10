using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using wrenstone.models.abstractions;
using wrenstone.models.characters;
using wrenstone.models.enums;
using wrenstone.models.users;

namespace wrenstone.contexts
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
            : base(options) { }

        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Character>? Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var keysProperties = modelBuilder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x!.Properties);

            foreach (var property in keysProperties)
                property.ValueGenerated = ValueGenerated.OnAdd;

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasDiscriminator<UserType>(nameof(UserType))
                .HasValue<DefaultUser>(UserType.Default)
                .IsComplete();

            modelBuilder.Entity<Character>()
                .ToTable("Characters")
                .HasDiscriminator<CharacterType>(nameof(CharacterType))
                .HasValue<DefaultCharacter>(CharacterType.Default)
                .IsComplete();
        }
    }
}
