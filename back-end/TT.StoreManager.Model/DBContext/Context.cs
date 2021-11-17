using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TT.StoreManager.Model
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Supplier> Supplier { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=localhost;User=root;Password=12345678@Abc;Database=tt_store_manager");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasComment("Sản phẩm");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned")
                    .HasComment("Khóa chính")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(255)
                    .HasComment("Người tạo");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasComment("Ngày tạo");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("longtext")
                    .HasComment("Hình ảnh sản phẩm");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(255)
                    .HasComment("Người sửa");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasComment("Ngày sửa");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .HasComment("Tên sản phẩm");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(18,4)")
                    .HasComment("Giá sản phẩm");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("supplier_id")
                    .HasColumnType("int unsigned")
                    .HasComment("ID nhà cung cấp");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Product)
                    .HasForeignKey<Product>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_id");

            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("stock");

                entity.HasComment("Kho hàng");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned")
                    .HasComment("Khóa chính");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(255)
                    .HasComment("Người tạo");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasComment("Ngày tạo");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(255)
                    .HasComment("Người sửa");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasComment("Ngày sửa");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int unsigned")
                    .HasComment("ID sản phẩm");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasColumnType("int unsigned")
                    .HasComment("Số lượng");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("supplier");

                entity.HasComment("Nhà cung cấp");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int unsigned")
                    .HasComment("Khóa chính");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(500)
                    .HasComment("Địa chỉ");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasMaxLength(255)
                    .HasComment("Người tạo");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasComment("Ngày tạo");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .HasComment("Email");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(50)
                    .HasComment("Số điện thoại");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(255)
                    .HasComment("Người sửa");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasComment("Ngày sửa");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .HasComment("Tên nhà cung cấp");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        /// <summary>
        /// Hàm thực hiện lấy table từ context để làm việc
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        /// created by vdthang 17.11.2021
        public object GetTable(Type modelType)
        {
            switch (modelType.Name)
            {
                case "Product":
                    return this.Product;
                case "Stock":
                    return this.Stock;
                case "Supplier":
                    return this.Supplier;
                default:
                    throw new NullReferenceException("Khởi tạo table bị null!");
            }
        }
    }
}
