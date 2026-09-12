using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingSearchVM
    {
        public BoardingSearchVM() 
        {
            CustomerSelectList = new List<SelectListItem>();
            PetSelectList = new List<SelectListItem>();
            BoardingUnitSelectList = new List<SelectListItem>();
            BoardingSummaryResults = new List<BoardingSummaryVM>();
        }

        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public Guid? BoardingUnitId { get; set; }

        public BoardingStatusEnum? BoardingStatus { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public List<SelectListItem> CustomerSelectList { get; set; }

        public List<SelectListItem> PetSelectList { get; set; }

        public List<SelectListItem> BoardingUnitSelectList { get; set; }

        public List<BoardingSummaryVM> BoardingSummaryResults { get; set; }
    }
}