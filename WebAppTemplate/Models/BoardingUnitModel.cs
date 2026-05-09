using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class BoardingUnitModel
    {

        [Key]
        public Guid BoardingUnitId { get; set; }

        public BoardingUnitModel()
        {
            BoardingUnitId = Guid.NewGuid();
        }

        [Required, MaxLength(50)]
        public string UnitName { get; set; }

        [Required, MaxLength(50)]
        public string UnitType { get; set; }

        [Required, MaxLength(20)]
        public string SpeciesAllowed { get; set; }

        [Required, MaxLength(20)]
        public string SizeCategory { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<BoardingModel> Boardings { get; set; }

    }
}