using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class Material
    {
        public int Id { get; set; }
        [Required, Display(Name = "Навзвание")]
        public string Name { get; set; }
        public List<MaterialPropertyBind> Properties { get; set; } = new();
        public List<MaterialEmpiricBind> MathModelProperties { get; set; } = new();
    }
}
