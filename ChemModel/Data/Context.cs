using ChemModel.Data.DbTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data
{
    public class Context : DbContext
    {
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<MaterialMathModelProperty> MaterialMathModelProperties { get; set; } = null!;
        public DbSet<MaterialMathModelPropertyBind> MaterialMathModelPropBinds { get; set; } = null!;
        public DbSet<MaterialPropertyBind> MaterialPropertyBinds { get; set; } = null!;
        public DbSet<MathModelEmpiricBind> MathModelEmpiricBinds { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Material> Materials { get; set; } = null!;
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<MathModel> MathModels { get; set; } = null!;
        public DbSet<EmpiricCoefficient> EmpiricCoefficients { get; set; } = null!;
        public DbSet<VarCoefficient> VarCoefficients { get; set; } = null!;

        public Context() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D://games/ChemModel/ChemModel/ChemModel.db");
        }
    }
}
