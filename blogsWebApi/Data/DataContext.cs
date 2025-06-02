using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace blogsWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Define DbSet properties for your entities here
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Blog <-> User (Author)
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Author)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Blog <-> Category
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Blogs)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: Blog <-> Likes <-> User
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.LikedByUsers)
                .WithMany(u => u.LikedBlogs);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Slug)
                .IsUnique();
        }
    }
}