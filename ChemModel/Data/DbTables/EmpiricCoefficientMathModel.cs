using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class EmpiricCoefficientMathModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PropertyId { get; set; }
        [Required, ForeignKey("PropertyId")]
        public EmpiricCoefficient Property { get; set; }
        [Required]
        public int MathModelId { get; set; }
        [Required, ForeignKey("MathModelId")]
        public MathModel MathModel { get; set; }
    }
}
