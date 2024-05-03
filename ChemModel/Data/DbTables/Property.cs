using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemModel.Data.DbTables
{
    public class Property
    {
        public int Id { get; set; }
        [Required, Display(Name = "Название"), ColumnName("Название")]
        public string Name { get; set; }
        [Required, Display(Name = "Обозначение"), ColumnName("Обозначение")]
        public string Chars { get; set; }
        [Required, Display(Name = "Единицы измерения"), ColumnName("Единицы измерения")]
        public Unit Units { get; set; }
    }
}
