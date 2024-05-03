using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class Unit
    {
        public int Id { get; set; }
        [Required, Display(Name = "Единица измерения"), ColumnName("Название")]
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }

    }
}
