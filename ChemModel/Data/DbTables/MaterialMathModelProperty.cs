using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class MaterialMathModelProperty
    {
        public int Id { get; set; }
        [Required, Display(Name = "Название")]
        public string Name { get; set; }
        [Required, Display(Name = "Обозначение")]
        public string Chars { get; set; }
        [Required, Display(Name = "Единицы измерения")]
        public Unit Units { get; set; }
    }
}
