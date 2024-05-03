using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class MaterialMathModelPropertyBind
    {
        public int Id { get; set; }
        [Required]
        public double Value { get; set; }
        public int MaterialId { get; set; }
        [Required, ForeignKey("MaterialId")]
        public Material Material { get; set; }
        public int PropertyId { get; set; }
        [Required, ForeignKey("PropertyId")]
        public MaterialMathModelProperty Property { get; set; }
    }
}
