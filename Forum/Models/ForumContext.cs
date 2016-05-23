using Forum.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class ForumContext : IdentityDbContext
    {
        public ForumContext()
            : base("ForumConnection")
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ForumModel> Forum { get; set; }
        public DbSet<TematModel> Tematy { get; set; }
        public DbSet<PostModel> Posty { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TematModel>()
                .HasRequired(t => t.Forum).WithMany(f => f.Tematy).HasForeignKey(t => t.Forum_ID);

            modelBuilder.Entity<PostModel>()
                .HasRequired(p => p.Temat).WithMany(t => t.Posty).HasForeignKey(p => p.Temat_ID);

            modelBuilder.Entity<PostModel>()
                .HasRequired(p => p.Autor).WithMany(a => a.Posty).HasForeignKey(p => p.AutorID);
        }
    }
}