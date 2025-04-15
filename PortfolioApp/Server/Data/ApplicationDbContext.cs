using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioApp.Server.Models;

namespace PortfolioApp.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CodeSnippet> CodeSnippets { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CodeSnippetTag> CodeSnippetTags { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogPostTag> BlogPostTags { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationships
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Experience>()
                .HasOne(e => e.User)
                .WithMany(u => u.Experiences)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CodeSnippet>()
                .HasOne(c => c.User)
                .WithMany(u => u.CodeSnippets)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogPost>()
                .HasOne(p => p.User)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationships
            modelBuilder.Entity<ProjectTechnology>()
                .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTechnologies)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Technology)
                .WithMany(t => t.ProjectTechnologies)
                .HasForeignKey(pt => pt.TechnologyId);

            modelBuilder.Entity<CodeSnippetTag>()
                .HasKey(ct => new { ct.CodeSnippetId, ct.TagId });

            modelBuilder.Entity<CodeSnippetTag>()
                .HasOne(ct => ct.CodeSnippet)
                .WithMany(c => c.CodeSnippetTags)
                .HasForeignKey(ct => ct.CodeSnippetId);

            modelBuilder.Entity<CodeSnippetTag>()
                .HasOne(ct => ct.Tag)
                .WithMany(t => t.CodeSnippetTags)
                .HasForeignKey(ct => ct.TagId);

            modelBuilder.Entity<BlogPostTag>()
                .HasKey(bt => new { bt.BlogPostId, bt.TagId });

            modelBuilder.Entity<BlogPostTag>()
                .HasOne(bt => bt.BlogPost)
                .WithMany(b => b.BlogPostTags)
                .HasForeignKey(bt => bt.BlogPostId);

            modelBuilder.Entity<BlogPostTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BlogPostTags)
                .HasForeignKey(bt => bt.TagId);
        }
    }
}