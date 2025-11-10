using Microsoft.EntityFrameworkCore;
using URLShorteningApi.Models;

namespace URLShortening.Data
{
    public class URLShorteningContext : DbContext
    {
        public URLShorteningContext(DbContextOptions<URLShorteningContext> options)
            : base(options)
        {
        }

        public DbSet<URLShorteningItem> URLShorteningItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<URLShorteningItem>(entity =>
            {
                entity.HasIndex(e => e.shortCode).IsUnique();
                entity.Property(e => e.createdAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.updatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.accessCount).HasDefaultValue(0);
            });
        }
    }
}