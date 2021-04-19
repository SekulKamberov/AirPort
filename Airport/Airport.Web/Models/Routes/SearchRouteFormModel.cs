namespace Airport.Web.Models.Routes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using Common.Constants; 

    public class SearchRouteFormModel
    {
        [Display(Name = WebConstants.FieldDisplay.StartDestination)]
        public int StartTown { get; set; }

        [Display(Name = WebConstants.FieldDisplay.EndDestination)]
        public int EndTown { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = WebConstants.FieldDisplay.Company)]
        public string CompanyId { get; set; }

        public IEnumerable<SelectListItem> Towns { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }

    }
}
