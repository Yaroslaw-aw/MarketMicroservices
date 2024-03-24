using Microsoft.EntityFrameworkCore;
using WareHouse.Models.Connections;

namespace WareHouse.Models
{
    public partial class WareHouseContext : DbContext
    {
        public string? connectionString;
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }

        public DbSet<StorageProduct> StoragesProducts { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public WareHouseContext(DbContextOptions<WareHouseContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(product => product.Id).HasName("product_pkey");

                entity.Property(product => product.Id)
                .HasColumnName("productId")
                .IsRequired();

                entity.Property(product => product.Name)
                .HasColumnName("name")
                .IsRequired();

                entity.Property(product => product.Description)
                .HasColumnName("description")
                .IsRequired();
            });

            modelBuilder.Entity<ProductDetails>(entity =>
            {
                entity.HasKey(product => product.ProductId);
                entity.HasOne(product => product.Product);

                #region Данные по умолчанию

                #endregion

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(category => category.Id).HasName("category_pkey");
                entity.ToTable("categories");

                entity.Property(category => category.Id)
                .HasColumnName("categoryId")
                .IsRequired();

                entity.Property(category => category.Name)
                .HasColumnName("name")
                .IsRequired();

                entity.Property(category => category.Description)
                .HasColumnName("description")
                .IsRequired();

                entity
                    .HasMany(category => category.Products)
                    .WithMany(category => category.Categories)
                    .UsingEntity<CategoryProduct>(
                        product => product
                                .HasOne(categoryProduct => categoryProduct.Product)
                                .WithMany(product => product.CategoryProducts)
                                .HasForeignKey(categoryProduct => categoryProduct.ProductId),
                        category => category
                                .HasOne(categoryProduct => categoryProduct.Category)
                                .WithMany(category => category.CategoryProducts)
                                .HasForeignKey(categoryProduct => categoryProduct.CategoryId));
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(storage => storage.Id).HasName("storage_pkey");
                entity.ToTable("storagies");

                entity.Property(storage => storage.Id)
                .HasColumnName("storageId")
                .IsRequired();

                entity.Property(storage => storage.Name)
                .HasColumnName("name")
                .IsRequired();

                entity.Property(storage => storage.Description)
                .HasColumnName("description")
                .IsRequired();

                entity
                    .HasMany(storage => storage.Products)
                    .WithMany(product => product.Storages)
                    .UsingEntity<StorageProduct>(
                        product => product
                                .HasOne(storageProduct => storageProduct.Product)
                                .WithMany(product => product.StorageProducts)
                                .HasForeignKey(storageProduct => storageProduct.ProductId),
                        category => category
                                .HasOne(storageProduct => storageProduct.Storage)
                                .WithMany(storage => storage.StorageProducts)
                                .HasForeignKey(storageProduct => storageProduct.StorageId));
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
