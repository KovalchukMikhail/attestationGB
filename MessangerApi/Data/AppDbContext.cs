using MessangerApi.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MessangerApi.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Guid).HasName("message_pkey");

                entity.ToTable("messages");

                entity.Property(e => e.Guid).HasColumnName("id");
                entity.Property(e => e.Text)
                    .HasColumnName("text");

                entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
                entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
                entity.Property(e => e.MessageStatus).HasConversion<int>();
            });

            modelBuilder.Entity<Status>()
                .HasKey(e => e.MessageStatus).HasName("status_pkey");
            
            modelBuilder.Entity<Status>().Property(e => e.MessageStatus)
                .HasConversion<int>();

            modelBuilder.Entity<Status>()
                .HasData(Enum.GetValues(typeof(MessageStatus))
                .Cast<MessageStatus>()
                .Select(e => new Status()
                {
                    MessageStatus = e,
                    Name = e.ToString(),
                }));
        }
    }
}
