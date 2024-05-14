﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping_Cart_2.Models;
using System.Reflection.Emit;

namespace Shopping_Cart_2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Item> items { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> ordersItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OrderItem>()
           .HasKey(e => new { e.OrderId,e.ItemId  });

            builder.Entity<OrderItem>()
            .HasOne<Item>(sc => sc.Item)
            .WithMany(s => s.Orders)
            .HasForeignKey(sc => sc.ItemId);

            builder.Entity<OrderItem>()
                .HasOne<Order>(sc => sc.Order)
                .WithMany(c => c.Items)
                .HasForeignKey(sc => sc.OrderId);
        }
    }
}