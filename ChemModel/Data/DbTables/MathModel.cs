using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class MathModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Название"), Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Required, Display(Name = "Формула"), Column(TypeName = "varchar(200)")]
        public string Formula { get; set; }
        [Required, Display(Name = "TeX формула")]
        public string TexFormula { get; set; }
        public List<EmpiricCoefficientMathModel> EmpiricCoefficients { get; set; } = new();
        public List<VarCoefficientMathModel> VarCoefficients { get; set; } = new();
        public override string ToString()
        {
            return Name;
        }
    }
}
