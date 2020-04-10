using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectToDoList.Models
{
    public class ToDoTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        
        [Column(TypeName = "nvarchar(250)")]
        public string Description { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }
        public int CompletePercent { get; set; }

    }
}
