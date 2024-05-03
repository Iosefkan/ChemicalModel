using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class MathModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Название")]
        public string Name { get; set; }
        [Required, Display(Name = "Формула")]
        public string Formula { get; set; }
        [Required, Display(Name = "TeX формула")]
        public string TexFormula { get; set; }
        public List<MathModelEmpiricBind> EmpiricCoefficients { get; set; } = new();
        public List<VarCoefficient> VarCoefficients { get; set; } = new();
        public override string ToString()
        {
            return Name;
        }
    }
}
