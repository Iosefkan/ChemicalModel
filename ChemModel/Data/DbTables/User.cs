using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChemModel.Data.DbTables
{
    public class User
    {
        public int Id { get; set; }
        [Required, Display(Name = "Имя"), ColumnName("Имя")]
        public string? Name { get; set; }
        [Required, Display(Name = "Пароль"), ColumnName("Пароль")]
        public string? Password { get; set; }
        [Required, Display(Name = "Роль"), ColumnName("Роль")]
        public Role? Role { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
