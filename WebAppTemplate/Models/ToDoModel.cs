using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class ToDoModel
    {
        [Key]
        public Guid ToDoID { get; set; }

        [Required]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsComplete { get; set; }
    }
}