using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Models;
using System.Reflection.Emit;

namespace OnlineShoppingApp.Context
{
    public class ShoppingContext : IdentityDbContext<
            AppUser,
            AppRole,
            int,
            IdentityUserClaim<int>,
            AppUserRole,
            IdentityUserLogin<int>,
            IdentityRoleClaim<int>,
            IdentityUserToken<int>

        >
    {
        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)
        {
        }


        public DbSet<Admin> Admins { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Brand> Brands { get; set; }

        public DbSet<ProductSeller> ProductSellers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<Rate> Rates { get; set; }
		public DbSet<Comment> Comments { get; set; }

        public DbSet<Replies> Replies { get; set; } 




		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /// Changing the default name for tables
            builder.Entity<AppUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<AppRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<AppUserRole>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<int>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<int>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<int>>(entity => { entity.ToTable("UserTokens"); });
            builder.Entity<IdentityRoleClaim<int>>(entity => { entity.ToTable("RoleClaims"); });
            ///

            /// TPT approach
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<Admin>().ToTable("Admins");
            builder.Entity<Seller>().ToTable("Sellers");
            builder.Entity<Buyer>().ToTable("Buyers");
            ///


            /// [AppUser] * <have> * [AppRole]
            builder.Entity<AppUserRole>()
              .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder
                .Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder
                .Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            ///

			/// [Buyer] * <rate> * [Product]
			builder.Entity<Rate>().HasKey(p => new { p.ProductId, p.BuyerId });

			builder
				.Entity<Rate>()
				.HasOne(R => R.Product)
				.WithMany(R => R.Rates)
				.HasForeignKey(R => R.ProductId);

			builder
				.Entity<Rate>()
				.HasOne(R => R.Buyer);
            ///


            /// [Seller] * <Sell> * [Product]
            builder.Entity<ProductSeller>().HasKey(ps => new { ps.ProductId, ps.SellerId });

            builder
                .Entity<ProductSeller>()
                .HasOne(R => R.Product)
                .WithMany(R => R.ProductSellers)
                .HasForeignKey(R => R.ProductId);

            builder
                .Entity<ProductSeller>()
                .HasOne(R => R.Seller);
            ///

            builder.Entity<Order>().Property(O => O.SubTotal).HasColumnType("decimal(18,2)");
            builder.Entity<DeliveryMethod>().Property(D => D.DeliveryCost).HasColumnType("decimal(18,2)");
            builder.Entity<OrderItem>().Property(I => I.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().Property(P => P.Price).HasColumnType("decimal(18,2)");

		}
	}
}
