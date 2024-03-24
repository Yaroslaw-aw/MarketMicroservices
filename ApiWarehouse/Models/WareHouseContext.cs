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

                /*
                //entity
                //     .HasMany(product => product.Categories)
                //     .WithMany(category => category.Products);

                //entity
                //      .HasMany(product => product.Storages)
                //      .WithMany(storage => storage.Products);   
                */
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
            //base.OnModelCreating(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //modelBuilder.Entity<ProductCategory>(entity =>
        //{
        //    entity.HasKey(pc => new { pc.ProductId, pc.CategoryId });
        //    entity.ToTable("ProductCategory");

        //    entity.HasOne(pc => pc.Category)
        //          .WithMany()
        //          .HasForeignKey(pc => pc.CategoryId);
        //});

        // Определение таблицы и свойств для связи между продуктами и складами
        //modelBuilder.Entity<ProductStorage>(entity =>
        //{
        //    entity.HasKey(ps => new { ps.ProductId, ps.StorageId });
        //    entity.ToTable("ProductStorage");

        //    entity.HasOne(ps => ps.Storage)
        //          .WithMany(s => s.ProductStorages)
        //          .HasForeignKey(ps => ps.StorageId);
        //});


        //    OnModelCreatingPartial(modelBuilder);
        //}
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    

        public void ValidateDbSets()
        {
            Console.WriteLine("Validating DbSet configurations...");

            // Проверка Products
            Console.WriteLine($"Products DbSet configured: {Products != null}");
            if (Products != null)
            {
                Console.WriteLine($"Products DbSet entity type: {Products.EntityType}");
            }

            // Проверка Categories
            Console.WriteLine($"Categories DbSet configured: {Categories != null}");
            if (Categories != null)
            {
                Console.WriteLine($"Categories DbSet entity type: {Categories.EntityType}");
            }

            // Проверка Storages
            Console.WriteLine($"Storages DbSet configured: {Storages != null}");
            if (Storages != null)
            {
                Console.WriteLine($"Storages DbSet entity type: {Storages.EntityType}");
            }

            // Проверка ProductCategories
            //Console.WriteLine($"ProductCategories DbSet configured: {ProductCategories != null}");
            //if (ProductCategories != null)
            //{
            //    Console.WriteLine($"ProductCategories DbSet entity type: {ProductCategories.EntityType}");
            //}

            // Проверка ProductStorages
            //Console.WriteLine($"ProductStorages DbSet configured: {ProductStorages != null}");
            //if (ProductStorages != null)
            //{
            //    Console.WriteLine($"ProductStorages DbSet entity type: {ProductStorages.EntityType}");
            //}
        }
    }
}

//entity.HasMany(product => product.Categories)
//      .WithMany(category => category.Products)
//      .UsingEntity<ProductCategory>(
//          j =>
//          {
//              j.ToTable("ProductCategory"); // Название таблицы связей
//              j.HasOne(typeof(Category)).WithMany().HasForeignKey("CategoryId"); // Свойство Category
//              j.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId");   // Свойство Product
//              j.HasKey("ProductId", "CategoryId"); // Композитный ключ
//              j.HasIndex("ProductId", "CategoryId").IsUnique(); // Уникальный индекс
//          });

//entity.HasMany(product => product.Storages)
//      .WithMany(storage => storage.Products)
//      .UsingEntity<ProductStorage>(
//          j =>
//          {
//              j.ToTable("ProductCategory"); // Название таблицы связей
//              j.HasOne(typeof(Storage)).WithMany().HasForeignKey("StorageId"); // Свойство Storage
//              j.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId"); // Свойство Product
//              j.HasKey("ProductId", "StorageId"); // Композитный ключ
//              j.HasIndex("ProductId", "StorageId").IsUnique(); // Уникальный индекс
//          });


//public DbSet<ProductCategory> ProductCategories { get; set; }
//public DbSet<ProductStorage> ProductStorages { get; set; }






//modelBuilder.Entity<ProductCategory>(entity =>
//{
//    entity.HasNoKey();

//    entity.Property(e => e.CategoryId)
//          .HasColumnName("category_id")
//          .IsRequired();

//    entity.Property(e => e.ProductId)
//          .HasColumnName("product_id")
//          .IsRequired();


//});

//modelBuilder.Entity<ProductStorage>(entity =>
//{
//    entity.HasNoKey();

//    entity.Property(e => e.StorageId)
//          .HasColumnName("storage_id")
//          .IsRequired();

//    entity.Property(e => e.ProductId)
//          .HasColumnName("product_id")
//          .IsRequired();

//    entity.Property(e => e.Price)
//          .HasColumnName("price")
//          .IsRequired();

//    entity.Property(e => e.Count)
//          .HasColumnName("count")
//          .IsRequired();

//    entity.Property(e => e.Name)
//          .HasColumnName("name")
//          .IsRequired();

//    entity.Property(e => e.Description)
//          .HasColumnName("descriprion")
//          .IsRequired();
//});