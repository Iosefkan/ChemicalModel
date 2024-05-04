using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Data.DbTables
{
    public class UserAddMathModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required, ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int MathModelId { get; set; }
        [Required, ForeignKey(nameof(MathModelId))]
        public MathModel MathModel { get; set; }
    }
}
