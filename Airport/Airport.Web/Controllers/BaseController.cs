namespace Airport.Web.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Common.Constants;
    using Services.Contracts;

    public abstract class BaseController : Controller
    {
        protected readonly ITownService towns;

        protected BaseController() { }

        protected BaseController(ITownService towns)
            :this()
        {
            this.towns = towns;
        }


        protected List<SelectListItem> GenerateSelectListTowns()
        {
            var list = new List<SelectListItem>();
            var towns = this.towns.GetTownsListItems();

            list.Add(new SelectListItem()
            {
                Disabled = true,
                Text = WebConstants.SelectListDefaultItem.SelectTown,
                Value = WebConstants.SelectListDefaultItem.DefaultItemValue,
                Selected = true
            });

            foreach (var t in towns)
            {
                list.Add(new SelectListItem()
                {
                    Text = t.Name,
                    Value = t.id.ToString()
                });
            }

            return list;
        }
    }
}
