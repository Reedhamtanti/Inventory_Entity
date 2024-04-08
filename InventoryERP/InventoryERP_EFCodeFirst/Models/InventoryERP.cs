using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace InventoryERP_EFCodeFirst.Models
{
    public class InventoryERP : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'InventoryERP_EFCodeFirst.Models.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public InventoryERP()
            : base("name=InventoryERP")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Inventory> Inventories { get; set; }

        public virtual DbSet<Inventory> Warehouses { get; set; }
    }

    public class Inventory
    {
        [Key]
        public Guid Inventoryld { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int Qty { get; set; }

        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }
    public class Warehouse
    {
        [Key]
        public Guid Warehouseld { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}